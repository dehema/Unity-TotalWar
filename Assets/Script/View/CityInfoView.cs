using DotLiquid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public partial class CityInfoView : BaseView
{
    //UI
    ObjPool buildingPool;
    ObjPool upgradeConditionPool;
    Dictionary<int, CityInfoBuildingItem> buildingItemDict = new Dictionary<int, CityInfoBuildingItem>();
    //data
    CityInfoViewParams viewParams;
    int cityID;
    CityData cityData;
    CityConfig cityConfig;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        viewParams = _params[0] as CityInfoViewParams;
        cityID = viewParams.cityID;
        cityData = CityMgr.Ins.GetCityDataByID(cityID);
        cityConfig = CityMgr.Ins.GetCityConfig(cityID);
        //pool
        buildingPool = PoolMgr.Ins.CreatePool(buildingItem);
        upgradeConditionPool = PoolMgr.Ins.CreatePool(upgradeCondition);
        //UI
        btUpgradeBuilding_Button.SetButton(OnClickUpgradeBuilding);
        debugBtUpgradeBuilding_Button.SetDebugButton(OnClickDebugUpgradeBuilding);
        debugBtFinishBuild_Button.SetDebugButton(OnClickDebugBtFinishBuild);
        btclose_Button.SetButton(Close);
        btRecruit_Button.SetButton(OnClickRecruit);
        buildingTips.SetActive(false);
        btCloseBuildingTips_Button.SetButton(() => { buildingTips.SetActive(false); });
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        txtCityName_Text.text = LangMgr.Ins.Get(cityConfig.name);
        InitOption();
        InitBuilding();
        PlayerMgr.Ins.SetPlayerScene(PlayerScene.city);
        buildingTips.SetActive(false);
    }

    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        PlayerMgr.Ins.SetPlayerScene(PlayerScene.world);
    }

    /// <summary>
    /// ��ʼ��ѡ��
    /// </summary>
    private void InitOption()
    {
        int row = Mathf.CeilToInt(optionList.transform.childCount / 5f);
        float height = optionList_GridLayoutGroup.cellSize.y * row + optionList_GridLayoutGroup.spacing.y * (row - 1);
        optionList_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void InitBuilding()
    {
        buildingPool.CollectAll();
        buildingItemDict.Clear();
        int buildingNum = cityData.buildingDict.Count;
        int row = Mathf.CeilToInt(buildingNum / 4f);
        float height = buildingList_GridLayoutGroup.cellSize.y * row + buildingList_GridLayoutGroup.spacing.y * (row - 1);
        buildingList_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        foreach (var item in cityData.buildingDict)
        {
            GameObject go = buildingPool.Get(item.Value);
            CityInfoBuildingItem cityInfoBuildingItem = go.GetComponent<CityInfoBuildingItem>();
            buildingItemDict.Add(item.Key, cityInfoBuildingItem);
        }
    }

    /// <summary>
    /// �����ļ
    /// </summary>
    public void OnClickRecruit()
    {
        RecruitViewParams viewParams = new RecruitViewParams();
        viewParams.unitNum = cityData.recruitUnit;
        viewParams.cityData = cityData;
        UIMgr.Ins.OpenView<RecruitView>(viewParams);
    }

    //��ǰѡ�еĽ���
    BuildingConfig selectBuildingConfig;
    /// <summary>
    /// ��ʾ������ʾ���
    /// </summary>
    /// <param name="_buildingConfig"></param>
    public void ShowBuildingTips(BuildingConfig _buildingConfig)
    {
        if (buildingTips.activeSelf && _buildingConfig == selectBuildingConfig)
        {
            buildingTips.SetActive(false);
            return;
        }
        //����
        buildingTips.transform.position = buildingItemDict[_buildingConfig.ID].transform.position;
        buildingTips_Rect.anchoredPosition += new Vector2(0, 100);
        //data
        selectBuildingConfig = _buildingConfig;
        buildingTips.SetActive(true);
        //
        //������ť
        btUpgradeBuilding.SetActive(false);
        //�����������
        constructNeed.SetActive(false);
        //����������ʾ
        constructTips.SetActive(false);
        //������ʾ
        buildingMaxLv.SetActive(false);
        inBuildingTips.SetActive(false);
        int newBuildingID = _buildingConfig.upgradeBuildingID;
        //����
        if (newBuildingID == 0)
        {
            buildingMaxLv.SetActive(true);
            return;
        }
        //������
        if (cityData.inBuildIngData.ContainsKey(_buildingConfig.ID))
        {
            inBuildingTips.SetActive(true);
            InBuildIngData inBuildIngData = cityData.inBuildIngData[_buildingConfig.ID];
            inBuildingTips_Text.text = LangMgr.Ins.Get("1667200206") + "\n" + string.Format(LangMgr.Ins.Get("1667200207"), inBuildIngData.endHour - DataMgr.Ins.gameData.worldTime.TotalHour);
            return;
        }
        BuildingConfig newBuildingConfig = CityMgr.Ins.GetBuildingConfig(newBuildingID);
        //����������ʾ
        bool isCondition = true;
        upgradeConditionPool.CollectAll();
        foreach (var preBuildingID in newBuildingConfig.preBuildingIDs)
        {
            if (!CityMgr.Ins.IsHasBuilding(cityID, preBuildingID))
            {
                GameObject go = upgradeConditionPool.Get();
                BuildingConfig needCityConfig = CityMgr.Ins.GetBuildingConfig(preBuildingID);
                go.GetComponent<Text>().text = LangMgr.Ins.Get("1667200205") + LangMgr.Ins.Get(needCityConfig.name);
                isCondition = false;
            }
        }
        constructTips.SetActive(!isCondition);
        //
        if (isCondition)
        {
            //�����������
            constructNeed.SetActive(true);
            int costGold = newBuildingConfig.costGold;
            constructNeedGold_Text.text = costGold.ToString();
            constructNeedGold_Text.color = DataMgr.Ins.playerData.gold.Value >= costGold ? Color.white : Color.red;
            constructNeedTime_Text.text = newBuildingConfig.costHour + "H";
            //������ť
            btUpgradeBuilding.SetActive(true);
            txtUpgradeBuilding_Text.text = LangMgr.Ins.Get("1667200203") + LangMgr.Ins.Get(newBuildingConfig.name);
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    void OnClickUpgradeBuilding()
    {
        BuildingConfig newBuildingConfig = CityMgr.Ins.GetBuildingConfig(selectBuildingConfig.upgradeBuildingID);
        if (DataMgr.Ins.playerData.gold.Value < newBuildingConfig.costGold)
        {
            Utility.PopTips(LangMgr.Ins.Get("1672500003"));
            return;
        }
        InBuildIngData inBuildIngData = new InBuildIngData();
        inBuildIngData.startHour = DataMgr.Ins.gameData.worldTime.TotalHour;
        inBuildIngData.endHour = DataMgr.Ins.gameData.worldTime.TotalHour + newBuildingConfig.costHour;
        inBuildIngData.originBuildingID = selectBuildingConfig.ID;
        inBuildIngData.targetBuildingID = selectBuildingConfig.upgradeBuildingID;
        cityData.inBuildIngData.Add(selectBuildingConfig.ID, inBuildIngData);
        DataMgr.Ins.playerData.gold.Value -= newBuildingConfig.costGold;
        DataMgr.Ins.SaveAllData();
        buildingTips.SetActive(false);
        Debug.Log(newBuildingConfig.costHour + "Сʱ������Ϊ" + LangMgr.Ins.Get(newBuildingConfig.name));
    }

    /// <summary>
    /// ��������(����)
    /// </summary>
    void OnClickDebugUpgradeBuilding()
    {
        CityMgr.Ins.UpgradeBuilding(cityID, selectBuildingConfig.ID, selectBuildingConfig.upgradeBuildingID);
        buildingTips.SetActive(false);
        InitBuilding();
    }

    /// <summary>
    /// ˲����ɽ���(����)
    /// </summary>
    public void OnClickDebugBtFinishBuild()
    {
        OnClickDebugUpgradeBuilding();
    }
}

public class CityInfoViewParams
{
    //����ID
    public int cityID;
    public CityInfoViewParams(int _cityID)
    {
        cityID = _cityID;
    }
}

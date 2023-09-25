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
        //data
        viewParams = _params[0] as CityInfoViewParams;
        cityID = viewParams.cityID;
        cityData = CityMgr.Ins.GetCityDataByID(cityID);
        cityConfig = CityMgr.Ins.GetCityConfig(cityID);
        //UI
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
    /// 初始化选项
    /// </summary>
    private void InitOption()
    {
        int row = Mathf.CeilToInt(optionList.transform.childCount / 5f);
        float height = optionList_GridLayoutGroup.cellSize.y * row + optionList_GridLayoutGroup.spacing.y * (row - 1);
        optionList_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    /// <summary>
    /// 初始化建筑
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
    /// 点击招募
    /// </summary>
    public void OnClickRecruit()
    {
        RecruitViewParams viewParams = new RecruitViewParams();
        viewParams.unitNum = cityData.recruitUnit;
        viewParams.cityData = cityData;
        UIMgr.Ins.OpenView<RecruitView>(viewParams);
    }

    //当前选中的建筑
    BuildingConfig selectBuildingConfig;
    BuildingData selectBuildingData;
    /// <summary>
    /// 显示建筑提示面板
    /// </summary>
    /// <param name="_buildingConfig"></param>
    public void ShowBuildingTips(BuildingConfig _buildingConfig)
    {
        if (buildingTips.activeSelf && _buildingConfig == selectBuildingConfig)
        {
            buildingTips.SetActive(false);
            return;
        }
        //坐标
        buildingTips.transform.position = buildingItemDict[_buildingConfig.ID].transform.position;
        buildingTips_Rect.anchoredPosition += new Vector2(0, 100);
        //data
        selectBuildingConfig = _buildingConfig;
        selectBuildingData = cityData.buildingDict[selectBuildingConfig.ID];
        buildingTips.SetActive(true);
        //
        //升级按钮
        btUpgradeBuilding.SetActive(false);
        //升级所需材料
        constructNeed.SetActive(false);
        //升级条件提示
        constructTips.SetActive(false);
        //满级提示
        buildingMaxLv.SetActive(false);
        inBuildingTips.SetActive(false);
        int newBuildingID = _buildingConfig.upgradeBuildingID;
        //满级
        if (newBuildingID == 0)
        {
            buildingMaxLv.SetActive(true);
            return;
        }
        //建造中
        if (cityData.inBuildIngData.ContainsKey(_buildingConfig.ID))
        {
            inBuildingTips.SetActive(true);
            InBuildIngData inBuildIngData = cityData.inBuildIngData[_buildingConfig.ID];
            inBuildingTips_Text.text = LangMgr.Ins.Get("1667200206") + "\n" + string.Format(LangMgr.Ins.Get("1667200207"), inBuildIngData.endHour - DataMgr.Ins.gameData.worldTime.TotalHour);
            return;
        }
        BuildingConfig newBuildingConfig = CityMgr.Ins.GetBuildingConfig(newBuildingID);
        //升级条件提示
        bool isCondition = true;
        if (!selectBuildingData.isInitialBuilded)
        {
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
        }
        //可建造的
        if (isCondition)
        {
            //升级所需材料
            constructNeed.SetActive(true);
            int costGold = selectBuildingData.isInitialBuilded ? selectBuildingConfig.costGold : newBuildingConfig.costGold;
            constructNeedGold_Text.text = costGold.ToString();
            constructNeedGold_Text.color = DataMgr.Ins.playerData.gold.Value >= costGold ? Color.white : Color.red;
            int costHour = selectBuildingData.isInitialBuilded ? selectBuildingConfig.costHour : newBuildingConfig.costHour;
            constructNeedTime_Text.text = costHour + "H";
            //升级按钮
            btUpgradeBuilding.SetActive(true);
            string costName = LangMgr.Ins.Get(selectBuildingData.isInitialBuilded ? selectBuildingConfig.name : newBuildingConfig.name);
            txtUpgradeBuilding_Text.text = LangMgr.Ins.Get("1667200203") + costName;
        }
    }

    /// <summary>
    /// 升级建筑
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
        int costHour = selectBuildingData.isInitialBuilded ? selectBuildingConfig.costHour : newBuildingConfig.costHour;
        inBuildIngData.endHour = DataMgr.Ins.gameData.worldTime.TotalHour + costHour;
        inBuildIngData.originBuildingID = selectBuildingConfig.ID;
        int newBuildingID = selectBuildingData.isInitialBuilded ? selectBuildingConfig.ID : selectBuildingConfig.upgradeBuildingID;
        inBuildIngData.targetBuildingID = newBuildingID;
        cityData.inBuildIngData.Add(selectBuildingConfig.ID, inBuildIngData);
        int costGold = selectBuildingData.isInitialBuilded ? selectBuildingConfig.costGold : newBuildingConfig.costGold;
        DataMgr.Ins.playerData.gold.Value -= costGold;
        DataMgr.Ins.SaveAllData();
        buildingTips.SetActive(false);
        string costName = LangMgr.Ins.Get(selectBuildingData.isInitialBuilded ? selectBuildingConfig.name : newBuildingConfig.name);
        Debug.Log(costHour + "小时后升级为" + costName);
    }

    /// <summary>
    /// 升级建筑(测试)
    /// </summary>
    void OnClickDebugUpgradeBuilding()
    {
        int newBuildingID = selectBuildingData.isInitialBuilded ? selectBuildingConfig.ID : selectBuildingConfig.upgradeBuildingID;
        CityMgr.Ins.UpgradeBuilding(cityID, selectBuildingConfig.ID, newBuildingID);
        buildingTips.SetActive(false);
        InitBuilding();
    }

    /// <summary>
    /// 瞬间完成建造(测试)
    /// </summary>
    public void OnClickDebugBtFinishBuild()
    {
        OnClickDebugUpgradeBuilding();
    }
}

public class CityInfoViewParams
{
    //城镇ID
    public int cityID;
    public CityInfoViewParams(int _cityID)
    {
        cityID = _cityID;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CityInfoView : BaseView
{
    //UI
    ObjPool buildingPool;
    ObjPool constructPool;
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
        constructPool = PoolMgr.Ins.CreatePool(btConstruct);
        //UI
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
        int buildingNum = cityData.buildingDict.Count;
        int row = Mathf.CeilToInt(buildingNum / 4f);
        float height = buildingList_GridLayoutGroup.cellSize.y * row + buildingList_GridLayoutGroup.spacing.y * (row - 1);
        buildingList_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        foreach (var item in cityData.buildingDict)
        {
            buildingPool.Get(item.Value);
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
        selectBuildingConfig = _buildingConfig;
        buildingTips.SetActive(true);
        //����
        constructPool.CollectAll();
        foreach (var item in _buildingConfig.upgradeBuildingIDs)
        {
            GameObject go = constructPool.Get();
            go.GetComponent<Button>().onClick.RemoveAllListeners();
            int newBuildingID = item;
            BuildingConfig newBuildingConfig = CityMgr.Ins.GetBuildingConfig(newBuildingID);
            go.transform.Find("Text").GetComponent<Text>().text = LangMgr.Ins.Get("1667200203") + LangMgr.Ins.Get(newBuildingConfig.name);
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("����Ϊ" + LangMgr.Ins.Get(newBuildingConfig.name));
                CityMgr.Ins.UpgradeBuilding(cityID, _buildingConfig.ID);
            });
        }
        //�����ɱ�
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

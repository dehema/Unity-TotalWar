using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CityInfoView : BaseView
{
    //UI
    ObjPool buildingPool;
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
        //UI
        buildingPool = PoolMgr.Ins.CreatePool(buildingItem);
        btclose_Button.SetButton(Close);
        btRecruit_Button.SetButton(OnClickRecruit);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        txtCityName_Text.text = LangMgr.Ins.Get(cityConfig.name);
        InitOption();
        InitBuilding();
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
        int buildingNum = cityData.buildingDict.Count;
        int row = Mathf.CeilToInt(buildingNum / 4f);
        float height = buildingList_GridLayoutGroup.cellSize.y * row + buildingList_GridLayoutGroup.spacing.y * (row - 1);
        buildingList_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        foreach (var item in cityData.buildingDict)
        {
            buildingPool.Get(item.Value);
        }
    }

    public void OnClickRecruit()
    {
        RecruitViewParams viewParams = new RecruitViewParams();
        viewParams.unitNum = cityData.recruitUnit;
        viewParams.cityData = cityData;
        UIMgr.Ins.OpenView<RecruitView>(viewParams);
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

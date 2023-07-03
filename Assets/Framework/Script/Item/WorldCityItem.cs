using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCityItem : WorldUnitBase
{
    public CityConfig cityConfig;
    public CityData cityData { get { return CityMgr.Ins.GetCityDataByID(cityConfig.ID); } }

    public override void Init(params object[] _params)
    {
        cityConfig = _params[1] as CityConfig;
        base.Init(_params);
        //��������һ��ƫ��ֵ
        transform.position = new Vector3(cityConfig.posX, 0, cityConfig.posY);
    }


    public void OnClick()
    {
        Debug.Log("���������" + cityConfig.name);
        WorldMgr.Ins.worldPlayer.MoveToWorldUnit(this, NavPurpose.city);
    }
}

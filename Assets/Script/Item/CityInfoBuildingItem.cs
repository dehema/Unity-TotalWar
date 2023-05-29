using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CityInfoBuildingItem : PoolItemBase
{
    int buildID;
    BuildingConfig buildingConfig;
    BuildingData buildingData;
    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        buildingData = _params[0] as BuildingData;
        buildID = buildingData.buildingID;
        buildingConfig = CityMgr.Ins.GetBuildingConfig(buildID);
        buildingName_Text.text = LangMgr.Ins.Get(buildingConfig.name[buildingData.lv]);
    }
}

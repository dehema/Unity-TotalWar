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
        buildID = buildingData.id;
        buildingConfig = buildingData.buildingConfig;
        RefreshUI();
    }

    void RefreshUI()
    {
        buildingIcon_Image.sprite = Resources.Load<Sprite>("UI/Building/" + buildingConfig.icon);
        buildingName_Text.text = LangMgr.Ins.Get(buildingConfig.name);
        switch (buildingConfig.buildingType)
        {
            case BuildingType.MainBase:
                buildingFrame_Image.sprite = Resources.Load<Sprite>("UI/common/equipment_frame_gold");
                break;
            case BuildingType.Military:
                buildingFrame_Image.sprite = Resources.Load<Sprite>("UI/common/equipment_frame_red");
                break;
            case BuildingType.Economy:
                buildingFrame_Image.sprite = Resources.Load<Sprite>("UI/common/equipment_frame_yellow");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CityInfoBuildingItem : PoolItemBase
{
    [HideInInspector]
    public GameObject buildingIcon;
    [HideInInspector]
    public Image buildingIcon_Image;
    [HideInInspector]
    public GameObject buildingName;
    [HideInInspector]
    public Text buildingName_Text;

    override internal void _LoadUI()    
    {
        buildingIcon = transform.Find("$buildingIcon#Image").gameObject;
        buildingIcon_Image = buildingIcon.GetComponent<Image>();
        buildingName = transform.Find("$buildingName#Text").gameObject;
        buildingName_Text = buildingName.GetComponent<Text>();
    }
}
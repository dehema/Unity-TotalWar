using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CityInfoBuildingItem : PoolItemBase
{
    [HideInInspector]
    public GameObject buildingFrame;
    [HideInInspector]
    public Image buildingFrame_Image;
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
        buildingFrame = transform.Find("bg/$buildingFrame#Image").gameObject;
        buildingFrame_Image = buildingFrame.GetComponent<Image>();
        buildingIcon = transform.Find("bg/$buildingFrame#Image/$buildingIcon#Image").gameObject;
        buildingIcon_Image = buildingIcon.GetComponent<Image>();
        buildingName = transform.Find("bg/$buildingName#Text").gameObject;
        buildingName_Text = buildingName.GetComponent<Text>();
    }
}
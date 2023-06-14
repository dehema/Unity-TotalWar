using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CityInfoView : BaseView
{
    [HideInInspector]
    public GameObject bg;
    [HideInInspector]
    public Image bg_Image;
    [HideInInspector]
    public Button bg_Button;
    [HideInInspector]
    public GameObject content;
    [HideInInspector]
    public RectTransform content_Rect;
    [HideInInspector]
    public GameObject btclose;
    [HideInInspector]
    public Button btclose_Button;
    [HideInInspector]
    public GameObject txtCityName;
    [HideInInspector]
    public Text txtCityName_Text;
    [HideInInspector]
    public GameObject developProress;
    [HideInInspector]
    public Slider developProress_Slider;
    [HideInInspector]
    public GameObject txtDevelopProress;
    [HideInInspector]
    public Text txtDevelopProress_Text;
    [HideInInspector]
    public GameObject txtDevelop;
    [HideInInspector]
    public Text txtDevelop_Text;
    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public Text owner_Text;
    [HideInInspector]
    public GameObject optionList;
    [HideInInspector]
    public RectTransform optionList_Rect;
    [HideInInspector]
    public GridLayoutGroup optionList_GridLayoutGroup;
    [HideInInspector]
    public GameObject btRecruit;
    [HideInInspector]
    public Button btRecruit_Button;
    [HideInInspector]
    public RectTransform btRecruit_Rect;
    [HideInInspector]
    public GameObject buildingList;
    [HideInInspector]
    public RectTransform buildingList_Rect;
    [HideInInspector]
    public GridLayoutGroup buildingList_GridLayoutGroup;
    [HideInInspector]
    public GameObject buildingItem;
    [HideInInspector]
    public RectTransform buildingItem_Rect;
    [HideInInspector]
    public GameObject buildingTips;
    [HideInInspector]
    public RectTransform buildingTips_Rect;
    [HideInInspector]
    public GameObject btCloseBuildingTips;
    [HideInInspector]
    public Button btCloseBuildingTips_Button;
    [HideInInspector]
    public GameObject constructList;
    [HideInInspector]
    public GameObject btUpgradeBuilding;
    [HideInInspector]
    public Button btUpgradeBuilding_Button;
    [HideInInspector]
    public GameObject txtUpgradeBuilding;
    [HideInInspector]
    public Text txtUpgradeBuilding_Text;
    [HideInInspector]
    public GameObject constructNeed;
    [HideInInspector]
    public GameObject constructNeedGold;
    [HideInInspector]
    public Text constructNeedGold_Text;
    [HideInInspector]
    public GameObject constructNeedTime;
    [HideInInspector]
    public Text constructNeedTime_Text;
    [HideInInspector]
    public GameObject constructTips;
    [HideInInspector]
    public GameObject upgradeCondition;
    [HideInInspector]
    public GameObject buildingMaxLv;
    [HideInInspector]
    public Text buildingMaxLv_Text;
    [HideInInspector]
    public GameObject inBuildingTips;
    [HideInInspector]
    public Text inBuildingTips_Text;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        btclose = transform.Find("$content#Rect/pop/$btclose#Button").gameObject;
        btclose_Button = btclose.GetComponent<Button>();
        txtCityName = transform.Find("$content#Rect/pop/top/$txtCityName#Text").gameObject;
        txtCityName_Text = txtCityName.GetComponent<Text>();
        developProress = transform.Find("$content#Rect/pop/top/$developProress#Slider").gameObject;
        developProress_Slider = developProress.GetComponent<Slider>();
        txtDevelopProress = transform.Find("$content#Rect/pop/top/$developProress#Slider/$txtDevelopProress#Text").gameObject;
        txtDevelopProress_Text = txtDevelopProress.GetComponent<Text>();
        txtDevelop = transform.Find("$content#Rect/pop/top/$developProress#Slider/Level/$txtDevelop#Text").gameObject;
        txtDevelop_Text = txtDevelop.GetComponent<Text>();
        owner = transform.Find("$content#Rect/pop/top/ClaSymbol/$owner#Text").gameObject;
        owner_Text = owner.GetComponent<Text>();
        optionList = transform.Find("$content#Rect/pop/operation/Viewport/Content/$optionList#Rect,GridLayoutGroup").gameObject;
        optionList_Rect = optionList.GetComponent<RectTransform>();
        optionList_GridLayoutGroup = optionList.GetComponent<GridLayoutGroup>();
        btRecruit = transform.Find("$content#Rect/pop/operation/Viewport/Content/$optionList#Rect,GridLayoutGroup/$btRecruit#Button,Rect").gameObject;
        btRecruit_Button = btRecruit.GetComponent<Button>();
        btRecruit_Rect = btRecruit.GetComponent<RectTransform>();
        buildingList = transform.Find("$content#Rect/pop/building/Viewport/Content/$buildingList#Rect,GridLayoutGroup").gameObject;
        buildingList_Rect = buildingList.GetComponent<RectTransform>();
        buildingList_GridLayoutGroup = buildingList.GetComponent<GridLayoutGroup>();
        buildingItem = transform.Find("$content#Rect/pop/building/Viewport/Content/$buildingList#Rect,GridLayoutGroup/$buildingItem#Rect").gameObject;
        buildingItem_Rect = buildingItem.GetComponent<RectTransform>();
        buildingTips = transform.Find("$content#Rect/pop/$buildingTips#Rect").gameObject;
        buildingTips_Rect = buildingTips.GetComponent<RectTransform>();
        btCloseBuildingTips = transform.Find("$content#Rect/pop/$buildingTips#Rect/bg/$btCloseBuildingTips#Button").gameObject;
        btCloseBuildingTips_Button = btCloseBuildingTips.GetComponent<Button>();
        constructList = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructList").gameObject;
        btUpgradeBuilding = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructList/$btUpgradeBuilding#Button").gameObject;
        btUpgradeBuilding_Button = btUpgradeBuilding.GetComponent<Button>();
        txtUpgradeBuilding = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructList/$btUpgradeBuilding#Button/$txtUpgradeBuilding#Text").gameObject;
        txtUpgradeBuilding_Text = txtUpgradeBuilding.GetComponent<Text>();
        constructNeed = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructNeed").gameObject;
        constructNeedGold = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructNeed/$constructNeedGold#Text").gameObject;
        constructNeedGold_Text = constructNeedGold.GetComponent<Text>();
        constructNeedTime = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructNeed/$constructNeedTime#Text").gameObject;
        constructNeedTime_Text = constructNeedTime.GetComponent<Text>();
        constructTips = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructTips").gameObject;
        upgradeCondition = transform.Find("$content#Rect/pop/$buildingTips#Rect/$constructTips/$upgradeCondition").gameObject;
        buildingMaxLv = transform.Find("$content#Rect/pop/$buildingTips#Rect/$buildingMaxLv#Text").gameObject;
        buildingMaxLv_Text = buildingMaxLv.GetComponent<Text>();
        inBuildingTips = transform.Find("$content#Rect/pop/$buildingTips#Rect/$inBuildingTips#Text").gameObject;
        inBuildingTips_Text = inBuildingTips.GetComponent<Text>();
    }
}
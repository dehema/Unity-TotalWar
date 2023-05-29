using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class RecruitView : BaseView
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
    public GameObject unitItem;
    [HideInInspector]
    public GameObject btClose;
    [HideInInspector]
    public Button btClose_Button;
    [HideInInspector]
    public GameObject townName;
    [HideInInspector]
    public Text townName_Text;
    [HideInInspector]
    public GameObject btHome;
    [HideInInspector]
    public Button btHome_Button;
    [HideInInspector]
    public GameObject sortType;
    [HideInInspector]
    public Text sortType_Text;
    [HideInInspector]
    public GameObject sortByType;
    [HideInInspector]
    public Button sortByType_Button;
    [HideInInspector]
    public GameObject sortByPower;
    [HideInInspector]
    public Button sortByPower_Button;
    [HideInInspector]
    public GameObject sortByLevel;
    [HideInInspector]
    public Button sortByLevel_Button;
    [HideInInspector]
    public GameObject sureRecruit;
    [HideInInspector]
    public Button sureRecruit_Button;
    [HideInInspector]
    public GameObject debugRefreshRecruitUnit;
    [HideInInspector]
    public Button debugRefreshRecruitUnit_Button;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        unitItem = transform.Find("$content#Rect/Lobby_Panel_Heroes/ScrollRect/Content/$unitItem").gameObject;
        btClose = transform.Find("$content#Rect/Lobby_Panel_Heroes/Top/$btClose#Button").gameObject;
        btClose_Button = btClose.GetComponent<Button>();
        townName = transform.Find("$content#Rect/Lobby_Panel_Heroes/Top/$townName#Text").gameObject;
        townName_Text = townName.GetComponent<Text>();
        btHome = transform.Find("$content#Rect/Lobby_Panel_Heroes/Top/$btHome#Button").gameObject;
        btHome_Button = btHome.GetComponent<Button>();
        sortType = transform.Find("$content#Rect/Lobby_Panel_Heroes/TapMenu/$sortType#Text").gameObject;
        sortType_Text = sortType.GetComponent<Text>();
        sortByType = transform.Find("$content#Rect/Lobby_Panel_Heroes/TapMenu/sort/$sortByType#Button").gameObject;
        sortByType_Button = sortByType.GetComponent<Button>();
        sortByPower = transform.Find("$content#Rect/Lobby_Panel_Heroes/TapMenu/sort/$sortByPower#Button").gameObject;
        sortByPower_Button = sortByPower.GetComponent<Button>();
        sortByLevel = transform.Find("$content#Rect/Lobby_Panel_Heroes/TapMenu/sort/$sortByLevel#Button").gameObject;
        sortByLevel_Button = sortByLevel.GetComponent<Button>();
        sureRecruit = transform.Find("$content#Rect/Lobby_Panel_Heroes/TapMenu/$sureRecruit#Button").gameObject;
        sureRecruit_Button = sureRecruit.GetComponent<Button>();
        debugRefreshRecruitUnit = transform.Find("$content#Rect/Lobby_Panel_Heroes/TapMenu/$debugRefreshRecruitUnit#Button").gameObject;
        debugRefreshRecruitUnit_Button = debugRefreshRecruitUnit.GetComponent<Button>();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class RecruitUnitItem : PoolItemBase
{
    [HideInInspector]
    public GameObject unitIcon;
    [HideInInspector]
    public RawImage unitIcon_RawImage;
    [HideInInspector]
    public GameObject camera;
    [HideInInspector]
    public Camera camera_Camera;
    [HideInInspector]
    public GameObject unitNum;
    [HideInInspector]
    public Text unitNum_Text;
    [HideInInspector]
    public GameObject star_1;
    [HideInInspector]
    public GameObject star_2;
    [HideInInspector]
    public GameObject star_3;
    [HideInInspector]
    public GameObject star_4;
    [HideInInspector]
    public GameObject star_5;
    [HideInInspector]
    public GameObject name;
    [HideInInspector]
    public Text name_Text;
    [HideInInspector]
    public GameObject txtSelNum;
    [HideInInspector]
    public Text txtSelNum_Text;
    [HideInInspector]
    public GameObject iconSel;
    [HideInInspector]
    public GameObject btSel;
    [HideInInspector]
    public Button btSel_Button;
    [HideInInspector]
    public ShowCommonFloatTips btSel_ShowCommonFloatTips;

    override internal void _LoadUI()    
    {
        unitIcon = transform.Find("Mask/$unitIcon#RawImage").gameObject;
        unitIcon_RawImage = unitIcon.GetComponent<RawImage>();
        camera = transform.Find("Mask/$unitIcon#RawImage/$camera#Camera").gameObject;
        camera_Camera = camera.GetComponent<Camera>();
        unitNum = transform.Find("$unitNum#Text").gameObject;
        unitNum_Text = unitNum.GetComponent<Text>();
        star_1 = transform.Find("Stars/$star_1").gameObject;
        star_2 = transform.Find("Stars/$star_2").gameObject;
        star_3 = transform.Find("Stars/$star_3").gameObject;
        star_4 = transform.Find("Stars/$star_4").gameObject;
        star_5 = transform.Find("Stars/$star_5").gameObject;
        name = transform.Find("$name#Text").gameObject;
        name_Text = name.GetComponent<Text>();
        txtSelNum = transform.Find("$txtSelNum#Text").gameObject;
        txtSelNum_Text = txtSelNum.GetComponent<Text>();
        iconSel = transform.Find("$txtSelNum#Text/$iconSel").gameObject;
        btSel = transform.Find("$btSel#Button,ShowCommonFloatTips").gameObject;
        btSel_Button = btSel.GetComponent<Button>();
        btSel_ShowCommonFloatTips = btSel.GetComponent<ShowCommonFloatTips>();
    }
}
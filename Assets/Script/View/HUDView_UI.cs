using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class HUDView : BaseView
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
    public GameObject hudUnitStateBar;
    [HideInInspector]
    public GameObject hpSliderBg;
    [HideInInspector]
    public RectTransform hpSliderBg_Rect;
    [HideInInspector]
    public GameObject hpSlider;
    [HideInInspector]
    public RectTransform hpSlider_Rect;
    [HideInInspector]
    public GameObject txtHp;
    [HideInInspector]
    public Text txtHp_Text;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        hudUnitStateBar = transform.Find("$content#Rect/hudUnitStateBarList/$hudUnitStateBar").gameObject;
        hpSliderBg = transform.Find("$content#Rect/playerState/$hpSliderBg#Rect").gameObject;
        hpSliderBg_Rect = hpSliderBg.GetComponent<RectTransform>();
        hpSlider = transform.Find("$content#Rect/playerState/$hpSliderBg#Rect/$hpSlider#Rect").gameObject;
        hpSlider_Rect = hpSlider.GetComponent<RectTransform>();
        txtHp = transform.Find("$content#Rect/playerState/$txtHp#Text").gameObject;
        txtHp_Text = txtHp.GetComponent<Text>();
    }
}
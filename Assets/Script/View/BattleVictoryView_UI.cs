using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class BattleVictoryView : BaseView
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
    public GameObject btClose;
    [HideInInspector]
    public Button btClose_Button;
    [HideInInspector]
    public GameObject wing_l;
    [HideInInspector]
    public Image wing_l_Image;
    [HideInInspector]
    public RectTransform wing_l_Rect;
    [HideInInspector]
    public GameObject wing_r;
    [HideInInspector]
    public Image wing_r_Image;
    [HideInInspector]
    public RectTransform wing_r_Rect;
    [HideInInspector]
    public GameObject anyKeyClose;
    [HideInInspector]
    public Text anyKeyClose_Text;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        btClose = transform.Find("$content#Rect/$btClose#Button").gameObject;
        btClose_Button = btClose.GetComponent<Button>();
        wing_l = transform.Find("$content#Rect/icon/$wing_l#Image,Rect").gameObject;
        wing_l_Image = wing_l.GetComponent<Image>();
        wing_l_Rect = wing_l.GetComponent<RectTransform>();
        wing_r = transform.Find("$content#Rect/icon/$wing_r#Image,Rect").gameObject;
        wing_r_Image = wing_r.GetComponent<Image>();
        wing_r_Rect = wing_r.GetComponent<RectTransform>();
        anyKeyClose = transform.Find("$content#Rect/icon/$anyKeyClose#Text").gameObject;
        anyKeyClose_Text = anyKeyClose.GetComponent<Text>();
    }
}
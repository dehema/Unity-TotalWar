using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class TechInfoView : BaseView
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
    public GameObject techName;
    [HideInInspector]
    public Text techName_Text;
    [HideInInspector]
    public GameObject techDesc;
    [HideInInspector]
    public Text techDesc_Text;
    [HideInInspector]
    public GameObject cost;
    [HideInInspector]
    public Text cost_Text;
    [HideInInspector]
    public GameObject btLear;
    [HideInInspector]
    public Button btLear_Button;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        techName = transform.Find("$content#Rect/info/$techName#Text").gameObject;
        techName_Text = techName.GetComponent<Text>();
        techDesc = transform.Find("$content#Rect/info/$techDesc#Text").gameObject;
        techDesc_Text = techDesc.GetComponent<Text>();
        cost = transform.Find("$content#Rect/info/iconCost/$cost#Text").gameObject;
        cost_Text = cost.GetComponent<Text>();
        btLear = transform.Find("$content#Rect/info/$btLear#Button").gameObject;
        btLear_Button = btLear.GetComponent<Button>();
    }
}
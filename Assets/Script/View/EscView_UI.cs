using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class EscView : BaseView
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
    public GameObject title;
    [HideInInspector]
    public Text title_Text;
    [HideInInspector]
    public GameObject btClose;
    [HideInInspector]
    public Button btClose_Button;
    [HideInInspector]
    public GameObject btSave;
    [HideInInspector]
    public Button btSave_Button;
    [HideInInspector]
    public GameObject btExit;
    [HideInInspector]
    public Button btExit_Button;
    [HideInInspector]
    public GameObject btSetting;
    [HideInInspector]
    public Button btSetting_Button;
    [HideInInspector]
    public GameObject btDebugWin;
    [HideInInspector]
    public Button btDebugWin_Button;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        title = transform.Find("$content#Rect/Popup/$title#Text").gameObject;
        title_Text = title.GetComponent<Text>();
        btClose = transform.Find("$content#Rect/Popup/$btClose#Button").gameObject;
        btClose_Button = btClose.GetComponent<Button>();
        btSave = transform.Find("$content#Rect/Popup/ver/$btSave#Button").gameObject;
        btSave_Button = btSave.GetComponent<Button>();
        btExit = transform.Find("$content#Rect/Popup/ver/$btExit#Button").gameObject;
        btExit_Button = btExit.GetComponent<Button>();
        btSetting = transform.Find("$content#Rect/Popup/ver/$btSetting#Button").gameObject;
        btSetting_Button = btSetting.GetComponent<Button>();
        btDebugWin = transform.Find("debugPanel/$btDebugWin#Button").gameObject;
        btDebugWin_Button = btDebugWin.GetComponent<Button>();
    }
}
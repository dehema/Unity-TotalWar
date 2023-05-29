using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SettingView : BaseView
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
    public GameObject setting_icon_sound_on;
    [HideInInspector]
    public GameObject setting_icon_sound_off;
    [HideInInspector]
    public GameObject sound;
    [HideInInspector]
    public Slider sound_Slider;
    [HideInInspector]
    public GameObject setting_icon_music_on;
    [HideInInspector]
    public GameObject setting_icon_music_off;
    [HideInInspector]
    public GameObject music;
    [HideInInspector]
    public Slider music_Slider;
    [HideInInspector]
    public GameObject btLang;
    [HideInInspector]
    public Button btLang_Button;
    [HideInInspector]
    public GameObject txtLang;
    [HideInInspector]
    public Text txtLang_Text;
    [HideInInspector]
    public GameObject imgLangFlag;
    [HideInInspector]
    public Image imgLangFlag_Image;
    [HideInInspector]
    public GameObject langContent;
    [HideInInspector]
    public RectTransform langContent_Rect;
    [HideInInspector]
    public GameObject langTemp;
    [HideInInspector]
    public RectTransform langTemp_Rect;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        btClose = transform.Find("$content#Rect/Popup/$btClose#Button").gameObject;
        btClose_Button = btClose.GetComponent<Button>();
        setting_icon_sound_on = transform.Find("$content#Rect/Popup/Group_Left/sound/$setting_icon_sound_on").gameObject;
        setting_icon_sound_off = transform.Find("$content#Rect/Popup/Group_Left/sound/$setting_icon_sound_off").gameObject;
        sound = transform.Find("$content#Rect/Popup/Group_Left/sound/$sound#Slider").gameObject;
        sound_Slider = sound.GetComponent<Slider>();
        setting_icon_music_on = transform.Find("$content#Rect/Popup/Group_Left/music/$setting_icon_music_on").gameObject;
        setting_icon_music_off = transform.Find("$content#Rect/Popup/Group_Left/music/$setting_icon_music_off").gameObject;
        music = transform.Find("$content#Rect/Popup/Group_Left/music/$music#Slider").gameObject;
        music_Slider = music.GetComponent<Slider>();
        btLang = transform.Find("$content#Rect/Popup/Group_Right/$btLang#Button").gameObject;
        btLang_Button = btLang.GetComponent<Button>();
        txtLang = transform.Find("$content#Rect/Popup/Group_Right/$btLang#Button/$txtLang#Text").gameObject;
        txtLang_Text = txtLang.GetComponent<Text>();
        imgLangFlag = transform.Find("$content#Rect/Popup/Group_Right/$btLang#Button/$imgLangFlag#Image").gameObject;
        imgLangFlag_Image = imgLangFlag.GetComponent<Image>();
        langContent = transform.Find("$content#Rect/Popup/Group_Right/$btLang#Button/$langContent#Rect").gameObject;
        langContent_Rect = langContent.GetComponent<RectTransform>();
        langTemp = transform.Find("$content#Rect/Popup/Group_Right/$btLang#Button/$langContent#Rect/$langTemp#Rect").gameObject;
        langTemp_Rect = langTemp.GetComponent<RectTransform>();
    }
}
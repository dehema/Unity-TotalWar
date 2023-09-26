using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public partial class SettingView : BaseView
{
    ObjPool langPool;
    bool isLangContentShow = true;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        langPool = PoolMgr.Ins.CreatePool(langTemp);
        sound_Slider.onValueChanged.AddListener(val =>
        {
            DataMgr.Ins.settingData.soundVolume = val;
            AudioMgr.Ins.soundVolume = val;
            DataMgr.Ins.SaveSettingData();
            RefreshAudioUI();
        });
        music_Slider.onValueChanged.AddListener(val =>
        {
            DataMgr.Ins.settingData.musicVolume = val;
            AudioMgr.Ins.musicVolume = val;
            DataMgr.Ins.SaveSettingData();
            RefreshMusicUI();
        });
        btLang_Button.SetButton(() =>
        {
            ShowLangContent(!isLangContentShow);
        });
        btClose_Button.SetButton(Close);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        isLangContentShow = true;
        sound_Slider.value = DataMgr.Ins.settingData.soundVolume;
        music_Slider.value = DataMgr.Ins.settingData.musicVolume;
        RefreshAudioUI();
        RefreshMusicUI();
        RefreshLangUI();
        ShowLangContent(false);
    }

    private void RefreshAudioUI()
    {
        setting_icon_sound_on.SetActive(DataMgr.Ins.settingData.soundVolume > 0);
        setting_icon_sound_off.SetActive(DataMgr.Ins.settingData.soundVolume == 0);
    }

    private void RefreshMusicUI()
    {
        setting_icon_music_on.SetActive(DataMgr.Ins.settingData.musicVolume > 0);
        setting_icon_music_off.SetActive(DataMgr.Ins.settingData.musicVolume == 0);
    }

    public void RefreshLangUI()
    {
        imgLangFlag_Image.sprite = Resources.Load<Sprite>("UI/setting/language_small_" + DataMgr.Ins.settingData.language);
        txtLang_Text.text = LangMgr.Ins.GetLanguageName(DataMgr.Ins.settingData.language);
    }

    /// <summary>
    /// œ‘ æ”Ô—‘ƒ⁄»›
    /// </summary>
    private void ShowLangContent(bool _isShow)
    {
        isLangContentShow = _isShow;
        if (_isShow)
        {
            langPool.CollectAll();
            int langNum = 0;
            foreach (var _lang in LangMgr.Ins.supportLanguage)
            {
                SystemLanguage lang = _lang;
                if (lang == LangMgr.Ins.currLang)
                {
                    continue;
                }
                GameObject item = langPool.Get();
                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    DataMgr.Ins.settingData.language = lang;
                    DataMgr.Ins.SaveSettingData();
                    LangMgr.Ins.LoadLangConfig();
                    RefreshLangUI();
                    ShowLangContent(false);
                });
                item.transform.Find("flag").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/setting/language_small_" + lang);
                Text text = item.transform.Find("txt").GetComponent<Text>();
                text.text = LangMgr.Ins.GetLanguageName(lang);
                langNum++;
            }
            langContent.SetActive(true);
        }
        else
        {
            langContent.SetActive(false);
        }
    }
}


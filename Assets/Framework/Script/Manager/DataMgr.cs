using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DataMgr : Singleton<DataMgr>
{
    /// <summary>
    /// ��Ҵ浵
    /// </summary>
    public PlayerData playerData;
    bool isLoaded = false;
    public GameData gameData;
    public SettingData settingData;

    public void Load()
    {
        if (isLoaded)
        {
            return;
        }
        //�������
        playerData = new PlayerData();
        string playerDataStr = PlayerPrefs.GetString(SaveField.playerData);
        if (string.IsNullOrEmpty(playerDataStr))
        {
            playerData.playerName.Value = "dehema";
            SavePlayerData();
        }
        else
        {
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(playerDataStr);
            Utility.Dump("-------------------------------�������--------------------------------");
            playerData.SetVal(dict);
        }
        //��Ϸ����
        gameData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(SaveField.gameData));
        if (gameData == null)
        {
            gameData = new GameData();
            //��������
            foreach (var item in ConfigMgr.Ins.cityConfig.city)
            {
                int cityID = item.Key;
                gameData.cityData.Add(cityID, new CityData(cityID));
            }
            SaveGameData();
        }
        //����
        settingData = JsonConvert.DeserializeObject<SettingData>(PlayerPrefs.GetString(SaveField.settingData));
        if (settingData == null)
        {
            settingData = new SettingData();
            if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                settingData.language = SystemLanguage.Chinese;
            }
            else
            {
                settingData.language = SystemLanguage.English;
            }
            SaveSettingData();
        }
        AudioMgr.Ins.soundVolume = settingData.soundVolume;
        AudioMgr.Ins.musicVolume = settingData.musicVolume;
        isLoaded = true;
        //login
        Login();
    }

    /// <summary>
    /// ������Ϸ���� �����á�ǩ���ȣ�
    /// </summary>
    public void SaveGameData()
    {
        string data = JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString(SaveField.gameData, data);
    }

    /// <summary>
    /// ����������ݣ�������ֵ��
    /// </summary>
    public void SavePlayerData()
    {
        string str = playerData.ToJson();
        PlayerPrefs.SetString(SaveField.playerData, str);
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void SaveSettingData()
    {
        settingData.musicVolume = AudioMgr.Ins.musicVolume;
        settingData.soundVolume = AudioMgr.Ins.soundVolume;
        PlayerPrefs.SetString(SaveField.settingData, JsonConvert.SerializeObject(settingData));
    }

    /// <summary>
    /// ��ҵ�½
    /// </summary>
    public void Login()
    {
        if (gameData.lastLoginDate.Date != DateTime.Now.Date)
        {
            NewDay();
        }
        gameData.lastLoginDate = DateTime.Now;
        //��չ�����
        gameData.lastADInterstitialTime = DateTime.Now;
        gameData.lastADRewardTime = DateTime.Now;
        gameData.passRewardVideoTime = 0;
        SaveGameData();
    }

    /// <summary>
    /// �µĵ�¼��
    /// </summary>
    public void NewDay()
    {
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void SaveAllData()
    {
        DataMgr.Ins.SaveGameData();
        DataMgr.Ins.SavePlayerData();
        DataMgr.Ins.SaveSettingData();
    }
}

public class SaveField
{
    public const string playerData = "playerData";
    public const string gameData = "gameData";
    public const string settingData = "settingData";
}
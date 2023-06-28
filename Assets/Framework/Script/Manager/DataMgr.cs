using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

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
        InitGameData();
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
    /// ��ʼ����Ϸ����
    /// </summary>
    void InitGameData()
    {
        if (gameData == null)
        {
            gameData = new GameData();
            //��ʼ����ϵ����
            foreach (var faction in ConfigMgr.Ins.factionConfig.faction)
            {
                int factionID = faction.Key;
                FactionData factionData = new FactionData();
                gameData.factions[factionID] = factionData;
                foreach (var city in ConfigMgr.Ins.cityConfig.city)
                {
                    if (city.Value.factionID == factionID)
                    {
                        factionData.citys.Add(city.Key);
                    }
                }
            }
            //��������
            foreach (var city in ConfigMgr.Ins.cityConfig.city)
            {
                int cityID = city.Key;
                gameData.cityData.Add(cityID, new CityData(cityID));
            }
            //��ʼ���̶�����
            foreach (var city in ConfigMgr.Ins.cityConfig.city)
            {
                int tradeNum = city.Value.tradeCaravan_num;
                for (int i = 0; i < tradeNum; i++)
                {
                    int cityID = city.Key;
                    CityConfig cityConfig = ConfigMgr.Ins.cityConfig.city[cityID];
                    int factionID = CityMgr.Ins.GetCityFactionID(cityID);
                    TroopData troopData = new TroopData(TroopType.Trade);
                    troopData.wuid = GetWUID(WorldUnitType.troop);
                    troopData.posX = cityConfig.posX;
                    troopData.posY = cityConfig.posY;
                    troopData.cityID = cityID;
                    gameData.factions[factionID].troops.Add(troopData);
                }
            }
            SaveGameData();
        }
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

    /// <summary>
    /// ��ȡ�������ID
    /// </summary>
    /// <param name="_worldUnitType"></param>
    /// <returns></returns>
    public int GetWUID(WorldUnitType _worldUnitType, int _wuidOffset = 0)
    {
        int startIndex = (int)_worldUnitType * 1000;
        if (_worldUnitType == WorldUnitType.troop)
        {
            List<int> ids = new List<int>();
            foreach (var faction in DataMgr.Ins.gameData.factions)
            {
                foreach (var troop in faction.Value.troops)
                {
                    ids.Add(troop.wuid);
                }
            }
            int index = startIndex;
            while (ids.Contains(index))
            {
                index++;
            }
            return index;
        }
        return startIndex;
    }
}

public class SaveField
{
    public const string playerData = "playerData";
    public const string gameData = "gameData";
    public const string settingData = "settingData";
}
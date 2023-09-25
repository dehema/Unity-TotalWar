using JetBrains.Annotations;
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
    /// 玩家存档
    /// </summary>
    public PlayerData playerData;
    bool isLoaded = false;
    public GameData gameData;
    public SettingData settingData;
    /// <summary>
    /// 玩家派系
    /// </summary>
    public const int playerFactionID = 11;

    public void Load()
    {
        if (isLoaded)
        {
            return;
        }
        //玩家数据
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
            Utility.Dump("-------------------------------玩家数据--------------------------------");
            playerData.SetVal(dict);
        }
        //游戏数据
        gameData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(SaveField.gameData));
        InitGameData();
        //设置
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
    /// 初始化游戏数据
    /// </summary>
    void InitGameData()
    {
        if (gameData == null)
        {
            gameData = new GameData();
            //初始化派系数据
            foreach (var faction in ConfigMgr.Ins.factionConfig.faction)
            {
                int factionID = faction.Key;
                if (gameData.factions.ContainsKey(factionID))
                    continue;
                FactionData factionData = new FactionData();
                gameData.factions[factionID] = factionData;
                foreach (var city in ConfigMgr.Ins.allCityConfig.city)
                {
                    if (city.Value.factionID == factionID)
                    {
                        factionData.citys.Add(city.Key);
                    }
                }
            }
            //城镇数据
            foreach (var city in ConfigMgr.Ins.allCityConfig.city)
            {
                int cityID = city.Key;
                CityData cityData = new CityData(cityID);
                cityData.wuid = CommonMgr.Ins.GetCityWUID(cityID);
                gameData.cityData.Add(cityID, cityData);
            }
            //初始化商队数据
            foreach (var config in ConfigMgr.Ins.allCityConfig.city)
            {
                CityData cityData = GetCityData(config.Key);
                cityData.RefreshTradeTroop();
            }
            //玩家军队
            TroopData playerTroop = new TroopData(TroopType.Player);
            playerTroop.wuid = CommonMgr.Ins.GetWUID(WorldUnitType.player);
            FactionConfig playerFactionConfig = ConfigMgr.Ins.GetFactionConfig(playerFactionID);
            playerTroop.units = new Dictionary<int, int>(playerFactionConfig.init_troop_unit);
            FactionData playerFactionData = GetFactionData(playerFactionID);
            playerFactionData.AddTroop(playerTroop);
            SaveGameData();
        }
    }

    /// <summary>
    /// 保存游戏数据 （设置、签到等）
    /// </summary>
    public void SaveGameData()
    {
        string data = JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString(SaveField.gameData, data);
    }

    /// <summary>
    /// 保存玩家数据（属性数值）
    /// </summary>
    public void SavePlayerData()
    {
        string str = playerData.ToJson();
        PlayerPrefs.SetString(SaveField.playerData, str);
    }

    /// <summary>
    /// 保存设置数据
    /// </summary>
    public void SaveSettingData()
    {
        settingData.musicVolume = AudioMgr.Ins.musicVolume;
        settingData.soundVolume = AudioMgr.Ins.soundVolume;
        PlayerPrefs.SetString(SaveField.settingData, JsonConvert.SerializeObject(settingData));
    }

    /// <summary>
    /// 玩家登陆
    /// </summary>
    public void Login()
    {
        if (gameData.lastLoginDate.Date != DateTime.Now.Date)
        {
            NewDay();
        }
        gameData.lastLoginDate = DateTime.Now;
        //清空广告计数
        gameData.lastADInterstitialTime = DateTime.Now;
        gameData.lastADRewardTime = DateTime.Now;
        gameData.passRewardVideoTime = 0;
        SaveGameData();
    }

    /// <summary>
    /// 新的登录日
    /// </summary>
    public void NewDay()
    {

    }

    /// <summary>
    /// 保存所有数据
    /// </summary>
    public void SaveAllData()
    {
        DataMgr.Ins.SaveGameData();
        DataMgr.Ins.SavePlayerData();
        DataMgr.Ins.SaveSettingData();
    }

    /// <summary>
    /// 获取派系数据
    /// </summary>
    /// <param name="_factionID"></param>
    /// <returns></returns>
    public FactionData GetFactionData(int _factionID)
    {
        return gameData.factions[_factionID];
    }

    /// <summary>
    /// 获取城镇数据
    /// </summary>
    /// <param name="_cityID"></param>
    /// <returns></returns>
    public CityData GetCityData(int _cityID)
    {
        return gameData.cityData[_cityID];
    }

    /// <summary>
    /// 获取科技数据
    /// </summary>
    /// <param name="_techID"></param>
    /// <returns></returns>
    public TechData GetTechData(int _techID)
    {
        return new TechData();
    }
}

public class SaveField
{
    public const string playerData = "playerData";
    public const string gameData = "gameData";
    public const string settingData = "settingData";
}
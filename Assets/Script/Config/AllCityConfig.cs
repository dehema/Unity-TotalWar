using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllCityConfig : ConfigBase
{
    public Dictionary<int, CityConfig> city = new Dictionary<int, CityConfig>();
    public Dictionary<RaceType, RaceBuildingConfig> race = new Dictionary<RaceType, RaceBuildingConfig>();
    public Dictionary<int, BuildingConfig> building = new Dictionary<int, BuildingConfig>();
    public Dictionary<int, RecruitDailyConfig> recruitDaily = new Dictionary<int, RecruitDailyConfig>();

    public override void Init()
    {
        base.Init();
        foreach (var item in race)
        {
            item.Value.Init();
        }
        foreach (var item in building)
        {
            item.Value.Init();
        }
        foreach (var item in recruitDaily)
        {
            item.Value.Init();
        }
    }
}

public class CityConfig
{
    public int ID;
    public string name;
    public float posX;
    public float posY;
    public string icon;
    public RaceType raceType;
}

/// <summary>
/// 种族建筑
/// </summary>
public class RaceBuildingConfig
{
    /// <summary>
    /// 种族类型
    /// </summary>
    public RaceType raceType;
    public string _mainBase;
    public string _military;
    public string _economy;
    public string _defaultBuilding;
    public string _initial_Building;
    /// <summary>
    /// 主要序列
    /// </summary>
    public List<int> mainBase = new List<int>();
    /// <summary>
    /// 军事序列
    /// </summary>
    public List<int> military = new List<int>();
    /// <summary>
    /// 经济序列
    /// </summary>
    public List<int> economy = new List<int>();
    /// <summary>
    /// 默认被建造的建筑
    /// </summary>
    public List<int> defaultBuilding = new List<int>();
    /// <summary>
    /// 初始可显示的
    /// </summary>
    public List<int> initial_Building = new List<int>();
    public void Init()
    {
        Array.ForEach(_mainBase.Split(','), val => { mainBase.Add(int.Parse(val)); });
        Array.ForEach(_military.Split(','), val => { military.Add(int.Parse(val)); });
        Array.ForEach(_economy.Split(','), val => { economy.Add(int.Parse(val)); });
        Array.ForEach(_defaultBuilding.Split(','), val => { defaultBuilding.Add(int.Parse(val)); });
        Array.ForEach(_initial_Building.Split(','), val => { initial_Building.Add(int.Parse(val)); });
    }
}

public class BuildingConfig
{
    public int ID;
    /// <summary>
    /// 建筑ID
    /// </summary>
    public string buildingID;
    /// <summary>
    /// 种族
    /// </summary>
    public RaceType raceType;
    /// <summary>
    /// 建筑类型
    /// </summary>
    public BuildingType buildingType;
    /// <summary>
    /// 建筑二级类型
    /// </summary>
    public BuildingSubType buildingSubType;
    /// <summary>
    /// 建筑名称
    /// </summary>
    public string name;
    /// <summary>
    /// 前置建筑
    /// </summary>
    public string _preBuildingIDs;
    /// <summary>
    /// 升级后的建筑ID
    /// </summary>
    public int upgradeBuildingID = 0;
    /// <summary>
    /// 图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 花费金钱
    /// </summary>
    public int costGold;
    /// <summary>
    /// 升级耗时
    /// </summary>
    public int costHour;
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool enable;

    /// <summary>
    /// 前置建筑
    /// </summary>
    public List<int> preBuildingIDs = new List<int>();

    public void Init()
    {
        if (!string.IsNullOrEmpty(_preBuildingIDs))
            Array.ForEach(_preBuildingIDs.Split(','), val => { preBuildingIDs.Add(int.Parse(val)); });
    }
}

/// <summary>
/// 军事设施每日招募设置
/// </summary>
public class RecruitDailyConfig
{
    public int ID;
    /// <summary>
    /// 生成总数
    /// </summary>
    public int totalNum;
    public string _recruitNum;
    /// <summary>
    /// 招募
    /// </summary>
    public Dictionary<int, int> recruitNum;

    public void Init()
    {
        recruitNum = JsonConvert.DeserializeObject<Dictionary<int, int>>(_recruitNum);
    }
}
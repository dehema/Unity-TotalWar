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
    /// <summary>
    /// 主要序列
    /// </summary>
    public int MainBase;
    /// <summary>
    /// 近战序列
    /// </summary>
    public int Barracks;
    /// <summary>
    /// 远程序列
    /// </summary>
    public int Range;
    /// <summary>
    /// 骑兵序列
    /// </summary>
    public int Stable;
    /// <summary>
    /// 经济序列
    /// </summary>
    public int Economy;
    public string _defaultBuilding;
    /// <summary>
    /// 默认建筑
    /// </summary>
    public List<int> defaultBuilding = new List<int>();
    public void Init()
    {
        Array.ForEach(_defaultBuilding.Split(','), val => { defaultBuilding.Add(int.Parse(val)); });
    }
}

public class BuildingConfig
{
    public int ID;
    public string buildingID;
    public string raceType;
    public BuildingType buildingType;
    public BuildingSubType buildingSubType;
    public string _name;
    public string _mainBaseLv;
    public string _costGold;
    public string _costHour;

    public List<string> name;
    public List<int> mainBaseLv = new List<int>();
    public List<int> costGold = new List<int>();
    public List<int> costHour = new List<int>();

    public void Init()
    {
        name = new List<string>(_name.Split(','));
        Array.ForEach(_name.Split(','), val => { mainBaseLv.Add(int.Parse(val)); });
        Array.ForEach(_mainBaseLv.Split(','), val => { mainBaseLv.Add(int.Parse(val)); });
        Array.ForEach(_costGold.Split(','), val => { costGold.Add(int.Parse(val)); });
        Array.ForEach(_costHour.Split(','), val => { costHour.Add(int.Parse(val)); });
    }
}

/// <summary>
/// 军事设施每日招募设置
/// </summary>
public class RecruitDailyConfig
{
    public int ID;
    public string _lv1;
    public string _lv2;
    public string _lv3;
    public string _lv4;
    public string _lv5;
    public RecruitDailyLvConfig lv1;
    public RecruitDailyLvConfig lv2;
    public RecruitDailyLvConfig lv3;
    public RecruitDailyLvConfig lv4;
    public RecruitDailyLvConfig lv5;

    public void Init()
    {
        lv1 = JsonConvert.DeserializeObject<RecruitDailyLvConfig>(_lv1);
        lv2 = JsonConvert.DeserializeObject<RecruitDailyLvConfig>(_lv2);
        lv3 = JsonConvert.DeserializeObject<RecruitDailyLvConfig>(_lv3);
        lv4 = JsonConvert.DeserializeObject<RecruitDailyLvConfig>(_lv4);
        lv5 = JsonConvert.DeserializeObject<RecruitDailyLvConfig>(_lv5);
    }
}
public class RecruitDailyLvConfig
{
    /// <summary>
    /// 每日生成的士兵总数
    /// </summary>
    public int totalNum;
    /// <summary>
    /// 士兵ID和权重
    /// </summary>
    public Dictionary<int, int> recruitNum;
}
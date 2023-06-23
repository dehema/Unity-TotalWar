using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionConfig : ConfigBase
{
    public Dictionary<RaceType, RaceBuildingConfig> race = new Dictionary<RaceType, RaceBuildingConfig>();

    public override void Init()
    {
        base.Init();
        foreach (var item in race)
        {
            item.Value.Init();
        }
    }
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
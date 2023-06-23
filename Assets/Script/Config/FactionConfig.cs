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
/// ���彨��
/// </summary>
public class RaceBuildingConfig
{
    /// <summary>
    /// ��������
    /// </summary>
    public RaceType raceType;
    public string _mainBase;
    public string _military;
    public string _economy;
    public string _defaultBuilding;
    public string _initial_Building;
    /// <summary>
    /// ��Ҫ����
    /// </summary>
    public List<int> mainBase = new List<int>();
    /// <summary>
    /// ��������
    /// </summary>
    public List<int> military = new List<int>();
    /// <summary>
    /// ��������
    /// </summary>
    public List<int> economy = new List<int>();
    /// <summary>
    /// Ĭ�ϱ�����Ľ���
    /// </summary>
    public List<int> defaultBuilding = new List<int>();
    /// <summary>
    /// ��ʼ����ʾ��
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
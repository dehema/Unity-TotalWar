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
/// ���彨��
/// </summary>
public class RaceBuildingConfig
{
    /// <summary>
    /// ��������
    /// </summary>
    public RaceType raceType;
    public string _MainBase;
    public string _Military;
    public string _Economy;
    public string _defaultBuilding;
    /// <summary>
    /// ��Ҫ����
    /// </summary>
    public List<int> MainBase = new List<int>();
    /// <summary>
    /// ��������
    /// </summary>
    public List<int> Military = new List<int>();
    /// <summary>
    /// ��������
    /// </summary>
    public List<int> Economy = new List<int>();
    /// <summary>
    /// Ĭ�Ͻ���
    /// </summary>
    public List<int> DefaultBuilding = new List<int>();
    public void Init()
    {
        Array.ForEach(_MainBase.Split(','), val => { MainBase.Add(int.Parse(val)); });
        Array.ForEach(_Military.Split(','), val => { Military.Add(int.Parse(val)); });
        Array.ForEach(_Economy.Split(','), val => { Economy.Add(int.Parse(val)); });
        Array.ForEach(_defaultBuilding.Split(','), val => { DefaultBuilding.Add(int.Parse(val)); });
    }
}

public class BuildingConfig
{
    public int ID;
    /// <summary>
    /// ����ID
    /// </summary>
    public string buildingID;
    /// <summary>
    /// ����
    /// </summary>
    public RaceType raceType;
    /// <summary>
    /// ��������
    /// </summary>
    public BuildingType buildingType;
    /// <summary>
    /// ������������
    /// </summary>
    public BuildingSubType buildingSubType;
    /// <summary>
    /// ��������
    /// </summary>
    public string name;
    /// <summary>
    /// ǰ�ý���
    /// </summary>
    public string _preBuildingIDs;
    /// <summary>
    /// ������Ľ���ID
    /// </summary>
    public string _upgradeBuildingIDs;
    /// <summary>
    /// ͼ��
    /// </summary>
    public string icon;
    /// <summary>
    /// ���ѽ�Ǯ
    /// </summary>
    public int costGold;
    /// <summary>
    /// ������ʱ
    /// </summary>
    public int costHour;
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool enable;

    /// <summary>
    /// ǰ�ý���
    /// </summary>
    public List<int> preBuildingIDs = new List<int>();
    /// <summary>
    /// ������Ľ���ID
    /// </summary>
    public List<int> upgradeBuildingIDs = new List<int>();

    public void Init()
    {
        if (!string.IsNullOrEmpty(_preBuildingIDs))
            Array.ForEach(_preBuildingIDs.Split(','), val => { preBuildingIDs.Add(int.Parse(val)); });
        if (!string.IsNullOrEmpty(_upgradeBuildingIDs))
            Array.ForEach(_upgradeBuildingIDs.Split(','), val => { upgradeBuildingIDs.Add(int.Parse(val)); });
    }
}

/// <summary>
/// ������ʩÿ����ļ����
/// </summary>
public class RecruitDailyConfig
{
    public int ID;
    /// <summary>
    /// ��������
    /// </summary>
    public int totalNum;
    public string _recruitNum;
    /// <summary>
    /// ��ļ
    /// </summary>
    public Dictionary<int, int> recruitNum;

    public void Init()
    {
        recruitNum = JsonConvert.DeserializeObject<Dictionary<int, int>>(_recruitNum);
    }
}
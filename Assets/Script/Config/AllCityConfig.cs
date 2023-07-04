using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllCityConfig : ConfigBase
{
    public Dictionary<int, CityConfig> city = new Dictionary<int, CityConfig>();
    public Dictionary<int, BuildingConfig> building = new Dictionary<int, BuildingConfig>();
    public Dictionary<int, RecruitDailyConfig> recruitDaily = new Dictionary<int, RecruitDailyConfig>();

    public override void Init()
    {
        base.Init();
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
    /// <summary>
    /// X����
    /// </summary>
    public float posX;
    /// <summary>
    /// Y����
    /// </summary>
    public float posY;
    public string icon;
    /// <summary>
    /// ��ϵID
    /// </summary>
    public int factionID;
    /// <summary>
    /// �̶�����
    /// </summary>
    public int tradeCaravan_num;
    /// <summary>
    /// �̶ӳ�ʼ�ʽ�
    /// </summary>
    public int tradeCaravan_gold;
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
    public int upgradeBuildingID = 0;
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
    /// ÿ�չ���
    /// </summary>
    public int dailySalary;

    /// <summary>
    /// ǰ�ý���
    /// </summary>
    public List<int> preBuildingIDs = new List<int>();

    public void Init()
    {
        if (!string.IsNullOrEmpty(_preBuildingIDs))
            Array.ForEach(_preBuildingIDs.Split(','), val => { preBuildingIDs.Add(int.Parse(val)); });
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
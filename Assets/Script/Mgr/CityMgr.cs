using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class CityMgr : MonoSingleton<CityMgr>
{
    public AllCityConfig allCityConfig { get { return ConfigMgr.Ins.cityConfig; } }
    public Dictionary<int, WorldCityItem> allCityItem = new Dictionary<int, WorldCityItem>();

    /// <summary>
    /// ��һ�ν�������
    /// </summary>
    public void OnFirstEnterWorld()
    {
        WorldMgr.Ins.worldDate.onNewDay += () =>
        {
            //ˢ��();
            foreach (var item in allCityItem)
            {
                item.Value.cityData.RefreshRecruitUnit();
            }
            //��нˮ

            //ÿ������

        };
        WorldMgr.Ins.worldDate.onNewHour += () =>
        {
            AddInBuildingProgress();
        };
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="_cityID"></param>
    /// <returns></returns>
    public CityConfig GetCityConfig(int _cityID)
    {
        return allCityConfig.city[_cityID];
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="_buildingID"></param>.
    public BuildingConfig GetBuildingConfig(int _buildingID)
    {
        return allCityConfig.building[_buildingID];
    }

    /// <summary>
    /// ��ȡ����ÿ����ļ����
    /// </summary>
    public RecruitDailyConfig RecruitDailyConfig(int _buildingID)
    {
        return allCityConfig.recruitDaily[_buildingID];
    }

    /// <summary>
    /// ��ȡ���彨����Ϣ
    /// </summary>
    /// <returns></returns>
    public RaceConfig GetRaceConfig(RaceType _raceType)
    {
        return ConfigMgr.Ins.factionConfig.race[_raceType];
    }

    /// <summary>
    /// ��ȡ����ID
    /// </summary>
    /// <returns></returns>
    public CityData GetCityDataByID(int cityID)
    {
        return DataMgr.Ins.gameData.cityData[cityID];
    }

    /// <summary>
    /// ��ѯĳ������Ľ�������
    /// </summary>
    /// <returns></returns>
    public BuildingData GetBuildingData(int _cityID, BuildingType _buildingType)
    {
        CityData cityData = GetCityDataByID(_cityID);
        if (cityData == null)
        {
            return null;
        }
        return cityData.buildingDict[_cityID];
    }

    /// <summary>
    /// ˢ�����г������ļ��λ
    /// </summary>
    public void RefreshAllCityRecruitUnit()
    {
        foreach (var item in DataMgr.Ins.gameData.cityData)
        {
            item.Value.RefreshRecruitUnit();
        }
    }

    /// <summary>
    /// �Ƿ���ڽ��� ��������Ҳ��
    /// </summary>
    /// <param name="_cityID"></param>
    /// <param name="_buildingID"></param>
    /// <returns></returns>
    public bool IsHasBuilding(int _cityID, int _buildingID)
    {
        if (!DataMgr.Ins.gameData.cityData.ContainsKey(_cityID))
        {
            return false;
        }
        CityData cityData = DataMgr.Ins.gameData.cityData[_cityID];
        foreach (var item in cityData.buildingDict)
        {
            if (IsContainBuilding(item.Key, _buildingID))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ����A�Ƿ��������B
    /// </summary>
    /// <returns></returns>
    public bool IsContainBuilding(int _buildingIDA, int _buildingIDB)
    {
        return (_buildingIDA / 10 == _buildingIDB / 10) && (_buildingIDA >= _buildingIDB);
    }

    /// <summary>
    /// ���ӽ����н�������
    /// </summary>
    void AddInBuildingProgress()
    {
        foreach (var cityData in DataMgr.Ins.gameData.cityData)
        {
            for (int i = cityData.Value.inBuildIngData.Count - 1; i >= 0; i--)
            {
                KeyValuePair<int, InBuildIngData> data = cityData.Value.inBuildIngData.ElementAt(i);
                if (DataMgr.Ins.gameData.worldTime.TotalHour >= data.Value.endHour)
                {
                    //�������
                    UpgradeBuilding(cityData.Key, data.Value.originBuildingID, data.Value.targetBuildingID);
                }
            }
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="_cityID"></param>
    /// <param name="_buildingID"></param>
    public void UpgradeBuilding(int _cityID, int _originBuildingID, int _newBuildingID)
    {
        CityData cityData = DataMgr.Ins.gameData.cityData[_cityID];
        if (cityData.inBuildIngData.ContainsKey(_originBuildingID))
            cityData.inBuildIngData.Remove(_originBuildingID);
        if (cityData.buildingDict.ContainsKey(_originBuildingID))
            cityData.buildingDict.Remove(_originBuildingID);
        cityData.buildingDict.Add(_newBuildingID, new BuildingData(_newBuildingID));
        DataMgr.Ins.SaveAllData();
    }

    /// <summary>
    /// ��ѯ������ϵID
    /// </summary>
    /// <returns></returns>
    public int GetCityFactionID(int _cityID)
    {
        foreach (var factionData in DataMgr.Ins.gameData.factions)
        {
            foreach (var cityID in factionData.Value.citys)
            {
                if (cityID == _cityID)
                {
                    return factionData.Key;
                }
            }
        }
        return -1;
    }
}

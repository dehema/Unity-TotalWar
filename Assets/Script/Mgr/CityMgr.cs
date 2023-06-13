using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
    public RaceBuildingConfig GetRaceBuildingConfig(RaceType _raceType)
    {
        return allCityConfig.race[_raceType];
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
    /// ��������
    /// </summary>
    /// <param name="cityID"></param>
    /// <param name="_buildingID"></param>
    public void UpgradeBuilding(int cityID, int _buildingID)
    {

    }
}

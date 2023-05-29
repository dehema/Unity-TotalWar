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
    /// 第一次进入世界
    /// </summary>
    public void OnFirstEnterWorld()
    {
        WorldMgr.Ins.worldDate.onNewDay += () =>
        {
            //刷兵();
            foreach (var item in allCityItem)
            {
                item.Value.cityData.RefreshRecruitUnit();
            }
        };
    }

    /// <summary>
    /// 获取城镇配置
    /// </summary>
    /// <param name="_cityID"></param>
    /// <returns></returns>
    public CityConfig GetCityConfig(int _cityID)
    {
        return allCityConfig.city[_cityID];
    }

    /// <summary>
    /// 获取建筑配置
    /// </summary>
    /// <param name="_buildingID"></param>.
    public BuildingConfig GetBuildingConfig(int _buildingID)
    {
        return allCityConfig.building[_buildingID];
    }

    /// <summary>
    /// 获取种族建筑信息
    /// </summary>
    /// <returns></returns>
    public RaceBuildingConfig GetRaceBuildingConfig(RaceType _raceType)
    {
        return allCityConfig.race[_raceType];
    }

    /// <summary>
    /// 获取城镇ID
    /// </summary>
    /// <returns></returns>
    public CityData GetCityDataByID(int cityID)
    {
        return DataMgr.Ins.gameData.cityData[cityID];
    }

    /// <summary>
    /// 查询某个城镇的建筑数据
    /// </summary>
    /// <returns></returns>
    public BuildingData GetBuildingData(int _cityID, BuildingType _buildingType)
    {
        CityData cityData = GetCityDataByID(_cityID);
        if (cityData == null)
        {
            return null;
        }
        return cityData.buildingDict[_buildingType];
    }

    /// <summary>
    /// 获得某指定等级的建筑 每日刷新招募士兵的配置
    /// </summary>
    /// <param name="_buildingID"></param>
    /// <param name="_lv"></param>
    /// <returns></returns>
    public RecruitDailyLvConfig GetRecruitableDailyLv(int _buildingID, int _lv)
    {
        RecruitDailyConfig config = allCityConfig.recruitDaily[_buildingID];
        if (config == null)
            return null;
        string targetLv = "lv" + _lv;
        Type type = config.GetType();
        FieldInfo[] fieldInfos = type.GetFields();
        foreach (var f in fieldInfos)
        {
            if (f.Name == targetLv)
            {
                return f.GetValue(config) as RecruitDailyLvConfig;
            }
        }
        return null;
    }

    /// <summary>
    /// 刷新所有城镇的招募单位
    /// </summary>
    public void RefreshAllCityRecruitUnit()
    {
        foreach (var item in DataMgr.Ins.gameData.cityData)
        {
            item.Value.RefreshRecruitUnit();
        }
    }
}

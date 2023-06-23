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
            //发薪水

            //每日收入

        };
        WorldMgr.Ins.worldDate.onNewHour += () =>
        {
            AddInBuildingProgress();
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
    /// 获取建筑每日招募配置
    /// </summary>
    public RecruitDailyConfig RecruitDailyConfig(int _buildingID)
    {
        return allCityConfig.recruitDaily[_buildingID];
    }

    /// <summary>
    /// 获取种族建筑信息
    /// </summary>
    /// <returns></returns>
    public RaceConfig GetRaceConfig(RaceType _raceType)
    {
        return ConfigMgr.Ins.factionConfig.race[_raceType];
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
        return cityData.buildingDict[_cityID];
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

    /// <summary>
    /// 是否存在建筑 升级过的也算
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
    /// 建筑A是否包含建筑B
    /// </summary>
    /// <returns></returns>
    public bool IsContainBuilding(int _buildingIDA, int _buildingIDB)
    {
        return (_buildingIDA / 10 == _buildingIDB / 10) && (_buildingIDA >= _buildingIDB);
    }

    /// <summary>
    /// 增加建造中建筑进度
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
                    //建造完成
                    UpgradeBuilding(cityData.Key, data.Value.originBuildingID, data.Value.targetBuildingID);
                }
            }
        }
    }

    /// <summary>
    /// 升级建筑
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
    /// 查询城镇派系ID
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

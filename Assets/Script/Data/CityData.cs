using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 据点数据
/// </summary>
public class CityData
{
    /// <summary>
    /// 城镇ID
    /// </summary>
    public int cityID;
    /// <summary>
    /// 繁荣度
    /// </summary>
    public int develop = 0;
    /// <summary>
    /// 繁荣进度 100进制 
    /// </summary>
    public float developProgress = 0;
    /// <summary>
    /// 存在建筑信息
    /// </summary>
    public Dictionary<int, BuildingData> buildingDict = new Dictionary<int, BuildingData>();
    /// <summary>
    /// 建造中的建筑信息
    /// </summary>
    public Dictionary<int, InBuildIngData> inBuildIngData = new Dictionary<int, InBuildIngData>();
    /// <summary>
    /// 可招募单位 <士兵ID,士兵数量>
    /// </summary>
    public Dictionary<int, int> recruitUnit = new Dictionary<int, int>();

    public CityData() { }
    public CityData(int _cityID)
    {
        cityID = _cityID;
        CityConfig cityConfig = CityMgr.Ins.GetCityConfig(_cityID);
        RaceBuildingConfig _raceBuildingConfig = CityMgr.Ins.GetRaceBuildingConfig(cityConfig.raceType);
        //填充默认建筑
        Action<List<int>> fillData = (buildingIDs) =>
        {
            foreach (var buildingID in buildingIDs)
            {
                var buildingConfig = CityMgr.Ins.GetBuildingConfig(buildingID);
                buildingDict[buildingID] = new BuildingData(buildingID);
            }
        };
        fillData(_raceBuildingConfig.MainBase);
        fillData(_raceBuildingConfig.Military);
        fillData(_raceBuildingConfig.Economy);
        fillData(_raceBuildingConfig.DefaultBuilding);
    }

    /// <summary>
    /// 获取建筑信息
    /// </summary>
    /// <param name="_buildingID"></param>
    /// <returns></returns>
    public BuildingData GetBuildingData(int _buildingID)
    {
        foreach (var item in buildingDict)
        {
            if (item.Value.id == _buildingID)
            {
                return item.Value;
            }
        }
        return null;
    }

    /// <summary>
    /// 刷新招募单位
    /// </summary>
    public void RefreshRecruitUnit()
    {
        recruitUnit.Clear();
        foreach (var item in buildingDict)
        {
            if (item.Value.buildingConfig.buildingType != BuildingType.Military)
            {
                //是否是军事序列建筑
                continue;
            }
            int buildingID = item.Value.id;
            BuildingConfig buildingConfig = CityMgr.Ins.GetBuildingConfig(buildingID);
            if (buildingConfig.buildingType != BuildingType.Military)
                continue;
            Dictionary<int, int> units = item.Value.GetRecruitUnitDaily();
            foreach (var unit in units)
            {
                if (recruitUnit.ContainsKey(unit.Key))
                {
                    recruitUnit[unit.Key] += unit.Value;
                }
                else
                {
                    recruitUnit.Add(unit.Key, unit.Value);
                }
            }
        }
        CityConfig cityConfig = CityMgr.Ins.GetCityConfig(cityID);
        Utility.Dump(recruitUnit, $"城镇{LangMgr.Ins.Get(cityConfig.name)}每日刷洗招募士兵");
    }
}

/// <summary>
/// 建筑数据
/// </summary>
public class BuildingData
{
    public int id;
    public BuildingConfig buildingConfig;
    public BuildingData() { }
    public BuildingData(int _buildingID)
    {
        id = _buildingID;
        buildingConfig = CityMgr.Ins.GetBuildingConfig(id);
    }

    /// <summary>
    /// 获取每日招募单位和数量
    /// </summary>
    public Dictionary<int, int> GetRecruitUnitDaily()
    {
        Dictionary<int, int> units = new Dictionary<int, int>();
        List<int> options = new List<int>();
        List<int> weights = new List<int>();
        RecruitDailyConfig recruitConfig = CityMgr.Ins.RecruitDailyConfig(id);
        //获取权重
        foreach (var item in recruitConfig.recruitNum)
        {
            options.Add(item.Key);
            weights.Add(item.Value);
        }
        for (int i = 0; i < recruitConfig.totalNum; i++)
        {
            int unitID = RandomTools.GetWeightRandomIndex(options, weights);
            if (units.ContainsKey(unitID))
            {
                units[unitID]++;
            }
            else
            {
                units[unitID] = 1;
            }
        }
        return units;
    }
}

/// <summary>
/// 正在建造的建筑数据
/// </summary>
public class InBuildIngData
{
    //开始时间
    public int startHour;
    //结束时间
    public int endHour;
    //被升级建筑
    public int originBuildingID;
    //升级后的建筑
    public int targetBuildingID;
}
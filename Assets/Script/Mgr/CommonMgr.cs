using System;
using System.Collections.Generic;
using UnityEditor;

public class CommonMgr : Singleton<CommonMgr>
{
    /// <summary>
    /// 获取世界对象ID
    /// </summary>
    /// <param name="_worldUnitType"></param>
    /// <returns></returns>
    public int GetWUID(WorldUnitType _worldUnitType)
    {
        int startIndex = (int)_worldUnitType * 1000;
        if (_worldUnitType == WorldUnitType.troop)
        {
            //部队类型
            List<int> ids = new List<int>();
            foreach (var faction in DataMgr.Ins.gameData.factions)
            {
                foreach (var tradeTroops in faction.Value.tradeTroops)
                {
                    foreach (var troop in tradeTroops.Value)
                    {
                        ids.Add(troop.wuid);
                    }
                }
            }
            int index = startIndex;
            while (ids.Contains(index))
            {
                index++;
            }
            return index;
        }
        return startIndex;
    }

    /// <summary>
    /// 获取城镇世界ID
    /// </summary>
    /// <param name="_cityID"></param>
    /// <returns></returns>
    public int GetCityWUID(int _cityID)
    {
        return GetWUID(WorldUnitType.city) + _cityID;
    }

    /// <summary>
    /// 根据部队信息获取所属派系ID
    /// </summary>
    public int GetFactionIDByTroop(TroopData _troopData)
    {
        foreach (var faction in DataMgr.Ins.gameData.factions)
        {
            foreach (var tradeTroops in faction.Value.tradeTroops)
            {
                foreach (var troopData in tradeTroops.Value)
                {
                    if (troopData == _troopData)
                    {
                        return faction.Key;
                    }
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// 增加派系金币
    /// </summary>
    public void AddFactionMoney(int _factionID, int _gold)
    {
        DataMgr.Ins.gameData.factions[_factionID].gold += _gold;
    }

    /// <summary>
    /// 获取派系商队数量
    /// </summary>
    /// <returns></returns>
    public int GetFactionTradeTotalNum(int _factionID)
    {
        FactionData data = DataMgr.Ins.gameData.factions[_factionID];
        int num = 0;
        foreach (var troops in data.tradeTroops)
        {
            num += troops.Value.Count;
        }
        return num;
    }

    /// <summary>
    /// 获取城镇商队数量
    /// </summary>
    /// <param name="_cityID"></param>
    /// <returns></returns>
    public int GetCityTradeTotalNum(int _cityID)
    {
        int factionID = GetCityFactionID(_cityID);
        FactionData factionData = DataMgr.Ins.GetFactionData(factionID);
        if (!factionData.tradeTroops.ContainsKey(_cityID))
            return 0;
        return factionData.tradeTroops[_cityID].Count;
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

    /// <summary>
    /// 移除指定商队
    /// </summary>
    /// <param name="factionID"></param>
    /// <param name="_troopData"></param>
    public void RemoveFactionTrade(int _factionID, TroopData _troopData)
    {
        FactionData factionData = DataMgr.Ins.GetFactionData(_factionID);
        factionData.RemoveTradeTroop(_troopData);
    }
}

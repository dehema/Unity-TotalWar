using System;
using System.Collections.Generic;

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
            List<int> ids = new List<int>();
            foreach (var faction in DataMgr.Ins.gameData.factions)
            {
                foreach (var troop in faction.Value.troops)
                {
                    ids.Add(troop.wuid);
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
}

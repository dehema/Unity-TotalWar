using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FactionData
{
    /// <summary>
    /// 金币
    /// </summary>
    public int gold;
    /// <summary>
    /// 建筑数据
    /// </summary>
    public List<int> citys = new List<int>();
    /// <summary>
    /// 部队数据 <TroopData>
    /// </summary>
    public List<TroopData> troops = new List<TroopData>();
    /// <summary>
    /// 部队数据 <CityID, TroopData>
    /// </summary>
    public Dictionary<int, List<TroopData>> tradeTroops = new Dictionary<int, List<TroopData>>();

    /// <summary>
    /// 增加部队
    /// </summary>
    /// <param name="_troopData"></param>
    public void AddTroop(TroopData _troopData)
    {
        troops.Add(_troopData);
    }

    /// <summary>
    /// 移除部队
    /// </summary>
    /// <param name="_troopData"></param>
    public void RemoveTroop(TroopData _troopData)
    {
        troops.Remove(_troopData);
    }

    /// <summary>
    /// 增加商队
    /// </summary>
    /// <param name="_cityID"></param>
    /// <param name="_troopData"></param>
    public void AddTradeTroop(int _cityID, TroopData _troopData)
    {
        if (!tradeTroops.ContainsKey(_cityID))
        {
            tradeTroops.Add(_cityID, new List<TroopData>());
        }
        List<TroopData> troops = tradeTroops[_cityID];
        troops.Add(_troopData);
    }

    /// <summary>
    /// 移除商队
    /// </summary>
    /// <param name="_cityID"></param>
    /// <param name="_troopData"></param>
    public void RemoveTradeTroop(TroopData _troopData)
    {
        foreach (var troopDatas in tradeTroops.Values)
        {
            if (troopDatas.Contains(_troopData))
            {
                troopDatas.Remove(_troopData);
                return;
            }
        }
    }
}

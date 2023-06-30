using System.Collections;
using System.Collections.Generic;
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
    /// 部队数据 <WUID, TroopData>
    /// </summary>
    public List<TroopData> troops = new List<TroopData>();
}

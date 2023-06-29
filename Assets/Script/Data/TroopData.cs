using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopData
{
    /// <summary>
    /// 世界唯一ID
    /// </summary>
    public int wuid;
    public TroopType troopType = TroopType.Army;
    public float posX;
    public float posY;
    public Dictionary<int, int> units = new Dictionary<int, int>();
    /// <summary>
    /// 所属城镇ID
    /// </summary>
    public int cityID;
    /// <summary>
    /// 目标世界ID
    /// </summary>
    public int targetWUID;
    public TroopState troopState = TroopState.wait;
    /// <summary>
    /// 携带金币
    /// </summary>
    public int initGold = 100;
    /// <summary>
    /// 携带金币
    /// </summary>
    public int gold = 100;

    public TroopData(TroopType _troopType)
    {
        troopType = _troopType;
    }
}


public enum TroopState
{
    wait,
    /// <summary>
    /// 向目标移动
    /// </summary>
    moveTarget,
    /// <summary>
    /// 抵达目标
    /// </summary>
    arriveTarget,
    /// <summary>
    /// 向主城移动
    /// </summary>
    moveBackHome,
    /// <summary>
    /// 抵达主城
    /// </summary>
    arriveHome
}
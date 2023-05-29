using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public UnitConfig unitConfig;
    public float hpMax;
    public float hp;
    public float attack;
    public int exp;
    /// <summary>
    /// ¹¥»÷¼ä¸ô
    /// </summary>
    public float arttckInterval = 2;
    public ArmyType armyType;
    public string unitCreateIndex;

    public UnitData() { }

    public UnitData(int unitID)
    {
        unitConfig = ConfigMgr.Ins.GetUnitConfig(unitID);
        ResetData();
    }

    public UnitData(UnitConfig _unitConfig)
    {
        unitConfig = _unitConfig;
        ResetData();
    }

    public void ResetData()
    {
        hpMax = unitConfig.hp;
        hp = unitConfig.hp;
        attack = unitConfig.attack;
    }
}

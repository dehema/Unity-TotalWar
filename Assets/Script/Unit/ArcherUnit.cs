using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : UnitBase
{
    //Éä¼ýÉä»÷Î»ÖÃ
    Transform shootPos;
    public override void Init(UnitConfig unitConfig)
    {
        base.Init(unitConfig);
        shootPos = transform.Find("shootPos");
    }

    public override void AttackUnit(UnitBase _defender)
    {
        if (this == null)
        {
            return;
        }
        if (_defender == null || _defender.IsDead)
        {
            return;
        }
        GameObject itemArrow = BattleMgr.Ins.GetArrow(shootPos.transform, _defender.transform, this);
    }
}

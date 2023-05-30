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
        GameObject itemArrow = BattleMgr.Ins.arrowPool.Get();
        itemArrow.transform.position = shootPos.position;
    }
}

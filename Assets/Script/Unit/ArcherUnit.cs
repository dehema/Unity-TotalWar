using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : UnitBase
{
    //������λ��
    Transform shootPos;
    public override void Init(UnitConfig unitConfig)
    {
        base.Init(unitConfig);
        shootPos = transform.Find("shootPos");
    }

    public override void AttackUnit(UnitBase _defender)
    {
        GameObject itemArrow = BattleMgr.Ins.GetArrow(shootPos.transform, _defender.transform, this);
    }
}

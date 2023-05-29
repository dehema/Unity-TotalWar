using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class WorldTroop : WorldUnitBase
{
    NavMeshAgent nav;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        InitNav();
    }

    public void OnClick()
    {
        Utility.Dump($"µã»÷²¿¶Ó{wuid}");
        WorldMgr.Ins.worldPlayer.MoveToWorldUnit(this, NavPurpose.troop);
    }
}

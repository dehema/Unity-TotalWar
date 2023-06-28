using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class WorldTroop : WorldUnitBase
{
    NavMeshAgent nav;
    public TroopData troopData;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        InitNav();
    }

    public void OnClick()
    {
        Utility.Dump($"�������{wuid}");
        WorldMgr.Ins.worldPlayer.MoveToWorldUnit(this, NavPurpose.troop);
    }

    /// <summary>
    /// ��ʼó��
    /// </summary>
    public void StartTrade()
    {
        if (troopData.troopType != TroopType.Trade)
        {
            return;
        }
        if (troopData.troopState == TroopState.wait)
        {
            troopData.targetWUID = GetATradeCityID();
            SetTroopState(TroopState.moveTarget);
            NavMgr.Ins.SetNav(wuid, 2, NavPurpose.trade, troopData.targetWUID);
        }
    }

    /// <summary>
    /// ��ȡó�׳��ж���
    /// </summary>
    /// <returns></returns>
    int GetATradeCityID()
    {
        List<int> cityIDs = new List<int>();
        foreach (var item in ConfigMgr.Ins.cityConfig.city)
        {
            cityIDs.Add(item.Key);
        }
        cityIDs.Remove(troopData.cityID);
        return RandomTools.GetVal<int>(cityIDs);
    }

    public void SetTroopState(TroopState _troopState)
    {
        troopData.troopState = _troopState;
    }
}

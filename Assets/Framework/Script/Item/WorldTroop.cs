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
        Utility.Dump($"点击部队{wuid}");
        WorldMgr.Ins.worldPlayer.MoveToWorldUnit(this, NavPurpose.troop);
    }

    public void StartAction()
    {
        if (troopData.troopType == TroopType.Trade)
        {
            TradeState();
        }
    }

    void SetState(TroopState _troopState)
    {
        troopData.troopState = _troopState;
        StartAction();
    }

    void TradeState()
    {
        switch (troopData.troopState)
        {
            case TroopState.wait:
            case TroopState.moveTarget:
                {
                    if (troopData.targetWUID == 0)
                        troopData.targetWUID = GetRandomTradeCityGUID();
                    StartTrade();
                    break;
                }
            case TroopState.arriveTarget:
                {
                    troopData.gold = (int)(troopData.gold * 1.2f);
                    SetState(TroopState.moveBackHome);
                    break;
                }
            case TroopState.moveBackHome:
                {
                    troopData.targetWUID = CommonMgr.Ins.GetCityWUID(troopData.cityID);
                    StartTrade();
                    break;
                }
            case TroopState.arriveHome:
                break;
        }
    }

    /// <summary>
    /// 开始贸易
    /// </summary>
    public void StartTrade()
    {
        NavMgr.Ins.SetNav(wuid, 2, NavPurpose.trade, troopData.targetWUID);
    }

    /// <summary>
    /// 随机获取贸易城市
    /// </summary>
    /// <returns></returns>
    int GetRandomTradeCityGUID()
    {
        List<int> cityIDs = new List<int>();
        foreach (var item in ConfigMgr.Ins.cityConfig.city)
        {
            cityIDs.Add(item.Key);
        }
        cityIDs.Remove(troopData.cityID);
        int cityID = RandomTools.GetVal<int>(cityIDs);
        int cityWUID = CommonMgr.Ins.GetCityWUID(cityID);
        return cityWUID;
    }

    public void SetTroopState(TroopState _troopState)
    {
        troopData.troopState = _troopState;
    }
}

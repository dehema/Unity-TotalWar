using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldPlayer : WorldUnitBase
{
    //data
    //达到位置后的回调
    Action moveCB;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        //transform.position = new Vector3(0, posYOffset, 0);
        //nav
        InitNav();
        //camera
        //WorldMgr.Ins.worldCamera.
    }

    public void OnMoveBegin()
    {
        if (WorldMgr.Ins.worldDate.timeSpeed == TimeSpeed.pause)
        {
            WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.normal);
        }
    }

    /// <summary>
    /// 移动到某个位置
    /// </summary>
    /// <param name="_position"></param>
    public void MoveToPos(Vector3 _position)
    {
        OnMoveBegin();
        NavMgr.Ins.SetNav(wuid, 3.5f, NavPurpose.movePos, _position);
        WorldMgr.Ins.worldCamera.SetLookAtTarget(gameObject);
    }

    /// <summary>
    /// 移动到某个目标
    /// </summary>
    /// <param name="_worldUnitBase"></param>
    public void MoveToWorldUnit(WorldUnitBase _worldUnitBase, NavPurpose _navPurpose)
    {
        OnMoveBegin();
        NavMgr.Ins.SetNav(wuid, 3.5f, _navPurpose, _worldUnitBase.wuid);
        WorldMgr.Ins.worldCamera.SetLookAtTarget(gameObject);
    }

    /// <summary>
    /// 寻路完成
    /// </summary>
    /// <param name="_navData"></param>
    public override void OnNavArrive(NavData _navData)
    {
        base.OnNavArrive(_navData);
        switch (_navData.navPurpose)
        {
            case NavPurpose.movePos:
                Utility.Dump("玩家移动到目标点" + _navData.targetPos);
                break;
            case NavPurpose.city:
                WorldCityItem worldCityItem = _navData.targetUnit as WorldCityItem;
                Debug.Log("玩家移动到城镇" + LangMgr.Ins.Get(worldCityItem.cityConfig.name));
                CityInfoViewParams viewParams = new CityInfoViewParams(worldCityItem.cityConfig.ID);
                UIMgr.Ins.OpenView<CityInfoView>(viewParams);
                break;
            case NavPurpose.troop:
                WorldTroop worldTroop = _navData.targetUnit as WorldTroop;
                Debug.Log("玩家访问到部队:" + worldTroop.wuid);
                SceneMgr.Ins.ChangeScene(SceneID.BattleField, () =>
                {
                    BattleMgr.Ins.Init(worldTroop.troopData);
                });
                break;
        }
        //玩家移动后 暂停
        WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.pause);
    }
}

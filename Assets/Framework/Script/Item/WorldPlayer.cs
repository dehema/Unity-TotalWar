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
    bool isMoving = false;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        transform.position = new Vector3(0, posYOffset, 0);
        isMoving = false;
        //nav
        InitNav();
        //camera
        //WorldMgr.Ins.worldCamera.
    }

    private void Update()
    {
        isMoving = nav.velocity != Vector3.zero;
        MoveAniamtion();
    }

    public override void SetWUID()
    {
        wuidOffset = 0;
        base.SetWUID();
    }

    //方向值，控制来回旋转
    int rotDir = 1;
    //旋转的局部坐标z值
    float axisZ = 0;
    /// <summary>
    /// 移动动画
    /// </summary>
    private void MoveAniamtion()
    {
        if (!isMoving)
        {
            if (transform.localEulerAngles.z != 0)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
            return;
        }
        axisZ += 200f * Time.deltaTime * rotDir;
        if (axisZ >= 20f)
            rotDir = -1;
        if (axisZ <= -20f)
            rotDir = 1;
        axisZ = ClampAngle(axisZ, -20f, 20f);
        Quaternion quaternion = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, axisZ);
        transform.localRotation = quaternion;
    }

    /// <summary>
    ///  角度区间
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
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
                    BattleMgr.Ins.Init();
                });
                break;
        }
        //玩家移动后 暂停
        WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.pause);
    }
}

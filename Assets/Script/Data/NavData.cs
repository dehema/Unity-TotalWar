using UnityEngine;
using UnityEngine.AI;

public class NavData
{
    /// <summary>
    /// 自身ID
    /// </summary>
    public int selfWUID;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 移动目的
    /// </summary>
    public NavPurpose navPurpose;
    /// <summary>
    /// 目标ID
    /// </summary>
    public int targetWUID;
    /// <summary>
    /// 目标位置
    /// </summary>
    public Vector3 targetPos;
    /// <summary>
    /// 寻路组件
    /// </summary>
    public NavMeshAgent navAgent;
    /// <summary>
    /// 自身单位
    /// </summary>
    public WorldUnitBase selfUnit;
    /// <summary>
    /// 目标单位
    /// </summary>
    public WorldUnitBase targetUnit;

    public NavData(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose)
    {
        selfWUID = _selfWUID;
        moveSpeed = _moveSpeed;
        selfUnit = WorldMgr.Ins.worldUnitDict[_selfWUID];
        navAgent = selfUnit.GetComponent<NavMeshAgent>();
        navPurpose = _navPurpose;
        RefreshNavSpeed();
    }

    public void SetTargetPos(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        SetDestination();
    }

    public void SetTargetWUID(int _wuid)
    {
        targetWUID = _wuid;
        targetUnit = WorldMgr.Ins.GetUnitByWUID(_wuid);
        SetDestination();
    }

    public void SetDestination()
    {
        if (targetUnit != null)
        {
            navAgent.SetDestination(targetUnit.transform.position);
        }
        else
        {
            navAgent.SetDestination(targetPos);
        }
    }

    /// <summary>
    /// 刷新寻路组件
    /// </summary>
    public void RefreshNavSpeed()
    {
        navAgent.speed = moveSpeed * WorldMgr.Ins.worldDate.speed;
    }
}
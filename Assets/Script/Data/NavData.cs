using UnityEngine;
using UnityEngine.AI;

public class NavData
{
    /// <summary>
    /// ����ID
    /// </summary>
    public int selfWUID;
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// �ƶ�Ŀ��
    /// </summary>
    public NavPurpose navPurpose;
    /// <summary>
    /// Ŀ��ID
    /// </summary>
    public int targetWUID;
    /// <summary>
    /// Ŀ��λ��
    /// </summary>
    public Vector3 targetPos;
    /// <summary>
    /// Ѱ·���
    /// </summary>
    public NavMeshAgent navAgent;
    /// <summary>
    /// ����λ
    /// </summary>
    public WorldUnitBase selfUnit;
    /// <summary>
    /// Ŀ�굥λ
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
    /// ˢ��Ѱ·���
    /// </summary>
    public void RefreshNavSpeed()
    {
        navAgent.speed = moveSpeed * WorldMgr.Ins.worldDate.speed;
    }
}
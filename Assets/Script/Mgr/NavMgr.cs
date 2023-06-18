using JetBrains.Annotations;
using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ѱ·ģ��
/// </summary>
public class NavMgr : MonoSingleton<NavMgr>
{
    public Dictionary<int, NavData> navDict = new Dictionary<int, NavData>();

    public void SetNav(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose, int _targetWUID)
    {
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        navData.SetTargetWUID(_targetWUID);
    }

    public void SetNav(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose, Vector3 _targetPos)
    {
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        navData.SetTargetPos(_targetPos);
    }

    //�ϸ�Ѱ·������ID
    int lastEndNavID;
    bool havNavEnd = false;
    public void Update()
    {
        foreach (var item in navDict)
        {
            if (item.Value.navAgent.remainingDistance < 1)
            {
                lastEndNavID = item.Key;
                havNavEnd = true;
                item.Value.selfUnit.OnNavArrive(item.Value);
                break;
            }
        }
        if (havNavEnd)
        {
            navDict.Remove(lastEndNavID);
            havNavEnd = false;
        }
    }

    /// <summary>
    /// ˢ������Ѱ·���ٶ�
    /// </summary>
    public void RefreshAllNavSpeed()
    {
        foreach (var item in navDict)
        {
            item.Value.RefreshNavSpeed();
        }
    }
}
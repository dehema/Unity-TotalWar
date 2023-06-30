using JetBrains.Annotations;
using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 寻路模块
/// </summary>
public class NavMgr : MonoSingleton<NavMgr>
{
    public Dictionary<int, NavData> navDict = new Dictionary<int, NavData>();

    //设置寻路
    public void SetNav(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose, int _targetWUID)
    {
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        navDict[_selfWUID] = navData;
        navData.SetTargetWUID(_targetWUID);
    }

    //设置寻路
    public void SetNav(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose, Vector3 _targetPos)
    {
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        if (navDict.ContainsKey(_selfWUID))
        {
            navDict[_selfWUID] = null;
        }
        navDict[_selfWUID] = navData;
        navData.SetTargetPos(_targetPos);
    }

    //上个寻路结束的ID
    int lastEndNavID;
    bool havNavEnd = false;
    public void Update()
    {
        foreach (var item in navDict)
        {
            if (!item.Value.navAgent.pathPending && item.Value.navAgent.remainingDistance < 1)
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
    /// 刷新所有寻路的速度
    /// </summary>
    public void RefreshAllNavSpeed()
    {
        foreach (var item in navDict)
        {
            item.Value.RefreshNavSpeed();
        }
    }
}
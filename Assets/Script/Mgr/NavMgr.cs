using JetBrains.Annotations;
using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public void Update()
    {
        for (int i = 0; i < navDict.Count; i++)
        {
            var ele = navDict.ElementAt<KeyValuePair<int, NavData>>(i);
            if (!ele.Value.navAgent.pathPending && ele.Value.navAgent.remainingDistance < 1)
            {
                int wuid = ele.Key;
                NavData navData = ele.Value;
                navDict.Remove(wuid);
                navData.selfUnit.OnNavArrive(navData);
                navData = null;
            }
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

    /// <summary>
    /// 清除所有寻路
    /// </summary>
    /// <param name="_invoke"></param>
    public void ClearAllNavData(bool _invoke = false)
    {
        if (_invoke)
        {
            foreach (var item in navDict)
            {
                item.Value.selfUnit.OnNavArrive(item.Value);
            }
        }
        navDict.Clear();
    }
}
using JetBrains.Annotations;
using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ѱ·ģ��
/// </summary>
public class NavMgr : MonoSingleton<NavMgr>
{
    public Dictionary<int, NavData> navDict = new Dictionary<int, NavData>();

    //����Ѱ·
    public void SetNav(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose, int _targetWUID)
    {
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        navDict[_selfWUID] = navData;
        navData.SetTargetWUID(_targetWUID);
    }

    //����Ѱ·
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

    //�ϸ�Ѱ·������ID
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
    /// ˢ������Ѱ·���ٶ�
    /// </summary>
    public void RefreshAllNavSpeed()
    {
        foreach (var item in navDict)
        {
            item.Value.RefreshNavSpeed();
        }
    }

    /// <summary>
    /// �������Ѱ·
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
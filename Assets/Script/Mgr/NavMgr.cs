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
        ClearNavData(_selfWUID);
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        navData.SetTargetWUID(_targetWUID);
        navDict[_selfWUID] = navData;
    }

    //����Ѱ·
    public void SetNav(int _selfWUID, float _moveSpeed, NavPurpose _navPurpose, Vector3 _targetPos)
    {
        ClearNavData(_selfWUID);
        NavData navData = new NavData(_selfWUID, _moveSpeed, _navPurpose);
        if (navDict.ContainsKey(_selfWUID))
        {
            navDict[_selfWUID] = null;
        }
        navData.SetTargetPos(_targetPos);
        navDict[_selfWUID] = navData;
    }

    //�ϸ�Ѱ·������ID
    public void Update()
    {
        for (int i = navDict.Count - 1; i >= 0; i--)
        {
            var ele = navDict.ElementAt(i);
            NavData navData = ele.Value;
            ///
            if (!navData.navAgent.pathPending && navData.navAgent.remainingDistance != 0 && navData.navAgent.remainingDistance < 0.1f)
            {
                int wuid = ele.Key;
                navDict.Remove(wuid);
                navData.selfUnit.OnNavArrive(navData);
            }
            else
            {
                if (navData.navPurpose == NavPurpose.troop)
                {
                    //Ŀ������ƶ� ��������·��
                    navData.SetDestination();
                }
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
    /// ���Ѱ·
    /// </summary>
    /// <param name="_invoke"></param>
    public void ClearNavData(int _wuid, bool _invoke = false)
    {
        if (!navDict.ContainsKey(_wuid))
        {
            return;
        }
        NavData navData = navDict[_wuid];
        if (_invoke)
        {
            navData.selfUnit.OnNavArrive(navData);
        }
        navDict.Remove(_wuid);
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
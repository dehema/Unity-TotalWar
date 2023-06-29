using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopData
{
    /// <summary>
    /// ����ΨһID
    /// </summary>
    public int wuid;
    public TroopType troopType = TroopType.Army;
    public float posX;
    public float posY;
    public Dictionary<int, int> units = new Dictionary<int, int>();
    /// <summary>
    /// ��������ID
    /// </summary>
    public int cityID;
    /// <summary>
    /// Ŀ������ID
    /// </summary>
    public int targetWUID;
    public TroopState troopState = TroopState.wait;
    /// <summary>
    /// Я�����
    /// </summary>
    public int initGold = 100;
    /// <summary>
    /// Я�����
    /// </summary>
    public int gold = 100;

    public TroopData(TroopType _troopType)
    {
        troopType = _troopType;
    }
}


public enum TroopState
{
    wait,
    /// <summary>
    /// ��Ŀ���ƶ�
    /// </summary>
    moveTarget,
    /// <summary>
    /// �ִ�Ŀ��
    /// </summary>
    arriveTarget,
    /// <summary>
    /// �������ƶ�
    /// </summary>
    moveBackHome,
    /// <summary>
    /// �ִ�����
    /// </summary>
    arriveHome
}
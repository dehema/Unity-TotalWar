using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionData
{
    /// <summary>
    /// ���
    /// </summary>
    public int gold;
    /// <summary>
    /// ��������
    /// </summary>
    public List<int> citys = new List<int>();
    /// <summary>
    /// �������� <WUID, TroopData>
    /// </summary>
    public List<TroopData> troops = new List<TroopData>();
}

using System;
using System.Collections.Generic;

public class AllUnitConfig : ConfigBase
{
    public Dictionary<int, UnitConfig> unit = new Dictionary<int, UnitConfig>();

    public override void Init()
    {

    }
}

public class UnitConfig
{
    /// <summary>
    /// ID ��1λ������ID �ڶ�λ�Ǳ������� �����λ�Ƿ���
    /// </summary>
    public int ID;
    public UnitType type;
    public string name;
    /// <summary>
    /// ����ֵ
    /// </summary>
    public int hp;
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// ������
    /// </summary>
    public float attack;
    /// <summary>
    /// �������
    /// </summary>
    public float attackInterval;
    /// <summary>
    /// ������Χ
    /// </summary>
    public float attackRange;
    /// <summary>
    /// �������˺�,�Է��������
    /// </summary>
    public float attackHurtTime;
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public float attackDuration;
    /// <summary>
    /// ת���ٶ�
    /// </summary>
    public float angularSpeed;
    /// <summary>
    /// ģ�͸߶�
    /// </summary>
    public float height;
    /// <summary>
    /// ��ֵ/��ļ�۸�
    /// </summary>
    public int value;
    /// <summary>
    /// ÿ����ֵ����
    /// </summary>
    public int value_pre;
    /// <summary>
    /// ά����
    /// </summary>
    public int upkeep;
    public string fullID;
}
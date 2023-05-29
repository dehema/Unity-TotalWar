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
    /// ID 第1位是种族ID 第二位是兵种类型 最后两位是分类
    /// </summary>
    public int ID;
    public UnitType type;
    public string name;
    public int hp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public float attack;
    /// <summary>
    /// 攻击间隔
    /// </summary>
    public float attackInterval;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float attackRange;
    /// <summary>
    /// 转向速度
    /// </summary>
    public float angularSpeed;
    /// <summary>
    /// 模型高度
    /// </summary>
    public float height;
    /// <summary>
    /// 价值/招募价格
    /// </summary>
    public int value;
    /// <summary>
    /// 每级价值增长
    /// </summary>
    public int value_pre;
    /// <summary>
    /// 维护费
    /// </summary>
    public int upkeep;
    public string fullID;
}
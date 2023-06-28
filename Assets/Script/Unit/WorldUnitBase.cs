using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldUnitBase : MonoBehaviour
{
    /// <summary>
    /// 世界ID
    /// </summary>
    [HideInInspector]
    public int wuid = 0;
    protected WorldUnitBaseParams worldUnitBaseParams;
    /// <summary>
    /// 世界对象偏移值 （wuid叠加值）
    /// </summary>
    protected int wuidOffset = 0;
    /// <summary>
    /// 寻路
    /// </summary>
    protected NavMeshAgent nav;
    //图片 SpriteRenderer每100像素占用一个单位
    protected SpriteRenderer spriteRenderer;
    //世界单位配置
    WorldUnitConfig worldUnitConfig;
    //获取世界单位类型
    protected WorldUnitType worldUnitType = WorldUnitType.player;

    //Y轴偏移值
    protected float posYOffset = 0;
    //X轴旋转值
    protected float rotX = 0;
    //缩放值
    protected float unitScale = 1;

    public virtual void Init(params object[] _params)
    {
        worldUnitBaseParams = _params[0] as WorldUnitBaseParams;
        worldUnitType = worldUnitBaseParams.worldUnitType;
        worldUnitConfig = ConfigMgr.Ins.worldConfig.Unit[worldUnitType];
        SetWUID();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        RefrehUnitUI();
        GetPosYOffset();
    }

    /// <summary>
    /// 设置世界对象ID
    /// </summary>
    public virtual void SetWUID()
    {
        wuid = DataMgr.Ins.GetWUID(worldUnitType, wuidOffset);
        WorldMgr.Ins.worldUnitDict[wuid] = this;
    }

    /// <summary>
    /// 设置单元大小（比如单元为1，需要根据单元大小）
    /// </summary>
    public void RefrehUnitUI()
    {
        unitScale = 1 / (spriteRenderer.sprite.rect.width / 100) * unitSize;
        transform.localScale = new Vector3(unitScale, unitScale, unitScale);
        rotX = Utility.GetSetting<float>(SettingField.World.World_Unit_RotX);
        transform.localEulerAngles = new Vector3(rotX, 0, 0);
    }

    /// <summary>
    /// 获取单位大小
    /// </summary>
    /// <returns></returns>
    private float unitSize
    {
        get { return worldUnitConfig.size; }
    }

    /// <summary>
    /// 坐标在Y轴上的偏移量
    /// </summary>
    public void GetPosYOffset()
    {
        float offset = unitSize / 2;
        //角度转弧度
        offset *= Mathf.Cos(rotX * Mathf.Deg2Rad);
        posYOffset = offset;
    }

    /// <summary>
    /// 当寻路到达时
    /// </summary>
    public virtual void OnNavArrive(NavData _navData)
    {
        switch (_navData.navPurpose)
        {
            case NavPurpose.movePos:
                break;
            case NavPurpose.city:
                break;
            case NavPurpose.troop:
                break;
        }
    }

    /// <summary>
    /// 初始化寻路
    /// </summary>
    protected void InitNav()
    {
        nav = GetComponent<NavMeshAgent>();
        if (nav == null)
            return;
        nav.Warp(transform.position);
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.acceleration = 100000;
    }
}

public class WorldUnitBaseParams
{
    public WorldUnitType worldUnitType;

    public WorldUnitBaseParams(WorldUnitType _worldUnitType)
    {
        worldUnitType = _worldUnitType;
    }
}
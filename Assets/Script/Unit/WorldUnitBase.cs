using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldUnitBase : MonoBehaviour
{
    protected WorldUnitBaseParams worldUnitBaseParams;
    /// <summary>
    /// 寻路
    /// </summary>
    protected NavMeshAgent nav;
    //图片 SpriteRenderer每100像素占用一个单位
    protected SpriteRenderer spriteRenderer;
    //世界单位配置
    WorldUnitConfig worldUnitConfig;
    protected Transform icon;
    //显示图标
    //获取世界单位类型
    public WorldUnitType worldUnitType = WorldUnitType.player;

    //Y轴偏移值
    //protected float posYOffset = 0;
    //X轴旋转值
    protected float rotX = 0;
    //缩放值
    protected float unitScale = 1;
    //是否在移动
    bool isMoving = false;

    public virtual void Init(params object[] _params)
    {
        //data
        worldUnitBaseParams = _params[0] as WorldUnitBaseParams;
        worldUnitType = worldUnitBaseParams.worldUnitType;
        worldUnitConfig = ConfigMgr.Ins.worldConfig.Unit[worldUnitType];
        isMoving = false;
        //component
        icon = transform.Find("icon");
        nav = GetComponent<NavMeshAgent>();
        spriteRenderer = icon.GetComponent<SpriteRenderer>();
        //UI
        if (worldUnitType == WorldUnitType.troop)
        {
            int factionID = CommonMgr.Ins.GetCityFactionID((this as WorldTroop).troopData.cityID);
            FactionConfig factionConfig = ConfigMgr.Ins.GetFactionConfig(factionID);
            spriteRenderer.sprite = Resources.Load<Sprite>("UI/troopIcon/troopIcon_" + (int)factionConfig.raceID);
        }
        RefrehIconUI();
        RefrehNavIconUI();
    }

    private void Update()
    {
        if (nav != null)
        {
            isMoving = nav.velocity != Vector3.zero;
            MoveAniamtion();
        }
    }

    //方向值，控制来回旋转
    int rotDir = 1;
    //旋转的局部坐标z值
    float axisZ = 0;
    /// <summary>
    /// 移动动画
    /// </summary>
    private void MoveAniamtion()
    {
        if (!isMoving)
        {
            if (icon.localEulerAngles.z != 0)
                icon.localEulerAngles = new Vector3(icon.localEulerAngles.x, 0, 0);
            return;
        }
        axisZ += 200f * Time.deltaTime * rotDir;
        if (axisZ >= 20f)
            rotDir = -1;
        if (axisZ <= -20f)
            rotDir = 1;
        axisZ = ClampAngle(axisZ, -20f, 20f);
        Quaternion quaternion = Quaternion.Euler(icon.localEulerAngles.x, icon.localEulerAngles.y, axisZ);
        icon.localRotation = quaternion;
    }

    /// <summary>
    ///  角度区间
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void RefrehIconUI()
    {
        unitScale = 1 / (spriteRenderer.sprite.rect.width / 100) * unitSize;
        icon.localScale = new Vector3(unitScale, unitScale, unitScale);
        rotX = Utility.GetSetting<float>(SettingField.World.World_Unit_RotX);
        icon.localEulerAngles = new Vector3(rotX, 0, 0);
    }

    /// <summary>
    /// 处理
    /// </summary>
    void RefrehNavIconUI()
    {
        //icon偏移
        float offset = unitSize / 2;
        //角度转弧度
        offset *= Mathf.Cos(rotX * Mathf.Deg2Rad);
        if (nav == null)
        {
            //没有寻路系统 比如城镇
            icon.transform.localPosition = new Vector3(0, offset, 0);
            return;
        }
        float iconFactHeight = spriteRenderer.sprite.rect.height / 100;
        nav.height = iconFactHeight;
        transform.position = new Vector3(transform.position.x, iconFactHeight / 2, transform.position.z);
        icon.transform.localPosition = new Vector3(0, -transform.position.y + offset, 0);
        transform.position = new Vector3(transform.position.x, nav.height / 2, transform.position.z);
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
        if (nav == null)
            return;
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.acceleration = 100000;
        nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        bool isWarp = nav.Warp(transform.position);
        if (!isWarp)
        {
            Debug.LogError("寻路初始化错误");
        }
    }
    /// <summary>
    /// 世界ID
    /// </summary>
    public int wuid
    {
        get
        {
            switch (worldUnitType)
            {
                case WorldUnitType.city:
                    return (this as WorldCityItem).cityData.wuid;
                case WorldUnitType.troop:
                    return (this as WorldTroop).troopData.wuid;
                default:
                    return CommonMgr.Ins.GetWUID(worldUnitType);
            }
        }
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
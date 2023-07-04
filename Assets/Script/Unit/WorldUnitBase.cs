using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldUnitBase : MonoBehaviour
{
    protected WorldUnitBaseParams worldUnitBaseParams;
    /// <summary>
    /// Ѱ·
    /// </summary>
    protected NavMeshAgent nav;
    //ͼƬ SpriteRendererÿ100����ռ��һ����λ
    protected SpriteRenderer spriteRenderer;
    //���絥λ����
    WorldUnitConfig worldUnitConfig;
    protected Transform icon;
    //��ʾͼ��
    //��ȡ���絥λ����
    public WorldUnitType worldUnitType = WorldUnitType.player;

    //Y��ƫ��ֵ
    //protected float posYOffset = 0;
    //X����תֵ
    protected float rotX = 0;
    //����ֵ
    protected float unitScale = 1;
    //�Ƿ����ƶ�
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

    //����ֵ������������ת
    int rotDir = 1;
    //��ת�ľֲ�����zֵ
    float axisZ = 0;
    /// <summary>
    /// �ƶ�����
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
    ///  �Ƕ�����
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
    /// ����
    /// </summary>
    void RefrehNavIconUI()
    {
        //iconƫ��
        float offset = unitSize / 2;
        //�Ƕ�ת����
        offset *= Mathf.Cos(rotX * Mathf.Deg2Rad);
        if (nav == null)
        {
            //û��Ѱ·ϵͳ �������
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
    /// ��ȡ��λ��С
    /// </summary>
    /// <returns></returns>
    private float unitSize
    {
        get { return worldUnitConfig.size; }
    }

    /// <summary>
    /// ��Ѱ·����ʱ
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
    /// ��ʼ��Ѱ·
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
            Debug.LogError("Ѱ·��ʼ������");
        }
    }
    /// <summary>
    /// ����ID
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
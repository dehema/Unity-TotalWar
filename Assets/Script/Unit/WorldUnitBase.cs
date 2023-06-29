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
    //��ȡ���絥λ����
    public WorldUnitType worldUnitType = WorldUnitType.player;

    //Y��ƫ��ֵ
    protected float posYOffset = 0;
    //X����תֵ
    protected float rotX = 0;
    //����ֵ
    protected float unitScale = 1;

    public virtual void Init(params object[] _params)
    {
        worldUnitBaseParams = _params[0] as WorldUnitBaseParams;
        worldUnitType = worldUnitBaseParams.worldUnitType;
        worldUnitConfig = ConfigMgr.Ins.worldConfig.Unit[worldUnitType];
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        RefrehUnitUI();
        GetPosYOffset();
    }

    /// <summary>
    /// ���õ�Ԫ��С�����絥ԪΪ1����Ҫ���ݵ�Ԫ��С��
    /// </summary>
    public void RefrehUnitUI()
    {
        unitScale = 1 / (spriteRenderer.sprite.rect.width / 100) * unitSize;
        transform.localScale = new Vector3(unitScale, unitScale, unitScale);
        rotX = Utility.GetSetting<float>(SettingField.World.World_Unit_RotX);
        transform.localEulerAngles = new Vector3(rotX, 0, 0);
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
    /// ������Y���ϵ�ƫ����
    /// </summary>
    public void GetPosYOffset()
    {
        float offset = unitSize / 2;
        //�Ƕ�ת����
        offset *= Mathf.Cos(rotX * Mathf.Deg2Rad);
        posYOffset = offset;
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
        nav = GetComponent<NavMeshAgent>();
        if (nav == null)
            return;
        nav.Warp(transform.position);
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.acceleration = 100000;
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
using System.Collections.Generic;
using UnityEngine;

public class WorldMgr : MonoBehaviour
{
    public static WorldMgr Ins;

    bool inited = false;

    [Header("�������")]
    public WorldCamera worldCamera;

    //field
    [HideInInspector]
    //���ͼ���
    public WorldPlayer worldPlayer;
    [HideInInspector]
    public int UnitPixelSize { get { return Utility.GetSetting<int>(SettingField.World.World_UnitPixelSize); } }

    //data
    [HideInInspector]
    //��ͼ�ߴ�
    public Vector2 worldSize = Vector2.zero;
    //����ʱ��
    public WorldDate worldDate = new WorldDate();
    private Dictionary<int, WorldUnitBase> worldUnitDict = new Dictionary<int, WorldUnitBase>();


    private void Awake()
    {
        Ins = this;
        if (!inited)
        {
            InitWorld();
            worldCamera.Init();
            InitPlayer();
            InitTroops();
            inited = true;
        }
        UIMgr.Ins.OpenView<TopView>();
        CityMgr.Ins.OnFirstEnterWorld();
        StartTrade();
    }

    private void FixedUpdate()
    {
        worldDate.UpdateWroldTime();
    }

    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="_mousePos"></param>
    public void OnClick(Vector3 _mousePos)
    {
        Ray ray = worldCamera.camera.ScreenPointToRay(_mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            WorldCityItem worldCityItem = hit.collider.GetComponent<WorldCityItem>();
            if (worldCityItem != null)
            {
                worldCityItem.OnClick();
                return;
            }
            WorldTroop worldTroop = hit.collider.GetComponent<WorldTroop>();
            if (worldTroop != null)
            {
                worldTroop.OnClick();
                return;
            }
            WorldGround worldGround = hit.collider.GetComponent<WorldGround>();
            if (worldGround != null)
            {
                worldGround.OnClick(hit.point);
                return;
            }
        }
    }

    private void InitWorld()
    {
        worldSize = Utility.GetSetting_Vector2(SettingField.World.World_Size);
        InitCity();
    }

    /// <summary>
    /// ��ʼ�����г���
    /// </summary>
    public void InitCity()
    {
        foreach (var config in CityMgr.Ins.allCityConfig.city)
        {
            int cityID = config.Key;
            WorldCityItem city = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_city), transform).GetComponent<WorldCityItem>();
            city.transform.SetParent(transform.Find(WorldUnitType.city.ToString()));
            city.Init(new WorldUnitBaseParams(WorldUnitType.city), config.Value);
            CityMgr.Ins.allCityItem.Add(cityID, city);
            worldUnitDict.Add(CommonMgr.Ins.GetCityWUID(cityID), city);
        }
    }

    /// <summary>
    /// ��ʼ�����
    /// </summary>
    private void InitPlayer()
    {
        worldPlayer = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_player), transform).GetComponent<WorldPlayer>();
        worldPlayer.transform.position = Vector3.zero;
        worldPlayer.transform.SetParent(transform.Find(WorldUnitType.player.ToString()));
        worldPlayer.Init(new WorldUnitBaseParams(WorldUnitType.player));
        worldCamera.SetLookAtTarget(worldPlayer.gameObject);
        worldUnitDict[worldPlayer.wuid] = worldPlayer;
    }

    /// <summary>
    /// �����������ID��ȡ����
    /// </summary>
    /// <param name="_wuid"></param>
    /// <returns></returns>
    public WorldUnitBase GetUnitByWUID(int _wuid)
    {
        if (worldUnitDict.ContainsKey(_wuid))
        {
            return worldUnitDict[_wuid];
        }
        return null;
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void InitTroops()
    {
        //�������в���
        foreach (var faction in DataMgr.Ins.gameData.factions)
        {
            foreach (var troop in faction.Value.troops)
            {
                if (troop.troopType == TroopType.Player)
                {
                    continue;
                }
                Vector3 pos = new Vector3(troop.posX, 0, troop.posY);
                Transform parent = transform.Find(WorldUnitType.troop.ToString());
                WorldTroop WorldTroop = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_troop), pos, Quaternion.identity, parent).GetComponent<WorldTroop>();
                WorldTroop.troopData = troop;
                WorldTroop.Init(new WorldUnitBaseParams(WorldUnitType.troop));
                worldUnitDict[WorldTroop.wuid] = WorldTroop;
            }
        }
    }

    /// <summary>
    /// ��ʼ����
    /// </summary>
    public void StartTrade()
    {
        if (SceneMgr.Ins.IsWorld)
        {
            foreach (var troop in worldUnitDict)
            {
                WorldUnitBase worldUnitBase = troop.Value;
                if (worldUnitBase.worldUnitType == WorldUnitType.troop)
                {
                    (worldUnitBase as WorldTroop).StateAction();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMgr : MonoBehaviour
{
    public static WorldMgr Ins;

    [Header("世界相机")]
    public WorldCamera worldCamera;

    //field
    [HideInInspector]
    //大地图玩家
    public WorldPlayer worldPlayer;
    [HideInInspector]
    public int UnitPixelSize { get { return Utility.GetSetting<int>(SettingField.World.World_UnitPixelSize); } }

    //data
    [HideInInspector]
    //地图尺寸
    public Vector2 worldSize = Vector2.zero;
    //世界时间
    public WorldDate worldDate = new WorldDate();
    //世界单位 <wuid,WorldUnitBase>
    private Dictionary<int, WorldUnitBase> worldUnitDict = new Dictionary<int, WorldUnitBase>();
    //全部城市
    public Dictionary<int, WorldCityItem> allCityItem = new Dictionary<int, WorldCityItem>();


    private void Awake()
    {
        Ins = this;
        InitWorld();
        worldCamera.Init();
        InitPlayer();
        InitTroops();
        OnFirstEnterWorld();
        UIMgr.Ins.OpenView<TopView>();
        StartTrade();
    }

    private void FixedUpdate()
    {
        worldDate.UpdateWroldTime();
    }
    /// <summary>
    /// 第一次进入世界
    /// </summary>
    public void OnFirstEnterWorld()
    {
        worldDate.onNewDay += () =>
        {
            //刷兵();
            foreach (var item in WorldMgr.Ins.allCityItem)
            {
                item.Value.cityData.RefreshRecruitUnit();
            }
            //发薪水

            //每日收入

            //重新生成商队
            RefreshCreateTrade();
        };
        worldDate.onNewHour += () =>
        {
            CityMgr.Ins.AddInBuildingProgress();
        };
    }

    /// <summary>
    /// 重新生成商队
    /// </summary>
    public void RefreshCreateTrade()
    {
        foreach (var cityData in DataMgr.Ins.gameData.cityData.Values)
        {
            cityData.RefreshTradeTroop();
        }
        InitTroops();
    }

    /// <summary>
    /// 点击事件
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
    /// 初始化所有城镇
    /// </summary>
    public void InitCity()
    {
        foreach (var config in CityMgr.Ins.allCityConfig.city)
        {
            int cityID = config.Key;
            WorldCityItem city = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_city), transform).GetComponent<WorldCityItem>();
            city.transform.SetParent(transform.Find(WorldUnitType.city.ToString()));
            city.Init(new WorldUnitBaseParams(WorldUnitType.city), config.Value);
            allCityItem.Add(cityID, city);
            worldUnitDict.Add(CommonMgr.Ins.GetCityWUID(cityID), city);
        }
    }

    /// <summary>
    /// 初始化玩家
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
    /// 根据世界对象ID获取对象
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
    /// 初始化部队
    /// </summary>
    private void InitTroops()
    {
        Action<TroopData> action = (troopData) =>
        {
            Vector3 pos = new Vector3(troopData.posX, 0, troopData.posY);
            Transform parent = transform.Find(WorldUnitType.troop.ToString());
            WorldTroop WorldTroop = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_troop), pos, Quaternion.identity, parent).GetComponent<WorldTroop>();
            WorldTroop.troopData = troopData;
            WorldTroop.Init(new WorldUnitBaseParams(WorldUnitType.troop));
            worldUnitDict[WorldTroop.wuid] = WorldTroop;
        };
        //生成所有部队
        foreach (var faction in DataMgr.Ins.gameData.factions)
        {
            foreach (var troops in faction.Value.tradeTroops)
            {
                foreach (var troopData in troops.Value)
                {
                    if (troopData.troopType == TroopType.Player)
                        continue;
                    if (worldUnitDict.ContainsKey(troopData.wuid))
                        continue;
                    action(troopData);
                }
            }
        }
    }

    /// <summary>
    /// 开始跑商
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YamlDotNet.Core.Tokens;

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
    public Dictionary<int, WorldUnitBase> worldUnitDict = new Dictionary<int, WorldUnitBase>();


    private void Awake()
    {
        Ins = this;
        InitWorld();
        worldCamera.Init();
        InitPlayer();
        InitTroops();
        UIMgr.Ins.OpenView<TopView>();
        CityMgr.Ins.OnFirstEnterWorld();
    }

    private void FixedUpdate()
    {
        worldDate.UpdateWroldTime();
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
            WorldCityItem city = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_city), transform).GetComponent<WorldCityItem>();
            city.transform.SetParent(transform.Find(WorldUnitType.city.ToString()));
            city.Init(new WorldUnitBaseParams(WorldUnitType.city), config.Value);
            CityMgr.Ins.allCityItem.Add(config.Key, city);
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
    }

    /// <summary>
    /// 获取世界对象ID
    /// </summary>
    /// <param name="_worldUnitType"></param>
    /// <param name="_offest"></param>
    /// <returns></returns>
    public int GetWUID(WorldUnitType _worldUnitType, int _offest = 0)
    {
        if (_offest == 0)
        {
            if (_worldUnitType == WorldUnitType.troop)
            {
                _offest = DataMgr.Ins.gameData.troops.Count;
            }
        }
        return (int)_worldUnitType * 1000 + _offest;
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
        //测试部队
        InitTestTroop();
        foreach (var item in DataMgr.Ins.gameData.troops)
        {
            var WorldTroop = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_troop), transform).GetComponent<WorldTroop>();
            WorldTroop.transform.SetParent(transform.Find(WorldUnitType.troop.ToString()));
            WorldTroop.transform.position = new Vector3(item.Value.posX, 0, item.Value.posY);
            WorldTroop.Init(new WorldUnitBaseParams(WorldUnitType.troop));
        }
    }

    /// <summary>
    /// 初始化测试部队
    /// </summary>
    private void InitTestTroop()
    {
        TroopData troopData = new TroopData();
        troopData.wuid = GetWUID(WorldUnitType.troop);
        troopData.posX = 10;
        troopData.posY = 0;
        troopData.units = new Dictionary<int, int> { { 1101, 5 } };
        DataMgr.Ins.gameData.troops.Add(troopData.wuid, troopData);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YamlDotNet.Core.Tokens;

public class WorldMgr : MonoBehaviour
{
    public static WorldMgr Ins;

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
            WorldCityItem city = Instantiate(Resources.Load<GameObject>(PrefabPath.prefab_wrold_city), transform).GetComponent<WorldCityItem>();
            city.transform.SetParent(transform.Find(WorldUnitType.city.ToString()));
            city.Init(new WorldUnitBaseParams(WorldUnitType.city), config.Value);
            CityMgr.Ins.allCityItem.Add(config.Key, city);
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
    }

    /// <summary>
    /// ��ȡ�������ID
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
        //���Բ���
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
    /// ��ʼ�����Բ���
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

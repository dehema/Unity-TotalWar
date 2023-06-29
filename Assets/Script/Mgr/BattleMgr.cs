using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoSingleton<BattleMgr>
{
    GameObject player;
    PlayerController playerController;
    public List<UnitBase> myArmyList = new List<UnitBase>();
    public List<UnitBase> enemyList = new List<UnitBase>();
    public Dictionary<UnitType, Dictionary<int, int>> playerDataQueue = new Dictionary<UnitType, Dictionary<int, int>>();
    public Dictionary<UnitType, Dictionary<int, int>> enemyDataQueue = new Dictionary<UnitType, Dictionary<int, int>>();
    //创建单位的索引
    int createUnitIndex = 0;
    //战斗是否开始
    public bool isBattleStart = false;
    public BattleState battleState;
    //弓箭对象池
    public ObjPool arrowPool;

    /// <summary>
    /// 双方单位数量上限
    /// </summary>
    private int UnitNumLimitOnField { get { return 3; } }


    public void Init(TroopData _enemyTroop = null)
    {
        ResetData();
        SetBattleState(BattleState.Init);
        _enemyTroop = DebugInitUnits();
        InitArmyQueueData(ArmyType.player);
        InitArmyQueueData(ArmyType.enemy, _enemyTroop);
        Timer.Ins.SetTimeOut(BattleInit, 0.5f);
        InitPool();
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    void InitPool()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefab/Weapon/Arrow");
        GameObject goArrow = Instantiate(prefab, transform);
        arrowPool = PoolMgr.Ins.CreatePool(goArrow);
    }

    /// <summary>
    /// 战斗开始
    /// </summary>
    private void BattleInit()
    {
        UIMgr.Ins.OpenView<HUDView>();
        isBattleStart = false;
        createUnitIndex = 0;
        CreatePlayer();
        CreateBatchArmyUnit(ArmyType.player);
        CreateBatchArmyUnit(ArmyType.enemy);
        Timer.Ins.SetTimeOut(BattleStart, 0.5f);
    }

    void ResetData()
    {
        myArmyList.Clear();
        enemyList.Clear();
        playerDataQueue.Clear();
        enemyDataQueue.Clear();
    }

    /// <summary>
    /// 战斗开始
    /// </summary>
    void BattleStart()
    {
        SetBattleState(BattleState.Fighting);
        isBattleStart = true;
        myArmyList.Add(playerController);
        foreach (var item in myArmyList)
        {
            UnitBaseBattleParams battleParams = new UnitBaseBattleParams(ArmyType.player);
            item.StartBattle(battleParams);
        }
        foreach (var item in enemyList)
        {
            UnitBaseBattleParams battleParams = new UnitBaseBattleParams(ArmyType.enemy);
            item.StartBattle(battleParams);
        }
    }

    /// <summary>
    /// 调试初始化初始单位
    /// </summary>
    /// <returns></returns>
    private TroopData DebugInitUnits()
    {
        int _unitNum = 5;
        //我军
        for (int i = 0; i < _unitNum; i++)
        {
            PlayerMgr.Ins.AddPlayerUnit(1101);
        }
        //敌军
        TroopData troopData = new TroopData(TroopType.Army);
        //troopData.units.Add(2101, _unitNum);
        troopData.units.Add(1201, _unitNum);
        return troopData;
    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    private void CreatePlayer()
    {
        player = Instantiate(Resources.Load<GameObject>(PrefabPath.BattleField_player), BattleSceneMgr.Ins.myArmyStartPoint);
        player.transform.eulerAngles = BattleSceneMgr.Ins.myArmyStartPoint.eulerAngles;
        player.transform.position = BattleSceneMgr.Ins.myArmyStartPoint.position + BattleSceneMgr.Ins.myArmyStartPoint.forward * BattleSceneMgr.birthSize;
        player.transform.SetParent(BattleSceneMgr.Ins.myArmyList);
        playerController = player.gameObject.GetComponent<PlayerController>();
        UnitBaseBattleParams battleParams = new UnitBaseBattleParams(ArmyType.player);
        playerController.Init(battleParams);
    }

    /// <summary>
    /// 根据类型获取军队
    /// </summary>
    /// <param name="_armyType"></param>
    /// <returns></returns>
    private Dictionary<UnitType, Dictionary<int, int>> GetArmyByType(ArmyType _armyType)
    {
        switch (_armyType)
        {
            case ArmyType.enemy:
                return enemyDataQueue;
            case ArmyType.player:
            default:
                return playerDataQueue;
        }
    }

    /// <summary>
    /// 初始化军队 
    /// </summary>
    /// <param name="_troopData"></param>
    /// <param name="_armyType"></param>
    private void InitArmyQueueData(ArmyType _armyType, TroopData _troopData = null)
    {
        var armyQueue = GetArmyByType(_armyType);
        Action<int, int> AddToQueue = (int unitID, int unitNum) =>
        {
            UnitConfig unitConfig = ConfigMgr.Ins.GetUnitConfig(unitID);
            if (!armyQueue.ContainsKey(unitConfig.type))
                armyQueue.Add(unitConfig.type, new Dictionary<int, int>());
            if (!armyQueue[unitConfig.type].ContainsKey(unitID))
                armyQueue[unitConfig.type].Add(unitID, unitNum);
            else
                armyQueue[unitConfig.type][unitID] += unitNum;
        };
        if (_armyType == ArmyType.player)
        {
            foreach (var item in PlayerMgr.Ins.GetPlayerTroop().units)
            {
                AddToQueue(item.Key, item.Value);
            }
        }
        else if (_armyType == ArmyType.enemy)
        {
            foreach (var item in _troopData.units)
            {
                AddToQueue(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// 创建军队士兵
    /// </summary>
    private void CreateBatchArmyUnit(ArmyType _armyType)
    {
        ///士兵总数
        int createNum = GetBatchCreateSoliderNum(_armyType);
        var army = GetArmyByType(_armyType);
        Transform unitParent = GetUnitCreateParent(_armyType);
        Transform startPoint = GetUnitCreateStartPoint(_armyType);
        List<UnitBase> unitBaseList = GetUnitList(_armyType);
        //开始生成
        Dictionary<int, int> createUnits = GetCreateUnitTypeAndNum(army, createNum);
        foreach (var item in createUnits)
        {
            UnitConfig unitConfig = ConfigMgr.Ins.GetUnitConfig(item.Key);
            for (int i = 0; i < item.Value; i++)
            {
                var unitModel = Instantiate(Resources.Load<GameObject>(PrefabPath.unit + unitConfig.fullID));
                unitModel.transform.SetParent(unitParent);
                //士兵出生位置 横向偏移
                float offset = createNum > 1 ? BattleSceneMgr.birthSize * 2 / (createNum - 1) : 0;
                unitModel.transform.position = startPoint.position + startPoint.right * offset * i;
                unitModel.transform.eulerAngles = startPoint.eulerAngles;
                UnitBase unitBase = unitModel.GetComponent<UnitBase>();
                unitBase.Init(unitConfig);
                unitBase.SetCreateUnitIndex(createUnitIndex);
                unitBaseList.Add(unitBase);
                unitBase.gameObject.name = _armyType.ToString() + "_" + createUnitIndex;
                if (isBattleStart)
                {
                    BattleStart();
                }
                createUnitIndex++;
            }
        }
    }

    /// <summary>
    /// 获取单位生成的父物体
    /// </summary>
    private Transform GetUnitCreateParent(ArmyType _armyType)
    {
        switch (_armyType)
        {
            case ArmyType.player:
                return BattleSceneMgr.Ins.myArmyList.transform;
            case ArmyType.enemy:
                return BattleSceneMgr.Ins.enemyList.transform;
            default:
                return BattleSceneMgr.Ins.transform;
        }
    }

    /// <summary>
    /// 获取单位的生成点
    /// </summary>
    /// <param name="_armyType"></param>
    /// <returns></returns>
    private Transform GetUnitCreateStartPoint(ArmyType _armyType)
    {
        switch (_armyType)
        {
            case ArmyType.player:
            default:
                return BattleSceneMgr.Ins.myArmyStartPoint;
            case ArmyType.enemy:
                return BattleSceneMgr.Ins.enemyStartPoint;
        }
    }

    /// <summary>
    /// 获取军队单位数量
    /// </summary>
    private int GetArmyUnitNum(Dictionary<UnitType, Dictionary<int, int>> _army)
    {
        int num = 0;
        foreach (var item in _army)
        {
            foreach (var unit in item.Value)
            {
                num += unit.Value;
            }
        }
        return num;
    }

    /// <summary>
    /// 生成军队数量 <兵种ID，数量>
    /// </summary>
    /// <param name="_army"></param>
    /// <param name="_totalNum"></param>
    private Dictionary<int, int> GetCreateUnitTypeAndNum(Dictionary<UnitType, Dictionary<int, int>> _army, int _totalNum)
    {
        //军队总数
        int num = GetArmyUnitNum(_army);
        Dictionary<UnitType, int> units = new Dictionary<UnitType, int>();
        //每一种兵种和他的总数
        foreach (var item in _army)
        {
            int _num = 0;
            foreach (var unit in item.Value)
            {
                _num += unit.Value;
            }
            units[item.Key] = units.ContainsKey(item.Key) ? units[item.Key] + _num : _num;
        }
        float _f = num / (_totalNum * 1.0f);
        //求出每一种兵种的份额
        Dictionary<UnitType, int> numEveryUnits = new Dictionary<UnitType, int>();
        //每个兵种有几个士兵
        foreach (var item in units)
        {
            numEveryUnits[item.Key] = Mathf.RoundToInt(item.Value * _f);
        }
        Dictionary<int, int> createUnitIDNum = new Dictionary<int, int>();
        foreach (var createUnit in numEveryUnits)
        {
            //当前兵种招募数量
            int _num = createUnit.Value;
            foreach (var _unit in _army[createUnit.Key])
            {
                if (_totalNum == 0)
                    break;
                if (_unit.Value == 0)
                    continue;
                int removeNum = Mathf.Min(_num, _unit.Value, _totalNum);
                createUnitIDNum.Add(_unit.Key, removeNum);
                _num -= removeNum;
                _totalNum -= removeNum;
                //清空队列数据
                _army[createUnit.Key][_unit.Key] -= removeNum;
                if (_army[createUnit.Key][_unit.Key] == 0)
                {
                    _army[createUnit.Key].Remove(_unit.Key);
                    if (_army[createUnit.Key].Count == 0)
                    {
                        _army.Remove(createUnit.Key);
                    }
                }
                if (_totalNum == 0)
                {
                    return createUnitIDNum;
                }
                if (_num == 0)
                {
                    //该兵种招募数量凑齐
                    break;
                }
            }
        }
        return createUnitIDNum;
    }

    /// <summary>
    /// 获取创建士兵的单位数量
    /// </summary>
    /// <returns></returns>
    private int GetBatchCreateSoliderNum(ArmyType _armyType)
    {
        int unitNum = 0;
        if (_armyType == ArmyType.player)
        {
            unitNum = myArmyList.Count;
        }
        else if (_armyType == ArmyType.enemy)
        {
            unitNum = enemyList.Count;
        }
        return UnitNumLimitOnField - unitNum;
    }

    /// <summary>
    /// 获取军队士兵队列
    /// </summary>
    /// <param name="_armyType"></param>
    /// <returns></returns>
    public List<UnitBase> GetUnitList(ArmyType armyType)
    {
        switch (armyType)
        {
            case ArmyType.player:
            default:
                return myArmyList;
            case ArmyType.enemy:
                return enemyList;
        }
    }

    public ArmyType GetEnemyType(ArmyType _armyType)
    {
        switch (_armyType)
        {
            case ArmyType.player:
            default:
                return ArmyType.enemy;
            case ArmyType.enemy:
                return ArmyType.player;
        }
    }

    /// <summary>
    /// 找到距离改单位最近的敌人
    /// </summary>
    /// <param name="_unitBase"></param>
    /// <returns></returns>
    public UnitBase FindANearestEnemy(UnitBase _unitBase)
    {
        ArmyType enemyArmyType = GetEnemyType(_unitBase.battleParams.armyType);
        List<UnitBase> enemyUnitList = GetUnitList(enemyArmyType);
        if (enemyUnitList.Count == 0)
        {
            return null;
        }
        UnitBase nearestEnemy = null;
        float distance = 10000;
        foreach (var item in enemyUnitList)
        {
            float _distance = Vector3.Distance(_unitBase.transform.position, item.transform.position);
            if (nearestEnemy == null || _distance < distance)
            {
                distance = _distance;
                nearestEnemy = item;
            }
        }
        return nearestEnemy;
    }

    /// <summary>
    /// 单位死亡
    /// </summary>
    /// <param name="_unitBase"></param>
    public void UnitDead(UnitBase _unitBase)
    {
        ArmyType armyType = _unitBase.unitData.armyType;
        List<UnitBase> armyList = GetUnitList(armyType);
        armyList.Remove(_unitBase);
        if (battleState != BattleState.Finish)
        {
            if (CheckArmyQueueUnitNum(armyType) <= 0 && armyList.Count <= 0)
            {
                SetBattleState(BattleState.Finish);
                //没有预备士兵可以创建
                //战斗结束
                if (armyType == ArmyType.player)
                {
                    BattleDefeat();
                }
                else
                {
                    BattleVictory();
                }
                UIMgr.Ins.CloseView<HUDView>();
                return;
            }
        }
        CreateBatchArmyUnit(armyType);
    }

    /// <summary>
    /// 战斗胜利
    /// </summary>
    void BattleVictory()
    {
        Debug.Log("战斗胜利");
        //animator
        foreach (var item in myArmyList)
        {
            item.SetBattleState(UnitBattleState.Win);
        }
        //UI
        Timer.Ins.SetTimeOut(() =>
        {
            ExitBattleScene();
            BattleVictoryViewParams viewParams = new BattleVictoryViewParams();
            viewParams.closeCB = () => { SceneMgr.Ins.ChangeScene(SceneID.WorldMap); };
            UIMgr.Ins.OpenView<BattleVictoryView>(viewParams);
        }, 2);
    }

    /// <summary>
    /// 战斗失败
    /// </summary>
    void BattleDefeat()
    {
        Debug.Log("战斗失败");
        //animator
        foreach (var item in enemyList)
        {
            item.SetBattleState(UnitBattleState.Win);
        }
        //UI
        Timer.Ins.SetTimeOut(() =>
        {
            ExitBattleScene();
            BattleDefeatViewParams viewParams = new BattleDefeatViewParams();
            viewParams.closeCB = () => { SceneMgr.Ins.ChangeScene(SceneID.WorldMap); };
            UIMgr.Ins.OpenView<BattleDefeatView>(viewParams);
        }, 2);
    }

    /// <summary>
    /// 关闭战斗场景
    /// </summary>
    void ExitBattleScene()
    {
        arrowPool.CollectAll();
    }

    /// <summary>
    /// 获取阵营的预备单位数量
    /// </summary>
    /// <returns></returns>
    private int CheckArmyQueueUnitNum(ArmyType _armyType)
    {
        int num = 0;
        Dictionary<UnitType, Dictionary<int, int>> army = GetArmyByType(_armyType);
        foreach (var item in army)
        {
            Dictionary<int, int> dict = item.Value;
            foreach (var units in dict)
            {
                num += units.Value;
            }
        }
        return num;
    }

    /// <summary>
    /// 战斗状态机
    /// </summary>
    public void SetBattleState(BattleState _battleState)
    {
        battleState = _battleState;
    }

    /// <summary>
    /// 战斗结束
    /// </summary>
    public bool IsBattleFinish { get { return BattleMgr.Ins.battleState == BattleState.Finish; } }

    const int maxArrowNum = 5;
    /// <summary>
    /// 从对象池中获取弓箭
    /// </summary>
    /// <param name="_parms"></param>
    /// <returns></returns>
    public GameObject GetArrow(params object[] _parms)
    {
        if (arrowPool.activePool.Count > maxArrowNum)
        {
            arrowPool.CollectOne(arrowPool.activePool[0]);
        }
        return arrowPool.Get(_parms);
    }

    /// <summary>
    /// 是否锁定玩家相机
    /// </summary>
    public void SetLockPlayerCamera(bool _lock)
    {
        playerController.thirdPersonController.LockCameraPosition = _lock;
    }
}

/// <summary>
/// 军队类型
/// </summary>
public enum ArmyType
{
    player,
    enemy
}
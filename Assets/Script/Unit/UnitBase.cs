using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnitBase : MonoBehaviour, IUnitBase
{
    public UnitConfig unitConfig;
    public UnitData unitData;
    public Animator animator;
    // 状态条位置
    [HideInInspector]
    public Transform startBarPos;
    protected NavMeshAgent nav;
    public Rigidbody rigidbody;
    //战斗参数
    public UnitBaseBattleParams battleParams;
    UnitBattleState unitBattleState = UnitBattleState.Idle;
    //攻击目标
    public UnitBase attackTarget;
    //移动时调用的委托
    public Action OnPosChangeDele;
    //单位死亡时委托
    public Action OnDeadDele;
    //碰撞体
    Collider collider;
    Vector3 lastPos;
    public bool IsPlayer { get { return unitConfig.type == UnitType.Player; } }
    //是否能攻击 攻击有攻击CD
    public bool canAttack = true;

    void Awake()
    {
        startBarPos = transform.Find("startBarPos");
    }

    bool isInitComponent = false;
    void InitComponent()
    {
        if (isInitComponent)
            return;
        isInitComponent = true;
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        if (nav != null)
        {
            nav.angularSpeed = unitConfig.angularSpeed;
        }
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public virtual void Init(int _unitID)
    {
        Init(ConfigMgr.Ins.GetUnitConfig(_unitID));
    }

    public virtual void Init(UnitData _unitData)
    {
        unitData = _unitData;
        Init(ConfigMgr.Ins.GetUnitConfig(unitData.unitConfig.ID));
    }

    public virtual void Init(UnitConfig _unitConfig)
    {
        unitConfig = _unitConfig;
        if (unitData == null)
        {
            unitData = new UnitData(_unitConfig);
        }
        InitComponent();
    }

    public virtual float UnitHp
    {
        get { return unitData.hp; }
        set { unitData.hp = value; }
    }

    public virtual float UnitHpMax
    {
        get { return unitData.hpMax; }
        set { unitData.hpMax = value; }
    }

    public virtual float UnitAttack
    {
        get { return unitData.attack; }
        set { unitData.attack = value; }
    }

    /// <summary>
    /// 设置单位索引
    /// </summary>
    /// <param name="_index"></param>
    public void SetCreateUnitIndex(int _index)
    {
        string index = unitData.armyType.ToString() + _index;
        unitData.unitCreateIndex = index;
    }

    /// <summary>
    /// 攻击单位
    /// </summary>
    /// <param name="_defender">防御者</param>
    public virtual void AttackUnit(UnitBase _defender)
    {
        if (!canAttack)
        {
            return;
        }
        if (IsDead)
        {
            return;
        }
        Debug.Log($"{LangMgr.Ins.Get(unitConfig.name)}攻击了{LangMgr.Ins.Get(_defender.unitConfig.name)}");
        _defender.TakeDamage(this);
        if (!IsPlayer)
        {
            SetBattleState(UnitBattleState.FindEnemy);
        }
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="_attacker"></param>
    public virtual void TakeDamage(UnitBase _attacker)
    {
        float _attack = _attacker.unitData.attack;
        if (_attacker.IsPlayer && !IsPlayer)
        {
            //当受到来自玩家的伤害时 显示状态条
            ShowStateBar(_attack, Utility.GetSetting<float>(SettingField.UI.HPSliderDisplay));
        }
        if (!IsPlayer)
        {
            UnitHp -= _attack;
            Debug.Log($"{LangMgr.Ins.Get(unitConfig.name)}收到了{_attack}点伤害，剩余生命值ֵ{UnitHp}");
        }
        CheckDead();
        if (IsDead)
        {
            return;
        }
        if (!IsPlayer)
        {
            //不是玩家的单位受伤后 重新寻找攻击目标
            SetBattleState(UnitBattleState.FindEnemy);
        }
    }

    TimerHandler ShowStateBar_Timer;
    /// <summary>
    /// 显示状态条
    /// </summary>
    /// <param name="_duration"></param>
    public void ShowStateBar(float lostHp, float _duration = -1)
    {
        //重置显示时间
        if (ShowStateBar_Timer != null)
        {
            ShowStateBar_Timer.Remove();
            ShowStateBar_Timer = null;
        }
        HUDView hudView = UIMgr.Ins.GetView<HUDView>();
        if (hudView != null)
        {
            hudView.ShowStateBar(this, UnitHp - lostHp, UnitHp, UnitHpMax);
            if (_duration != -1)
            {
                ShowStateBar_Timer = Timer.Ins.SetTimeOut(HideStateBar, _duration, unitData.unitCreateIndex);
            }
        }
    }

    /// <summary>
    /// 隐藏状态条
    /// </summary>
    public void HideStateBar()
    {
        ShowStateBar_Timer?.Remove();
        HUDView hudView = UIMgr.Ins.GetView<HUDView>();
        if (hudView != null)
        {
            hudView.HideStateBar(this);
        }
    }

    /// <summary>
    /// 设置状态机
    /// </summary>
    public void SetAnimation(string _name)
    {
        animator.Play(_name, 0);
    }
    /// <summary>
    /// 开始战斗
    /// </summary> 
    public void StartBattle(UnitBaseBattleParams _battleParams)
    {
        if (battleParams != null)
            return;
        Timer.Ins.SetInterval(() => { SetAttackTarget(); }, 2, int.MaxValue, unitData.unitCreateIndex);
        battleParams = _battleParams;
        unitData.armyType = _battleParams.armyType;
        nav.Warp(transform.position);
        nav.enabled = true;
        if (!IsPlayer)
        {
            SetBattleState(UnitBattleState.StartBattle);
        }
    }

    public virtual void Update()
    {
        if (IsDead)
        {
            //单位死亡
            return;
        }
        if (BattleMgr.Ins.IsBattleFinish)
        {
            //战斗已经结束
            return;
        }
        if (transform.position != lastPos)
        {
            OnPosChangeDele?.Invoke();
            lastPos = transform.position;
        }
        if (unitBattleState == UnitBattleState.MoveToEnemy)
        {
            NavToAttackTarget();
            if (attackTarget == null)
            {
                SetBattleState(UnitBattleState.FindEnemy);
                return;
            }
            if (attackTarget != null && !nav.pathPending && Vector3.Distance(attackTarget.transform.position, transform.position) < MoveStopDistance)
            {
                nav.isStopped = true;
                SetAnimatorTrigger(AnimatorParams.idle);
                if (canAttack)
                {
                    SetBattleState(UnitBattleState.Attack);
                }
            }
        }
    }

    /// <summary>
    /// 寻路停止范围 自身半径+攻击距离+对方半径
    /// </summary>
    float MoveStopDistance
    {
        get
        {
            float attackRadius = 0.3f;
            if (attackTarget.nav)
            {
                attackRadius = attackTarget.nav.radius;
            }
            return nav.radius + attackRadius + unitConfig.attackRange;
        }
    }
    TimerHandler SetAttackTargetTimer;
    /// <summary>
    /// 设置战斗状态
    /// </summary>
    /// <param name="_unitBattleState"></param>
    public void SetBattleState(UnitBattleState _unitBattleState)
    {
        unitBattleState = _unitBattleState;
        if (unitBattleState == UnitBattleState.StartBattle)
        {
            SetBattleState(UnitBattleState.FindEnemy);
        }
        else if (unitBattleState == UnitBattleState.FindEnemy)
        {
            SetAttackTarget();
            SetBattleState(UnitBattleState.MoveToEnemy);
        }
        else if (unitBattleState == UnitBattleState.MoveToEnemy)
        {
            if (attackTarget != null)
            {
                if (Vector3.Distance(attackTarget.transform.position, transform.position) <= MoveStopDistance)
                {
                    //在攻击范围内
                    SetBattleState(UnitBattleState.Attack);
                    return;
                }
                else
                {
                    //不在攻击范围内
                }
            }
        }
        else if (unitBattleState == UnitBattleState.Attack)
        {
            transform.LookAt(attackTarget.transform.position);
            Timer.Ins.SetTimeOut(() => { SetBattleState(UnitBattleState.MoveToEnemy); }, unitData.arttckInterval, unitData.unitCreateIndex);
        }
        else if (unitBattleState == UnitBattleState.Dead)
        {
            Dead();
        }
        else if (unitBattleState == UnitBattleState.Win)
        {
            Timer.Ins.RemoveTimerGroup(unitData.unitCreateIndex);
            if (nav != null)
            {
                nav.isStopped = true;
            }
            if (!IsPlayer)
            {
                SetAnimatorTrigger(AnimatorParams.idle);
            }
        }
        RefreshAnimator();
    }

    /// <summary>
    /// 设置一个攻击对象
    /// </summary>
    void SetAttackTarget()
    {
        UnitBase enemy = BattleMgr.Ins.FindANearestEnemy(this);
        if (enemy != null)
        {
            attackTarget = enemy;
            nav.isStopped = false;
        }
    }

    void OnAttackTargetPosChange()
    {
    }

    /// <summary>
    /// 当攻击对象死亡时
    /// </summary>
    void OnAttackTargetDead()
    {
        if (attackTarget != null)
        {
        }
    }

    void NavToAttackTarget()
    {
        if (attackTarget != null)
        {
            nav.SetDestination(attackTarget.transform.position);
        }
    }

    /// <summary>
    /// 设置动画状态机触发器
    /// </summary>
    /// <param name="_params"></param>
    public void SetAnimatorTrigger(string _params)
    {
        string triggerName = _params;
        Debug.Log($"触发状态机->{triggerName}");
        animator.SetTrigger(triggerName);
    }

    /// <summary>
    /// 刷新状态机
    /// </summary>
    void RefreshAnimator()
    {
        string triggerName = "";
        switch (unitBattleState)
        {
            case UnitBattleState.Idle:
                triggerName = AnimatorParams.idle;
                break;
            case UnitBattleState.StartBattle:
                break;
            case UnitBattleState.MoveToEnemy:
                triggerName = AnimatorParams.run;
                break;
            case UnitBattleState.Attack:
                if (Random.Range(0, 100) < 50)
                    triggerName = AnimatorParams.attack_A;
                else
                    triggerName = AnimatorParams.attack_B;
                Timer.Ins.SetTimeOut(() => { AttackUnit(attackTarget); }, Random.Range(0.4f, 0.6f), unitData.unitCreateIndex);
                canAttack = false;
                SetCanAttackTrue();
                break;
            case UnitBattleState.Dead:

                if (Random.Range(0, 100) < 50)
                    triggerName = AnimatorParams.death_A;
                else
                    triggerName = AnimatorParams.death_B;
                break;
        }
        if (!string.IsNullOrEmpty(triggerName))
        {
            SetAnimatorTrigger(triggerName);
        }
    }

    void SetCanAttackTrue()
    {
        Timer.Ins.SetTimeOut(() => { canAttack = true; }, unitConfig.attackInterval, unitData.unitCreateIndex);
    }

    void CheckDead()
    {
        if (UnitHp <= 0)
        {
            BattleMgr.Ins.UnitDead(this);
            SetBattleState(UnitBattleState.Dead);
        }
    }

    /// <summary>
    /// 单位死亡
    /// </summary>
    void Dead()
    {
        OnDeadDele?.Invoke();
        HideStateBar();
        Timer.Ins.RemoveTimerGroup(unitData.unitCreateIndex);
        Release();
    }

    public void Release()
    {
        if (rigidbody != null)
        {
            Destroy(rigidbody);
        }
        if (collider != null)
        {
            Destroy(collider);
        }
        if (nav != null)
        {
            Destroy(nav);
        }
        Destroy(this);
    }

    /// <summary>
    /// 是否死亡
    /// </summary>
    /// <returns></returns>
    public bool IsDead { get { return this == null || unitData == null || UnitHp <= 0; } }
}

public class UnitBaseBattleParams
{
    public ArmyType armyType = ArmyType.player;

    public UnitBaseBattleParams(ArmyType _armyType)
    {
        armyType = _armyType;
    }
}

public enum UnitBattleState
{
    Idle,
    StartBattle,
    FindEnemy,
    MoveToEnemy,
    Attack,
    Dead,
    Win,
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : UnitBase
{
    Animator animator;
    CharacterController characterController;
    HitBoxCollider hitBoxCollider;
    PlayerInput playerInput;
    PlayerData playerData { get { return DataMgr.Ins.playerData; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        hitBoxCollider = transform.Find("hitBoxCollider").GetComponent<HitBoxCollider>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultControlScheme = "KeyboardMouse";
        hitBoxCollider.Init(this);
        //玩家数据
        unitData = new UnitData();
        unitData.attack = 30;
        UnitHpMax = 1000;
        UnitHp = 1000;
        unitData.unitCreateIndex = "PlayerSelf";
        unitConfig = new UnitConfig();
        unitConfig.type = UnitType.Player;
        unitConfig.name = "1667200201";

    }
    public override void TakeDamage(UnitBase _attacker)
    {
        UnitHp -= (int)_attacker.unitData.attack;
        base.TakeDamage(_attacker);
    }

    public void Init(UnitBaseBattleParams _battleParams)
    {
        battleParams = _battleParams;
        HideHideCollider();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack_Sword");
        }
    }

    public void Hit()
    {
        Debug.Log("触发hit");
        hitBoxCollider.StartTrigger();
    }

    private void ShowHitCollider()
    {
        hitBoxCollider.StartTrigger();
    }

    private void HideHideCollider()
    {
        hitBoxCollider.SetColliderEnable(false);
    }

    public override float UnitHp
    {
        get
        {
            return playerData.hp.Value;
        }
        set
        {
            playerData.hp.Value = value;
        }
    }

    public override float UnitHpMax
    {
        get
        {
            return playerData.hpMax.Value;
        }
        set { playerData.hpMax.Value = value; }
    }
}

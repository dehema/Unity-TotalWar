using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArrow : PoolItemBase3D
{
    Rigidbody rigi;
    new BoxCollider collider;
    //瞄准目标
    Transform shootTarget;
    //攻击者
    UnitBase attacker;
    const float moveSpeed = 20;
    const float angleSpeed = 20;
    //激活状态
    bool isEnable = false;

    private void Update()
    {
        if (isEnable)
        {
            transform.position += (transform.up * Time.deltaTime * moveSpeed);
            if (transform.eulerAngles.z < 180)
            {
                transform.eulerAngles += new Vector3(0, 0, angleSpeed) * Time.deltaTime;
            }
        }
    }

    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        if (rigi == null)
        {
            rigi = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();
        }
        Transform startPos = _params[0] as Transform;
        shootTarget = _params[1] as Transform;
        attacker = _params[2] as UnitBase;
        transform.SetParent(attacker.transform);
        transform.position = startPos.position;
        transform.eulerAngles = startPos.eulerAngles;
        //下面求弓箭发射角度
        //直线距离
        float distance = Vector3.Distance(transform.position, shootTarget.transform.position);
        //飞行时间
        float flyTime = distance / moveSpeed;
        //起始角度
        float startAngle = angleSpeed * (flyTime / 2);
        //高度差
        float height = Mathf.Abs(transform.position.y - shootTarget.transform.position.y);
        float heightAngle = height * flyTime * angleSpeed / 10;
        transform.eulerAngles = startPos.eulerAngles - new Vector3(0, 0, startAngle + heightAngle);
        //增加一个随机角度
        transform.eulerAngles += new Vector3(0, Random.Range(-0.5f, 0.5f), Random.Range(-1, 1));
        isEnable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //攻击者本身
        if (other.gameObject == attacker.gameObject)
        {
            return;
        }
        //地形
        if (other.tag == TagType.Env.ToString())
        {
            StopAndSeParent(other.transform);
        }
        //玩家
        else if (other.tag == TagType.Player.ToString())
        {
            StopAndSeParent(other.transform);
            if (attacker.unitData.armyType == ArmyType.enemy)
            {
                other.GetComponent<PlayerController>().TakeDamage(attacker);
            }
        }
        //士兵单位
        else if (other.GetComponent<UnitBase>() != null)
        {
            UnitBase defender = other.GetComponent<UnitBase>();
            if (attacker.unitData.armyType != defender.unitData.armyType)
            {
                defender.TakeDamage(attacker);
            }
            BattleMgr.Ins.arrowPool.CollectOne(gameObject);
        }
    }

    private void StopAndSeParent(Transform _target)
    {
        isEnable = false;
        collider.enabled = false;
        transform.SetParent(_target);
    }

    public override void OnCollect()
    {
        base.OnCollect();
        collider.enabled = true;
        isEnable = false;
    }
}

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

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!rigi.IsSleeping())
        {
            transform.position += (transform.up * Time.deltaTime * moveSpeed);
            if (transform.eulerAngles.z < 180)
            {
                transform.eulerAngles += new Vector3(0, 0, angleSpeed) * Time.deltaTime;
            }
            transform.TransformVector(transform.right);
        }
    }

    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        Transform startPos = _params[0] as Transform;
        shootTarget = _params[1] as Transform;
        attacker = _params[2] as UnitBase;
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
        Debug.Log("height:" + height + "heightAngle" + heightAngle);
        transform.eulerAngles = startPos.eulerAngles - new Vector3(0, 0, startAngle + heightAngle);
        //增加一个随机角度
        transform.eulerAngles += new Vector3(0, Random.Range(-2, 2), Random.Range(-3, 3));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagType.Env.ToString())
        {
            StopAndSeParent(other.transform);
        }
        else if (other.tag == TagType.Player.ToString())
        {
            StopAndSeParent(other.transform);
            if (attacker.unitData.armyType == ArmyType.enemy)
            {
                other.GetComponent<PlayerController>().TakeDamage(attacker);
            }
        }
        else if (other.GetComponent<UnitBase>() != null)
        {
            UnitBase defender = other.GetComponent<UnitBase>();
            if (attacker.unitData.armyType != defender.unitData.armyType)
            {
                defender.TakeDamage(defender);
            }
            StopAndSeParent(other.transform);
        }
    }

    private void StopAndSeParent(Transform _target)
    {
        transform.SetParent(_target);
        collider.enabled = false;
        rigi.Sleep();
    }

    public override void OnCollect()
    {
        base.OnCollect();
        collider.enabled = true;
        rigi.WakeUp();
    }
}

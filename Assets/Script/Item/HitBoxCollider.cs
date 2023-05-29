using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour
{
    bool colliderEnable = false;
    List<Collider> triggerColliders = new List<Collider>();
    BoxCollider boxCollider;
    //¹¥»÷Õß
    UnitBase attackerUnitBase;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    public void Init(UnitBase _unitBase)
    {
        attackerUnitBase = _unitBase;
    }

    private void OnTriggerEnter(Collider _defender)
    {
        if (colliderEnable && !triggerColliders.Contains(_defender))
        {
            triggerColliders.Add(_defender);
            UnitBase _defenderUnitBase = _defender.GetComponent<UnitBase>();
            //ÉËº¦µ½µ¥Î»ÇÒ²»ÊÇÓÑ¾ü
            if (_defenderUnitBase != null && _defenderUnitBase.battleParams.armyType != attackerUnitBase.battleParams.armyType)
            {
                attackerUnitBase.AttackUnit(_defenderUnitBase);
            }
        }
    }

    public void StartTrigger()
    {
        colliderEnable = true;
        boxCollider.enabled = true;
        StartCoroutine(StopTrigger());
    }

    private IEnumerator StopTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var item in triggerColliders)
        {
            Debug.Log("ÉËº¦Åö×²ºÐ²âÊÔ hit" + item);
        }
        colliderEnable = false;
        boxCollider.enabled = false;
        triggerColliders.Clear();
    }

    public void SetColliderEnable(bool _enable)
    {
        colliderEnable = _enable;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArrow : PoolItemBase3D
{
    Rigidbody rigi;
    new BoxCollider collider;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigi.AddForce(0, 0, 10);
    }

    void Update()
    {
    }
}

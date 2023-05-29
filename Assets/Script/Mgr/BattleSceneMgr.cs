using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneMgr : MonoBehaviour
{
    public static BattleSceneMgr Ins;
    [HideInInspector]
    public Transform myArmyList;
    [HideInInspector]
    public Transform enemyList;
    [HideInInspector]
    public Transform myArmyStartPoint;
    [HideInInspector]
    public Transform enemyStartPoint;
    [HideInInspector]
    public Camera battleFieldCamera;
    /// <summary>
    /// ³öÉú°ë¾¶
    /// </summary>
    public const float birthSize = 4;

    public void Awake()
    {
        Ins = this;
        myArmyList = transform.Find("myArmyList");
        enemyList = transform.Find("enemyList");
        myArmyStartPoint = transform.Find("myArmyStartPoint");
        enemyStartPoint = transform.Find("enemyStartPoint");
        battleFieldCamera = GameObject.Find("BattleFieldCamera").GetComponent<Camera>();
    }
}

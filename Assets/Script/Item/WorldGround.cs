using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGround : MonoBehaviour
{
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        transform.localScale = new Vector3(WorldMgr.Ins.worldSize.x / 5, 1, WorldMgr.Ins.worldSize.y / 5);
    }

    internal void OnClick(Vector3 point)
    {
        Utility.Dump($"点击地面{{{point.x.ToString("f2")},{point.z.ToString("f2")}}}");
        WorldMgr.Ins.worldPlayer.MoveToPos(point);
    }
}

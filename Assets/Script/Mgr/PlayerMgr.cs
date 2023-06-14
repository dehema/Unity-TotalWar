using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoSingleton<PlayerMgr>
{
    float world_moveSpeed = 5;
    public PlayerScene playerScene = PlayerScene.world;

    /// <summary>
    /// 增加玩家军队单位
    /// </summary>
    /// <param name="_unitID"></param>
    public void AddPlayerUnit(int _unitID)
    {
        DataMgr.Ins.gameData.armyUnits.Add(new UnitData(_unitID));
    }

    public void SetPlayerScene(PlayerScene _playerScene)
    {
        playerScene = _playerScene;
    }
}

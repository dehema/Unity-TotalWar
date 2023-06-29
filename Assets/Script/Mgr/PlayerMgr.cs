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
        TroopData troop = GetPlayerTroop();
        if (troop.units.ContainsKey(_unitID))
            troop.units[_unitID] += 1;
        else
            troop.units.Add(_unitID, 1);
    }

    public TroopData GetPlayerTroop()
    {
        foreach (var troop in DataMgr.Ins.gameData.factions[DataMgr.playerFactionID].troops)
        {
            if (troop.troopType == TroopType.Player)
            {
                return troop;
            }
        }
        return null;
    }

    public void SetPlayerScene(PlayerScene _playerScene)
    {
        playerScene = _playerScene;
    }
}

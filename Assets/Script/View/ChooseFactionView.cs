using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChooseFactionView : BaseView
{
    /// <summary>
    /// ѡ������
    /// </summary>
    public void OnClickChooseRace()
    {
        //ѡ������
        DataMgr.Ins.gameData.isRoleInit = true;
        DataMgr.Ins.gameData.roleRace = (int)RaceType.Human;
        DataMgr.Ins.gameData.roleFaction = 11;
        DataMgr.Ins.SaveGameData();
        Close();
    }


    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        GameMgr.Ins.StartGame();
    }
}

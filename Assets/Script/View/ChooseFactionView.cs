using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChooseFactionView : BaseView
{
    /// <summary>
    /// 选择种族
    /// </summary>
    public void OnClickChooseRace()
    {
        //选择人族
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

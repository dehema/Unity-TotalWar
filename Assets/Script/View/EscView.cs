using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public partial class EscView : BaseView
{

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        btClose_Button.SetButton(Close);
        btSetting_Button.SetButton(() => { UIMgr.Ins.OpenView<SettingView>(); });
        btExit_Button.SetButton(OnClickExit);
        btSave_Button.SetButton(OnClickSave);
        btDebugWin_Button.SetDebugButton(OnClickDebugWin);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        //����ʱ������
        WorldMgr.Ins?.worldDate.SetTimeSpeed(TimeSpeed.pause);
        //����ս������ӽ�
        if (SceneMgr.Ins.IsBattleField)
        {
            BattleMgr.Ins.SetLockPlayerCamera(true);
        }
    }

    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        //����ս������ӽ�
        if (SceneMgr.Ins.IsBattleField)
        {
            BattleMgr.Ins.SetLockPlayerCamera(false);
        }
    }

    public void OnClickExit()
    {
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            DataMgr.Ins.SaveAllData();
            Application.Quit();
        }
    }

    public void OnClickSave()
    {
        DataMgr.Ins.SaveAllData();
        Close();
    }

    private void OnClickDebugWin()
    {
        BattleMgr.Ins.enemyList.Clear();
        BattleMgr.Ins.BattleVictory();
    }
}

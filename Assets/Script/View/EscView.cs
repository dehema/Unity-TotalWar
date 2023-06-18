using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public partial class EscView : BaseView
{
    float timeScale = 1;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        btClose_Button.SetButton(Close);
        btSetting_Button.SetButton(() => { UIMgr.Ins.OpenView<SettingView>(); });
        btExit_Button.SetButton(OnClickExit);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        //控制时间流速
        timeScale = Time.timeScale;
        Time.timeScale = 0;
        WorldMgr.Ins?.worldDate.SetTimeSpeed(TimeSpeed.pause);
        //锁定战场玩家视角
        if (SceneMgr.Ins.IsBattleField)
        {
            BattleMgr.Ins.SetLockPlayerCamera(true);
        }
    }

    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        //控制时间流速
        Time.timeScale = timeScale;
        //锁定战场玩家视角
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
}

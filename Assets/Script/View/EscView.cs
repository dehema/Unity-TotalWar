using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class EscView : BaseView
{
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
        WorldMgr.Ins?.worldDate.SetTimeSpeed(TimeSpeed.pause);
    }

    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        if (SceneMgr.Ins.IsBattleField)
        {
            InputMgr.Ins.SetMouseModel(MouseModel.BattleField);
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

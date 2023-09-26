using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleVictoryView : BaseView
{
    BattleVictoryViewParams viewParams;
    bool isAnyKeyClose = false;

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        InputMgr.Ins.SetMouseModel(MouseModel.UI);
        viewParams = _params.Length > 0 ? _params[0] as BattleVictoryViewParams : null;
        isAnyKeyClose = false;
        ResetUI();

        reward_CanvasGroup.alpha = 0;
        ani_CanvasGroup.alpha = 1;
        Timer.Ins.SetTimeOut(() =>
        {
            isAnyKeyClose = true;
            ani_CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                reward_CanvasGroup.DOFade(1, 1);
            });
        }, 2);
    }

    public void ResetUI()
    {
        //pos
        wing_l_Rect.anchoredPosition = new Vector2(-500, 7);
        wing_r_Rect.anchoredPosition = new Vector2(500, 7);
        wing_l_Rect.DOAnchorPos3DX(-329, 1);
        wing_r_Rect.DOAnchorPos3DX(329, 1);
        //color
        wing_l_Image.color = new Color(1, 1, 1, 0);
        wing_r_Image.color = new Color(1, 1, 1, 0);
        wing_l_Image.DOFade(1, 1);
        wing_r_Image.DOFade(1, 1);
        //anyKey
        anyKeyClose_Text.color = new Color(1, 1, 1, 0);
        anyKeyClose_Text.DOFade(1, 0.5f).SetDelay(1);
    }

    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        viewParams?.closeCB?.Invoke();
    }

    private void Update()
    {
        if (isAnyKeyClose && Input.anyKeyDown)
        {
            Close();
        }
    }
}

public class BattleVictoryViewParams
{
    public Action closeCB;
}
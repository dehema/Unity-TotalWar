using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class TopView : BaseView
{
    //data
    bool isShow = true;
    const float showDuration = 0.5f;
    int oldGold = 0;
    int oldExp = 0;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        oldGold = DataMgr.Ins.playerData.gold.Value;
        oldExp = DataMgr.Ins.playerData.exp.Value;
        //bind
        InitBinding();
        //event
        btSetting_Button.SetButton(() => { UIMgr.Ins.OpenView<EscView>(); });
        btTimePause_Button.SetButton(() => { OnClickTimeSpeed(TimeSpeed.pause); });
        btTimeNormal_Button.SetButton(() => { OnClickTimeSpeed(TimeSpeed.normal); });
        btTimeQuick_Button.SetButton(() => { OnClickTimeSpeed(TimeSpeed.quick); });
        tech_Button.SetButton(() => { UIMgr.Ins.OpenView<TechView>(); });
        debugAddGold1K_Button.SetButton(() => { ResMgr.Ins.AddRes(ResType.gold, 1000); DataMgr.Ins.SavePlayerData(); });
        //UI
        RefreshTimeSpeedUI();
    }

    private void Update()
    {
        UpdateDateText();
    }

    void InitBinding()
    {
        DataMgr.Ins.playerData.gold.Bind((dm) =>
        {
            Utility.DONumVal(oldGold, DataMgr.Ins.playerData.gold.Value, _num =>
            {
                int num = (int)_num;
                goldNum_Text.text = Utility.GetValByThousands(num);
                oldGold = num;
            });
        });
        DataMgr.Ins.playerData.level.Bind(dm => { txtLevel_Text.text = dm.value.ToString(); });
        DataMgr.Ins.playerData.exp.Bind(dm =>
        {
            int num = (int)dm.value;
            int nextExp = 1000;
            txtExp_Text.text = $"{num}/{nextExp}";
            if (num > oldExp)
            {
                expSlider_Slider.DOValue(num / (float)nextExp, 1);
            }
            else
            {
                expSlider_Slider.value = num / (float)nextExp;
            }
            oldExp = num;
        });
        DataMgr.Ins.playerData.playerName.Bind(dm =>
        {
            txtName_Text.text = dm.value.ToString();
        });
    }

    public void SetShow(bool _isShow, Action _closeCB = null)
    {
        isShow = _isShow;
        if (isShow)
        {
            top_Rect.DOAnchorPos(Vector2.zero, showDuration);
        }
        else
        {
            top_Rect.DOAnchorPos(new Vector2(0, top_Rect.rect.height), showDuration).onComplete = () =>
            {
                _closeCB?.Invoke();
            };
        }
    }

    /// <summary>
    /// 刷新日期文本
    /// </summary>
    public void UpdateDateText()
    {
        if (WorldMgr.Ins?.worldDate.timeSpeed != TimeSpeed.pause)
        {
            txtDate_Text.text = WorldMgr.Ins.worldDate.GetDateStr();
        }
    }

    /// <summary>
    /// 点击时间速度
    /// </summary>
    /// <param name="_timeSpeed"></param>
    public void OnClickTimeSpeed(TimeSpeed _timeSpeed)
    {
        WorldMgr.Ins.worldDate.SetTimeSpeed(_timeSpeed);
    }

    /// <summary>
    /// 刷新时间流速相关UI
    /// </summary>
    public void RefreshTimeSpeedUI()
    {
        if (WorldMgr.Ins == null)
        {
            btTimePause_light.SetActive(true);
            return;
        }
        btTimePause_light.SetActive(WorldMgr.Ins?.worldDate.timeSpeed == TimeSpeed.pause);
        btTimeNormal_light.SetActive(WorldMgr.Ins?.worldDate.timeSpeed == TimeSpeed.normal);
        btTimeQuick_light.SetActive(WorldMgr.Ins?.worldDate.timeSpeed == TimeSpeed.quick);
    }
}

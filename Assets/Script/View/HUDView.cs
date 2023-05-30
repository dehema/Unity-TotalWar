using DB;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class HUDView : BaseView
{
    ObjPool stateBarPool;
    Dictionary<UnitBase, HUDUnitStateBar> stateBarDict = new Dictionary<UnitBase, HUDUnitStateBar>();

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        stateBarPool = PoolMgr.Ins.CreatePool(hudUnitStateBar);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        hpDBHandler = DataMgr.Ins.playerData.hp.Bind(BindHp);
        //UI
        InitHpSlider();
    }

    private void InitHpSlider()
    {
        hpSlider_Rect.anchoredPosition = Vector2.zero;
        hpSlider_Rect.anchorMin = Vector2.zero;
        hpSlider_Rect.anchorMax = Vector2.zero;
        hpSlider_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpSliderBg_Rect.rect.width);
        hpSlider_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hpSliderBg_Rect.rect.height);
    }


    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
        stateBarPool.CollectAll();
        DataMgr.Ins.playerData.hp.UnBind(hpDBHandler);
    }

    DBHandler.Binding hpDBHandler;
    void BindHp(DBModify _dm)
    {
        if (_dm.action != DBAction.Update)
            return;
        float hp = DataMgr.Ins.playerData.hp.Value;
        float maxHp = DataMgr.Ins.playerData.hpMax.Value;
        txtHp_Text.text = $"{(int)hp}/{(int)maxHp}";
        float progress = hp / maxHp;
        Vector2 newRect = new Vector2(hpSliderBg_Rect.rect.width * progress, hpSliderBg_Rect.rect.height);
        hpSlider_Rect.DOSizeDelta(newRect, 0.2f);
    }

    /// <summary>
    /// 显示某个对象的状态表
    /// </summary>
    /// <param name="_baseUnit">标注的单位</param>
    /// <param name="_realHp">被攻击后的生命值</param>
    /// <param name="_hpJust">被攻击前的生命值</param>
    /// <param name="_max">生命最大值</param>
    public void ShowStateBar(UnitBase _baseUnit, float _realHp, float _hpJust, float _max)
    {
        HUDUnitStateBar item;
        if (stateBarDict.ContainsKey(_baseUnit))
        {
            item = stateBarDict[_baseUnit];
        }
        else
        {
            item = stateBarPool.Get<HUDUnitStateBar>();
            stateBarDict[_baseUnit] = item;
        }
        item.ShowTween(_baseUnit, _realHp, _hpJust, _max);
    }

    /// <summary>
    /// 隐藏某个对象的状态表
    /// </summary>
    /// <param name="_baseUnit"></param>
    public void HideStateBar(UnitBase _baseUnit)
    {
        if (!stateBarDict.ContainsKey(_baseUnit))
        {
            return;
        }
        stateBarPool.CollectOne(stateBarDict[_baseUnit].gameObject);
        stateBarDict.Remove(_baseUnit);
    }
}

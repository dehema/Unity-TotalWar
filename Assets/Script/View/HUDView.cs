using DB;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float hp = DataMgr.Ins.playerData.hp.Value;
        float maxHp = DataMgr.Ins.playerData.hpMax.Value;
        txtHp_Text.text = $"{(int)hp}/{(int)maxHp}";
        hpSlider_Slider.value = hp / maxHp;
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

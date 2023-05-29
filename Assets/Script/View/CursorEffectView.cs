using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class CursorEffectView : BaseView
{
    ObjPool cursorPool;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        Utility.SetParticleOrder(cursorClickEffect, canvas.sortingOrder);
        cursorPool = PoolMgr.Ins.CreatePool(cursorClickEffect);
    }

    public void ClickEffect(Vector3 _mouseEffect)
    {
        GameObject item = cursorPool.Get();
        RectTransform rect = item.GetComponent<RectTransform>();
        Vector3 pos = _mouseEffect - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        rect.anchoredPosition = pos;
        Timer.Ins.SetTimeOut(() => { cursorPool.CollectOne(item); }, 3);
    }
}

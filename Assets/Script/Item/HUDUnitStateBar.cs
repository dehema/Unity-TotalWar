using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class HUDUnitStateBar : PoolItemBase
{
    UnitBase unitBase;
    float rectWidth;
    float rectHeight;
    RectTransform rect;
    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        rect = GetComponent<RectTransform>();
        rectWidth = rect.rect.width;
        rectHeight = rect.rect.height;
        _LoadUI();
    }

    /// <summary>
    /// 生命值动画
    /// </summary>
    /// <param name="_baseUnit">标注的单位</param>
    /// <param name="_realHp">被攻击后的生命值</param>
    /// <param name="_hpJust">被攻击前的生命值</param>
    /// <param name="_max">生命最大值</param>
    public void ShowTween(UnitBase _baseUnit, float _realHp, float _hpJust, float _max)
    {
        unitBase = _baseUnit;
        float hpJust = _hpJust / _max;
        float hpNow = _realHp / _max;
        redHp_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpJust * rectWidth);
        greenHp_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpJust * rectWidth);
        greenHp_Rect.DOSizeDelta(new Vector2(hpNow * rectWidth, rectHeight), 0.5f);
    }



    public void Update()
    {
        if (unitBase)
        {
            //屏幕坐标
            //模型坐标在脚下 + 身高 + 0.2偏移值
            Vector3 sPos = BattleSceneMgr.Ins.battleFieldCamera.WorldToScreenPoint(unitBase.transform.position + new Vector3(0, unitBase.unitConfig.height + 0.2f));
            //Debug.LogError($"X:{sPos.y}  Y:{sPos.y}");
            //
            Vector2 uiPos = new Vector3(sPos.x - Screen.width / 2, sPos.y - Screen.height / 2);
            GetComponent<RectTransform>().anchoredPosition = uiPos;
        }
    }
}

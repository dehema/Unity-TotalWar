using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RecruitUnitItem : PoolItemBase
{
    public RecruitUnitItemParams itemParams;
    GameObject unitModel;
    UnitConfig UnitConfig;
    Animator animator;
    [HideInInspector]
    public int selNum = 0;
    RenderTexture renderTexture;
    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        //reset
        btSel_Button.SetButton(OnClick);
        selNum = 0;
        iconSel.SetActive(false);
        renderTexture = new RenderTexture(1024, 1024, 0);
        camera_Camera.targetTexture = renderTexture;
        unitIcon_RawImage.texture = renderTexture;

        itemParams = _params[0] as RecruitUnitItemParams;
        UnitConfig = ConfigMgr.Ins.GetUnitConfig(itemParams.unitID);
        //模型
        UnitConfig unitConfig = ConfigMgr.Ins.GetUnitConfig(itemParams.unitID);
        unitModel = Instantiate(Resources.Load<GameObject>(PrefabPath.unit + unitConfig.fullID), camera.transform);
        unitModel.transform.localPosition = new Vector3(0, -0.8f, 2);
        unitModel.transform.localEulerAngles = new Vector3(0, 180, 0);
        unitModel.transform.localScale = new Vector3(1, 1, 1);
        Utility.SetLayer(unitModel, (int)GoLayer.Model);

        animator = unitModel.GetComponent<Animator>();
        animator.Play(ActionState.idle.ToString(), 0);
        //
        SetStar();
        name_Text.text = LangMgr.Ins.Get(UnitConfig.name);
        unitNum_Text.text = "x" + itemParams.unitNum;
        SetFloatTips();
    }

    /// <summary>
    /// 设置浮动提示
    /// </summary>
    /// <param name="_tips"></param>
    private void SetFloatTips()
    {
        List<string> strs = new List<string>();
        strs.Add(LangMgr.Ins.Get(UnitConfig.name));
        strs.Add(LangMgr.Ins.Get("1672500001") + UnitConfig.value);
        strs.Add(LangMgr.Ins.Get("1672500002") + UnitConfig.upkeep);
        string _tips = Utility.GetParagraphText(strs);
        btSel_ShowCommonFloatTips.SetTips(_tips);
    }

    private void OnClick()
    {
        selNum++;
        if (selNum > itemParams.unitNum)
        {
            selNum = 0;
        }
        bool isShow = selNum != 0;
        iconSel.SetActive(isShow);
        if (isShow)
        {
            txtSelNum_Text.text = "x" + selNum;
        }
    }

    public override void OnCollect()
    {
        base.OnCollect();
        btSel_Button.onClick.RemoveAllListeners();
        if (unitModel != null)
        {
            Destroy(unitModel);
        }
    }

    public void SetStar(int _lv = 1)
    {
        Action<GameObject> SetActive = (item) =>
        {
            int index = int.Parse(item.name.Substring(item.name.Length - 1, 1));
            item.SetActive(index <= _lv);
        };
        SetActive(star_1);
        SetActive(star_2);
        SetActive(star_3);
        SetActive(star_4);
        SetActive(star_5);
    }
}

public class RecruitUnitItemParams
{
    public int unitID;
    /// <summary>
    /// 士兵数量
    /// </summary>
    public int unitNum;

    public RecruitUnitItemParams(int _unitID, int _unitNum)
    {
        unitID = _unitID;
        unitNum = _unitNum;
    }
}

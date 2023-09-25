using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TechInfoView : BaseView
{
    TechInfoViewParams viewParams;
    TechData techData;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        btLear_Button.SetButton(Close);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        viewParams = _params[0] as TechInfoViewParams;
        techData = DataMgr.Ins.GetTechData(viewParams.techID);
        techName_Text.text = LangMgr.Ins.Get(techData.techName);
        techDesc_Text.text = LangMgr.Ins.Get(techData.techDesc);
    }
}


public class TechInfoViewParams
{
    public int techID;
}
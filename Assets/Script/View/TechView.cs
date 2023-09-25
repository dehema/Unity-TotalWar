using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TechView : BaseView
{

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        btClose_Button.SetButton(Close);
    }
}

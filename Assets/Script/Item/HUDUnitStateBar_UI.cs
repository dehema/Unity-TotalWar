using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class HUDUnitStateBar : PoolItemBase
{
    [HideInInspector]
    public GameObject redHp;
    [HideInInspector]
    public Image redHp_Image;
    [HideInInspector]
    public RectTransform redHp_Rect;
    [HideInInspector]
    public GameObject greenHp;
    [HideInInspector]
    public Image greenHp_Image;
    [HideInInspector]
    public RectTransform greenHp_Rect;

    internal void _LoadUI()    
    {
        redHp = transform.Find("$redHp#Image,Rect").gameObject;
        redHp_Image = redHp.GetComponent<Image>();
        redHp_Rect = redHp.GetComponent<RectTransform>();
        greenHp = transform.Find("$greenHp#Image,Rect").gameObject;
        greenHp_Image = greenHp.GetComponent<Image>();
        greenHp_Rect = greenHp.GetComponent<RectTransform>();
    }
}
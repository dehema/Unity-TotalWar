using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    int teckID;
    GameObject goLight;

    void Awake()
    {
        GetComponent<Button>().SetButton(OnButtonClick);
        goLight = transform.Find("light").gameObject;
    }

    void OnButtonClick()
    {
        TechInfoViewParams viewParams = new TechInfoViewParams();
        viewParams.techID = teckID;
        UIMgr.Ins.OpenView<TechInfoView>(viewParams);
    }

    private void OnEnable()
    {
        goLight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        goLight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        goLight.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RecruitView : BaseView
{
    RecruitViewParams viewParams;
    ObjPool unitPool;
    /// <summary>
    /// �����UnitID
    /// </summary>
    List<int> sortUnitIDs = new List<int>();
    Dictionary<int, RecruitUnitItem> unitItemDict = new Dictionary<int, RecruitUnitItem>();

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        unitPool = PoolMgr.Ins.CreatePool(unitItem);
        btClose_Button.SetButton(Close);
        btHome_Button.SetButton(Close);
        sureRecruit_Button.SetButton(OnClickSureRecruit);
        debugRefreshRecruitUnit_Button.SetDebugButton(OnClickDebugRefreshRecruitUnit);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        viewParams = _params[0] as RecruitViewParams;
        InitSortUnitIDs(viewParams.unitNum);
        RefreshUnitItems();
    }

    /// <summary>
    /// ���ּ�������
    /// </summary>
    private void InitSortUnitIDs(Dictionary<int, int> _unitNum)
    {
        sortUnitIDs.Clear();
        foreach (var item in _unitNum)
        {
            sortUnitIDs.Add(item.Key);
        }
    }

    /// <summary>
    /// ȷ����ļ
    /// </summary>
    private void OnClickSureRecruit()
    {
        Dictionary<int, int> unitIDs = new Dictionary<int, int>();
        foreach (var item in unitItemDict)
        {
            if (item.Value.selNum != 0)
            {
                int unitID = item.Key;
                unitIDs[unitID] = item.Value.selNum;
            }
        }
        if (unitIDs.Count == 0)
        {
            return;
        }
        int cost = 0;
        foreach (var item in unitIDs)
        {
            UnitConfig unitConfig = ConfigMgr.Ins.GetUnitConfig(item.Key);
            cost += unitConfig.value * item.Value;
        }
        Debug.Log($"������{cost}���");
        if (DataMgr.Ins.playerData.gold.Value < cost)
        {
            //��Ҳ���
            UIMgr.Ins.OpenView<TipsView>().Tips(LangMgr.Ins.Get("1672500003"));
            return;
        }
        ResMgr.Ins.AddRes(ResType.gold, -cost);
        foreach (var item in unitIDs)
        {
            UnitConfig unitConfig = ConfigMgr.Ins.GetUnitConfig(item.Key);
            Debug.Log($"��ļ��{item.Value}����{LangMgr.Ins.Get(unitConfig.name)}��");
            for (int i = 0; i < item.Value; i++)
            {
                PlayerMgr.Ins.AddPlayerUnit(item.Key);
            }
        }
        //��ȥ��ļ��λ
        foreach (var item in unitIDs)
        {
            viewParams.unitNum[item.Key] -= item.Value;
            if (viewParams.unitNum[item.Key] == 0)
            {
                //����ʿ����ȫ����ļ 
                viewParams.unitNum.Remove(item.Key);
                sortUnitIDs.Remove(item.Key);
            }
        }
        //ˢ��UI
        RefreshUnitItems();
    }

    /// <summary>
    /// ˢ����������
    /// </summary>
    private void RefreshSortUnitIDs()
    {
        sortUnitIDs.Sort();
    }

    /// <summary>
    /// ˢ�����ж���
    /// </summary>
    private void RefreshUnitItems()
    {
        RefreshSortUnitIDs();
        unitPool.CollectAll();
        unitItemDict.Clear();
        foreach (var unitID in sortUnitIDs)
        {
            int unitNum = viewParams.unitNum[unitID];
            RecruitUnitItemParams itemParams = new RecruitUnitItemParams(unitID, unitNum);
            RecruitUnitItem recruitUnitItem = unitPool.Get<RecruitUnitItem>(itemParams);
            unitItemDict[unitID] = recruitUnitItem;
        }
    }

    /// <summary>
    /// ���ˢ�µ�λ ���԰�ť
    /// </summary>
    private void OnClickDebugRefreshRecruitUnit()
    {
        viewParams.cityData.RefreshRecruitUnit();
        InitSortUnitIDs(viewParams.cityData.recruitUnit);
        RefreshUnitItems();
    }
}

public class RecruitViewParams
{
    /// <summary>
    /// ʿ��ID������
    /// </summary>
    public Dictionary<int, int> unitNum = new Dictionary<int, int>();
    public CityData cityData;
}
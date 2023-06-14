using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimeData
{
    /// <summary>
    /// �ڼ��� 4���Ƚ�1��
    /// </summary>
    public int year = 1;
    /// <summary>
    /// ��ǰ���ȵĵڼ��� 30���1����
    /// </summary>
    public int day = 1;
    /// <summary>
    /// 24Сʱ�� 24��1
    /// </summary>
    public int hour = 12;
    public float minute = 0;
    /// <summary>
    /// 4���Ƚ�1��
    /// </summary>
    public Season season = Season.spring;

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns></returns>
    public string GetSeasonStr()
    {
        string str = "";
        switch (season)
        {
            case Season.spring:
                str = LangMgr.Ins.Get("1672306551");
                break;
            case Season.summer:
                str = LangMgr.Ins.Get("1672306552");
                break;
            case Season.autumn:
                str = LangMgr.Ins.Get("1672306553");
                break;
            case Season.winter:
                str = LangMgr.Ins.Get("1672306554");
                break;
        }
        return str;
    }

    /// <summary>
    /// ��ȡСʱ����
    /// </summary>
    public int TotalHour
    {
        get
        {
            int _season = (year - 1) * 4 + (int)season - 1;
            int _day = _season * 30 + day;
            int _hours = _day * 24 + hour;
            return _hours;
        }
    }
}

public enum Season
{
    spring = 1,
    summer = 2,
    autumn = 3,
    winter = 4
}

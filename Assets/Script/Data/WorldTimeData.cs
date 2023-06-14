using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimeData
{
    /// <summary>
    /// 第几年 4季度进1年
    /// </summary>
    public int year = 1;
    /// <summary>
    /// 当前季度的第几天 30天进1季度
    /// </summary>
    public int day = 1;
    /// <summary>
    /// 24小时制 24进1
    /// </summary>
    public int hour = 12;
    public float minute = 0;
    /// <summary>
    /// 4季度进1年
    /// </summary>
    public Season season = Season.spring;

    /// <summary>
    /// 获取季节语言
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
    /// 获取小时总数
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

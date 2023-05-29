using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDate
{
    public TimeSpeed timeSpeed = TimeSpeed.pause;

    public delegate void OnNewDay();
    public OnNewDay onNewDay;
    /// <summary>
    /// 时间流速
    /// </summary>
    public float speed = 0;

    public void UpdateWroldTime()
    {
        if (timeSpeed != TimeSpeed.pause)
        {
            AddTime(speed);
        }
    }

    /// <summary>
    /// 设置时间流速
    /// </summary>
    /// <param name="_timeSpeed"></param>
    public void SetTimeSpeed(TimeSpeed _timeSpeed)
    {
        if (timeSpeed == _timeSpeed)
        {
            return;
        }
        timeSpeed = _timeSpeed;
        switch (timeSpeed)
        {
            case TimeSpeed.pause:
                speed = 0;
                break;
            case TimeSpeed.normal:
                speed = Utility.GetSetting<float>(SettingField.Time.World_Time_NormalSpeed);
                break;
            case TimeSpeed.quick:
                speed = Utility.GetSetting<float>(SettingField.Time.World_Time_QuickSpeed);
                break;
        }
        NavMgr.Ins.RefreshAllNavSpeed();
        UIMgr.Ins.GetView<TopView>()?.RefreshTimeSpeedUI();
    }
    public void AddTime(float _minute)
    {
        Action checkMinute = () =>
        {
            if (DataMgr.Ins.gameData.worldTime.minute >= 60)
            {
                DataMgr.Ins.gameData.worldTime.minute = 0;
                DataMgr.Ins.gameData.worldTime.hour++;
                LogDate();
            }
        };
        Action checkHour = () =>
        {
            if (DataMgr.Ins.gameData.worldTime.hour >= 24)
            {
                onNewDay();
                DataMgr.Ins.gameData.worldTime.hour = 0;
                DataMgr.Ins.gameData.worldTime.day++;
            }
        };
        Action checkDay = () =>
        {
            if (DataMgr.Ins.gameData.worldTime.day >= 30)
            {
                DataMgr.Ins.gameData.worldTime.day = 0;
                if (DataMgr.Ins.gameData.worldTime.season == Season.winter)
                {
                    DataMgr.Ins.gameData.worldTime.year++;
                }
                DataMgr.Ins.gameData.worldTime.season++;
            }
        };
        DataMgr.Ins.gameData.worldTime.minute += _minute;
        checkMinute();
        checkHour();
        checkDay();
    }

    /// <summary>
    /// log日期
    /// </summary>
    private void LogDate()
    {
        Debug.Log("更新时间：" + GetDateStr());
    }

    public string GetDateStr()
    {
        var time = DataMgr.Ins.gameData.worldTime;
        string timeQuan = "";
        if (time.hour >= 6 && time.hour < 12)
        {
            timeQuan = LangMgr.Ins.Get("1672306557");
        }
        else if (time.hour >= 12 && time.hour < 18)
        {
            timeQuan = LangMgr.Ins.Get("1672306558");
        }
        else
        {
            timeQuan = LangMgr.Ins.Get("1672306559");
        }
        string str = string.Format(LangMgr.Ins.Get("1672306570"), time.year, time.GetSeasonStr(), time.day, timeQuan, time.hour);
        return str;
    }

    /// <summary>
    /// 获取相差小时数
    /// </summary>
    /// <param name="_data1"></param>
    /// <param name="_data2"></param>
    /// <returns></returns>
    public static int GetHourSpan(WorldTimeData _data1, WorldTimeData _data2)
    {
        return Mathf.Abs(_data1.TotalHour - _data2.TotalHour);
    }
}
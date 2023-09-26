using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    /// <summary>
    /// 是否评价过
    /// </summary>
    public bool evaluate = false;
    /// <summary>
    /// 获胜次数
    /// </summary>
    public int winNumber = 0;
    /// <summary>
    /// 游戏时长
    /// </summary>
    public float gameTotalTime = 0;
    /// <summary>
    /// 游戏切出后台又回来的次数
    /// </summary>
    public int gamePauseNum = 0;
    /// <summary>
    /// 开宝箱时选择nothanks的次数
    /// </summary>
    public int nothanksNum = 0;
    /// <summary>
    /// 上次登录日期
    /// </summary>
    public DateTime lastLoginDate = DateTime.MinValue;
    /// <summary>
    /// 上次离线日期
    /// </summary>
    public DateTime lastOffLine = DateTime.MinValue;
    /// <summary>
    /// 跳过激励视频的次数
    /// </summary>
    public int passRewardVideoTime;
    /// <summary>
    /// 上次播放插屏广告的时间
    /// </summary>
    public DateTime lastADInterstitialTime;
    /// <summary>
    /// 上次播放激励广告的时间
    /// </summary>
    public DateTime lastADRewardTime;
    public WorldTimeData worldTime = new WorldTimeData();
    /// <summary>
    /// 城镇数据
    /// </summary>
    public Dictionary<int, CityData> cityData = new Dictionary<int, CityData>();
    /// <summary>
    /// 进入游戏次数
    /// </summary>
    public int enterGameTime = 0;
    /// <summary>
    /// 派系数据
    /// </summary>
    public Dictionary<int, FactionData> factions = new Dictionary<int, FactionData>();
    /// <summary>
    /// 角色是否完成初始化
    /// </summary>
    public bool isRoleInit = false;
    /// <summary>
    /// 玩家种族
    /// </summary>
    public int roleRace;
    /// <summary>
    /// 玩家派系
    /// </summary>
    public int roleFaction;
}

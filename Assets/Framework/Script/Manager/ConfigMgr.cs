using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.IO;
using System;

public class ConfigMgr : MonoSingleton<ConfigMgr>
{
    const string configPath = "Json/";
    public ImageTextMixConfig imageTextMix;
    public GameSettingConfig settingConfig;
    public AllUnitConfig allUnitConfig;
    public AllCityConfig allCityConfig;
    public WorldConfig worldConfig;
    public AllFactionConfig factionConfig;

    public void Init()
    {
    }

    public void LoadAllConfig(bool _localConfig = true)
    {
        imageTextMix = LoadConfig<ImageTextMixConfig>("ImageTextMix");
        settingConfig = LoadConfig<GameSettingConfig>("Setting");
        allUnitConfig = LoadConfig<AllUnitConfig>("Unit");
        allCityConfig = LoadConfig<AllCityConfig>("City");
        worldConfig = LoadConfig<WorldConfig>("World");
        factionConfig = LoadConfig<AllFactionConfig>("Faction");
        AllLoadComplete();
    }

    private void AllLoadComplete()
    {

    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="_config"></param>
    /// <returns></returns>
    private T LoadConfig<T>(string key, string _config = "")
    {
        if (string.IsNullOrEmpty(_config))
        {
            string filePath = configPath + key;
            _config = Resources.Load<TextAsset>(filePath).text;
            Debug.Log("��ȡ�������ļ�" + filePath + "\n" + _config);
        }
        else
        {
            Debug.Log("��ȡ��Զ������" + typeof(T).ToString() + "\n" + _config);
        }
        T t = JsonConvert.DeserializeObject<T>(_config);
        ConfigBase configBase = t as ConfigBase;
        configBase.Init();
        return t;
    }

    /// <summary>
    /// ��ȡUIView����Yaml�ļ�·��
    /// </summary>
    /// <returns></returns>
    public string GetUIViewConfigPath()
    {
        return "Config/UIView";
    }

    /// <summary>
    /// ��ȡUI����
    /// </summary>
    /// <returns></returns>
    public UIViewConfig LoadUIConfig()
    {
        Utility.Log("��ʼ��ȡUI����");
        string configPath = GetUIViewConfigPath();
        string config = Resources.Load<TextAsset>(configPath).text;
        var deserializer = new DeserializerBuilder()
              .WithNamingConvention(CamelCaseNamingConvention.Instance)
              .Build();
        Utility.Dump(config);
        UIViewConfig UIViewConfig = deserializer.Deserialize<UIViewConfig>(config);
        return UIViewConfig;
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns></returns>
    public CityConfig GetCityConfig(int _cityID)
    {
        return allCityConfig.city[_cityID];
    }

    /// <summary>
    /// ��ȡ��λ����
    /// </summary>
    /// <returns></returns>
    public UnitConfig GetUnitConfig(int _unitID)
    {
        return allUnitConfig.unit[_unitID];
    }

    /// <summary>
    /// ��ȡ��ϵ����
    /// </summary>
    /// <returns></returns>
    public FactionConfig GetFactionConfig(int _factionID)
    {
        return factionConfig.faction[_factionID];
    }
}

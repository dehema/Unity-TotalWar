
using NUnit.Framework;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Object = UnityEngine.Object;
using UnityEditor.VersionControl;
using System.Collections;
using Unity.EditorCoroutines.Editor;

public class EditorAudioVolume : EditorWindow
{
    /// <summary>
    /// �������Ƚϵı������ݣ������һ���򱣴�
    /// </summary>
    AudioVolumeData volumeData_origin = new AudioVolumeData();
    AudioVolumeData volumeData = new AudioVolumeData();
    readonly string audioVolumeConfigPath = Application.streamingAssetsPath + "/AudioVolumeConfig.json";
    static GUIStyle titleLabelStyle;
    /// <summary>
    /// �Ƿ��Ѿ���ȡ������
    /// </summary>
    bool isLoadData = false;
    bool isSaveData = false;

    private void OnEnable()
    {
        titleLabelStyle = new GUIStyle() { fontSize = 20, alignment = TextAnchor.MiddleCenter };
        LoadData();
        isLoadData = false;
        isSaveData = false;
    }

    private void LoadData()
    {
        if (File.Exists(audioVolumeConfigPath))
        {
            string config = File.ReadAllText(audioVolumeConfigPath);
            volumeData_origin = JsonConvert.DeserializeObject<AudioVolumeData>(config);
            volumeData = JsonConvert.DeserializeObject<AudioVolumeData>(config);
        }
    }

    [MenuItem("��������/��Ƶ�ļ�������С����")]
    static void OpenMainWindow()
    {
        EditorAudioVolume window = GetWindow<EditorAudioVolume>();
        window.titleContent = new GUIContent("��Ƶ�ļ�������С����");
        window.position = new Rect(400, 100, 640, 480);
        window.Show();
    }

    EditorCoroutine saveCoroutine = null;
    private void OnGUI()
    {
        EditorGUILayout.LabelField("<color=white>------------------- ��Ч -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        AudioGUI(typeof(AudioSound));
        EditorGUILayout.LabelField("<color=white>------------------- ���� -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        AudioGUI(typeof(AudioMusic));
        if (saveCoroutine != null)
        {
            EditorCoroutineUtility.StopCoroutine(saveCoroutine);
            saveCoroutine = null;
        }
        saveCoroutine = EditorCoroutineUtility.StartCoroutine(SaveAudioVolumeConfig(), this);
        //SaveAudioVolumeConfig();
        isLoadData = true;
        isSaveData = true;
    }


    void AudioGUI(Type _type)
    {
        foreach (string audioName in Enum.GetNames(_type))
        {
            if (audioName != AudioSound.None.ToString())
            {
                AudioGUI(audioName, _type);
            }
        }
    }

    void AudioGUI(string _audioName, Type _type)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(_audioName);
        float currVolume = GetVolume(_audioName, _type);
        float volume = GUILayout.HorizontalSlider(currVolume, 0, 1, GUILayout.Width(200));
        volume *= 100;
        if ((volume * 10) % 10 > 5)
        {
            volume = Mathf.CeilToInt(volume);
        }
        else
        {
            volume = Mathf.FloorToInt(volume);
        }
        volume /= 100f;

        EditorGUILayout.LabelField("����:" + volume, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        //data
        if (volume != currVolume || !isSaveData || volume < 0)
        {
            SetVolume(_audioName, _type, volume);
        }
    }

    public void SetVolume(string _audioName, Type _type, float _volume)
    {
        if (_volume < 0)
            _volume = 1;
        if (_type == typeof(AudioSound))
        {
            if (!isLoadData)
                volumeData_origin.soundVolume[_audioName] = _volume;
            volumeData.soundVolume[_audioName] = _volume;
        }
        else if (_type == typeof(AudioMusic))
        {
            if (!isLoadData)
                volumeData_origin.musicVolume[_audioName] = _volume;
            volumeData.musicVolume[_audioName] = _volume;
        }
    }

    public float GetVolume(string _audioName, Type _type)
    {
        float val = -1f;
        if (_type == typeof(AudioSound))
        {
            if (volumeData.soundVolume.ContainsKey(_audioName))
            {
                val = volumeData.soundVolume[_audioName];
            }
        }
        else if (_type == typeof(AudioMusic))
        {
            if (volumeData.musicVolume.ContainsKey(_audioName))
            {
                val = volumeData.musicVolume[_audioName];
            }
        }
        return val;
    }

    /// <summary>
    /// ������Ƶ����
    /// </summary>
    private IEnumerator SaveAudioVolumeConfig()
    {
        yield return new EditorWaitForSeconds(0.4f);
        if (!isLoadData)
            yield break;
        if (!CheckData())
            yield break;
        File.WriteAllText(audioVolumeConfigPath, JsonConvert.SerializeObject(volumeData, Formatting.Indented));
        AssetDatabase.Refresh();
        ///��д�����ļ�
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(audioVolumeConfigPath));
        Debug.Log("�������������ļ���->" + audioVolumeConfigPath);
    }

    /// <summary>
    /// ��������Ƿ����� ������򱣴������ļ�
    /// </summary>
    /// <returns></returns>
    public bool CheckData()
    {
        bool res = false;
        foreach (var item in volumeData.musicVolume)
        {
            if (!volumeData_origin.musicVolume.ContainsKey(item.Key) || volumeData_origin.musicVolume[item.Key] != item.Value)
            {
                volumeData_origin.musicVolume[item.Key] = item.Value;
                res = true;
            }
        }
        foreach (var item in volumeData.soundVolume)
        {
            if (!volumeData_origin.soundVolume.ContainsKey(item.Key) || volumeData_origin.soundVolume[item.Key] != item.Value)
            {
                volumeData_origin.soundVolume[item.Key] = item.Value;
                res = true;
            }
        }
        return res;
    }
}

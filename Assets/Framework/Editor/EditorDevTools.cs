using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines.Editor;
using System.IO;
using UnityEngine.UI;
using System;
using Object = UnityEngine.Object;
using Path = System.IO.Path;
using DotLiquid;
using System.Collections;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEditor.Callbacks;

public class EditorDevTools : EditorWindow
{
    static GUILayoutOption[] commonLayout = { GUILayout.Height(30) };
    public static GUIStyle titleLabelStyle;
    float gameTimeSpeed = 1;
    static string editorScriptPath;

    private void OnEnable()
    {
        titleLabelStyle = new GUIStyle() { fontSize = 20, alignment = TextAnchor.MiddleCenter };
    }

    [MenuItem("��������/��������")]
    static void OpenMainWindow()
    {
        EditorDevTools window = GetWindow<EditorDevTools>();
        window.titleContent = new GUIContent("BM Development Tools");
        window.position = new Rect(400, 100, 640, 480);
        window.Show();
    }

    public static string GetEditorScriptPath()
    {
        return Application.dataPath + "/Script/Editor/EditorDevTools.cs";
    }

    Vector2 scrollPosition;
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        OnGUIUIPlaying();
        OnGUIEditor();
        OnGUIUI();
        OnGUIUIConfig();
        OnGUIUIOther();
        OnGUIDebug();
        EditorGUILayout.EndScrollView();
    }

    private void OnGUIEditor()
    {
        EditorGUILayout.LabelField("<color=white>------------------- �༭ -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        editorScriptPath = GetEditorScriptPath();
        GUILayout.Label("�༭�ű�·��:" + editorScriptPath);
        if (GUILayout.Button("�򿪱༭���ű�"))
        {
            OpenEditorScript();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("���PlayerPrefs"))
        {
            PlayerPrefs.DeleteAll(); ;
        }
        if (GUILayout.Button("ɾ�����ж�ʧ�Ľű����"))
        {
            DelAllMissScripts();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("�滻���ж�ʧ������ı�"))
        {
            ReplaceAllFont(true);
        }
        if (GUILayout.Button("�滻�����ı�������"))
        {
            ReplaceAllFont();
        }
        newfont = EditorGUILayout.ObjectField(newfont, typeof(Font)) as Font;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����Excel"))
        {
            ExportExcel2Json();
        }
        GUILayout.EndHorizontal();
    }

    string createViewName = "ViewNameYouWantCreate";
    private void OnGUIUI()
    {
        //UI
        EditorGUILayout.LabelField("<color=white>------------------- UI -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        createViewName = GUILayout.TextField(createViewName, 30, GUILayout.Width(170));
        if (GUILayout.Button("����View"))
        {
            EditorViewCreater.CreateView(createViewName);
        }
        if (GUILayout.Button("����UI"))
        {
            EditorExportUI.ExportViewUI();
        }
        GUILayout.EndHorizontal();
    }
    private void OnGUIUIConfig()
    {
        //UI����
        EditorGUILayout.LabelField("<color=white>------------------- ���� -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("��View�����ļ�"))
        {
            OpenUIViewConfig();
        }
        if (GUILayout.Button("����View����"))
        {
            ExportViewConfig();
        }
        if (GUILayout.Button("��Excel�����ļ���"))
        {
            string path = Directory.GetParent(Application.dataPath).FullName + @"\Product\StaticData";
            System.Diagnostics.Process.Start("Explorer.exe", path);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("�򿪶���������"))
        {
            string langName = Application.systemLanguage.ToString();
            if (Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                langName = SystemLanguage.Chinese.ToString();
            }
            string path = Application.streamingAssetsPath + $"/Lang/{langName}.json";
            Debug.Log(path);
            EditorUtility.OpenWithDefaultApp(path);
        }
        if (GUILayout.Button("�����Ժ���Ӣ"))
        {
            EditorTranslate.OnClickTranslateLanguage();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("���ñ�������"))
            {
                ConfigMgr.Ins.LoadAllConfig();
            }
            if (GUILayout.Button("����Զ������"))
            {
                ConfigMgr.Ins.LoadAllConfig(false);
            }
        }
        GUILayout.EndHorizontal();
    }

    public static string GetLangPath(SystemLanguage language)
    {
        return Application.streamingAssetsPath + $"/Lang/{language}.json";
    }

    bool audit = false;
    private void OnGUIUIPlaying()
    {
        //����
        EditorGUILayout.LabelField("<color=white>------------------- ���� -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("ֹͣ��Ϸ Ctrl+P", commonLayout))
            {
                EditorCoroutineUtility.StartCoroutine(StopGame(), this);
            }
        }
        else
        {
            if (GUILayout.Button("������Ϸ Ctrl+P | Ctrl+R", commonLayout))
            {
                StartGame();
            }
        }
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("������Ϸ Ctrl+R", commonLayout))
            {
                ResetGame();
            }
        }
        GUILayout.EndHorizontal();
        if (EditorApplication.isPlaying)
        {
            GUILayout.BeginHorizontal();
            //Time.timeScale
            GUILayout.Label("��Ϸȫ���ٶ� x1 - x10", GUILayout.Width(130));
            gameTimeSpeed = GUILayout.HorizontalSlider(gameTimeSpeed, 1, 10, GUILayout.Width(this.position.width - 420));
            GUILayout.Label("x0.2 - x1", GUILayout.Width(50));
            gameTimeSpeed = GUILayout.HorizontalSlider(gameTimeSpeed, 0.2f, 1f, GUILayout.Width(100));
            if (gameTimeSpeed >= 1)
            {
                if ((gameTimeSpeed * 10) % 10 > 5)
                {
                    gameTimeSpeed = Mathf.CeilToInt(gameTimeSpeed);
                }
                else
                {
                    gameTimeSpeed = Mathf.FloorToInt(gameTimeSpeed);
                }
            }
            Time.timeScale = gameTimeSpeed;
            GUIStyle timeScaleStyle = new GUIStyle() { fontSize = 14, alignment = TextAnchor.MiddleLeft };
            EditorGUILayout.LabelField("<color=#02FF19>x" + gameTimeSpeed.ToString("f2") + "</color>", timeScaleStyle, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GameSettingStatic.ResLog = GUILayout.Toggle(GameSettingStatic.ResLog, "��Դlog");
            audit = GUILayout.Toggle(audit, "���ģʽ");
            GUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void OnGUIUIOther()
    {
        GUILayout.Label("<color=white>------------------- ���� -------------------</color>", titleLabelStyle, GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Hierarchy�Ҽ�->BM����", GUILayout.Width(150));
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// ����
    /// </summary>
    private void OnGUIDebug()
    {
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
    }

    [MenuItem("��������/���� %R")]
    static void ResetGame()
    {
        if (Application.isPlaying)
        {
            EditorWindow.GetWindow<EditorDevTools>()._ResetGame();
        }
        else
        {
            StartGame();
        }
    }
    /// <summary>
    /// ���д��ļ�ʱ˵������������Ҫ����
    /// </summary>
    readonly static string ReStartGameFile = "temp_ReStartGameFile";
    void _ResetGame()
    {
        string tempFilePath = Directory.GetParent(Application.dataPath).FullName + ReStartGameFile;
        File.WriteAllText(tempFilePath, string.Empty);
        EditorCoroutineUtility.StartCoroutine(ReStart(), this);
    }

    /// <summary>
    /// �˱�ǿ����ýű��ڱ�����ڵ���һ��
    /// </summary>
    [DidReloadScripts]
    public static void OnCompileScripts()
    {
        if (!Application.isEditor || EditorApplication.isPlaying)
            return;
        string tempFilePath = Directory.GetParent(Application.dataPath).FullName + ReStartGameFile;
        if (File.Exists(tempFilePath))
        {
            File.Delete(tempFilePath);
            StartGame();
        }
    }

    public IEnumerator StopGame()
    {
        EditorApplication.isPlaying = false;
        EditorUtility.DisplayProgressBar("����", "�ȴ�����ֹͣ", 0.1f);

        yield return new EditorWaitForSeconds(0.1f);

        EditorUtility.ClearProgressBar();
        yield return null;
    }

    static IEnumerator PlayGame()
    {
        if (Application.isPlaying)
            yield break;
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity", OpenSceneMode.Single);
        EditorSceneManager.SetActiveScene(scene);
        EditorUtility.DisplayProgressBar("��", "GameScene����", 0.1f);
        yield return new EditorWaitForSeconds(0.1f);
        EditorUtility.ClearProgressBar();
        EditorApplication.isPlaying = true;
    }

    static IEnumerator ReStart()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(PlayGame());
            yield break;
        }
        while (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            yield return new EditorWaitForSeconds(0.1f);
            EditorCoroutineUtility.StartCoroutineOwnerless(PlayGame());
        }
    }

    public static void StartGame()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(PlayGame());
        }
    }

    [MenuItem("GameObject/BM����/ֻ ��ʾ&&����������", priority = 0)]
    public static void PickingAndIsolateObj()
    {
        Object obj = Selection.activeObject;
        if (obj == null)
        {
            return;
        }
        //��ʾ
        GameObject go = obj as GameObject;
        //���
        HideAllGameObject();
        SceneVisibilityManager.instance.EnablePicking(go, true);
        SceneVisibilityManager.instance.Show(go, true);
    }

    [MenuItem("GameObject/BM����/ֻ��ʾ���һ��NormalUI", priority = 1)]
    public static void OnlyShowLastNormalUI()
    {
        OnlyShowLastUI(ViewLayer.NormalUI.ToString());
    }

    [MenuItem("GameObject/BM����/ֻ��ʾ���һ��PopUpUI", priority = 2)]
    public static void OnlyShowLastPopUI()
    {
        OnlyShowLastUI(ViewLayer.PopUI.ToString());
    }

    [MenuItem("GameObject/BM����/ֻ��ʾ���һ��TipsUI", priority = 3)]
    public static void OnlyShowLastTipsUI()
    {
        OnlyShowLastUI(ViewLayer.TipsUI.ToString());
    }

    public static void OnlyShowLastUI(string _rootName)
    {
        GameObject uiRoot = GameObject.Find(typeof(UIMgr).ToString());
        if (uiRoot == null)
        {
            return;
        }
        Transform tf = uiRoot.transform.Find(_rootName);
        if (tf == null)
        {
            return;
        }
        if (tf.childCount <= 0)
        {
            return;
        }

        GameObject uigo = null;
        for (int i = tf.childCount - 1; i >= 0; i--)
        {
            var tmp = tf.GetChild(i).gameObject;
            if (!tmp.activeSelf)
            {
                continue;
            }
            uigo = tf.GetChild(i).gameObject;
            break;
        }
        if (uigo == null)
        {
            return;
        }
        HideAllGameObject();
        SceneVisibilityManager.instance.EnablePicking(uigo, true);
        SceneVisibilityManager.instance.Show(uigo, true);
        //Hierarchy���ѡ�������
        EditorGUIUtility.PingObject(uigo);
        Selection.activeGameObject = uigo;
    }


    [MenuItem("GameObject/BM����/���� ��ʾ&&���", priority = 999)]
    public static void PickingAndIsolateReset()
    {
        ShowAllGameObject();
    }

    /// <summary>
    /// ��ʾ�Ϳɵ�����е�����
    /// </summary>
    private static void ShowAllGameObject()
    {
        foreach (GameObject item in FindObjectsOfType<GameObject>())
        {
            SceneVisibilityManager.instance.Show(item, true);
            SceneVisibilityManager.instance.EnablePicking(item, true);
        }
    }

    /// <summary>
    /// ���غͲ��ɵ�����е�����
    /// </summary>
    private static void HideAllGameObject()
    {
        foreach (GameObject item in FindObjectsOfType<GameObject>())
        {
            SceneVisibilityManager.instance.Hide(item, true);
            SceneVisibilityManager.instance.DisablePicking(item, true);
        }
    }

    /// <summary>
    /// �򿪴˽ű�
    /// </summary>
    private static void OpenEditorScript()
    {
        editorScriptPath = GetEditorScriptPath();
        Debug.Log("�򿪽ű�" + editorScriptPath);
        EditorUtility.OpenWithDefaultApp(editorScriptPath);
    }

    /// <summary>
    /// ɾ�����ж�ʧ�Ľű����
    /// </summary>
    public static void DelAllMissScripts()
    {
        Action<GameObject, string> action = (GameObject _ui, string _uiPath) =>
        {
            int missNum = 0;
            foreach (var trans in _ui.GetComponentsInChildren<Transform>(true))
            {
                missNum += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(trans.gameObject);
            }
            if (missNum > 0)
            {
                PrefabUtility.SaveAsPrefabAsset(_ui, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(_ui));
                Debug.Log(string.Format("{0}ɾ��{1}����ʧ�ű�", _uiPath, missNum));
            }
        };
        ForeachAllUIPrefab(action);
    }

    [SerializeField]
    static Font newfont;
    /// �滻���ж�ʧ������ı����
    /// </summary>
    /// </summary>
    /// <param name="_onlyMissFont">ֻ�ж�ʧ������ı�</param>
    public static void ReplaceAllFont(bool _onlyMissFont = false)
    {
        if (newfont == null)
        {
            EditorUtility.DisplayDialog("��ʾ", "������������", "ȷ��");
            return;
        }
        Action<GameObject, string> action = (GameObject _ui, string _uiPath) =>
        {
            int textNum = 0;
            foreach (var text in _ui.GetComponentsInChildren<Text>(true))
            {
                if (_onlyMissFont && text.font != null)
                {
                    continue;
                }
                textNum++;
                text.font = newfont;
            }
            if (textNum > 0)
            {
                PrefabUtility.SaveAsPrefabAsset(_ui, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(_ui));
                Debug.Log($"{_uiPath}�滻{textNum}���ı�");
            }
        };
        ForeachAllUIPrefab(action);
    }

    /// <summary>
    /// ��������UIԤ����
    /// </summary>
    public static void ForeachAllUIPrefab(Action<GameObject, string> _action)
    {
        List<string> PrefabPath = new List<string>();
        Action<string, string> FindPrefabPath = (_resDirPath, prefix) =>
        {
            foreach (var filePath in Directory.GetFiles(_resDirPath + prefix, "*.prefab", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(filePath);
                fileName = fileName.Replace(".prefab", string.Empty);
                PrefabPath.Add(prefix + fileName);
            }
        };
        FindPrefabPath(Application.dataPath + "/Resources/", "View/");
        FindPrefabPath(Application.dataPath + "/Framework/Resources/", "View/");
        foreach (var path in PrefabPath)
        {
            GameObject ui = PrefabUtility.InstantiatePrefab(Resources.Load(path) as GameObject) as GameObject;
            _action(ui, path);
            DestroyImmediate(ui);
        }
    }

    /// <summary>
    /// Excel����Json
    /// </summary>
    public static void ExportExcel2Json()
    {
        try
        {
            string batPath = Directory.GetParent(Application.dataPath).FullName;
            batPath += "/Product/excel2json/excel2json.bat";
            if (!File.Exists(batPath))
            {
                EditorUtility.DisplayDialog("����", "û���ҵ��ļ�" + batPath, "ȷ��");
                return;
            }
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
            pro.WaitForExit();
            Debug.Log("�������->Resources/Json/");
        }
        catch (Exception ex)
        {
            Debug.LogError("ִ��ʧ�� ����ԭ��:" + ex.Message);
        }
    }

    /// <summary>
    /// ��ȡUIView����Yaml�ļ�·��
    /// </summary>
    /// <returns></returns>
    public string GetUIViewConfigPath()
    {
        return Application.dataPath + "/Framework/Resources/Config/UIView.yaml";
    }

    /// <summary>
    /// ��ȡUIView����Yaml�ļ�·��
    /// </summary>
    /// <returns></returns>
    public string GetUIViewTemplatePath()
    {
        return Application.dataPath + "/Framework/Script/Config/UIViewGenTemplate.txt";
    }

    /// <summary>
    /// ��ȡUIView����Yaml�ļ�·��
    /// </summary>
    /// <returns></returns>
    public string GetUIViewUIViewGenPath()
    {
        return Application.dataPath + "/Framework/Script/Config/UIViewGen.cs";
    }

    /// <summary>
    /// ��View��yaml�����ļ�
    /// </summary>
    public void OpenUIViewConfig()
    {
        EditorUtility.OpenWithDefaultApp(GetUIViewConfigPath());
    }

    /// <summary>
    /// ����View����
    /// </summary>
    public void ExportViewConfig()
    {
        string configPath = GetUIViewConfigPath();
        if (!File.Exists(configPath))
        {
            Debug.LogError("�Ҳ���UIView.yaml�ļ�");
            return;
        }
        string config = File.ReadAllText(configPath);
        var deserializer = new DeserializerBuilder()
              .WithNamingConvention(CamelCaseNamingConvention.Instance)
              .Build();
        UIViewConfig UIViewConfig = deserializer.Deserialize<UIViewConfig>(config);
        Utility.Dump(UIViewConfig);
        //����ģ��
        string template = File.ReadAllText(GetUIViewTemplatePath());
        Template temp = Template.Parse(template);
        List<object> layerConfigList = new List<object>();
        foreach (var item in UIViewConfig.layer)
        {
            layerConfigList.Add(new { layerComment = item.Value.comment, layerVal = item.Value.order, layerName = item.Key });
        }
        List<object> viewList = new List<object>();
        foreach (var item in UIViewConfig.view)
        {
            viewList.Add(new { viewName = item.Key });
        }
        Hash hash = Hash.FromAnonymousObject(new { layer = layerConfigList, view = viewList });
        string result = temp.Render(hash);
        File.WriteAllText(GetUIViewUIViewGenPath(), result);
        AssetDatabase.Refresh();
    }
}
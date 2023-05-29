using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using Object = UnityEngine.Object;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;
using UnityEditor.SceneManagement;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEngine.UI;

public class EditorViewCreater
{
    static Scene currScene;
    const string tempViewCreater = "/._tempViewCreater.txt";
    /// <summary>
    /// ����ʾ��UI
    /// </summary>
    public static void CreateView(string _viewName)
    {
        if (!CheckRegular(_viewName))
            return;
        CreateViewScene(_viewName);
        CreateViewScript(_viewName);
        ///����CreateViewScript֮�󣬽ű�һ�������ɹ�����ʱ����Ҫ�ر�����룬�ȱ�����ɺ����OnCompileScripts
        CreateViewPrefab(_viewName);
        //view.AddComponent(Type.GetType("UnityEngine.Rigidbody, UnityEngine.PhysicsModule"));
        //view.AddComponent(Type.GetType("DebugView, Assembly-CSharp"));
        Debug.Log("����View--->" + _viewName);
    }

    /// <summary>
    /// �˱�ǿ����ýű��ڱ�����ڵ���һ��
    /// </summary>
    [DidReloadScripts]
    public static void OnCompileScripts()
    {
        if (!Application.isEditor || EditorApplication.isPlaying)
            return;
        string tempFilePath = Directory.GetParent(Application.dataPath).FullName + tempViewCreater;
        if (!File.Exists(tempFilePath))
            return;
        string viewName = File.ReadAllText(tempFilePath);
        File.Delete(tempFilePath);
        GameObject prefab = GameObject.Find(viewName);
        AddScriptAndSavePrefab(viewName, prefab);
        EditorExportUI.ExportViewUI(prefab);
        EditorSceneManager.SaveOpenScenes();
    }

    public static Scene CreateViewScene(string _viewName)
    {
        string templateScenePath = $"Assets/Scenes/TemplateScene.unity";
        string scenePath = $"Assets/Scenes/{_viewName}.unity";
        AssetDatabase.CopyAsset(templateScenePath, scenePath);
        currScene = EditorSceneManager.OpenScene(scenePath);
        return currScene;
    }

    public static GameObject CreateViewPrefab(string _viewName)
    {
        if (Resources.Load<GameObject>(UIMgr.uiPrefabPath + _viewName))
        {
            EditorUtility.DisplayDialog("����", $"�Ѵ���ͬ��Ԥ����--->{UIMgr.uiPrefabPath + _viewName}", "ok");
            return null;
        }
        GameObject view = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>(UIMgr.uiPrefabPath + "TemplateView")) as GameObject;
        PrefabUtility.UnpackPrefabInstance(view, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        view.name = _viewName;
        view.transform.SetAsLastSibling();
        Canvas canvas = view.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        CanvasScaler canvasScaler = view.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        string tempFile = Directory.GetParent(Application.dataPath).FullName + tempViewCreater;
        File.WriteAllText(tempFile, _viewName);
        return view;
    }

    static GameObject AddScriptAndSavePrefab(string _viewName, GameObject _view)
    {
        try
        {
            _view.AddComponent(Type.GetType($"{_viewName}, Assembly-CSharp"));
        }
        catch (Exception)
        {
            Debug.Log($"������{_viewName}ʧ��", _view);
            throw;
        }
        string sceneName = EditorSceneManager.GetActiveScene().name;
        GameObject viewGo = GameObject.Find(sceneName);
        string relativePath = $"/Resources/View/";
        string prefabPath = Application.dataPath + relativePath + sceneName + ".prefab";
        //if (File.Exists(prefabPath))
        //{
        //    EditorUtility.DisplayDialog("����", $"�Ѵ���ͬ��Ԥ����--->{prefabPath}", "ok");
        //    return null;
        //}
        Debug.Log("����ViewԤ����" + prefabPath, viewGo);
        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(viewGo, prefabPath, InteractionMode.AutomatedAction);
        return prefab;
    }



    public static bool CreateViewScript(string _viewName)
    {
        string relativePath = $"/Script/View/{_viewName}.cs";
        string viewPath = Application.dataPath + relativePath;
        if (File.Exists(viewPath))
        {
            EditorUtility.DisplayDialog("����", $"�Ѵ���ͬ���ű�--->{viewPath}", "ok");
            return false;
        }
        string viewTemp = File.ReadAllText(GetAutoCreateViewTemplatePath());
        string viewScript = viewTemp.Replace("#ViewName#", _viewName);
        File.WriteAllText(viewPath, viewScript);
        Debug.Log("����View�ű�" + viewPath);
        AssetDatabase.Refresh();
        return true;
    }

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="_viewName"></param>
    /// <returns></returns>
    public static bool CheckRegular(string _viewName)
    {
        if (string.IsNullOrEmpty(_viewName))
        {
            EditorUtility.DisplayDialog("����", "��ǰ��������������Ҫ������View", "ok");
            return false;
        }
        foreach (char item in _viewName)
        {
            if ((!char.IsLetter(item) || char.ToLower(item) < 'a' || char.ToLower(item) > 'z') && item != '_')
            {
                EditorUtility.DisplayDialog("����", "���������Ϲ���", "ok");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ��ȡ�Զ�ҳ���ģ���ļ�·��
    /// </summary>
    /// <returns></returns>
    public static string GetAutoCreateViewTemplatePath()
    {
        return Application.dataPath + "/Framework/Script/View/AutoCreateViewTemplate.txt";
    }
}

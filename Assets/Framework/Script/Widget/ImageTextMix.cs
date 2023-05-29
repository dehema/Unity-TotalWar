using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//˼·����
//1.����ԭ�ı��ַ��Ļ����ϼ�һ���ո񣬻�ȡһ���ո���ռ�Ŀ�ȣ��е��ı����������bestFit�ֶΣ����Լ�һ���ո�ᵼ�¼�����Ŀ��С��������ȣ���ʵ��������Ժ��Բ��ơ�
//2.�滻���е�ռλ�ַ�Ϊ��С�����������Ŀո��������ͼռ30�����أ���������ı����������һ���ո��20���أ����滻Ϊ�����ո�ͬʱ��¼���滻�ո������
//3.���ݿո�������λ������ͼƬ��ͼƬ��λ���� �����ո�Ĺ��λ�ã����Ͻǣ�+��ȣ��ո�����*�ո���/2��-�߶ȣ����и߶�/2��

//֮ǰ����������ͼƬ��λ����һ��һ�����ɵģ���һ��ͼƬ����֮���滻�Ŀո��ַ��ͻᵼ�������ı���������øı䣬Ӧ�ð����ݴ���ã����һ������ͼƬ

/// <summary>
/// ͼ�Ļ�Ͻű�����Ҫ���ͼ�����úͶ������ı���
/// </summary>
[RequireComponent(typeof(Text))]
public class ImageTextMix : MonoBehaviour
{
    Text text;
    public string mixID;
    ImageTextMixUnitConfig config;
    RectTransform rect;
    string textStr;
    //�ո���
    float spaceUnitLenth;
    TextGenerator generator;
    int spaceNum;
    List<int> replaceStrIndex = new List<int>();

    List<GameObject> createImages = new List<GameObject>();

    private void Awake()
    {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        config = ConfigMgr.Ins.imageTextMix.Mix[mixID];
        textStr = LangMgr.Ins.Get(config.tid);
        GetSpaceUnitWidth();
        InitGenerator();
        InitStr();
        CreateImages();
    }

    private void GetSpaceUnitWidth()
    {
        string _textStr = " " + textStr.Remove(0, 1);
        generator = new TextGenerator();
        var size = rect.rect.size;
        var settings = text.GetGenerationSettings(size);
        generator.Populate(_textStr, settings);
        spaceUnitLenth = generator.characters[0].charWidth;
    }

    void InitStr()
    {
        replaceStrIndex.Clear();
        string replaceStr;
        for (int i = 0; i < config.imgConfigs.Count; i++)
        {
            TIMix_Image imgConfig = config.imgConfigs[i];
            int charIndex = 0;
            replaceStr = GetReplaceStr(imgConfig, i, ref charIndex);
            string spaceIndexStr = "{" + i + "}";
            textStr = textStr.Replace(spaceIndexStr, replaceStr);
            InitGenerator();
            if (string.IsNullOrEmpty(replaceStr)) continue;
            replaceStrIndex.Add(charIndex);
        }
        text.text = textStr;
    }

    private void InitGenerator()
    {
        generator = new TextGenerator();
        var size = rect.rect.size;
        var settings = text.GetGenerationSettings(size);
        generator.Populate(textStr, settings);
    }

    private string GetReplaceStr(TIMix_Image _imgConfig, int _index, ref int charIndex)
    {
        charIndex = textStr.IndexOf("{" + _index + "}");
        if (generator.characters.Count <= charIndex)
        {
            return "";
        }
        //����ո����� 
        spaceNum = Mathf.CeilToInt(_imgConfig.width / spaceUnitLenth);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < spaceNum; i++)
        {
            sb.Append(" ");
        }
        return sb.ToString();
    }

    void CreateImages()
    {
        foreach (var item in createImages)
        {
            Destroy(item);
        }
        createImages.Clear();
        for (int i = 0; i < replaceStrIndex.Count; i++)
        {
            int charIndex = replaceStrIndex[i];
            TIMix_Image _imgConfig = config.imgConfigs[i];
            //����ͼƬ
            GameObject go = Tools.Ins.Create2DGo("img1", transform);
            createImages.Add(go);
            Image img = go.AddComponent<Image>();
            img.preserveAspect = true;
            img.sprite = Resources.Load<Sprite>(_imgConfig.imagePath);
            RectTransform imgRect = go.GetComponent<RectTransform>();
            imgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _imgConfig.width);
            imgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _imgConfig.height);
            //��ȡλ��
            Vector2 cursorPos = generator.characters[charIndex].cursorPos;
            cursorPos += new Vector2(spaceNum * (spaceUnitLenth / 2), -generator.lines[0].height / 2);
            imgRect.anchoredPosition = cursorPos;
        }
    }
}

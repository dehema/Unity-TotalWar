using Codice.Utils;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using static UnityEditor.PlayerSettings.Switch;
using Random = UnityEngine.Random;

public class EditorTranslate : MonoBehaviour
{

    /// <summary>
    /// �������
    /// </summary>
    public static void OnClickTranslateLanguage()
    {
        Dictionary<string, string> chineseDict = ReadChineseJson(SystemLanguage.Chinese);
        Dictionary<string, string> englishDict = ReadChineseJson(SystemLanguage.English);
        List<string> transLangKey = new List<string>();
        StringBuilder transStr = new StringBuilder();
        foreach (var item in chineseDict)
        {
            if (!englishDict.ContainsKey(item.Key))
            {
                transLangKey.Add(item.Key);
            }
        }
        foreach (var item in transLangKey)
        {
            transStr.Append(chineseDict[item]);
            if (transLangKey.Last() != item)
            {
                transStr.Append("\n");
            }
        }
        string transTxt = transStr.ToString();
        //ִ�з���
        List<string> transVal = TransStr(transTxt);
        if (transLangKey.Count != transVal.Count)
        {
            Debug.LogError("�������key��val�������Բ���");
            return;
        }
        //��䷭��������
        for (int i = 0; i < transLangKey.Count; i++)
        {
            string key = transLangKey[i];
            string val = transVal[i];
            englishDict[key] = val;
        }
        //���� �������ı�һ��
        Dictionary<string, string> newEnglishDict = new Dictionary<string, string>();
        foreach (var item in chineseDict)
        {
            newEnglishDict.Add(item.Key, englishDict[item.Key]);
        }
        string transFileTxt = JsonConvert.SerializeObject(newEnglishDict);
        File.WriteAllText(EditorDevTools.GetLangPath(SystemLanguage.English), transFileTxt);
        Debug.Log("�������");
    }

    static Dictionary<string, string> ReadChineseJson(SystemLanguage language)
    {
        string path = EditorDevTools.GetLangPath(language);
        string content = File.ReadAllText(path);
        Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        return dict;
    }

    /// <summary>
    /// �����ַ���
    /// </summary>
    static List<string> TransStr(string q)
    {
        // Դ����
        string from = "zh";
        // Ŀ������
        string to = "en";
        //appId
        string appId = "20230925001829523";
        //�����
        string salt = Random.Range(0, 100000).ToString();
        // ��Կ
        string secretKey = "CLJf59XylJlwYqwE35TW";
        string sign = EncryptString(appId + q + salt + secretKey);
        string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
        url += "q=" + HttpUtility.UrlEncode(q);
        url += "&from=" + from;
        url += "&to=" + to;
        url += "&appid=" + appId;
        url += "&salt=" + salt;
        url += "&sign=" + sign;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "text/html;charset=UTF-8";
        request.UserAgent = null;
        request.Timeout = 6000;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        string retString = myStreamReader.ReadToEnd();
        myStreamReader.Close();
        myResponseStream.Close();
        Console.ReadLine();
        BaiduTranslate baiduTranslate = JsonConvert.DeserializeObject<BaiduTranslate>(retString);
        List<string> strList = new List<string>();
        foreach (var item in baiduTranslate.trans_result)
        {
            strList.Add(item.dst);
        }
        return strList;
    }

    // ����MD5ֵ
    public static string EncryptString(string str)
    {
        MD5 md5 = MD5.Create();
        // ���ַ���ת�����ֽ�����
        byte[] byteOld = Encoding.UTF8.GetBytes(str);
        // ���ü��ܷ���
        byte[] byteNew = md5.ComputeHash(byteOld);
        // �����ܽ��ת��Ϊ�ַ���
        StringBuilder sb = new StringBuilder();
        foreach (byte b in byteNew)
        {
            // ���ֽ�ת����16���Ʊ�ʾ���ַ�����
            sb.Append(b.ToString("x2"));
        }
        // ���ؼ��ܵ��ַ���
        return sb.ToString();
    }
}

public class BaiduTranslate
{
    public string from;
    public string to;
    public BaiduTransResult[] trans_result;
}

public class BaiduTransResult
{
    public string src;
    public string dst;

}
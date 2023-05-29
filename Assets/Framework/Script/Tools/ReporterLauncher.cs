using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReporterLauncher : MonoBehaviour
{
    void Start()
    {
        //���������ݷ���
        Timer.Ins.SetTimeOut(() =>
        {
            bool enable = Utility.IsDebug && !Application.isEditor;
            if (enable)
            {
                Reporter reporter = gameObject.GetComponent<Reporter>();
                reporter.Initialize();
            }
            Debug.Log("ReporterLauncher����" + enable);
        }, 5);
    }
}

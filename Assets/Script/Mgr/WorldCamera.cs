using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
    public bool inited = false;
    public new Camera camera;
    private float cameraMoveLerpTime = 8f;
    private float cameraRotateLerpTime = 8f;
    private Vector3 cameraPositionTemp;
    //摄像机朝向的对象
    GameObject lookAtTarget;

    public void Init()
    {
        camera = GetComponent<Camera>();
        cameraPositionTemp = transform.position;
        //配置
        moveLimitX = new Vector2(-WorldMgr.Ins.worldSize.x, WorldMgr.Ins.worldSize.x);
        moveLimitY = new Vector2(-WorldMgr.Ins.worldSize.y - 10, WorldMgr.Ins.worldSize.y);
        moveSpeed = Utility.GetSetting<float>(SettingField.World.World_Camera_MoveSpeed);
        dragSpeed = Utility.GetSetting<float>(SettingField.World.World_Camera_DragSpeed);
        zoomMinHeightY = Utility.GetSetting_Vector2(SettingField.World.World_Camera_ZoomHeight).x;
        zoomMaxHeightY = Utility.GetSetting_Vector2(SettingField.World.World_Camera_ZoomHeight).y;
        zoomMinRotateX = Utility.GetSetting_Vector2(SettingField.World.World_Camera_ZoomRot).x;
        zoomMaxRotateX = Utility.GetSetting_Vector2(SettingField.World.World_Camera_ZoomRot).y;
        zoomScaleSpeed = Utility.GetSetting<float>(SettingField.World.World_Camera_ZoomSpeed);
        inited = true;
    }

    void Update()
    {
        if (!inited)
            return;
        Move();
        Zoom();
        LookAtTarget();
        cameraPositionTemp = new Vector3(Mathf.Clamp(cameraPositionTemp.x, moveLimitX[0], moveLimitX[1])
        , Mathf.Clamp(cameraPositionTemp.y, zoomMinHeightY, zoomMaxHeightY)
        , Mathf.Clamp(cameraPositionTemp.z, moveLimitY[0], moveLimitY[1]));
        float rotateX = (cameraPositionTemp.y - zoomMinHeightY) / (zoomMaxHeightY - zoomMinHeightY)
        * (zoomMaxRotateX - zoomMinRotateX) + zoomMinRotateX;
        transform.position = Vector3.Lerp(transform.position, cameraPositionTemp, cameraMoveLerpTime * Time.deltaTime);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(rotateX, 0, 0), cameraRotateLerpTime * Time.deltaTime);
    }

    [Header("镜头X轴区间")]
    [SerializeField] private Vector2 moveLimitX = new Vector2(-200, 200);
    [Header("镜头Y轴区间")]
    [SerializeField] private Vector2 moveLimitY = new Vector2(-200, 200);
    [Header("镜头移动速度")]
    [SerializeField] private float moveSpeed = 15;
    [Header("镜头拖拽速度")]
    [SerializeField] private float dragSpeed = 0.1f;
    private Vector3 oldMousePos;
    private void Move()
    {
        if (InputMgr.Ins.mouseModel != MouseModel.World)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            oldMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition != oldMousePos)
            {
                //拖拽停止追踪
                SetLookAtTarget(null);
            }
            Vector3 deltaPosition = Input.mousePosition - oldMousePos;
            Vector3 delta = new Vector2(deltaPosition.x, deltaPosition.y) * dragSpeed;
            cameraPositionTemp -= new Vector3(delta.x, 0, delta.y);
            oldMousePos = Input.mousePosition;
        }
    }

    [Header("镜头最小高度")]
    [SerializeField] private float zoomMinHeightY = 10;
    [Header("镜头最大高度")]
    [SerializeField] private float zoomMaxHeightY = 60;
    [Header("镜头缩放下限")]
    [SerializeField] private float zoomMinRotateX = 50;
    [Header("镜头缩放上限")]
    [SerializeField] private float zoomMaxRotateX = 70;
    Touch oldTouch1; //上次触摸点1(手指1)
    Touch oldTouch2; //上次触摸点2(手指2)
    [Header("镜头缩放速度")]
    [SerializeField] private float zoomScaleSpeed = 0.3f;
    private void Zoom()
    {
        if (InputMgr.Ins.mouseModel != MouseModel.World)
        {
            return;
        }
        cameraPositionTemp -= new Vector3(0, Input.mouseScrollDelta.y * 10f * zoomScaleSpeed, 0);
    }

    public void SetLookAtTarget(GameObject _lookAtTarget)
    {
        lookAtTarget = _lookAtTarget;
    }

    void LookAtTarget()
    {
        if (lookAtTarget == null)
            return;
        Vector3 targetPos = lookAtTarget.transform.position;
        //目标到相机的向量
        Vector3 offset = cameraPositionTemp - targetPos;
        cameraPositionTemp = new Vector3(targetPos.x, cameraPositionTemp.y, targetPos.z - transform.forward.z * Vector3.Distance(targetPos, cameraPositionTemp));
    }

    /// <summary>
    /// 根据鼠标位置移动相机(鼠标在屏幕边缘拖拽屏幕)
    /// </summary>
    public void MoveCameraByMousePos()
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }
        Vector2 dire2 = InputMgr.Ins.GetDireByMousePos();
        if (dire2 == Vector2.zero)
            return;
        SetLookAtTarget(null);
        Vector3 dire = new Vector3(dire2.x, 0, dire2.y);
        cameraPositionTemp += dire * Time.deltaTime * moveSpeed;
    }
}

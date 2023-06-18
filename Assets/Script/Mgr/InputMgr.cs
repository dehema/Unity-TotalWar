using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputMgr : MonoSingleton<InputMgr>
{
    Vector3 mouseDownPos;
    /// <summary>
    /// 修改鼠标光标样式时用的偏移值
    /// </summary>
    Vector2 mouseOffset = new Vector2(0, 0);
    CursorStyle cursorSytle = CursorStyle.common;
    /// <summary>
    /// 鼠标触发边界 左右下上
    /// </summary>
    public readonly Vector4 mouseBound = new Vector4(5, Screen.width - 10, 15, Screen.height - 5);
    public MouseModel mouseModel = MouseModel.World;

    public void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        CheckMouseInput();
        CheckMouseScreenBound();
        CheckKeyboardInput();
    }

    /// <summary>
    /// 检测键盘输入
    /// </summary>
    private void CheckKeyboardInput()
    {
        if (SceneMgr.Ins.IsWorld)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (WorldMgr.Ins.worldDate.timeSpeed == TimeSpeed.pause)
                {
                    WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.normal);
                }
                else
                {
                    WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.pause);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (PlayerMgr.Ins.playerScene != PlayerScene.world)
                    return;
                WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.pause);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (PlayerMgr.Ins.playerScene != PlayerScene.world)
                    return;
                WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.normal);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (PlayerMgr.Ins.playerScene != PlayerScene.world)
                    return;
                WorldMgr.Ins.worldDate.SetTimeSpeed(TimeSpeed.quick);
            }
        }
        else if (SceneMgr.Ins.IsBattleField)
        {
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BaseView escCloseView = UIMgr.Ins.GetEscView();
            if (escCloseView)
            {
                UIMgr.Ins.CloseView(escCloseView._viewName);
            }
            else
            {
                UIMgr.Ins.OpenView<EscView>();
            }
        }
    }

    /// <summary>
    /// 检测鼠标屏幕边缘
    /// </summary>
    private void CheckMouseScreenBound()
    {
        if (mouseModel != MouseModel.World)
        {
            return;
        }
        //鼠标图片
        Vector2 dire = GetDireByMousePos();
        if (dire == new Vector2(1, 1))
        {
            SetCursorType(CursorStyle.rightUp);
        }
        else if (dire == new Vector2(1, -1))
        {
            SetCursorType(CursorStyle.rightDown);
        }
        else if (dire == new Vector2(-1, 1))
        {
            SetCursorType(CursorStyle.leftUp);
        }
        else if (dire == new Vector2(-1, -1))
        {
            SetCursorType(CursorStyle.leftDown);
        }
        else if (dire.x == -1)
        {
            SetCursorType(CursorStyle.left);
        }
        else if (dire.x == 1)
        {
            mouseOffset = new Vector2(Screen.width - Input.mousePosition.x + 45, 20);
            SetCursorType(CursorStyle.right);
        }
        else if (dire.y == -1)
        {
            mouseOffset = new Vector2(20, Input.mousePosition.y + 30);
            SetCursorType(CursorStyle.down);
        }
        else if (dire.y == 1)
        {
            SetCursorType(CursorStyle.up);
        }
        else
        {
            SetCursorType(CursorStyle.common);
        }
    }

    /// <summary>
    /// 检测鼠标输入
    /// </summary>
    private void CheckMouseInput()
    {
        //点击事件
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(Input.mousePosition, mouseDownPos) < 50)
            {
                OnClick(Input.mousePosition);
            }
        }
    }

    public void OnClick(Vector3 _mousePos)
    {
        if (!SceneMgr.Ins.IsBattleField)
        {
            UIMgr.Ins.GetView<CursorEffectView>()?.ClickEffect(_mousePos);
        }
        if (mouseModel == MouseModel.World)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //如果是
                if (SceneMgr.Ins.IsWorld)
                {
                    WorldMgr.Ins?.OnClick(_mousePos);
                }
            }
        }
    }

    private void SetCursorType(CursorStyle _cursorStyle)
    {
        if (_cursorStyle != CursorStyle.common)
        {
            WorldMgr.Ins?.worldCamera.MoveCameraByMousePos();
        }
        if (cursorSytle != _cursorStyle)
        {
            cursorSytle = _cursorStyle;
            switch (cursorSytle)
            {
                case CursorStyle.left:
                    mouseOffset = new Vector2(10, 20);
                    break;
                case CursorStyle.up:
                    mouseOffset = new Vector2(20, 10);
                    break;
                case CursorStyle.leftUp:
                    mouseOffset = new Vector2(15, 15);
                    break;
                case CursorStyle.leftDown:
                    mouseOffset = new Vector2(20, 30);
                    break;
                case CursorStyle.rightUp:
                    mouseOffset = new Vector2(45, 15);
                    break;
                case CursorStyle.rightDown:
                    mouseOffset = new Vector2(40, 35);
                    break;
                case CursorStyle.common:
                    mouseOffset = Vector2.zero;
                    break;
            }
            SetCursorStyle(cursorSytle, mouseOffset.x, mouseOffset.y);
        }
    }

    /// <summary>
    ///  鼠标风格
    /// </summary>
    /// <param name="_cursorStyle"></param>
    /// <param name="_offstX"></param>
    /// <param name="_offstY"></param>
    public void SetCursorStyle(CursorStyle _cursorStyle, float _offstX = 0, float _offstY = 0)
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("UI/cursor/cursor_" + _cursorStyle), new Vector2(_offstX, _offstY), CursorMode.Auto);
    }

    /// <summary>
    /// 根据鼠标位置获取向量 [-1,1]
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDireByMousePos()
    {
        Vector2 dire = Vector2.zero;
        if (Input.mousePosition.x < 5)
        {
            dire.x = -1;
        }
        else if (Input.mousePosition.x > Screen.width - 10)
        {
            dire.x = 1;
        }
        if (Input.mousePosition.y < 15)
        {
            dire.y = -1;
        }
        else if (Input.mousePosition.y > Screen.height - 5)
        {
            dire.y = 1;
        }
        return dire;
    }

    /// <summary>
    /// 设置光标模式
    /// </summary>
    public void SetMouseModel(MouseModel _mouseModel)
    {
        //    if (mouseModel != _mouseModel)
        //    {
        //        return;
        //    }
        Debug.Log("设置鼠标格式为" + _mouseModel);
        mouseModel = _mouseModel;
        if (mouseModel == MouseModel.UI)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (mouseModel == MouseModel.World)
        {
            //世界地图 鼠标锁边
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (mouseModel == MouseModel.BattleField)
        {
            //大战场 鼠标锁边 准心居中
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
/// <summary>
/// 光标模式
/// </summary>
public enum MouseModel
{
    /// <summary>
    /// UI模式下变光标样式不变
    /// </summary>
    UI,
    /// <summary>
    /// 世界模式下光标会在屏幕边缘处改变样式
    /// </summary>
    World,
    /// <summary>
    /// 隐藏光标
    /// </summary>
    BattleField,
}

/// <summary>
/// 鼠标样式
/// </summary>
public enum CursorStyle
{
    common,
    left,
    right,
    up,
    down,
    leftUp,
    leftDown,
    rightUp,
    rightDown
}
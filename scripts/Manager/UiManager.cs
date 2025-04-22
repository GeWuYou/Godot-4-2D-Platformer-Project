using System.Collections.Generic;
using Godot;
using Godot42DPlatformerProject.scripts.Config;
using Godot42DPlatformerProject.scripts.UI;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class UiManager : Node, IRegisterAbleManager
{
    private readonly Stack<Node> _uiStack = new();

    /// <summary>
    /// 当前场景根节点
    /// </summary>
    private Node _currentSceneRoot;

    /// <summary>
    /// UI 根节点
    /// </summary>
    private CanvasLayer _uiRoot;

    public void Initialize()
    {
        _currentSceneRoot = GetTree().CurrentScene;
        _uiRoot = _currentSceneRoot.GetNodeOrNull<CanvasLayer>("UIRoot");
        // 将主页面压入栈
        PushUi(UiConfig.MainInterfaceScenePath);
    }

    /// <summary>
    /// 推入并显示新的 UI
    /// </summary>
    public void PushUi(string scenePath)
    {
        var uiScene = (PackedScene)GD.Load(scenePath);
        var newUi = uiScene.Instantiate();

        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Peek();
            if (topUi is CanvasItem canvasItem)
            {
                canvasItem.Visible = false;
            }
            
        }

        _uiRoot.AddChild(newUi);
        _uiStack.Push(newUi);

        if (newUi is IUiPage uiPage)
        {
            uiPage.OnPush(this);
        }
    }

    /// <summary>
    /// 推入并显示新的 UI
    /// </summary>
    public void PushUi(CanvasItem newUi)
    {
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Peek();
            if (topUi is CanvasItem canvasItem)
            {
                canvasItem.Visible = false;
            }
        }

        _uiRoot.AddChild(newUi);
        _uiStack.Push(newUi);

        if (newUi is IUiPage uiPage)
        {
            uiPage.OnPush(this);
        }
    }

  
    /// <summary>
    /// 弹出指定场景路径的UI界面。
    /// 如果UI堆栈为空，则直接返回。
    /// 如果堆栈顶部的UI界面路径与指定路径相同，则弹出该UI界面。
    /// </summary>
    /// <param name="scenePath">要弹出的UI界面的场景路径。</param>
    public void PopUi(string scenePath)
    {
        if (_uiStack.Count == 0) return;

        var topUi = _uiStack.Peek();
        if (scenePath.Equals(topUi.SceneFilePath))
        {
            PopUi();
        }
    }


    /// <summary>
    /// 弹出当前 UI，并显示上一个
    /// </summary>
    public void PopUi()
    {
        if (_uiStack.Count == 0) return;

        var topUi = _uiStack.Pop();
        topUi.QueueFree();

        if (_uiStack.Count <= 0) return;
        var previousU = _uiStack.Peek();
        if (previousU is CanvasItem canvasItem)
        {
            canvasItem.Visible = true;
        }

        if (previousU is IUiPage uiPage)
        {
            uiPage.OnReturn(this);
        }
    }

    /// <summary>
    /// 替换当前 UI
    /// </summary>
    public void ReplaceUi(string scenePath)
    {
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Pop();
            topUi.QueueFree();
        }

        PushUi(scenePath);
    }

    /// <summary>
    /// 替换当前 UI
    /// </summary>
    public void ReplaceUi(CanvasItem newUi)
    {
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Pop();
            topUi.QueueFree();
        }

        PushUi(newUi);
    }


    /// <summary>
    /// 清除所有 UI
    /// </summary>
    public void ClearAllUi()
    {
        while (_uiStack.Count > 0)
        {
            var ui = _uiStack.Pop();
            ui.QueueFree();
        }
    }
}
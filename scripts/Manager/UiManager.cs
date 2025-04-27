using System.Collections.Generic;
using Godot;
using Godot42DPlatformerProject.scripts.Config;
using Godot42DPlatformerProject.scripts.UI;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class UiManager : Node, IRegisterAbleManager
{
    /// <summary>
    /// 用于存储UI界面的栈
    /// </summary>
    private readonly Stack<Node> _uiStack = new();

    /// <summary>
    /// 当前场景根节点
    /// </summary>
    private Node _currentSceneRoot;

    /// <summary>
    /// UI 根节点
    /// </summary>
    private CanvasLayer _uiRoot;
    
    /// <summary>
    /// 玩家管理器，用于管理玩家相关数据
    /// </summary>
    private PlayerManager _playerManager;

    /// <summary>
    /// 暂停菜单
    /// </summary>
    [Export]
    [ExportCategory("暂停菜单")]
    private Control _pauseMenu;

    /// <summary>
    /// 初始化UI管理器
    /// </summary>
    public void Initialize()
    {
        // 获取当前场景的根节点
        _currentSceneRoot = GetTree().CurrentScene;
        // 获取UI根节点
        _uiRoot = _currentSceneRoot.GetNodeOrNull<CanvasLayer>("UIRoot");
        // 获取玩家管理器实例
        _playerManager = ServiceLocator.Resolve<PlayerManager>();
        // 将主页面压入栈
        PushUi(UiConfig.MainInterfaceScenePath);
    }

    /// <summary>
    /// 推入并显示新的 UI
    /// </summary>
    /// <param name="scenePath">UI场景路径</param>
    public void PushUi(string scenePath)
    {
        // 加载新的UI场景
        var uiScene = (PackedScene)GD.Load(scenePath);
        var newUi = uiScene.Instantiate();

        // 如果栈不为空，将栈顶UI隐藏
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Peek();
            if (topUi is CanvasItem canvasItem)
            {
                canvasItem.Visible = false;
            }
        }

        // 将新的UI添加到UI根节点和栈中
        _uiRoot.AddChild(newUi);
        _uiStack.Push(newUi);

        // 如果新的UI实现了IUiPage接口，调用其OnPush方法
        if (newUi is IUiPage uiPage)
        {
            uiPage.OnPush(this);
        }
    }

    /// <summary>
    /// 推入并显示新的 UI
    /// </summary>
    /// <param name="newUi">新的UI实例</param>
    public void PushUi(CanvasItem newUi)
    {
        // 如果栈不为空，将栈顶UI隐藏
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Peek();
            if (topUi is CanvasItem canvasItem)
            {
                canvasItem.Visible = false;
            }
        }

        // 将新的UI添加到UI根节点和栈中
        _uiRoot.AddChild(newUi);
        _uiStack.Push(newUi);

        // 如果新的UI实现了IUiPage接口，调用其OnPush方法
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

        // 弹出并销毁栈顶UI
        var topUi = _uiStack.Pop();
        topUi.QueueFree();

        // 如果栈不为空，显示栈顶UI
        if (_uiStack.Count > 0)
        {
            var previousU = _uiStack.Peek();
            if (previousU is CanvasItem canvasItem)
            {
                canvasItem.Visible = true;
            }

            // 如果栈顶UI实现了IUiPage接口，调用其OnReturn方法
            if (previousU is IUiPage uiPage)
            {
                uiPage.OnReturn(this);
            }
        }
    }

    /// <summary>
    /// 替换当前 UI
    /// </summary>
    /// <param name="scenePath">新的UI场景路径</param>
    public void ReplaceUi(string scenePath)
    {
        // 如果栈不为空，弹出并销毁栈顶UI
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Pop();
            topUi.QueueFree();
        }

        // 推入新的UI
        PushUi(scenePath);
    }

    /// <summary>
    /// 替换当前 UI
    /// </summary>
    /// <param name="newUi">新的UI实例</param>
    public void ReplaceUi(CanvasItem newUi)
    {
        // 如果栈不为空，弹出并销毁栈顶UI
        if (_uiStack.Count > 0)
        {
            var topUi = _uiStack.Pop();
            topUi.QueueFree();
        }

        // 推入新的UI
        PushUi(newUi);
    }

    /// <summary>
    /// 清除所有 UI
    /// </summary>
    public void ClearAllUi()
    {
        // 循环弹出并销毁所有UI
        while (_uiStack.Count > 0)
        {
            var ui = _uiStack.Pop();
            ui.QueueFree();
        }
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void GoToMainMenu()
    {
        // 获取当前玩家角色
        var player = _playerManager.GetCurrentCharacter();
        // 禁用当前玩家角色
        PlayerManager.DeactivateCharacter(player);
        // 回到主界面
        PushUi(UiConfig.MainInterfaceScenePath);
    }

    public void DisplayPauseMenu()
    {
        _pauseMenu.Visible = true;
    }
}

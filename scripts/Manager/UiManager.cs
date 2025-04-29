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
    private readonly Stack<CanvasItem> _uiStack = new();

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
        PushUi<CanvasItem>(UiConfig.MainInterfaceScenePath);
    }

    /// <summary>
    /// Push a UI by scene path and return the instance casted to T
    /// </summary>
    public T PushUi<T>(string scenePath) where T : CanvasItem
    {
        var packedScene = GD.Load<PackedScene>(scenePath);
        var newUi = packedScene.Instantiate<T>();
        PushInternal(newUi);
        return newUi;
    }

    /// <summary>
    /// Push a UI by instance and return it
    /// </summary>
    public T PushUi<T>(T newUi) where T : CanvasItem
    {
        PushInternal(newUi);
        return newUi;
    }

    private void PushInternal(CanvasItem newUi)
    {
        if (_uiStack.Count > 0)
            _uiStack.Peek().Visible = false;

        _uiRoot.AddChild(newUi);
        _uiStack.Push(newUi);

        if (newUi is IUiPage uiPage)
            uiPage.OnPush(this);
    }

    /// <summary>
    /// Pop the current UI and return the new top UI (or null)
    /// </summary>
    public CanvasItem PopUi()
    {
        if (_uiStack.Count == 0)
        {
            return null;
        }

        var topUi = _uiStack.Pop();
        topUi.QueueFree();

        if (_uiStack.Count <= 0)
        {
            return null;
        }

        var nextUi = _uiStack.Peek();
        nextUi.Visible = true;

        if (nextUi is IUiPage uiPage)
            uiPage.OnReturn(this);

        return nextUi;
    }

    /// <summary>
    /// Pop the UI only if it matches the scene path
    /// </summary>
    public CanvasItem PopUi(string scenePath)
    {
        if (_uiStack.Count == 0)
        {
            return null;
        }

        var topUi = _uiStack.Peek();
        return scenePath.Equals(topUi.SceneFilePath) ? PopUi() : topUi;
    }

    /// <summary>
    /// Peek the current top UI without modifying the stack
    /// </summary>
    public CanvasItem PeekUi()
    {
        return _uiStack.Count > 0 ? _uiStack.Peek() : null;
    }

    /// <summary>
    /// Peek the current top UI and cast to specified type
    /// </summary>
    public T PeekUi<T>() where T : CanvasItem
    {
        return PeekUi() as T;
    }

    /// <summary>
    /// Replace current UI with a new UI scene and return the new UI
    /// </summary>
    public T ReplaceUi<T>(string scenePath) where T : CanvasItem
    {
        if (_uiStack.Count <= 0)
        {
            return PushUi<T>(scenePath);
        }

        var topUi = _uiStack.Pop();
        topUi.QueueFree();

        return PushUi<T>(scenePath);
    }

    /// <summary>
    /// Replace current UI with a new UI instance and return it
    /// </summary>
    public T ReplaceUi<T>(T newUi) where T : CanvasItem
    {
        if (_uiStack.Count <= 0)
        {
            return PushUi(newUi);
        }

        var topUi = _uiStack.Pop();
        topUi.QueueFree();

        return PushUi(newUi);
    }

    /// <summary>
    /// Clear all UIs
    /// </summary>
    public void ClearAllUi()
    {
        while (_uiStack.Count > 0)
        {
            var ui = _uiStack.Pop();
            ui.QueueFree();
        }
    }

    /// <summary>
    /// Return to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        var player = _playerManager.GetCurrentCharacter();
        PlayerManager.DeactivateCharacter(player);
        ClearAllUi();
        PushUi<CanvasItem>(UiConfig.MainInterfaceScenePath);
    }
}
using Godot;
using Godot42DPlatformerProject.scripts.manager;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class SceneManager : Node, IRegisterAbleManager
{
    /// <summary>
    /// 当前场景根节点
    /// </summary>
    private Node _currentSceneRoot;

    /// <summary>
    /// UI 根节点
    /// </summary>
    private Node _uiRoot;

    /// <summary>
    /// 主菜单节点
    /// </summary>
    private Node _mainMenu;

    /// <summary>
    /// 选关容器节点
    /// </summary>
    private PanelContainer _levelContainer;

    /// <summary>
    /// 主菜单按钮容器节点
    /// </summary>
    private VBoxContainer _mainMenuButtons;

    /// <summary>
    /// 选关按钮容器节点
    /// </summary>
    private GridContainer _levelButtons;

    public void Initialize()
    {
        _currentSceneRoot = GetTree().CurrentScene;
        _uiRoot = _currentSceneRoot.GetNodeOrNull<Node>("UIRoot");
        // 获取主菜单
        _mainMenu = _uiRoot.GetNodeOrNull<Node>("MainMenu");
        _mainMenuButtons = _mainMenu.GetNodeOrNull<VBoxContainer>("MainMenuButtons");
        _levelContainer = _mainMenu.GetNodeOrNull<PanelContainer>("LevelContainer");
        _levelButtons = _levelContainer.GetNode<GridContainer>("VBoxContainer/ScrollContainer/LevelButtons");
        GD.Print("[SceneManager] 初始化完成");
    }

    /// <summary>
    /// 显示某个 UI 控件
    /// </summary>
    public void ShowUi(string nodePath)
    {
        var node = _uiRoot?.GetNodeOrNull<CanvasItem>(nodePath);
        if (node != null)
            node.Visible = true;
    }

    /// <summary>
    /// 隐藏某个 UI 控件
    /// </summary>
    public void HideUi(string nodePath)
    {
        var node = _uiRoot?.GetNodeOrNull<CanvasItem>(nodePath);
        if (node != null)
            node.Visible = false;
    }

    /// <summary>
    /// 显示选关 UI
    /// </summary>
    public void ShowLevelSelectionUi()
    {
        ShowUi("LevelContainer");
        HideUi("MainMenu");
    }

    /// <summary>
    /// 显示主菜单 UI
    /// </summary>
    public void ShowMainMenuUi()
    {
        ShowUi("MainMenu");
        HideUi("LevelContainer");
    }

    /// <summary>
    /// 切换完整场景
    /// </summary>
    public void ChangeScene(string scenePath)
    {
        GD.Print($"[SceneManager] 切换场景: {scenePath}");
        GetTree().ChangeSceneToFile(scenePath);
    }
}
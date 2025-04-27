using Godot;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.Manager;

namespace Godot42DPlatformerProject.scripts.UI;

public partial class PauseMenu : Control
{
    /// <summary>
    /// 恢复按钮，用于恢复游戏。
    /// </summary>
    [Export] [ExportCategory("恢复")] private Button _resumeButton;

    /// <summary>
    /// 重新开始按钮，用于重新开始当前关卡。
    /// </summary>
    [Export] [ExportCategory("重新开始")] private Button _restartButton;

    /// <summary>
    /// 设置按钮，用于打开设置菜单（尚未实现）。
    /// </summary>
    [Export] [ExportCategory("设置")] private Button _optionsButton;

    /// <summary>
    /// 返回主菜单按钮，用于返回到主菜单。
    /// </summary>
    [Export] [ExportCategory("主菜单")] private Button _mainMenuButton;

    /// <summary>
    /// 退出游戏按钮，用于退出游戏。
    /// </summary>
    [Export] [ExportCategory("退出游戏")] private Button _exitGameButton;

    /// <summary>
    /// 级别管理器，用于管理游戏中的级别操作。
    /// </summary>
    private LevelManager _levelManager;

    /// <summary>
    /// UI管理器，用于管理用户界面的显示和交互。
    /// </summary>
    private UiManager _uiManager;

    /// <summary>
    /// 游戏管理器，用于管理游戏的整体逻辑和状态。
    /// </summary>
    private GameManager _gameManager;

    /// <summary>
    /// 当节点准备就绪时调用。
    /// </summary>
    public override void _Ready()
    {
        // 隐藏当前界面或组件，以响应恢复操作
        Visible = false;
        // 设置处理模式为总是处理，以确保在恢复后能够立即响应
        ProcessMode = ProcessModeEnum.Always;
        _resumeButton.Pressed += OnResumeButtonPressed;
        _restartButton.Pressed += OnRestartButtonPressed;
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _exitGameButton.Pressed += OnExitGameButtonPressed;
        // 初始化级别管理器、UI管理器和游戏管理器
        _levelManager = ServiceLocator.Resolve<LevelManager>();
        _uiManager = ServiceLocator.Resolve<UiManager>();
        _gameManager = ServiceLocator.Resolve<GameManager>();
        // 绑定按钮点击事件
    }
    
    /// <summary>
    /// 处理恢复按钮按下时的动作。
    /// </summary>
    private void OnResumeButtonPressed()
    {
        // 隐藏当前界面或组件，以响应恢复操作
        Visible = false;
        _gameManager.ChangeState(GameState.Playing);
    }

    /// <summary>
    /// 处理重新开始按钮按下时的动作。
    /// </summary>
    private void OnRestartButtonPressed()
    {
        // 首先执行恢复操作，以确保界面隐藏和处理模式设置
        OnResumeButtonPressed();
        // 重新加载当前级别，以便玩家可以从头开始
        _levelManager.ReLoadLevel();
    }

    /// <summary>
    /// 处理返回主菜单按钮按下时的动作。
    /// </summary>
    private void OnMainMenuButtonPressed()
    {
        // 恢复冻结状态
        OnResumeButtonPressed();
        _gameManager.ChangeState(GameState.MainMenu);
        // 退出当前关卡
        _levelManager.CleanUpOldLevel();
        // 返回主菜单
        _uiManager.GoToMainMenu();
    }

    /// <summary>
    /// 处理退出游戏按钮按下时的动作。
    /// </summary>
    private void OnExitGameButtonPressed()
    {
        // 退出游戏
        _gameManager.Quit();
    }
}
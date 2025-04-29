using Godot;
using Godot42DPlatformerProject.scripts.Config;
using Godot42DPlatformerProject.scripts.Extension;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.UI.Buttons;

namespace Godot42DPlatformerProject.scripts.UI;
public partial class MainMenuButtonContainer : VBoxContainer
{
    /// <summary>
    /// 游戏界面中的开始按钮
    /// </summary>
    private Button _startButton;

    /// <summary>
    /// 游戏界面中的继续按钮
    /// </summary>
    private Button _continueButton;

    /// <summary>
    /// 游戏界面中的等级选择按钮
    /// </summary>
    private Button _levelButton;

    /// <summary>
    /// 游戏界面中的选项设置按钮
    /// </summary>
    private Button _optionsButton;

    /// <summary>
    /// 游戏界面中的退出按钮
    /// </summary>
    private Button _exitButton;

    /// <summary>
    /// 包含等级选择界面的容器
    /// </summary>
    private PanelContainer _levelContainer;

    /// <summary>
    /// 包含多个等级按钮的网格容器
    /// </summary>
    [Export][ExportCategory("关卡按钮容器")]
    private GridContainer _levelItemButtonContainer;

    /// <summary>
    /// 游戏管理器，负责游戏的核心逻辑
    /// </summary>
    private GameManager _gameManager;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
        _startButton = GetNode<Button>("StartButton");
        _continueButton = GetNode<Button>("ContinueButton");
        _levelButton = GetNode<Button>("LevelButton");
        _optionsButton = GetNode<Button>("OptionButton");
        _exitButton = GetNode<Button>("ExitButton");
        var root = GetParent();
        _levelContainer = root.GetNode<PanelContainer>("LevelContainer");
        // 绑定选关按钮事件
        _levelButton.Pressed += OnLevelButtonPressed;
        _exitButton.Pressed += _gameManager.Quit;
    }

    /// <summary>
    ///  按下选择关卡按钮时触发
    /// </summary>
    private void OnLevelButtonPressed()
    {
        InitLevelItemButtons();
        // 失效并隐藏主菜单按钮列表
        this.SetActive(false);
        // 激活LevelContainer节点
        _levelContainer.SetActive(true);
    }
    

    /// <summary>
    /// 初始化等级项按钮
    /// </summary>
    private void InitLevelItemButtons()
    {
        if (_levelItemButtonContainer.GetChildCount() > 0)
        {
            return;
        }

        var buttonScene = (PackedScene)ResourceLoader.Load(UiConfig.LevelItemButtonScenePath);
        for (var i = 1; i <= _gameManager.LevelCount; i++)
        {
            // 实例化 LevelItemButton
            var levelButton = (LevelItemButton)buttonScene.Instantiate();
            levelButton.Init(i);
            // 将 LevelItemButton 添加到 LevelContainer
            _levelItemButtonContainer.AddChild(levelButton);
        }
    }
}

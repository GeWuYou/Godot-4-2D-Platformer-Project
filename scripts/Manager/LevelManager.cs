using Godot;
using Godot42DPlatformerProject.scripts.Config;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.Model;

namespace Godot42DPlatformerProject.scripts.Manager;

// LevelManager类负责关卡的加载、重载以及关卡内元素的管理
public partial class LevelManager : Node, IRegisterAbleManager
{
    // 玩家管理器，用于操作和管理玩家相关的信息和状态
    private PlayerManager _playerManager;

    // 关卡根节点，用于装载关卡场景
    public Node LevelRoot { get; private set; }

    // 游戏管理器，用于管理游戏的整体状态和数据
    private GameManager _gameManager;

    // 场景管理器，用于管理场景的切换和加载
    private SceneManager _sceneManager;

    // UI管理器，用于管理用户界面的显示和交互
    private UiManager _uiManager;

    // 标记是否正在加载关卡，以防止重复加载
    private bool _isLoadingLevel;

    /// <summary>
    /// 初始化LevelManager，主要用于初始化关卡根节点和获取必要的管理器引用
    /// </summary>
    public void Initialize()
    {
        // 获取当前场景
        var currentScene = GetTree().CurrentScene;
        // 通过服务定位器获取必要的管理器实例
        _gameManager = ServiceLocator.Resolve<GameManager>();
        _playerManager = ServiceLocator.Resolve<PlayerManager>();
        _sceneManager = ServiceLocator.Resolve<SceneManager>();
        _uiManager = ServiceLocator.Resolve<UiManager>();
        // 获取关卡根节点
        LevelRoot = currentScene.GetNode("LevelRoot");
    }

    /// <summary>
    /// 重新加载当前关卡。
    /// </summary>
    public void ReLoadLevel()
    {
        // 调用LoadLevel方法重新加载当前关卡。
        LoadLevel(_gameManager.CurrentLevel);
    }

    /// <summary>
    /// 加载指定的关卡，并作为子场景加载到当前场景中
    /// </summary>
    /// <param name="levelId">要加载的关卡ID</param>
    public void LoadLevel(int levelId)
    {
        GD.Print(System.Environment.StackTrace);
        if (_isLoadingLevel)
        {
            GD.Print($"[LevelManager] 忽略重复 LoadLevel({levelId}) 请求");
            return;
        }

        _gameManager.ChangeState(GameState.Playing);
        _isLoadingLevel = true;
        CallDeferred(nameof(DeferredLoadLevel), levelId);
    }

    /// <summary>
    /// 延迟执行关卡加载，以确保在正确的线程中执行场景操作
    /// </summary>
    /// <param name="levelId">要加载的关卡ID</param>
    public void DeferredLoadLevel(int levelId)
    {
        // 清理旧关卡
        CleanUpOldLevel();
        var player = _playerManager.GetCurrentCharacter();
        if (levelId > _gameManager.LevelCount)
        {
            // 切换状态
            _gameManager.ChangeState(GameState.MainMenu);
            // 回到主界面
            _uiManager.GoToMainMenu();
            return;
        }

        var scene = (PackedScene)ResourceLoader.Load($"res://scenes/level/level{levelId}.tscn");
        if (scene is null)
        {
            GD.PrintErr($"Level {levelId} scene not found!");
            return;
        }

        // 实例化关卡场景
        var newLevelInstance = scene.Instantiate();
        // 修改游戏管理器中的当前关卡ID
        _gameManager.SetLevel(levelId);

        // 加载后尝试读取关卡数据
        var data = newLevelInstance.GetNodeOrNull<LevelData>("LevelData");
        if (data != null)
        {
            var spawnPoint = data.GetPlayerInitStartPoint();
            PlayerManager.ActivateCharacter(player, spawnPoint);
            GD.Print($"[LevelManager] 玩家出生点：{spawnPoint}");
        }

        // 将新关卡作为子场景添加到关卡根节点
        LevelRoot.AddChild(newLevelInstance);
        // 压入暂停界面
        _uiManager.PushUi<CanvasItem>(UiConfig.PauseMenuScenePath);
        _isLoadingLevel = false;
    }

    /// <summary>
    /// 清理当前关卡，包括卸载关卡内的所有子节点
    /// </summary>
    public void CleanUpOldLevel()
    {
        // 卸载旧关卡
        foreach (var child in LevelRoot.GetChildren())
        {
            child.QueueFree();
        }
    }
}
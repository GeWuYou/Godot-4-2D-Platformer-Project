using Godot;
using Godot42DPlatformerProject.scripts.Config;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.Model;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class LevelManager : Node, IRegisterAbleManager
{
    private PlayerManager _playerManager;
    public Node LevelRoot { get; private set; }
    private GameManager _gameManager;
    private SceneManager _sceneManager;
    private UiManager _uiManager;


    public void Initialize()
    {
        var currentScene = GetTree().CurrentScene;
        _gameManager = ServiceLocator.Resolve<GameManager>();
        _playerManager = ServiceLocator.Resolve<PlayerManager>();
        _sceneManager = ServiceLocator.Resolve<SceneManager>();
        _uiManager = ServiceLocator.Resolve<UiManager>();
        LevelRoot = currentScene.GetNode("LevelRoot");
    }

    /// <summary>
    /// 加载指定的关卡，并作为子场景加载到当前场景中
    /// </summary>
    public void LoadLevel(int levelId)
    {
        CallDeferred(nameof(DeferredLoadLevel), levelId);
    }

    public void DeferredLoadLevel(int levelId)
    {
        // 卸载旧关卡
        foreach (var child in LevelRoot.GetChildren())
        {
            child.QueueFree();
        }
        var player = _playerManager.GetCurrentCharacter();
        if (levelId > _gameManager.LevelCount)
        {
            PlayerManager.DeactivateCharacter(player);
            // 回到主界面
            _uiManager.PushUi(UiConfig.MainInterfaceScenePath);
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
        // 弹出主界面
        _uiManager.PopUi(UiConfig.MainInterfaceScenePath);
    }
}
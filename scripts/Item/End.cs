using Godot;
using Godot42DPlatformerProject.scripts.MainCharacter;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.Manager;

namespace Godot42DPlatformerProject.scripts.Item;

public partial class End : Area2D
{
    private GameManager _gameManager;
    private LevelManager _levelManager;

    public override void _Ready()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
        _levelManager = ServiceLocator.Resolve<LevelManager>();
    }

    private void _on_body_entered(Node body)
    {
        if (body is not IPlayer)
        {
            return;
        }
        // 修改当前游戏管理器的关卡id
        var gameManagerCurrentLevel = _gameManager.CurrentLevel;
        // 加载下一关
        _levelManager.LoadLevel(++gameManagerCurrentLevel);
    }
}
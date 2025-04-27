using Godot;
using Godot42DPlatformerProject.scripts.MainCharacter;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.Manager;

namespace Godot42DPlatformerProject.scripts.Item;

public partial class FallResetArea : Area2D
{
    private bool _isReady;
    private LevelManager _levelManager;
    private GameManager _gameManager;

    [Export(PropertyHint.Range, "0.1,5,0.1")]
    private float _resetCooldownSeconds = 5.0f;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        _levelManager = ServiceLocator.Resolve<LevelManager>();
        _gameManager = ServiceLocator.Resolve<GameManager>();
        // 延迟激活，避免加载前误触
        GetTree().CreateTimer(0.5f).Timeout += () => _isReady = true;
    }

    private void OnBodyEntered(Node body)
    {
        // If the body entered is a player, load the current level again
        if (!_isReady || body is not IPlayer)
        {
            return;
        }

        GD.Print("[FallResetArea] 玩家掉落，准备重置关卡");
        _levelManager.ReLoadLevel();
    }
}
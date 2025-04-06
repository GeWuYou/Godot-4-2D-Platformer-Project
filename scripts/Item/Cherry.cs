using Godot;
using Godot42DPlatformerProject.scripts.MainCharacter;
using Godot42DPlatformerProject.scripts.manager;

namespace Godot42DPlatformerProject.scripts.Item;

public partial class Cherry : Area2D
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
    }

    private void _on_body_entered(Node2D body)
    {
        if (body is IPlayer)
        {
            _gameManager.AddScore();
            QueueFree();
        }
    }
}
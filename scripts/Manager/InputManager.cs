using Godot;
using Godot42DPlatformerProject.scripts.manager;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class InputManager : Node, IRegisterAbleManager
{
    private GameManager _gameManager;


    public void Initialize()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
        // 确保在游戏暂停时仍然能够处理输入
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel")) // 默认 Esc
        {
            switch (_gameManager.CurrentState)
            {
                // 如果游戏状态为“游戏进行中”时，切换为“暂停”状态
                case GameState.Playing:
                    _gameManager.ChangeState(GameState.Paused); // 👈 只调用状态切换
                    break;
                // 如果游戏状态为“暂停”时，切换为“游戏进行中”状态
                case GameState.Paused:
                    _gameManager.ChangeState(GameState.Playing);
                    break;
            }
        }

        // 更多按键处理逻辑可在此扩展
    }
}
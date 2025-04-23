using Godot;

namespace Godot42DPlatformerProject.scripts.Component;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Hit,
    Dead
}

/// <summary>
/// PlayerStateComponent
/// </summary>
public class PlayerStateComponent : IStateComponent<PlayerState>
{

    /// <summary>
    /// Current state of the player
    /// </summary>
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    public void ChangeState(PlayerState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        // GD.Print($"状态切换: {CurrentState} -> {newState}");
        CurrentState = newState;
    }
}
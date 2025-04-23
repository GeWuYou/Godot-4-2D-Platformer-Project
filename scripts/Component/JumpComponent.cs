

using Godot;


namespace Godot42DPlatformerProject.scripts.Component;

/// <summary>
/// 跳跃组件
/// </summary>
public class JumpComponent(CharacterBody2D body, float jumpVelocity = -900f)
{
    public bool CanJump { get; set; }
    public bool RequireGround = true;

    public void Jump(ref Vector2 velocity)
    {
        CheckJump();
        TryJump(ref velocity);
    }

    public void RequestJump()
    {
        CanJump = true;
    }
    public void CheckJump()
    {
        if (!Input.IsActionJustPressed("jump") || !body.IsOnFloor())
        {
            return;
        }
        GD.Print("跳跃");
        RequestJump();
    }
    public void TryJump(ref Vector2 velocity)
    {
        var isAllowed = !RequireGround || (body != null && body.IsOnFloor());
        if (!CanJump || !isAllowed)
        {
            return;
        }
        velocity.Y = jumpVelocity;
        // 如果有冷却或者跳跃次数限制，这里处理
        CanJump = false;
    }
}
using Godot;

namespace Godot42DPlatformerProject.scripts.Component;

/// <summary>
/// 平台碰撞组件
/// </summary>
public class PlatformCollisionComponent(CharacterBody2D body)
{
    /// <summary>
    ///  处理平台碰撞
    /// </summary>
    public void HandleCollisionControl()
    {
        // 处理下落
        if (Input.IsActionJustPressed("down") && body.IsOnFloor())
        {
            GD.Print("下落触发：禁用平台碰撞");
            body.SetCollisionMaskValue(2, false); // 关闭第二层检测
            body.GetTree().CreateTimer(0.2f).Timeout += () =>
            {
                GD.Print("恢复平台碰撞");
                body.SetCollisionMaskValue(2, true); // 恢复
            };
        }
    }
}
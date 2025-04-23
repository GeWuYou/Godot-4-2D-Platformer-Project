using Godot;

namespace Godot42DPlatformerProject.scripts.Component;

/// <summary>
/// 移动组件
/// </summary>
public partial class MoveComponent(float speed, float friction):Node
{
    /// <summary>
    /// 方向改变
    /// </summary>
    [Signal]
    public delegate void DirectionChangedEventHandler(bool facingRight);
    public void Move(ref Vector2 velocity, double delta)
    {
        // 获取输入方向
        var direction = Input.GetVector("left", "right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * speed;
            switch (direction.X)
            {
                case > 0:
                    EmitSignal(nameof(DirectionChanged), true); // 朝右
                    break;
                case < 0:
                    EmitSignal(nameof(DirectionChanged), false); // 朝左
                    break;
            }
        }
        else
        {
            // 根据角色当前速度和目标速度（0），在指定的速度变化率（Speed）下平滑地改变角色的水平速度
            velocity.X = Mathf.MoveToward(velocity.X, 0, friction * (float)delta);
        }
    }
}
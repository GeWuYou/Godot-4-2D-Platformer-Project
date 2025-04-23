
using Godot;

namespace Godot42DPlatformerProject.scripts.Component;

/// <summary>
/// 物理组件
/// </summary>
public class PhysicsComponent
{
    public void AddGravity(bool isOnFloor,ref Vector2 velocity,Vector2 gravity,float delta)
    {
        // 添加重力
        if (!isOnFloor)
        {
            velocity += gravity * delta;
        }
    }
}
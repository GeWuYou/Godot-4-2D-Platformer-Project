using Godot;

namespace Godot42DPlatformerProject.scripts.Body;

public interface IVelocityBody
{
    /// <summary>
    /// 速率
    /// </summary>
    Vector2 Velocity { get; set; }
}
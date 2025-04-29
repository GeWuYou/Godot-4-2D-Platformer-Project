using Godot;
using Godot42DPlatformerProject.scripts.Component;

namespace Godot42DPlatformerProject.scripts.Body;

/// <summary>
/// 可受击体接口（用于组件通信）
/// </summary>
public interface IHitAbleBody : IVelocityBody
{
    float MaxHealthValue { get; }
    float CurrentHealthValue { get; set; }
    bool IsInvincible { get; }

    /// <summary>
    /// Global Position
    /// </summary>
    Vector2 GlobalPosition { get; set; }

    HitComponent HitComponent { get; }

    /// <summary>
    /// 播放受击动画
    /// </summary>
    void PlayHitAnimation();

    /// <summary>
    /// 处理伤害
    /// </summary>
    /// <param name="amount"> 伤害值 </param>
    void ApplyDamage(int amount);

    /// <summary>
    /// 开始无敌状态
    /// </summary>
    /// <param name="duration"> 持续时间 </param>
    void StartInvincibility(float duration);
}
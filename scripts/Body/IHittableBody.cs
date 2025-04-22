using Godot;

namespace Godot42DPlatformerProject.scripts.Body;

/// <summary>
/// 可受击体接口（用于组件通信）
/// </summary>
public interface IHittableBody
{
    /// <summary>
    /// 速率
    /// </summary>
    Vector2 Velocity { get; set; }

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
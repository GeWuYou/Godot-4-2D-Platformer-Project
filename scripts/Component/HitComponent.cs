
using Godot;
using Godot42DPlatformerProject.scripts.Body;

namespace Godot42DPlatformerProject.scripts.Component;

public class HitComponent(IHitAbleBody body)
{
    
    public void ReceiveHit(Vector2 knockbackForce, int damage=1, float invincibilityTime = 0.5f)
    {
        if (body.IsInvincible) return;
        // 设置击退
        body.Velocity = knockbackForce;

        // 播放受击动画
        body.PlayHitAnimation();

        // 扣血等逻辑
        body.ApplyDamage(damage);

        // 设置无敌帧
        body.StartInvincibility(invincibilityTime);
    }
}

using Godot;
using Godot42DPlatformerProject.scripts.Body;

namespace Godot42DPlatformerProject.scripts.Component;

public class HitComponent(IHittableBody body)
{
    public void ReceiveHit(Vector2 knockbackForce, int damage, float invincibilityTime = 0.5f)
    {
        // 设置击退
        body.Velocity = knockbackForce;

        // 播放受击动画
        body.PlayHitAnimation();

        // 扣血等逻辑
        body.ApplyDamage(damage);

        // 设置无敌帧（可选）
        body.StartInvincibility(invincibilityTime);
    }
}
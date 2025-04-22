using Godot;
using Godot42DPlatformerProject.scripts.Body;
using Godot42DPlatformerProject.scripts.Component;
using Godot42DPlatformerProject.scripts.MainCharacter;

namespace Godot42DPlatformerProject.scripts.Npc.Enemies;

/// <summary>
/// 蘑菇敌人
/// </summary>
public partial class Mushroom : CharacterBody2D, IHittableBody
{
    private bool _isDead;
    private AnimatedSprite2D _animatedSprite2D;
    private HitComponent _hitComponent;

    public override void _PhysicsProcess(double delta)
    {
        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is not IPlayer player) continue;
            var from = (player.GlobalPosition - GlobalPosition).Normalized();
            player.OnHitByEnemy(from);
        }
    }

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _hitComponent = new HitComponent(this);
    }

    public new Vector2 Velocity
    {
        get => base.Velocity;
        set => base.Velocity = value;
    }

    public void PlayHitAnimation()
    {
        _animatedSprite2D.Play("Hit");
    }

    public void ApplyDamage(int amount)
    {
        QueueFree();
    }

    public void StartInvincibility(float duration)
    {
    }
}
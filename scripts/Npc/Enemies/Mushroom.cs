using Godot;
using Godot42DPlatformerProject.scripts.Body;
using Godot42DPlatformerProject.scripts.Component;
using Godot42DPlatformerProject.scripts.MainCharacter;
using Godot42DPlatformerProject.scripts.Responder;

namespace Godot42DPlatformerProject.scripts.Npc.Enemies;

/// <summary>
/// 蘑菇敌人
/// </summary>
public partial class Mushroom : CharacterBody2D, IHitAbleBody
{
    private bool _isDead;
    private AnimatedSprite2D _animatedSprite2D;
    public HitComponent HitComponent { get; private set; }
    private Area2D _area2D;
    public bool IsInvincible => false;
    private TouchResponderRegistry _responderRegistry;
    public new Vector2 GlobalPosition
    {
        get => base.GlobalPosition;
        set => base.GlobalPosition = value;
    }


    public override void _PhysicsProcess(double delta)
    {
    }

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HitComponent = new HitComponent(this);
        _area2D = GetNode<Area2D>("Area2D");
        _responderRegistry = ServiceLocator.Resolve<TouchResponderRegistry>();
        _responderRegistry.Register(new MushroomTouchResponder());
        _area2D.BodyEntered += OnBodyEntered;
    }
    private void OnBodyEntered(Node body)
    {
        if (body is IPlayer player)
        {
            _responderRegistry.TryDispatch(this, player);
        }
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
        GetTree().CreateTimer(0.2f).Timeout += QueueFree;
    }

    public void StartInvincibility(float duration)
    {
    }
}
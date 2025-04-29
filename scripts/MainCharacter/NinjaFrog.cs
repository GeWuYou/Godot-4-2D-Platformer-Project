using Godot;
using Godot42DPlatformerProject.scripts.Component;
using Godot42DPlatformerProject.scripts.Extension;

namespace Godot42DPlatformerProject.scripts.MainCharacter;

public partial class NinjaFrog : CharacterBody2D, IPlayer
{
    [Export] [ExportCategory("移动速度")] private float _speed = 300.0f;
    [Export] [ExportCategory("跳跃速度")] private float _jumpVelocity = -900.0f;
    [Export] [ExportCategory("速度变化率")] private float _friction = 800.0f;
    private AnimatedSprite2D _animatedSprite2D;
    private MoveComponent _moveComponent;
    private AnimationComponent _animationComponent;
    private PhysicsComponent _physicsComponent;
    private PlatformCollisionComponent _platformCollisionComponent;
    public HitComponent HitComponent { get; private set; }
    private IStateComponent<PlayerState> _playerStateComponent;
    public JumpComponent JumpComponent { get; private set; }
    [Export] [ExportCategory("血条")] public TextureProgressBar HealthProgressBar { get; set; }
    public void Reset()
    {
        CurrentHealthValue = MaxHealthValue;
        HealthProgressBar.Value = 100;
    }

    public float MaxHealthValue => 10;
    public float CurrentHealthValue { get; set; }
    public bool IsInvincible { get; private set; }

    public new Vector2 Velocity
    {
        get => base.Velocity;
        set => base.Velocity = value;
    }

    public new Vector2 GlobalPosition
    {
        get => base.GlobalPosition;
        set => base.GlobalPosition = value;
    }

    public override void _Ready()
    {
        CurrentHealthValue = MaxHealthValue;
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        JumpComponent = new JumpComponent(this, _jumpVelocity);
        HitComponent = new HitComponent(this);
        _moveComponent = new MoveComponent(_speed, _friction);
        _animationComponent = new AnimationComponent(_animatedSprite2D);
        _physicsComponent = ServiceLocator.Resolve<PhysicsComponent>();
        _platformCollisionComponent = new PlatformCollisionComponent(this);
        _playerStateComponent = new PlayerStateComponent();
        // 连接方向修改信号
        _moveComponent.Connect("DirectionChanged",
            new Callable(_animationComponent, nameof(_animationComponent.OnDirectionChanged)));
        HealthProgressBar.Value = 100;
    }
    

    public override void _PhysicsProcess(double delta)
    {
        // 角色未激活时不进行任何处理
        if (!this.IsActive())
        {
            return;
        }

        var velocity = Velocity;
        // 添加重力
        _physicsComponent.AddGravity(IsOnFloor(), ref velocity, GetGravity(), (float)delta);
        // 处理平台碰撞(下落逻辑)
        _platformCollisionComponent.HandleCollisionControl();
        // 移动输入
        _moveComponent.Move(ref velocity, delta);
        // 尝试跳跃
        JumpComponent.Jump(ref velocity);
        Velocity = velocity;
        MoveAndSlide();
        // 处理角色状态
        UpdateStatus();
        _animationComponent.UpdateAnimation(_playerStateComponent.CurrentState.ToString());
    }

    private void UpdateStatus()
    {
        // ✅ 只在最后根据状态设置一次动画
        _playerStateComponent.ChangeState(PlayerState.Idle);
        if (!IsOnFloor())
        {
            _playerStateComponent.ChangeState(PlayerState.Jumping);
        }
        else if (Mathf.Abs(Velocity.X) > 1)
        {
            _playerStateComponent.ChangeState(PlayerState.Running);
        }
    }


    public void PlayHitAnimation()
    {
        _animatedSprite2D.Play("Hit");
    }

    public void ApplyDamage(int amount)
    {
        CurrentHealthValue -= amount;
        HealthProgressBar.Value = (CurrentHealthValue / MaxHealthValue) * 100;
    }

    public void StartInvincibility(float duration)
    {
        if (IsInvincible) return;
        IsInvincible = true;
        GetTree().CreateTimer(duration).Timeout += () => { IsInvincible = false; };
    }
}
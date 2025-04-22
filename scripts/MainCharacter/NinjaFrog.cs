using Godot;
using Godot42DPlatformerProject.scripts.Body;
using Godot42DPlatformerProject.scripts.Component;

namespace Godot42DPlatformerProject.scripts.MainCharacter;

public partial class NinjaFrog : CharacterBody2D, IPlayer
{
    [Export] [ExportCategory("移动速度")] private float _speed = 300.0f;
    [Export] [ExportCategory("跳跃速度")] private float _jumpVelocity = -900.0f;
    [Export] [ExportCategory("速度变化率")] private float _friction = 800.0f;
    private AnimatedSprite2D _animatedSprite2D;
    private JumpComponent _jumpComponent;
    private HitComponent _hitComponent;
    private Area2D _enemyDetector;
    private bool _isInvincible;

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
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _enemyDetector = GetNode<Area2D>("EnemyDetector");
        _enemyDetector.BodyEntered += OnStompEnemy;
        _jumpComponent = new JumpComponent(this, _jumpVelocity);
        _hitComponent = new HitComponent(this);
    }
    
    private void Jump()
    {
        _jumpComponent.CanJump = true;
    }


    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        // 添加重力
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        if (Input.IsActionJustPressed("down") && IsOnFloor())
        {
            GD.Print("下落触发：禁用平台碰撞");
            SetCollisionMaskValue(2, false); // 关闭第二层检测
            GetTree().CreateTimer(0.2f).Timeout += () =>
            {
                GD.Print("恢复平台碰撞");
                SetCollisionMaskValue(2, true); // 恢复
            };
        }

        // 跳跃输入
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            Jump();
        }

        // 获取输入方向
        var direction = Input.GetVector("left", "right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * _speed;

            // 翻转贴图方向
            _animatedSprite2D.FlipH = direction.X switch
            {
                > 0 => false,
                < 0 => true,
                _ => _animatedSprite2D.FlipH
            };
        }
        else
        {
            // 根据角色当前速度和目标速度（0），在指定的速度变化率（Speed）下平滑地改变角色的水平速度
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _friction * (float)delta);
        }

        // 尝试跳跃
        _jumpComponent.TryJump(ref velocity);
        Velocity = velocity;
        MoveAndSlide();

        // ✅ 只在最后根据状态设置一次动画
        var animationToPlay = "Idle";
        if (!IsOnFloor())
        {
            animationToPlay = "Jumping";
        }
        else if (Mathf.Abs(Velocity.X) > 1)
        {
            animationToPlay = "Running";
        }

        if (_animatedSprite2D.Animation != animationToPlay)
        {
            _animatedSprite2D.Play(animationToPlay);
        }
    }

    private void OnStompEnemy(Node body)
    {
        if (body==this||body is not IHittableBody hittable)
        {
            return;
        }
        GD.Print("踩中敌人！");
        hittable.ApplyDamage(1); // 或调用更通用：ReceiveHit(...)
        Jump();
    }
    


    public void PlayHitAnimation()
    {
        _animatedSprite2D.Play("Hit");
    }

    public void ApplyDamage(int amount)
    {
    }

    public void StartInvincibility(float duration)
    {
    }

    public void OnHitByEnemy(Vector2 fromDir)
    {
        if (_isInvincible) return;
        var knockback = new Vector2(fromDir.X * 300, -200);
        _hitComponent.ReceiveHit(knockback, 1);
    }
}
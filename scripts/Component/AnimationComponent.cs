using Godot;

namespace Godot42DPlatformerProject.scripts.Component;

/// <summary>
/// 动画组件
/// </summary>
public partial class AnimationComponent(AnimatedSprite2D animatedSprite2D) : Node
{
    // 响应朝向改变的信号
    public void OnDirectionChanged(bool facingRight)
    {
        // 处理角色的翻转
        animatedSprite2D.FlipH = !facingRight;
    }

    /// <summary>
    ///  更新动画
    /// </summary>
    /// <param name="currentAnimation"> 当前动画名称 </param>
    public void UpdateAnimation(string currentAnimation)
    {
        if (animatedSprite2D.Animation != currentAnimation)
        {
            animatedSprite2D.Play(currentAnimation);
        }
    }
}
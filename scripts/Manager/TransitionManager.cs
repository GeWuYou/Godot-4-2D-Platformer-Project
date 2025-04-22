using System;
using Godot;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class TransitionManager : CanvasLayer, IRegisterAbleManager
{
    [Signal]
    public delegate void FadeInCompletedEventHandler();

    private ColorRect _blackOverlay;
    private AnimationPlayer _anim;

    public override void _Ready()
    {
        _blackOverlay = GetNode<ColorRect>("BlackOverlay");
        _anim = GetNode<AnimationPlayer>("AnimationPlayer");
        Visible = false;
    }

    public void PlayFadeIn()
    {
        if (!Visible)
        {
            Visible = true;
        }

        _anim.Play("fade_in");
        _anim.AnimationFinished += OnFadeInFinished;
    }

    public void PlayFadeOut()
    {
        _anim.Play("fade_out");
    }

    private void OnFadeInFinished(StringName name)
    {
        if (name != "fade_in")
        {
            return;
        }

        // 触发 FadeInCompleted 信号
        EmitSignal(SignalName.FadeInCompleted);

        // 在动画完成后隐藏黑幕
        _anim.AnimationFinished -= OnFadeInFinished;

        // 动画完成后，隐藏黑幕，恢复 UI 交互
        Visible = false;
    }

    /// <summary>
    ///  执行淡入淡出动画，并在动画淡入完成后执行回调
    /// </summary>
    /// <param name="callback"> 回调函数 </param>
    public void RunWithFadeIn(Action callback)
    {
        // 播放淡入动画
        PlayFadeIn();
        // 等待 FadeIn 完成，之后执行回调
        FadeInCompleted += () =>
        {
            callback?.Invoke(); // 执行回调
            PlayFadeOut(); // 播放淡出动画
        };
    }
}
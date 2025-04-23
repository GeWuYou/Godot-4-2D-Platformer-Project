namespace Godot42DPlatformerProject.scripts.Component;

public interface IStateComponent<T>
{
    /// <summary>
    /// 当前状态
    /// </summary>
    T CurrentState { get; }
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newState"></param>
    void ChangeState(T newState); 
    /// <summary>
    /// 是否处于指定状态
    /// </summary>
    /// <param name="state"></param>
    bool Is(T state) => CurrentState.Equals(state);
}
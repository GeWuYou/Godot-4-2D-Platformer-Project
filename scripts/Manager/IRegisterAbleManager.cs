namespace Godot42DPlatformerProject.scripts.Manager;

/// <summary>
/// all need to be registered managers must implement this interface
/// </summary>
public interface IRegisterAbleManager
{
    int InitOrder => 0; // 默认优先级为 0
    /// <summary>
    /// 初始化管理器的方法
    /// </summary>
    public void Initialize()
    {
        // Code to initialize the manager
    }

}
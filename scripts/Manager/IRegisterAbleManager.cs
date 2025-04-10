namespace Godot42DPlatformerProject.scripts.Manager;

/// <summary>
/// all need to be registered managers must implement this interface
/// </summary>
public interface IRegisterAbleManager
{
    /// <summary>
    /// 初始化管理器的方法
    /// </summary>
    public void Initialize()
    {
        // Code to initialize the manager
    }

}
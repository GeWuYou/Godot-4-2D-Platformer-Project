using Godot;

namespace Godot42DPlatformerProject.scripts.Manager;

/// <summary>
///  Loads all managers and registers them with the service locator.
/// </summary>
public partial class ManagerLoader : Node
{
    public override void _Ready()
    {
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is not IRegisterAbleManager manager)
            {
                continue;
            }
            GD.Print($"Loading manager: {child.Name}");
           
            var actualType = manager.GetType(); // 获取具体类型
            GD.Print($"[ManagerLoader] 注册管理器: {actualType.Name}");

            ServiceLocator.Register(actualType, manager);
        }
        foreach (var child in children)
        {
            if (child is not IRegisterAbleManager manager)
            {
                continue;
            }
            // Initialize the manager and register it with the service locator
            manager.Initialize();
        }
        
    }
}
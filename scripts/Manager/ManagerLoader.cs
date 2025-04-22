using System.Collections.Generic;
using Godot;

namespace Godot42DPlatformerProject.scripts.Manager;

/// <summary>
///  Loads all managers and registers them with the service locator.
/// </summary>
public partial class ManagerLoader : Node
{
    public override void _Ready()
    {
        RegisterAndInitializeManagers();
    }

    private void RegisterAndInitializeManagers()
    {
        var managers = new List<IRegisterAbleManager>();

        foreach (var child in GetChildren())
        {
            if (child is not IRegisterAbleManager manager) continue;
            var type = manager.GetType();
            GD.Print($"[ManagerLoader] 注册管理器: {type.Name}");
            ServiceLocator.Register(type, manager);
            managers.Add(manager);
        }
        // 按 InitOrder 排序
        managers.Sort((a, b) => a.InitOrder.CompareTo(b.InitOrder));
        // 第二遍初始化
        foreach (var manager in managers)
        {
            manager.Initialize();
        }
    }
}
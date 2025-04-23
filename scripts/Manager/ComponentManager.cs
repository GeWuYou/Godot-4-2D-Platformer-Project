using System.Collections.Generic;
using Godot;
using Godot42DPlatformerProject.scripts.Component;
using Godot42DPlatformerProject.scripts.Responder;

namespace Godot42DPlatformerProject.scripts.Manager;

/// <summary>
/// 在此注册线程安全的组件，并将它们注册到服务定位器中
/// </summary>
public partial class ComponentManager : Node, IRegisterAbleManager
{
    public void Initialize()
    {
        List<object> components =
        [
            new PhysicsComponent(),
            new TouchResponderRegistry()
        ];
        foreach (var component in components)
        {
            var type = component.GetType();
            ServiceLocator.Register(type, component);
        }
    }
}
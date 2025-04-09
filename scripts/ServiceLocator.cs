using System;
using System.Collections.Generic;
using Godot;

namespace Godot42DPlatformerProject.scripts;

/// <summary>
/// ServiceLocator
/// </summary>
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void Register(Type type, object instance)
    {
        if (!Services.TryAdd(type, instance))
        {
            GD.PrintErr($"[ServiceLocator] 类型 {type.Name} 已注册！");
        }
    }
    

    public static T Resolve<T>() where T : class
    {
        if (Services.TryGetValue(typeof(T), out var instance))
            return instance as T;
        GD.PrintErr($"[ServiceLocator] 未找到类型 {typeof(T).Name} 的服务！");
        throw new KeyNotFoundException($"未注册的服务类型: {typeof(T).Name}");
    }
}
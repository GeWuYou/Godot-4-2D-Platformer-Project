
using System;
using System.Collections.Generic;

namespace Godot42DPlatformerProject.scripts;

/// <summary>
/// ServiceLocator
/// </summary>
public class ServiceLocator
{
    private static Dictionary<Type, object> _services = new();

    public static void Register<T>(T instance) where T : class
    {
        _services[typeof(T)] = instance;
    }

    public static T Resolve<T>() where T : class
    {
        return _services[typeof(T)] as T;
    }
}
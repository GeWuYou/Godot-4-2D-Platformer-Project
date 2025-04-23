
using System;
using System.Collections.Generic;

namespace Godot42DPlatformerProject.scripts.Responder;

/// <summary>
/// TouchResponderRegistry
/// </summary>
public class TouchResponderRegistry
{
    private readonly Dictionary<Type, object> _responders = new();

    public void Register<TSource, TTarget>(ITouchResponder<TSource, TTarget> responder)
    {
        _responders[typeof(TSource)] = responder;
    }

    public bool TryDispatch<TSource>(TSource source, object target)
    {
        var sourceType = source.GetType();
        if (!_responders.TryGetValue(sourceType, out var responder)) return false;
        var method = responder.GetType().GetMethod("OnTouched");
        method?.Invoke(responder, [source, target]);
        return true;

    }
}
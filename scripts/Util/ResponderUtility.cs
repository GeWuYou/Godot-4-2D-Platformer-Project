using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot42DPlatformerProject.scripts.Responder;

namespace Godot42DPlatformerProject.scripts.Util;

/// <summary>
/// ResponderUtility
/// </summary>
public static class ResponderUtility
{
    public static IEnumerable<object> GetMatchingResponders<TSource, TTarget>(Node node)
    {
        return node.GetChildren()
            .Concat([node]) // 包括自己和子节点
            .Select(child => child.GetScript().Obj)
            .Where(script => script is ITouchResponder<TSource, TTarget>);
    }
}
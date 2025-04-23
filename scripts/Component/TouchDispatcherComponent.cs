using Godot;
using Godot42DPlatformerProject.scripts.Responder;
using Godot42DPlatformerProject.scripts.Util;

namespace Godot42DPlatformerProject.scripts.Component;

/// <summary>
/// TouchDispatcherComponent
/// </summary>
public partial class TouchDispatcherComponent<TSource> : Node
{
    public override void _Ready()
    {
        if (GetParent() is not Area2D area)
        {
            GD.PushWarning("TouchDispatcherComponent 必须挂在 Area2D 子节点下");
            return;
        }

        area.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not Node2D targetNode) return;
        if (GetParent().GetParent() is not TSource sourceObj) return;

        foreach (var responder in ResponderUtility.GetMatchingResponders<TSource, Node2D>(targetNode))
        {
            if (responder is ITouchResponder<TSource, Node2D> r)
            {
                r.OnTouched(sourceObj, targetNode);
            }
        }
    }
}
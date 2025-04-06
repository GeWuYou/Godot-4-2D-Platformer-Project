
using Godot;

namespace Godot42DPlatformerProject.scripts.Extension;

/// <summary>
/// NodeExtension
/// </summary>
public static class NodeExtension
{
    public static void SetActive(this Node node, bool active)
    {
        if (node is not Control control)
        {
            return;
        }
        control.Visible = active;
        control.SetProcess(active);
        control.SetPhysicsProcess(active);
    }
}
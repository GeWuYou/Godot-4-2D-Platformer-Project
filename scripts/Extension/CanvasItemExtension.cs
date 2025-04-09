
using Godot;

namespace Godot42DPlatformerProject.scripts.Extension;

/// <summary>
/// CanvasItemExtension
/// </summary>
public static class CanvasItemExtension
{
    /// <summary>
    /// Set the active state of the CanvasItem
    /// </summary>
    /// <param name="item"> The CanvasItem to set </param>
    /// <param name="active"> The active state to set </param>
    public static void SetActive(this CanvasItem item, bool active)
    {
        // 设置不可见
        item.Visible = active;
        // 不处理输入
        item.SetProcess(active);
        // 不处理物理
        item.SetPhysicsProcess(active);
    }

    /// <summary>
    /// Get the active state of the CanvasItem
    /// </summary>
    /// <param name="item"> The CanvasItem to check </param>
    /// <returns></returns>
    public static bool IsActive(this CanvasItem item)
    {
        return item.Visible&&item.IsPhysicsProcessing()&&item.IsProcessing();
    }
}
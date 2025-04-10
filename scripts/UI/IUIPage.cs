using Godot42DPlatformerProject.scripts.Manager;

namespace Godot42DPlatformerProject.scripts.UI;

/// <summary>
///  Interface for UI pages
/// </summary>
public interface IUiPage
{
    /// <summary>
    /// Called when the page is pushed onto the stack
    /// </summary>
    /// <param name="manager"></param>
    void OnPush(UiManager manager); 
    /// <summary>
    /// Called when the page is popped off the stack
    /// </summary>
    /// <param name="manager"></param>
    void OnReturn(UiManager manager);
}
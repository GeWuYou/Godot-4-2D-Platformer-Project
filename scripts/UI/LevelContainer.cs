using Godot;
using Godot42DPlatformerProject.scripts.Extension;

namespace Godot42DPlatformerProject.scripts.UI;

public partial class LevelContainer : PanelContainer
{
    
    /// <summary>
    /// 选关界面返回按钮
    /// </summary>
    [Export][ExportCategory("选关界面返回按钮")]
    private TextureButton _selectLevelToReturnButton;

    /// <summary>
    /// 选关界面返回按钮
    /// </summary>
    [Export][ExportCategory("选关界面返回按钮")]
    private VBoxContainer _mainMenuButtonContainer;
    public override void _Ready()
    {
        _selectLevelToReturnButton.Pressed+=OnSelectLevelToReturnButtonPressed;
    }

    private void OnSelectLevelToReturnButtonPressed()
    {
        this.SetActive(false);
        _mainMenuButtonContainer.SetActive(true);
    }
}
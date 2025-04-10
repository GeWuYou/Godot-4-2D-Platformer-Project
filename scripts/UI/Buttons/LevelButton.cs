using System;
using Godot;
using Godot42DPlatformerProject.scripts.Config;
using Godot42DPlatformerProject.scripts.Extension;
using Godot42DPlatformerProject.scripts.manager;

namespace Godot42DPlatformerProject.scripts.UI.Buttons;

public partial class LevelButton : Button
{
    private PanelContainer _levelContainer;
    private VBoxContainer _mainMenuButtonContainer;
    private GameManager _gameManager;
    private GridContainer _levelButtons;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _mainMenuButtonContainer = GetParent() as VBoxContainer;
        _gameManager = ServiceLocator.Resolve<GameManager>();
        if (_mainMenuButtonContainer == null)
        {
            GD.Print("LevelButton::_Ready: _mainMenuButtonContainer is null");
        }

        var root = _mainMenuButtonContainer.GetParent();
        // 获取LevelContainer节点
        _levelContainer = root.GetNode<PanelContainer>("LevelContainer");
        _levelButtons = _levelContainer.GetNode<GridContainer>("VBoxContainer/ScrollContainer/LevelButtons");
        GD.Print(_levelContainer.Name);
    }


    /// <summary>
    ///  按下按钮时触发
    /// </summary>
    private void OnPressed()
    {
        GD.Print("Pressed");
        var buttonScene = (PackedScene)ResourceLoader.Load(UiConfig.LevelItemButtonScenePath);
        for (var i = 1; i <= _gameManager.LevelCount; i++)
        {
            // 实例化 LevelItemButton
            var levelButton = (LevelItemButton)buttonScene.Instantiate();
            levelButton.Init(i);
            // 将 LevelItemButton 添加到 LevelContainer
            _levelButtons.AddChild(levelButton);
        }
        // 失效并隐藏主菜单按钮列表
        _mainMenuButtonContainer.SetActive(false);
        // 激活LevelContainer节点
        _levelContainer.SetActive(true);
    }
}
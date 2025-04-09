using Godot;
using Godot42DPlatformerProject.scripts.manager;
using Godot42DPlatformerProject.scripts.Manager;

namespace Godot42DPlatformerProject.scripts.UI.Buttons;

public partial class LevelItemButton : TextureButton
{
    private GameManager _gameManager;
    private LevelManager _levelManager;
    public int LevelId { get; set; } // 每个按钮绑定一个关卡ID

    public override void _Ready()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
        _levelManager = ServiceLocator.Resolve<LevelManager>();
    }

    public void Init(int levelId)
    {
        LevelId = levelId;
        // 设置按钮的纹理
        if (LevelId < 10)
        {
            TextureNormal =
                (Texture2D)ResourceLoader.Load("res://assets/Pixel Adventure/Menu/Levels/0" + LevelId + ".png");
        }
        else
        {
            TextureNormal =
                (Texture2D)ResourceLoader.Load("res://assets/Pixel Adventure/Menu/Levels/" + LevelId + ".png");
        }
    }

    private void OnLevelItemPressed()
    {
        GD.Print("Level " + LevelId + " is selected.");
        _levelManager.LoadLevel(LevelId);
    }
}
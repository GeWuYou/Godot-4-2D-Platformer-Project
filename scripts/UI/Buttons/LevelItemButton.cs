using Godot;

namespace Godot42DPlatformerProject.scripts.UI.Buttons;

public partial class LevelItemButton : TextureButton
{
    public int LevelId { get; set; } // 每个按钮绑定一个关卡ID

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
        // GetTree().ChangeSceneToFile("res://scenes/level/level" + LevelId + ".tscn");
        var scene = (PackedScene)ResourceLoader.Load("res://scenes/level/level" + LevelId + ".tscn");
        var level = scene.Instantiate();
        AddChild(level); // 不会销毁当前场景，只是新增一个子场景
    }
}
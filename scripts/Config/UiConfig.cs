namespace Godot42DPlatformerProject.scripts.Config;

public static class UiConfig
{
    private static string ScenesPath => "res://scenes/";
    private static string AssetsPath => "res://assets/";
    private static string UiScenesPath => ScenesPath + "ui/";
    private static string ButtonsScenesPath => UiScenesPath + "buttons/";
    
    public static readonly string LevelButtonTexturesPath = AssetsPath + "Pixel Adventure/Menu/Levels/";
    public static readonly string MainInterfaceScenePath = ScenesPath + "main_interface.tscn";
    public static readonly string LevelItemButtonScenePath = ButtonsScenesPath + "level_item_button.tscn";
}
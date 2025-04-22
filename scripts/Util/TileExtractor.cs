using Godot;

namespace Godot42DPlatformerProject.scripts.Util;

public partial class TileExtractor : Node2D
{
    [Export] [ExportCategory("需要拆分的 PNG 纹理")]
    public Texture2D TileTexture;

    [Export] [ExportCategory("提取 Tile 的行数")]
    public int TileRows = 4;

    [Export] [ExportCategory("提取 Tile 的列数")]
    public int TileCols = 4;

    [Export] [ExportCategory("目标存储路径")] public string OutputDir = "res://Assets/Textures/Tiles/";

    public override void _Ready()
    {
        if (TileTexture == null)
        {
            GD.PrintErr("❌ 没有 PNG 纹理，无法导出！");
            return;
        }

        var textureWidth = TileTexture.GetWidth();
        var textureHeight = TileTexture.GetHeight();

        // 计算每个 Tile 的宽度和高度
        var tileWidth = textureWidth / TileCols;
        var tileHeight = textureHeight / TileRows;

        // 创建文件夹
        var dir = DirAccess.Open("res://");
        if (!dir.DirExists(OutputDir))
        {
            dir.MakeDirRecursive(OutputDir);
        }

        var tileId = 0;
        for (var row = 0; row < TileRows; row++)
        {
            for (var col = 0; col < TileCols; col++)
            {
                var savePath = $"{OutputDir}tile_{tileId}.tres";
                var region = new Rect2(col * tileWidth, row * tileHeight, tileWidth, tileHeight);
                // 生成 Texture 并存储
                var atlasTexture = new AtlasTexture
                {
                    Atlas = TileTexture,
                    Region = region
                };
                ResourceSaver.Save(atlasTexture, savePath);
                GD.Print($"✅ 已导出: {savePath}");
                tileId++;
            }
        }
    }
}
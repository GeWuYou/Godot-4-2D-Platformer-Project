using Godot;

namespace Godot42DPlatformerProject.scripts.Model;

public partial class LevelData : Node
{
	private Marker2D _playerInitStartPoint;

	public Vector2 GetPlayerInitStartPoint()
	{
		_playerInitStartPoint ??= GetNodeOrNull<Marker2D>("PlayerInitStartPoint");
		return _playerInitStartPoint?.GlobalPosition ?? Vector2.Zero;
	}
}
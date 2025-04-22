using Godot;
using Godot42DPlatformerProject.scripts.Body;

namespace Godot42DPlatformerProject.scripts.MainCharacter;

/// <summary>
/// Interface for MainCharacter
/// </summary>
public interface IPlayer: IHittableBody
{
    /// <summary>
    /// Global Position
    /// </summary>
    Vector2 GlobalPosition { get; set; }
    /// <summary>
    /// Called when the player is hit by an enemy
    /// </summary>
    /// <param name="fromDir"> The direction the enemy was coming from </param>
    public void OnHitByEnemy(Vector2 fromDir);
}
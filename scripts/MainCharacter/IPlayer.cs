using Godot;
using Godot42DPlatformerProject.scripts.Body;
using Godot42DPlatformerProject.scripts.Component;

namespace Godot42DPlatformerProject.scripts.MainCharacter;

/// <summary>
/// Interface for MainCharacter
/// </summary>
public interface IPlayer: IHitAbleBody
{
    JumpComponent JumpComponent { get;}
    
    protected TextureProgressBar HealthProgressBar { get; set; }

    public void Reset();
}
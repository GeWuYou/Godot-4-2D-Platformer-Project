using Godot;
using Godot42DPlatformerProject.scripts.MainCharacter;
using Godot42DPlatformerProject.scripts.Npc.Enemies;

namespace Godot42DPlatformerProject.scripts.Responder;

/// <summary>
/// MushroomTouchResponder
/// </summary>
public class MushroomTouchResponder : ITouchResponder<Mushroom, IPlayer>
{
    public void OnTouched(Mushroom source, IPlayer target)
    {
        var verticalDiff = source.GlobalPosition.Y - target.GlobalPosition.Y;
        if (verticalDiff >= 40f)
        {
            GD.Print("玩家从上方踩中蘑菇");
            target.JumpComponent.RequestJump();
            source.HitComponent.ReceiveHit(Vector2.Zero);
        }
        else
        {
            GD.Print("玩家侧面碰到蘑菇，受到伤害");
            target.HitComponent.ReceiveHit((source.GlobalPosition - target.GlobalPosition).Normalized() * 300f);
        }
    }
}
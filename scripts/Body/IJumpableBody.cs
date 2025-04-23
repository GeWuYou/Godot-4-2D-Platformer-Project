namespace Godot42DPlatformerProject.scripts.Body;

public interface IJumpableBody : IJumpableAnimation, IJumpablePhysics,IVelocityBody
{
    void ExecuteJump();
}

public interface IJumpableAnimation
{
    void PlayJumpAnimation();
}

public interface IJumpablePhysics
{
    void ApplyJumpForce();
}
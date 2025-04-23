namespace Godot42DPlatformerProject.scripts.Responder;

public interface ITouchResponder<in T, in TS>
{
    /// <summary>
    ///  Called when the source object is touched by the target object.
    /// </summary>
    /// <param name="source"> The source object. </param>
    /// <param name="target"> The target object. </param>
    void OnTouched(T source,TS target);
}
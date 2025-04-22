using Godot;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class SceneManager : Node, IRegisterAbleManager
{
    /// <summary>
    /// 当前场景根节点
    /// </summary>
    private Node _currentSceneRoot;

    /// <summary>
    /// UI 根节点
    /// </summary>
    private Node _uiRoot;


    public void Initialize()
    {
        _currentSceneRoot = GetTree().CurrentScene;
        _uiRoot = _currentSceneRoot.GetNodeOrNull<Node>("UIRoot");
        GD.Print("[SceneManager] 初始化完成");
    }
}
using Godot;

namespace Godot42DPlatformerProject.scripts.manager;

public partial class GameManager : Node
{
    /// <summary>
    /// 当分数变化时发出信号。
    /// </summary>
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    [Export] [ExportCategory("分数")] private int _score;
    [Export] [ExportCategory("关卡数量")] private int _levelCount = 2;
    public int Score => _score;
    
    public int LevelCount => _levelCount;

    public override void _Ready()
    {
        ServiceLocator.Register(this);
    }

    public void AddScore()
    {
        GD.Print("Score: " + _score);
        _score++;
        EmitSignal(SignalName.ScoreChanged, _score);
    }
}
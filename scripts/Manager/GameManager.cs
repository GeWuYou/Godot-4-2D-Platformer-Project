using Godot;
using Godot42DPlatformerProject.scripts.Manager;

namespace Godot42DPlatformerProject.scripts.manager;

public partial class GameManager : Node,IRegisterAbleManager
{
    /// <summary>
    /// 当分数变化时发出信号。
    /// </summary>
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);
    
    [Export] [ExportCategory("分数")] private int _score;
    [Export] [ExportCategory("关卡数量")] private int _levelCount = 2;
    [Export] [ExportCategory("当前关卡级别")]private int _currentLevel = 1;
    public int Score => _score;
    
    public int LevelCount => _levelCount;
    
    public int CurrentLevel => _currentLevel;
    

    public void SetLevel(int level)
    {
        _currentLevel = level;
    }
    
    public void AddScore()
    {
        GD.Print("Score: " + _score);
        _score++;
        EmitSignal(SignalName.ScoreChanged, _score);
    }
}
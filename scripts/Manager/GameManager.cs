using System;
using Godot;
using Godot42DPlatformerProject.scripts.Component;
using Godot42DPlatformerProject.scripts.Manager;
using Godot42DPlatformerProject.scripts.UI;

namespace Godot42DPlatformerProject.scripts.manager;

public enum GameState
{
    Playing,
    Paused,
    Dialog,
    Cutscene,
    MainMenu
}

public partial class GameManager : Node, IRegisterAbleManager, IStateComponent<GameState>
{
    /// <summary>
    /// 当分数变化时发出信号。
    /// </summary>
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public GameState PreviousState { get; private set; }
    private SceneTree _sceneTree;

    private UiManager _uiManager;

    [Export] [ExportCategory("分数")] private int _score;
    [Export] [ExportCategory("关卡数量")] private int _levelCount = 2;
    [Export] [ExportCategory("当前关卡级别")] private int _currentLevel;

    /// <summary>
    /// 获取当前分数。
    /// </summary>
    public int Score
    {
        get => _score;
        set => _score = value;
    }

    /// <summary>
    /// 获取关卡总数。
    /// </summary>
    public int LevelCount => _levelCount;

    /// <summary>
    /// 获取当前关卡。
    /// </summary>
    public int CurrentLevel => _currentLevel;

    public void Initialize()
    {
        _sceneTree = GetTree();
        _uiManager = ServiceLocator.Resolve<UiManager>();
    }

    public void Quit()
    {
        _sceneTree.Quit();
    }

    /// <summary>
    /// 设置当前关卡。
    /// </summary>
    /// <param name="level">关卡级别</param>
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

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;
        GD.Print($"[GameState] Changing from {CurrentState} to {newState}");
        PreviousState = CurrentState;
        CurrentState = newState;
        JudgmentStatus();
    }

    private void JudgmentStatus()
    {
        switch (CurrentState)
        {
            // 如果当前状态设置为暂停且上一次状态为游戏进行中，则暂停游戏
            case GameState.Paused:
                if (PreviousState == GameState.Playing)
                {
                    _sceneTree.Paused = true;
                    _uiManager.PeekUi<PauseMenu>().DisplayPauseMenu();
                }
                break;
            case GameState.Playing:
                if (PreviousState == GameState.Paused)
                {
                    _sceneTree.Paused = false;
                    _uiManager.PeekUi<PauseMenu>().HidePauseMenu();
                }
                break;
        }
    }

    public bool Is(GameState state) => CurrentState.Equals(state);
}
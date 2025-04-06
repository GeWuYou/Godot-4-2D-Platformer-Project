using Godot;
using Godot42DPlatformerProject.scripts.manager;

namespace Godot42DPlatformerProject.scripts.UI;

public partial class ScorePanel : Panel
{
    private Label _scoreLabel;
    private GameManager _gameManager;

    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>("ScoreLabel");

        _gameManager = ServiceLocator.Resolve<GameManager>();
        _gameManager.ScoreChanged += OnScoreChanged;
        // 初始值也显示一次
        OnScoreChanged(_gameManager.Score);
    }

    private void OnScoreChanged(int newScore)
    {
        _scoreLabel.Text = $"Score: {newScore}";
    }
}
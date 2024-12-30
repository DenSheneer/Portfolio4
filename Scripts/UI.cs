using Godot;
using System;

public partial class UI : Control
{
    [Export] CoinManager _coinManager;
    [Export] RichTextLabel _coinNrText;
    public override void _Ready()
    {
        _coinManager.ScoreChanged += updateScoreUI;
    }

    private void updateScoreUI(int newScore)
    {
        _coinNrText.Text = newScore.ToString();
    }
}
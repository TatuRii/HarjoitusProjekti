using Godot;
using System;

namespace SnakeGame.UI
{
	public partial class TopUIControl : Node
	{
		[Export] private Label _scoreLabel = null;
		[Export] private BaseButton _restartButton = null;

		public override void _Ready()
		{
			if (_restartButton != null)
			{
				_restartButton.Pressed += OnRestartPressed;
			}
		}

		private void OnRestartPressed()
		{
			Level.Current.ResetGame();
		}

		public void SetScore(int score)
		{
			_scoreLabel.Text = $"Points: {score}";
		}
	}
}
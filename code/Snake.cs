using Godot;
using Microsoft.Win32.SafeHandles;
using SnakeGame;
using System;

namespace SnakeGame
{
	public partial class Snake : Node2D
	{
		[Export] public float speed = 100;
		[Export] private Node2D mato = null;
		[Export] private Grid _grid;
		[Export] private NuclearWaste _nuclearWaste;
		private Vector2I gridPosition;
		private Vector2 currentDirection = Vector2.Up;
		private bool isDead = false;
		private Timer _timer;

		public override void _Ready()
		{
			if (_grid != null)
			{
				gridPosition = new Vector2I(_grid.Width / 2, _grid.Height / 2);
				if (_grid.GetWorldPosition(gridPosition, out Vector2 worldPosition))
				{
					mato.Position = worldPosition;
				}
			}

			_nuclearWaste.SetRandomGridPosition(_grid, gridPosition);

			_timer = new Timer();
			AddChild(_timer);
			_timer.WaitTime = 0.5f; // Aika sekunteina
			_timer.OneShot = false; // Toistetaan ajastinta
			_timer.Timeout += OnTimerTimeout; // Yhdistetään timeout-signaali OnTimerTimeout-metodiin
			_timer.Start(); // Käynnistetään ajastin
		}

		private void OnTimerTimeout()
		{
			MoveSnake(); // Kutsutaan MoveSnake-metodia ajastimen jokaisella tikityksellä
		}


		public override void _Process(double delta)
		{
			if (gridPosition == _nuclearWaste.GridPosition)
			{
				mato.Visible = false;
				isDead = true;
			}
			if (Input.IsActionJustPressed("MoveUp"))
			{
				currentDirection = Vector2.Up;
				mato.RotationDegrees = 0;
			}
			else if (Input.IsActionJustPressed("MoveDown"))
			{
				currentDirection = Vector2.Down;
				mato.RotationDegrees = 180;
			}
			else if (Input.IsActionJustPressed("MoveLeft"))
			{
				currentDirection = Vector2.Left;
				mato.RotationDegrees = -90;
			}
			else if (Input.IsActionJustPressed("MoveRight"))
			{
				currentDirection = Vector2.Right;
				mato.RotationDegrees = 90;
			}
			Reset();
		}

		private void MoveSnake()
		{
			Vector2I newGridPosition = gridPosition + new Vector2I((int)currentDirection.X, (int)currentDirection.Y);

			// Prevent moving outside the grid
			if (newGridPosition.X < 0 || newGridPosition.X >= _grid.Width ||
				newGridPosition.Y < 0 || newGridPosition.Y >= _grid.Height)
			{
				return; // Stop movement if out of bounds
			}

			gridPosition = newGridPosition; // Update position if valid

			if (_grid.GetWorldPosition(gridPosition, out Vector2 worldPosition))
			{
				mato.Position = worldPosition;
			}
		}

		public void Reset()
		{
			if (isDead && Input.IsActionJustPressed("Restart"))
			{
				_grid.ClearGrid();

				mato.QueueFree();
				_nuclearWaste.QueueFree();

				mato = (Node2D)GD.Load<PackedScene>("res://Levels/Snake.tscn").Instantiate();
				AddChild(mato);

				_nuclearWaste = (NuclearWaste)GD.Load<PackedScene>("res://Levels/NuclearWaste.tscn").Instantiate();
				AddChild(_nuclearWaste);

				gridPosition = new Vector2I(_grid.Width / 2, _grid.Height / 2);
				if (_grid.GetWorldPosition(gridPosition, out Vector2 worldPosition))
				{
					mato.Position = worldPosition; ;
				}

				// NuclearWaste saa satunnaisen paikan ruudukosta
				_nuclearWaste.SetRandomGridPosition(_grid, gridPosition);

				isDead = false;
			}
		}
	}
}
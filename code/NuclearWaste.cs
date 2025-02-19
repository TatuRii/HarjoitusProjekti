using System;
using Godot;

namespace SnakeGame
{
	public partial class NuclearWaste : Node2D
	{
		private Vector2I _gridPosition;
		private Random _random = new Random(); // Satunnaislukugeneraattori

		public Vector2I GridPosition
		{
			get { return _gridPosition; }
			set
			{
				Vector2I newValue = value;
				if (newValue.X < 0) newValue.X = 0;
				if (newValue.Y < 0) newValue.Y = 0;

				_gridPosition = newValue;
			}
		}

		public override void _Ready()
		{
			// Ei aseteta sijaintia vielä täällä, vaan annetaan Gridin hallita sijaintia
		}

		public void SetRandomGridPosition(Grid grid, Vector2I snakePosition)
		{
			Vector2I randomPos;
			do
			{
				randomPos = new Vector2I(
					_random.Next(0, grid.Width),
					_random.Next(0, grid.Height)
				);
			} while (randomPos == snakePosition); // Ei saa mennä käärmeen aloituspaikkaan

			GridPosition = randomPos;

			if (grid.GetWorldPosition(GridPosition, out Vector2 worldPosition))
			{
				Position = worldPosition;
			}
		}
	}
}

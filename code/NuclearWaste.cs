using Godot;
using System;

namespace SnakeGame
{


	public partial class NuclearWaste : Node
	{
		private Vector2I _gridPosition;

		public Vector2I GridPosition
		{
			get { return _gridPosition; }
			set
			{
				Vector2I newValue = value;
				if (newValue.X < 0)
				{
					newValue.X = 0;
				}

				if (newValue.Y < 0)
				{
					newValue.Y = 0;
				}

				_gridPosition = newValue;
			}
		}

		public override void _Ready()
		{
			// Aseta _gridPosition-arvo
			GridPosition = new Vector2I(9, 8);
		}
	}

}

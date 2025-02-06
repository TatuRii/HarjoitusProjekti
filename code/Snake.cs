using Godot;
using System;

namespace snakegame
{
	public partial class Snake : Node2D
	{
		[Export] public float speed = 100;

		[Export] private Node2D mato = null;

		private Vector2 gridPosition;
		private bool isMoving = false;

		public override void _Ready()
		{
			// Aseta mato johonkin lailliseen koordinaattiin gridillä pelin alussa
			gridPosition = new Vector2(0, 0);
			UpdateMatoPosition();
		}

		public override void _Process(double delta)
		{
			Vector2 direction = Vector2.Zero; // Oletusarvoisesti ei liikuta

			if (Input.IsActionJustPressed("MoveUp"))
			{
				direction = Vector2.Up;
			}
			else if (Input.IsActionJustPressed("MoveDown"))
			{
				direction = Vector2.Down;
			}
			else if (Input.IsActionJustPressed("MoveLeft"))
			{
				direction = Vector2.Left;
			}
			else if (Input.IsActionJustPressed("MoveRight"))
			{
				direction = Vector2.Right;
			}

			if (direction != Vector2.Zero) // Jos liikkumiskomento annettu
			{
				gridPosition += direction;
				UpdateMatoPosition();
			}
		}

		private void UpdateMatoPosition()
		{
			// Muunna gridin koordinaatit pikselikoordinaateiksi
			mato.Position = gridPosition * 16;
		}
	}
}

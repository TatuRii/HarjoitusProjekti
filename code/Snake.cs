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
		private Vector2 currentDirection = Vector2.Up;

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

			if (direction != Vector2.Zero)
			{
				currentDirection = direction; // Päivitetään nykyinen suunta
				gridPosition += currentDirection;
				UpdateMatoPosition();

				// Käännä matoa
				if (currentDirection == Vector2.Up)
				{
					mato.RotationDegrees = 0;
				}
				else if (currentDirection == Vector2.Down)
				{
					mato.RotationDegrees = 180;
				}
				else if (currentDirection == Vector2.Left)
				{
					mato.RotationDegrees = -90;
				}
				else if (currentDirection == Vector2.Right)
				{
					mato.RotationDegrees = 90;
				}
			}
		}

		private void UpdateMatoPosition()
		{
			// Muunna gridin koordinaatit pikselikoordinaateiksi
			mato.Position = gridPosition * 16;
		}
	}
}

using Godot;
using System;

namespace snakegame
{


	public partial class Snake : Node2D
	{
		[Export] public float speed = 100;

		[Export] private Node2D mato = null;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			mato.Position = GlobalPosition;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			Move();
		}

		public void Move()
		{
			if (Input.IsActionPressed("MoveUp"))
			{
				GD.Print("I'm moving up!");
				Vector2 movement = Vector2.Up * speed * (float)GetProcessDeltaTime();
				mato.Translate(movement);

			}
			if (Input.IsActionPressed("MoveDown"))
			{
				GD.Print("I'm moving down!");
				Vector2 movement = Vector2.Down * speed * (float)GetProcessDeltaTime();
				mato.Translate(movement);
			}
			if (Input.IsActionPressed("MoveLeft"))
			{
				GD.Print("I'm moving left!");
				Vector2 movement = Vector2.Left * speed * (float)GetProcessDeltaTime();
				mato.Translate(movement);
			}
			if (Input.IsActionPressed("MoveRight"))
			{
				GD.Print("I'm moving right!");
				Vector2 movement = Vector2.Right * speed * (float)GetProcessDeltaTime();
				mato.Translate(movement);
			}
		}
	}
}

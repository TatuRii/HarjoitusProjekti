using Godot;
using System;

namespace snakegame
{


	public partial class Snake : Node2D
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			ReadInput();
		}

		public void ReadInput()
		{
			if (Input.IsActionJustPressed("MoveUp"))
			{
				GD.Print("I'm moving up!");
			}
			if (Input.IsActionJustPressed("MoveDown"))
			{
				GD.Print("I'm moving down!");
			}
			if (Input.IsActionJustPressed("MoveLeft"))
			{
				GD.Print("I'm moving left!");
			}
			if (Input.IsActionJustPressed("MoveRight"))
			{
				GD.Print("I'm moving right!");
			}

			// Jos halutaan että voi painaa pohjassa ja liikkua käytetään silloin IsActionPressed

		}
	}
}

using Godot;
using System;

namespace snakegame
{

	public partial class ProtoMover : Node2D
	{
		[Export] public float speed = 100;
		[Export] public Vector2 pointA = new Vector2(50, 50);
		[Export] public Vector2 pointB = new Vector2(50, 50);



		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GlobalPosition = pointA;

		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{

			Vector2 movement = GlobalPosition.DirectionTo(pointB);
			GlobalPosition += movement * speed * (float)delta;
			if(GlobalPosition.DistanceTo(pointB) < 1)
			{
				Vector2 temp = pointA;
				pointA = pointB;
				pointB = temp;
			}

		}
	}
}

using Godot;
using System;

public partial class ProtoMover : Node2D
{
	[Export] public float _speed = 100.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalPosition = new Vector2(50, 50);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 movement = Vector2.Down;
		GlobalPosition += movement;

	}
}

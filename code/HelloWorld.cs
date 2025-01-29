using Godot;
using System;

namespace snakegame
{

	public partial class HelloWorld : Node2D
	{
		int currentFrame = 1;
		int previousNumber = 0;
		int currentNumber = 1;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print("Hello World!");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			GD.Print("Frame " + currentFrame + ": " + previousNumber);

			if (previousNumber >= 1000)
			{
				SetProcess(false);
			}

			int nextNumber = previousNumber + currentNumber;
			previousNumber = currentNumber;
			currentNumber = nextNumber;

			currentFrame++;
		}
	}
}

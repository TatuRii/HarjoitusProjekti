using Godot;
using System;

namespace SnakeGame
{
	public partial class Timer : Node
	{
		[Export] private double _time = 1;
		private double _timer = 0;
		private bool _isRunning = false;

		private bool _isComplete = false;

		public bool IsRunning
		{
			get { return _isRunning; }
			private set { _isRunning = value; }
		}

		public bool IsComplete
		{
			get { return _isComplete; }
			private set { _isComplete = value; }
		}

		public override void _Process(double delta)
		{
			if (IsRunning && !IsComplete)
			{
				_timer -= delta;

				if (_timer <= 0)
				{
					_timer = 0;
					IsComplete = true;
					Stop();
				}
			}
		}

		public void SetTime(double time)
		{
			_time = time;
			_timer = time;
		}

		public void Start()
		{
			IsRunning = true;
		}

		public void Stop()
		{
			IsRunning = false;
		}

		public void Reset(bool autoStart)
		{
			IsComplete = false;
			SetTime(_time);

			if (autoStart)
			{
				Start();
			}
			else
			{

				Stop();
			}
		}
	}
}

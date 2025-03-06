using Godot;
using System;

namespace SnakeGame
{
    public partial class Snake : Node2D, ICellOccupier
    {
        // Enum to define the possible movement directions of the snake.
        public enum Direction
        {
            None = 0,
            Up,
            Down,
            Left,
            Right,
        }

        [Export] private float _speed = 1;
        [Export] private Timer _moveTimer = null;

        // Current grid position of the snake.
        private Vector2I _currentPosition = new Vector2I(5, 5);
        // Current movement direction of the snake.
        private Direction _currentDirection = Direction.Up;
        // Input direction from the player.
        private Direction _inputDirection = Direction.None;

        // Property to indicate that this object is a snake for cell occupation.
        public CellOccupierType Type
        {
            get { return CellOccupierType.Snake; }
        }

        public Vector2I GridPosition
        {
            get { return _currentPosition; }
        }

        public override void _Ready()
        {
            // Set the initial position of the snake on the grid.
            if (Level.Current.Grid.GetWorldPosition(_currentPosition, out Vector2 worldPosition))
            {
                Position = worldPosition;
            }

            // Get the move timer node.
            if (_moveTimer == null)
            {
                _moveTimer = GetNode<Timer>("MoveTimer");

                if (_moveTimer == null)
                {
                    GD.PrintErr("Move timer cannot be found!");
                }
            }

            // Start the move timer.
            if (_moveTimer != null)
            {
                _moveTimer.Start();
            }
        }

        public override void _Process(double delta)
        {
            // Read player input and update the input direction.
            Direction direction = ReadInput();
            if (direction != Direction.None)
            {
                _inputDirection = direction;
            }

            // When the move timer is complete, move the snake.
            if (_moveTimer.IsComplete)
            {
                // Validate and apply the input direction.
                _inputDirection = ValidateInput(_inputDirection, _currentDirection);
                _currentDirection = _inputDirection;
                _inputDirection = Direction.None;

                if (_currentDirection != Direction.None)
                {
                    Move(_currentDirection);
                }

                // Restart the move timer.
                _moveTimer.Reset(autoStart: true);
            }
        }

        // Ensures the snake cannot move directly opposite to its current direction.
        private Direction ValidateInput(Direction inputDirection, Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.Up:
                case Direction.Down:
                    if (inputDirection == Direction.Left || inputDirection == Direction.Right)
                    {
                        return inputDirection;
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    if (inputDirection == Direction.Up || inputDirection == Direction.Down)
                    {
                        return inputDirection;
                    }
                    break;
            }

            return currentDirection;
        }

        // Moves the snake in the given direction.
        private void Move(Direction direction)
        {
            // Calculate the next grid position.
            Vector2I nextPosition = GetNextGridPosition(direction, _currentPosition);
            // If the next position is valid, move the snake.
            if (Level.Current.Grid.GetWorldPosition(nextPosition, out Vector2 worldPosition))
            {
                // Check for collectables at the next position.
                Collectable collectable = Level.Current.Grid.GetCollectable(nextPosition);
                if (collectable != null)
                {
                    // Collect the collectable.
                    collectable.Collect(this);
                }

                // Update the snake's position and rotation.
                _currentPosition = nextPosition;
                Position = worldPosition;
                RotationDegrees = GetRotation(direction);
            }
        }

        // Calculates the next grid position based on the current position and direction.
        private Vector2I GetNextGridPosition(Direction direction, Vector2I currentPosition)
        {
            switch (direction)
            {
                case Direction.Up: return currentPosition + Vector2I.Up;
                case Direction.Down: return currentPosition + Vector2I.Down;
                case Direction.Right: return currentPosition + Vector2I.Right;
                case Direction.Left: return currentPosition + Vector2I.Left;
                default: return currentPosition;
            }
        }

        // Gets the rotation angle for the given direction.
        private float GetRotation(Direction movementDirection)
        {
            switch (movementDirection)
            {
                case Direction.Up: return 0;
                case Direction.Down: return 180;
                case Direction.Right: return 90;
                case Direction.Left: return -90;
                default: return 0;
            }
        }

        // Gets the direction vector for the given direction.
        private Vector2 GetDirectionVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Vector2.Up;
                case Direction.Down: return Vector2.Down;
                case Direction.Right: return Vector2.Right;
                case Direction.Left: return Vector2.Left;
                default: return Vector2.Zero;
            }
        }

        // Reads player input and returns the corresponding direction.
        private Direction ReadInput()
        {
            Direction direction = Direction.None;

            if (Input.IsActionJustPressed(Config.MoveUpAction))
            {
                direction = Direction.Up;
            }
            else if (Input.IsActionJustPressed(Config.MoveDownAction))
            {
                direction = Direction.Down;
            }
            else if (Input.IsActionJustPressed(Config.MoveLeftAction))
            {
                direction = Direction.Left;
            }
            else if (Input.IsActionJustPressed(Config.MoveRightAction))
            {
                direction = Direction.Right;
            }

            return direction;
        }
    }
}
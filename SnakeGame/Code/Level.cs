using Godot;
using SnakeGame.UI;
using System;

namespace SnakeGame
{
    public partial class Level : Node2D
    {
        // Static instance of the current level, allowing access from anywhere.
        private static Level _current = null;
        public static Level Current
        {
            get { return _current; }
        }

        // Paths to the scenes for the snake, apple, and nuclear waste.
        [Export] private string _snakeScenePath = "res://Scenes/Snake.tscn";
        [Export] private string _appleScenePath = "res://Scenes/Apple.tscn";
        [Export] private string _nuclearWasteScenePath = "res://Scenes/NuclearWaste.tscn";

        // Reference to the UI control for displaying the score.
        [Export] private TopUIControl _topUIControl = null;

        // Packed scenes for the snake, apple, and nuclear waste.
        private PackedScene _snakeScene = null;
        private PackedScene _appleScene = null;
        private PackedScene _nuclearWasteScene = null;

        // Current score of the game.
        private int _score = 0;

        // Reference to the grid.
        private Grid _grid = null;

        // References to the snake, apple, and nuclear waste instances.
        private Snake _snake = null;
        private Apple _apple = null;
        private NuclearWaste _nuclearWaste = null;

        // Property to access and modify the score, ensuring it's not negative and updating the UI.
        public int Score
        {
            get { return _score; }
            set
            {
                if (value < 0)
                {
                    _score = 0;
                }
                else
                {
                    _score = value;
                }

                if (_topUIControl != null)
                {
                    _topUIControl.SetScore(_score);
                }
            }
        }

        public Grid Grid
        {
            get { return _grid; }
        }

        public Snake Snake
        {
            get { return _snake; }
        }

        public Level()
        {
            _current = this;
        }

        public override void _Ready()
        {
            // Get the grid node.
            _grid = GetNode<Grid>("Grid");
            if (_grid == null)
            {
                GD.PrintErr("Grid not found!");
            }

            // Reset the game state.
            ResetGame();
        }

        // Creates a new snake instance.
        private Snake CreateSnake()
        {
            if (_snakeScene == null)
            {
                // Load the snake scene if it hasn't been loaded yet.
                _snakeScene = ResourceLoader.Load<PackedScene>(_snakeScenePath);
                if (_snakeScene == null)
                {
                    GD.PrintErr("Can't load Snake scene!");
                    return null;
                }
            }

            return _snakeScene.Instantiate<Snake>();
        }

        // Resets the game state, destroying the old snake and creating a new one, resetting the score, and placing a new apple and nuclear waste.
        public void ResetGame()
        {
            DestroySnake();

            _snake = CreateSnake();
            AddChild(_snake);

            Score = 0;

            ReplaceApple();

            ReplaceNuclearWaste();
        }

        // Destroys the current snake and releases its cell in the grid.
        public void DestroySnake()
        {
            if (_snake != null)
            {
                Vector2I snakePosition = _snake.GridPosition;
                Grid.ReleaseCell(snakePosition);

                _snake.QueueFree();
                _snake = null;
            }
        }

        // Replaces the nuclear waste with a new one at a random free cell.
        public void ReplaceNuclearWaste()
        {
            if (_nuclearWaste != null)
            {
                Grid.ReleaseCell(_nuclearWaste.GridPosition);

                _nuclearWaste.QueueFree();
                _nuclearWaste = null;
            }

            if (_nuclearWasteScene == null)
            {
                // Load the nuclear waste scene if it hasn't been loaded yet.
                _nuclearWasteScene = ResourceLoader.Load<PackedScene>(_nuclearWasteScenePath);
                if (_nuclearWasteScene == null)
                {
                    GD.PrintErr("Can't load Nuclear Waste scene!");
                    return;
                }
            }

            _nuclearWaste = _nuclearWasteScene.Instantiate<NuclearWaste>();
            AddChild(_nuclearWaste);

            Cell freeCell = Grid.GetRandomFreeCell();
            if (Grid.OccupyCell(_nuclearWaste, freeCell.GridPosition))
            {
                _nuclearWaste.SetPosition(freeCell.GridPosition);
            }
        }

        // Replaces the apple with a new one at a random free cell.
        public void ReplaceApple()
        {
            if (_apple != null)
            {
                Grid.ReleaseCell(_apple.GridPosition);

                _apple.QueueFree();
                _apple = null;
            }

            if (_appleScene == null)
            {
                // Load the apple scene if it hasn't been loaded yet.
                _appleScene = ResourceLoader.Load<PackedScene>(_appleScenePath);
                if (_appleScene == null)
                {
                    GD.PrintErr("Can't load apple scene!");
                    return;
                }
            }

            _apple = _appleScene.Instantiate<Apple>();
            AddChild(_apple);

            Cell freeCell = Grid.GetRandomFreeCell();
            if (Grid.OccupyCell(_apple, freeCell.GridPosition))
            {
                _apple.SetPosition(freeCell.GridPosition);
            }
        }
    }
}
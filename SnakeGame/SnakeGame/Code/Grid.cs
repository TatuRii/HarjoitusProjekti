using System.Collections.Generic;
using Godot;

namespace SnakeGame
{
    public partial class Grid : Node2D
    {
        [Export] private string _cellScenePath = "res://Scenes/Cell.tscn";
        [Export] private int _width = 0;
        [Export] private int _height = 0;

        [Export] private Vector2I _cellSize = Vector2I.Zero;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        // Private 2D array to store the cells in the grid.
        private Cell[,] _cells = null;

        // Private list to store the free cells in the grid.
        private List<Cell> _freeCells = new List<Cell>();

        public override void _Ready()
        {
            // Initialize the 2D array of cells with the specified width and height.
            _cells = new Cell[Width, Height];

            // Calculate the offset to center the grid.
            Vector2 offset = new Vector2((_width * _cellSize.X) / 2, (_height * _cellSize.Y) / 2);

            // Calculate half of the cell node size.
            Vector2 halfNode = new Vector2(_cellSize.X / 2f, _cellSize.Y / 2f);

            // Adjust the offset to align the grid correctly.
            offset.X -= halfNode.X;
            offset.Y -= halfNode.Y;

            // Load the cell scene from the specified path.
            PackedScene cellScene = ResourceLoader.Load<PackedScene>(_cellScenePath);
            // Check if the cell scene was loaded successfully.
            if (cellScene == null)
            {
                GD.PrintErr("Cell scene not found");
                return;
            }

            // Loop through each cell in the grid and instantiate the cell scene.
            for (int x = 0; x < _width; ++x)
            {
                for (int y = 0; y < _height; ++y)
                {
                    // Instantiate the cell scene.
                    Cell cell = cellScene.Instantiate<Cell>();
                    // Add the cell as a child of the grid.
                    AddChild(cell);

                    // Set the position of the cell based on its grid coordinates and the offset.
                    cell.Position = new Vector2(x * _cellSize.X, y * _cellSize.Y) - offset;
                    // Set the grid position of the cell.
                    cell.GridPosition = new Vector2I(x, y);

                    // Store the cell in the 2D array.
                    _cells[x, y] = cell;
                    // Add the cell to the list of free cells.
                    _freeCells.Add(cell);
                }
            }
        }

        // Method to get the world position of a cell based on its grid position.
        public bool GetWorldPosition(Vector2I gridPosition, out Vector2 worldPosition)
        {
            // Check if the grid position is valid.
            if (IsInvalidCoordinate(gridPosition))
            {
                worldPosition = Vector2.Zero;
                return false;
            }

            // Get the world position of the cell from its position in the 2D array.
            worldPosition = _cells[gridPosition.X, gridPosition.Y].Position;
            return true;
        }

        // Private method to check if a grid position is invalid.
        private bool IsInvalidCoordinate(Vector2I gridPosition)
        {
            return gridPosition.X < 0 || gridPosition.Y < 0
                || gridPosition.X >= Width || gridPosition.Y >= Height;
        }

        // Method to occupy a cell with a cell occupier.
        public bool OccupyCell(ICellOccupier occupier, Vector2I gridPosition)
        {
            // Check if the grid position is valid.
            if (IsInvalidCoordinate(gridPosition))
            {
                return false;
            }

            // Get the cell from the 2D array.
            Cell cell = _cells[gridPosition.X, gridPosition.Y];
            // Try to occupy the cell.
            bool canOccupy = cell.Occupy(occupier);
            // If the cell was occupied successfully, remove it from the list of free cells.
            if (canOccupy)
            {
                _freeCells.Remove(cell);
            }

            return canOccupy;
        }

        // Method to release a cell.
        public bool ReleaseCell(Vector2I gridPosition)
        {
            // Check if the grid position is valid.
            if (IsInvalidCoordinate(gridPosition))
            {
                return false;
            }

            // Get the cell from the 2D array.
            Cell cell = _cells[gridPosition.X, gridPosition.Y];
            // Release the cell.
            cell.Release();
            // Add the cell back to the list of free cells.
            _freeCells.Add(cell);

            return true;
        }

        // Method to get a random free cell.
        public Cell GetRandomFreeCell()
        {
            // Get a random index from the list of free cells.
            int randomIndex = GD.RandRange(0, _freeCells.Count - 1);
            // Return the cell at the random index.
            return _freeCells[randomIndex];
        }

        // Method to check if a cell at a grid position has a collectable.
        public bool HasCollectable(Vector2I gridPosition)
        {
            // Check if the grid position is valid.
            if (IsInvalidCoordinate(gridPosition))
            {
                return false;
            }

            // Get the cell from the 2D array.
            Cell cell = _cells[gridPosition.X, gridPosition.Y];
            // Check if the cell's occupier is a collectable.
            return cell.Occupier.Type == CellOccupierType.Collectable;
        }

        // Method to get the collectable at a grid position.
        public Collectable GetCollectable(Vector2I gridPosition)
        {
            // Check if the grid position is valid.
            if (IsInvalidCoordinate(gridPosition))
            {
                return null;
            }

            // Get the cell from the 2D array.
            Cell cell = _cells[gridPosition.X, gridPosition.Y];
            // Check if the cell's occupier is a collectable and return it.
            if (cell.Occupier is Collectable)
            {
                return cell.Occupier as Collectable;
            }

            return null;
        }
    }
}
using Godot;
using System;

namespace SnakeGame
{
    public partial class Cell : Sprite2D
    {
        // Static read-only instance of an empty cell, used to represent an unoccupied cell.
        public static EmptyCell Empty = new EmptyCell();

        // Private field to store the grid position of the cell.
        private Vector2I _gridPosition;

        // Public property to access and modify the grid position of the cell.
        public Vector2I GridPosition
        {
            get { return _gridPosition; }
            set
            {
                // Ensure the grid position is not negative.
                Vector2I newValue = value;
                if (newValue.X < 0)
                {
                    newValue.X = 0;
                }

                if (newValue.Y < 0)
                {
                    newValue.Y = 0;
                }

                _gridPosition = newValue;
            }
        }

        // Public property to access the occupier of the cell.
        public ICellOccupier Occupier
        {
            get;
            private set;
        }

        // Public property to check if the cell is free (not occupied).
        public bool IsFree
        {
            get { return Occupier == null || Occupier.Type == CellOccupierType.None; }
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Initialize the cell as empty.
            Occupier = Empty;
        }

        // Attempts to occupy the cell with the given occupier.
        public bool Occupy(ICellOccupier occupier)
        {
            // If the cell is not free, return false.
            if (!IsFree)
            {
                return false;
            }

            // Occupy the cell with the given occupier.
            Occupier = occupier;
            return true;
        }

        // Releases the cell, making it empty.
        public void Release()
        {
            // Set the occupier to the empty cell.
            Occupier = Empty;
        }
    }
}
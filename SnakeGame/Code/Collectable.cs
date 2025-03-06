using Godot;

namespace SnakeGame
{
    // Abstract class representing collectable items in the game.
    public abstract partial class Collectable : Sprite2D, ICellOccupier
    {
        // Property to define the type of cell occupier.
        public CellOccupierType Type => CellOccupierType.Collectable;

        // Property to access the current level's grid.
        public Grid Grid
        {
            get { return Level.Current.Grid; }
        }

        // Property to store the grid position of the collectable.
        public Vector2I GridPosition { get; set; }

        // Method to set the world position of the collectable based on its grid position.
        public bool SetPosition(Vector2I gridPosition)
        {
            // Try to get the world position from the grid based on the grid position.
            if (Grid.GetWorldPosition(gridPosition, out Vector2 worldPosition))
            {
                // Set the position of the sprite to the calculated world position.
                Position = worldPosition;
                // Update the grid position property.
                GridPosition = gridPosition;
                // Return true to indicate successful position setting.
                return true;
            }

            // Return false if the grid position could not be converted to a world position.
            return false;
        }

        // Abstract method to define the behavior when the collectable is collected by a snake.
        public abstract void Collect(Snake snake);
    }
}
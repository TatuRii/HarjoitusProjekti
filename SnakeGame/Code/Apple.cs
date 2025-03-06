using Godot;
using System;

namespace SnakeGame
{
    public partial class Apple : Collectable
    {
        // Exported variable to set the score value of the apple in the Godot editor.
        [Export] private int _score = 10;

        // Override the Collect method from the Collectable base class.
        public override void Collect(Snake snake)
        {
            // Increase the current level's score by the apple's score value.
            Level.Current.Score += _score;

            // Replace the current apple with a new one in the level.
            Level.Current.ReplaceApple();

            // Replace the nuclear waste with a new one in the level.
            Level.Current.ReplaceNuclearWaste();
        }
    }
}
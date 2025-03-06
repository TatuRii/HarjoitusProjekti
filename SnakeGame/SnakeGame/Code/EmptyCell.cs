namespace SnakeGame
{
	public class EmptyCell : ICellOccupier
	{
		public CellOccupierType Type => CellOccupierType.None;
	}
}

using System.Collections.Generic;
using Godot;

namespace SnakeGame
{
	public partial class Grid : Node2D
	{
		[Export] private string _cellScenePath = "res://Levels/Cell.tscn";
		[Export] private int _width = 15;
		[Export] private int _height = 13;

		// Vector2I on integeriä kullekin koordinaatille yksikkönä käyttävä vektorityyppi.
		[Export] private Vector2I _cellSize = new Vector2I(16, 16);

		public int Width { get { return _width; } }
		public int Height { get { return _height; } }

		// Tähän 2-uloitteiseen taulukkoon on tallennettu gridin solut. Alussa taulukkoa ei ole, vaan
		// muuttujassa on tyhjä viittaus (null). Taulukko pitää luoda pelin alussa (esim. _Ready-metodissa).
		private Cell[,] _cells = null;

		public override void _Ready()
		{
			// Alusta _cells taulukko
			_cells = new Cell[_width, _height];

			// Laske se piste, josta taulukon rakentaminen aloitetaan. Koska 1. solu luodaan gridin vasempaan
			// yläkulmaan, on meidän laskettava sitä koordinaattia vastaava piste. Oletetaan Gridin pivot-pisteen
			// olevan kameran keskellä (https://en.wikipedia.org/wiki/Pivot_point).
			Vector2 offset = new Vector2((_width * _cellSize.X) / 2, (_height * _cellSize.Y) / 2);

			// Lataa Cell-scene. Luomme tästä uuden olion kutakin ruutua kohden.
			PackedScene cellScene = ResourceLoader.Load<PackedScene>(_cellScenePath);
			if (cellScene == null)
			{
				GD.PrintErr("Cell sceneä ei löydy! Gridiä ei voi luoda!");
				return;
			}

			// Alustetaan Grid kahdella sisäkkäisellä for-silmukalla.
			for (int x = 0; x < _width; ++x)
			{
				for (int y = 0; y < _height; ++y)
				{
					// Luo uusi olio Cell-scenestä.
					Cell cell = cellScene.Instantiate<Cell>();
					// Lisää juuri luotu Cell-olio gridin Nodepuuhun.
					AddChild(cell);

					// Laske ja aseta ruudun sijainti niin maailman koordinaatistossa kuin
					// ruudukonkin koordinaatistossa. Aseta ruudun sijainti käyttäen cell.Position propertyä.
					cell.Position = new Vector2(x * _cellSize.X, y * _cellSize.Y) - offset;
					// Ruudukon koordinaatit tallennetaan solun tietoihin.
					cell.GridPosition = new Vector2I(x, y);

					// Tallenna ruutu tietorakenteeseen oikealle paikalle.
					_cells[x, y] = cell;
				}
			}
		}

		public bool GetWorldPosition(Vector2I gridPosition, out Vector2 worldPosition)
		{
			if (gridPosition.X < 0 || gridPosition.Y < 0
				|| gridPosition.X >= Width || gridPosition.Y >= Height)
			{
				worldPosition = Vector2.Zero;
				// Koordinaatti ei ole gridillä
				return false;
			}
			// Olettaa Gridin olevan 0,0 koordinaatissa. Voi käyttää myös GlobalPositionia.
			worldPosition = _cells[gridPosition.X, gridPosition.Y].Position;
			return true;
		}

		public void ClearGrid()
		{
			// Käy läpi kaikki ruudukon solut ja vapauta ne
			for (int x = 0; x < _width; ++x)
			{
				for (int y = 0; y < _height; ++y)
				{
					// Varmistetaan, että solu ei ole null
					if (_cells[x, y] != null)
					{
						_cells[x, y].QueueFree();  // Vapautetaan solu
					}
				}
			}

			// Piirrä ruudukko uudelleen
			RedrawGrid();
		}

		public void RedrawGrid()
		{
			// Laske se piste, josta taulukon rakentaminen aloitetaan
			Vector2 offset = new Vector2((_width * _cellSize.X) / 2, (_height * _cellSize.Y) / 2);

			// Lataa Cell-scene
			PackedScene cellScene = ResourceLoader.Load<PackedScene>(_cellScenePath);
			if (cellScene == null)
			{
				GD.PrintErr("Cell sceneä ei löydy! Gridiä ei voi luoda!");
				return;
			}

			// Alustetaan Grid uudelleen
			for (int x = 0; x < _width; ++x)
			{
				for (int y = 0; y < _height; ++y)
				{
					// Luo uusi olio Cell-scenestä
					Cell cell = cellScene.Instantiate<Cell>();
					// Lisää juuri luotu Cell-olio gridin Nodepuuhun
					AddChild(cell);

					// Laske ja aseta ruudun sijainti niin maailman koordinaatistossa kuin
					// ruudukon koordinaatistossa. Aseta ruudun sijainti käyttäen cell.Position propertyä.
					cell.Position = new Vector2(x * _cellSize.X, y * _cellSize.Y) - offset;

					// Ruudukon koordinaatit tallennetaan solun tietoihin
					cell.GridPosition = new Vector2I(x, y);

					// Tallenna ruutu tietorakenteeseen oikealle paikalle
					_cells[x, y] = cell;
				}
			}
		}
	}
}
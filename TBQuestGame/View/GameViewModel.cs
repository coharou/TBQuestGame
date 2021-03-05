using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.View
{
    public class GameViewModel : ObservableObject
    {
        private Player _player;

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        private Gamestate _gameState;

        public Gamestate Gamestate
        {
            get { return _gameState; }
            set 
            { 
                _gameState = value;
                OnPropertyChanged(nameof(Gamestate));
            }
        }

        private Tiles[,] _mapGrid;

        public Tiles[,] MapGrid
        {
            get { return _mapGrid; }
            set { _mapGrid = value; }
        }


        private TileConstants _c;

        public TileConstants C
        {
            get { return _c; }
            set { _c = value; }
        }


        public GameViewModel()
        {

        }

        private Location _location;

        public Location Location
        {
            get { return _location; }
            set 
            { 
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }


        public GameViewModel(Player player, Location location)
        {
            _player = player;
            C = new TileConstants();
            _location = location;

            MapGrid = _location.TileGrid;

            Gamestate gamestate = new Gamestate();
            _gameState = gamestate;
        }

        public void ChangeGamestates(string type)
        {
            switch (type)
            {
                case "Options":
                    Gamestate.PausedByOptions = true;
                    break;
                case "Traits":
                    Gamestate.PausedByTraits = true;
                    break;
                case "ReturnGame":
                    Gamestate.PausedByOptions = false;
                    Gamestate.PausedByTraits = false;
                    break;
                default:
                    break;
            }
        }

        #region GRID METHODS
        // Applies adequate View -> Viewmodel -> Model translations for building the grid.
        // This ensures that the view does not have to directly reference the model.
        //  ex. Creating a TileConstants object in the view is not necessary.
        //      If it were included, it would distort the separation between the three elements.

        public int GetTotalTilesForGrid()
        {
            return C.TotalTileCount;
        }

        public int GetTotalTilesPerRow()
        {
            return C.TilesPerRow;
        }

        public int GetGridDimensions()
        {
            return C.GridDimensions;
        }

        public int GetTileDimensions()
        {
            return C.TileDimensions;
        }

        public string GetTileImagePath(int x, int y)
        {
            string path = MapGrid[x, y].Path;
            return path;
        }

        public string GetTileName(int x, int y)
        {
            string name = MapGrid[x, y].Name;
            return name;
        }
        #endregion
    }
}

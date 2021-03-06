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
        #region PROPERTIES
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
        #endregion

        #region CONSTRUCTORS
        public GameViewModel(Player player, Location location, Gamestate gamestate)
        {
            _player = player;

            C = new TileConstants();

            _location = location;

            MapGrid = _location.TileGrid;

            _gameState = gamestate;
        }
        #endregion

        #region Gamestate CHANGING METHODS
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
        #endregion

        #region Tile AND Movement METHODS
        public void DoPlayerMovement(string tag)
        {
            // If the player has movement points at their disposal ...
            if (Player.MovementCurrent > 0)
            {
                string[] parts = tag.Split('_');

                int column = UpdateToCoordinates(parts[0]);
                int row = UpdateToCoordinates(parts[1]);

                bool tileInRange = IsTileInPlayerMoveRange(column, row);

                // If the player has sufficient movement points to move to the clicked tile ...
                if (tileInRange == true)
                {
                    bool passable = AreTilesPassable(column, row);

                    // If the tile is passable into, checking if the tiles there are passable as well ...
                    if (passable == true)
                    {

                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }

        private bool AreTilesPassable(int tileCol, int tileRow)
        {
            bool passable = true;

            int pCol = Player.TilePositionColumn;
            int pRow = Player.TilePositionRow;

            int colDist = tileCol - pCol;
            int colDistAbs = Math.Abs(colDist);

            int rowDist = tileRow - pRow;
            int rowDistAbs = Math.Abs(rowDist);

            // If the player is moving vertically or horizontally ...
            // This code should be revised in the future.
            if (colDistAbs == 0 && rowDistAbs != 0)
            {
                if (rowDist < 0)
                {
                    for (int i = -1; i >= rowDist; i--)
                    {
                        int pos = pRow + i;
                        passable = MapGrid[pCol, pos].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= rowDist; i++)
                    {
                        int pos = pRow + i;
                        passable = MapGrid[pCol, pos].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
            }
            else if (colDistAbs != 0 && rowDistAbs == 0)
            {
                if (colDist < 0)
                {
                    for (int i = -1; i >= colDist; i--)
                    {
                        int pos = pCol + i;
                        passable = MapGrid[pos, pRow].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= colDist; i++)
                    {
                        int pos = pCol + i;
                        passable = MapGrid[pos, pRow].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                // Diagonal distance check
            }

            return passable;
        }

        private bool IsTileInPlayerMoveRange(int tileCol, int tileRow)
        {
            bool enoughMoves = false;

            int pCol = Player.TilePositionColumn;
            int pRow = Player.TilePositionRow;
            int pMoves = Player.MovementCurrent;

            if ((pCol < tileCol) && (pCol + pMoves >= tileCol))
            {
                enoughMoves = true;
            }
            if ((pCol > tileCol) && (pCol - pMoves <= tileCol))
            {
                enoughMoves = true;
            }
            if ((pRow < tileRow) && (pRow + pMoves >= tileRow))
            {
                enoughMoves = true;
            }
            if ((pRow > tileRow) && (pRow - pMoves <= tileRow))
            {
                enoughMoves = true;
            }

            return enoughMoves;
        }

        private static int UpdateToCoordinates(string part)
        {
            part = part.Remove(0, 1);
            int coord = int.Parse(part);
            return coord;
        }
        #endregion

        #region Grid METHODS
        // Applies adequate View -> Viewmodel -> Model translations for building the grid.
        // This ensures that the view does not have to directly reference the model.
        //  ex. Creating a TileConstants object in the view is not necessary.
        //      If it were included, it would distort the separation between the three elements.

        public int GetTotalTilesPerRow()
        {
            return C.TilesPerRow;
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

        public string GetCharacterIconPath()
        {
            string path = Player.Icon.Path;
            return path;    
        }

        public (int, int) GetCharacterIconPosition()
        {
            int pos = Player.TilePosition;
            int column, row;

            if (pos == 0)
            {
                column = 0;
                row = 0;
            }
            else
            {
                double col = pos / C.TilesPerRow;
                column = (int)Math.Floor(col);

                row = pos % C.TilesPerRow;
            }

            return (column, row);
        }
        #endregion
    }
}

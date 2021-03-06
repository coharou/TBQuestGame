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
        public bool DoPlayerMovement(string tag)
        {
            bool canMove = false;

            // If the player has movement points at their disposal ...
            if (Player.MovementCurrent > 0)
            {
                (int, int) coord = CharacterCoordinates(tag);
                int column = coord.Item1;
                int row = coord.Item2;

                bool tileInRange = IsTileInPlayerMoveRange(column, row);

                // If the player has sufficient movement points to move to the clicked tile ...
                if (tileInRange == true)
                {
                    bool passable = AreTilesPassable(column, row);

                    // If the tile can be passed into, and the tiles on the way there are passable ...
                    if (passable == true)
                    {
                        canMove = true;

                        Player.TilePositionColumn = column;
                        Player.TilePositionRow = row;
                        Player.TilePosition = CalculateTilePosition(column, row);
                        
                        // Check if this is an exit.

                        // If it is an exit, alter values in the Gamestate,
                        //  create a new location, and update the map grid.

                        // If it isn't, add a turn to the game counter,
                        //  let the enemies move, then continue forward.
                    }
                }
            }

            return canMove;
        }

        public (int, int) CharacterCoordinates(string tag)
        {
            string[] parts = tag.Split('_');

            int column = UpdateToCoordinates(parts[0]);
            int row = UpdateToCoordinates(parts[1]);

            return (column, row);
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
                // Diagonal distance check for movement.
                // Temporarily, this will be set to false.

                // I am afraid that diagonal movement may hasten the pace of the game too much.
                // Players would be able to dodge enemies more easily than they should,
                // and also arrive to the exit very quickly.

                // If the map grid were larger, this wouldn't be as much of an issue.
                // But with it only being 8 by 8 tiles, movement needs to be heavily restricted.

                passable = false;
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

        public int CalculateTilePosition(int column, int row)
        {
            int estimatedTotal = C.TilesPerRow * row;
            int tilesToSubtract = C.TilesPerRow - column;
            int position = estimatedTotal - tilesToSubtract;
            return position;
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

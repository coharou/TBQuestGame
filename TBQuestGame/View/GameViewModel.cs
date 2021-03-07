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

            MatchPlayerPositionToEntrance();

            _player = Player;

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

        public void TurnTransition()
        {
            Gamestate.CanPlayerAct = false;
            Gamestate.TurnCount += 1;
            Player.MovementCurrent = Player.MovementMax;
            Gamestate.CanPlayerAct = true;
        }
        #endregion

        #region Enemy TURN PROCESS
        public void DoEnemyTurn()
        {
            // When enemies are implemented, CanPlayerAct will likely be
            // used to stop players from pressing buttons while the enemy
            // makes their moves. The Gamestate property will be bound to
            // property IsEnabled on the tile buttons of the grid_Map. If false,
            // the buttons will not work. If true, they will, and the player
            // can make their moves. If binding CanPlayerAct to the tile
            // buttons does not work, it will be used in an if statement
            // for the DoPlayerMovement method. In that case, the button
            // presses will not be processed if the property is false.
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
                        UpdatePlayerMovement(column, row);
                        UpdatePlayerPositions(column, row);
                        canMove = true;
                    }
                }
            }

            return canMove;
        }

        public void MatchPlayerPositionToEntrance()
        {
            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    string name = MapGrid[x, y].Name;
                    if (name == "Entrance")
                    {
                        UpdatePlayerPositions(x, y);
                        break;
                    }
                }
            }
        }

        public int RetrievePositionOfEntrance()
        {
            int position = 0;
            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    string name = MapGrid[x, y].Name;
                    if (name == "Entrance")
                    {
                        position = CalculateTilePosition(x, y);
                        break;
                    }
                }
            }

            return position;
        }

        public bool IsTileExit(int column, int row)
        {
            bool isTileExit = false;
            string tileName = MapGrid[column, row].Name;
            if (tileName == "Exit")
            {
                isTileExit = true;
            }
            else
            {
                DoEnemyTurn();
            }

            TurnTransition();

            return isTileExit;
        }

        public void InitiateNewLocation()
        {
            Location standard = new Location(1, "Default", "Initialized when the game loads", Gamestate.RandObj);
            MapGrid = standard.TileGrid;
        }

        public void UpdatePlayerMovement(int tCol, int tRow)
        {
            int pRow = Player.TilePositionRow;
            int pCol = Player.TilePositionColumn;
            int distance = 0;

            if (pRow == tRow)
            {
                distance = Math.Abs(tCol - pCol);
            }

            if(pCol == tCol)
            {
                distance = Math.Abs(tRow - pRow);
            }

            Player.MovementCurrent -= distance;
        }

        public void UpdatePlayerPositions(int column, int row)
        {
            Player.TilePositionColumn = column;
            Player.TilePositionRow = row;
            Player.TilePosition = CalculateTilePosition(column, row);
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

            int position = 0;
            if (estimatedTotal != 0)
            {
                position = estimatedTotal - tilesToSubtract;
            }

            return position;
        }
        #endregion

        #region Grid METHODS
        // Applies adequate View -> Viewmodel -> Model translations for building the grid.
        // This ensures that the view does not have to directly reference the model.
        //  ex. Creating a TileConstants object in the view is not necessary.
        //      If it were included, it would distort the separation between the three elements.

        public int GetTotalGridTiles()
        {
            return C.TotalTileCount;
        }

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
            int column = Player.TilePositionColumn;
            int row = Player.TilePositionRow;

            return (column, row);
        }
        #endregion
    }
}

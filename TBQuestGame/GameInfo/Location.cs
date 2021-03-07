using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Location
    {
        #region PROPERTIES
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Tiles[,] TileGrid { get; set; }
        #endregion

        #region TILE GENERATION
        protected virtual Tiles[,] GenerateTiles(Random randObj, TileConstants C)
        {
            Tiles[,] tiles = new Tiles[C.TilesPerRow, C.TilesPerRow];

            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    tiles[x, y] = MatchTileID(0);
                }
            }

            return tiles;
        }

        private Tiles MatchTileID(int id)
        {
            List<Tiles> t = Data.GameData.GetTilesResources();
            Tiles tile = t.Find(x => x.ID == id);
            return tile;
        }
        #endregion

        #region DOOR GENERATION
        protected virtual Tiles[,] SetDoorPositions(Random randObj, Tiles[,] tiles, TileConstants C)
        {
            (int, int) entry = CalculateDoorPositions(randObj, C);
            tiles[entry.Item1, entry.Item2] = MatchTileID(3);

            (int, int) exit = CalculateDoorPositions(randObj, C);
            tiles[exit.Item1, exit.Item2] = MatchTileID(4);

            return tiles;
        }

        private (int, int) CalculateDoorPositions(Random randObj, TileConstants C)
        {
            int pos = randObj.Next(0, C.TotalTileCount);

            double col = pos / C.TilesPerRow;
            int column = (int)Math.Floor(col);

            int row = pos % C.TilesPerRow;

            return (column, row);
        }
        #endregion

        #region CONSTRUCTOR
        public Location(int id, string name, string description, Random randObj)
        {
            ID = id;
            Name = name;
            Description = description;

            TileConstants C = new TileConstants();

            Tiles[,] tiles = GenerateTiles(randObj, C);
            TileGrid = SetDoorPositions(randObj, tiles, C);
        }
        #endregion
    }
}

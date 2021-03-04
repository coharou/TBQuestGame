using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Location
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Tiles[,] TileGrid { get; set; }

        private TileConstants _c;

        public TileConstants C
        {
            get { return _c; }
            set { _c = value; }
        }

        protected virtual Tiles[,] GeneratePlots(Random randObj)
        {
            Tiles[,] tiles = new Tiles[C.TilesPerRow, C.TilesPerRow];
            Art a = new Art(0, "tile_standard", "/TBQuestGame;component/Assets/tile_standard.png");
            Tiles t = new Tiles(0, "default", a);

            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    tiles[x, y] = t;
                }
            }

            return tiles;
        }

        protected virtual Tiles[,] SetDoorPositions(Random randObj, Tiles[,] tiles)
        {
            List<Tiles> t = Data.GameData.GetTilesResources();
            Tiles tEntry = t.Find(x => x.ID == 3);
            Tiles tExit = t.Find(x => x.ID == 4);

            (int, int) entry = CalculateDoorPositions(randObj);
            tiles[entry.Item1, entry.Item2] = tEntry;

            (int, int) exit = CalculateDoorPositions(randObj);
            tiles[exit.Item1, exit.Item2] = tExit;

            return tiles;
        }

        private (int, int) CalculateDoorPositions(Random randObj)
        {
            int pos = randObj.Next(0, C.TotalTileRandAdj);

            double col = pos / C.TilesPerRow;
            int column = (int)Math.Floor(col);

            int row = pos % C.TilesPerRow;

            return (column, row);
        }

        public Location(int id, string name, string description, Random randObj)
        {
            ID = id;
            Name = name;
            Description = description;

            Tiles[,] tiles = GeneratePlots(randObj);
            tiles = SetDoorPositions(randObj, tiles);
            TileGrid = tiles;
        }
    }
}

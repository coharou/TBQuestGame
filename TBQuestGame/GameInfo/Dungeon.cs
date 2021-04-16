using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Dungeon : Location
    { 
        public Location Location { get; set; }

        public Biome BiomeType { get; set; }

        public enum Biome
        {
            Forest,
            River
        }

        #region CONSTRUCTORS
        public Dungeon(int id, string name, string description, Random randObj, Biome biomeType) :
            base(id, name, description, randObj)
        {
            ID = id;
            Name = name;
            Description = description;
            BiomeType = biomeType;

            TileConstants C = new TileConstants();

            TileGrid = GenerateTiles(randObj, C);
        }
        #endregion

        #region MAP GENERATION
        protected override Tiles[,] GenerateTiles(Random randObj, TileConstants C)
        {
            Tiles[,] tiles = new Tiles[C.TilesPerRow, C.TilesPerRow];

            // Applies Field to all tiles to ensure none are undefined
            tiles = FillAllTilesWithField(tiles, C);

            // Adds the rest of the terrain types for the given dungeon
            tiles = AddAllTerrainTypes(tiles, C, randObj);

            // Spreads the forest types throughout the map for dense clusters
            tiles = SpreadFringeForest(tiles, C, randObj);

            if (BiomeType == Biome.River)
            {
                tiles = GenerateRiverTiles(tiles, C, randObj);
            }

            // Sets the door positions on the grid
            tiles = SetDoorPositions(randObj, tiles, C);

            // Clears most of the impassable terrain around the doors
            tiles = ClearAreaAroundDoors(randObj, tiles, C);
            return tiles;
        }

        private Tiles[,] FillAllTilesWithField(Tiles[,] tiles, TileConstants c)
        {
            for (int x = 0; x < c.TilesPerRow; x++)
            {
                for (int y = 0; y < c.TilesPerRow; y++)
                {
                    tiles[x, y] = MatchTile(5);
                }
            }

            return tiles;
        }

        private Tiles[,] AddAllTerrainTypes(Tiles[,] tiles, TileConstants c, Random randObj)
        {
            for (int x = 0; x < c.TilesPerRow; x++)
            {
                for (int y = 0; y < c.TilesPerRow; y++)
                {
                    // Thorns and rocks will be removed until their art is revised

                    int val = randObj.Next(0, 100);
                    if (val >= 0 && val <= 11)
                    {
                        tiles[x, y] = MatchTile(2);
                    }
                    else if (val > 17 && val <= 55)
                    {
                        tiles[x, y] = MatchTile(5);
                    }
                    else if (val > 55 && val <= 100)
                    {
                        tiles[x, y] = MatchTile(6);
                    }
                    else
                    {
                        tiles[x, y] = MatchTile(5);
                    }
                }
            }

            return tiles;
        }

        private Tiles[,] SpreadFringeForest(Tiles[,] tiles, TileConstants c, Random randObj)
        {
            for (int x = 0; x < c.TilesPerRow; x++)
            {
                for (int y = 0; y < c.TilesPerRow; y++)
                {
                    Tiles tile = tiles[x, y];
                    Tiles fringe = MatchTile(6);
                    Tiles dense = MatchTile(2);

                    if (tile.Name == fringe.Name)
                    {
                        tiles = ReplaceSurroundingTiles(x, y, tiles, tile, fringe, c, randObj, 20);
                    }
                    if (tile.Name == dense.Name)
                    {
                        tiles = ReplaceSurroundingTiles(x, y, tiles, tile, dense, c, randObj, 3);
                    }
                }
            }

            return tiles;
        }

        private Tiles[,] GenerateRiverTiles(Tiles[,] tiles, TileConstants C, Random randObj)
        {
            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    if (x == 1 || x == 6)
                    {
                        tiles[y, x] = GenerateBetweenTwoTileTypes(C, randObj, MatchTileID(8), MatchTileID(5), 75);
                    }
                    if (x == 2 || x == 5)
                    {
                        tiles[y, x] = GenerateBetweenTwoTileTypes(C, randObj, MatchTileID(9), MatchTileID(1), 95);
                    }
                    if (x == 3 || x == 4)
                    {
                        tiles[y, x] = GenerateBetweenTwoTileTypes(C, randObj, MatchTileID(9), MatchTileID(1), 80);
                    }
                }
            }

            return tiles;
        }

        private Tiles GenerateBetweenTwoTileTypes(TileConstants C, Random randObj, Tiles desired, Tiles alternate, int chance)
        {
            Tiles tile = MatchTile(5);

            int value = randObj.Next(0, 100);

            if (value <= chance)
            {
                tile = desired;
            }
            else
            {
                tile = alternate;
            }

            return tile;
        }

        protected override Tiles[,] SetDoorPositions(Random randObj, Tiles[,] tiles, TileConstants C)
        {
            // The number of tiles the door can be spawned at from 0 => start
            int start = 8;

            // The number of tiles the door can be spawned at from last => 64
            int last = C.TotalTileCount - start;

            int exit = randObj.Next(0, start);
            int entry = randObj.Next(last, C.TotalTileCount);

            int exitX, exitY, entryX, entryY;
            (exitX, exitY) = GetColumnAndRow(exit, C);
            (entryX, entryY) = GetColumnAndRow(entry, C);

            tiles[entryY, entryX] = MatchTileID(3);
            tiles[exitY, exitX] = MatchTileID(4);

            return tiles;
        }

        private Tiles[,] ClearAreaAroundDoors(Random randObj, Tiles[,] tiles, TileConstants c)
        {
            Tiles entry = MatchTile(3);
            Tiles exit = MatchTile(4);
            Tiles fields = MatchTile(5);

            for (int x = 0; x < c.TilesPerRow; x++)
            {
                for (int y = 0; y < c.TilesPerRow; y++)
                {
                    Tiles tile = tiles[x, y];

                    if (tile.Name == entry.Name || tile.Name == exit.Name)
                    {
                        ClearSurroundingImpassableTiles(x, y, tiles, c, fields);
                    }
                }
            }

            return tiles;
        }

        private Tiles[,] ClearSurroundingImpassableTiles(int x, int y, Tiles[,] tiles, TileConstants C, Tiles desired)
        {
            int right = x + 1;

            if (x == (C.TilesPerRow - 1))
            {
                right = C.TilesPerRow - 1;
            }

            int left = x - 1;

            if (x == 0)
            {
                left = 0;
            }

            int above = y - 1;

            if (y == 0)
            {
                above = 0;
            }

            int below = y + 1;

            if (y == (C.TilesPerRow - 1))
            {
                below = C.TilesPerRow - 1;
            }

            if (tiles[right, y].Passable == false)
            {
                tiles[right, y] = desired;
            }
            else if (tiles[left, y].Passable == false)
            {
                tiles[left, y] = desired;
            }
            else if (tiles[x, below].Passable == false)
            {
                tiles[x, below] = desired;
            }
            else if (tiles[x, above].Passable == false)
            {
                tiles[x, above] = desired;
            }

            return tiles;
        }

        private Tiles[,] ReplaceSurroundingTiles(int x, int y, Tiles[,] tiles, Tiles tile, Tiles sampler, TileConstants c, Random randObj, int chance)
        {
            Tiles plains = MatchTile(5);

            int right = x + 1;

            if (x == (c.TilesPerRow - 1))
            {
                right = c.TilesPerRow - 1;
            }

            int left = x - 1;

            if (x == 0)
            {
                left = 0;
            }

            int above = y - 1;

            if (y == 0)
            {
                above = 0;
            }

            int below = y + 1;

            if (y == (c.TilesPerRow - 1))
            {
                below = c.TilesPerRow - 1;
            }

            int value = randObj.Next(0, 100);

            if (tiles[right, y].Name == plains.Name)
            {
                if (value < chance)
                {
                    tiles[right, y] = sampler;
                }
            }
            else if (tiles[left, y].Name == plains.Name)
            {
                if (value < chance)
                {
                    tiles[left, y] = sampler;
                }
            }
            else if (tiles[x, below].Name == plains.Name)
            {
                if (value < chance)
                {
                    tiles[x, below] = sampler;
                }
            }
            else if (tiles[x, above].Name == plains.Name)
            {
                if (value < chance)
                {
                    tiles[x, above] = sampler;
                }
            }

            return tiles;
        }
        #endregion

        #region HELPERS
        private (int, int) GetColumnAndRow(int pos, TileConstants C)
        {
            double col = pos / C.TilesPerRow;
            int column = (int)Math.Floor(col);

            int row = pos % C.TilesPerRow;

            return (column, row);
        }

        private Tiles MatchTile(int id)
        {
            List<Tiles> t = Data.GameData.GetTilesResources();
            Tiles tile = t.Find(x => x.ID == id);
            return tile;
        }
        #endregion
    }
}

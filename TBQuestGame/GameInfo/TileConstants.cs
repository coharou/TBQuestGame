﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class TileConstants
    {
        #region PROPERTIES
        public int TilesPerRow;
        public int TotalTileCount;
        public int TotalTileRandAdj;
        public int GridDimensions;
        public int TileDimensions;
        #endregion

        #region CONSTRUCTOR
        public TileConstants()
        {
            TilesPerRow = 8;
            TotalTileCount = TilesPerRow * TilesPerRow;
            TotalTileRandAdj = TotalTileCount + 1;
            GridDimensions = 480;
            TileDimensions = (int)Math.Floor((decimal)GridDimensions / TilesPerRow);
        }
        #endregion
    }
}

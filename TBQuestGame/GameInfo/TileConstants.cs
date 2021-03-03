using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class TileConstants
    {
        public int TilesPerRow = 8;
        public int TotalTileCount;

        public TileConstants()
        {
            TotalTileCount = TilesPerRow * TilesPerRow;
        }
    }
}

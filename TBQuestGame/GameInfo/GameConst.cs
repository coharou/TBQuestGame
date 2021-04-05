using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class GameConst
    {
        public int COINS_FROM_KILL;
        public int COINS_FROM_COMPLETING_LAYER;
        public int COINS_FROM_COMPLETING_DUNGEON;

        public GameConst()
        {
            COINS_FROM_KILL = 50;
            COINS_FROM_COMPLETING_LAYER = 100;
            COINS_FROM_COMPLETING_DUNGEON = 500;
        }
    }
}

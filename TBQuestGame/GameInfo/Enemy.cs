using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Enemy : Combatant
    {
        public int PrevPosition { get; set; }

        private Moves _move;

        public Moves Move
        {
            get { return _move; }
            set { _move = value; }
        }

        public Enemy(int id, string name, int locationId, int tilePosition, Art icon, Role role, SoldierRole extendedRole, Moves move):
            base(id, name, locationId, tilePosition, icon, role, extendedRole)
        {
            PrevPosition = tilePosition;
            Move = move;
        }
    }
}

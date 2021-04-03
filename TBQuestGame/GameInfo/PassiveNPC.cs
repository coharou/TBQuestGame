using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class PassiveNPC : Character, IShop
    {
        public PassiveNPC(int id, string name, int locationId, int tilePosition, Art icon, Character.Role role): 
            base(id, name, locationId, tilePosition, icon, role)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Player : Combatant
    {
        #region TRAITS
        // TraitChosenFirst prop

        // TraitChosenSecond prop

        // TraitRandomPos prop

        // TraitRandomNeg prop
        #endregion

        #region SKILLS (traits derived)
        public int Foraging { get; set; }

        public int Looting { get; set; }

        public int Charisma { get; set; }
        #endregion

        #region TANGIBLES
        public int Experience { get; set; }

        // Inventory prop

        public int Coins { get; set; }
        #endregion

        public Player(int id, string name, int locationId): 
            base (id, name, locationId)
        {
            ID = id;
            Name = name;
            LocationID = locationId;
        }
    }
}

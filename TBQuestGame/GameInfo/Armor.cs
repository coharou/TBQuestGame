using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Armor : GameObject
    {
        #region RESISTANCES
        public int ResistRanged { get; set; }

        public int ResistMelee { get; set; }

        public int ResistGunpowder { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Armor(int id, string name, string description, int resistRanged, int resistMelee, int resistGunpowder):
            base (id, name, description)
        {
            ResistRanged = resistRanged;
            ResistMelee = resistMelee;
            ResistGunpowder = resistGunpowder;
        }
        #endregion
    }
}

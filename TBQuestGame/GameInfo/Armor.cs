using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Armor
    {
        #region TEXT DETAILS
        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        #region RESISTANCES
        public int ResistRanged { get; set; }

        public int ResistMelee { get; set; }

        public int ResistGunpowder { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Armor(string name, string description, int resistRanged, int resistMelee, int resistGunpowder)
        {
            Name = name;
            Description = description;
            ResistRanged = resistRanged;
            ResistMelee = resistMelee;
            ResistGunpowder = resistGunpowder;
        }
        #endregion
    }
}

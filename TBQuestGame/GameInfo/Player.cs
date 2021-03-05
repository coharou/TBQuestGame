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
        public Traits TraitChosenFirst { get; set; }

        public Traits TraitChosenSecond { get; set; }

        public Traits TraitRandomPos { get; set; }

        public Traits TraitRandomNeg { get; set; }
        #endregion

        #region SKILLS (traits derived)
        public int Foraging { get; set; }

        public int Looting { get; set; }

        public int Charisma { get; set; }
        #endregion

        #region TANGIBLES
        public int Experience { get; set; }
        public int ExperienceMax { get; set; }

        // Inventory prop - to be added later

        public int Coins { get; set; }
        #endregion

        public int TilePositionRow { get; set; }

        public int TilePositionColumn { get; set; }

        private int DetermineRowPosition(int tilePosition)
        {
            int pos = tilePosition;
            int row;

            if (pos == 0)
            {
                row = 0;
            }
            else
            {
                TileConstants C = new TileConstants();
                row = pos % C.TilesPerRow;
            }

            return row;
        }

        private int DetermineColumnPosition(int tilePosition)
        {
            int pos = tilePosition;
            int column;

            if (pos == 0)
            {
                column = 0;
            }
            else
            {
                TileConstants C = new TileConstants();
                double col = pos / C.TilesPerRow;
                column = (int)Math.Floor(col);
            }
            return column;
        }

        #region CONSTRUCTOR
        public Player(int id, string name, int locationId, int tilePosition, Art icon, Role role, SoldierRole extendedRole) :
            base(id, name, locationId, tilePosition, icon, role, extendedRole)
        {
            TilePositionRow = DetermineRowPosition(tilePosition);
            TilePositionColumn = DetermineColumnPosition(tilePosition);

            HealthBase = 100;
            HealthCurrent = HealthBase;
            HealthMax = HealthBase;
            HealthRegenerationBase = 1;
            HealthRegenerationRate = HealthRegenerationBase;
            IsHealthRegenerating = true;

            // Ignore armor - assigned during player customization

            DefenseModRanged = 0;
            DefenseModMelee = 0;
            DefenseModGunpowder = 0;
            MovementBase = 1;
            MovementCurrent = MovementBase;
            MovementMax = MovementBase;

            // Ignore power and valor - applied during combat

            StatusInflictMod = 0;
            IsBleeding = false;
            IsBurning = false;
            IsPoisoned = false;
            StrengthModGunpowder = 0;
            StrengthModMelee = 0;
            StrengthModRanged = 0;
            AccuracyModGunpowder = 0;
            AccuracyModMelee = 0;
            AccuracyModRanged = 0;

            // Ignore combatant moves - assigned during player customization

            ResourcefulnessMod = 0;

            // Ignore player traits - assigned during player customization

            Foraging = 0;
            Charisma = 0;
            Looting = 0;
            Experience = 0;
            ExperienceMax = 100;

            // Ignore inventory - assigned during player customization

            Coins = 0;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Combatant : Character
    {
        #region HEALTH
        public int HealthCurrent { get; set; }

        public int HealthMax { get; set; }

        public int HealthBase { get; set; }

        public bool IsHealthRegenerating { get; set; }

        public int HealthRegenerationRate { get; set; }

        public int HealthRegenerationBase { get; set; }
        #endregion

        #region ARMOR
        // Armor class required for the armor prop

        public int DefenseModRanged { get; set; }

        public int DefenseModMelee { get; set; }

        public int DefenseModGunpowder { get; set; }
        #endregion

        #region MOVEMENT
        public int MovementCurrent { get; set; }

        public int MovementMax { get; set; }

        public int MovementBase { get; set; }
        #endregion

        #region POWER / VALOR
        public int Power { get; set; }

        public int PowerModifier { get; set; }

        public int Valor { get; set; }
        #endregion

        #region STATUS EFFECTS
        public int StatusInflictMod { get; set; }

        public bool IsPoisoned { get; set; }

        public bool IsBurning { get; set; }

        public bool IsBleeding { get; set; }
        #endregion

        #region STRENGTH
        public int StrengthModRanged { get; set; }

        public int StrengthModMelee { get; set; }

        public int StrengthModGunpowder { get; set; }
        #endregion

        #region ACCURACY
        public int AccuracyModRanged { get; set; }

        public int AccuracyModMelee { get; set; }

        public int AccuracyModGunpowder { get; set; }
        #endregion

        #region MOVES (attacks)

        // MoveOne prop

        // MoveTwo prop

        // MoveThree prop

        // MoveFour prop

        // MoveFive prop

        // MoveSix prop

        public int ResourcefulnessMod { get; set; }
        #endregion

        public Combatant(int id, string name, int locationId):
            base (id, name, locationId)
        {
            ID = id;
            Name = name;
            LocationID = locationId;
        }
    }
}

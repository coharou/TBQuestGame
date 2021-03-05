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
        public Armor ArmorType { get; set; }

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

        public Moves MoveOne { get; set; }

        public Moves MoveTwo { get; set; }

        public Moves MoveThree { get; set; }

        public Moves MoveFour { get; set; }

        public Moves MoveFive { get; set; }

        public Moves MoveSix { get; set; }

        public int ResourcefulnessMod { get; set; }
        #endregion

        #region NEW ROLE
        public SoldierRole ExtendedRole { get; set; }

        public enum SoldierRole
        {
            Pikeman,
            Crossbowman,
            Archer,
            Knight,
            Lancer,
            Musketeer
        }

        public override string FullTitle()
        {
            string title = $"{Name} the {ExtendedRole} {Descriptor}";
            return title;
        }
        #endregion

        #region CONSTRUCTOR
        public Combatant(int id, string name, int locationId, int tilePosition, Role role, SoldierRole extendedRole) :
            base(id, name, locationId, tilePosition, role)
        {
            ID = id;
            Name = name;
            RoleDescriptor = role;
            ExtendedRole = extendedRole;
            LocationID = locationId;
            TilePosition = tilePosition;
            HealthBase = 100;
            HealthCurrent = HealthBase;
            HealthMax = HealthBase;
            HealthRegenerationBase = 1;
            HealthRegenerationRate = HealthRegenerationBase;
            IsHealthRegenerating = true;

            // Ignore armor - assigned in the GameData class

            DefenseModRanged = 0;
            DefenseModMelee = 0;
            DefenseModGunpowder = 0;
            MovementBase = 1;
            MovementCurrent = MovementBase;
            MovementMax = MovementBase;

            // Ignore power and valor - assigned during combat

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

            // Ignore combatant moves - assigned in the GameData class

            ResourcefulnessMod = 0;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Moves
    {
        #region TEXT DETAILS
        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        #region DAMAGE
        public DamageType DamageClass { get; set; }

        public enum DamageType
        {
            Melee,
            Ranged,
            Gunpowder
        }

        public int Damage { get; set; }
        #endregion

        #region ACCURACY
        public int Accuracy { get; set; }
        #endregion

        #region STATUS EFFECTS
        public StatusType StatusEffect { get; set; }

        public enum StatusType
        {
            Bleeding,
            Burning,
            Poisoning,
            None
        }

        public int StatusChances { get; set; }
        #endregion

        #region ITEM REQUIREMENTS
        public bool AreResourcesRequired { get; set; }

        public Ammunition AmmunitionType { get; set; }

        public enum Ammunition
        {
            Gunpowder,
            Arrow,
            None
        }
        #endregion

        #region CONSTRUCTOR
        public Moves(string name, string description, DamageType damageClass, int damage, int accuracy, StatusType status, int statusChance, bool isItemUsed, Ammunition ammunition)
        {
            Name = name;
            Description = description;
            DamageClass = damageClass;
            Damage = damage;
            Accuracy = accuracy;

            if (status == StatusType.None)
            {
                statusChance = 0;
            }
            StatusChances = statusChance;

            AreResourcesRequired = isItemUsed;
            if (isItemUsed == false)
            {
                ammunition = Ammunition.None;
            }
            AmmunitionType = ammunition;
        }
        #endregion
    }
}

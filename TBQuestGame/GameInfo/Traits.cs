using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Traits : GameObject
    {
        #region CONSTRUCTOR
        public Traits(int id, string name, string description, bool isPositive):
            base (id, name, description)
        {
            IsPositive = isPositive;
        }
        #endregion

        #region POSITIVITY

        private bool _isPositive;

        public bool IsPositive
        {
            get { return _isPositive; }
            set { _isPositive = value; }
        }

        #endregion

        #region PROPERTIES
        public int HealthMod { get; set; }

        public int RegenMod { get; set; }

        public int CoinBonus { get; set; }

        public int AmmoUseMod { get; set; }

        public int RangedStrMod { get; set; }

        public int GunpowderStrMod { get; set; }

        public int ForagingMod { get; set; }

        public int StatusEffectMod { get; set; }

        public int AccuracyMod { get; set; }

        public int ExperienceBonus { get; set; }

        public int MeleeStrMod { get; set; }

        public int DefenseMod { get; set; }

        public int MerchantInfluence { get; set; }
        #endregion
    }
}

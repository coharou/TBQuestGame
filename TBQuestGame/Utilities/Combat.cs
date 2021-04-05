using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.Utilities
{
    public class Combat
    {
        public Combatant ProcessAttack(Combatant agg, Combatant def, Random ran)
        {
            if (DoesAttackHit(agg, ran))
            {
                int dmg = AggDamage(agg);
                double dfn = DefDefense(agg, def);
                
                def.HealthCurrent -= CalcRealDmg(dmg, dfn);

                if (agg.SelectedMove.StatusEffect != Moves.StatusType.None)
                {
                    def = ApplyStatusEffect(agg, def, ran);
                }
            }

            return def;
        }

        private bool DoesAttackHit(Combatant com, Random ran)
        {
            int mod = 0;

            Moves.DamageType type = com.SelectedMove.DamageClass;
            switch (type)
            {
                case Moves.DamageType.Gunpowder:
                    mod = com.AccuracyModGunpowder;
                    break;
                case Moves.DamageType.Ranged:
                    mod = com.AccuracyModRanged;
                    break;
                case Moves.DamageType.Melee:
                    mod = com.AccuracyModMelee;
                    break;
                default:
                    break;
            }

            int accuracy = AdjustedValueFromModifier(com.SelectedMove.Accuracy, mod);
            int rng = ran.Next(0, 100);

            if (accuracy >= rng)
            {
                return true;
            }

            return false;
        }

        private int AggDamage(Combatant agg)
        {
            int mod = 0;

            Moves.DamageType type = agg.SelectedMove.DamageClass;
            switch (type)
            {
                case Moves.DamageType.Gunpowder:
                    mod = agg.StrengthModGunpowder;
                    break;
                case Moves.DamageType.Ranged:
                    mod = agg.StrengthModRanged;
                    break;
                case Moves.DamageType.Melee:
                    mod = agg.StrengthModMelee;
                    break;
                default:
                    break;
            }

            return AdjustedValueFromModifier(agg.SelectedMove.Damage, mod);
        }

        private int AdjustedValueFromModifier(int initial, int iModifier)
        {
            double dModifier = Convert.ToDouble(iModifier);
            dModifier /= 100;
            int iExtra = (int)Math.Floor(dModifier * initial);
            int total = initial + iExtra;
            return total;
        }

        private double DefDefense(Combatant agg, Combatant def)
        {
            double dfn = 0;

            Moves.DamageType type = agg.SelectedMove.DamageClass;
            switch (type)
            {
                case Moves.DamageType.Gunpowder:
                    dfn += def.DefenseModGunpowder + def.ArmorType.ResistGunpowder;
                    break;
                case Moves.DamageType.Ranged:
                    dfn += def.DefenseModRanged + def.ArmorType.ResistRanged;
                    break;
                case Moves.DamageType.Melee:
                    dfn += def.DefenseModMelee + def.ArmorType.ResistMelee;
                    break;
                default:
                    break;
            }

            double dfnMod = dfn / 100;
            return dfnMod;
        }

        private int CalcRealDmg(int dmg, double dfn)
        {
            int sub = (int)Math.Floor(dfn * dmg);
            dmg -= sub;
            return dmg;
        }

        private Combatant ApplyStatusEffect(Combatant agg, Combatant def, Random ran)
        {
            int stat = agg.StatusInflictMod + agg.SelectedMove.StatusChances;
            int rng = ran.Next(0, 100);

            if (stat >= rng)
            {
                Moves.StatusType type = agg.SelectedMove.StatusEffect;
                switch (type)
                {
                    case Moves.StatusType.Bleeding:
                        def.IsBleeding = true;
                        break;
                    case Moves.StatusType.Burning:
                        def.IsBurning = true;
                        break;
                    case Moves.StatusType.Poisoning:
                        def.IsPoisoned = true;
                        break;
                    default:
                        break;
                }
            }

            return def;
        }
    }
}

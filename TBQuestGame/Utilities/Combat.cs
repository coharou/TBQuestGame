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
            bool doesMoveLand = AccuracyCheck(agg, ran);
            if (doesMoveLand == true)
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

        private bool AccuracyCheck(Combatant com, Random ran)
        {
            bool doesAttackLand = false;
            int accMod = 0;

            Moves.DamageType type = com.SelectedMove.DamageClass;
            switch (type)
            {
                case Moves.DamageType.Gunpowder:
                    accMod = com.AccuracyModGunpowder;
                    break;
                case Moves.DamageType.Ranged:
                    accMod = com.AccuracyModRanged;
                    break;
                case Moves.DamageType.Melee:
                    accMod = com.AccuracyModMelee;
                    break;
                default:
                    break;
            }

            int acc = com.SelectedMove.Accuracy;
            double dMod = accMod / 100;
            int iMod = (int)Math.Floor(dMod * acc);
            int fullAcc = acc + iMod;

            int rng = ran.Next(0, 100);
            if (fullAcc >= rng)
            {
                doesAttackLand = true;
            }

            return doesAttackLand;
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

            int wpnDmg = agg.SelectedMove.Damage;
            double extraDmg = (mod * wpnDmg) / 100;
            int dmgAdj = (int)Math.Floor(extraDmg + wpnDmg);

            return dmgAdj;
        }

        private double DefDefense(Combatant agg, Combatant def)
        {
            int dfn = 0;

            Moves.DamageType type = agg.SelectedMove.DamageClass;
            switch (type)
            {
                case Moves.DamageType.Gunpowder:
                    dfn = def.DefenseModGunpowder;
                    break;
                case Moves.DamageType.Ranged:
                    dfn = def.DefenseModRanged;
                    break;
                case Moves.DamageType.Melee:
                    dfn = def.DefenseModMelee;
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

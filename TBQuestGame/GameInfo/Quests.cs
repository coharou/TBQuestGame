using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Quests
    {
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Moves.DamageType EnemyClass { get; set; }

        public int GoalValue { get; set; }

        private int _currentValue;

        public int CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        public int Reward { get; set; }

        private string BuildDescription()
        {
            string text = $"Defeat {GoalValue} {EnemyClass} enemies for a reward of {Reward}. Enemies defeated: {CurrentValue}.";
            return text;
        }

        private bool WasEnemyDefeated(Enemy enemy)
        {
            if (enemy.SelectedMove.DamageClass == EnemyClass)
            {
                return true;
            }

            return false;
        }

        public bool IsGoalCompleted(Enemy enemy)
        {
            if (WasEnemyDefeated(enemy))
            {
                CurrentValue = UpdateCurrentValue();

                Description = BuildDescription();

                if (CurrentValue >= GoalValue)
                {
                    return true;
                }
            }

            return false;
        }

        private int UpdateCurrentValue()
        {
            return CurrentValue + 1;
        }

        public int ProvidePlayerReward()
        {
            return Reward;
        }

        public Quests()
        {
            CurrentValue = 0;
            EnemyClass = ChooseAnEnemyType();
            GoalValue = NumberToDefeat();
            Reward = DetermineReward();
            Description = BuildDescription();
        }

        private int DetermineReward()
        {
            int basic = 125;

            if (GoalValue == 6)
            {
                return basic;
            }
            else
            {
                for (int i = 6; i < GoalValue; i++)
                {
                    basic += 15;
                    int rng = GetQuestsRNG().Next(0, 100);
                    Console.WriteLine(rng);
                    if (rng <= 45)
                    {
                        basic += 10;
                    }
                }

                return basic;
            }
        }

        private int NumberToDefeat()
        {
            int rng = GetQuestsRNG().Next(6, 13);
            return rng;
        }

        private Moves.DamageType ChooseAnEnemyType()
        {
            Moves.DamageType type = Moves.DamageType.Melee;

            int rng = GetQuestsRNG().Next(0, 100);
            if (rng >= 50)
            {
                type = Moves.DamageType.Ranged;
            }

            return type;
        }

        public Random GetQuestsRNG()
        {
            long tick = DateTime.Now.Ticks;
            int iTick = (int)tick;
            iTick = Math.Abs(iTick);
            Random _random = new Random(iTick);
            return _random;
        }
    }
}

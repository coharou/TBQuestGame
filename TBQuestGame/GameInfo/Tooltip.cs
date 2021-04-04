using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Tooltip : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _currentHP;

        public string CurrentHP
        {
            get { return _currentHP; }
            set 
            { 
                _currentHP = value;
                OnPropertyChanged(nameof(CurrentHP));
            }
        }

        private string _maxHP;

        public string MaxHP
        {
            get { return _maxHP; }
            set 
            { 
                _maxHP = value;
                OnPropertyChanged(nameof(MaxHP));
            }
        }

        private string _armor;

        public string Armor
        {
            get { return _armor; }
            set 
            { 
                _armor = value;
                OnPropertyChanged(nameof(Armor));
            }
        }

        private string _move;

        public string Move
        {
            get { return _move; }
            set 
            {
                _move = value;
                OnPropertyChanged(nameof(Move));
            }
        }


        /// <summary>
        /// Used for displaying information about an object in the view.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currentHP"></param>
        /// <param name="maxHP"></param>
        /// <param name="armor"></param>
        /// <param name="move"></param>
        public Tooltip(string name, string currentHP, string maxHP, string armor, string move)
        {
            Name = name;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            Armor = armor;
            Move = move;
        }
    }
}

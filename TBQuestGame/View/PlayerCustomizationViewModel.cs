using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.View
{
    public class PlayerCustomizationViewModel : ObservableObject
    {
        #region PROPS
        private Armor[] _armors;

        public Armor[] Armors
        {
            get { return _armors; }
            set
            {
                _armors = value;
                OnPropertyChanged(nameof(Armors));
            }
        }

        private Traits[] _traits;

        public Traits[] Traits
        {
            get { return _traits; }
            set
            {
                _traits = value;
                OnPropertyChanged(nameof(Traits));
            }
        }

        private Moves[] _moves;

        public Moves[] Moves
        {
            get { return _moves; }
            set
            {
                _moves = value;
                OnPropertyChanged(nameof(Moves));
            }
        }

        private Player _player;

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        private string _playerMessage;

        public string PlayerMessage
        {
            get { return _playerMessage; }
            set 
            { 
                _playerMessage = value;
                OnPropertyChanged(nameof(PlayerMessage));
            }
        }
        #endregion

        public PlayerCustomizationViewModel(Armor[] armor, Traits[] traits, Moves[] moves, Player player)
        {
            Armors = armor;
            Traits = traits;
            Moves = moves;
            Player = player;
        }

        public Player SendPlayer()
        {
            Player player = Data.GameData.InitPlayer();

            // Enter the player moves, armor, and traits here

            return player;
        }
    }
}

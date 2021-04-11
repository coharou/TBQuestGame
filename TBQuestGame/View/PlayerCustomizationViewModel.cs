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
        private List<Armor> _armors;

        public List<Armor> Armors
        {
            get { return _armors; }
            set
            {
                _armors = value;
                OnPropertyChanged(nameof(Armors));
            }
        }

        private List<Traits> _traits;

        public List<Traits> Traits
        {
            get { return _traits; }
            set
            {
                _traits = value;
                OnPropertyChanged(nameof(Traits));
            }
        }

        private List<Moves> _moves;

        public List<Moves> Moves
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

        #region CONSTRUCTOR
        public PlayerCustomizationViewModel(List<Armor> armor, List<Traits> traits, List<Moves> moves, Player player)
        {
            Armors = armor;
            Traits = traits;
            Moves = moves;
            Player = player;
        }
        #endregion
    }
}

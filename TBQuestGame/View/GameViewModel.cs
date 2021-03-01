using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.View
{
    public class GameViewModel : ObservableObject
    {
        private Player _player;

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        private Gamestate _gameState;

        public Gamestate Gamestate
        {
            get { return _gameState; }
            set 
            { 
                _gameState = value;
                OnPropertyChanged(nameof(Gamestate));
            }
        }

        public GameViewModel()
        {

        }

        public GameViewModel(Player player)
        {
            _player = player;
            Gamestate gamestate = new Gamestate();
            _gameState = gamestate;
        }

        public void ChangeGamestates(string type)
        {
            switch (type)
            {
                case "Options":
                    Gamestate.PausedByOptions = true;
                    break;
                case "Traits":
                    Gamestate.PausedByTraits = true;
                    break;
                case "ReturnGame":
                    Gamestate.PausedByOptions = false;
                    Gamestate.PausedByTraits = false;
                    break;
                default:
                    break;
            }
        }

    }
}

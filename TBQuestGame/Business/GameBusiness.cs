using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;
using TBQuestGame.Data;
using TBQuestGame.View;

namespace TBQuestGame.Business
{
    public class GameBusiness
    {
        #region PROPS
        GameViewModel _gameViewModel;
        Player _player;
        Location _location;
        Gamestate _gamestate;
        #endregion

        #region CONSTRUCTOR
        public GameBusiness()
        {
            _gamestate = new Gamestate(false);

            // Insert the customization methods here to get player data
            // Replace InitPlayer() with the information for the customization window
            _player = GameData.InitPlayer();

            _location = GameData.InitDefaultLocation(_gamestate);

            _gameViewModel = new GameViewModel(_player, _location, _gamestate);

            GameSession gameSession = new GameSession(_gameViewModel);
            gameSession.DataContext = _gameViewModel;
            gameSession.Show();
        }
        #endregion
    }
}

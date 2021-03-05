﻿using System;
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
        GameViewModel _gameViewModel;
        Player _player;
        Location _location;

        public GameBusiness()
        {
            _player = GameData.InitPlayer();
            _location = GameData.InitDefaultLocation();

            // Insert the customization methods here to get player data
            // Replace InitPlayer() with the stuff for the customization window

            _gameViewModel = new GameViewModel(_player, _location);
            GameSession gameSession = new GameSession(_gameViewModel);
            gameSession.DataContext = _gameViewModel;
            gameSession.Show();
        }
    }
}

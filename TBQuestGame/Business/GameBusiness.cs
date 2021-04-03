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
        PlayerCustomizationViewModel _playerCustoms;
        Player _player;
        Gamestate _gamestate;
        Armor[] _armor;
        Moves[] _moves;
        Traits[] _traits;
        List<Item> _items;
        List<Enemy> _enemies;
        List<PassiveNPC> _passives;
        #endregion

        #region CONSTRUCTOR
        public GameBusiness()
        {
            _gamestate = new Gamestate(false);

            _armor = GameData.InitArmor();
            _moves = GameData.InitMoves();
            _traits = GameData.InitTraits();
            _player = GameData.InitPlayer();

            _playerCustoms = new PlayerCustomizationViewModel(_armor, _traits, _moves, _player);
            PlayerCustomization customsSession = new PlayerCustomization(_playerCustoms);
            customsSession.DataContext = _playerCustoms;
            customsSession.ShowDialog();

            _player = _playerCustoms.Player;
            _items = GameData.InitItems();
            _enemies = GameData.InitEnemyTypes();
            _passives = GameData.InitPassiveTypes();
            _gameViewModel = new GameViewModel(_player, _gamestate, _traits, _items, _enemies, _passives);

            GameSession gameSession = new GameSession(_gameViewModel);
            gameSession.DataContext = _gameViewModel;
            gameSession.Show();
            customsSession.Close();
        }
        #endregion
    }
}

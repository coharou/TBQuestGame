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

        List<Armor> _armor;
        List<Armor> _customizeArmor;

        List<Moves> _moves;
        List<Moves> _customizeMoves;

        List<Traits> _traits;
        List<Traits> _customizeTraits;

        List<Item> _items;

        List<Enemy> _enemies;
        List<PassiveNPC> _passives;
        #endregion

        #region CONSTRUCTOR
        public GameBusiness()
        {
            _gamestate = new Gamestate(false);

            GameData.InitArmor(out List<Armor> fullArmor, out List<Armor> customizeArmor);
            _customizeArmor = customizeArmor;
            _armor = fullArmor;
         
            GameData.InitMoves(out List<Moves> fullSet, out List<Moves> customizeSet);
            _customizeMoves = customizeSet;
            _moves = fullSet;

            GameData.InitTraits(out List<Traits> fullTraits, out List<Traits> customizeTraits);
            _customizeTraits = customizeTraits;
            _traits = fullTraits;

            _player = GameData.InitPlayer();

            _playerCustoms = new PlayerCustomizationViewModel(_customizeArmor, _customizeTraits, _customizeMoves, _player);
            PlayerCustomization customsSession = new PlayerCustomization(_playerCustoms)
            {
                DataContext = _playerCustoms
            };
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

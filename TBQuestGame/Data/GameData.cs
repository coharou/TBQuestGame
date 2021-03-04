using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.Data
{
    public class GameData
    {
        public static Player InitPlayer() 
        {
            int id = 1;
            string name = "player_default";
            int locationId = 0;
            Character.Role role = Character.Role.Soldier;
            Player.SoldierRole soldierRole = Player.SoldierRole.Knight;
            Player player = new Player(id, name, locationId, role, soldierRole);

            // Remove when player customization is added
            Moves standard = new Moves("move_default", "default", Moves.DamageType.Melee, 1, 100, Moves.StatusType.None, 0, false, Moves.Ammunition.None);
            player.MoveOne = standard;
            player.MoveTwo = standard;
            player.MoveThree = standard;
            player.MoveFour = standard;
            player.MoveFive = standard;
            player.MoveSix = standard;

            // Remove when player customization is added
            Traits trait_standard = new Traits("DEFAULT TRAIT", "Default text for the default trait, applied before opening the window.");
            player.TraitChosenFirst = trait_standard;
            player.TraitChosenSecond = trait_standard;
            player.TraitRandomNeg = trait_standard;
            player.TraitRandomPos = trait_standard;

            return player;
        }

        public static List<Tiles> GetTilesResources()
        {
            List<Tiles> assets = new List<Tiles>()
            {
                new Tiles(0, "Default", "/TBQuestGame;component/Assets/tile_samples/default.png", "Generic"),
                new Tiles(1, "Deep Water", "/TBQuestGame;component/Assets/tile_samples/deep_water.png", "River"),
                new Tiles(2, "Dense Forest", "/TBQuestGame;component/Assets/tile_samples/dense_forest.png", "General"),
                new Tiles(3, "Entrance", "/TBQuestGame;component/Assets/tile_samples/entrance.png", "Generic"),
                new Tiles(4, "Exit", "/TBQuestGame;component/Assets/tile_samples/exit.png", "Generic"),
                new Tiles(5, "Fields", "/TBQuestGame;component/Assets/tile_samples/fields.png", "General"),
                new Tiles(6, "Forest Fringe", "/TBQuestGame;component/Assets/tile_samples/fringe_forest.png", "General"),
                new Tiles(7, "Rocks", "/TBQuestGame;component/Assets/tile_samples/rocks.png", "General"),
                new Tiles(8, "Sand", "/TBQuestGame;component/Assets/tile_samples/sand.png", "River"),
                new Tiles(9, "Shallow Water", "/TBQuestGame;component/Assets/tile_samples/shallow_water.png", "General"),
                new Tiles(10, "Thorns", "/TBQuestGame;component/Assets/tile_samples/thorns.png", "Forest")
            };
            return assets;
        }

        /*
        public static List<Moves> InitMoves()
        {
            List<Moves> moves = new List<Moves>();
            Moves standard = new Moves("move_default", "default", Moves.DamageType.Melee, 1, 100, Moves.StatusType.None, 0, false, Moves.Ammunition.None);
            moves.Add(standard);
            return moves;
        }
        */
    }
}

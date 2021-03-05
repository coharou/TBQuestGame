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
            string basePath = @"Assets/tile_samples/";

            // The path will need to be updated to the actual assets folder in the solution.

            // Currently, it is going to the Debug folder. This is okay for now, but should be changed later
            // before the game is released.

            List<Tiles> assets = new List<Tiles>()
            {
                new Tiles(0, "Default", $"{basePath}default.png", "Generic"),
                new Tiles(1, "Deep Water", $"{basePath}deep_water.png", "River"),
                new Tiles(2, "Dense Forest", $"{basePath}dense_forest.png", "General"),
                new Tiles(3, "Entrance", $"{basePath}entrance.png", "Generic"),
                new Tiles(4, "Exit", $"{basePath}exit.png", "Generic"),
                new Tiles(5, "Fields", $"{basePath}fields.png", "General"),
                new Tiles(6, "Forest Fringe", $"{basePath}fringe_forest.png", "General"),
                new Tiles(7, "Rocks", $"{basePath}rocks.png", "General"),
                new Tiles(8, "Sand", $"{basePath}sand.png", "River"),
                new Tiles(9, "Shallow Water", $"{basePath}shallow_water.png", "General"),
                new Tiles(10, "Thorns", $"{basePath}thorns.png", "Forest")
            };

            return assets;
        }

        public static Location InitDefaultLocation()
        {
            Gamestate gamestate = new Gamestate();
            Location standard = new Location(0, "Default", "Initialized when the game loads", gamestate.RandObj);
            return standard;
        }
    }
}

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
            int tilePosition = 0;

            string path = "pack://application:,,,/Assets/character_icons/";
            Art icon = new Art(0, "Player", path + "default_icon.png");

            Character.Role role = Character.Role.Soldier;
            Player.SoldierRole soldierRole = Player.SoldierRole.Knight;

            Player player = new Player(id, name, locationId, tilePosition, icon, role, soldierRole);

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
            #region A note on the path ...

            //  https://stackoverflow.com/a/5556068

            //  This seems to be the simplest solution to the issue I was 
            //  having, which was how to reference images in xaml.

            //  Before using this user's solution for image resource paths,
            //  I had tried using relative image paths as the sources,
            //  but these cannot work outside of the Visual Studio environment.

            //  I then tried converting from System.Drawing.Image to 
            //  the Image source control in WPF - that did not work either.

            #endregion

            string path = "pack://application:,,,/Assets/tile_samples/";

            List<Tiles> assets = new List<Tiles>()
            {
                new Tiles(0, "Default", path + "default.png", "Generic", true),
                new Tiles(1, "Deep Water", path + "deep_water.png", "River", false),
                new Tiles(2, "Dense Forest", path + "dense_forest.png", "General", false),
                new Tiles(3, "Entrance", path + "entrance.png", "Generic", true),
                new Tiles(4, "Exit", path + "exit.png", "Generic", true),
                new Tiles(5, "Fields", path + "fields.png", "General", true),
                new Tiles(6, "Forest Fringe", path + "fringe_forest.png", "General", true),
                new Tiles(7, "Rocks", path + "rocks.png", "General", false),
                new Tiles(8, "Sand", path + "sand.png", "River", true),
                new Tiles(9, "Shallow Water", path + "shallow_water.png", "General", true),
                new Tiles(10, "Thorns", path + "thorns.png", "Forest", false)
            };

            return assets;
        }

        public static Location InitDefaultLocation(Gamestate gamestate)
        {
            Location standard = new Location(0, "Default", "Initialized when the game loads", gamestate.RandObj);
            return standard;
        }
    }
}

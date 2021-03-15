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
            Moves standard = new Moves("move_default", Moves.DamageType.Melee, 1, 100, Moves.StatusType.None, 0, false, Moves.Ammunition.None);
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

        public static Armor[] InitArmor()
        {
            Armor[] armors = new Armor[]
            {
                new Armor(0, "Leather", "The lightest pair of armor. Offers little protection overall.", 10, 20, 0),
                new Armor(1, "Chainmail", "Protective against arrows and other ranged moves.", 20, 40, 0),
                new Armor(2, "Plated", "The best suit of armor. Slightly resistant to gunpowder moves.", 30, 50, 20)
            };

            return armors;
        }

        public static Moves[] InitMoves()
        {
            #region A note on moves ...

            // The moves listed here are temporary, and will not represent the end result.
            // Ideally, these moves would have custom names, unrelated to any weapons.
            // Instead, they could only be used if the player had the correct equipment to use it.
            //      i.e A player with a ranged weapon could only use ranged moves.
            // They will also need to be updated for better historical accuracy and terminology.

            #endregion

            Moves[] moves = new Moves[]
            {
                new Moves("Javelin", Moves.DamageType.Ranged, 120, 90, Moves.StatusType.Bleeding, 20, false, Moves.Ammunition.None),
                new Moves("Crossbow", Moves.DamageType.Ranged, 105, 95, Moves.StatusType.None, 0, true, Moves.Ammunition.Arrow),
                new Moves("Longbow", Moves.DamageType.Ranged, 80, 100, Moves.StatusType.Burning, 80, true, Moves.Ammunition.Arrow),
                new Moves("Lance", Moves.DamageType.Melee, 105, 95, Moves.StatusType.Bleeding, 30, false, Moves.Ammunition.None),
                new Moves("Pike", Moves.DamageType.Melee, 90, 100, Moves.StatusType.Bleeding, 10, false, Moves.Ammunition.None),
                new Moves("Halberd", Moves.DamageType.Melee, 100, 80, Moves.StatusType.Bleeding, 45, false, Moves.Ammunition.None),
                new Moves("Hand Cannon", Moves.DamageType.Gunpowder, 145, 75, Moves.StatusType.None, 0, true, Moves.Ammunition.Gunpowder),
                new Moves("Musket", Moves.DamageType.Gunpowder, 120, 85, Moves.StatusType.None, 0, true, Moves.Ammunition.Gunpowder)
            };

            return moves;
        }

        public static Traits[] InitTraits()
        {
            Traits[] traits = new Traits[]
            {
                new Traits("CHARISMATIC", "Merchants offer items at a lower price than usual. At the start of the game, receive a one-time boost to Coins."),
                new Traits("RESOURCEFUL", "75% chance to not consume ammunition on using a move. 10% more damage from Ranged and Gunpowder moves."),
                new Traits("HARDY", "Regenerate Health 100% faster."),
                new Traits("FORAGER", "50% chance to receive more items when killing enemies or picking up items off of the ground."),
                new Traits("ECCENTRIC", "Status effects are 40% more likely to land on an enemy. Status effects deal more damage to enemies."),
                new Traits("EXPERIENCED", "All moves are 15% more accurate. At the start of the game, receive a one-time boost to Experience."),
                new Traits("WEAKLING", "-10% damage when using Melee moves. +5% damage taken from enemy moves."),
                new Traits("RECKLESS", "50% chance to consume additional ammunition on using a move."),
                new Traits("ASOCIAL", "Merchants offer items at a higher price than usual.")
            };

            return traits;
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
                new Tiles(1, "Deep_Water", path + "deep_water.png", "River", false),
                new Tiles(2, "Dense_Forest", path + "forest.png", "General", false),
                new Tiles(3, "Entrance", path + "entrance.png", "Generic", true),
                new Tiles(4, "Exit", path + "exit.png", "Generic", true),
                new Tiles(5, "Fields", path + "fields.png", "General", true),
                new Tiles(6, "Forest_Fringe", path + "fringe_forest.png", "General", true),
                new Tiles(7, "Rocks", path + "rocks.png", "General", false),
                new Tiles(8, "Sand", path + "sand.png", "River", true),
                new Tiles(9, "Shallow_Water", path + "shallow_water.png", "General", true),
                new Tiles(10, "Thorns", path + "thorns.png", "Forest", false)
            };

            return assets;
        }

        public static Location InitDefaultLocation(Gamestate gamestate)
        {
            Location standard = new Dungeon(0, "Default", "Initialized when the game loads", gamestate.RandObj);
            return standard;
        }
    }
}

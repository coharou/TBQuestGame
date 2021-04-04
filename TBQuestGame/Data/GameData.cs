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

            return player;
        }

        public static List<PassiveNPC> InitPassiveTypes()
        {
            string path = "pack://application:,,,/Assets/character_icons/";
            Art icon = new Art(2, "Merchant", path + "peaceful.png");

            List<PassiveNPC> passiveTypes = new List<PassiveNPC>
            {
                new PassiveNPC(0, "Merchant", 0, 0, icon, Character.Role.Merchant)
            };

            return passiveTypes;
        }

        public static List<Enemy> InitEnemyTypes()
        {
            string path = "pack://application:,,,/Assets/character_icons/";
            Art icon = new Art(1, "Enemy", path + "enemy.png");

            Moves[] moves = InitMoves();
            Armor[] armor = InitArmor();

            List<Enemy> enemies = new List<Enemy>
            {
                new Enemy(0, "Pikeman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Pikeman, moves[4], armor[0]),
                new Enemy(1, "Crossbowman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Crossbowman, moves[1], armor[1]),
                new Enemy(2, "Longbowman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Archer, moves[2], armor[0]),
                new Enemy(3, "Knight", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Knight, moves[3], armor[1]),
                new Enemy(4, "Knight", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Knight, moves[5], armor[2]),
                new Enemy(5, "Arquebusier ", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Musketeer, moves[6], armor[1]),
                new Enemy(6, "Musketman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Musketeer, moves[7], armor[2])
            };

            return enemies;
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
                new Moves(0, "Javelin", "Ranged weapon.", Moves.DamageType.Ranged, 30, 85, Moves.StatusType.Bleeding, 20, false, Moves.Ammunition.None),
                new Moves(1, "Crossbow", "Ranged weapon.", Moves.DamageType.Ranged, 45, 95, Moves.StatusType.None, 0, true, Moves.Ammunition.Arrow),
                new Moves(2, "Longbow", "Ranged weapon.", Moves.DamageType.Ranged, 30, 100, Moves.StatusType.Burning, 80, true, Moves.Ammunition.Arrow),
                new Moves(3, "Lance", "Melee weapon.", Moves.DamageType.Melee, 35, 95, Moves.StatusType.None, 30, false, Moves.Ammunition.None),
                new Moves(4, "Pike", "Melee weapon.", Moves.DamageType.Melee, 25, 100, Moves.StatusType.Bleeding, 10, false, Moves.Ammunition.None),
                new Moves(5, "Halberd", "Melee weapon.", Moves.DamageType.Melee, 45, 80, Moves.StatusType.Bleeding, 45, false, Moves.Ammunition.None),
                new Moves(6, "Arquebus", "Gunpowder weapon.", Moves.DamageType.Gunpowder, 65, 75, Moves.StatusType.None, 0, true, Moves.Ammunition.Gunpowder),
                new Moves(7, "Musket", "Gunpowder weapon.", Moves.DamageType.Gunpowder, 75, 85, Moves.StatusType.None, 0, true, Moves.Ammunition.Gunpowder)
            };

            return moves;
        }

        public static Traits[] InitTraits()
        {
            Traits[] traits = new Traits[]
            {
                new Traits(0, "CHARISMATIC", "Merchants offer items at a lower price than usual. At the start of the game, receive a one-time bonus in Coins.", true),
                new Traits(1, "RESOURCEFUL", "75% chance to not consume ammunition on using a move. 10% more damage from Ranged and Gunpowder moves.", true),
                new Traits(2, "HARDY", "Regenerate Health 100% faster.", true),
                new Traits(3, "FORAGER", "50% chance to receive more items when killing enemies or picking up items off of the ground.", true),
                new Traits(4, "ECCENTRIC", "Status effects are 40% more likely to land on an enemy. At the start of the game, receive a one-time bonus in Coins.", true),
                new Traits(5, "EXPERIENCED", "All moves are 15% more accurate. At the start of the game, receive a one-time boost to Experience.", true),
                new Traits(6, "WEAKLING", "-10% damage when using Melee moves. +5% damage taken from enemy moves.", false),
                new Traits(7, "RECKLESS", "50% chance to consume additional ammunition on using a move.", false),
                new Traits(8, "ASOCIAL", "Merchants offer items at a higher price than usual.", false)
            };

            traits[0].MerchantInfluence = -25;
            traits[0].CoinBonus = 200;

            traits[1].AmmoUseMod = -75;
            traits[1].GunpowderStrMod = 10;
            traits[1].RangedStrMod = 10;

            traits[2].RegenMod = 2;

            traits[3].ForagingMod = 50;

            traits[4].StatusEffectMod = 40;
            traits[4].CoinBonus = 100;

            traits[5].AccuracyMod = 15;
            traits[5].ExperienceBonus = 10;

            traits[6].MeleeStrMod = -10;
            traits[6].DefenseMod = -5;

            traits[7].AmmoUseMod = -50;

            traits[8].MerchantInfluence = 25;

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

        public static List<Item> InitItems()
        {
            List<Item> items = new List<Item>()
            {
                new Item(0, "None", "DEFAULT", Item.Tag.None, 0),
                new Item(1, "Bandage", "Heals the player when used.", Item.Tag.Health, 200),
                new Item(2, "Evasion", "The player moves one dungeon layer ahead when used.", Item.Tag.Teleport, 500)
            };

            return items;
        }
    }
}

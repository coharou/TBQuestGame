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

            List<Enemy> enemies = new List<Enemy>
            {
                new Enemy(0, "Pikeman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Pikeman, ReturnMoveByID(4), ReturnArmorByID(0)),
                new Enemy(1, "Crossbowman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Crossbowman, ReturnMoveByID(1), ReturnArmorByID(1)),
                new Enemy(2, "Longbowman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Archer, ReturnMoveByID(2), ReturnArmorByID(4)),
                new Enemy(3, "Knight", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Knight, ReturnMoveByID(3), ReturnArmorByID(2)),
                new Enemy(4, "Knight", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Knight, ReturnMoveByID(5), ReturnArmorByID(2)),
                new Enemy(5, "Arquebusier ", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Musketeer, ReturnMoveByID(6), ReturnArmorByID(0)),
                new Enemy(6, "Musketman", 0, 0, icon, Character.Role.Soldier, Combatant.SoldierRole.Musketeer, ReturnMoveByID(7), ReturnArmorByID(1))
            };

            return enemies;
        }

        public static void InitArmor(out List<Armor> fullArmor, out List<Armor> customizeArmor)
        {
            fullArmor = new List<Armor>
            {
                new Armor(1, "Chainmail", "Protective against arrows and other ranged moves.", 20, 40, 0),
                new Armor(2, "Plated", "The best suit of armor. Slightly resistant to gunpowder moves.", 30, 50, 20)
            };

            customizeArmor = new List<Armor>
            {
                new Armor(4, "Armorless", "Only for the most experienced players.", 0, 0, 0),
                new Armor(0, "Leather", "The lightest pair of armor. Offers little protection overall.", 10, 20, 0)
            };

            foreach (var a in customizeArmor)
            {
                fullArmor.Add(a);
            }
        }

        private static Armor ReturnArmorByID(int id)
        {
            InitArmor(out List<Armor> fullArmor, out List<Armor> customizeArmor);
            Armor a = fullArmor.Find(x => x.ID == id);
            return a;
        }

        private static Moves ReturnMoveByID(int id)
        {
            InitMoves(out List<Moves> fullSet, out List<Moves> customizeSet);
            Moves m = fullSet.Find(x => x.ID == id);
            return m;
        }

        public static void InitMoves(out List<Moves> fullSet, out List<Moves> customizeSet)
        {
            #region A note on moves ...

            // The moves listed here are temporary, and will not represent the end result.
            // Ideally, these moves would have custom names, unrelated to any weapons.
            // Instead, they could only be used if the player had the correct equipment to use it.
            //      i.e A player with a ranged weapon could only use ranged moves.
            // They will also need to be updated for better historical accuracy and terminology.

            #endregion

            fullSet = new List<Moves>
            {
                new Moves(1, "Crossbow", "Ranged weapon.", Moves.DamageType.Ranged, 45, 95, Moves.StatusType.None, 0, true, Moves.Ammunition.Arrow),
                new Moves(5, "Halberd", "Melee weapon.", Moves.DamageType.Melee, 45, 80, Moves.StatusType.Bleeding, 45, false, Moves.Ammunition.None),
                new Moves(6, "Arquebus", "Gunpowder weapon.", Moves.DamageType.Gunpowder, 65, 75, Moves.StatusType.None, 0, true, Moves.Ammunition.Gunpowder),
                new Moves(7, "Musket", "Gunpowder weapon.", Moves.DamageType.Gunpowder, 75, 85, Moves.StatusType.None, 0, true, Moves.Ammunition.Gunpowder)
            };

            customizeSet = new List<Moves>
            {
                new Moves(0, "Javelin", "Ranged weapon.", Moves.DamageType.Ranged, 30, 85, Moves.StatusType.Bleeding, 20, false, Moves.Ammunition.None),
                new Moves(2, "Longbow", "Ranged weapon.", Moves.DamageType.Ranged, 30, 100, Moves.StatusType.Burning, 80, true, Moves.Ammunition.Arrow),
                new Moves(3, "Lance", "Melee weapon.", Moves.DamageType.Melee, 35, 95, Moves.StatusType.None, 30, false, Moves.Ammunition.None),
                new Moves(4, "Pike", "Melee weapon.", Moves.DamageType.Melee, 25, 100, Moves.StatusType.Bleeding, 10, false, Moves.Ammunition.None),
            };

            foreach (var m in customizeSet)
            {
                fullSet.Add(m);
            }
        }

        public static void InitTraits(out List<Traits> fullTraits, out List<Traits> customizeTraits)
        {
            fullTraits = new List<Traits>
            {
                new Traits(2, "HARDY", "Regenerate Health 100% faster.", true),
                new Traits(6, "WEAKLING", "-10% damage when using Melee moves. +5% damage taken from enemy moves.", false),
                new Traits(7, "RECKLESS", "50% chance to consume additional ammunition on using a move.", false),
                new Traits(8, "ASOCIAL", "Merchants offer items at a higher price than usual.", false)
            };

            fullTraits[0].RegenMod = 2;

            fullTraits[1].MeleeStrMod = -10;
            fullTraits[1].DefenseMod = -5;

            fullTraits[2].AmmoUseMod = -50;

            fullTraits[2].MerchantInfluence = 25;

            customizeTraits = new List<Traits>
            {
                new Traits(0, "CHARISMATIC", "Merchants offer items at a lower price than usual. At the start of the game, receive a one-time bonus in Coins.", true),
                new Traits(1, "RESOURCEFUL", "75% chance to not consume ammunition on using a move. 10% more damage from Ranged and Gunpowder moves.", true),
                new Traits(3, "FORAGER", "50% chance to receive more items when killing enemies or picking up items off of the ground.", true),
                new Traits(4, "ECCENTRIC", "Status effects are 40% more likely to land on an enemy. At the start of the game, receive a one-time bonus in Coins.", true),
                new Traits(5, "EXPERIENCED", "All moves are 15% more accurate. At the start of the game, receive a one-time boost to Experience.", true),
            };

            customizeTraits[0].MerchantInfluence = -25;
            customizeTraits[0].CoinBonus = 200;

            customizeTraits[1].AmmoUseMod = -75;
            customizeTraits[1].GunpowderStrMod = 10;
            customizeTraits[1].RangedStrMod = 10;

            customizeTraits[2].ForagingMod = 50;

            customizeTraits[3].StatusEffectMod = 40;
            customizeTraits[3].CoinBonus = 100;

            customizeTraits[4].AccuracyMod = 15;
            customizeTraits[4].ExperienceBonus = 10;

            foreach (var t in customizeTraits)
            {
                fullTraits.Add(t);
            }
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

            string path = "pack://application:,,,/Assets/";

            List<Tiles> assets = new List<Tiles>()
            {
                new Tiles(1, "Deep_Water", path + "tiles/deep_water.png", "River", false),
                new Tiles(2, "Dense_Forest", path + "tiles/forest.png", "General", false),
                new Tiles(3, "Entrance", path + "tiles/entrance.png", "Generic", true),
                new Tiles(4, "Exit", path + "tiles/exit.png", "Generic", true),
                new Tiles(5, "Fields", path + "tiles/fields.png", "General", true),
                new Tiles(6, "Forest_Fringe", path + "tiles/fringe_forest.png", "General", true),
                new Tiles(8, "Sand", path + "tiles/sand.png", "River", true),
                new Tiles(9, "Shallow_Water", path + "tiles/shallow_water.png", "General", true)
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

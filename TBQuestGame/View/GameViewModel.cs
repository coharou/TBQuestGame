using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.View
{
    public class GameViewModel : ObservableObject
    {
        #region PROPERTIES
        private Player _player;

        public Player Player
        {
            get { return _player; }
            set 
            { 
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        private Traits[] _traits;

        public Traits[] Traits
        {
            get { return _traits; }
            set { _traits = value; }
        }


        private Gamestate _gameState;

        public Gamestate Gamestate
        {
            get { return _gameState; }
            set
            {
                _gameState = value;
                OnPropertyChanged(nameof(Gamestate));
            }
        }

        private Tiles[,] _mapGrid;

        public Tiles[,] MapGrid
        {
            get { return _mapGrid; }
            set { _mapGrid = value; }
        }


        private TileConstants _c;

        public TileConstants C
        {
            get { return _c; }
            set { _c = value; }
        }

        private Location _location;

        public Location Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private Item[,] _itemGrid;

        public Item[,] ItemGrid
        {
            get { return _itemGrid; }
            set 
            { 
                _itemGrid = value;
            }
        }

        private List<Item> _items;

        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private List<Enemy> _enemyTypes;

        public List<Enemy> EnemyTypes
        {
            get { return _enemyTypes; }
            set 
            { 
                _enemyTypes = value;
                OnPropertyChanged(nameof(EnemyTypes));
            }
        }

        private List<PassiveNPC> _passiveTypes;

        public List<PassiveNPC> PassiveTypes
        {
            get { return _passiveTypes; }
            set { _passiveTypes = value; }
        }

        private List<PassiveNPC> _passiveNPCs;

        public List<PassiveNPC> PassiveNPCs
        {
            get { return _passiveNPCs; }
            set { _passiveNPCs = value; }
        }

        private List<Enemy> _enemyNPCs;

        public List<Enemy> EnemyNPCs
        {
            get { return _enemyNPCs; }
            set 
            { 
                _enemyNPCs = value;
                OnPropertyChanged(nameof(EnemyNPCs));
            }
        }

        private List<int> _enemyPosX;

        public List<int> EnemyPosX
        {
            get { return _enemyPosX; }
            set { _enemyPosX = value; }
        }

        private List<int> _enemyPosY;

        public List<int> EnemyPosY
        {
            get { return _enemyPosY; }
            set { _enemyPosY = value; }
        }

        private Utilities.Combat _combat;

        public Utilities.Combat Combat
        {
            get { return _combat; }
            set { _combat = value; }
        }

        private Tooltip _tips;

        public Tooltip Tips
        {
            get { return _tips; }
            set 
            {
                _tips = value;
                OnPropertyChanged(nameof(Tips));
            }
        }


        #endregion

        #region CONSTRUCTORS
        public GameViewModel(Player player, Gamestate gamestate, Traits[] traits, List<Item> items, List<Enemy> enemyTypes, List<PassiveNPC> passiveTypes)
        {
            _player = player;
            _traits = traits;
            TraitStartUp(gamestate);
            _player = Player;

            C = new TileConstants();

            _gameState = gamestate;
            _items = items;
            _enemyTypes = enemyTypes;
            _passiveTypes = passiveTypes;
            _location = SetupStartLocation();
            MapGrid = _location.TileGrid;

            EnemyPosX = new List<int>();
            EnemyPosY = new List<int>();

            Player.SelectedMove = Player.Moves[0];

            Combat = new Utilities.Combat();

            Tips = new Tooltip("", "", "", "", "");

            GenerateItemList();
            CreateListsOfNPCs();
            MatchPlayerPositionToEntrance();
            UpdateDungeonValues();
        }
        #endregion

        #region Player Trait METHODS
        /// <summary>
        /// Inits the player's random traits. Inits application of the player's traits to their stats.
        /// </summary>
        /// <param name="gamestate"></param>
        public void TraitStartUp(Gamestate gamestate)
        {
            Player.TraitRandomPos = GetRandomTraits(true, gamestate);
            Player.TraitRandomNeg = GetRandomTraits(false, gamestate);
            ApplyTraitToPlayerStats(Player.TraitChosenFirst);
            ApplyTraitToPlayerStats(Player.TraitChosenSecond);
            ApplyTraitToPlayerStats(Player.TraitRandomPos);
            ApplyTraitToPlayerStats(Player.TraitRandomNeg);
        }

        public void ApplyTraitToPlayerStats(Traits trait)
        {
            if (trait.AccuracyMod.GetType() != null)
            {
                Player.AccuracyModGunpowder += trait.AccuracyMod;
                Player.AccuracyModMelee += trait.AccuracyMod;
                Player.AccuracyModRanged += trait.AccuracyMod;
            }
            if (trait.AmmoUseMod.GetType() != null)
            {
                Player.ResourcefulnessMod += trait.AmmoUseMod;
            }
            if (trait.DefenseMod.GetType() != null)
            {
                Player.DefenseModMelee += trait.DefenseMod;
                Player.DefenseModGunpowder += trait.DefenseMod;
                Player.DefenseModRanged += trait.DefenseMod;
            }
            if (trait.ExperienceBonus.GetType() != null)
            {
                Player.Experience += trait.ExperienceBonus;
            }
            if (trait.ForagingMod.GetType() != null)
            {
                Player.Foraging += trait.ForagingMod;
            }
            if (trait.CoinBonus.GetType() != null)
            {
                Player.Coins += trait.CoinBonus;
            }
            if (trait.GunpowderStrMod.GetType() != null)
            {
                Player.StrengthModGunpowder += trait.GunpowderStrMod;
            }
            if (trait.HealthMod.GetType() != null)
            {
                Player.HealthMax += trait.HealthMod;
                Player.HealthCurrent += trait.HealthMod;
            }
            if (trait.MeleeStrMod.GetType() != null)
            {
                Player.StrengthModMelee += trait.MeleeStrMod;
            }
            if (trait.MerchantInfluence.GetType() != null)
            {
                Player.Charisma += trait.MerchantInfluence;
            }
            if (trait.RangedStrMod.GetType() != null)
            {
                Player.StrengthModRanged += trait.RangedStrMod;
            }
            if (trait.RegenMod.GetType() != null)
            {
                Player.HealthRegenerationRate += trait.RegenMod;
            }
            if (trait.StatusEffectMod.GetType() != null)
            {
                Player.StatusInflictMod += trait.StatusEffectMod;
            }
        }

        public Traits GetRandomTraits(bool isPositive, Gamestate gamestate)
        {
            Traits trait = Traits[0];
            List<Traits> traitCollection = new List<Traits>();
            bool traitFound = false;

            foreach (var tr in Traits)
            {
                if (isPositive == tr.IsPositive)
                {
                    if (Player.TraitChosenFirst.ID != tr.ID || Player.TraitChosenSecond.ID != tr.ID)
                    {
                        traitCollection.Add(tr);
                    }
                }
            }

            do
            {
                foreach (var tr in traitCollection)
                {
                    int r = gamestate.RandObj.Next(0, 10);
                    if (r == 1)
                    {
                        trait = tr;
                        traitFound = true;
                        break;
                    }
                }
            } while (traitFound == false);

            return trait;
        }

        #endregion

        #region Gamestate CHANGING METHODS
        public void ChangeGamestates(string type)
        {
            switch (type)
            {
                case "Options":
                    Gamestate.PausedByOptions = true;
                    break;

                case "Traits":
                    Gamestate.PausedByTraits = true;
                    break;

                case "Inventory":
                    Gamestate.PausedByInventory = true;
                    break;

                case "ReturnGame":
                    Gamestate.PausedByOptions = false;
                    Gamestate.PausedByTraits = false;
                    Gamestate.PausedByInventory = false;
                    break;

                default:
                    break;
            }
        }

        public void TurnTransition()
        {
            Gamestate.CanPlayerAct = false;
            Gamestate.TurnCount += 1;
            Player.MovementCurrent = Player.MovementMax;
            Gamestate.CanPlayerAct = true;
        }
        #endregion

        #region NPC SPAWNING
        public void CreateListsOfNPCs()
        {
            // Spawn passive NPCs first, then enemies
            // bool isMerchantSpawning = ShouldMerchantSpawn();
            /*
            if (isMerchantSpawning == true)
            {
                PassiveNPC merchant = FindPassiveByID(0);
                PassiveNPCs.Add(merchant);
            }
            */

            int enemyCount = NumberOfEnemiesToSpawn();

            List<Enemy> tempEnemies = GetEnemiesForList(enemyCount);
            EnemyNPCs = tempEnemies;

            GiveEnemiesPositions();
        }

        public void GiveEnemiesPositions()
        {
            for (int i = 0; i < EnemyNPCs.Count; i++)
            {
                bool passable = false;
                bool occupied = false;

                do
                {
                    int sampleX = Gamestate.RandObj.Next(0, C.TilesPerRow);
                    int sampleY = Gamestate.RandObj.Next(0, C.TilesPerRow);

                    if (MapGrid[sampleX, sampleY].Passable == true)
                    {
                        if (MapGrid[sampleX, sampleY].ID != 3 && MapGrid[sampleX, sampleY].ID != 4)
                        {
                            if (i == 0)
                            {
                                EnemyPosX.Add(sampleX);
                                EnemyPosY.Add(sampleY);
                                passable = true;
                            }
                            else
                            {
                                for (int e = 0; e < EnemyPosX.Count; e++)
                                {
                                    if (EnemyPosX[e] == sampleX && EnemyPosY[e] == sampleY)
                                    {
                                        occupied = true;
                                    }
                                }

                                if (occupied == false)
                                {
                                    EnemyPosX.Add(sampleX);
                                    EnemyPosY.Add(sampleY);
                                    passable = true;
                                }

                                occupied = false;
                            }

                        }
                    }
                } while (passable == false);
            }
        }

        private List<Enemy> GetEnemiesForList(int enemyCount)
        {
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy sample = FindEnemyByID(3);

                int ran = Gamestate.RandObj.Next(0, 100);

                if (Gamestate.LocationCount < 6)
                {
                    if (ran >= 0 && ran < 50)
                    {
                        // 50%, Pikeman
                        sample = FindEnemyByID(0);
                    }
                    else if (ran >= 50 && ran < 55)
                    {
                        // 5%, Crossbowman
                        sample = FindEnemyByID(1);
                    }
                    else if (ran >= 55 && ran < 90)
                    {
                        // 35%, Longbowman
                        sample = FindEnemyByID(2);
                    }
                    else
                    {
                        // 10%, Lance-equipped Knight
                        sample = FindEnemyByID(3);
                    }
                }
                else
                {
                    if (ran >= 0 && ran < 15)
                    {
                        // 15%, Lance-equipped Knight
                        sample = FindEnemyByID(3);
                    }
                    else if (ran >= 15 && ran < 35)
                    {
                        // 20%, Halberd-equipped Knight
                        sample = FindEnemyByID(4);
                    }
                    else if (ran >= 35 && ran < 60)
                    {
                        // 25%, Arquebusier
                        sample = FindEnemyByID(5);
                    }
                    else if (ran >= 60 && ran < 75)
                    {
                        // 15%, Musketman
                        sample = FindEnemyByID(6);
                    }
                    else
                    {
                        // 25%, Crossbowman
                        sample = FindEnemyByID(1);
                    }
                }

                Enemy e = new Enemy(sample.ID, sample.Name, sample.LocationID, sample.TilePosition, sample.Icon, sample.RoleDescriptor, sample.ExtendedRole, sample.SelectedMove, sample.ArmorType);

                enemies.Add(e);
            }

            return enemies;
        }

        private int NumberOfEnemiesToSpawn()
        {
            // Six (6) enemies are guaranteed to spawn.
            int count = 6;

            // For each dungeon layer the player has moved through, test adding another enemy.
            // There is a 50% chance to spawn an additional enemy on the first layer.

            // If the user is at the third layer, there is are 3 opportunities to add more enemies, at 50% chances.
            // For each unsuccessful attempt, there is a 20% extra chance to spawn an enemy.
            // Once a successful attempt occurs after a miss, the chance is set back to 50%.
            // i.e  ATTEMPT 1 @ 50% = miss; so ATTEMPT 2 @ 70%.
            //      IF ATTEMPT 2 = success, ATTEMPT 3 will be @ 50% chance.

            // Only 1, 2, 3

            int chance = 50;
            for (int i = 0; i < Gamestate.LayerCount; i++)
            {
                int ran = Gamestate.RandObj.Next(0, 100);
                if (chance > ran)
                {
                    count++;

                    if (chance > 50)
                    {
                        chance = 50;
                    }
                }
                else
                {
                    chance += 20;
                }
            }

            return count;
        }

        private Enemy FindEnemyByID(int id)
        {
            List<Enemy> eTypes = EnemyTypes;
            Enemy e = eTypes.Find(x => x.ID == id);
            return e;
        }

        private PassiveNPC FindPassiveByID(int id)
        {
            List<PassiveNPC> pTypes = PassiveTypes;
            PassiveNPC p = pTypes.Find(x => x.ID == id);
            return p;
        }

        private bool ShouldMerchantSpawn()
        {
            bool isMerchantSpawning = false;

            int chance = 0;

            switch (Gamestate.TimeSinceMerchantSpawn)
            {
                case 0:
                    chance = 50;
                    break;
                case 1:
                    chance = 60;
                    break;
                case 2:
                    chance = 70;
                    break;
                case 3:
                    chance = 80;
                    break;
                case 4:
                    chance = 90;
                    break;
                default:
                    chance = 100;
                    break;
            }

            int ran = Gamestate.RandObj.Next(0, 100);

            if (chance >= ran)
            {
                isMerchantSpawning = true;
            }

            return isMerchantSpawning;
        }
        #endregion

        #region NPC LOCATIONS
        public bool IsOccupiedByEnemy(int x, int y)
        {
            bool isOccupied = false;

            for (int i = 0; i < EnemyNPCs.Count; i++)
            {
                if (EnemyPosX[i] == x && EnemyPosY[i] == y)
                {
                    isOccupied = true;
                }
            }

            return isOccupied;
        }

        public bool TestPlayerAttack(string tag)
        {
            bool didEnemyDie = false;

            (int, int) coords = CharacterCoordinates(tag);
            int x = coords.Item1;
            int y = coords.Item2;

            bool isPlayerAdj = IsPlayerAdjacent(x, y);

            if (isPlayerAdj == true)
            {
                Enemy enemy = FindEnemyFromList(x, y);

                enemy = (Enemy)Combat.ProcessAttack(Player, enemy, Gamestate.RandObj);

                Console.WriteLine("hello!");

                didEnemyDie = TestIfEnemyAlive(enemy);
                if (didEnemyDie == true)
                {
                    RemoveEnemyFromList(enemy);
                }
            }

            return didEnemyDie;
        }

        public bool TestIfEnemyAlive(Enemy e)
        {
            bool isDead = false;

            int hp = e.HealthCurrent;
            if (hp <= 0)
            {
                isDead = true;
            }

            return isDead;
        }

        public void RemoveEnemyFromList(Enemy e)
        {
            EnemyNPCs.Remove(e);
        }

        /// <summary>
        /// Finds an Enemy NPC by their tile position from among all the enemy NPCs on the field.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Enemy FindEnemyFromList(int x, int y)
        {
            Enemy enemy = FindEnemyByID(0);

            for (int i = 0; i < EnemyNPCs.Count; i++)
            {
                if (EnemyPosX[i] == x && EnemyPosY[i] == y)
                {
                    enemy = EnemyNPCs[i];
                }
            }

            return enemy;
        }

        private bool IsPlayerAdjacent(int enemyX, int enemyY)
        {
            bool isAdj = false;    

            if (enemyX - 1 == Player.TilePositionColumn && enemyY == Player.TilePositionRow)
            {
                isAdj = true;
            }
            if (enemyX + 1 == Player.TilePositionColumn && enemyY == Player.TilePositionRow)
            {
                isAdj = true;
            }
            if (enemyY - 1 == Player.TilePositionRow && enemyX == Player.TilePositionColumn)
            {
                isAdj = true;
            }
            if (enemyY + 1 == Player.TilePositionRow && enemyX == Player.TilePositionColumn)
            {
                isAdj = true;
            }

            return isAdj;
        }

        public string GetEnemyIconPath()
        {
            // This will need to be updated when many enemy sprites are added
            // It will need parameters for enemy position

            string path = EnemyNPCs[0].Icon.Path;
            return path;
        }
        #endregion

        #region ITEM GENERATION
        public void GenerateItemList()
        {
            bool isGenerating = ShouldItemsBeGenerated();
            if (isGenerating == true)
            {
                int itemCount = QuantityOfItemsToGenerate();
                ItemGrid = BuildItemList(itemCount);
            }
            else
            {
                ItemGrid = BuildItemList(0);
            }
        }

        public Item[,] BuildItemList(int amount)
        {
            Item[,] items = new Item[C.TilesPerRow, C.TilesPerRow];

            Item def = MatchItemType(Item.Tag.None);

            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    items[x, y] = def;
                }
            }

            for (int i = 0; i < amount; i++)
            {
                bool passable = false;
                do
                {
                    int sampleX = Gamestate.RandObj.Next(0, C.TilesPerRow);
                    int sampleY = Gamestate.RandObj.Next(0, C.TilesPerRow);
                    if (MapGrid[sampleX, sampleY].Passable == true)
                    {
                        items[sampleX, sampleY] = GetRandomItem();

                        passable = true;
                    }
                } while (passable == false);
            }

            return items;
        }

        public Item GetRandomItem()
        {
            Item item = MatchItemType(Item.Tag.None);

            int sample = Gamestate.RandObj.Next(0, 100);

            if (sample < 80)
            {
                item = MatchItemType(Item.Tag.Health);
            }
            else
            {
                item = MatchItemType(Item.Tag.Teleport);
            }

            return item;
        }

        public Item MatchItemType(Item.Tag tag)
        {
            List<Item> iList = Items;
            Item i = iList.Find(x => x.ItemTag == tag);
            return i;
        }

        public Item MatchItemName(string name)
        {
            List<Item> iList = Items;
            Item i = iList.Find(x => x.Name == name);
            return i;
        }

        public int QuantityOfItemsToGenerate()
        {
            // 100% chance to have an item on the map
            int quantity = 1;

            int sample = Gamestate.RandObj.Next(0, 100);

            if (sample < 50)
            {
                // 50% chance to have two items on the map
                quantity += 1;
            }

            return quantity;
        }

        public bool ShouldItemsBeGenerated()
        {
            bool generateItems = false;

            int sample = Gamestate.RandObj.Next(0, 100);

            if (sample < 70)
            {
                // 70% chance that items will be generated on the layer
                generateItems = true;
            }

            return generateItems;
        }

        public bool DoesTileHaveItems(int x, int y)
        {
            bool anyItems = false;

            Item def = MatchItemType(Item.Tag.None);

            Item item = ItemGrid[x, y];

            if (item.ItemTag != def.ItemTag)
            {
                anyItems = true;
            }

            return anyItems;
        }

        public void MoveItemToInventory(int x, int y)
        {
            Item def = MatchItemType(Item.Tag.None);
            Item item = ItemGrid[x, y];

            ItemGrid[x, y] = def;

            Player.Inventory.Add(item);
        }

        public void RemoveItemFromInventory(string name)
        {
            Item item = MatchItemName(name);
            Player.Inventory.Remove(item);
        }

        public bool UseItemFromInventory(string name)
        {
            Item item = MatchItemName(name);

            bool teleport = false;

            switch (item.ItemTag)
            {
                case Item.Tag.Health:
                    HealPlayer();
                    break;
                case Item.Tag.Teleport:
                    teleport = true;
                    break;
                case Item.Tag.None:
                    break;
                default:
                    break;
            }

            Player.Inventory.Remove(item);

            return teleport;
        }

        public void HealPlayer()
        {
            Player.HealthCurrent += 50;
            if (Player.HealthCurrent > 100)
            {
                Player.HealthCurrent = 100;
            }
        }

        public List<String> FilterInventoryByList(string name)
        {
            List<Item.Tag> tags = new List<Item.Tag>();

            switch (name)
            {
                case "Status":
                    tags.Add(Item.Tag.Health);
                    break;
                case "Location":
                    tags.Add(Item.Tag.Teleport);
                    break;
                default:
                    break;
            }

            List<String> names = new List<String>();

            foreach (var i in tags)
            {
                string v = i.ToString();
                names.Add(v);
            }

            return names;
        }

        public void ResortInventory()
        {
            List<String> names = new List<String>();

            foreach (var i in Player.Inventory)
            {
                names.Add(i.Name);
            }

            names.Sort();

            Player.Inventory.Clear();

            foreach (var i in names)
            {
                Item item = MatchItemName(i);
                Player.Inventory.Add(item);
            }
        }
        #endregion

        #region DUNGEON TRANSITIONING PROCESS

        public Location SetupStartLocation()
        {
            Location location = new Dungeon(Gamestate.LocID, "The Beginning", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
            return location;
        }

        public void DungeonTransition()
        {
            // Called when the player has completed a layer of a dungeon
            // Examples: reaching the dungeon exit; using an item

            UpdateDungeonValues();
            InitiateNewLocation();

            GenerateItemList();

            EnemyNPCs.Clear();
            EnemyPosX.Clear();
            EnemyPosY.Clear();

            CreateListsOfNPCs();

            Gamestate.Location = Location.Name;
        }

        public void UpdateDungeonValues()
        {
            Gamestate.LayerCount += 1;
            Gamestate.LocID += 1;

            if (Gamestate.LayerCount > 3)
            {
                Gamestate.LocationCount += 1;
                Gamestate.LayerCount = 1;
            }

            PrizesForCompletingDungeonLayer();
        }

        public void InitiateNewLocation()
        {
            if (Gamestate.LocationCount <= 2)
            {
                Location.Name = "The Beginning";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 3 || Gamestate.LocationCount == 4)
            {
                Location.Name = "The River";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.River);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 5)
            {
                Location.Name = "The Village";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 6)
            {
                Location.Name = "The Return";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 7 || Gamestate.LocationCount == 8)
            {
                Location.Name = "The River";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.River);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 9)
            {
                Location.Name = "The Village";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 10)
            {
                Location.Name = "A Flashback";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }  
        }

        #endregion

        #region Enemy TURN PROCESS
        public void DoEnemyTurn()
        {
            // When enemies are implemented, CanPlayerAct will likely be
            // used to stop players from pressing buttons while the enemy
            // makes their moves. The Gamestate property will be bound to
            // property IsEnabled on the tile buttons of the grid_Map. If false,
            // the buttons will not work. If true, they will, and the player
            // can make their moves. If binding CanPlayerAct to the tile
            // buttons does not work, it will be used in an if statement
            // for the DoPlayerMovement method. In that case, the button
            // presses will not be processed if the property is false.
        }
        #endregion

        #region Tile AND Movement METHODS
        public bool DoPlayerMovement(string tag)
        {
            bool canMove = false;

            // If the player has movement points at their disposal ...
            if (Player.MovementCurrent > 0)
            {
                (int, int) coord = CharacterCoordinates(tag);
                int column = coord.Item1;
                int row = coord.Item2;

                bool tileInRange = IsTileInPlayerMoveRange(column, row);

                // If the player has sufficient movement points to move to the clicked tile ...
                if (tileInRange == true)
                {
                    bool passable = AreTilesPassable(column, row);

                    // If the tile can be passed into, and the tiles on the way there are passable ...
                    if (passable == true)
                    {
                        UpdatePlayerMovement(column, row);
                        UpdatePlayerPositions(column, row);
                        canMove = true;
                    }
                }
            }

            return canMove;
        }

        public (int, int) FindTileByName(string sampler)
        {
            for (int x = 0; x < C.TilesPerRow; x++)
            {
                for (int y = 0; y < C.TilesPerRow; y++)
                {
                    string name = MapGrid[x, y].Name;
                    if (name == sampler)
                    {
                        return (x, y);
                    }
                }
            }

            return (0, 0);
        }

        public void MatchPlayerPositionToEntrance()
        {
            (int, int) position = FindTileByName("Entrance");
            UpdatePlayerPositions(position.Item1, position.Item2);
        }

        public bool IsTileExit(int column, int row)
        {
            bool isTileExit = false;
            string tileName = MapGrid[column, row].Name;
            if (tileName == "Exit")
            {
                isTileExit = true;
            }
            else
            {
                DoEnemyTurn();
            }

            TurnTransition();

            return isTileExit;
        }

        public void UpdatePlayerMovement(int tCol, int tRow)
        {
            int pRow = Player.TilePositionRow;
            int pCol = Player.TilePositionColumn;
            int distance = 0;

            if (pRow == tRow)
            {
                distance = Math.Abs(tCol - pCol);
            }

            if(pCol == tCol)
            {
                distance = Math.Abs(tRow - pRow);
            }

            Player.MovementCurrent -= distance;
        }

        public void UpdatePlayerPositions(int column, int row)
        {
            Player.TilePositionColumn = column;
            Player.TilePositionRow = row;
            Player.TilePosition = CalculateTilePosition(column, row);
        }

        public (int, int) CharacterCoordinates(string tag)
        {
            string[] parts = tag.Split('_');

            int column = UpdateToCoordinates(parts[0]);
            int row = UpdateToCoordinates(parts[1]);

            return (column, row);
        }

        private bool AreTilesPassable(int tileCol, int tileRow)
        {
            bool passable = true;

            int pCol = Player.TilePositionColumn;
            int pRow = Player.TilePositionRow;

            int colDist = tileCol - pCol;
            int colDistAbs = Math.Abs(colDist);

            int rowDist = tileRow - pRow;
            int rowDistAbs = Math.Abs(rowDist);

            // If the player is moving vertically or horizontally ...
            // This code should be revised in the future.
            if (colDistAbs == 0 && rowDistAbs != 0)
            {
                if (rowDist < 0)
                {
                    for (int i = -1; i >= rowDist; i--)
                    {
                        int pos = pRow + i;
                        passable = MapGrid[pCol, pos].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= rowDist; i++)
                    {
                        int pos = pRow + i;
                        passable = MapGrid[pCol, pos].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
            }
            else if (colDistAbs != 0 && rowDistAbs == 0)
            {
                if (colDist < 0)
                {
                    for (int i = -1; i >= colDist; i--)
                    {
                        int pos = pCol + i;
                        passable = MapGrid[pos, pRow].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= colDist; i++)
                    {
                        int pos = pCol + i;
                        passable = MapGrid[pos, pRow].Passable;
                        if (passable == false)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                // Diagonal distance check for movement.
                // Temporarily, this will be set to false.

                // I am afraid that diagonal movement may hasten the pace of the game too much.
                // Players would be able to dodge enemies more easily than they should,
                // and also arrive to the exit very quickly.

                // If the map grid were larger, this wouldn't be as much of an issue.
                // But with it only being 8 by 8 tiles, movement needs to be heavily restricted.

                passable = false;
            }

            return passable;
        }

        private bool IsTileInPlayerMoveRange(int tileCol, int tileRow)
        {
            bool enoughMoves = false;

            int pCol = Player.TilePositionColumn;
            int pRow = Player.TilePositionRow;
            int pMoves = Player.MovementCurrent;

            if ((pCol < tileCol) && (pCol + pMoves >= tileCol))
            {
                enoughMoves = true;
            }
            if ((pCol > tileCol) && (pCol - pMoves <= tileCol))
            {
                enoughMoves = true;
            }
            if ((pRow < tileRow) && (pRow + pMoves >= tileRow))
            {
                enoughMoves = true;
            }
            if ((pRow > tileRow) && (pRow - pMoves <= tileRow))
            {
                enoughMoves = true;
            }

            return enoughMoves;
        }

        private static int UpdateToCoordinates(string part)
        {
            part = part.Remove(0, 1);
            int coord = int.Parse(part);
            return coord;
        }

        public int CalculateTilePosition(int column, int row)
        {
            int estimatedTotal = C.TilesPerRow * row;
            int tilesToSubtract = C.TilesPerRow - column;

            int position = 0;
            if (estimatedTotal != 0)
            {
                position = estimatedTotal - tilesToSubtract;
            }

            return position;
        }
        #endregion

        #region Experience METHODS
        public void PrizesForCompletingDungeonLayer()
        {
            Player.Coins += 100;
        }
        #endregion

        #region Grid METHODS
        // Applies adequate View -> Viewmodel -> Model translations for building the grid.
        // This ensures that the view does not have to directly reference the model.
        //  ex. Creating a TileConstants object in the view is not necessary.
        //      If it were included, it would distort the separation between the three elements.

        public int GetTotalGridTiles()
        {
            return C.TotalTileCount;
        }

        public int GetTotalTilesPerRow()
        {
            return C.TilesPerRow;
        }

        public int GetTileDimensions()
        {
            return C.TileDimensions;
        }

        public string GetTileImagePath(int x, int y)
        {
            string path = MapGrid[x, y].Path;
            return path;
        }

        public string GetTileName(int x, int y)
        {
            string name = MapGrid[x, y].Name;
            return name;
        }

        public string GetCharacterIconPath()
        {
            string path = Player.Icon.Path;
            return path;    
        }

        public (int, int) GetCharacterIconPosition()
        {
            int column = Player.TilePositionColumn;
            int row = Player.TilePositionRow;

            return (column, row);
        }

        public string GetItemIconPath()
        {
            string path = "pack://application:,,,/Assets/item.png";
            return path;
        }

        public bool IsItemReal(int x, int y)
        {
            bool isItemReal = false;

            Item.Tag tag = ItemGrid[x, y].ItemTag;
            Item.Tag def = Item.Tag.None;

            if (tag != def)
            {
                isItemReal = true;
            }

            return isItemReal;
        }

        public (List<String>, List<String>, List<String>) GetPlayerInventory()
        {
            List<Item> items = Player.Inventory;

            List<String> name = new List<string>();
            List<String> description = new List<string>();
            List<String> tag = new List<string>();

            foreach (var i in items)
            {
                name.Add(i.Name);
                description.Add(i.Description);
                tag.Add(i.ItemTag.ToString());
            }

            return (name, description, tag);
        }

        public void HoverOverInfo(string tag)
        {
            (int, int) coords = CharacterCoordinates(tag);
            int x = coords.Item1;
            int y = coords.Item2;

            Enemy enemy = FindEnemyFromList(x, y);

            Tips.Name = enemy.Name;
            Tips.CurrentHP = $"{enemy.HealthCurrent}";
            Tips.MaxHP = $"{enemy.HealthMax}";
            Tips.Armor = enemy.ArmorType.Name;
            Tips.Move = enemy.SelectedMove.Name;
        }

        public void RemoveHoverInfo()
        {
            Tips.Name = "";
            Tips.CurrentHP = "";
            Tips.MaxHP = "";
            Tips.Armor = "";
            Tips.Move = "";
        }
        #endregion
    }
}

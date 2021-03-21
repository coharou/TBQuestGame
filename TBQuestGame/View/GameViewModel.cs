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

        #endregion

        #region CONSTRUCTORS
        public GameViewModel(Player player, Gamestate gamestate, Traits[] traits, List<Item> items)
        {
            _player = player;
            _traits = traits;
            TraitStartUp(gamestate);
            _player = Player;

            C = new TileConstants();

            _gameState = gamestate;
            _items = items;
            _location = SetupStartLocation();
            MapGrid = _location.TileGrid;

            GenerateItemList();
            MatchPlayerPositionToEntrance();
            UpdateDungeonValues();
        }
        #endregion

        #region Player Trait METHODS
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
        #endregion

        #region DUNGEON TRANSITIONING PROCESS

        public Location SetupStartLocation()
        {
            Location location = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
            return location;
        }

        public void DungeonTransition()
        {
            // Called when the player has completed a layer of a dungeon
            // Examples: reaching the dungeon exit; using an item

            UpdateDungeonValues();
            InitiateNewLocation();

            GenerateItemList();

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
                Location.Name = "Starting Area Forest";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 3 || Gamestate.LocationCount == 4)
            {
                Location.Name = "River Seine, Part 1";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.River);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 5)
            {
                Location.Name = "Village, Part 1";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 6)
            {
                Location.Name = "Return to the Forest";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 7 || Gamestate.LocationCount == 8)
            {
                Location.Name = "River Seine, Part 2";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.River);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 9)
            {
                Location.Name = "Village, Part 2";
                Location standard = new Dungeon(Gamestate.LocID, "Default", "Initialized when the game loads", Gamestate.RandObj, Dungeon.Biome.Forest);
                MapGrid = standard.TileGrid;
            }
            if (Gamestate.LocationCount == 10)
            {
                Location.Name = "Flashback";
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
        #endregion
    }
}

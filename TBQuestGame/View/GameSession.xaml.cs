using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TBQuestGame.View
{
    /// <summary>
    /// Interaction logic for GameSession.xaml
    /// </summary>
    public partial class GameSession : Window
    {
        #region PROPS
        GameViewModel _gameViewModel;
        #endregion

        #region CONSTRUCTORS
        public GameSession(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            InitializeComponent();
        }
        #endregion

        #region GameSession LOAD METHOD
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetGridDefinitions(grid_Map);
            CreateMapGrid();
            SetGridDefinitions(grid_Action);
            AddItemsToGrid();
            AddEnemiesToGrid();
            AddPassivesToGrid();
            CreateCharacterIcon();
        }
        #endregion

        #region INTERFACE BUTTONS
        private void btn_Inventory_Clicked(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("Inventory");
            DisplayItemLists(GameInfo.Character.Role.Soldier, inv_Obj);
        }

        private void Btn_Inventory_Closed(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("ReturnGame");
            ClearItemLists(inv_Obj);
        }

        private void Btn_Inventory_TrashItem(object sender, RoutedEventArgs e)
        {
            RemoveItemFromInventory();
            ClearItemLists(inv_Obj);
            DisplayItemLists(GameInfo.Character.Role.Soldier, inv_Obj);
        }

        private void Btn_Inventory_UseItem(object sender, RoutedEventArgs e)
        {
            UseItemFromInventory();
            ClearItemLists(inv_Obj);
            DisplayItemLists(GameInfo.Character.Role.Soldier, inv_Obj);
        }

        private void Btn_Inventory_Sort(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ResortInventory();
            ClearItemLists(inv_Obj);
            DisplayItemLists(GameInfo.Character.Role.Soldier, inv_Obj);
        }

        private void btn_Traits_Clicked(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("Traits");
        }

        private void btn_ReturnGame_Clicked(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("ReturnGame");
        }

        private void btn_Help_Clicked(object sender, RoutedEventArgs e)
        {
            // Update when a manual window is required
        }

        private void btn_Options_Clicked(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("Options");
        }

        private void btn_Exit_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_Inventory_Refresh_Clicked(object sender, RoutedEventArgs e)
        {
            ClearItemLists(inv_Obj);
            DisplayItemLists(GameInfo.Character.Role.Soldier, inv_Obj);
        }

        private void Btn_Inventory_Filter_Clicked(object sender, RoutedEventArgs e)
        {
            Button filter = (Button)e.Source;
            string name = filter.Name;
            List<String> names = _gameViewModel.FilterInventoryByList(name);

            List<TextBlock> textBlocks = new List<TextBlock>();
            List<RadioButton> radioButtons = new List<RadioButton>();

            UIElementCollection collection = inv_Obj.Children;


            foreach (var x in collection)
            {
                foreach (var y in names)
                {
                    if (x is TextBlock)
                    {
                        TextBlock xBlock = (TextBlock)x;
                        if (xBlock.Tag.ToString() != y)
                        {
                            textBlocks.Add(xBlock);
                        }
                    }
                    if (x is RadioButton)
                    {
                        RadioButton xBtn = (RadioButton)x;
                        if (xBtn.Tag.ToString() != y)
                        {
                            radioButtons.Add(xBtn);
                        }
                    }
                }
            }

            for (int i = 0; i < textBlocks.Count; i++)
            {
                collection.Remove(textBlocks[i]);
                collection.Remove(radioButtons[i]);
            }

        }
        #endregion

        #region INVENTORY MANAGEMENT

        private void UseItemFromInventory()
        {
            if (IsRadioButtonChecked(inv_Obj, out string name))
            {
                bool teleport = _gameViewModel.UseItemFromInventory(name);
                if (teleport == true)
                {
                    TransitionDungeonLayers();
                }
            }
        }

        private void RemoveItemFromInventory()
        {
            if (IsRadioButtonChecked(inv_Obj, out string name))
            {
                _gameViewModel.RemoveItemFromInventory(name);
            }
        }
        #endregion

        #region ITEM GENERATION
        private void AddItemsToGrid()
        {
            int tpr = _gameViewModel.GetTotalTilesPerRow();

            for (int x = 0; x < tpr; x++)
            {
                for (int y = 0; y < tpr; y++)
                {
                    bool isItemReal = _gameViewModel.IsItemReal(x, y);
                
                    if (isItemReal == true)
                    {
                        Image item = new Image();
                        grid_Map.Children.Add(item);
                        item.Name = "Item";
                        item.Tag = $"{x}-{y}";

                        string path = _gameViewModel.GetItemIconPath();
                        item.Source = ReturnImageSource(path);

                        Grid.SetColumn(item, x);
                        Grid.SetRow(item, y);
                    }
                }
            }
        }

        private void RemoveAllItemsFromGrid()
        {
            RemoveAllGameObjectsByName(grid_Map, "Item");
        }

        private void RemoveSpecificItem(int x, int y)
        {
            UIElementCollection collection = grid_Map.Children;

            List<Image> images = new List<Image>();

            foreach (var item in collection)
            {
                if (item is Image)
                {
                    Image image = (Image)item;
                    string tag = $"{x}-{y}";

                    if (image.Tag.ToString() == tag)
                    {
                        images.Add(image);
                    }
                }
            }

            foreach (var item in images)
            {
                collection.Remove(item);
            }
        }

        private void CheckForItems()
        {
             _gameViewModel.GetCharacterIconPosition(out int x, out int y);

            bool itemFound = _gameViewModel.DoesTileHaveItems(x, y);

            if (itemFound == true)
            {
                _gameViewModel.MoveItemToInventory(x, y);
                RemoveSpecificItem(x, y);
            }
        }
        #endregion

        #region PassiveNPC SPAWN, DESPAWN
        private void AddPassivesToGrid()
        {
            int tpr = _gameViewModel.GetTotalTilesPerRow();
            for (int x = 0; x < tpr; x++)
            {
                for (int y = 0; y < tpr; y++)
                {
                    bool isPassiveThere = _gameViewModel.IsPassiveThere(x, y);

                    if (isPassiveThere == true)
                    {
                        Image p = new Image();
                        grid_Action.Children.Add(p);
                        p.Name = "Passive";
                        p.Tag = $"c{x}_r{y}";
                        p.MouseRightButtonUp += Rt_Clicked_Merchant;
                        p.MouseEnter += HoverOverPassiveNPCs;
                        p.MouseLeave += RemoveHoverTooltip;

                        string path = _gameViewModel.GetMerchantIcon();
                        p.Source = ReturnImageSource(path);

                        Grid.SetColumn(p, x);
                        Grid.SetRow(p, y);
                    }
                }
            }
        }

        private void HoverOverPassiveNPCs(object sender, RoutedEventArgs e)
        {
            _gameViewModel.HoverOverPassive();
        }

        private void RemovePassivesFromGrid()
        {
            RemoveAllGameObjectsByName(grid_Action, "Passive");
        }
        #endregion

        #region Merchant MENU METHODS
        private void Rt_Clicked_Merchant(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("Merchant");
            DisplayItemLists(GameInfo.Character.Role.Merchant, shop_Obj);
        }

        private void DisplayItemLists(GameInfo.Character.Role role, StackPanel panelObj)
        {
            (List<String>, List<String>, List<String>) obj = _gameViewModel.GetRoleInventory(role);

            List<String> name = obj.Item1;
            List<String> desc = obj.Item2;
            List<String> tag = obj.Item3;

            for (int i = 0; i < name.Count; i++)
            {
                RadioButton btn = new RadioButton
                {
                    Tag = $"{tag[i]}",
                    Content = $"{name[i]}",
                    Name = $"{name[i]}"
                };
                panelObj.Children.Add(btn);

                TextBlock block = new TextBlock
                {
                    Tag = $"{tag[i]}",
                    TextWrapping = TextWrapping.Wrap,
                    Text = $"{desc[i]}\n"
                };
                panelObj.Children.Add(block);
            }
        }

        private void ClearItemLists(StackPanel panelObj)
        {
            UIElementCollection collection = panelObj.Children;
            List<Object> storage = new List<Object>();

            foreach (var item in collection)
            {
                storage.Add(item);
            }

            foreach (var item in storage)
            {
                collection.Remove((UIElement)item);
            }
        }

        private void PurchaseItemFromShop()
        {
            if (IsRadioButtonChecked(shop_Obj, out string name))
            {
                _gameViewModel.TestTransaction(name);
            }
        }

        private void Btn_Merchant_PurchaseItem(object sender, RoutedEventArgs e)
        {
            PurchaseItemFromShop();
            ClearItemLists(shop_Obj);
            DisplayItemLists(GameInfo.Character.Role.Merchant, shop_Obj);
        }

        private void Btn_Merchant_Closed(object sender, RoutedEventArgs e)
        {
            _gameViewModel.ChangeGamestates("ReturnGame");
            ClearItemLists(shop_Obj);
        }
        #endregion

        #region Enemy SPAWN, DESPAWN METHODS
        private void AddEnemiesToGrid()
        {
            int tpr = _gameViewModel.GetTotalTilesPerRow();
            for (int x = 0; x < tpr; x++)
            {
                for (int y = 0; y < tpr; y++)
                {
                    bool isEnemyThere = _gameViewModel.IsOccupiedByEnemy(x, y);

                    if (isEnemyThere == true)
                    {
                        Image e = new Image();
                        grid_Action.Children.Add(e);
                        e.Name = "Enemy";
                        e.Tag = $"c{x}_r{y}";
                        e.MouseRightButtonUp += Enemy_Right_Clicked;
                        e.MouseEnter += HoverTooltipDisplay;
                        e.MouseLeave += RemoveHoverTooltip;

                        string path = _gameViewModel.GetEnemyIconPath();
                        e.Source = ReturnImageSource(path);

                        Grid.SetColumn(e, x);
                        Grid.SetRow(e, y);
                    }
                }
            }
        }

        private void Enemy_Right_Clicked(object sender, RoutedEventArgs e)
        {
            Image img = (Image)e.Source;
            string tag = (string)img.Tag;
            bool isEnemyRemoved = _gameViewModel.TestPlayerAttack(tag);
            if (isEnemyRemoved == true)
            {
                RemoveSpecificEnemyFromGrid(tag);
            }
            else
            {
                bool didPlayerDie = _gameViewModel.EnemyDefends(tag);
                if (didPlayerDie == true)
                {
                    Close();
                }
            }
        }

        private void RemoveSpecificEnemyFromGrid(string tag)
        {
            UIElementCollection collection = grid_Action.Children;
            List<Image> images = new List<Image>();

            foreach (var elem in collection)
            {
                if (elem is Image)
                {
                    Image image = (Image)elem;
                    images.Add(image);
                }
            }

            foreach (var img in images)
            {
                string name = img.Name;
                if (name == "Enemy")
                {
                    if (img.Tag.ToString() == tag)
                    {
                        collection.Remove(img);
                    }
                }
            }
        }

        private void RemoveAllEnemiesFromGrid()
        {
            RemoveAllGameObjectsByName(grid_Action, "Enemy");
        }
        #endregion

        #region HOVER TOOLTIP DISPLAY
        private void HoverTooltipDisplay(object sender, RoutedEventArgs e)
        {
            Image img = (Image)e.Source;
            string tag = (string)img.Tag;
            _gameViewModel.HoverOverInfo(tag);
        }

        private void RemoveHoverTooltip(object sender, RoutedEventArgs e)
        {
            _gameViewModel.RemoveHoverInfo();
        }
        #endregion

        #region Character RESPONSE METHODS
        private void CreateCharacterIcon()
        {
            Image character = new Image();
            grid_Action.Children.Add(character);
            character.Name = "Character";

            string path = _gameViewModel.GetCharacterIconPath();
            character.Source = ReturnImageSource(path);

            _gameViewModel.GetCharacterIconPosition(out int x, out int y);
            Grid.SetColumn(character, x);
            Grid.SetRow(character, y);
        }

        private void ChangeCharacterIconPosition()
        {
            _gameViewModel.GetCharacterIconPosition(out int x, out int y);

            UIElementCollection collection = grid_Action.Children;

            foreach (var item in collection)
            {
                Image img = (Image)item;
                if (img.Name == "Character")
                {
                    Image character = img;
                    Grid.SetColumn(character, x);
                    Grid.SetRow(character, y);
                }
            }
        }

        private bool PlayerCanMove(RoutedEventArgs e)
        {
            bool canMove = false;
            Button tile = (Button)e.Source;
            string tag = (string)tile.Tag;
            canMove = _gameViewModel.DoPlayerMovement(tag);
            return canMove;
        }

        private bool MoveIsExit()
        {
            _gameViewModel.GetCharacterIconPosition(out int x, out int y);
            bool isExit = _gameViewModel.IsTileExit(x, y);
            return isExit;
        }
        #endregion

        #region MAP Grid GENERATOR
        private void SetGridDefinitions(Grid grid)
        {
            for (int x = 0; x < _gameViewModel.GetTotalTilesPerRow(); x++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();

                columnDefinition.Name = $"cd_{x}";
                rowDefinition.Name = $"rd_{x}";

                grid.ColumnDefinitions.Add(columnDefinition);
                grid.RowDefinitions.Add(rowDefinition);
            }
        }

        private ImageBrush PrepareTileBackground(int x, int y)
        {
            ImageBrush brush = new ImageBrush();

            string path = _gameViewModel.GetTileImagePath(x, y);

            ImageSource imageSource = ReturnImageSource(path);
            brush.ImageSource = imageSource;

            return brush;
        }

        private void CreateMapGrid()
        {
            int tpr = _gameViewModel.GetTotalTilesPerRow();
            int dim = _gameViewModel.GetTileDimensions();

            for (int x = 0; x < tpr; x++)
            {
                for (int y = 0; y < tpr; y++)
                {
                    Button tile = new Button();

                    grid_Map.Children.Add(tile);

                    tile.Tag = $"c{x}_r{y}";
                    tile.Name = _gameViewModel.GetTileName(x, y);

                    tile.Click += Tile_Click;

                    tile.Width = dim;
                    tile.Height = dim;
                    tile.Background = PrepareTileBackground(x, y);
                    tile.Style = (Style)Resources["sty_btn_tile"];

                    Grid.SetColumn(tile, x);
                    Grid.SetRow(tile, y);
                }
            }
        }

        private void UpdateMapGridTiles()
        {
            UIElementCollection collection = grid_Map.Children;

            int tpr = _gameViewModel.GetTotalTilesPerRow();
            int item = 0;

            for (int x = 0; x < tpr; x++)
            {
                for (int y = 0; y < tpr; y++)
                {
                    Button tile = (Button)collection[item];
                    tile.Name = _gameViewModel.GetTileName(x, y);
                    tile.Tag = $"c{x}_r{y}";
                    tile.Background = PrepareTileBackground(x, y);
                    collection[item] = tile;
                    item++;
                }
            }
        }

        private void TransitionDungeonLayers()
        {
            _gameViewModel.DungeonTransition();

            RemoveAllItemsFromGrid();
            RemoveAllEnemiesFromGrid();
            RemovePassivesFromGrid();

            UpdateMapGridTiles();

            AddItemsToGrid();
            AddEnemiesToGrid();
            AddPassivesToGrid();

            _gameViewModel.MatchPlayerPositionToEntrance();
            ChangeCharacterIconPosition();
        }
        #endregion

        #region Tile CLICK METHODS
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerCanMove(e))
            {
                ChangeCharacterIconPosition();

                CheckForItems();

                if (MoveIsExit())
                {
                    TransitionDungeonLayers();
                }
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Checks if a radio button is checked in a given StackPanel object.
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="name"></param>
        /// <returns>The name of the checked button. True if checked.</returns>
        private bool IsRadioButtonChecked(StackPanel panel, out string name)
        {
            UIElementCollection collection = panel.Children;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] is RadioButton btn)
                {
                    if (btn.IsChecked == true)
                    {
                        name = btn.Name;
                        return true;
                    }
                }
            }

            name = null;
            return false;
        }

        /// <summary>
        /// Removes all objects from a given grid based on the generic name provided.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objName"></param>
        private void RemoveAllGameObjectsByName(Grid grid, string objName)
        {
            UIElementCollection collection = grid.Children;

            List<Image> images = new List<Image>();

            foreach (var elem in collection)
            {
                if (elem is Image image)
                {
                    images.Add(image);
                }
            }

            foreach (var img in images)
            {
                string iName = img.Name;
                if (iName == objName)
                {
                    collection.Remove(img);
                }
            }
        }

        /// <summary>
        /// Converts a string address path to an ImageSource.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>An ImageSource object, which can be used by Image.Source properties.</returns>
        private ImageSource ReturnImageSource(string path)
        {
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            ImageSource source = (ImageSource)imageSourceConverter.ConvertFromString(path);
            return source;
        }
        #endregion
    }
}

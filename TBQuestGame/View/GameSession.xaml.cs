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

            CreateCharacterIcon();
        }
        #endregion

        #region INTERFACE BUTTONS
        private void btn_Inventory_Clicked(object sender, RoutedEventArgs e)
        {
            // Update when the inventory is added
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
        #endregion

        #region Character RESPONSE METHODS
        private void CreateCharacterIcon()
        {
            Image character = new Image();
            grid_Action.Children.Add(character);
            character.Name = "Character";

            string path = _gameViewModel.GetCharacterIconPath();
            character.Source = ReturnImageSource(path);

            (int, int) pos = _gameViewModel.GetCharacterIconPosition();
            Grid.SetColumn(character, pos.Item1);
            Grid.SetRow(character, pos.Item2);
        }

        private void ChangeCharacterIconPositionAlternate()
        {
            (int, int) coords = _gameViewModel.GetCharacterIconPosition();
            int x = coords.Item1;
            int y = coords.Item2;

            UIElementCollection collection = grid_Action.Children;

            // At the moment, this is only prepared for the character object.
            // Grid updates for other objects, like enemies or items, will not be added until necessary.

            Image character = (Image)collection[0];
            Grid.SetColumn(character, x);
            Grid.SetRow(character, y);
        }

        private (int, int) ChangeCharacterIconPosition(string tag)
        {
            (int, int) coords = _gameViewModel.CharacterCoordinates(tag);
            int x = coords.Item1;
            int y = coords.Item2;

            UIElementCollection collection = grid_Action.Children;

            // At the moment, this is only prepared for the character object.
            // Grid updates for other objects, like enemies or items, will not be added until necessary.

            Image character = (Image)collection[0];
            Grid.SetColumn(character, x);
            Grid.SetRow(character, y);

            return (x, y);
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
            int TotalTiles = _gameViewModel.GetTotalGridTiles();

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

        private string GetEntranceDoorTag()
        {
            string tag = "";
            int position = _gameViewModel.RetrievePositionOfEntrance();
            UIElementCollection collection = grid_Map.Children;
            Button tile = (Button)collection[position];
            tag = (string)tile.Tag;
            return tag;
        }
        #endregion

        #region Tile CLICK METHODS
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            Button tile = (Button)e.Source;
            string tag = (string)tile.Tag;
            bool canMove = _gameViewModel.DoPlayerMovement(tag);

            if (canMove == true)
            {
                (int, int) coords = ChangeCharacterIconPosition(tag);

                int x = coords.Item1;
                int y = coords.Item2;

                bool isExit = _gameViewModel.IsTileExit(x, y);

                if (isExit == true)
                {
                    _gameViewModel.DungeonTransition();
                    UpdateMapGridTiles();
                    _gameViewModel.MatchPlayerPositionToEntrance();
                    ChangeCharacterIconPositionAlternate();
                }
            }
        }
        #endregion

        #region Image RELATED METHODS
        private ImageSource ReturnImageSource(string path)
        {
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            ImageSource source = (ImageSource)imageSourceConverter.ConvertFromString(path);
            return source;
        }
        #endregion
    }
}

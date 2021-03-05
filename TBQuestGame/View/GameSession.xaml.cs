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
        GameViewModel _gameViewModel;

        public GameSession(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            InitializeComponent();
        }

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetGridDefinitions();
            CreateMapGrid();
        }

        private void SetGridDefinitions()
        {
            for (int x = 0; x < _gameViewModel.GetTotalTilesPerRow(); x++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();

                columnDefinition.Name = $"cd_{x}";
                rowDefinition.Name = $"rd_{x}";

                grid_Map.ColumnDefinitions.Add(columnDefinition);
                grid_Map.RowDefinitions.Add(rowDefinition);
            }

            Console.WriteLine("Grid columns and rows added to the tile grid.");
        }

        private ImageBrush PrepareTileBackground(int x, int y)
        {
            ImageBrush brush = new ImageBrush();

            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();

            string path = _gameViewModel.GetTileImagePath(x, y);

            ImageSource imageSource = (ImageSource)imageSourceConverter.ConvertFromString(path);

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

                    // Requires a tile name

                    // Requires a click function
                    //
                    //      The function will return the name to the viewmodel
                    //      where it will move through a switch-case method.
                    //
                    //      It will be tested up against the player's location
                    //      where it will check to see if the player can move
                    //      into the tile they desire.
                    //      
                    //      It will also need to check if the player's location
                    //      is on a tile with a name of "exit".

                    tile.Width = dim;
                    tile.Height = dim;
                    tile.Background = PrepareTileBackground(x, y);
                    tile.Style = (Style)Resources["sty_btn_tile"];

                    Grid.SetColumn(tile, x);
                    Grid.SetRow(tile, y);
                }
            }

            Console.WriteLine("All grid tiles created.");
        }
    }
}

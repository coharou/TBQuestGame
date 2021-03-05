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
            ColumnDefinition columnDefinition = new ColumnDefinition();
            RowDefinition rowDefinition = new RowDefinition();

            for (int x = 0; x < _gameViewModel.GetTotalTilesPerRow(); x++)
            {
                columnDefinition.Name = $"cd-{x}";
                rowDefinition.Name = $"rd-{x}";

                grid_Map.ColumnDefinitions.Add(columnDefinition);
                grid_Map.RowDefinitions.Add(rowDefinition);
            }

            Console.WriteLine("Grid columns and rows added to the tile grid.");
        }

        private string ImageSourcePath(int x, int y)
        {
            string beforeIndex = "{Binding MapGrid";
            string onIndex = $"[{x}, {y}].Path";

            string fullBind = beforeIndex + onIndex;

            return fullBind;
        }

        private ImageBrush PrepareTileBackground(int x, int y)
        {
            ImageBrush brush = new ImageBrush();
            ImageSourceConverter converter = new ImageSourceConverter();

            string path = ImageSourcePath(x, y);

            ImageSource source = (ImageSource)converter.ConvertFromString(path);

            brush.ImageSource = source;

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

                    tile.Background = PrepareTileBackground(x, y);
                    tile.Tag = $"c{x}-r{y}";
                    tile.BorderBrush = Brushes.Transparent;

                    Grid.SetColumn(tile, x);
                    Grid.SetRow(tile, y);

                    // Update how the button works on mouseover
                    // Update how the button works on focus
                    // Update on if the button can be tabbed into
                }
            }
        }
    }
}

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
using TBQuestGame.GameInfo;

namespace TBQuestGame.View
{
    /// <summary>
    /// Interaction logic for PlayerCustomization.xaml
    /// </summary>
    public partial class PlayerCustomization : Window
    {
        private PlayerCustomizationViewModel _viewModel;

        public PlayerCustomization(PlayerCustomizationViewModel viewModel)
        {
            _viewModel = viewModel;

            InitializeComponent();
            
            // May want a method that adds info to the screen / lists (like a for loop of armor and add it to the items source)

            // Make sure to set up the window like it is set up in the main game session (full screen, etc.)
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }
    }
}

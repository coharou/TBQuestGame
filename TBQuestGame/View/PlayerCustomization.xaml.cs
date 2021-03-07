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

            AddTraitsToScrollViewer(_viewModel.Traits);

            AddMovesToScrollViewer(_viewModel.Moves);

            AddArmorToScrollViewer(_viewModel.Armors);
            
            // May want a method that adds info to the screen / lists (like a for loop of armor and add it to the items source)

            // Make sure to set up the window like it is set up in the main game session (full screen, etc.)
        }

        private void AddMovesToScrollViewer(Moves[] moves)
        {
            for (int i = 0; i < moves.Length; i++)
            {
                CheckBox box = new CheckBox();
                box.Tag = "moves";
                box.Content = $"{moves[i].Name}";
                panel_Moves.Children.Add(box);

                TextBlock block = new TextBlock();
                block.TextWrapping = TextWrapping.Wrap;
                block.Text = $"{moves[i].DamageClass.ToString()} weapon.\n";
                panel_Moves.Children.Add(block);
            }
        }

        private void AddTraitsToScrollViewer(Traits[] traits)
        {
            for (int i = 0; i < traits.Length; i++)
            {
                CheckBox box = new CheckBox();
                box.Tag = "traits";
                box.Content = $"{traits[i].Name}";
                panel_Traits.Children.Add(box);

                TextBlock block = new TextBlock();
                block.TextWrapping = TextWrapping.Wrap;
                block.Text = $"{traits[i].Description}\n";
                panel_Traits.Children.Add(block);
            }
        }

        private void AddArmorToScrollViewer(Armor[] armors)
        {
            for (int i = 0; i < armors.Length; i++)
            {
                RadioButton btn = new RadioButton();
                btn.Tag = "armor";
                btn.Content = $"{armors[i].Name}";
                panel_Armor.Children.Add(btn);

                TextBlock block = new TextBlock();
                block.TextWrapping = TextWrapping.Wrap;
                block.Text = $"{armors[i].Description}\n";
                panel_Armor.Children.Add(block);
            }
        }

        private void Btn_Click_Confirm(object sender, RoutedEventArgs e)
        {
            bool isPlayerReady = CheckIfPlayerReady();

            if (isPlayerReady == true)
            {
                Visibility = Visibility.Hidden;
            }
        }

        private bool CheckIfPlayerReady()
        {
            bool isPlayerReady = false;
            return isPlayerReady;
        }

        private void Btn_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

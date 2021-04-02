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
        public PlayerCustomizationViewModel _viewModel;

        public PlayerCustomization(PlayerCustomizationViewModel viewModel)
        {
            _viewModel = viewModel;

            InitializeComponent();

            AddObjectsToScrollViewer(_viewModel.Traits, "traits", panel_Traits);
            AddObjectsToScrollViewer(_viewModel.Moves, "moves", panel_Moves);

            // Requires a separate function due to buttons
            AddArmorToScrollViewer(_viewModel.Armors);
        }

        private void AddObjectsToScrollViewer(GameObject[] objs, string tag, StackPanel panel)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                CheckBox box = new CheckBox();
                box.Tag = tag;
                box.Content = $"{objs[i].Name}";
                panel.Children.Add(box);

                TextBlock block = new TextBlock();
                block.TextWrapping = TextWrapping.Wrap;
                block.Text = $"{objs[i].Description}\n";
                panel.Children.Add(block);
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
                ApplyPlayerProps();
                HideDialog();
            }
        }

        private void ApplyPlayerProps()
        {
            _viewModel.Player.ArmorType = GetArmorInUse();

            List<Traits> traits = GetTraitsInUse();
            _viewModel.Player.TraitChosenFirst = traits[0];
            _viewModel.Player.TraitChosenSecond = traits[1];

            List<Moves> moves = GetMovesInUse();
            _viewModel.Player.Moves = moves;
            _viewModel.Player.SelectedMove = moves[0];
        }

        private void HideDialog()
        {
            Visibility = Visibility.Hidden;
        }

        private List<Moves> GetMovesInUse()
        {
            List<Moves> moves = new List<Moves>();

            UIElementCollection collection = panel_Moves.Children;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] is CheckBox)
                {
                    CheckBox box = (CheckBox)collection[i];

                    if (box.IsChecked == true)
                    {
                        string name = (string)box.Content;

                        for (int y = 0; y < _viewModel.Moves.Length; y++)
                        {
                            if (_viewModel.Moves[y].Name == name)
                            {
                                moves.Add(_viewModel.Moves[y]);
                            }
                        }
                    }
                }
            }

            return moves;
        }

        private List<Traits> GetTraitsInUse()
        {
            List<Traits> traits = new List<Traits>();

            UIElementCollection collection = panel_Traits.Children;

            for (int i = 0; i < collection.Count; i++)
            {
                if(collection[i] is CheckBox)
                {
                    CheckBox box = (CheckBox)collection[i];

                    if (box.IsChecked == true)
                    {
                        string name = (string)box.Content;

                        for (int y = 0; y < _viewModel.Traits.Length; y++)
                        {
                            if (_viewModel.Traits[y].Name == name)
                            {
                                traits.Add(_viewModel.Traits[y]);
                            }
                        }
                    }
                }
            }

            return traits;
        }

        private Armor GetArmorInUse()
        {
            Armor armor = _viewModel.Armors[0];

            UIElementCollection collection = panel_Armor.Children;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] is RadioButton)
                {
                    RadioButton btn = (RadioButton)collection[i];

                    if (btn.IsChecked == true)
                    {
                        string name = btn.Name;

                        foreach (var item in _viewModel.Armors)
                        {
                            if (item.Name == name)
                            {
                                armor = item;
                            }
                        }
                    }
                }
            }

            return armor;
        }

        private bool CheckIfPlayerReady()
        {
            bool isPlayerReady = false;

            bool areMovesReady = AreObjectsChecked(panel_Moves, 2);
            bool areTraitsReady = AreObjectsChecked(panel_Traits, 2);
            bool isArmorReady = AreObjectsChecked(panel_Armor, 1);

            if ((areMovesReady == true) && (areTraitsReady == true) && (isArmorReady == true))
            {
                isPlayerReady = true;
            }

            return isPlayerReady;
        }

        private bool AreObjectsChecked(StackPanel panel, int total_obj)
        {
            bool areObjectsReady = false;
            int objects_used = 0;

            UIElementCollection collection = panel.Children;
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] is CheckBox)
                {
                    CheckBox box = (CheckBox)collection[i];
                    if (box.IsChecked == true)
                    {
                        objects_used++;
                    }
                }

                if (collection[i] is RadioButton)
                {
                    RadioButton btn = (RadioButton)collection[i];
                    if (btn.IsChecked == true)
                    {
                        objects_used++;
                    }
                }
            }

            if (objects_used == total_obj)
            {
                areObjectsReady = true;
            }

            return areObjectsReady;
        }

        private void Btn_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

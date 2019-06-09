using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WinForms = System.Windows.Forms;

namespace HeavenBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string chosenPath;

        #region Constructor
        /// <summary>
        /// Initializes things.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region LoadDataGrid
        /// <summary>
        /// Loads all DataGrid's info.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PathTextBox.Text) || !FamiliarProperties.PathIsValid(PathTextBox.Text))
                {
                    MessageBox.Show("The .wz files were not found.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TabItem activeTab = (TabItem) DataPicker.SelectedItem;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (activeTab.Name == "FamiliarTab")
                {
                    FamiliarGrid.ItemsSource = FamiliarProperties.LoadCollectionData(chosenPath);
                }
                else
                {
                    string category = activeTab.Name.Substring(0, activeTab.Name.Length - 3);
                    GetActiveGrid().ItemsSource = FamiliarProperties.LoadWeaponData(chosenPath, category);
                }
                stopwatch.Stop();
                TimeSpan timespan = stopwatch.Elapsed;
                LoadingTimeBox.Text = $"Loading Time: {timespan.Minutes:D2}:{timespan.Seconds:D2}:{timespan.Milliseconds:D2}";
            }
            catch (IOException)
            {
                MessageBox.Show("The .wz files are being used by another application.", "Access Conflict", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region FolderDialog
        /// <summary>
        /// Gets the root folder for the .wz archives (ideally).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathTextBox_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog fbd = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = fbd.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                chosenPath = fbd.SelectedPath;
                ((TextBox)sender).Text = chosenPath;
            }
        }
        #endregion

        #region RowHighlight
        /// <summary>
        /// Makes the row of the selected cell highlighted as well.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventSetter_OnHandlerSelected(object sender, RoutedEventArgs e)
        {
            DataGridRow dgr = FindParent<DataGridRow>(sender as DataGridCell);
            dgr.Background = new SolidColorBrush(Colors.Gold);
        }

        private void EventSetter_OnHandlerLostFocus(object sender, RoutedEventArgs e)
        {
            DataGridRow dgr = FindParent<DataGridRow>(sender as DataGridCell);
            dgr.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFdcdcdc"));
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
        #endregion

        #region SearchFilter
        /// <summary>
        /// Make the datagrid show only the elements which share the textbox's text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = ((TextBox)sender).Text;
            var itemsSource = GetActiveGrid().ItemsSource;
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsSource);

            if (!string.IsNullOrEmpty(filterText))
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    foreach (PropertyInfo property in o.GetType().GetProperties())
                    {
                        if (property.PropertyType == typeof(string) || property.PropertyType == typeof(int))
                        {
                            if (property.GetValue(o).ToString().ToUpper().Contains(filterText.ToUpper()))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                };
            }
            else
            {
                cv.Filter = null;
            }
        }

        #endregion

        private void RowSelectionCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            FamiliarGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
        }

        private void RowSelectionCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            FamiliarGrid.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
        }

        private void DataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<TabItem> tabs = GetTabs();
            List<DataGrid> datagrids = GetDatagrids();

            TabItem chosenTab = ((sender as TabControl).SelectedItem as TabItem);
            if(chosenTab.Name == "FamiliarTab")
            {
                datagrids[0].Visibility = Visibility.Visible;
                datagrids[1].Visibility = Visibility.Collapsed;
            }
            else
            {
                datagrids[0].Visibility = Visibility.Collapsed;
                datagrids[1].Visibility = Visibility.Visible;
            }
            /*
            foreach (TabItem tab in tabs)
            {
                if(tab.Name == chosenTab.Name)
                {
                    // datagrids[tabs.IndexOf(tab)].Visibility = Visibility.Visible;
                    if (tab.Name == "FamiliarTab")
                    {
                        datagrids[0].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        datagrids[1].Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    // datagrids[tabs.IndexOf(tab)].Visibility = Visibility.Collapsed;
                    if (tab.Name == "FamiliarTab")
                    {
                        datagrids[0].Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        datagrids[1].Visibility = Visibility.Collapsed;
                    }
                }
            }*/
        }

        private DataGrid GetActiveGrid()
        {
            List<DataGrid> datagrids = GetDatagrids();

            foreach (DataGrid datagrid in datagrids)
            {
                if(datagrid.Visibility == Visibility.Visible)
                {
                    return datagrid;
                }
            }
            return null;
        }

        private List<TabItem> GetTabs()
        {
            return new List<TabItem>()
            {
                FamiliarTab,
                WeaponTab,
                CapTab,
            };
        }

        private List<DataGrid> GetDatagrids()
        {
            return new List<DataGrid>()
            {
                FamiliarGrid,
                EquipGrid,
            };
        }

        private void FamiliarGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedCells = FamiliarGrid.SelectedCells;
            if(selectedCells.Count == 0)
            {
                ResetInfoPage();
                return;
            }
            Familiar familiar = selectedCells[0].Item as Familiar;
            foreach (var selectedCell in selectedCells)
            {
                Familiar selectedFamiliar = selectedCell.Item as Familiar;
                if(selectedFamiliar.FamiliarID != familiar.FamiliarID)
                {
                    ResetInfoPage();
                    return;
                }
            }
            MobImage.Source = familiar.MobImage;
            MobLevel.Text = $"Lv. {familiar.Level} ";
            MobName.Text = familiar.MobName;

            MobRarity.Text = $"\nRarity: {familiar.Rarity}";

            MobATT.Text = $"\nATT: {familiar.ATT.ToString()}";
            MobRange.Text = $"Range: {familiar.Range.ToString()}";

            MobSkillName.Text = $"\nSkill Name: {familiar.SkillName}";
            MobSkillCategory.Text = $"Skill Category: {familiar.SkillCategory}";
            MobSkillDescription.Text = $"Skill Description: {familiar.SkillDescription}";  

            MobPassiveEffect.Text = $"\nPassive Effect: {familiar.PassiveEffect}";
            MobPassiveEffectBonus.Text = $"Passive Effect Bonus: {familiar.PassiveEffectBonus}";
            MobPassiveEffectTarget.Text = $"Passive Effect Target: {familiar.PassiveEffectTarget}";

            MobID.Text = $"\nMob ID: {familiar.MobID.ToString()}";
            MobCardID.Text = $"Card ID: {familiar.CardID.ToString()}";
            MobPassiveEffectID.Text = $"Passive Effect ID: {familiar.PassiveEffectID.ToString()}";
        }

        private void ResetInfoPage()
        {
            MobImage.Source = null;
            MobLevel.Text = "";
            MobName.Text = "";
            MobATT.Text = "";
            MobRarity.Text = "";
            MobSkillName.Text = "";
            MobSkillCategory.Text = "";
            MobSkillDescription.Text = "";
            MobRange.Text = "";
            MobPassiveEffect.Text = "";
            MobPassiveEffectBonus.Text = "";
            MobPassiveEffectTarget.Text = "";
            MobID.Text = "";
            MobCardID.Text = "";
            MobPassiveEffectID.Text = "";
        }
    }
}

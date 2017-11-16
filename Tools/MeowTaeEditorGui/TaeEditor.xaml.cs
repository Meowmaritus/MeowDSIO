using MeowDSIO.DataFiles;
using MeowDSIO.DataTypes.TAE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeowTaeEditorGui
{
    public enum TaeDataGrid
    {
        EventGrid,
        AnimList,
        AnimGroupList
    }
    /// <summary>
    /// Interaction logic for TaeEditor.xaml
    /// </summary>
    public partial class TaeEditor : UserControl
    {
        private const string WHY_NO_GRAYED_OUT = "\n\n\nThe amount of code I would be required to write in order to gray out\n" +
            "these menu items when the action is not available is absolutely staggering,\n" +
            "so you'll just have to put up with this for now.";

        private void ShowExclamation(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void ShowGrayedOutOptionExclamation(string reason)
        {
            ShowExclamation(reason + WHY_NO_GRAYED_OUT, "Action Not Available");
        }

        private Dictionary<TaeDataGrid, Thread> pendingRefreshThreads = new Dictionary<TaeDataGrid, Thread>
        {
            [TaeDataGrid.EventGrid] = null,
            [TaeDataGrid.AnimList] = null,
            [TaeDataGrid.AnimGroupList] = null,
        };

        private void TriggerRefresh(TaeDataGrid dgType)
        {
            if (pendingRefreshThreads[dgType]?.IsAlive ?? false)
                return;

            pendingRefreshThreads[dgType] = new Thread(new ThreadStart(() => { while (true) {
                bool success = false;
                Dispatcher.Invoke(() => {
                    try {
                        if (dgType == TaeDataGrid.EventGrid)
                            EventGrid.Items.Refresh();
                        else if (dgType == TaeDataGrid.AnimList)
                            AnimList.Items.Refresh();
                        else if (dgType == TaeDataGrid.AnimGroupList)
                            AnimGroupList.Items.Refresh();
                        success = true;
                    } catch { } });
                if (success)
                    break;
                Thread.Sleep(50);
            }})){ IsBackground = true };

            pendingRefreshThreads[dgType].Start();
        }

        private TAE _currentTae;

        public TAE CurrentTae
        {
            get => _currentTae;
            set
            {
                _currentTae = value;
                NewTaeInit();
            }
        }

        public string CurrentTaeName { get; set; }

        private void NewTaeInit()
        {
            AnimList.ItemsSource = CurrentTae.Animations;
            AnimGroupList.ItemsSource = CurrentTae.AnimationGroups;

            CurrentAnimationProperties.Visibility = AnimList.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            CurrentAnimationProperties.IsEnabled = AnimList.Items.Count > 0;

            if (AnimList.Items.Count > 0)
                AnimList.SelectedIndex = 0;

            SibName.DataContext = CurrentTae;
            SibName.SetBinding(TextBox.TextProperty, new Binding("SibName"));

            SkeletonName.DataContext = CurrentTae;
            SkeletonName.SetBinding(TextBox.TextProperty, new Binding("SkeletonName"));
        }

        public TaeEditor()
        {
            InitializeComponent();
        }

        private void EventGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if ((string)e.Column.Header == "Event Type" && e.EditAction == DataGridEditAction.Commit)
            {
                TriggerRefresh(TaeDataGrid.EventGrid);
            }
        }

#region KeyDown

        private void AnimList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Delete_AnimList();
            }
        }

        private void AnimGroupList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Delete_AnimGroupList();
            }

        }

        private void EventGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V &&
                (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {

                // 2-dim array containing clipboard data
                string[][] clipboardData =
                    ((string)Clipboard.GetData(DataFormats.Text)).Split('\n')
                    .Select(row =>
                        row.Split('\t')
                        .Select(cell =>
                            cell.Length > 0 && cell[cell.Length - 1] == '\r' ?
                            cell.Substring(0, cell.Length - 1) : cell).ToArray())
                    .Where(a => a.Any(b => b.Length > 0)).ToArray();

                // the index of the first DataGridRow          
                int startRow = EventGrid.ItemContainerGenerator.IndexFromContainer(
                   (DataGridRow)EventGrid.ItemContainerGenerator.ContainerFromItem
                   (EventGrid.SelectedCells[0].Item));
                int targetRowCount = EventGrid.SelectedCells.Count;

                // the destination rows 
                //  (from startRow to either end or length of clipboard rows)
                DataGridRow[] rows =
                    Enumerable.Range(
                        startRow, Math.Min(EventGrid.Items.Count, targetRowCount))
                    .Select(rowIndex =>
                        EventGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow)
                    .Where(a => a != null).ToArray();

                // the destination columns 
                //  (from selected row to either end or max. length of clipboard colums)
                DataGridColumn[] columns =
                    EventGrid.Columns.OrderBy(column => column.DisplayIndex)
                    .SkipWhile(column => column != EventGrid.CurrentCell.Column)
                    .Take(clipboardData.Max(row => row.Length)).ToArray();

                for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
                {
                    string[] rowContent = clipboardData[rowIndex % clipboardData.Length];
                    for (int colIndex = 0; colIndex < columns.Length; colIndex++)
                    {
                        string cellContent =
                            colIndex >= rowContent.Length ? "" : rowContent[colIndex];
                        columns[colIndex].OnPastingCellClipboardContent(
                            rows[rowIndex].Item, cellContent);
                    }
                }

                TriggerRefresh(TaeDataGrid.EventGrid);
            }
            else if (e.Key == Key.Delete)
            {
                Delete_EventGrid();
            }
        }
#endregion

        private void AnimList_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            bool isThisTheFirstItem = AnimList.Items.Count == 0;
            e.NewItem = new AnimationRef(CurrentTae.Animations.Count > 0 ? CurrentTae.Animations.Last().ID + 1 : 0);
            
            if (isThisTheFirstItem)
            {
                AnimList.SelectedIndex = 0;
                AnimGroupList.CanUserAddRows = true;
                CurrentAnimationProperties.Visibility = Visibility.Visible;
                CurrentAnimationProperties.IsEnabled = true;
            }
        }

        private void AnimGroupList_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            int first = 0, last = 0;
            if (CurrentTae.AnimationGroups.Count > 0)
            {
                first = CurrentTae.AnimationGroups[CurrentTae.AnimationGroups.Count - 1].FirstID;
                last = CurrentTae.AnimationGroups[CurrentTae.AnimationGroups.Count - 1].LastID;
            }
            else if (CurrentTae.Animations.Count > 0)
            {
                var sorted = CurrentTae.Animations.OrderBy(x => x.ID);
                first = sorted.First().ID;
                last = sorted.Last().ID;
            }

            e.NewItem = new AnimationGroup(CurrentTae.AnimationGroups.Count + 1) { FirstID = first, LastID = last };
        }

        private void EventGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            var selAnim = (AnimList.SelectedItem as AnimationRef);
            e.NewItem = new AnimationEvent(selAnim.Anim.Events.Count + 1, AnimationEventType.ApplySpecialProperty, selAnim.ID);
        }

        private bool CheckDelete(string thingToDelete)
        {
            return MessageBox.Show($"Are you sure you want to delete {thingToDelete}?", "Delete?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        #region Delete_
        private void Delete_AnimList()
        {
            if (AnimList.Items.Count == 0)
            {
                ShowGrayedOutOptionExclamation("No animations to delete.");
                return;
            }
            else if (AnimList.Items.Count == 1)
            {
                AnimList.SelectedIndex = -1;
            }

            bool isThisTheLastItem = AnimList.Items.Count == 1;

            var selectedAnimRef = AnimList.SelectedItem as AnimationRef;
            if (CheckDelete($"animation {selectedAnimRef.ID}"))
            {
                CurrentTae.Animations.Remove(selectedAnimRef);
                TriggerRefresh(TaeDataGrid.AnimList);
                TriggerRefresh(TaeDataGrid.AnimGroupList);

                if (isThisTheLastItem)
                {
                    CurrentAnimationProperties.Visibility = Visibility.Collapsed;
                    CurrentAnimationProperties.IsEnabled = false;
                }
            }
        }
        
        private void Delete_AnimGroupList()
        {
            if (AnimGroupList.Items.Count == 0)
            {
                ShowGrayedOutOptionExclamation("No animation groups to delete.");
                return;
            }
            else if (AnimGroupList.Items.Count == 1)
            {
                AnimGroupList.SelectedIndex = -1;
            }

            var selectedCount = AnimGroupList.SelectedItems.Count;

            if (selectedCount == 0)
            {
                var selectedItem = AnimGroupList.SelectedItem as AnimationGroup;
                if (CheckDelete($"animation group #{selectedItem.DisplayIndex}"))
                {
                    CurrentTae.AnimationGroups.Remove(selectedItem);

                    CurrentTae.UpdateAnimGroupIndices();
                    AnimGroupList.Items.Refresh();
                }
            }
            else
            {
                if (CheckDelete(selectedCount == 1
                        ? $"animation group #{(AnimGroupList.SelectedItems[0] as AnimationGroup).DisplayIndex}"
                        : $"the {selectedCount} selected animation groups"))
                {
                    foreach (var selItem in AnimGroupList.SelectedItems)
                    {
                        CurrentTae.AnimationGroups.Remove(selItem as AnimationGroup);
                    }

                    CurrentTae.UpdateAnimGroupIndices();
                    AnimGroupList.Items.Refresh();
                }

            }
        }

        private void Delete_EventGrid()
        {
            if (EventGrid.Items.Count == 0)
            {
                ShowGrayedOutOptionExclamation("No items to delete.");
                return;
            }
            else if (EventGrid.Items.Count == 1)
            {
                EventGrid.SelectedIndex = -1;
            }

            var selectedCount = EventGrid.SelectedItems.Count;

            if (selectedCount == 0)
            {
                var selectedItem = EventGrid.SelectedItem as AnimationEvent;
                if (CheckDelete($"event #{selectedItem.DisplayIndex}"))
                {
                    (AnimList.SelectedItem as AnimationRef).Anim.Events.Remove(selectedItem);
                    (AnimList.SelectedItem as AnimationRef).Anim.UpdateEventIndices();
                    EventGrid.Items.Refresh();
                }
            }
            else
            {
                if (CheckDelete(selectedCount == 1
                        ? $"event #{(EventGrid.SelectedItems[0] as AnimationEvent).DisplayIndex}"
                        : $"the {selectedCount} selected events"))
                {
                    var animRef = (AnimList.SelectedItem as AnimationRef);

                    foreach (var selItem in EventGrid.SelectedItems)
                    {
                        animRef.Anim.Events.Remove(selItem as AnimationEvent);
                    }

                    animRef.Anim.UpdateEventIndices();
                    EventGrid.Items.Refresh();
                }

            }

        }

        #endregion

        private void EventGridContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete_EventGrid();
        }

        private void AnimListContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete_AnimList();
        }

        private void AnimListContextMenu_ResetNameToDefault_Click(object sender, RoutedEventArgs e)
        {
            (AnimList.SelectedItem as AnimationRef).ResetToDefaultFileName();
            TriggerRefresh(TaeDataGrid.AnimList);
        }

        private void AnimGroupListContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete_AnimGroupList();
        }

        private void AnimList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                TriggerRefresh(TaeDataGrid.AnimGroupList);
            }
        }

        //private void AnimGroupList_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    var item = (e.Row.Item as AnimationGroup);
        //    var rowIndex = CurrentTae.AnimationGroups.IndexOf(item);
        //    if (rowIndex >= 0)
        //    {
        //        item.DisplayIndex = rowIndex;
        //        e.Row.Item = item;
        //    }
        //}
    }
}

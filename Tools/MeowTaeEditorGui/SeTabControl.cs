using System;
using Xceed.Wpf.AvalonDock.Layout;
using System.Windows;
using System.IO;
using System.Xml;
using System.Windows.Input;
using MeowDSIO.DataFiles;
using System.Windows.Controls;

namespace MeowTaeEditorGui
{
    public class SeTabControl : TabControl
    {
        public TaeEditor Editor { get; private set; }

        public MainWindow Main;

        public event EventHandler<SeTabEventArgs> NewTabSelected;
        public event EventHandler<SeTabEventArgs> OldTabDeselected;
        public event EventHandler<SeTabEventArgs> CurrentTabIsModifiedChanged;

        private SeTab __selectedSeTab;
        public SeTab SelectedSeTab
        {
            get
            {
                object selCont = null;
                Dispatcher.Invoke(() =>
                {
                    selCont = SelectedItem;
                });
                return selCont as SeTab;
            }
            set
            {
                OnOldTabDeselected(__selectedSeTab);
                OnNewTabSelected(value);

                __selectedSeTab = value;
            }
        }

        public SeTabControl() : base()
        {

        }

        private void InitEditor()
        {
            Editor = new TaeEditor();
        }

        private void HookEditorEvents()
        {
            Editor.AnimList.CellEditEnding += AnimList_CellEditEnding;
            Editor.AnimGroupList.CellEditEnding += AnimGroupList_CellEditEnding;
            Editor.EventGrid.CellEditEnding += EventGrid_CellEditEnding;
        }

        private void EventGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                SelectedSeTab.IsModified = true;
            }
        }

        private void AnimGroupList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                SelectedSeTab.IsModified = true;
            }
        }

        private void AnimList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                SelectedSeTab.IsModified = true;
            }
        }

        public SeTab AddNewTab(Action<bool> loading)
        {
            SeTab newTab = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Editor == null)
                {
                    InitEditor();
                    HookEditorEvents();
                }

                newTab = new SeTab(this, loading);

                if (Items.Count == 0)
                {
                    newTab.Content = Editor;
                    Editor.CurrentTae = newTab.CurrentTae;
                    Editor.CurrentTaeName = newTab.CurrentTaeName;
                }
                Items.Add(newTab);
                newTab.InitEvents();

                newTab.IsModified = false;

                loading(false);
            });
            return newTab;
        }

        public void FocusTab(SeTab tab)
        {
            if (!Items.Contains(tab))
            {
                Items.Add(tab);
            }

            SelectedIndex = Items.IndexOf(tab);
        }

        public bool SaveAll(Action<bool> loading)
        {
            foreach (var thing in Items)
            {
                if (!((thing as SeTab).SeSave(loading)))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CloseAll(Action<bool> loading)
        {
            foreach (var thing in Items)
            {
                var tab = (thing as SeTab);
                
                if (tab.IsModified)
                {
                    var dlgResult = MessageBox.Show($"File '{tab.SeScriptShortName}' has unsaved changes. Would you like to save the changes before closing?",
                        $"Save '{tab.SeScriptShortName}'?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (dlgResult == MessageBoxResult.Yes)
                    {
                        tab.SeSave(loading);
                    }
                    else if (dlgResult == MessageBoxResult.Cancel)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public SeTab this[int i]
        {
            get
            {
                return Items[i] as SeTab;
            }
        }

        protected virtual void OnNewTabSelected(SeTab tab)
        {
            NewTabSelected?.Invoke(this, new SeTabEventArgs(tab));
        }

        protected virtual void OnOldTabDeselected(SeTab tab)
        {
            OldTabDeselected?.Invoke(this, new SeTabEventArgs(tab));
        }

        internal void RaiseCurrentTabIsModifiedChanged(SeTab tab)
        {
            if (SelectedSeTab == tab)
            {
                CurrentTabIsModifiedChanged?.Invoke(this, new SeTabEventArgs(tab));
            }
        }

        /// <summary>
        /// The open file dialog will be in the directory of this tab's script if applicable.
        /// </summary>
        public bool SeOpenFile(Action<bool> loading, string startDir = null)
        {
            loading(false);

            var dlg = new Microsoft.Win32.OpenFileDialog()
            {
                CheckFileExists = true,
                AddExtension = false,
                DefaultExt = ".tae",
                FileName = "",
                Filter = "TimeAct Event Files (*.TAE)|*.tae",
                Title = "Open one or more TimeAct Event file(s).",
                CheckPathExists = true,
                Multiselect = true,
                ValidateNames = true
            };

            if (startDir != null && Directory.Exists(startDir))
            {
                dlg.InitialDirectory = startDir;
            }
            

            if (dlg.ShowDialog() ?? false)
            {
                foreach (var f in dlg.FileNames)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var newTab = AddNewTab(loading);
                        newTab.CurrentTaeName = f;
                        newTab.INSTANT_LoadDocumentFromDisk(loading);
                        SelectedIndex = Items.Count - 1;
                    });
                    return true;
                }
            }

            return false;
        }
    }
}

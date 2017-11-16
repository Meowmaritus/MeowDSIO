#define DISABLE_ERR_MSG

using System;
using System.IO;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using System.ComponentModel;
using System.Threading;
using System.Text;
using MeowDSIO.DataFiles;
using MeowDSIO;
using System.Windows.Controls;
using MeowDSIO.DataTypes.TAE;
using System.Linq;

namespace MeowTaeEditorGui
{
    public class SeTab : TabItem
    {
        //Stuff:

        public const string DefaultScriptName = "New TAE (Untitled).tae";

        public Button TemplateCloseButton { get; private set; }

        public TAE CurrentTae { get; private set; } = new TAE();
        public string _currentTaeName { get; private set; }

        private AnimationRef taeEditorLastSelectedAnimation { get; set; }
        private AnimationGroup taeEditorLastSelectedAnimationGroup { get; set; }

        private bool __isModified = false;
        public bool IsModified
        {
            get
            {
                return __isModified;
            }
            set
            {
                __isModified = value;

                if (ParentLuaContainer.SelectedSeTab == this)
                {
                    ParentLuaContainer.RaiseCurrentTabIsModifiedChanged(this);
                }

                UpdateTabText();
            }
        }

        private void UpdateTabText()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Header = SeScriptShortName + (IsModified ? "*" : "");
            });
        }

        //LuaEditor storage:

        public SeTabControl ParentLuaContainer
        {
            get
            {
                return Parent as SeTabControl;
            }
        }

        //Constructor:

        public SeTab(SeTabControl parent, Action<bool> loading, string scriptFileName = null) : base()
        {
            CurrentTaeName = scriptFileName;

            if (scriptFileName != null)
            {
                INSTANT_LoadDocumentFromDisk(loading);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TemplateCloseButton = (Button)GetTemplateChild("CloseButton");
            TemplateCloseButton.Click += TemplateCloseButton_Click;
        }

        private void TemplateCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SeCheckIfCanClose((dummy) => { }))
            {
                e.Handled = true;
            }
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            ParentLuaContainer.SelectedSeTab = this;
        }

        public void InitEvents()
        {
            ParentLuaContainer.NewTabSelected += ParentLuaContainer_NewTabSelected;
            ParentLuaContainer.OldTabDeselected += ParentLuaContainer_OldTabDeselected;
        }

        private void ParentLuaContainer_OldTabDeselected(object sender, SeTabEventArgs e)
        {
            if (e.Script == this)
            {
                CurrentTae = ParentLuaContainer?.Editor.CurrentTae;
                CurrentTaeName = ParentLuaContainer?.Editor.CurrentTaeName;

                taeEditorLastSelectedAnimation = (AnimationRef)ParentLuaContainer?.Editor.AnimList.SelectedItem;

                taeEditorLastSelectedAnimationGroup = (AnimationGroup)ParentLuaContainer?.Editor.AnimGroupList.SelectedItem;

                Content = null;
            }
        }

        private void ParentLuaContainer_NewTabSelected(object sender, SeTabEventArgs e)
        {
            if (e.Script == this)
            {
                ParentLuaContainer.Editor.CurrentTae = CurrentTae;
                ParentLuaContainer.Editor.CurrentTaeName = CurrentTaeName;

                ParentLuaContainer.Editor.AnimList.SelectedItem = taeEditorLastSelectedAnimation;

                if (ParentLuaContainer.Editor.AnimList.SelectedItem != null)
                    ParentLuaContainer.Editor.AnimList.ScrollIntoView(ParentLuaContainer.Editor.AnimList.SelectedItem);

                ParentLuaContainer.Editor.AnimGroupList.SelectedItem = taeEditorLastSelectedAnimationGroup;

                if (ParentLuaContainer.Editor.AnimGroupList.SelectedItem != null)
                    ParentLuaContainer.Editor.AnimGroupList.ScrollIntoView(ParentLuaContainer.Editor.AnimGroupList.SelectedItem);

                Content = ParentLuaContainer.Editor;
            }
        }

        //protected override void OnClosing(CancelEventArgs args)
        //{
        //    base.OnClosing(args);

        //    if (!SeCheckIfCanClose((dummy) => { }))
        //    {
        //        args.Cancel = true;
        //    }
        //}

        public bool SeScriptFileExists
        {
            get
            {
                return CurrentTaeName != null && File.Exists(CurrentTaeName);
            }
        }

        public string CurrentTaeName
        {
            get
            {
                return _currentTaeName;
            }
            internal set
            {
                _currentTaeName = value;
                Header = SeScriptShortName;
            }
        }

        public string SeScriptShortName
        {
            get
            {
                return CurrentTaeName != null ? new FileInfo(CurrentTaeName).Name : DefaultScriptName;
            }
        }

        /// <summary>
        /// Saves this tab's script like you're hitting the save button. Will show a "Save As..." dialog if it is the first time saving, etc.
        /// </summary>
        /// <returns>True if the file was saved to disk in any way. False if user clicks "Cancel" to "Save As..." dialog (if applicable).</returns>
        public bool SeSave(Action<bool> loading)
        {
            return DoCheckSaveOrSaveAs(loading);
        }

        /// <summary>
        /// Shows "Save As..." dialog and saves the file to disk if the user chooses to.
        /// </summary>
        /// <returns>True if user ends up saving file. False if user clicks "Cancel".</returns>
        public bool SeSaveAs(Action<bool> loading)
        {
            return ShowSaveAsDialog(loading);
        }

        /// <summary>
        /// Checks if this tab can close ("Save unsaved changes?" etc)
        /// </summary>
        /// <returns>True if you can close. False if you can NOT close.</returns>
        public bool SeCheckIfCanClose(Action<bool> loading)
        {
            if (SeScriptFileExists && !IsModified)
            {
                return true;
            }

            var dlgResult = MessageBox.Show($"Save file \"{SeScriptShortName}\"?", "Save First?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (dlgResult == MessageBoxResult.Yes)
            {
                //User decides to save. We let the default save button logic determine the result for us ;)
                return SeSave(loading);
            }
            else if (dlgResult == MessageBoxResult.No)
            {
                //User decides not to save, so we return true.
                return true;
            }
            else
            {
                //User decides not to close tab after all.
                return false;
            }
        }

        private bool DoCheckSaveOrSaveAs(Action<bool> loading)
        {
            if (SeScriptFileExists)
            {
                INSTANT_SaveDocumentToDisk(loading);
                return true;
            }
            else
            {
                return ShowSaveAsDialog(loading);
            }
        }

        /// <summary>
        /// You are expected to perform any checks yourself before calling this or you will piss people off Kappa
        /// </summary>
        /// <param name="doc"></param>
        internal void INSTANT_SaveDocumentToDisk(Action<bool> loading)
        {
            loading(true);

            Application.Current.Dispatcher.Invoke(() =>
            {
#if !DISABLE_ERR_MSG
                try
                {
#endif
                    foreach (var g in CurrentTae.AnimationGroups)
                    {
                        if (!CurrentTae.Animations.Any(x => x.ID == g.FirstID))
                            throw new Exception("Animation group starts on animation ID not present in animation list.");

                        if (!CurrentTae.Animations.Any(x => x.ID == g.LastID))
                            throw new Exception("Animation group ends on animation ID not present in animation list.");  
                    }

                    DataFile.SaveToFile(CurrentTae, CurrentTaeName);
                    IsModified = false;
#if !DISABLE_ERR_MSG
            }
                catch (Exception e)
                {
                    MessageBox.Show("Unable to save TAE due to an error: \n\n" + e.Message + "\n\n>> NOTE: Your data has NOT been saved! <<\n"+
                        "This is a temporary workaround done to prevent file corruption and will be addressed in later releases. \n"+
                        "For now, please try to fix any issues mentioned in the above message and attempt to save again.", 
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
#endif
            });
            
            loading(false);
        }

        /// <summary>
        /// You are expected to perform any checks yourself before calling this or you will piss people off Keepo
        /// </summary>
        /// <param name="doc"></param>
        internal void INSTANT_LoadDocumentFromDisk(Action<bool> loading)
        {
            loading(true);

            Application.Current.Dispatcher.Invoke(() =>
            {
#if !DISABLE_ERR_MSG
                try
                {
#endif
                    CurrentTae = DataFile.LoadFromFile<TAE>(CurrentTaeName);
                    IsModified = false;
#if !DISABLE_ERR_MSG
            }
                catch (Exception e)
                {
                    MessageBox.Show("Unable to load TAE due to an error: \n\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
#endif
            });

            loading(false);
        }

        private bool ShowSaveAsDialog(Action<bool> loading)
        {
            loading(false);

            var dlg = new Microsoft.Win32.SaveFileDialog()
            {
                CheckFileExists = false,
                AddExtension = true,
                DefaultExt = ".lua",
                FileName = DefaultScriptName,
                Filter = "TimeAct Event Files (*.TAE)|*.TAE",
                InitialDirectory = new FileInfo(typeof(SeTab).Assembly.Location).Directory.FullName, //TODO: store last user save/load dir and load on startup
                CreatePrompt = false,
                OverwritePrompt = true,
                Title = "Save Lua Script"
            };

            if (SeScriptFileExists)
            {
                var cfi = new FileInfo(CurrentTaeName);
                dlg.InitialDirectory = cfi.Directory.FullName;
                dlg.FileName = cfi.Name;
            }

            bool doSave = dlg.ShowDialog() ?? false;

            if (doSave)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentTaeName = dlg.FileName;
                    INSTANT_SaveDocumentToDisk(loading);
                });
                return true;
            }

            return false;
        }

        /// <summary>
        /// The open file dialog will be in the directory of this tab's script if applicable.
        /// </summary>
        public bool SeOpenFile(Action<bool> loading)
        {
            return ParentLuaContainer.SeOpenFile(loading, CurrentTaeName != null ? new FileInfo(CurrentTaeName).DirectoryName : null);
        }
    }
}

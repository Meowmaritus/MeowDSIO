using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MK = System.Windows.Input.ModifierKeys;

namespace MeowTaeEditorGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<ICommand, SeButton> SeCommands = new Dictionary<ICommand, SeButton>();

        public MenuItemIndexer SeMenu;

        public class MenuItemIndexer
        {
            private readonly MainWindow main;
            public MenuItemIndexer(MainWindow m)
            {
                main = m;
            }

            public MenuItem this[SeButton b]
            {
                get
                {
                    switch (b)
                    {
                        case SeButton.NewDoc: return main.MenuNew;
                        case SeButton.OpenFile: return main.MenuOpen;
                        case SeButton.SaveAllFiles: return main.MenuSaveAll;
                        case SeButton.SaveFile: return main.MenuSave;
                        default: return null;
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SeMenu = new MenuItemIndexer(this);

            MainSeTabControl.Main = this;

            //Lazy init without creating tab  by creating tab then deleting tab lol
            MainSeTabControl.AddNewTab((x) => { });
            MainSeTabControl.Items.Clear();

            MainSeTabControl.CurrentTabIsModifiedChanged += MainSeTabControl_CurrentTabIsModifiedChanged;

            seToolbar.SeButtonClicked += SeToolbar_SeButtonClicked;
            seToolbar.SeButtonEnabledChanged += SeToolbar_SeButtonEnabledChanged;

            InitCommandBindings();
        }

        private void MainSeTabControl_CurrentTabIsModifiedChanged(object sender, SeTabEventArgs e)
        {
            seToolbar.SetButtonEnabled(SeButton.SaveFile, e.Script.IsModified);
        }

        private void SeToolbar_SeButtonEnabledChanged(object sender, SeButtonEventArgs e)
        {
            SeMenu[e.ButtonType]?.SeSetEnabled(seToolbar[e.ButtonType]?.IsHitTestVisible ?? false);
        }

        private void RegisterNewCommand(SeButton btn, MK modifierKey, Key key, MenuItem menuItem = null)
        {
            var cmd = new RoutedCommand();
            var gesture = new KeyGesture(key, modifierKey);
            cmd.InputGestures.Add(gesture);
            MainSeTabControl.Editor.CommandBindings.Add(new CommandBinding(cmd, OnSeCommand, SeCommandCanExecute));
            CommandBindings.Add(new CommandBinding(cmd, OnSeCommand, SeCommandCanExecute));
            SeCommands.Add(cmd, btn);

            if (menuItem != null)
            {
                menuItem.InputGestureText = gesture.GetDisplayStringForCulture(System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        private void InitCommandBindings()
        {
            RegisterNewCommand(SeButton.SaveFile, MK.Control, Key.S, MenuSave);
            RegisterNewCommand(SeButton.SaveAllFiles, MK.Control | MK.Alt, Key.S, MenuSaveAll);
            RegisterNewCommand(SeButton.SaveAs, MK.Control | MK.Shift, Key.S, MenuSaveAs);

            RegisterNewCommand(SeButton.NewDoc, MK.Control, Key.N, MenuNew);
            RegisterNewCommand(SeButton.OpenFile, MK.Control, Key.O, MenuOpen);
        }

        private void OnSeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SeButtonAction(SeCommands[e.Command]);
        }

        private void SeCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = seToolbar.GetButtonEnabled(SeCommands[e.Command]);
        }

        private void SeButtonAction(SeButton seButton)
        {
            seToolbar.SetButtonEnabled(seButton, false);
            ForceCursor = true;
            Cursor = Cursors.Wait;
            new Thread(new ParameterizedThreadStart((b) =>
            {
                Action<bool> loading = (isLoad) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (isLoad)
                        {
                            ForceCursor = true;
                            Cursor = Cursors.Wait;
                        }
                        else
                        {
                            Cursor = Cursors.Arrow;
                            ForceCursor = false;
                        }
                    });
                };

                var btn = b as SeButton? ?? SeButton.None;

                switch (btn)
                {
                    case SeButton.ExitEntireProgram:
                        Application.Current.Dispatcher.Invoke(() => Close());
                        break;
                    case SeButton.NewDoc:
                        Application.Current.Dispatcher.Invoke(
                            () =>
                            {
                                MainSeTabControl.AddNewTab(loading);
                                MainSeTabControl.SelectedIndex = MainSeTabControl.Items.Count - 1;
                            });
                        loading(false);
                        break;
                    case SeButton.OpenFile:
                        if (MainSeTabControl.SelectedSeTab != null)
                            MainSeTabControl.SelectedSeTab.SeOpenFile(loading);
                        else
                            MainSeTabControl.SeOpenFile(loading);
                        break;
                    case SeButton.SaveAllFiles:
                        MainSeTabControl.SaveAll(loading);
                        break;
                    case SeButton.SaveAs:
                        MainSeTabControl.SelectedSeTab.SeSaveAs(loading);
                        break;
                    case SeButton.SaveFile:
                        MainSeTabControl.SelectedSeTab.SeSave(loading);
                        break;
                }

                if (btn != SeButton.SaveFile)
                    Application.Current.Dispatcher.Invoke(() => seToolbar.SetButtonEnabled(seButton, true));

            })).Start(seButton);
        }

        private void SeToolbar_SeButtonClicked(object sender, SeButtonEventArgs e)
        {
            SeButtonAction(e.ButtonType);
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.NewDoc);
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.OpenFile);
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.SaveFile);
        }

        private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.SaveAs);
        }

        private void MenuSaveAll_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.SaveAllFiles);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.ExitEntireProgram);
        }

        private void LayoutAnchorable_IsActiveChanged(object sender, EventArgs e)
        {
            (sender as Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable).IsActive = false;
        }

        private void LayoutAnchorable_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}

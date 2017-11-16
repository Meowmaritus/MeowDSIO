using System;
using System.Windows;
using System.Windows.Controls;

namespace MeowTaeEditorGui
{
    /// <summary>
    /// Interaction logic for ScriptEditorToolbar.xaml
    /// </summary>
    public partial class SeToolbar : UserControl
    {
        public event EventHandler<SeButtonEventArgs> SeButtonClicked;

        public event EventHandler<SeButtonEventArgs> SeButtonEnabledChanged;

        protected virtual void RaiseSeButtonClicked(SeButton b)
        {
            SeButtonClicked?.Invoke(this, new SeButtonEventArgs(b));
        }

        protected virtual void RaiseSeButtonEnabledChanged(SeButton b)
        {
            SeButtonEnabledChanged?.Invoke(this, new SeButtonEventArgs(b));
        }

        public SeToolbar()
        {
            InitializeComponent();
        }

        public Button this[SeButton b]
        {
            get
            {
                switch(b)
                {
                    case SeButton.NewDoc: return ButtonNewDoc;
                    case SeButton.OpenFile: return ButtonOpenFile;
                    case SeButton.SaveAllFiles: return ButtonSaveAllFiles;
                    case SeButton.SaveFile: return ButtonSaveFile;
                    default: return null;
                }
            }
        }

        public SeButton this[Button b]
        {
            get
            {
                if (b == ButtonNewDoc) return SeButton.NewDoc;
                else if (b == ButtonOpenFile) return SeButton.OpenFile;
                else if (b == ButtonSaveAllFiles) return SeButton.SaveAllFiles;
                else if (b == ButtonSaveFile) return SeButton.SaveFile;
                return SeButton.None;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseSeButtonClicked(this[sender as Button]);
        }

        public void SetButtonEnabled(SeButton b, bool enabled)
        {
            this[b]?.SeSetEnabled(enabled);
            RaiseSeButtonEnabledChanged(b);
        }

        public bool GetButtonEnabled(SeButton b)
        {
            return this[b]?.IsHitTestVisible ?? true;
        }
    }

    public enum SeButton
    {
        ExitEntireProgram,
        None,
        NewDoc,
        OpenFile,
        SaveAllFiles,
        SaveFile,
        SaveAs,
    }

    public class SeButtonEventArgs : EventArgs
    {
        public SeButton ButtonType { get; set; }

        public SeButtonEventArgs(SeButton type)
        {
            ButtonType = type;
        }
    }


}

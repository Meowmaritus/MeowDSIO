using System;

namespace MeowTaeEditorGui
{
    public class SeTabEventArgs : EventArgs
    {
        public readonly SeTab Script;

        public SeTabEventArgs(SeTab script)
        {
            Script = script;
        }
    }
}

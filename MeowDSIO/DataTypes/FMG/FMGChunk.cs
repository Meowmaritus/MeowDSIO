using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.FMG
{
    public class FMGChunk : IList<string>
    {
        public int StartID { get; set; } = 0;
        public ObservableCollection<string> Entries = new ObservableCollection<string>();

        public FMGChunk()
        {

        }

        public override string ToString()
        {
            if (Entries.Count > 1)
                return $"FMGChunk[{StartID} -> {(StartID + (Entries.Count - 1))}]";
            else if (Entries.Count == 1)
                return $"FMGChunk[{StartID}: {(Entries[0] != null ? "\"" + Entries[0] + "\"" : "<NULL>")}]";
            else
                return "FMGChunk[Empty]";
        }

        #region IList

        public int IndexOf(string item)
        {
            return ((IList<string>)Entries).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)Entries).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)Entries).RemoveAt(index);
        }

        public string this[int index] { get => ((IList<string>)Entries)[index]; set => ((IList<string>)Entries)[index] = value; }

        public void Add(string item)
        {
            ((IList<string>)Entries).Add(item);
        }

        public void Clear()
        {
            ((IList<string>)Entries).Clear();
        }

        public bool Contains(string item)
        {
            return ((IList<string>)Entries).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((IList<string>)Entries).CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return ((IList<string>)Entries).Remove(item);
        }

        public int Count => ((IList<string>)Entries).Count;

        public bool IsReadOnly => ((IList<string>)Entries).IsReadOnly;

        public IEnumerator<string> GetEnumerator()
        {
            return ((IList<string>)Entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<string>)Entries).GetEnumerator();
        }
        #endregion

    }
}

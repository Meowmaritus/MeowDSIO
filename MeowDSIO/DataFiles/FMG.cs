using MeowDSIO.DataTypes.FMG;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MeowDSIO.DataFiles
{
    public class FMG : DataFile, IList<FMGEntryRef>
    {
        public const string NullString = "<null>";
        public const string EmptyString = "<empty>";

        private FMGHeader _header = new FMGHeader();
        public FMGHeader Header
        {
            get => _header;
            set
            {
                _header = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<FMGEntryRef> _entries = new ObservableCollection<FMGEntryRef>();
        public ObservableCollection<FMGEntryRef> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                RaisePropertyChanged();
            }
        }

        public FMG()
        {
            Entries.CollectionChanged += Entries_CollectionChanged;
        }

        private void entryValueModified(object sender, FMGEntryRefValueModifiedEventArgs e)
        {
            IsModified = true;
        }

        private void Entries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    ((FMGEntryRef)newItem).ValueModified += entryValueModified;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (var newItem in e.OldItems)
                {
                    ((FMGEntryRef)newItem).ValueModified -= entryValueModified;
                }
            }
        }

        private List<FMGChunk> CalculateChunks()
        {
            var chunks = new List<FMGChunk>();

            int startIndex = -1;
            int startID = -1;

            for (int i = 0; i < Entries.Count; i++)
            {
                if (startIndex < 0)
                {
                    startIndex = i;
                    startID = Entries[i].ID;
                    continue;
                }
                else if ((Entries[i].ID - Entries[i - 1].ID) > 1)
                {
                    chunks.Add(new FMGChunk(startIndex, startID, Entries[i - 1].ID));
                    startIndex = i;
                    startID = Entries[i].ID;
                }

            }

            // If there's an unfinished chunk, finish it
            if (chunks.Count > 0 && startIndex > chunks[chunks.Count - 1].StartIndex)
            {
                chunks.Add(new FMGChunk(startIndex, startID, Entries[Entries.Count - 1].ID));
            }

            return chunks;
        }

        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            //UniEscapeChar
            bin.ReadUInt16();

            Header.UnkFlag01 = bin.ReadByte();
            Header.UnkFlag02 = bin.ReadByte();

            //FileSize
            bin.ReadInt32();

            Header.UnkFlag03 = bin.ReadByte();
            Header.IsBigEndian = (bin.ReadByte() == FMGHeader.ENDIAN_FLAG_BIG);
            Header.UnkFlag04 = bin.ReadByte();
            Header.UnkFlag05 = bin.ReadByte();

            int chunkCount = bin.ReadInt32();
            int stringCount = bin.ReadInt32();
            int stringOffsetsBegin = bin.ReadInt32();

            //Pad
            bin.ReadUInt32();

            Entries.Clear();
            FMGChunkHeaderBuffer chunk = new FMGChunkHeaderBuffer(stringOffsetsBegin);
            for (int i = 0; i < chunkCount; i++)
            {
                chunk.FirstStringIndex = bin.ReadInt32();
                chunk.FirstStringID = bin.ReadInt32();
                chunk.LastStringID = bin.ReadInt32();

                chunk.ReadEntries(bin, Entries);
            }

            IsModified = false;
        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            var Chunks = CalculateChunks();

            bin.BigEndian = Header.IsBigEndian;

            bin.Write((ushort)0);
            bin.Write(Header.UnkFlag01);
            bin.Write(Header.UnkFlag02);

            bin.Placeholder("FileSize");

            bin.Write(Header.UnkFlag03);

            if (Header.IsBigEndian)
                bin.Write(FMGHeader.ENDIAN_FLAG_BIG);
            else
                bin.Write(FMGHeader.ENDIAN_FLAG_LITTLE);

            bin.Write(Header.UnkFlag04);
            bin.Write(Header.UnkFlag05);

            bin.Write(Chunks.Count);

            bin.Write(Entries.Count);

            bin.Placeholder("StringsBeginPointer");

            bin.Write(0); //PAD

            bin.Label("ChunksBeginOffset");

            foreach (var chunk in Chunks)
            {
                bin.Write(chunk.StartIndex);
                bin.Write(chunk.StartID);
                bin.Write(chunk.EndID);
            }

            bin.PointToHere("StringsBeginPointer");

            bin.Label("StringsBeginOffset");

            bin.Position += (Entries.Count * 4);

            var stringOffsetList = new List<int>();

            for (int i = 0; i < Entries.Count; i++)
            {
                string entryStringCheck = Entries[i].Value.Trim();

                if (entryStringCheck == NullString)
                {
                    stringOffsetList.Add(0);
                }
                else
                {
                    stringOffsetList.Add((int)bin.Position);

                    if (entryStringCheck == EmptyString)
                        bin.WriteStringUnicode(string.Empty, terminate: true);
                    else
                        bin.WriteStringUnicode(Entries[i].Value, terminate: true);

                }

                Entries[i].IsModified = false;
            }

            //At the very end of all the strings, place the file end padding:
            bin.Write((ushort)0); //PAD

            //Since we reached max length, might as well go fill in the file size:
            bin.Replace("FileSize", (int)bin.Length);

            bin.Goto("StringsBeginOffset");

            for (int i = 0; i < stringOffsetList.Count; i++)
            {
                bin.Write(stringOffsetList[i]);
            }

            bin.Position = bin.Length;
        }

        #region IList
        public int IndexOf(FMGEntryRef item)
        {
            return ((IList<FMGEntryRef>)Entries).IndexOf(item);
        }

        public void Insert(int index, FMGEntryRef item)
        {
            ((IList<FMGEntryRef>)Entries).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<FMGEntryRef>)Entries).RemoveAt(index);
        }

        public FMGEntryRef this[int index] { get => ((IList<FMGEntryRef>)Entries)[index]; set => ((IList<FMGEntryRef>)Entries)[index] = value; }

        public void Add(FMGEntryRef item)
        {
            ((IList<FMGEntryRef>)Entries).Add(item);
        }

        public void Clear()
        {
            ((IList<FMGEntryRef>)Entries).Clear();
        }

        public bool Contains(FMGEntryRef item)
        {
            return ((IList<FMGEntryRef>)Entries).Contains(item);
        }

        public void CopyTo(FMGEntryRef[] array, int arrayIndex)
        {
            ((IList<FMGEntryRef>)Entries).CopyTo(array, arrayIndex);
        }

        public bool Remove(FMGEntryRef item)
        {
            return ((IList<FMGEntryRef>)Entries).Remove(item);
        }

        public int Count => ((IList<FMGEntryRef>)Entries).Count;

        public bool IsReadOnly => ((IList<FMGEntryRef>)Entries).IsReadOnly;

        public IEnumerator<FMGEntryRef> GetEnumerator()
        {
            return ((IList<FMGEntryRef>)Entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<FMGEntryRef>)Entries).GetEnumerator();
        }
        #endregion
    }
}

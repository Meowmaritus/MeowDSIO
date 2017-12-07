using MeowDSIO.DataTypes.FMG;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MeowDSIO.DataFiles
{
    public class FMG : DataFile, IList<FMGChunk>
    {
        public FMGHeader Header { get; set; } = new FMGHeader();
        public ObservableCollection<FMGChunk> Chunks { get; set; } = new ObservableCollection<FMGChunk>();

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

            Chunks.Clear();
            FMGChunkHeaderBuffer chunk = new FMGChunkHeaderBuffer(stringOffsetsBegin);
            for (int i = 0; i < chunkCount; i++)
            {
                chunk.FirstStringIndex = bin.ReadInt32();
                chunk.FirstStringID = bin.ReadInt32();
                chunk.LastStringID = bin.ReadInt32();

                Chunks.Add(chunk.ReadChunk(bin));
            }
        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
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

            int stringsCount = 0;

            foreach (var c in Chunks)
            {
                foreach (var str in c)
                {
                    stringsCount++;
                }
            }

            bin.Write(stringsCount);

            bin.Placeholder("StringsBeginPointer");

            bin.Write(0); //PAD

            bin.Label("ChunksBeginOffset");

            //SKIP Chunks for now
            bin.Position += (Chunks.Count * 0x0C/*Length of chunk*/);

            bin.PointToHere("StringsBeginPointer");

            bin.Label("StringsBeginOffset");

            bin.Position += (stringsCount * 4);

            var stringOffsetList = new List<int>();
            var chunkStartIndexList = new List<int>();

            for (int i = 0; i < Chunks.Count; i++)
            {
                chunkStartIndexList.Add(stringOffsetList.Count/*Doubles as the string index!*/);

                foreach (var str in Chunks[i])
                {
                    if (str != null)
                    {
                        stringOffsetList.Add((int)bin.Position);
                        bin.WriteStringUnicode(str, terminate: true);
                    }
                    else
                    {
                        stringOffsetList.Add(0);
                    }
                }
            }

            //At the very end of all the strings, place the file end padding:
            bin.Write((ushort)0); //PAD

            //Since we reached max length, might as well go fill in the file size:
            bin.Replace("FileSize", (int)bin.Length);

            bin.Goto("ChunksBeginOffset");

            for (int i = 0; i < Chunks.Count; i++)
            {
                bin.Write(chunkStartIndexList[i]);
                bin.Write(Chunks[i].StartID);
                bin.Write(Chunks[i].StartID + (Chunks[i].Count - 1));
            }

            bin.Goto("StringsBeginOffset");

            for (int i = 0; i < stringOffsetList.Count; i++)
            {
                bin.Write(stringOffsetList[i]);
            }

            bin.Position = bin.Length;
        }

        #region IList

        public int IndexOf(FMGChunk item)
        {
            return ((IList<FMGChunk>)Chunks).IndexOf(item);
        }

        public void Insert(int index, FMGChunk item)
        {
            ((IList<FMGChunk>)Chunks).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<FMGChunk>)Chunks).RemoveAt(index);
        }

        public FMGChunk this[int index] { get => ((IList<FMGChunk>)Chunks)[index]; set => ((IList<FMGChunk>)Chunks)[index] = value; }

        public void Add(FMGChunk item)
        {
            ((IList<FMGChunk>)Chunks).Add(item);
        }

        public void Clear()
        {
            ((IList<FMGChunk>)Chunks).Clear();
        }

        public bool Contains(FMGChunk item)
        {
            return ((IList<FMGChunk>)Chunks).Contains(item);
        }

        public void CopyTo(FMGChunk[] array, int arrayIndex)
        {
            ((IList<FMGChunk>)Chunks).CopyTo(array, arrayIndex);
        }

        public bool Remove(FMGChunk item)
        {
            return ((IList<FMGChunk>)Chunks).Remove(item);
        }

        public int Count => ((IList<FMGChunk>)Chunks).Count;

        public bool IsReadOnly => ((IList<FMGChunk>)Chunks).IsReadOnly;

        public IEnumerator<FMGChunk> GetEnumerator()
        {
            return ((IList<FMGChunk>)Chunks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<FMGChunk>)Chunks).GetEnumerator();
        }

        #endregion
    }
}

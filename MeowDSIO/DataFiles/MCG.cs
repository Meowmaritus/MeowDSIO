using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataFiles
{
    public class McgUnkStructA
    {
        //public int UnkA { get; set; }
        public float UnkB  { get; set; }
        public float UnkC  { get; set; }
        public float UnkD  { get; set; }
        public List<int> IndicesA { get; set; } = new List<int>();
        public List<int> IndicesB { get; set; } = new List<int>();
        public int UnkE  { get; set; }
        public int UnkF  { get; set; }
    }

    public class McgUnkStructB
    {
        public int UnkA  { get; set; }
        //public int UnkB  { get; set; }
        public List<int> IndicesA { get; set; } = new List<int>();
        public int UnkC  { get; set; }
        //public int UnkD  { get; set; }
        public List<int> IndicesB  { get; set; } = new List<int>();
        public int UnkE  { get; set; }
        public byte UnkF { get; set; }
        public byte UnkG { get; set; }
        public byte UnkH { get; set; }
        public byte UnkI { get; set; }
        public float UnkJ  { get; set; }
    }

    public class MCG : DataFile
    {
        public List<McgUnkStructA> McgUnkStructA_List = new List<McgUnkStructA>();
        public List<McgUnkStructB> McgUnkStructB_List = new List<McgUnkStructB>();

        List<(long Offset, string VariableName)> DEBUG_IndexMap = 
            new List<(long Offset, string VariableName)>();

        private int ReadIndex(DSBinaryReader bin, string DEBUG_IndexMapStr)
        {
            int result = -1;
            var indexOffset = bin.ReadInt32();
            bin.StepIn(indexOffset);
            {
                result = bin.ReadInt32();
            }
            bin.StepOut();

            DEBUG_IndexMap.Add((Offset: indexOffset, VariableName: $"{DEBUG_IndexMapStr} = {result}"));

            return result;
        }

        private List<int> ReadIndices(DSBinaryReader bin, string DEBUG_IndexMapStr, int count)
        {
            List<int> result = new List<int>();
            var indexOffset = bin.ReadInt32();
            bin.StepIn(indexOffset);
            {
                for (int i = 0; i < count; i++)
                {
                    var newInt = bin.ReadInt32();
                    DEBUG_IndexMap.Add((Offset: (indexOffset + (i * 4)), VariableName: $"{DEBUG_IndexMapStr}[{i}] = {newInt}"));
                    result.Add(newInt);
                }
                
            }
            bin.StepOut();

            

            return result;
        }

        private McgUnkStructA ReadMcgUnkStructA(DSBinaryReader bin, int DEBUG_ArrayIndex)
        {
            var result = new McgUnkStructA();

            //result.UnkA = bin.ReadInt32();
            int indicesCount = bin.ReadInt32();
            result.UnkB = bin.ReadSingle();
            result.UnkC = bin.ReadSingle();
            result.UnkD = bin.ReadSingle();
            result.IndicesA = ReadIndices(bin, $"McgUnkStructA[{DEBUG_ArrayIndex}].IndicesA", indicesCount);
            result.IndicesB = ReadIndices(bin, $"McgUnkStructA[{DEBUG_ArrayIndex}].IndicesB", indicesCount);
            result.UnkE = bin.ReadInt32();
            result.UnkF = bin.ReadInt32();

            return result;
        }

        private McgUnkStructB ReadMcgUnkStructB(DSBinaryReader bin, int DEBUG_ArrayIndex)
        {
            var result = new McgUnkStructB();

            result.UnkA = bin.ReadInt32();
            int indicesA_Count = bin.ReadInt32();
            result.IndicesA = ReadIndices(bin, $"McgUnkStructB[{DEBUG_ArrayIndex}].IndicesA", indicesA_Count);
            result.UnkC = bin.ReadInt32();
            int indicesB_Count = bin.ReadInt32();
            result.IndicesB = ReadIndices(bin, $"McgUnkStructB[{DEBUG_ArrayIndex}].IndicesB", indicesB_Count);
            result.UnkE = bin.ReadInt32();
            result.UnkF = bin.ReadByte();
            result.UnkG = bin.ReadByte();
            result.UnkH = bin.ReadByte();
            result.UnkI = bin.ReadByte();
            result.UnkJ = bin.ReadSingle();

            return result;
        }

        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            DEBUG_IndexMap = new List<(long Offset, string VariableName)>();

            bin.AssertInt32(1);
            bin.AssertInt32(0);
            int McgUnkStructA_Count = bin.ReadInt32();
            int McgUnkStructA_Offset = bin.ReadInt32();
            int McgUnkStructB_Count = bin.ReadInt32();
            int McgUnkStructB_Offset = bin.ReadInt32();
            bin.AssertInt32(0);
            bin.AssertInt32(0);

            McgUnkStructA_List = new List<McgUnkStructA>();
            McgUnkStructB_List = new List<McgUnkStructB>();

            bin.StepIn(McgUnkStructA_Offset);
            {
                for (int i = 0; i < McgUnkStructA_Count; i++)
                {
                    McgUnkStructA_List.Add(ReadMcgUnkStructA(bin, i));
                }
            }
            bin.StepOut();

            bin.StepIn(McgUnkStructB_Offset);
            {
                for (int i = 0; i < McgUnkStructB_Count; i++)
                {
                    McgUnkStructB_List.Add(ReadMcgUnkStructB(bin, i));
                }
            }
            bin.StepOut();
        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            throw new NotImplementedException();
        }
    }
}

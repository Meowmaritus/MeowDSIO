using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataFiles
{
    public class ESD : DataFile
    {
        public class EzHeader
        {
            public int Unk0 = 1;
            public int Unk1 = 1;
            public int Unk2 = 1;
            public int Unk3 = 0x54;
            //int FileSize
            public int Unk4 = 6;
            public int Unk5 = 0x2C;
            public int Unk6 = 1;
            public int Unk7 = 0x10;
            public int Unk8 = 2;
            public int Unk9 = 0x24;
            //int NumStateEntry
            public int Unk11 = 0x1C;
            //int NumNextEntry
            public int Unk13 = 0x10;
            //int NumFuncEntry
            public int Unk15 = 8;
            //int NumFunctionParamEntry
            //int OffsetTransitionEntry
            //int NumTransitionEntry
            //int OffsetEndOfBS0
            //pad0
            //OffsetEndOfBS1
            //pad1
            //OffsetEndOfBS2
            //pad2
            public int Unk18 = 1;

            public const int UNK_BYTES_COUNT = 16;
            public byte[] UnkBytes;
        }

        public class EzFunctionParam
        {
            public const int SIZE = sizeof(int) * 2;

            public byte[] Params;
        }

        public class EzFunction
        {
            public const int SIZE = sizeof(int) * 4;

            public int Unk0 = 1;
            public int ID;
            public List<EzFunctionParam> Parameters = new List<EzFunctionParam>();
        }

        public class EzState
        {
            public const int SIZE = sizeof(int) * 9;

            public int Index;
            public List<EzSubState> SubstatesA = new List<EzSubState>();
            public List<EzFunction> FunctionsA = new List<EzFunction>();
            public List<EzFunction> FunctionsB = new List<EzFunction>();
            public List<EzSubState> SubstatesB = new List<EzSubState>();
        }

        public class EzSubState
        {
            public const int SIZE = sizeof(int) * 7;

            public EzState NextState;
            public int FunctionID;
            public int Unk1;
            public List<EzFunctionParam> Params = new List<EzFunctionParam>();
            public byte[] PackedData;
        }

        const int FILE_OFFSET = 0x6C;

        public class ESDLoader
        {
            private readonly DSBinaryReader bin;

            public ESDLoader(DSBinaryReader b)
            {
                bin = b;
            }

            Dictionary<int, EzFunction> DictEzFunction = new Dictionary<int, EzFunction>();
            Dictionary<int, EzFunctionParam> DictEzFunctionParam = new Dictionary<int, EzFunctionParam>();
            Dictionary<int, EzState> DictEzState = new Dictionary<int, EzState>();
            Dictionary<int, EzSubState> DictEzSubState = new Dictionary<int, EzSubState>();

            public EzFunctionParam GetEzFunctionParam(int offset)
            {
                if (DictEzFunctionParam.ContainsKey(offset))
                    return DictEzFunctionParam[offset];

                var esfp = new EzFunctionParam();

                DictEzFunctionParam.Add(offset, esfp);

                bin.StepIn(offset);
                {
                    int bytesOffset = bin.ReadInt32();
                    int bytesCount = bin.ReadInt32();

                    bin.StepIn(FILE_OFFSET + bytesOffset);
                    {
                        esfp.Params = bin.ReadBytes(bytesCount);
                    }
                    bin.StepOut();
                }
                bin.StepOut();

                return esfp;
            }

            public EzFunction GetEzFunction(int offset)
            {
                if (DictEzFunction.ContainsKey(offset))
                    return DictEzFunction[offset];

                var esf = new EzFunction();

                DictEzFunction.Add(offset, esf);

                bin.StepIn(offset);
                {
                    esf.Unk0 = bin.AssertInt32(1);
                    esf.ID = bin.ReadInt32();
                    int paramOffset = bin.ReadInt32();
                    int paramCount = bin.ReadInt32();

                    if (paramOffset != -1)
                    {
                        for (int i = 0; i < paramCount; i++)
                        {
                            esf.Parameters.Add(GetEzFunctionParam(FILE_OFFSET + paramOffset + EzFunctionParam.SIZE * i));
                        }
                    }
                }
                bin.StepOut();

                return esf;
            }

            public EzSubState GetEzSubState(int offset)
            {
                if (DictEzSubState.ContainsKey(offset))
                    return DictEzSubState[offset];

                var ess = new EzSubState();

                DictEzSubState.Add(offset, ess);

                bin.StepIn(offset);
                {
                    int nextStateOffset = bin.ReadInt32();

                    ess.FunctionID = bin.ReadInt32();
                    ess.Unk1 = bin.ReadInt32();

                    int funcParamOffset = bin.ReadInt32();
                    int funcParamCount = bin.ReadInt32();

                    int packedDataOffset = bin.ReadInt32();
                    int packedDataCount = bin.ReadInt32();

                    if (nextStateOffset != -1)
                    {
                        ess.NextState = GetEzState(FILE_OFFSET + nextStateOffset);
                    }

                    if (funcParamOffset != -1)
                    {
                        for (int i = 0; i < funcParamCount; i++)
                        {
                            ess.Params.Add(GetEzFunctionParam(FILE_OFFSET + funcParamOffset + i * EzFunctionParam.SIZE));
                        }
                    }

                    if (packedDataOffset != -1)
                    {
                        bin.StepIn(FILE_OFFSET + packedDataOffset);
                        {
                            ess.PackedData = bin.ReadBytes(packedDataCount);
                        }
                        bin.StepOut();
                    }
                }

                bin.StepOut();

                return ess;
            }

            public EzState GetEzState(int offset)
            {
                if (DictEzState.ContainsKey(offset))
                    return DictEzState[offset];

                var s = new EzState();

                DictEzState.Add(offset, s);

                bin.StepIn(offset);
                {
                    s.Index = bin.ReadInt32();

                    int transOffsetA = bin.ReadInt32();
                    int transCountA = bin.ReadInt32();

                    int funcOffsetA = bin.ReadInt32();
                    int funcCountA = bin.ReadInt32();

                    int funcOffsetB = bin.ReadInt32();
                    int funcCountB = bin.ReadInt32();

                    int transOffsetB = bin.ReadInt32();
                    int transCountB = bin.ReadInt32();

                    //bin.AssertInt32(-1);
                    //bin.AssertInt32(0);

                    if (transOffsetA != -1)
                    {
                        bin.StepIn(transOffsetA);
                        {
                            for (int i = 0; i < transCountA; i++)
                            {
                                int substateOffset = bin.ReadInt32();
                                bin.StepIn(FILE_OFFSET + substateOffset);
                                {
                                    s.SubstatesA.Add(GetEzSubState(FILE_OFFSET + substateOffset));
                                }
                                bin.StepOut();
                            }
                        }
                        bin.StepOut();
                    }

                    if (funcOffsetA != -1)
                    {
                        for (int i = 0; i < funcCountA; i++)
                        {
                            s.FunctionsA.Add(GetEzFunction(FILE_OFFSET + funcOffsetA + EzFunction.SIZE * i));
                        }
                    }

                    if (funcOffsetB != -1)
                    {
                        for (int i = 0; i < funcCountB; i++)
                        {
                            s.FunctionsB.Add(GetEzFunction(FILE_OFFSET + funcOffsetB + EzFunction.SIZE * i));
                        }
                    }

                    if (transOffsetB != -1)
                    {
                        bin.StepIn(transOffsetB);
                        {
                            for (int i = 0; i < transCountB; i++)
                            {
                                int substateOffset = bin.ReadInt32();
                                bin.StepIn(FILE_OFFSET + substateOffset);
                                {
                                    s.SubstatesB.Add(GetEzSubState(FILE_OFFSET + substateOffset));
                                }
                                bin.StepOut();
                            }
                        }
                        bin.StepOut();
                    }
                }
                bin.StepOut();

                return s;
            }
        }

        public const int MAGIC_DS1_PTDE = 0x4C535366; //fSSL

        public EzHeader Header { get; set; } = new EzHeader();

        public List<EzState> StatesA { get; set; } = new List<EzState>();
        public List<EzState> StatesB { get; set; } = new List<EzState>();

        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            bin.AssertInt32(MAGIC_DS1_PTDE);

            var loader = new ESDLoader(bin);

            Header = new EzHeader();
            Header.Unk0 = bin.AssertInt32(1);
            Header.Unk1 = bin.AssertInt32(1);
            Header.Unk2 = bin.AssertInt32(1);
            Header.Unk3 = bin.AssertInt32(0x54);
            int someEofOffsetShit = bin.ReadInt32();
            Header.Unk4 = bin.AssertInt32(6);
            Header.Unk5 = bin.AssertInt32(0x2C);
            Header.Unk6 = bin.AssertInt32(1);
            Header.Unk7 = bin.AssertInt32(0x10);
            Header.Unk8 = bin.AssertInt32(2);
            Header.Unk9 = bin.AssertInt32(0x24);
            int statesCount = bin.ReadInt32();
            Header.Unk11 = bin.AssertInt32(0x1C);
            int subStateCount = bin.ReadInt32();
            Header.Unk13 = bin.AssertInt32(0x10);
            int funcEntryCount = bin.ReadInt32();
            Header.Unk15 = bin.AssertInt32(8);
            int funcParamEntryCount = bin.ReadInt32();

            int transEntryOffset = bin.ReadInt32();
            int transEntryCount = bin.ReadInt32();

            //if (transEntryOffset != -1)
            //{
            //    for (int i = 0; i < transEntryCount; i++)
            //    {
            //        Transitions.Add(loader.GetEzTransition(transEntryOffset + EzTransition.SIZE * i));
            //    }
            //}

            int endOfBS0Offset = bin.ReadInt32();
            bin.AssertInt32(0);
            int endOfBS1Offset = bin.AssertInt32(endOfBS0Offset);
            bin.AssertInt32(0);
            int endOfBS2Offset = bin.AssertInt32(endOfBS0Offset);
            bin.AssertInt32(0);
            Header.Unk18 = bin.AssertInt32(1);

            //State Table Header

            Header.UnkBytes = bin.ReadBytes(EzHeader.UNK_BYTES_COUNT);

            int someOffsetA = bin.AssertInt32(0x2C);
            int someCountA = bin.ReadInt32();
            int someOffsetB = bin.AssertInt32(-1);
            int someCountB = bin.ReadInt32();

            bin.AssertRepeatedBytes(0, 12);

            int statesAOffset = bin.AssertInt32(0x4C);
            int statesACount = bin.ReadInt32();

            int statesAOffsetAGAIN = bin.AssertInt32(0x4C);
            int statesACountAGAIN = bin.ReadInt32();

            int statesBOffset = bin.ReadInt32();
            int statesBCount = bin.ReadInt32();

            int statesBOffsetAGAIN = bin.ReadInt32();

            int realStatesAOffset = (int)bin.Position;

            for (int i = 0; i < statesCount + 1; i++)
            {
                StatesA.Add(loader.GetEzState(realStatesAOffset + EzState.SIZE * i));
            }

            int realStatesBOffset = (int)bin.Position;

            for (int i = 0; i < statesBCount + 1; i++)
            {
                StatesB.Add(loader.GetEzState(realStatesBOffset + EzState.SIZE * i));
            }
        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            throw new NotImplementedException();
        }
    }
}

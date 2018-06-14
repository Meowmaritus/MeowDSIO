//using MeowDSIO.DataTypes.MSB;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MeowDSIO.DataFiles
//{
//    public enum MsbSectorFormat
//    {
//        NONE,
//        MODEL_PARAM_ST,
//        EVENT_PARAM_ST,
//        POINT_PARAM_ST,
//        PARTS_PARAM_ST,
//    }

//    public class MSB : DataFile
//    {
//        public int Unknown1;

//        public List<MsbParamEntry> Models;
//        public List<MsbParamEntry> Events;
//        public List<MsbParamEntry> Points;
//        public List<MsbParamEntry> Parts;



//        /*
//int Unknown1;

//int MODEL_PARAM_NameOffset;
//int MODEL_PARAM_Count;
//int[] MODEL_PARAM_Pointers[MODEL_PARAM_Count];
//int startOfNextSectionOffset;
//int end = 0;

//int EVENT_PARAM_NameOffset;
//int EVENT_PARAM_Count;
//int[] EVENT_PARAM_Pointers[EVENT_PARAM_Count];

//int POINT_PARAM_NameOffset;
//int POINT_PARAM_Count;
//int[] POINT_PARAM_Pointers[POINT_PARAM_Count];

//int PARTS_PARAM_NameOffset;
//int PARTS_PARAM_Count;
//int[] PARTS_PARAM_Pointers[PARTS_PARAM_Count];

//            each struct thing:

//            int nameOffset;

//            int otherStringOffsetEtc;

//            <data>

//            string name (nameOffset points here)

//            string otherStringEtc (otherStringOffsetEtc points here etc)


//         */

//        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
//        {
//            Unknown1 = bin.ReadInt32();

//            Models = new List<MsbParamEntry>();
//            Events = new List<MsbParamEntry>();
//            Points = new List<MsbParamEntry>();
//            Parts = new List<MsbParamEntry>();

//            MsbSectorFormat currentSectorFormat = MsbSectorFormat.NONE;

//            do
//            {
//                int currentSectorNameOffset = bin.ReadInt32();

//                if (currentSectorNameOffset == 0)
//                    break;

//                bin.StepIn(currentSectorNameOffset);
//                {
//                    currentSectorFormat = (MsbSectorFormat)Enum.Parse(typeof(MsbSectorFormat), bin.ReadStringAscii());
//                }
//                bin.StepOut();

//                int structCount = bin.ReadInt32();

//                for (int i = 0; i < structCount; i++)
//                {
//                    int structOffset = bin.ReadInt32();

//                    bin.StepIn(structOffset);
//                    {
//                        MsbParamEntry entry_struct = new MsbParamEntry();
//                        switch (currentSectorFormat)
//                        {
//                            case MsbSectorFormat.MODEL_PARAM_ST:
//                                entry_struct.ParamType = MsbParamType.MODEL_PARAM_ST;
//                                entry_struct.ReadFromBinary(bin);
//                                Models.Add(entry_struct);
//                                break;
//                            case MsbSectorFormat.EVENT_PARAM_ST:
//                                entry_struct.ParamType = MsbParamType.EVENT_PARAM_ST;
//                                entry_struct.ReadFromBinary(bin);
//                                Events.Add(entry_struct);
//                                break;
//                            case MsbSectorFormat.POINT_PARAM_ST:
//                                entry_struct.ParamType = MsbParamType.POINT_PARAM_ST;
//                                entry_struct.ReadFromBinary(bin);
//                                Points.Add(entry_struct);
//                                break;
//                            case MsbSectorFormat.PARTS_PARAM_ST:
//                                entry_struct.ParamType = MsbParamType.PARTS_PARAM_ST;
//                                entry_struct.ReadFromBinary(bin);
//                                Parts.Add(entry_struct);
//                                break;
//                        }
//                    }
//                    bin.StepOut();
//                }

//                int sectionEndOffset = bin.ReadInt32();

//                if (sectionEndOffset == 0)
//                {
//                    //LAST SECTION YEET
//                    break;
//                }
//                else
//                {
//                    bin.Position = sectionEndOffset + 4;
//                }

                

//            }
//            while (true); //Maybe double check here so it doesnt keep reading on dumb files


//        }

//        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
//        {
//            bin.Write(Unknown1);


//            bin.Placeholder("MODEL_PARAM_ST");
//            bin.Write(Models.Count);
//            for (int i = 0; i < Models.Count; i++)
//            {
//                bin.Placeholder($"MODEL_PARAM_ST_{i}");
//            }
//            bin.Placeholder("MODEL_PARAM_ST_END");
//            bin.Write((int)0);

//            bin.Replace("MODEL_PARAM_ST", (int)bin.Position);
//            bin.WriteStringAscii("MODEL_PARAM_ST", terminate: true);
//            bin.Pad(align: 0x4);
//            for (int i = 0; i < Models.Count; i++)
//            {
//                bin.Replace($"MODEL_PARAM_ST_{i}", (int)bin.Position);
//                MsbModelParam.Write(bin, Models[i], i);
//            }
//            bin.Replace("MODEL_PARAM_ST_END", (int)bin.Position);
//            bin.Write((int)0);





//            bin.Placeholder("EVENT_PARAM_ST");
//            bin.Write(Events.Count);
//            for (int i = 0; i < Events.Count; i++)
//            {
//                bin.Placeholder($"EVENT_PARAM_ST_{i}");
//            }
//            bin.Placeholder("EVENT_PARAM_ST_END");
//            bin.Write((int)0);

//            bin.Replace("EVENT_PARAM_ST", (int)bin.Position);
//            bin.WriteStringAscii("EVENT_PARAM_ST", terminate: true);
//            bin.Pad(align: 0x4);
//            for (int i = 0; i < Events.Count; i++)
//            {
//                bin.Replace($"EVENT_PARAM_ST_{i}", (int)bin.Position);
//                MsbEventParam.Write(bin, Events[i], i);
//            }
//            bin.Replace("EVENT_PARAM_ST_END", (int)bin.Position);
//            bin.Write((int)0);






//            bin.Placeholder("POINT_PARAM_ST");
//            bin.Write(Points.Count);
//            for (int i = 0; i < Points.Count; i++)
//            {
//                bin.Placeholder($"POINT_PARAM_ST_{i}");
//            }
//            bin.Placeholder("POINT_PARAM_ST_END");
//            bin.Write((int)0);

//            bin.Replace("POINT_PARAM_ST", (int)bin.Position);
//            bin.WriteStringAscii("POINT_PARAM_ST", terminate: true);
//            bin.Pad(align: 0x4);
//            for (int i = 0; i < Points.Count; i++)
//            {
//                bin.Replace($"POINT_PARAM_ST_{i}", (int)bin.Position);
//                MsbPointParam.Write(bin, Points[i], i);
//            }
//            bin.Replace("POINT_PARAM_ST_END", (int)bin.Position);
//            bin.Write((int)0);



//            bin.Placeholder("PARTS_PARAM_ST");
//            bin.Write(Parts.Count);
//            for (int i = 0; i < Parts.Count; i++)
//            {
//                bin.Placeholder($"PARTS_PARAM_ST_{i}");
//            }
//            //NO END MARKER BECAUSE LAST SECTION
//            bin.Write((int)0);

//            bin.Replace("PARTS_PARAM_ST", (int)bin.Position);
//            bin.WriteStringAscii("PARTS_PARAM_ST", terminate: true);
//            bin.Pad(align: 0x4);
//            for (int i = 0; i < Parts.Count; i++)
//            {
//                bin.Replace($"PARTS_PARAM_ST_{i}", (int)bin.Position);
//                MsbPartsParam.Write(bin, Parts[i], i);
//            }
//            bin.Write((int)0);
//        }
//    }
//}

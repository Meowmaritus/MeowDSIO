using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae300_DoCommandFancy : TimeActEventBase
    {
        public Tae300_DoCommandFancy(float StartTime, float EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public override IList<object> Parameters
        {
            get => new List<object>
            {
                CommandType,
                UNK2,
                UNK3,
                Parameter,
                SomeID,
            };
        }

        public TaeGeneralCommandType CommandType { get; set; } = 0;
        public short UNK2 { get; set; } = 1;
        public int UNK3 { get; set; } = 0;
        public float Parameter { get; set; } = 1;
        public int SomeID { get; set; } = -1;

        public override void ReadParameters(DSBinaryReader bin)
        {
            CommandType = (TaeGeneralCommandType)bin.ReadInt16();
            UNK2 = bin.ReadInt16();
            UNK3 = bin.ReadInt32();
            Parameter = bin.ReadSingle();
            SomeID = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write((short)CommandType);
            bin.Write(UNK2);
            bin.Write(UNK3);
            bin.Write(Parameter);
            bin.Write(SomeID);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.DoCommandFancy;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae000_DoCommand : TimeActEventBase
    {
        public Tae000_DoCommand(float StartTime, float EndTime)
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
            };
        }

        public TaeGeneralCommandType CommandType { get; set; } = 0;
        public float UNK2 { get; set; } = 0;
        public int UNK3 { get; set; } = -1;

        public override void ReadParameters(DSBinaryReader bin)
        {
            CommandType = (TaeGeneralCommandType)bin.ReadInt32();
            UNK2 = bin.ReadSingle();
            UNK3 = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write((int)CommandType);
            bin.Write(UNK2);
            bin.Write(UNK3);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.DoCommand;
        }
    }
}

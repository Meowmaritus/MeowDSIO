using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae000 : TimeActEventBase
    {
        public int AnimCancelType { get; set; } = 0;
        public float UNK1 { get; set; } = 0;
        public int UNK2 { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            AnimCancelType = bin.ReadInt32();
            UNK1 = bin.ReadSingle();
            UNK2 = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(AnimCancelType);
            bin.Write(UNK1);
            bin.Write(UNK2);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.Type0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae300 : TimeActEventBase
    {
        public override IList<object> Parameters
        {
            get => new List<object>
            {
                UNK1,
                UNK2,
                UNK3,
                UNK4,
                UNK5,
            };
            set
            {
                UNK1 = (short)value[0];
                UNK2 = (short)value[1];
                UNK3 = (int)value[2];
                UNK4 = (float)value[3];
                UNK5 = (int)value[4];
            }
        }

        public short UNK1 { get; set; } = 0;
        public short UNK2 { get; set; } = 0;
        public int UNK3 { get; set; } = 0;
        public float UNK4 { get; set; } = 0;
        public int UNK5 { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            UNK1 = bin.ReadInt16();
            UNK2 = bin.ReadInt16();
            UNK3 = bin.ReadInt32();
            UNK4 = bin.ReadSingle();
            UNK5 = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(UNK1);
            bin.Write(UNK2);
            bin.Write(UNK3);
            bin.Write(UNK4);
            bin.Write(UNK5);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.Type300;
        }
    }
}

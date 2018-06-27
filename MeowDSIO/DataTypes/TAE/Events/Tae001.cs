using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae001 : TimeActEventBase
    {
        public int HitType { get; set; } = 0;
        public int UNK2 { get; set; } = 0;
        public int DmgLevel { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            HitType = bin.ReadInt32();
            UNK2 = bin.ReadInt32();
            DmgLevel = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(HitType);
            bin.Write(UNK2);
            bin.Write(DmgLevel);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.Type1;
        }
    }
}

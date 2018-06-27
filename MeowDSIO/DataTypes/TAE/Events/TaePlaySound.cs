using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class TaePlaySound : TimeActEventBase
    {
        public MSB.MsbSoundType SoundType { get; set; } = 0;
        public int SoundID { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            SoundType = (MSB.MsbSoundType)bin.ReadInt32();
            SoundID = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write((int)SoundType);
            bin.Write(SoundID);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.PlaySound;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae144_CameraShakeSpecific : TimeActEventBase
    {
        public Tae144_CameraShakeSpecific(float StartTime, float EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public override IList<object> Parameters
        {
            get => new List<object>
            {
                RumbleCamID,
                UNK1,
                IntensityA,
                IntensityB,
            };
        }

        public short RumbleCamID { get; set; } = 0;
        public short UNK1 { get; set; } = 0;
        public float IntensityA { get; set; } = 0;
        public float IntensityB { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            RumbleCamID = bin.ReadInt16();
            UNK1 = bin.ReadInt16();
            IntensityA = bin.ReadSingle();
            IntensityB = bin.ReadSingle();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(RumbleCamID);
            bin.Write(UNK1);
            bin.Write(IntensityA);
            bin.Write(IntensityB);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.CameraShakeSpecific;
        }
    }
}

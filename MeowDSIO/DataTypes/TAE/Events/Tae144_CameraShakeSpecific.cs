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
                SomeID,
                IntensityA,
                IntensityB,
            };
        }

        public int SomeID { get; set; } = 0;
        public float IntensityA { get; set; } = 0;
        public float IntensityB { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            SomeID = bin.ReadInt32();
            IntensityA = bin.ReadSingle();
            IntensityB = bin.ReadSingle();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(SomeID);
            bin.Write(IntensityA);
            bin.Write(IntensityB);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.CameraShakeSpecific;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae145_CameraShakeGeneric : TimeActEventBase
    {
        public Tae145_CameraShakeGeneric(float StartTime, float EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public override IList<object> Parameters
        {
            get => new List<object>
            {
                Intensity,
            };
        }

        public float Intensity { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            Intensity = bin.ReadSingle();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(Intensity);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.CameraShakeGeneric;
        }
    }
}

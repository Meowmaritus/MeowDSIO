using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.POINT_PARAM_ST
{
    public class MsbRegionSphere : MsbRegionBase
    {
        public float Radius { get; set; } = 1;
        public int EventEntityID { get; set; } = -1;

        public override (int, int, int) GetOffsetDeltas()
        {
            return (4, 8, 12);
        }

        public override PointParamSubtype GetSubtypeValue()
        {
            return PointParamSubtype.Spheres;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            Radius = bin.ReadSingle();
            EventEntityID = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(Radius);
            bin.Write(EventEntityID);
        }
    }
}

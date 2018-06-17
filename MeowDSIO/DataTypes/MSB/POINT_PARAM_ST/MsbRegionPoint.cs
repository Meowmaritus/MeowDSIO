using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.POINT_PARAM_ST
{
    public class MsbRegionPoint : MsbRegionBase
    {
        public int EventEntityID { get; set; } = -1;

        public override (int, int, int) GetOffsetDeltas()
        {
            return (4, -1, 8);
        }

        public override PointParamSubtype GetSubtypeValue()
        {
            return PointParamSubtype.Points;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            EventEntityID = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(EventEntityID);
        }
    }
}

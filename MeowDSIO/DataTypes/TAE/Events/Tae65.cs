using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae65 : TimeActEventBase
    {


        public override void ReadParameters(DSBinaryReader bin)
        {

        }

        public override void WriteParameters(DSBinaryWriter bin)
        {

        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.Type65;
        }
    }
}

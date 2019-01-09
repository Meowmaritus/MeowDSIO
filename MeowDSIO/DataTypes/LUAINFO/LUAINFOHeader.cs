using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.LUAINFO
{
    public class LUAINFOHeader
    {
        public const int UnknownA_Length = 4;
        //Seems to always be 01 00 00 00
        public byte[] UnknownA = new byte[] { 1, 0, 0, 0 };


        public const int UnknownB_Length = 4;
        //Seems to always be 00 00 00 00
        public byte[] UnknownB = new byte[] { 0, 0, 0, 0 };
    }
}

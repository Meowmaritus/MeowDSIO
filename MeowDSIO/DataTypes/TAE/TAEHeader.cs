using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    public class TAEHeader
    {
        //"TAE "
        [JsonConverter(typeof(Json.ByteArrayConverter))]
        public byte[] Signature { get; set; } = { 0x54, 0x41, 0x45, 0x20 };

        //////////////////////
        // Data Block Start //
        //////////////////////
        public uint UnknownA00 { get; set; } = 0x00000040u;
        public uint UnknownA01 { get; set; } = 0x00000001u;
        public uint UnknownA02 { get; set; } = 0x00000050u;
        public uint UnknownA03 { get; set; } = 0x00000070u;
        public uint UnknownA04 { get; set; } = 0x00010002u;
        public uint UnknownA05 { get; set; } = 0x00000000u;
        public uint UnknownA06 { get; set; } = 0x00000000u;
        public uint UnknownA07 { get; set; } = 0x00000000u;
        public uint UnknownA08 { get; set; } = 0x00000000u;
        public uint UnknownA09 { get; set; } = 0x00000000u;
        public uint UnknownA10 { get; set; } = 0x00000000u;
        public uint UnknownA11 { get; set; } = 0x00000000u;
        public uint UnknownA12 { get; set; } = 0x02010001u;
        public uint UnknownA13 { get; set; } = 0x01010002u;
        public uint UnknownA14 { get; set; } = 0x00000001u;
        public uint UnknownA15 { get; set; } = 0x00000000u;
        //////////////////////
        //  Data Block End  //
        //////////////////////

        //Value samples below taken from Artorias (c4100.tae)
        public int UnknownB { get; set; } = 0;
        public int UnknownC { get; set; } = 65547;
        public int UnknownD { get; set; } = 0x00000090;

        public uint UnknownE00 = 0x00000000u;
        public uint UnknownE01 = 0x00000001u;
        public uint UnknownE02 = 0x00000080u;
        public uint UnknownE03 = 0x00000000u;
        public uint UnknownE04 = 0x00000000u;
        public uint UnknownE05 = 0x00031D44u;
        public uint UnknownE06 = 0x00031D44u;
        public uint UnknownE07 = 0x00000050u;
        public uint UnknownE08 = 0x00000000u;
        public uint UnknownE09 = 0x00000000u;

        public int ID { get; set; } = 204100;
    }
}

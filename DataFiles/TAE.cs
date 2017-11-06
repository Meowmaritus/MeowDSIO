using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeowDSIO.DataTypes.TAE;

namespace MeowDSIO.DataFiles
{
    public class TAE : DataFile
    {
        public class TAEHeader
        {
            //"TAE "
            public byte[] Signature = { 0x54, 0x41, 0x45, 0x20 };

            //Sample taken from Artorias (c4100.tae)
            public byte[] MagicBytes =
            { 
                0x40, 0x00, 0x00, 0x00, 
                0x01, 0x00, 0x00, 0x00,
                0x50, 0x00, 0x00, 0x00, 
                0x70, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x01, 0x00, 0x01, 0x02, 
                0x02, 0x00, 0x01, 0x01, 
                0x01, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00 
            };

            //Value samples below taken from Artorias (c4100.tae)

            public int Unk1 = 0;
            public int Unk2 = 65547;
            public byte[] Unk3 = { 0x90, 0, 0, 0 };
            public byte[] Unk4 = 
            {
                0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00,
                0x80, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x44, 0x1D, 0x03, 0x00,
                0x44, 0x1D, 0x03, 0x00, 
                0x50, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            public int ID = 204100;
        }

        public TAEHeader Header;
        public string SkeletonName;
        public string SibName;
        public Dictionary<int, Animation> Animations;
        public List<AnimationGroup> AnimationGroups;

        protected override void Read(BinaryReader bin)
        {
            throw new NotImplementedException();
        }

        protected override void Write(BinaryWriter bin)
        {
            throw new NotImplementedException();
        }
    }
}

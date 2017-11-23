﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.BND3
{
    public struct BND3EntryHeaderBuffer
    {
        public int FileSize;
        public int FileOffset;
        public int FileID;
        public int FileNameOffset;
        public int? Unknown1;

        public void Reset()
        {
            FileSize = -1;
            FileOffset = -1;
            FileID = -1;
            FileNameOffset = -1;
            Unknown1 = null;
        }

        public BND3Entry GetEntry(DSBinaryReader bin)
        {
            bin.StepIn(FileOffset);
            var bytes = bin.ReadBytes(FileSize);
            bin.StepOut();

            string fileName = null;

            if (FileNameOffset > -1)
            {
                bin.StepIn(FileNameOffset);
                {
                    fileName = bin.ReadStringShiftJIS();
                }
                bin.StepOut();
            }

            return new BND3Entry(FileID, fileName, Unknown1, ref bytes);
        }
    }
}
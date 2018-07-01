﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae008 : TimeActEventBase
    {
        public override IList<object> Parameters
        {
            get => new List<object>
            {
                UNK1,
                UNK2,
                UNK3,
                UNK4,
                UNK5,
                UNK6,
                UNK7,
                UNK8,
                UNK9,
                UNK10,
                UNK11,
                UNK12,
            };
            set
            {
                UNK1 = (float)value[0];
                UNK2 = (float)value[1];
                UNK3 = (float)value[2];
                UNK4 = (float)value[3];
                UNK5 = (float)value[4];
                UNK6 = (float)value[5];
                UNK7 = (float)value[6];
                UNK8 = (float)value[7];
                UNK9 = (float)value[8];
                UNK10 = (float)value[9];
                UNK11 = (float)value[10];
                UNK12 = (float)value[11];
            }
        }

        public float UNK1 { get; set; } = 0;
        public float UNK2 { get; set; } = 0;
        public float UNK3 { get; set; } = 0;
        public float UNK4 { get; set; } = 0;
        public float UNK5 { get; set; } = 0;
        public float UNK6 { get; set; } = 0;
        public float UNK7 { get; set; } = 0;
        public float UNK8 { get; set; } = 0;
        public float UNK9 { get; set; } = 0;
        public float UNK10 { get; set; } = 0;
        public float UNK11 { get; set; } = 0;
        public float UNK12 { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            UNK1 = bin.ReadSingle();
            UNK2 = bin.ReadSingle();
            UNK3 = bin.ReadSingle();
            UNK4 = bin.ReadSingle();
            UNK5 = bin.ReadSingle();
            UNK6 = bin.ReadSingle();
            UNK7 = bin.ReadSingle();
            UNK8 = bin.ReadSingle();
            UNK9 = bin.ReadSingle();
            UNK10 = bin.ReadSingle();
            UNK11 = bin.ReadSingle();
            UNK12 = bin.ReadSingle();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(UNK1);
            bin.Write(UNK2);
            bin.Write(UNK3);
            bin.Write(UNK4);
            bin.Write(UNK5);
            bin.Write(UNK6);
            bin.Write(UNK7);
            bin.Write(UNK8);
            bin.Write(UNK9);
            bin.Write(UNK10);
            bin.Write(UNK11);
            bin.Write(UNK12);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.Type8;
        }
    }
}

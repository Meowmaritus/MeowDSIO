﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae033 : TimeActEventBase
    {
        public override IList<object> Parameters
        {
            get => new List<object>
            {
                UNK1,
            };
            set
            {
                UNK1 = (int)value[0];
            }
        }

        public int UNK1 { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            UNK1 = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(UNK1);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.Type33;
        }
    }
}

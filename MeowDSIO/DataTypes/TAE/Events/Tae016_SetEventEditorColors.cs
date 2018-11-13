using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae016_SetEventEditorColors : TimeActEventBase
    {
        public Tae016_SetEventEditorColors(float StartTime, float EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public override IList<object> Parameters
        {
            get => new List<object>
            {
                EventKind,
                ColorA,
                ColorAFlag,
                ColorB,
                ColorBFlag,
                ColorC,
                ColorCFlag,
            };
        }

        public TimeActEventType EventKind { get; set; } = 0;

        public Color ColorA { get; set; }
        public byte ColorAFlag { get; set; } = 0;

        public Color ColorB { get; set; } = Color.Black;
        public byte ColorBFlag { get; set; } = 0;

        public Color ColorC { get; set; } = Color.Black;
        public byte ColorCFlag { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            EventKind = (TimeActEventType)bin.ReadInt32();

            byte c1r = bin.ReadByte();
            byte c1g = bin.ReadByte();
            byte c1b = bin.ReadByte();
            ColorA = new Color(c1r, c1g, c1b, (byte)255);
            ColorAFlag = bin.ReadByte();

            byte c2r = bin.ReadByte();
            byte c2g = bin.ReadByte();
            byte c2b = bin.ReadByte();
            ColorB = new Color(c2r, c2g, c2b, (byte)255);
            ColorBFlag = bin.ReadByte();

            byte c3r = bin.ReadByte();
            byte c3g = bin.ReadByte();
            byte c3b = bin.ReadByte();
            ColorC = new Color(c3r, c3g, c3b, (byte)255);
            ColorCFlag = bin.ReadByte();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write((int)EventKind);

            bin.Write(ColorA.R);
            bin.Write(ColorA.G);
            bin.Write(ColorA.B);
            bin.Write(ColorAFlag);

            bin.Write(ColorB.R);
            bin.Write(ColorB.G);
            bin.Write(ColorB.B);
            bin.Write(ColorBFlag);

            bin.Write(ColorC.R);
            bin.Write(ColorC.G);
            bin.Write(ColorC.B);
            bin.Write(ColorCFlag);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.SetEventEditorColors;
        }
    }
}

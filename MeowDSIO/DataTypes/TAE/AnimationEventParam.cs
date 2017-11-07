using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AnimationEventParam
    {
        [FieldOffset(0)]
        private float floatVal;

        [FieldOffset(0)]
        private int intVal;

        public static implicit operator AnimationEventParam(int a) => new AnimationEventParam() { Int = a };
        public static implicit operator AnimationEventParam(float a) => new AnimationEventParam() { Float = a };
        public static implicit operator int(AnimationEventParam a) => a.Int;
        public static implicit operator float(AnimationEventParam a) => a.Float;

        public float Float
        {
            get => floatVal;
            set => floatVal = value;
        }

        public int Int
        {
            get => intVal;
            set => intVal = value;
        }
    }
}

using System;
using System.Collections.Generic;
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
        public float Float;

        [FieldOffset(0)]
        public int Int;

        public static implicit operator AnimationEventParam(int a) => new AnimationEventParam() { Int = a };
        public static implicit operator AnimationEventParam(float a) => new AnimationEventParam() { Float = a };
        public static implicit operator int(AnimationEventParam a) => a.Int;
        public static implicit operator float(AnimationEventParam a) => a.Float;
    }
}

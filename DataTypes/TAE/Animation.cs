using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    public class Animation : Data
    {
        public string FileName = "a00_0000.hkxwin";
        public List<AnimationEvent> Events = new List<AnimationEvent>();
    }
}

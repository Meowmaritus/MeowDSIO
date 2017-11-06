using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    public enum AnimationEventType
    {
        Type0   = 0, 
        Type1   = 1, 
        Type2   = 2, 
        Type5   = 5, 
        Type16  = 16,
        Type32  = 32,
        Type33  = 33,
        Type66  = 66,
        Type96  = 96,
        Type100 = 100,
        Type101 = 101,
        Type109 = 109,
        Type112 = 112,
        Type118 = 118,
        Type128 = 128,
        Type129 = 129,
        Type130 = 130,
        Type144 = 144,
        Type193 = 193,
        Type224 = 224,
        Type225 = 225,
        Type226 = 226,
        Type228 = 228,
        Type229 = 229,
        Type303 = 303,
        Type306 = 306,
        Type401 = 401,
    }

    public class AnimationEvent : Data
    {
        public float StartTime = 0;
        public float EndTime = 0;

        private AnimationEventType _type;
        public AnimationEventType Type => _type;

        //Haven't decided if I want to include this.
        //Mainly because like...why would you ever call this method?
        //public void ChangeEventType(AnimationEventType newEventType)
        //{
        //    _type = newEventType;
        //    Array.Resize(ref _parameters, GetParamCount(newEventType));
        //}

        private AnimationEventParam[] _parameters;
        public AnimationEventParam[] Parameters { get => _parameters; private set => _parameters = value; }

        public AnimationEvent(AnimationEventType type)
        {
            _type = type;
            Parameters = new AnimationEventParam[GetParamCount(type)];
        }

        private static int GetParamCount(AnimationEventType type)
        {
            switch (type)
            {
                case AnimationEventType.Type0: return 3;
                case AnimationEventType.Type1: return 3;
                case AnimationEventType.Type2: return 4;
                case AnimationEventType.Type5: return 2;
                case AnimationEventType.Type16: return 4;
                case AnimationEventType.Type32: return 1;
                case AnimationEventType.Type33: return 1;
                case AnimationEventType.Type66: return 1;
                case AnimationEventType.Type96: return 3;
                case AnimationEventType.Type100: return 3;
                case AnimationEventType.Type101: return 1; 
                case AnimationEventType.Type109: return 3;
                case AnimationEventType.Type112: return 2;
                case AnimationEventType.Type118: return 3;
                case AnimationEventType.Type128: return 2;
                case AnimationEventType.Type129: return 4;
                case AnimationEventType.Type130: return 4;
                case AnimationEventType.Type144: return 3;
                case AnimationEventType.Type193: return 2;
                case AnimationEventType.Type224: return 1;
                case AnimationEventType.Type225: return 1;
                case AnimationEventType.Type226: return 1;
                case AnimationEventType.Type228: return 3;
                case AnimationEventType.Type229: return 1;
                case AnimationEventType.Type303: return 1;
                case AnimationEventType.Type306: return 3;
                case AnimationEventType.Type401: return 1;
            }
            throw new Exception($"{nameof(AnimationEventType)}.{type} does not have a param data " + 
                $"array length specified in {nameof(AnimationEvent)}.{nameof(GetParamCount)}().");

        }
    }
}

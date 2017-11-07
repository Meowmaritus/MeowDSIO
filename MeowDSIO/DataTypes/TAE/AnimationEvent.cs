using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    public enum AnimationEventType
    {
        PlayAnimation = 0, 
        Hitbox   = 1, 
        Type2   = 2, 
        Type5   = 5,
        Type8 = 8,
        Type16  = 16,
        //Type20 = 20,
        Type24 = 24,
        Type32  = 32,
        Type33  = 33,
        Type64 = 64,
        Type65 = 65,
        Type66  = 66,
        SpEffect = 67,
        FootParticles  = 96,
        Particles = 100,
        Type101 = 101,
        Type104 = 104,
        ParticlesB = 109,
        Type110 = 110,
        ParticlesLarge = 112,
        Type114 = 114,
        Type115 = 115,
        Type116 = 116,
        WeaponTrail = 118,
        Type119 = 119,
        Type120 = 120,
        Type121 = 121,
        BodySoundEffect = 128,
        WeaponSoundEffect = 129,
        Rotate = 130,
        ScreenShake = 144,
        ScreenShakeB = 145,
        Invisibility = 193,
        Type224 = 224,
        Type225 = 225,
        Type226 = 226,
        Ragdoll = 228,
        Type229 = 229,
        Type231 = 231,
        Type232 = 232,
        Type236 = 236,
        Type300 = 300,
        Type301 = 301,
        Type302 = 302,
        AnimationStack = 303,
        Type304 = 304,
        Type306 = 306,
        PlayerWeaponHitbox = 307,
        Type308 = 308,
        Type401 = 401,
    }

    public class AnimationEvent : Data
    {
        public float StartTime { get; set; } = 0;
        public float EndTime { get; set; } = 0;
        private AnimationEventType _type;
        [JsonProperty(Order = -2)]
        public AnimationEventType Type
        {
            get => _type;
            set
            {
                ChangeEventType(value);
            }
        }

        public override string ToString()
        {
            return $"{nameof(AnimationEventType)}.{Type}({string.Join(", ", Parameters)})";
        }

        public void ChangeEventType(AnimationEventType newEventType)
        {
            _type = newEventType;
            Array.Resize(ref _parameters, GetParamCount(newEventType, -1));
        }

        private string[] _parameters;

        [JsonProperty(Order = -2)]
        [JsonConverter(typeof(Json.StringArrayConverter))]
        public string[] Parameters { get => _parameters; private set => _parameters = value; }

        public static bool CheckIfParamIsUnlikelyToBeFloat(AnimationEventParam paramVal)
        {
            return (float.IsInfinity(paramVal.Float) 
                || float.IsNaN(paramVal.Float)
                || paramVal.Float > 1000000000 
                || paramVal.Float < 0.00001);
        }

        public AnimationEvent(AnimationEventType type, int animID_ForDebug)
        {
            _type = type;
            Parameters = new string[GetParamCount(type, animID_ForDebug)];
        }

        private static int GetParamCount(AnimationEventType type, int animID_ForDebug)
        {
            switch (type)
            {
                case AnimationEventType.PlayAnimation: return 3;
                case AnimationEventType.Hitbox: return 3;
                case AnimationEventType.Type2: return 4;
                case AnimationEventType.Type5: return 2;
                case AnimationEventType.Type8: return 14;
                case AnimationEventType.Type16: return 4;
                //case AnimationEventType.Type20: return 6;
                //case AnimationEventType.Type21: return 6;
                case AnimationEventType.Type24: return 6;
                case AnimationEventType.Type32: return 1;
                case AnimationEventType.Type33: return 1;
                case AnimationEventType.Type64: return 4;
                case AnimationEventType.Type65: return 2;
                case AnimationEventType.Type66: return 1;
                case AnimationEventType.SpEffect: return 3;
                case AnimationEventType.FootParticles: return 3;
                //case AnimationEventType.Type99: return 5;
                case AnimationEventType.Particles: return 3;
                case AnimationEventType.Type101: return 1;
                //case AnimationEventType.Type102: return 6;
                case AnimationEventType.Type104: return 5;
                //case AnimationEventType.Type108: return 6;
                case AnimationEventType.ParticlesB: return 3;
                case AnimationEventType.Type110: return 3;
                case AnimationEventType.ParticlesLarge: return 2;
                case AnimationEventType.Type114: return 5;
                case AnimationEventType.Type115: return 5;
                case AnimationEventType.Type116: return 4;
                case AnimationEventType.WeaponTrail: return 3;
                case AnimationEventType.Type119: return 5;
                case AnimationEventType.Type120: return 8;
                case AnimationEventType.Type121: return 4;
                case AnimationEventType.BodySoundEffect: return 2;
                case AnimationEventType.WeaponSoundEffect: return 4;
                case AnimationEventType.Rotate: return 4;
                case AnimationEventType.ScreenShake: return 3;
                case AnimationEventType.ScreenShakeB: return 3;
                case AnimationEventType.Invisibility: return 2;
                case AnimationEventType.Type224: return 1;
                case AnimationEventType.Type225: return 1;
                case AnimationEventType.Type226: return 1;
                case AnimationEventType.Ragdoll: return 3;
                case AnimationEventType.Type231: return 1;
                case AnimationEventType.Type232: return 3;
                case AnimationEventType.Type236: return 5;
                case AnimationEventType.Type229: return 1;
                case AnimationEventType.Type300: return 2;
                case AnimationEventType.Type301: return 3;
                case AnimationEventType.Type302: return 3;
                case AnimationEventType.AnimationStack: return 1;
                case AnimationEventType.Type304: return 4;
                case AnimationEventType.Type306: return 3;
                case AnimationEventType.PlayerWeaponHitbox: return 5;
                case AnimationEventType.Type308: return 1;
                case AnimationEventType.Type401: return 1;
            }
            Console.Error.WriteLine($"[ANIM {animID_ForDebug}] Animation Event Type {type} does not have a param data " + 
                $"array length specified in {nameof(AnimationEvent)}.{nameof(GetParamCount)}().");
            return 1;

        }
    }
}

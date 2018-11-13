using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE.Events
{
    public class Tae001_DoBehaviorAtk : TimeActEventBase
    {
        public Tae001_DoBehaviorAtk(float StartTime, float EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public override IList<object> Parameters
        {
            get => new List<object>
            {
                HitType,
                AttackIndex,
                BehaviorJudgeID,
            };
        }

        public int HitType { get; set; } = 0;
        public int AttackIndex { get; set; } = 0;
        public int BehaviorJudgeID { get; set; } = 0;

        public override void ReadParameters(DSBinaryReader bin)
        {
            HitType = bin.ReadInt32();
            AttackIndex = bin.ReadInt32();
            BehaviorJudgeID = bin.ReadInt32();
        }

        public override void WriteParameters(DSBinaryWriter bin)
        {
            bin.Write(HitType);
            bin.Write(AttackIndex);
            bin.Write(BehaviorJudgeID);
        }

        protected override TimeActEventType GetEventType()
        {
            return TimeActEventType.DoBehaviorAtk;
        }
    }
}

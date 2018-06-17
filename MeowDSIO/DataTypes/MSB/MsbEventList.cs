using MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB
{
    public class MsbEventList
    {
        public List<MsbEventBlackEyeOrbInvasion> BlackEyeOrbInvasion { get; set; } 
            = new List<MsbEventBlackEyeOrbInvasion>();
        public List<MsbEventBloodMsg> BloodMessages { get; set; }
            = new List<MsbEventBloodMsg>();
        public List<MsbEventEnvironment> EnvironmentEvents { get; set; }
            = new List<MsbEventEnvironment>();
        public List<MsbEventGenerator> Generators { get; set; }
            = new List<MsbEventGenerator>();
        public List<MsbEventLight> Lights { get; set; }
            = new List<MsbEventLight>();
        public List<MsbEventMapOffset> MapOffsets { get; set; }
            = new List<MsbEventMapOffset>();
        public List<MsbEventNavimesh> Navimeshes { get; set; }
            = new List<MsbEventNavimesh>();
        public List<MsbEventObjAct> ObjActs { get; set; }
            = new List<MsbEventObjAct>();
        public List<MsbEventSFX> SFXs { get; set; }
            = new List<MsbEventSFX>();
        public List<MsbEventSound> Sounds { get; set; }
            = new List<MsbEventSound>();
        public List<MsbEventSpawnPoint> SpawnPoints { get; set; }
            = new List<MsbEventSpawnPoint>();
        public List<MsbEventTreasure> Treasures { get; set; }
            = new List<MsbEventTreasure>();
        public List<MsbEventWindSFX> WindSFXs { get; set; }
            = new List<MsbEventWindSFX>();

        private void CheckIndexDictRegister(Dictionary<int, MsbEventBase> indexDict, MsbEventBase thing)
        {
            if (indexDict.ContainsKey(thing.EventIndex))
                throw new InvalidDataException($"Two events found with {nameof(thing.EventIndex)} == {thing.EventIndex} in this MSB!");
            else
                indexDict.Add(thing.EventIndex, thing);
        }

        public List<MsbEventBase> GetAllEventsInOrder()
        {
            Dictionary<int, MsbEventBase> indexDict = new Dictionary<int, MsbEventBase>();

            foreach (var thing in BlackEyeOrbInvasion)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in BloodMessages)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in EnvironmentEvents)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Generators)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Lights)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in MapOffsets)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Navimeshes)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in ObjActs)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in SFXs)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Sounds)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in SpawnPoints)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Treasures)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in WindSFXs)
                CheckIndexDictRegister(indexDict, thing);

            //int currentIndex = -1;

            //foreach (var kvp in indexDict)
            //{
            //    if (kvp.Key != (currentIndex + 1))
            //    {
            //        throw new InvalidDataException($"MSB Event list {nameof(MsbEventBase.EventIndex)} value skips from {currentIndex} to {kvp.Key}!");
            //    }
            //}

            return indexDict.Values.ToList();
        }
    }
}

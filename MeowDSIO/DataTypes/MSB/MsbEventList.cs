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

        public IList<MsbEventBase> GlobalList => Lights.Cast<MsbEventBase>()
            .Concat(Sounds)
            .Concat(SFXs)
            .Concat(WindSFXs)
            .Concat(Treasures)
            .Concat(Generators)
            .Concat(BloodMessages)
            .Concat(ObjActs)
            .Concat(SpawnPoints)
            .Concat(MapOffsets)
            .Concat(Navimeshes)
            .Concat(EnvironmentEvents)
            .Concat(BlackEyeOrbInvasion)
            .ToList();

        public string NameOf(int index)
        {
            if (index == -1)
            {
                return "";
            }
            return GlobalList[index].Name;
        }

        public int IndexOf(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return -1;
            }
            var matches = GlobalList.Where(x => x.Name == name);
            var matchCount = matches.Count();
            if (matchCount == 0)
            {
                throw new Exception($"MSB Event \"{name}\" does not exist!");
            }
            else if (matchCount > 1)
            {
                throw new Exception($"More than one MSB Event found named \"{name}\"!");
            }
            return GlobalList.IndexOf(matches.First());
        }
    }
}

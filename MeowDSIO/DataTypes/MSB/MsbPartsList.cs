using MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB
{
    public class MsbPartsList
    {
        public List<MsbPartsCollision> Collisions { get; set; }
            = new List<MsbPartsCollision>();

        public List<MsbPartsCollisionUnused> UnusedCollisions { get; set; }
            = new List<MsbPartsCollisionUnused>();

        public List<MsbPartsMapPiece> MapPieces { get; set; }
            = new List<MsbPartsMapPiece>();

        public List<MsbPartsNavimesh> Navimeshes { get; set; }
            = new List<MsbPartsNavimesh>();

        public List<MsbPartsNPC> NPCs { get; set; }
            = new List<MsbPartsNPC>();

        public List<MsbPartsNPCUnused> UnusedNPCs { get; set; }
            = new List<MsbPartsNPCUnused>();

        public List<MsbPartsObject> Objects { get; set; }
            = new List<MsbPartsObject>();

        public List<MsbPartsObjectUnused> UnusedObjects { get; set; }
            = new List<MsbPartsObjectUnused>();

        public List<MsbPartsPlayer> Players { get; set; }
            = new List<MsbPartsPlayer>();

        private void CheckIndexDictRegister(List<MsbPartsBase> indexDict, MsbPartsBase thing)
        {
            indexDict.Add(thing);
        }

        public IList<MsbPartsBase> GlobalList => MapPieces.Cast<MsbPartsBase>()
            .Concat(Objects)
            .Concat(NPCs)
            .Concat(Players)
            .Concat(Collisions)
            .Concat(Navimeshes)
            .Concat(UnusedObjects)
            .Concat(UnusedNPCs)
            .Concat(UnusedCollisions)
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
                throw new Exception($"MSB Part \"{name}\" does not exist!");
            }
            else if (matchCount > 1)
            {
                throw new Exception($"More than one MSB Part found named \"{name}\"!");
            }
            return GlobalList.IndexOf(matches.First());
        }
    }
}

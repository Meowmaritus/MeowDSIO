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

        public List<MsbPartsBase> GetAllPartsInOrder()
        {
            List<MsbPartsBase> indexDict = new List<MsbPartsBase>();

            foreach (var thing in MapPieces)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Objects)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in NPCs)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Players)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Collisions)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Navimeshes)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in UnusedObjects)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in UnusedNPCs)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in UnusedCollisions)
                CheckIndexDictRegister(indexDict, thing);

            //int currentIndex = -1;

            //foreach (var kvp in indexDict)
            //{
            //    if (kvp.Key != (currentIndex + 1))
            //    {
            //        throw new InvalidDataException($"MSB Parts list {nameof(MsbPartsBase.Index)} value skips from {currentIndex} to {kvp.Key}!");
            //    }
            //}

            return indexDict;
        }
    }
}

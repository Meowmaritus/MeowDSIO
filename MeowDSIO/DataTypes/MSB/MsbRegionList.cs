using MeowDSIO.DataTypes.MSB.POINT_PARAM_ST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB
{
    public class MsbRegionList
    {
        public List<MsbRegionPoint> Points { get; set; }
        = new List<MsbRegionPoint>();

        public List<MsbRegionSphere> Spheres { get; set; }
        = new List<MsbRegionSphere>();

        public List<MsbRegionCylinder> Cylinders { get; set; }
        = new List<MsbRegionCylinder>();

        public List<MsbRegionBox> Boxes { get; set; }
        = new List<MsbRegionBox>();

        private void CheckIndexDictRegister(Dictionary<int, MsbRegionBase> indexDict, MsbRegionBase thing)
        {
            if (indexDict.ContainsKey(thing.Index))
                throw new InvalidDataException($"Two regions found with {nameof(thing.Index)} == {thing.Index} in this MSB!");
            else
                indexDict.Add(thing.Index, thing);
        }

        public List<MsbRegionBase> GetAllRegionsInOrder()
        {
            Dictionary<int, MsbRegionBase> indexDict = new Dictionary<int, MsbRegionBase>();

            foreach (var thing in Points)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Spheres)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Cylinders)
                CheckIndexDictRegister(indexDict, thing);

            foreach (var thing in Boxes)
                CheckIndexDictRegister(indexDict, thing);

            //int currentIndex = -1;

            //foreach (var kvp in indexDict)
            //{
            //    if (kvp.Key != (currentIndex + 1))
            //    {
            //        throw new InvalidDataException($"MSB Region list {nameof(MsbRegionBase.Index)} value skips from {currentIndex} to {kvp.Key}!");
            //    }
            //}

            return indexDict.Values.ToList();
        }
    }
}

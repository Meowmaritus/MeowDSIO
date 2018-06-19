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

        public string NameOf(int index)
        {
            if (index == -1)
            {
                return "";
            }
            return GlobalList[index].Name;
        }

        public IList<MsbRegionBase> GlobalList => Points.Cast<MsbRegionBase>()
            .Concat(Spheres)
            .Concat(Cylinders)
            .Concat(Boxes)
            .ToList();

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
                throw new Exception($"MSB Region \"{name}\" does not exist!");
            }
            else if (matchCount > 1)
            {
                throw new Exception($"More than one MSB Region found named \"{name}\"!");
            }
            return GlobalList.IndexOf(matches.First());
        }
    }
}

using MeowDSIO.DataTypes.TAE.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    public class TimeActEventList : IList<TimeActEventBase>
    {
        public List<Tae0> Events0 { get; set; } = new List<Tae0>();
        public List<Tae1> Events1 { get; set; } = new List<Tae1>();
        public List<Tae2> Events2 { get; set; } = new List<Tae2>();
        public List<Tae5> Events5 { get; set; } = new List<Tae5>();
        public List<Tae8> Events8 { get; set; } = new List<Tae8>();
        public List<Tae16> Events16 { get; set; } = new List<Tae16>();
        public List<Tae20> Events20 { get; set; } = new List<Tae20>();
        public List<Tae21> Events21 { get; set; } = new List<Tae21>();
        public List<Tae24> Events24 { get; set; } = new List<Tae24>();
        public List<Tae32> Events32 { get; set; } = new List<Tae32>();
        public List<Tae33> Events33 { get; set; } = new List<Tae33>();
        public List<Tae64> Events64 { get; set; } = new List<Tae64>();
        public List<Tae65> Events65 { get; set; } = new List<Tae65>();
        public List<Tae66> Events66 { get; set; } = new List<Tae66>();
        public List<Tae67> Events67 { get; set; } = new List<Tae67>();
        public List<Tae96> Events96 { get; set; } = new List<Tae96>();
        public List<Tae99> Events99 { get; set; } = new List<Tae99>();
        public List<Tae100> Events100 { get; set; } = new List<Tae100>();
        public List<Tae101> Events101 { get; set; } = new List<Tae101>();
        public List<Tae102> Events102 { get; set; } = new List<Tae102>();
        public List<Tae104> Events104 { get; set; } = new List<Tae104>();
        public List<Tae108> Events108 { get; set; } = new List<Tae108>();
        public List<Tae109> Events109 { get; set; } = new List<Tae109>();
        public List<Tae110> Events110 { get; set; } = new List<Tae110>();
        public List<Tae112> Events112 { get; set; } = new List<Tae112>();
        public List<Tae114> Events114 { get; set; } = new List<Tae114>();
        public List<Tae115> Events115 { get; set; } = new List<Tae115>();
        public List<Tae116> Events116 { get; set; } = new List<Tae116>();
        public List<Tae118> Events118 { get; set; } = new List<Tae118>();
        public List<Tae119> Events119 { get; set; } = new List<Tae119>();
        public List<Tae120> Events120 { get; set; } = new List<Tae120>();
        public List<Tae121> Events121 { get; set; } = new List<Tae121>();
        public List<Tae128> Events128 { get; set; } = new List<Tae128>();
        public List<Tae129> Events129 { get; set; } = new List<Tae129>();
        public List<Tae130> Events130 { get; set; } = new List<Tae130>();
        public List<Tae144> Events144 { get; set; } = new List<Tae144>();
        public List<Tae145> Events145 { get; set; } = new List<Tae145>();
        public List<Tae160> Events160 { get; set; } = new List<Tae160>();
        public List<Tae161> Events161 { get; set; } = new List<Tae161>();
        public List<Tae162> Events162 { get; set; } = new List<Tae162>();
        public List<Tae176> Events176 { get; set; } = new List<Tae176>();
        public List<Tae180> Events180 { get; set; } = new List<Tae180>();
        public List<Tae181> Events181 { get; set; } = new List<Tae181>();
        public List<Tae182> Events182 { get; set; } = new List<Tae182>();
        public List<Tae193> Events193 { get; set; } = new List<Tae193>();
        public List<Tae196> Events196 { get; set; } = new List<Tae196>();
        public List<Tae224> Events224 { get; set; } = new List<Tae224>();
        public List<Tae225> Events225 { get; set; } = new List<Tae225>();
        public List<Tae226> Events226 { get; set; } = new List<Tae226>();
        public List<Tae228> Events228 { get; set; } = new List<Tae228>();
        public List<Tae229> Events229 { get; set; } = new List<Tae229>();
        public List<Tae231> Events231 { get; set; } = new List<Tae231>();
        public List<Tae232> Events232 { get; set; } = new List<Tae232>();
        public List<Tae233> Events233 { get; set; } = new List<Tae233>();
        public List<Tae236> Events236 { get; set; } = new List<Tae236>();
        public List<Tae300> Events300 { get; set; } = new List<Tae300>();
        public List<Tae301> Events301 { get; set; } = new List<Tae301>();
        public List<Tae302> Events302 { get; set; } = new List<Tae302>();
        public List<Tae303> Events303 { get; set; } = new List<Tae303>();
        public List<Tae304> Events304 { get; set; } = new List<Tae304>();
        public List<Tae306> Events306 { get; set; } = new List<Tae306>();
        public List<Tae307> Events307 { get; set; } = new List<Tae307>();
        public List<Tae308> Events308 { get; set; } = new List<Tae308>();
        public List<Tae401> Events401 { get; set; } = new List<Tae401>();
        public List<Tae500> Events500 { get; set; } = new List<Tae500>();

        public IList<TimeActEventBase> GlobalList => 
            Events0.Cast<TimeActEventBase>()
            .Concat(Events1)
            .Concat(Events2)
            .Concat(Events5)
            .Concat(Events8)
            .Concat(Events16)
            .Concat(Events20)
            .Concat(Events21)
            .Concat(Events24)
            .Concat(Events32)
            .Concat(Events33)
            .Concat(Events64)
            .Concat(Events65)
            .Concat(Events66)
            .Concat(Events67)
            .Concat(Events96)
            .Concat(Events99)
            .Concat(Events100)
            .Concat(Events101)
            .Concat(Events102)
            .Concat(Events104)
            .Concat(Events108)
            .Concat(Events109)
            .Concat(Events110)
            .Concat(Events112)
            .Concat(Events114)
            .Concat(Events115)
            .Concat(Events116)
            .Concat(Events118)
            .Concat(Events119)
            .Concat(Events120)
            .Concat(Events121)
            .Concat(Events128)
            .Concat(Events129)
            .Concat(Events130)
            .Concat(Events144)
            .Concat(Events145)
            .Concat(Events160)
            .Concat(Events161)
            .Concat(Events162)
            .Concat(Events176)
            .Concat(Events180)
            .Concat(Events181)
            .Concat(Events182)
            .Concat(Events193)
            .Concat(Events196)
            .Concat(Events224)
            .Concat(Events225)
            .Concat(Events226)
            .Concat(Events228)
            .Concat(Events229)
            .Concat(Events231)
            .Concat(Events232)
            .Concat(Events233)
            .Concat(Events236)
            .Concat(Events300)
            .Concat(Events301)
            .Concat(Events302)
            .Concat(Events303)
            .Concat(Events304)
            .Concat(Events306)
            .Concat(Events307)
            .Concat(Events308)
            .Concat(Events401)
            .Concat(Events500)
            .ToList();

        public TimeActEventBase this[int index] { get => GlobalList[index]; set => GlobalList[index] = value; }

        public int Count => GlobalList.Count;

        public bool IsReadOnly => GlobalList.IsReadOnly;

        public void Add(TimeActEventBase item)
        {
            GlobalList.Add(item);
        }

        public void Clear()
        {
            GlobalList.Clear();
        }

        public bool Contains(TimeActEventBase item)
        {
            return GlobalList.Contains(item);
        }

        public void CopyTo(TimeActEventBase[] array, int arrayIndex)
        {
            GlobalList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TimeActEventBase> GetEnumerator()
        {
            return GlobalList.GetEnumerator();
        }

        public int IndexOf(TimeActEventBase item)
        {
            return GlobalList.IndexOf(item);
        }

        public void Insert(int index, TimeActEventBase item)
        {
            GlobalList.Insert(index, item);
        }

        public bool Remove(TimeActEventBase item)
        {
            return GlobalList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            GlobalList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GlobalList.GetEnumerator();
        }
    }
}

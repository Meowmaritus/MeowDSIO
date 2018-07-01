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
        public List<Tae000> Events000 { get; set; } = new List<Tae000>();
        public List<Tae001> Events001 { get; set; } = new List<Tae001>();
        public List<Tae002> Events002 { get; set; } = new List<Tae002>();
        public List<Tae005> Events005 { get; set; } = new List<Tae005>();
        public List<Tae008> Events008 { get; set; } = new List<Tae008>();
        public List<Tae016> Events016 { get; set; } = new List<Tae016>();
        public List<Tae024> Events024 { get; set; } = new List<Tae024>();
        public List<Tae032> Events032 { get; set; } = new List<Tae032>();
        public List<Tae033> Events033 { get; set; } = new List<Tae033>();
        public List<Tae064> Events064 { get; set; } = new List<Tae064>();
        public List<Tae065> Events065 { get; set; } = new List<Tae065>();
        public List<Tae066> Events066 { get; set; } = new List<Tae066>();
        public List<Tae067> Events067 { get; set; } = new List<Tae067>();
        public List<Tae096> Events096 { get; set; } = new List<Tae096>();
        public List<Tae099> Events099 { get; set; } = new List<Tae099>();
        public List<Tae100> Events100 { get; set; } = new List<Tae100>();
        public List<Tae101> Events101 { get; set; } = new List<Tae101>();
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
        public List<Tae193> Events193 { get; set; } = new List<Tae193>();
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

        private IList<TimeActEventBase> GlobalList => 
            Events000.Cast<TimeActEventBase>()
            .Concat(Events001)
            .Concat(Events002)
            .Concat(Events005)
            .Concat(Events008)
            .Concat(Events016)
            .Concat(Events024)
            .Concat(Events032)
            .Concat(Events033)
            .Concat(Events064)
            .Concat(Events065)
            .Concat(Events066)
            .Concat(Events067)
            .Concat(Events096)
            .Concat(Events099)
            .Concat(Events100)
            .Concat(Events101)
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
            .Concat(Events193)
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
            .OrderBy(x => x.Index)
            .ToList();

        public TimeActEventBase this[int index] { get => GlobalList[index]; set => GlobalList[index] = value; }

        public int Count => GlobalList.Count;

        public bool IsReadOnly => GlobalList.IsReadOnly;

        public void Add(TimeActEventBase item)
        {
            var gl = GlobalList;
            if (gl.Any())
                item.Index = gl.Last().Index + 1;
            else
                item.Index = 0;

            switch (item.EventType)
            {
                case TimeActEventType.Type000: Events000.Add((Tae000)item); break;
                case TimeActEventType.Type001: Events001.Add((Tae001)item); break;
                case TimeActEventType.Type002: Events002.Add((Tae002)item); break;
                case TimeActEventType.Type005: Events005.Add((Tae005)item); break;
                case TimeActEventType.Type008: Events008.Add((Tae008)item); break;
                case TimeActEventType.Type016: Events016.Add((Tae016)item); break;
                case TimeActEventType.Type024: Events024.Add((Tae024)item); break;
                case TimeActEventType.Type032: Events032.Add((Tae032)item); break;
                case TimeActEventType.Type033: Events033.Add((Tae033)item); break;
                case TimeActEventType.Type064: Events064.Add((Tae064)item); break;
                case TimeActEventType.Type065: Events065.Add((Tae065)item); break;
                case TimeActEventType.Type066: Events066.Add((Tae066)item); break;
                case TimeActEventType.Type067: Events067.Add((Tae067)item); break;
                case TimeActEventType.Type096: Events096.Add((Tae096)item); break;
                case TimeActEventType.Type099: Events099.Add((Tae099)item); break;
                case TimeActEventType.Type100: Events100.Add((Tae100)item); break;
                case TimeActEventType.Type101: Events101.Add((Tae101)item); break;
                case TimeActEventType.Type104: Events104.Add((Tae104)item); break;
                case TimeActEventType.Type108: Events108.Add((Tae108)item); break;
                case TimeActEventType.Type109: Events109.Add((Tae109)item); break;
                case TimeActEventType.Type110: Events110.Add((Tae110)item); break;
                case TimeActEventType.Type112: Events112.Add((Tae112)item); break;
                case TimeActEventType.Type114: Events114.Add((Tae114)item); break;
                case TimeActEventType.Type115: Events115.Add((Tae115)item); break;
                case TimeActEventType.Type116: Events116.Add((Tae116)item); break;
                case TimeActEventType.Type118: Events118.Add((Tae118)item); break;
                case TimeActEventType.Type119: Events119.Add((Tae119)item); break;
                case TimeActEventType.Type120: Events120.Add((Tae120)item); break;
                case TimeActEventType.Type121: Events121.Add((Tae121)item); break;
                case TimeActEventType.Type128: Events128.Add((Tae128)item); break;
                case TimeActEventType.Type129: Events129.Add((Tae129)item); break;
                case TimeActEventType.Type130: Events130.Add((Tae130)item); break;
                case TimeActEventType.Type144: Events144.Add((Tae144)item); break;
                case TimeActEventType.Type145: Events145.Add((Tae145)item); break;
                case TimeActEventType.Type193: Events193.Add((Tae193)item); break;
                case TimeActEventType.Type224: Events224.Add((Tae224)item); break;
                case TimeActEventType.Type225: Events225.Add((Tae225)item); break;
                case TimeActEventType.Type226: Events226.Add((Tae226)item); break;
                case TimeActEventType.Type228: Events228.Add((Tae228)item); break;
                case TimeActEventType.Type229: Events229.Add((Tae229)item); break;
                case TimeActEventType.Type231: Events231.Add((Tae231)item); break;
                case TimeActEventType.Type232: Events232.Add((Tae232)item); break;
                case TimeActEventType.Type233: Events233.Add((Tae233)item); break;
                case TimeActEventType.Type236: Events236.Add((Tae236)item); break;
                case TimeActEventType.Type300: Events300.Add((Tae300)item); break;
                case TimeActEventType.Type301: Events301.Add((Tae301)item); break;
                case TimeActEventType.Type302: Events302.Add((Tae302)item); break;
                case TimeActEventType.Type303: Events303.Add((Tae303)item); break;
                case TimeActEventType.Type304: Events304.Add((Tae304)item); break;
                case TimeActEventType.Type306: Events306.Add((Tae306)item); break;
                case TimeActEventType.Type307: Events307.Add((Tae307)item); break;
                case TimeActEventType.Type308: Events308.Add((Tae308)item); break;
                case TimeActEventType.Type401: Events401.Add((Tae401)item); break;
                case TimeActEventType.Type500: Events500.Add((Tae500)item); break;
            }
        }

        public void Clear()
        {
            Events000.Clear();
            Events001.Clear();
            Events002.Clear();
            Events005.Clear();
            Events008.Clear();
            Events016.Clear();
            Events024.Clear();
            Events032.Clear();
            Events033.Clear();
            Events064.Clear();
            Events065.Clear();
            Events066.Clear();
            Events067.Clear();
            Events096.Clear();
            Events099.Clear();
            Events100.Clear();
            Events101.Clear();
            Events104.Clear();
            Events108.Clear();
            Events109.Clear();
            Events110.Clear();
            Events112.Clear();
            Events114.Clear();
            Events115.Clear();
            Events116.Clear();
            Events118.Clear();
            Events119.Clear();
            Events120.Clear();
            Events121.Clear();
            Events128.Clear();
            Events129.Clear();
            Events130.Clear();
            Events144.Clear();
            Events145.Clear();
            Events193.Clear();
            Events224.Clear();
            Events225.Clear();
            Events226.Clear();
            Events228.Clear();
            Events229.Clear();
            Events231.Clear();
            Events232.Clear();
            Events233.Clear();
            Events236.Clear();
            Events300.Clear();
            Events301.Clear();
            Events302.Clear();
            Events303.Clear();
            Events304.Clear();
            Events306.Clear();
            Events307.Clear();
            Events308.Clear();
            Events401.Clear();
            Events500.Clear();
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
            var gl = GlobalList;
            for (int i = index; i < gl.Count; i++)
            {
                gl[i].Index++;
            }
            item.Index = index;

            switch (item.EventType)
            {
                case TimeActEventType.Type000: Events000.Add((Tae000)item); break;
                case TimeActEventType.Type001: Events001.Add((Tae001)item); break;
                case TimeActEventType.Type002: Events002.Add((Tae002)item); break;
                case TimeActEventType.Type005: Events005.Add((Tae005)item); break;
                case TimeActEventType.Type008: Events008.Add((Tae008)item); break;
                case TimeActEventType.Type016: Events016.Add((Tae016)item); break;
                case TimeActEventType.Type024: Events024.Add((Tae024)item); break;
                case TimeActEventType.Type032: Events032.Add((Tae032)item); break;
                case TimeActEventType.Type033: Events033.Add((Tae033)item); break;
                case TimeActEventType.Type064: Events064.Add((Tae064)item); break;
                case TimeActEventType.Type065: Events065.Add((Tae065)item); break;
                case TimeActEventType.Type066: Events066.Add((Tae066)item); break;
                case TimeActEventType.Type067: Events067.Add((Tae067)item); break;
                case TimeActEventType.Type096: Events096.Add((Tae096)item); break;
                case TimeActEventType.Type099: Events099.Add((Tae099)item); break;
                case TimeActEventType.Type100: Events100.Add((Tae100)item); break;
                case TimeActEventType.Type101: Events101.Add((Tae101)item); break;
                case TimeActEventType.Type104: Events104.Add((Tae104)item); break;
                case TimeActEventType.Type108: Events108.Add((Tae108)item); break;
                case TimeActEventType.Type109: Events109.Add((Tae109)item); break;
                case TimeActEventType.Type110: Events110.Add((Tae110)item); break;
                case TimeActEventType.Type112: Events112.Add((Tae112)item); break;
                case TimeActEventType.Type114: Events114.Add((Tae114)item); break;
                case TimeActEventType.Type115: Events115.Add((Tae115)item); break;
                case TimeActEventType.Type116: Events116.Add((Tae116)item); break;
                case TimeActEventType.Type118: Events118.Add((Tae118)item); break;
                case TimeActEventType.Type119: Events119.Add((Tae119)item); break;
                case TimeActEventType.Type120: Events120.Add((Tae120)item); break;
                case TimeActEventType.Type121: Events121.Add((Tae121)item); break;
                case TimeActEventType.Type128: Events128.Add((Tae128)item); break;
                case TimeActEventType.Type129: Events129.Add((Tae129)item); break;
                case TimeActEventType.Type130: Events130.Add((Tae130)item); break;
                case TimeActEventType.Type144: Events144.Add((Tae144)item); break;
                case TimeActEventType.Type145: Events145.Add((Tae145)item); break;
                case TimeActEventType.Type193: Events193.Add((Tae193)item); break;
                case TimeActEventType.Type224: Events224.Add((Tae224)item); break;
                case TimeActEventType.Type225: Events225.Add((Tae225)item); break;
                case TimeActEventType.Type226: Events226.Add((Tae226)item); break;
                case TimeActEventType.Type228: Events228.Add((Tae228)item); break;
                case TimeActEventType.Type229: Events229.Add((Tae229)item); break;
                case TimeActEventType.Type231: Events231.Add((Tae231)item); break;
                case TimeActEventType.Type232: Events232.Add((Tae232)item); break;
                case TimeActEventType.Type233: Events233.Add((Tae233)item); break;
                case TimeActEventType.Type236: Events236.Add((Tae236)item); break;
                case TimeActEventType.Type300: Events300.Add((Tae300)item); break;
                case TimeActEventType.Type301: Events301.Add((Tae301)item); break;
                case TimeActEventType.Type302: Events302.Add((Tae302)item); break;
                case TimeActEventType.Type303: Events303.Add((Tae303)item); break;
                case TimeActEventType.Type304: Events304.Add((Tae304)item); break;
                case TimeActEventType.Type306: Events306.Add((Tae306)item); break;
                case TimeActEventType.Type307: Events307.Add((Tae307)item); break;
                case TimeActEventType.Type308: Events308.Add((Tae308)item); break;
                case TimeActEventType.Type401: Events401.Add((Tae401)item); break;
                case TimeActEventType.Type500: Events500.Add((Tae500)item); break;
            }
        }

        public bool Remove(TimeActEventBase item)
        {
            var gl = GlobalList;

            var indexOfItem = gl.IndexOf(item);

            if (indexOfItem < 0)
            {
                return false;
            }

            for (int i = indexOfItem; i < gl.Count; i++)
            {
                gl[i].Index--;
            }

            switch (item.EventType)
            {
                case TimeActEventType.Type000: return Events000.Remove((Tae000)item);
                case TimeActEventType.Type001: return Events001.Remove((Tae001)item);
                case TimeActEventType.Type002: return Events002.Remove((Tae002)item);
                case TimeActEventType.Type005: return Events005.Remove((Tae005)item);
                case TimeActEventType.Type008: return Events008.Remove((Tae008)item);
                case TimeActEventType.Type016: return Events016.Remove((Tae016)item);
                case TimeActEventType.Type024: return Events024.Remove((Tae024)item);
                case TimeActEventType.Type032: return Events032.Remove((Tae032)item);
                case TimeActEventType.Type033: return Events033.Remove((Tae033)item);
                case TimeActEventType.Type064: return Events064.Remove((Tae064)item);
                case TimeActEventType.Type065: return Events065.Remove((Tae065)item);
                case TimeActEventType.Type066: return Events066.Remove((Tae066)item);
                case TimeActEventType.Type067: return Events067.Remove((Tae067)item);
                case TimeActEventType.Type096: return Events096.Remove((Tae096)item);
                case TimeActEventType.Type099: return Events099.Remove((Tae099)item);
                case TimeActEventType.Type100: return Events100.Remove((Tae100)item);
                case TimeActEventType.Type101: return Events101.Remove((Tae101)item);
                case TimeActEventType.Type104: return Events104.Remove((Tae104)item);
                case TimeActEventType.Type108: return Events108.Remove((Tae108)item);
                case TimeActEventType.Type109: return Events109.Remove((Tae109)item);
                case TimeActEventType.Type110: return Events110.Remove((Tae110)item);
                case TimeActEventType.Type112: return Events112.Remove((Tae112)item);
                case TimeActEventType.Type114: return Events114.Remove((Tae114)item);
                case TimeActEventType.Type115: return Events115.Remove((Tae115)item);
                case TimeActEventType.Type116: return Events116.Remove((Tae116)item);
                case TimeActEventType.Type118: return Events118.Remove((Tae118)item);
                case TimeActEventType.Type119: return Events119.Remove((Tae119)item);
                case TimeActEventType.Type120: return Events120.Remove((Tae120)item);
                case TimeActEventType.Type121: return Events121.Remove((Tae121)item);
                case TimeActEventType.Type128: return Events128.Remove((Tae128)item);
                case TimeActEventType.Type129: return Events129.Remove((Tae129)item);
                case TimeActEventType.Type130: return Events130.Remove((Tae130)item);
                case TimeActEventType.Type144: return Events144.Remove((Tae144)item);
                case TimeActEventType.Type145: return Events145.Remove((Tae145)item);
                case TimeActEventType.Type193: return Events193.Remove((Tae193)item);
                case TimeActEventType.Type224: return Events224.Remove((Tae224)item);
                case TimeActEventType.Type225: return Events225.Remove((Tae225)item);
                case TimeActEventType.Type226: return Events226.Remove((Tae226)item);
                case TimeActEventType.Type228: return Events228.Remove((Tae228)item);
                case TimeActEventType.Type229: return Events229.Remove((Tae229)item);
                case TimeActEventType.Type231: return Events231.Remove((Tae231)item);
                case TimeActEventType.Type232: return Events232.Remove((Tae232)item);
                case TimeActEventType.Type233: return Events233.Remove((Tae233)item);
                case TimeActEventType.Type236: return Events236.Remove((Tae236)item);
                case TimeActEventType.Type300: return Events300.Remove((Tae300)item);
                case TimeActEventType.Type301: return Events301.Remove((Tae301)item);
                case TimeActEventType.Type302: return Events302.Remove((Tae302)item);
                case TimeActEventType.Type303: return Events303.Remove((Tae303)item);
                case TimeActEventType.Type304: return Events304.Remove((Tae304)item);
                case TimeActEventType.Type306: return Events306.Remove((Tae306)item);
                case TimeActEventType.Type307: return Events307.Remove((Tae307)item);
                case TimeActEventType.Type308: return Events308.Remove((Tae308)item);
                case TimeActEventType.Type401: return Events401.Remove((Tae401)item);
                case TimeActEventType.Type500: return Events500.Remove((Tae500)item);   
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            Remove(GlobalList[index]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GlobalList.GetEnumerator();
        }
    }
}

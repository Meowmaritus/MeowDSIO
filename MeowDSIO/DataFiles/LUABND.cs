using MeowDSIO.DataTypes.BND;
using MeowDSIO.DataTypes.LUAINFO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataFiles
{
    public class LUABND : DataFile
    {
        public const int ID_ScriptListStart = 1000;
        public const int ID_GNL = 1000000;
        public const int ID_INFO = 1000001;

        public BNDHeader Header { get; set; } = null;
        public LUAINFOHeader LuaInfoHeader { get; set; } = null;

        public bool IsRemaster { get; set; } = false;
        public bool IsAI { get; set; } = true;

        public Dictionary<string, byte[]> Scripts { get; set; } = new Dictionary<string, byte[]>();
        public List<string> GlobalsList { get; set; } = new List<string>();
        public List<Goal> Goals { get; set; } = new List<Goal>();

        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            var bnd = bin.ReadAsDataFile<BND>();
            Header = bnd.Header;
            IsRemaster = false;
            IsAI = false;
            foreach (var entry in bnd)
            {
                if (!IsRemaster && entry.Name.ToUpper().Contains("INTERROOT_X64"))
                    IsRemaster = true;

                if (!IsAI && entry.Name.ToUpper().Contains(@"\AI\"))
                    IsAI = true;

                if (entry.ID >= ID_ScriptListStart && entry.ID < ID_GNL)
                {
                    Scripts.Add(MiscUtil.GetFileNameWithoutDirectoryOrExtension(entry.Name), entry.GetBytes());
                }
                else if (entry.ID == ID_GNL)
                {
                    var luagnl = entry.ReadDataAs<LUAGNL>();
                    GlobalsList = luagnl.GlobalVariableNames;
                }
                else if (entry.ID == ID_INFO)
                {
                    var luainfo = entry.ReadDataAs<LUAINFO>();
                    Goals = luainfo.Goals;
                    LuaInfoHeader = luainfo.Header;
                }
                else
                {
                    throw new Exception($"Unexpected binder ID in LUABND: {entry.ID}");
                }
            }
        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            var bnd = new BND();
            bnd.Header = Header;

            var luainfo = new LUAINFO()
            {
                Header = LuaInfoHeader,
                Goals = Goals,
            };

            var luagnl = new LUAGNL()
            {
                GlobalVariableNames = GlobalsList,
            };

            int i = 0;
            foreach (var script in Scripts)
            {
                var newEntry = new BNDEntry(ID_ScriptListStart + (i++), 
                    $@"N:\FRPG\data\INTERROOT_{(IsRemaster ? "x64" : "win32")}\script{(IsAI ? @"\ai\out" : "")}\bin\{script.Key}.lua",
                    script.Value);

                bnd.Add(newEntry);
            }

            var shortName = MiscUtil.GetFileNameWithoutDirectoryOrExtension(FilePath ?? VirtualUri);
            shortName = MiscUtil.GetFileNameWithoutDirectoryOrExtension(shortName);

            bnd.Add(new BNDEntry(ID_GNL, $"{shortName}.luagnl", 
                DataFile.SaveAsBytes(luagnl, $"{shortName}.luagnl")));

            bnd.Add(new BNDEntry(ID_INFO, $"{shortName}.luainfo", 
                DataFile.SaveAsBytes(luainfo, $"{shortName}.luainfo")));

            bin.WriteDataFile(bnd, FilePath ?? VirtualUri);
        }
    }
}

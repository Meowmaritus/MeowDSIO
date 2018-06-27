using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeowDSIO.DataTypes.TAE;
using System.ComponentModel;
using Newtonsoft.Json;
using MeowDSIO.DataTypes.TAE.Events;

namespace MeowDSIO.DataFiles
{
    
    public class TAE : DataFile
    {
        public IEnumerable<int> AnimationIDs => Animations.Select(x => x.ID);

        public void UpdateAnimGroupIndices()
        {
            for (int i = 0; i < AnimationGroups.Count; i++)
            {
                AnimationGroups[i].DisplayIndex = i + 1;
            }
        }

        public TAEHeader Header { get; set; }
        public string SkeletonName { get; set; }
        public string SibName { get; set; }
        public List<AnimationRef> Animations { get; set; } = new List<AnimationRef>();
        public List<AnimationGroup> AnimationGroups { get; set; } = new List<AnimationGroup>();

        public int EventHeaderSize
        {
            get
            {
                if (Header.VersionMajor == 11 && Header.VersionMinor == 1)
                    return 0x0C;
                else if (Header.VersionMajor == 1 && Header.VersionMinor == 0)
                    return 0x10;
                else
                    throw new NotImplementedException($"Don't know event header size of TAE Version {Header.VersionMajor}.{Header.VersionMinor}");
            }
        }

        private Animation LoadAnimationFromOffset(DSBinaryReader bin, int offset, int animID_ForDebug)
        {
            int oldOffset = (int)bin.BaseStream.Position;
            bin.BaseStream.Seek(offset, SeekOrigin.Begin);
            var anim = new Animation();

            try
            {
                int eventCount = bin.ReadInt32();
                int eventHeadersOffset = bin.ReadInt32();
                bin.BaseStream.Seek(0x10, SeekOrigin.Current); //skip shit we don't need
                int animFileOffset = bin.ReadInt32();

                for (int i = 0; i < eventCount; i++)
                {
                    //lazily seek to the start of each event manually.
                    bin.BaseStream.Seek(eventHeadersOffset + (EventHeaderSize * i), SeekOrigin.Begin);

                    int startTimeOffset = bin.ReadInt32();
                    int endTimeOffset = bin.ReadInt32();
                    int eventBodyOffset = bin.ReadInt32();

                    float startTime = -1;
                    float endTime = -1;

                    bin.StepIn(startTimeOffset);
                    {
                        startTime = bin.ReadSingle();
                    }
                    bin.StepOut();

                    bin.StepIn(endTimeOffset);
                    {
                        endTime = bin.ReadSingle();
                    }
                    bin.StepOut();

                    bin.StepIn(eventBodyOffset);
                    {
                        TimeActEventType eventType = (TimeActEventType)bin.ReadInt32();
                        int eventParamOffset = bin.ReadInt32();
                        bin.StepIn(eventParamOffset);
                        {
                            switch (eventType)
                            {
                                case TimeActEventType.Type0: var newType0 = new Tae000() { Index = i, StartTime = startTime, EndTime = endTime }; newType0.ReadParameters(bin); anim.EventList.Events0.Add(newType0); break;
                                case TimeActEventType.Type1: var newType1 = new Tae001() { Index = i, StartTime = startTime, EndTime = endTime }; newType1.ReadParameters(bin); anim.EventList.Events1.Add(newType1); break;
                                case TimeActEventType.Type2: var newType2 = new Tae002() { Index = i, StartTime = startTime, EndTime = endTime }; newType2.ReadParameters(bin); anim.EventList.Events2.Add(newType2); break;
                                case TimeActEventType.Type5: var newType5 = new Tae005() { Index = i, StartTime = startTime, EndTime = endTime }; newType5.ReadParameters(bin); anim.EventList.Events5.Add(newType5); break;
                                case TimeActEventType.Type8: var newType8 = new Tae008() { Index = i, StartTime = startTime, EndTime = endTime }; newType8.ReadParameters(bin); anim.EventList.Events8.Add(newType8); break;
                                case TimeActEventType.Type16: var newType16 = new Tae016() { Index = i, StartTime = startTime, EndTime = endTime }; newType16.ReadParameters(bin); anim.EventList.Events16.Add(newType16); break;
                                case TimeActEventType.Type24: var newType24 = new Tae024() { Index = i, StartTime = startTime, EndTime = endTime }; newType24.ReadParameters(bin); anim.EventList.Events24.Add(newType24); break;
                                case TimeActEventType.Type32: var newType32 = new Tae032() { Index = i, StartTime = startTime, EndTime = endTime }; newType32.ReadParameters(bin); anim.EventList.Events32.Add(newType32); break;
                                case TimeActEventType.Type33: var newType33 = new Tae033() { Index = i, StartTime = startTime, EndTime = endTime }; newType33.ReadParameters(bin); anim.EventList.Events33.Add(newType33); break;
                                case TimeActEventType.Type64: var newType64 = new Tae064() { Index = i, StartTime = startTime, EndTime = endTime }; newType64.ReadParameters(bin); anim.EventList.Events64.Add(newType64); break;
                                case TimeActEventType.Type65: var newType65 = new Tae065() { Index = i, StartTime = startTime, EndTime = endTime }; newType65.ReadParameters(bin); anim.EventList.Events65.Add(newType65); break;
                                case TimeActEventType.Type66: var newType66 = new Tae066() { Index = i, StartTime = startTime, EndTime = endTime }; newType66.ReadParameters(bin); anim.EventList.Events66.Add(newType66); break;
                                case TimeActEventType.Type67: var newType67 = new Tae067() { Index = i, StartTime = startTime, EndTime = endTime }; newType67.ReadParameters(bin); anim.EventList.Events67.Add(newType67); break;
                                case TimeActEventType.Type96: var newType96 = new Tae096() { Index = i, StartTime = startTime, EndTime = endTime }; newType96.ReadParameters(bin); anim.EventList.Events96.Add(newType96); break;
                                case TimeActEventType.Type99: var newType99 = new Tae099() { Index = i, StartTime = startTime, EndTime = endTime }; newType99.ReadParameters(bin); anim.EventList.Events99.Add(newType99); break;
                                case TimeActEventType.Type100: var newType100 = new Tae100() { Index = i, StartTime = startTime, EndTime = endTime }; newType100.ReadParameters(bin); anim.EventList.Events100.Add(newType100); break;
                                case TimeActEventType.Type101: var newType101 = new Tae101() { Index = i, StartTime = startTime, EndTime = endTime }; newType101.ReadParameters(bin); anim.EventList.Events101.Add(newType101); break;
                                case TimeActEventType.Type104: var newType104 = new Tae104() { Index = i, StartTime = startTime, EndTime = endTime }; newType104.ReadParameters(bin); anim.EventList.Events104.Add(newType104); break;
                                case TimeActEventType.Type108: var newType108 = new Tae108() { Index = i, StartTime = startTime, EndTime = endTime }; newType108.ReadParameters(bin); anim.EventList.Events108.Add(newType108); break;
                                case TimeActEventType.Type109: var newType109 = new Tae109() { Index = i, StartTime = startTime, EndTime = endTime }; newType109.ReadParameters(bin); anim.EventList.Events109.Add(newType109); break;
                                case TimeActEventType.Type110: var newType110 = new Tae110() { Index = i, StartTime = startTime, EndTime = endTime }; newType110.ReadParameters(bin); anim.EventList.Events110.Add(newType110); break;
                                case TimeActEventType.Type112: var newType112 = new Tae112() { Index = i, StartTime = startTime, EndTime = endTime }; newType112.ReadParameters(bin); anim.EventList.Events112.Add(newType112); break;
                                case TimeActEventType.Type114: var newType114 = new Tae114() { Index = i, StartTime = startTime, EndTime = endTime }; newType114.ReadParameters(bin); anim.EventList.Events114.Add(newType114); break;
                                case TimeActEventType.Type115: var newType115 = new Tae115() { Index = i, StartTime = startTime, EndTime = endTime }; newType115.ReadParameters(bin); anim.EventList.Events115.Add(newType115); break;
                                case TimeActEventType.Type116: var newType116 = new Tae116() { Index = i, StartTime = startTime, EndTime = endTime }; newType116.ReadParameters(bin); anim.EventList.Events116.Add(newType116); break;
                                case TimeActEventType.Type118: var newType118 = new Tae118() { Index = i, StartTime = startTime, EndTime = endTime }; newType118.ReadParameters(bin); anim.EventList.Events118.Add(newType118); break;
                                case TimeActEventType.Type119: var newType119 = new Tae119() { Index = i, StartTime = startTime, EndTime = endTime }; newType119.ReadParameters(bin); anim.EventList.Events119.Add(newType119); break;
                                case TimeActEventType.Type120: var newType120 = new Tae120() { Index = i, StartTime = startTime, EndTime = endTime }; newType120.ReadParameters(bin); anim.EventList.Events120.Add(newType120); break;
                                case TimeActEventType.Type121: var newType121 = new Tae121() { Index = i, StartTime = startTime, EndTime = endTime }; newType121.ReadParameters(bin); anim.EventList.Events121.Add(newType121); break;
                                case TimeActEventType.PlaySound: var newType128 = new TaePlaySound() { Index = i, StartTime = startTime, EndTime = endTime }; newType128.ReadParameters(bin); anim.EventList.Events128.Add(newType128); break;
                                case TimeActEventType.Type129: var newType129 = new Tae129() { Index = i, StartTime = startTime, EndTime = endTime }; newType129.ReadParameters(bin); anim.EventList.Events129.Add(newType129); break;
                                case TimeActEventType.Type130: var newType130 = new Tae130() { Index = i, StartTime = startTime, EndTime = endTime }; newType130.ReadParameters(bin); anim.EventList.Events130.Add(newType130); break;
                                case TimeActEventType.Type144: var newType144 = new Tae144() { Index = i, StartTime = startTime, EndTime = endTime }; newType144.ReadParameters(bin); anim.EventList.Events144.Add(newType144); break;
                                case TimeActEventType.Type145: var newType145 = new Tae145() { Index = i, StartTime = startTime, EndTime = endTime }; newType145.ReadParameters(bin); anim.EventList.Events145.Add(newType145); break;
                                case TimeActEventType.Type193: var newType193 = new Tae193() { Index = i, StartTime = startTime, EndTime = endTime }; newType193.ReadParameters(bin); anim.EventList.Events193.Add(newType193); break;
                                case TimeActEventType.Type224: var newType224 = new Tae224() { Index = i, StartTime = startTime, EndTime = endTime }; newType224.ReadParameters(bin); anim.EventList.Events224.Add(newType224); break;
                                case TimeActEventType.Type225: var newType225 = new Tae225() { Index = i, StartTime = startTime, EndTime = endTime }; newType225.ReadParameters(bin); anim.EventList.Events225.Add(newType225); break;
                                case TimeActEventType.Type226: var newType226 = new Tae226() { Index = i, StartTime = startTime, EndTime = endTime }; newType226.ReadParameters(bin); anim.EventList.Events226.Add(newType226); break;
                                case TimeActEventType.Type228: var newType228 = new Tae228() { Index = i, StartTime = startTime, EndTime = endTime }; newType228.ReadParameters(bin); anim.EventList.Events228.Add(newType228); break;
                                case TimeActEventType.Type229: var newType229 = new Tae229() { Index = i, StartTime = startTime, EndTime = endTime }; newType229.ReadParameters(bin); anim.EventList.Events229.Add(newType229); break;
                                case TimeActEventType.Type231: var newType231 = new Tae231() { Index = i, StartTime = startTime, EndTime = endTime }; newType231.ReadParameters(bin); anim.EventList.Events231.Add(newType231); break;
                                case TimeActEventType.Type232: var newType232 = new Tae232() { Index = i, StartTime = startTime, EndTime = endTime }; newType232.ReadParameters(bin); anim.EventList.Events232.Add(newType232); break;
                                case TimeActEventType.Type233: var newType233 = new Tae233() { Index = i, StartTime = startTime, EndTime = endTime }; newType233.ReadParameters(bin); anim.EventList.Events233.Add(newType233); break;
                                case TimeActEventType.Type236: var newType236 = new Tae236() { Index = i, StartTime = startTime, EndTime = endTime }; newType236.ReadParameters(bin); anim.EventList.Events236.Add(newType236); break;
                                case TimeActEventType.Type300: var newType300 = new Tae300() { Index = i, StartTime = startTime, EndTime = endTime }; newType300.ReadParameters(bin); anim.EventList.Events300.Add(newType300); break;
                                case TimeActEventType.Type301: var newType301 = new Tae301() { Index = i, StartTime = startTime, EndTime = endTime }; newType301.ReadParameters(bin); anim.EventList.Events301.Add(newType301); break;
                                case TimeActEventType.Type302: var newType302 = new Tae302() { Index = i, StartTime = startTime, EndTime = endTime }; newType302.ReadParameters(bin); anim.EventList.Events302.Add(newType302); break;
                                case TimeActEventType.Type303: var newType303 = new Tae303() { Index = i, StartTime = startTime, EndTime = endTime }; newType303.ReadParameters(bin); anim.EventList.Events303.Add(newType303); break;
                                case TimeActEventType.Type304: var newType304 = new Tae304() { Index = i, StartTime = startTime, EndTime = endTime }; newType304.ReadParameters(bin); anim.EventList.Events304.Add(newType304); break;
                                case TimeActEventType.Type306: var newType306 = new Tae306() { Index = i, StartTime = startTime, EndTime = endTime }; newType306.ReadParameters(bin); anim.EventList.Events306.Add(newType306); break;
                                case TimeActEventType.Type307: var newType307 = new Tae307() { Index = i, StartTime = startTime, EndTime = endTime }; newType307.ReadParameters(bin); anim.EventList.Events307.Add(newType307); break;
                                case TimeActEventType.Type308: var newType308 = new Tae308() { Index = i, StartTime = startTime, EndTime = endTime }; newType308.ReadParameters(bin); anim.EventList.Events308.Add(newType308); break;
                                case TimeActEventType.Type401: var newType401 = new Tae401() { Index = i, StartTime = startTime, EndTime = endTime }; newType401.ReadParameters(bin); anim.EventList.Events401.Add(newType401); break;
                                case TimeActEventType.Type500: var newType500 = new Tae500() { Index = i, StartTime = startTime, EndTime = endTime }; newType500.ReadParameters(bin); anim.EventList.Events500.Add(newType500); break;
                            }
                        }
                        bin.StepOut();
                    }
                    bin.StepOut();
                }

                bin.BaseStream.Seek(animFileOffset, SeekOrigin.Begin);

                int fileType = bin.ReadInt32();
                if (fileType == 0)
                {
                    int dataOffset = bin.ReadInt32();
                    //bin.BaseStream.Seek(dataOffset, SeekOrigin.Begin);

                    int nameOffset = bin.ReadInt32();

                    anim.Unk1 = bin.ReadInt32();
                    anim.Unk2 = bin.ReadInt32();

                    if (nameOffset == 0)
                    {
                        throw new Exception("Anim file type was that of a named one but the name pointer was NULL.");
                    }
                    bin.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
                    anim.FileName = ReadUnicodeString(bin);
                }
                else if (fileType == 1)
                {
                    anim.FileName = null;

                    bin.ReadInt32(); //offset pointing to next dword for some reason.
                    bin.ReadInt32(); //offset pointing to start of next anim file struct

                    anim.RefAnimID = bin.ReadInt32();
                    //Null 1
                    //Null 2
                    //Null 3
                }
                else
                {
                    throw new Exception($"Unknown anim file type code: {fileType}");
                }

                return anim;
            }
            catch (EndOfStreamException)
            {
                MiscUtil.PrintlnDX($"Warning: reached end of file while parsing animation {animID_ForDebug}; data may not be complete.", ConsoleColor.Yellow);
                //if (!MiscUtil.ConsolePrompYesNo("Would you like to continue loading the file and run the risk of " + 
                //    "accidentally outputting a file that might be missing some of its original data?"))
                //{
                //    throw new LoadAbortedException();
                //}
                //else
                //{
                //    return a;
                //}

                return anim;
            }
            finally
            {
                bin.BaseStream.Seek(oldOffset, SeekOrigin.Begin);
            }
        }

        private string ReadUnicodeString(DSBinaryReader bin)
        {
            StringBuilder sb = new StringBuilder();
            byte[] next = { 0, 0 };
            bool endString = false;
            do
            {
                next = bin.ReadBytes(2);
                endString = (next[0] == 0 && next[1] == 0);

                if (!endString)
                {
                    sb.Append(Encoding.Unicode.GetString(next));
                }
            }
            while (!endString);
            return sb.ToString();
        }

        //TODO: Measure real progress.
        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            Header = new TAEHeader();
            var fileSignature = bin.ReadBytes(4);
            if (fileSignature.Where((x, i) => x != Header.Signature[i]).Any())
            {
                throw new Exception($"Invalid signature in this TAE file: " + 
                    $"[{string.Join(",", fileSignature.Select(x => x.ToString("X8")))}] " + 
                    $"(ASCII: '{Encoding.ASCII.GetString(fileSignature)}')");
            }

            Header.IsBigEndian = bin.ReadBoolean();

            bin.BigEndian = Header.IsBigEndian;
            bin.ReadBytes(3); //3 null bytes after big endian flag

            Header.VersionMajor = bin.ReadUInt16();
            Header.VersionMinor = bin.ReadUInt16();

            var fileSize = bin.ReadInt32();

            Header.UnknownB00 = bin.ReadUInt32();
            Header.UnknownB01 = bin.ReadUInt32();
            Header.UnknownB02 = bin.ReadUInt32();
            Header.UnknownB03 = bin.ReadUInt32();

            Header.UnknownFlags = bin.ReadBytes(TAEHeader.UnknownFlagsLength);

            Header.FileID = bin.ReadInt32();
            
            //Animation IDs
            var animCount = bin.ReadInt32();
            int OFF_AnimID = bin.ReadInt32();
            bin.DoAt(OFF_AnimID, () =>
            {
                for (int i = 0; i < animCount; i++)
                {
                    var animID = bin.ReadInt32();
                    var animOffset = bin.ReadInt32();
                    var anim = LoadAnimationFromOffset(bin, animOffset, animID);
                    var animRef = new AnimationRef() { ID = animID, Anim = anim };

                    Animations.Add(animRef);
                }
            });

            //Anim Groups
            int OFF_AnimGroups = bin.ReadInt32();
            bin.DoAt(OFF_AnimGroups, () =>
            {
                int animGroupCount = bin.ReadInt32();
                int actualAnimGroupsOffset = bin.ReadInt32();

                bin.BaseStream.Seek(actualAnimGroupsOffset, SeekOrigin.Begin);

                for (int i = 0; i < animGroupCount; i++)
                {
                    var nextAnimGroup = new AnimationGroup(i + 1);
                    nextAnimGroup.FirstID = bin.ReadInt32();
                    nextAnimGroup.LastID = bin.ReadInt32();
                    var _firstIdOffset = bin.ReadInt32();

                    AnimationGroups.Add(nextAnimGroup);
                }
            });

            Header.UnknownC = bin.ReadInt32();
            //We already found the animation count and offsets from the anim ids earlier
            int _animCount = bin.ReadInt32();
            int _animOffset = bin.ReadInt32();
            if (_animCount != Animations.Count)
            {
                throw new Exception($"Animation IDs count [{Animations.Count}] is different than Animations count [{_animCount}]!");
            }

            Header.UnknownE00 = bin.ReadUInt32();
            Header.UnknownE01 = bin.ReadUInt32();
            Header.UnknownE02 = bin.ReadUInt32();
            Header.UnknownE03 = bin.ReadUInt32();
            Header.UnknownE04 = bin.ReadUInt32();
            Header.FileID2 = bin.ReadInt32();
            Header.FileID3 = bin.ReadInt32();
            Header.UnknownE07 = bin.ReadUInt32();
            Header.UnknownE08 = bin.ReadUInt32();
            Header.UnknownE09 = bin.ReadUInt32();

            int filenamesOffset = bin.ReadInt32();
            bin.BaseStream.Seek(filenamesOffset, SeekOrigin.Begin);

            int skeletonNameOffset = bin.ReadInt32();
            int sibNameOffset = bin.ReadInt32();

            bin.BaseStream.Seek(skeletonNameOffset, SeekOrigin.Begin);

            SkeletonName = ReadUnicodeString(bin);

            bin.BaseStream.Seek(sibNameOffset, SeekOrigin.Begin);

            SibName = ReadUnicodeString(bin);
        }

        //TODO: Measure real progress.
        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            //SkeletonName, SibName:
            bin.Seek(0x94, SeekOrigin.Begin);
            bin.Write(0x00000098);
            bin.Write(0x000000A8);
            bin.Write(0x000000A8 + (SkeletonName.Length * 2 + 2));
            bin.Write(0x00000000);
            bin.Write(0x00000000);
            bin.Write(Encoding.Unicode.GetBytes(SkeletonName));
            bin.Write((short)0); //string terminator
            bin.Write(Encoding.Unicode.GetBytes(SibName));
            bin.Write((short)0); //string terminator

            //Animation IDs - First Pass
            int OFF_AnimationIDs = (int)bin.BaseStream.Position;

            var animationIdOffsets = new Dictionary<int, int>(); //<animation ID, offset>
            foreach (var anim in Animations)
            {
                animationIdOffsets.Add(anim.ID, (int)bin.BaseStream.Position);
                bin.Write(anim.ID);
                bin.Placeholder(); //Pointer to animation will be inserted here.
            }

            //Animation Groups - Full Pass
            int OFF_AnimationGroups = (int)bin.BaseStream.Position;
            bin.Write(AnimationGroups.Count);
            bin.Write((int)(bin.BaseStream.Position + 4)); //Pointer that always points to the next dword for some reason.

            foreach (var g in AnimationGroups)
            {
                bin.Write(g.FirstID);
                bin.Write(g.LastID);

                if (!animationIdOffsets.ContainsKey(g.FirstID))
                    throw new Exception($"Animation group begins on an animation ID that isn't " + 
                        $"present in the list of animations.");

                bin.Write(animationIdOffsets[g.FirstID]);
            }

            //Animation First Pass
            int OFF_Animations = (int)bin.BaseStream.Position;
            var animationOffsets = new Dictionary<int, int>(); //<animation ID, offset>
            var animationTimeConstantLists = new Dictionary<int, List<float>>(); //<animation ID, List<time constant>>
            foreach (var anim in Animations)
            {
                animationOffsets.Add(anim.ID, (int)bin.BaseStream.Position);
                bin.Write(anim.Anim.EventList.Count);
                bin.Placeholder(); //PLACEHOLDER: animation event headers offset
                //Println($"Wrote Anim{anim.Key} event header offset placeholder value (0xDEADD00D) at address {(bin.BaseStream.Position-4):X8}");
                bin.Write(0); //Null 1
                bin.Write(0); //Null 2
                animationTimeConstantLists.Add(anim.ID, new List<float>());
                //Populate all of the time constants used:
                foreach (var e in anim.Anim.EventList)
                {
                    if (!animationTimeConstantLists[anim.ID].Contains(e.StartTime))
                        animationTimeConstantLists[anim.ID].Add(e.StartTime);
                    if (!animationTimeConstantLists[anim.ID].Contains(e.EndTime))
                        animationTimeConstantLists[anim.ID].Add(e.EndTime);
                }
                bin.Write(animationTimeConstantLists[anim.ID].Count); //# time constants in this anim
                bin.Placeholder(); //PLACEHOLDER: Time Constants offset
                //Println($"Wrote Anim{anim.Key} time constant offset placeholder value (0xDEADD00D) at address {(bin.BaseStream.Position-4):X8}");
                bin.Placeholder(); //PLACEHOLDER: Animation file struct offset
                //Println($"Wrote Anim{anim.Key} anim file offset placeholder value (0xDEADD00D) at address {(bin.BaseStream.Position-4):X8}");
            }



            //Animation ID Second Pass
            bin.DoAt(OFF_AnimationIDs, () =>
            {
                foreach (var anim in Animations)
                {
                    //Move from the animation ID offset to the animation pointer offset.
                    bin.Jump(4);

                    //Write pointer to animation into this animation ID.
                    bin.Write(animationOffsets[anim.ID]);
                }
            });


            //Animations Second Pass
            var eventHeaderStartOffsets = new Dictionary<int, int>(); //<anim ID, event header start offset>
            var animationFileOffsets = new Dictionary<int, int>(); //<animation ID, offset>
            var animTimeConstantStartOffsets = new Dictionary<int, int>(); //<animation ID, start offset>
            var animTimeConstantOffsets = new Dictionary<int, Dictionary<float, int>>(); //<animation ID, Dictionary<time const, offset>>
            {
                //The unnamed animation files contain the ID of the last named animation file.
                //int lastNamedAnimation = -1;

                //TODO: Check if it's possible for the very first animation data listed to be unnamed, 
                //and what would go in the last named animation ID value if that were the case?

                foreach (var anim in Animations)
                {
                    //ANIMATION FILE:
                    {
                        //Write anim file struct:
                        animationFileOffsets.Add(anim.ID, (int)bin.BaseStream.Position);
                        if (anim.Anim.FileName != null)
                        {
                            bin.Write(0x00000000); //type 0 - named
                            bin.Write((int)(bin.BaseStream.Position + 0x04)); //offset pointing to next dword for some reason.
                            bin.Write((int)(bin.BaseStream.Position + 0x10)); //offset pointing to name start
                            bin.Write(anim.Anim.Unk1); //Unknown
                            bin.Write(anim.Anim.Unk2); //Unknown
                            bin.Write(0x00000000); //Null
                            //name start:
                            if (anim.Anim.FileName.Length > 0)
                            {
                                bin.Write(Encoding.Unicode.GetBytes(anim.Anim.FileName));
                            }
                            bin.Write((short)0); //string terminator
                        }
                        else
                        {
                            bin.Write(0x00000001); //type 1 - nameless
                            bin.Write((int)(bin.BaseStream.Position + 0x04)); //offset pointing to next dword for some reason.
                            bin.Write((int)(bin.BaseStream.Position + 0x14)); //offset pointing to start of next anim file struct
                            bin.Write(anim.Anim.RefAnimID); //Last named animation ID, to which this one is linked.
                            bin.Write(0x00000000); //Null 1
                            bin.Write(0x00000000); //Null 2
                            bin.Write(0x00000000); //Null 3
                        }
                    }
                    animTimeConstantStartOffsets.Add(anim.ID, (int)bin.BaseStream.Position);
                    //Write the time constants and record their offsets:
                    animTimeConstantOffsets.Add(anim.ID, new Dictionary<float, int>());
                    foreach (var tc in animationTimeConstantLists[anim.ID])
                    {
                        animTimeConstantOffsets[anim.ID].Add(tc, (int)bin.BaseStream.Position);
                        bin.Write(tc);
                    }


                    //Event headers (note: all event headers are (EventHeaderSize) long):
                    eventHeaderStartOffsets.Add(anim.ID, (int)bin.BaseStream.Position);
                    foreach (var e in anim.Anim.EventList)
                    {
                        long currentEventHeaderStart = bin.Position;
                        bin.Write((int)animTimeConstantOffsets[anim.ID][e.StartTime]); //offset of start time in time constants.
                        bin.Write((int)animTimeConstantOffsets[anim.ID][e.EndTime]); //offset of end time in time constants.
                        bin.Placeholder(); //PLACEHOLDER: Event body
                        long currentEventHeaderLength = bin.Position - currentEventHeaderStart;
                        if (currentEventHeaderLength < EventHeaderSize)
                        {
                            bin.Write(new byte[EventHeaderSize - currentEventHeaderLength]);
                        }
                    }

                    //Event bodies
                    var eventBodyOffsets = new Dictionary<TimeActEventBase, int>();
                    foreach (var e in anim.Anim.EventList)
                    {
                        eventBodyOffsets.Add(e, (int)bin.BaseStream.Position);

                        bin.Write((int)e.EventType);
                        bin.Write((int)(bin.BaseStream.Position + 4)); //one of those pointers to very next dword.

                        //Note: the logic for the length of a particular event param array is handled 
                        //      in the read function as well as in the AnimationEvent class itself.

                        e.WriteParameters(bin);

                        //foreach (var p in e.Parameters)
                        //{
                        //    var paramVal = p.Value.ToUpper();
                        //    if (paramVal.EndsWith("F") || paramVal.Contains(".") || paramVal.Contains(","))
                        //    {
                        //        bin.Write(float.Parse(paramVal.Replace("F", "")));
                        //    }
                        //    else
                        //    {
                        //        bin.Write(int.Parse(paramVal));
                        //    }
                        //}
                    }

                    //Event headers pass 2:
                    bin.DoAt(eventHeaderStartOffsets[anim.ID], () =>
                    {
                        foreach (var e in anim.Anim.EventList)
                        {
                            //skip to event body offset field:
                            bin.Seek(8, SeekOrigin.Current);

                            //write event body offset:
                            bin.Write(eventBodyOffsets[e]);
                        }
                    });

                }
            }

            //Animations Third Pass
            bin.DoAt(OFF_Animations, () =>
            {
                foreach (var anim in Animations)
                {
                    bin.Seek(animationOffsets[anim.ID] + 4, SeekOrigin.Begin);
                    //event header start offset:
                    if (anim.Anim.EventList.Count > 0)
                        bin.Write(eventHeaderStartOffsets[anim.ID]);
                    else
                        bin.Write(0x00000000);
                    //Println($"Wrote Anim{anim.Key} event header offset value 0x{eventHeaderStartOffsets[anim.Key]:X8} at address {(bin.BaseStream.Position-4):X8}");
                    bin.Seek(0xC, SeekOrigin.Current);
                    //time constants offset (writing offset of the *first constant listed*):
                    if (animationTimeConstantLists[anim.ID].Count > 0)
                    {
                        bin.Write(animTimeConstantStartOffsets[anim.ID]);
                        //Println($"Wrote Anim{anim.Key} time constants offset value 0x{animTimeConstantStartOffsets[anim.Key]:X8} at address {(bin.BaseStream.Position-4):X8}");
                    }
                    else
                    {
                        bin.Write(0x00000000); //Null
                        //Println($"Wrote Anim{anim.Key} time constants offset value NULL at address {(bin.BaseStream.Position-4):X8}");
                    }
                    //anim file struct offset:
                    bin.Write((int)animationFileOffsets[anim.ID]);
                    //Println($"Wrote Anim{anim.Key} anim file offset value 0x{((int)animationFileOffsets[anim.Key]):X8} at address {(bin.BaseStream.Position-4):X8}");
                }
            });

            var END_OF_FILE = (int)bin.Position;

            //final header write:
            bin.Seek(0, SeekOrigin.Begin);

            bin.Write(Header.Signature);

            bin.Write(Header.IsBigEndian);
            bin.BigEndian = Header.IsBigEndian;
            bin.Write(new byte[] { 0, 0, 0 }); //3 null bytes after big endian flag.

            bin.Write(Header.VersionMajor);
            bin.Write(Header.VersionMinor);

            bin.Write((int)bin.BaseStream.Length); //File length

            bin.Write(Header.UnknownB00);
            bin.Write(Header.UnknownB01);
            bin.Write(Header.UnknownB02);
            bin.Write(Header.UnknownB03);

            bin.Write(Header.UnknownFlags);

            bin.Write(Header.FileID);
            bin.Write(Animations.Count); //Technically the animation ID count
            bin.Write(OFF_AnimationIDs);
            bin.Write(OFF_AnimationGroups);
            bin.Write(Header.UnknownC);
            bin.Write(Animations.Count); //Techically the animation count
            bin.Write(OFF_Animations);

            bin.Write(Header.UnknownE00);
            bin.Write(Header.UnknownE01);
            bin.Write(Header.UnknownE02);
            bin.Write(Header.UnknownE03);
            bin.Write(Header.UnknownE04);
            bin.Write(Header.FileID2);
            bin.Write(Header.FileID3);
            bin.Write(Header.UnknownE07);
            bin.Write(Header.UnknownE08);
            bin.Write(Header.UnknownE09);

            //Here would be the value at the very beginning of this method!

            //Go to end and pretend like this was a totally normal file write and not the shitshow it was.
            bin.Seek(END_OF_FILE, SeekOrigin.Begin);
        }
    }
}

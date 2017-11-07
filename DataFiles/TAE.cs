using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeowDSIO.DataTypes.TAE;
using System.ComponentModel;

namespace MeowDSIO.DataFiles
{
    
    public class TAE : DataFile
    {
        
        public class TAEHeader
        {
            //"TAE "
            public byte[] Signature { get; set; } = { 0x54, 0x41, 0x45, 0x20 };
            //Sample taken from Artorias (c4100.tae)
            public byte[] MagicBytes { get; set; } =
            { 
                0x40, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x50, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x01, 0x00, 0x01, 0x02, 0x02, 0x00, 0x01, 0x01, 
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
            };

            //Value samples below taken from Artorias (c4100.tae)
            public int Unk1 { get; set; } = 0;
            public int Unk2 { get; set; } = 65547;
            public byte[] Unk3 { get; set; } = { 0x90, 0, 0, 0 };
            public byte[] Unk4 { get; set; } =
            {
                0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00,
                0x80, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x44, 0x1D, 0x03, 0x00,
                0x44, 0x1D, 0x03, 0x00, 
                0x50, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            
            public int ID { get; set; } = 204100;
        }
        
        public TAEHeader Header { get; set; }
        public string SkeletonName { get; set; }
        public string SibName { get; set; }
        public Dictionary<int, Animation> Animations { get; set; }
        public List<AnimationGroup> AnimationGroups { get; set; }

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
                    bin.BaseStream.Seek(eventHeadersOffset + (0xC * i), SeekOrigin.Begin);

                    int startTimeOffset = bin.ReadInt32();
                    int endTimeOffset = bin.ReadInt32();
                    int eventBodyOffset = bin.ReadInt32();

                    bin.BaseStream.Seek(startTimeOffset, SeekOrigin.Begin);
                    float startTime = bin.ReadSingle();
                    bin.BaseStream.Seek(endTimeOffset, SeekOrigin.Begin);
                    float endTime = bin.ReadSingle();
                    bin.BaseStream.Seek(eventBodyOffset, SeekOrigin.Begin);
                    int eventTypeValue = bin.ReadInt32();
                    int eventParamOffset = bin.ReadInt32();
                    bin.BaseStream.Seek(eventParamOffset, SeekOrigin.Begin);

                    AnimationEventType eventType = (AnimationEventType)eventTypeValue;
                    var nextEvent = new AnimationEvent((AnimationEventType)eventType, animID_ForDebug);

                    nextEvent.StartTime = startTime;
                    nextEvent.EndTime = endTime;

                    for (int j = 0; j < nextEvent.Parameters.Length; j++)
                    {
                        nextEvent.Parameters[j] = bin.ReadInt32();
                    }

                    anim.Events.Add(nextEvent);
                }

                bin.BaseStream.Seek(animFileOffset, SeekOrigin.Begin);

                int fileType = bin.ReadInt32();
                if (fileType == 0)
                {
                    int dataOffset = bin.ReadInt32();
                    bin.BaseStream.Seek(dataOffset, SeekOrigin.Begin);

                    int nameOffset = bin.ReadInt32();
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

        protected override void Read(DSBinaryReader bin)
        {
            Header = new TAEHeader();
            var fileSignature = bin.ReadBytes(4);
            if (fileSignature.Where((x, i) => x != Header.Signature[i]).Any())
            {
                throw new Exception($"Invalid signature in this TAE file: " + 
                    $"[{string.Join(",", fileSignature.Select(x => x.ToString("X8")))}] " + 
                    $"(ASCII: '{Encoding.ASCII.GetString(fileSignature)}')");
            }

            Header.Unk1 = bin.ReadInt32();
            Header.Unk2 = bin.ReadInt32();
            var fileSize = bin.ReadInt32();
            Header.MagicBytes = bin.ReadBytes(64);
            Header.ID = bin.ReadInt32();
            
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

                    Animations.Add(animID, anim);
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
                    var nextAnimGroup = new AnimationGroup();
                    nextAnimGroup.FirstID = bin.ReadInt32();
                    nextAnimGroup.LastID = bin.ReadInt32();
                    var _firstIdOffset = bin.ReadInt32();

                    AnimationGroups.Add(nextAnimGroup);
                }
            });

            Header.Unk3 = bin.ReadBytes(4);
            //We already found the animation count and offsets from the anim ids earlier
            int _animCount = bin.ReadInt32();
            int _animOffset = bin.ReadInt32();
            if (_animCount != Animations.Count)
            {
                throw new Exception($"Animation IDs count [{Animations.Count}] is different than Animations count [{_animCount}]!");
            }

            Header.Unk4 = bin.ReadBytes(40);

            int filenamesOffset = bin.ReadInt32();
            bin.BaseStream.Seek(filenamesOffset, SeekOrigin.Begin);

            int skeletonNameOffset = bin.ReadInt32();
            int sibNameOffset = bin.ReadInt32();

            bin.BaseStream.Seek(skeletonNameOffset, SeekOrigin.Begin);

            SkeletonName = ReadUnicodeString(bin);

            bin.BaseStream.Seek(sibNameOffset, SeekOrigin.Begin);

            SibName = ReadUnicodeString(bin);
        }

        private void Println(string txt)
        {
            Console.WriteLine(txt);
        }

        protected override void Write(DSBinaryWriter bin)
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
                animationIdOffsets.Add(anim.Key, (int)bin.BaseStream.Position);
                bin.Write(anim.Key);
                bin.Write(0xDEADD00D); //Pointer to animation will be inserted here.
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
                        $"present in {nameof(TAE)}.{nameof(Animations)} dictionary.");

                bin.Write(animationIdOffsets[g.FirstID]);
            }

            //Animation First Pass
            int OFF_Animations = (int)bin.BaseStream.Position;
            var animationOffsets = new Dictionary<int, int>(); //<animation ID, offset>
            var animationTimeConstantLists = new Dictionary<int, List<float>>(); //<animation ID, List<time constant>>
            foreach (var anim in Animations)
            {
                animationOffsets.Add(anim.Key, (int)bin.BaseStream.Position);
                bin.Write(anim.Value.Events.Count);
                bin.Write(0xDEADD00D); //PLACEHOLDER: animation event headers offset
                //Println($"Wrote Anim{anim.Key} event header offset placeholder value (0xDEADD00D) at address {(bin.BaseStream.Position-4):X8}");
                bin.Write(0); //Null 1
                bin.Write(0); //Null 2
                animationTimeConstantLists.Add(anim.Key, new List<float>());
                //Populate all of the time constants used:
                foreach (var e in anim.Value.Events)
                {
                    if (!animationTimeConstantLists[anim.Key].Contains(e.StartTime))
                        animationTimeConstantLists[anim.Key].Add(e.StartTime);
                    if (!animationTimeConstantLists[anim.Key].Contains(e.EndTime))
                        animationTimeConstantLists[anim.Key].Add(e.EndTime);
                }
                bin.Write(animationTimeConstantLists[anim.Key].Count); //# time constants in this anim
                bin.Write(0xDEADD00D); //PLACEHOLDER: Time Constants offset
                //Println($"Wrote Anim{anim.Key} time constant offset placeholder value (0xDEADD00D) at address {(bin.BaseStream.Position-4):X8}");
                bin.Write(0xDEADD00D); //PLACEHOLDER: Animation file struct offset
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
                    bin.Write(animationOffsets[anim.Key]);
                }
            });


            //Animations Second Pass
            var eventHeaderStartOffsets = new Dictionary<int, int>(); //<anim ID, event header start offset>
            var animationFileOffsets = new Dictionary<int, int>(); //<animation ID, offset>
            var animTimeConstantStartOffsets = new Dictionary<int, int>(); //<animation ID, start offset>
            var animTimeConstantOffsets = new Dictionary<int, Dictionary<float, int>>(); //<animation ID, Dictionary<time const, offset>>
            {
                //The unnamed animation files contain the ID of the last named animation file.
                int lastNamedAnimation = -1;

                //TODO: Check if it's possible for the very first animation data listed to be unnamed, 
                //and what would go in the last named animation ID value if that were the case?

                foreach (var anim in Animations)
                {
                    //ANIMATION FILE:
                    {
                        //Write anim file struct:
                        animationFileOffsets.Add(anim.Key, (int)bin.BaseStream.Position);
                        if (anim.Value.FileName != null)
                        {
                            lastNamedAnimation = anim.Key; //Set last named animation ID value for the unnamed ones to reference.
                            bin.Write(0x00000000); //type 0 - named
                            bin.Write((int)(bin.BaseStream.Position + 0x04)); //offset pointing to next dword for some reason.
                            bin.Write((int)(bin.BaseStream.Position + 0x10)); //offset pointing to name start
                            bin.Write(anim.Value.Unk1); //Unknown
                            bin.Write(anim.Value.Unk2); //Unknown
                            bin.Write(0x00000000); //Null
                            //name start:
                            bin.Write(Encoding.Unicode.GetBytes(anim.Value.FileName));
                            bin.Write((short)0); //string terminator
                        }
                        else
                        {
                            bin.Write(0x00000001); //type 1 - nameless
                            bin.Write((int)(bin.BaseStream.Position + 0x04)); //offset pointing to next dword for some reason.
                            bin.Write((int)(bin.BaseStream.Position + 0x14)); //offset pointing to start of next anim file struct
                            bin.Write(lastNamedAnimation); //Last named animation ID, to which this one is linked.
                            bin.Write(0x00000000); //Null 1
                            bin.Write(0x00000000); //Null 2
                            bin.Write(0x00000000); //Null 3
                        }
                    }
                    animTimeConstantStartOffsets.Add(anim.Key, (int)bin.BaseStream.Position);
                    //Write the time constants and record their offsets:
                    animTimeConstantOffsets.Add(anim.Key, new Dictionary<float, int>());
                    foreach (var tc in animationTimeConstantLists[anim.Key])
                    {
                        animTimeConstantOffsets[anim.Key].Add(tc, (int)bin.BaseStream.Position);
                        bin.Write(tc);
                    }

                    
                    //Event headers (note: all event headers are 0xC long):
                    eventHeaderStartOffsets.Add(anim.Key, (int)bin.BaseStream.Position);
                    foreach (var e in anim.Value.Events)
                    {
                        bin.Write((int)animTimeConstantOffsets[anim.Key][e.StartTime]); //offset of start time in time constants.
                        bin.Write((int)animTimeConstantOffsets[anim.Key][e.EndTime]); //offset of end time in time constants.
                        bin.Write(0xDEADD00D); //PLACEHOLDER: Event body
                    }

                    //Event bodies
                    var eventBodyOffsets = new Dictionary<AnimationEvent, int>();
                    foreach (var e in anim.Value.Events)
                    {
                        eventBodyOffsets.Add(e, (int)bin.BaseStream.Position);

                        bin.Write((int)e.Type);
                        bin.Write((int)(bin.BaseStream.Position + 4)); //one of those pointers to very next dword.

                        //Note: the logic for the length of a particular event param array is handled 
                        //      in the read function as well as in the AnimationEvent class itself.

                        for (int i = 0; i < e.Parameters.Length; i++)
                        {
                            bin.Write(e.Parameters[i].Int);
                        }
                    }

                    //Event headers pass 2:
                    bin.DoAt(eventHeaderStartOffsets[anim.Key], () =>
                    {
                        foreach (var e in anim.Value.Events)
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
                    bin.Seek(animationOffsets[anim.Key] + 4, SeekOrigin.Begin);
                    //event header start offset:
                    if (anim.Value.Events.Count > 0)
                        bin.Write(eventHeaderStartOffsets[anim.Key]);
                    else
                        bin.Write(0x00000000);
                    //Println($"Wrote Anim{anim.Key} event header offset value 0x{eventHeaderStartOffsets[anim.Key]:X8} at address {(bin.BaseStream.Position-4):X8}");
                    bin.Seek(0xC, SeekOrigin.Current);
                    //time constants offset (writing offset of the *first constant listed*):
                    if (animationTimeConstantLists[anim.Key].Count > 0)
                    {
                        bin.Write(animTimeConstantStartOffsets[anim.Key]);
                        //Println($"Wrote Anim{anim.Key} time constants offset value 0x{animTimeConstantStartOffsets[anim.Key]:X8} at address {(bin.BaseStream.Position-4):X8}");
                    }
                    else
                    {
                        bin.Write(0x00000000); //Null
                        //Println($"Wrote Anim{anim.Key} time constants offset value NULL at address {(bin.BaseStream.Position-4):X8}");
                    }
                    //anim file struct offset:
                    bin.Write((int)animationFileOffsets[anim.Key]);
                    //Println($"Wrote Anim{anim.Key} anim file offset value 0x{((int)animationFileOffsets[anim.Key]):X8} at address {(bin.BaseStream.Position-4):X8}");
                }
            });

            //final header write:
            bin.Seek(0, SeekOrigin.Begin);

            bin.Write(Header.Signature);
            bin.Write(Header.Unk1);
            bin.Write(Header.Unk2);
            bin.Write((int)bin.BaseStream.Length); //File length
            bin.Write(Header.MagicBytes);
            bin.Write(Header.ID);
            bin.Write(Animations.Keys.Count); //Technically the animation ID count
            bin.Write(OFF_AnimationIDs);
            bin.Write(OFF_AnimationGroups);
            bin.Write(Header.Unk3);
            bin.Write(Animations.Values.Count); //Techically the animation count
            bin.Write(OFF_Animations);
            bin.Write(Header.Unk4);

            //Here would be the value at the very beginning of this method!

            //Go to end and pretend like this was a totally normal file write and not the shitshow it was.
            bin.Seek(0, SeekOrigin.End);
        }
    }
}

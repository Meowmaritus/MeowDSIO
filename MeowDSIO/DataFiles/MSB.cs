﻿using MeowDSIO.DataTypes.MSB;
using MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST;
using MeowDSIO.DataTypes.MSB.MODEL_PARAM_ST;
using MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST;
using MeowDSIO.DataTypes.MSB.POINT_PARAM_ST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataFiles
{
    public enum MsbSectorFormat
    {
        NONE,
        MODEL_PARAM_ST,
        EVENT_PARAM_ST,
        POINT_PARAM_ST,
        PARTS_PARAM_ST,
    }

    public class MSB : DataFile
    {
        public int Unknown1;

        public List<MsbModel> Models;
        public MsbEventList Events = new MsbEventList();
        public MsbRegionList Regions = new MsbRegionList();
        public MsbPartsList Parts = new MsbPartsList();


        /*
int Unknown1;

int MODEL_PARAM_NameOffset;
int MODEL_PARAM_Count;
int[] MODEL_PARAM_Pointers[MODEL_PARAM_Count];
int startOfNextSectionOffset;
int end = 0;

int EVENT_PARAM_NameOffset;
int EVENT_PARAM_Count;
int[] EVENT_PARAM_Pointers[EVENT_PARAM_Count];

int POINT_PARAM_NameOffset;
int POINT_PARAM_Count;
int[] POINT_PARAM_Pointers[POINT_PARAM_Count];

int PARTS_PARAM_NameOffset;
int PARTS_PARAM_Count;
int[] PARTS_PARAM_Pointers[PARTS_PARAM_Count];

            each struct thing:

            int nameOffset;

            int otherStringOffsetEtc;

            <data>

            string name (nameOffset points here)

            string otherStringEtc (otherStringOffsetEtc points here etc)


         */

        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            Unknown1 = bin.ReadInt32();



            Models = new List<MsbModel>();
            Events = new MsbEventList();
            Regions = new MsbRegionList();
            Parts = new MsbPartsList();
            

            MsbSectorFormat currentSectorFormat = MsbSectorFormat.NONE;

            do
            {
                int currentSectorNameOffset = bin.ReadInt32();

                if (currentSectorNameOffset == 0)
                    break;

                bin.StepIn(currentSectorNameOffset);
                {
                    currentSectorFormat = (MsbSectorFormat)Enum.Parse(typeof(MsbSectorFormat), bin.ReadStringAscii());
                }
                bin.StepOut();

                int structCount = bin.ReadInt32() - 1;

                for (int i = 0; i < structCount; i++)
                {
                    int structOffset = bin.ReadInt32();

                    bin.StepIn(structOffset);
                    {
                        switch (currentSectorFormat)
                        {
                            case MsbSectorFormat.MODEL_PARAM_ST:
                                var newModel = new MsbModel();
                                newModel.Read(bin);
                                Models.Add(newModel);
                                break;
                            case MsbSectorFormat.EVENT_PARAM_ST:
                                var eventType = EventParamSubtype.Lights;
                                bin.StepIn(bin.Position + 0x08);
                                {
                                    eventType = (EventParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (eventType)
                                {
                                    case EventParamSubtype.BlackEyeOrbInvasions:
                                        var newMEBEOI = new MsbEventBlackEyeOrbInvasion();
                                        newMEBEOI.Read(bin);
                                        Events.BlackEyeOrbInvasion.Add(newMEBEOI);
                                        break;
                                    case EventParamSubtype.BloodMsg:
                                        var newMEBM = new MsbEventBloodMsg();
                                        newMEBM.Read(bin);
                                        Events.BloodMessages.Add(newMEBM);
                                        break;
                                    case EventParamSubtype.Environment:
                                        var newMEE = new MsbEventEnvironment();
                                        newMEE.Read(bin);
                                        Events.EnvironmentEvents.Add(newMEE);
                                        break;
                                    case EventParamSubtype.Generators:
                                        var newMEG = new MsbEventGenerator();
                                        newMEG.Read(bin);
                                        Events.Generators.Add(newMEG);
                                        break;
                                    case EventParamSubtype.Lights:
                                        var newMEL = new MsbEventLight();
                                        newMEL.Read(bin);
                                        Events.Lights.Add(newMEL);
                                        break;
                                    case EventParamSubtype.MapOffset:
                                        var newMEMO = new MsbEventMapOffset();
                                        newMEMO.Read(bin);
                                        Events.MapOffsets.Add(newMEMO);
                                        break;
                                    case EventParamSubtype.Navimesh:
                                        var newMEN = new MsbEventNavimesh();
                                        newMEN.Read(bin);
                                        Events.Navimeshes.Add(newMEN);
                                        break;
                                    case EventParamSubtype.ObjActs:
                                        var newMEOA = new MsbEventObjAct();
                                        newMEOA.Read(bin);
                                        Events.ObjActs.Add(newMEOA);
                                        break;
                                    case EventParamSubtype.SFX:
                                        var newMESFX = new MsbEventSFX();
                                        newMESFX.Read(bin);
                                        Events.SFXs.Add(newMESFX);
                                        break;
                                    case EventParamSubtype.Sounds:
                                        var newMES = new MsbEventSound();
                                        newMES.Read(bin);
                                        Events.Sounds.Add(newMES);
                                        break;
                                    case EventParamSubtype.SpawnPoints:
                                        var newMESP = new MsbEventSpawnPoint();
                                        newMESP.Read(bin);
                                        Events.SpawnPoints.Add(newMESP);
                                        break;
                                    case EventParamSubtype.Treasures:
                                        var newMET = new MsbEventTreasure();
                                        newMET.Read(bin);
                                        Events.Treasures.Add(newMET);
                                        break;
                                    case EventParamSubtype.WindSFX:
                                        var newMEWS = new MsbEventWindSFX();
                                        newMEWS.Read(bin);
                                        Events.WindSFXs.Add(newMEWS);
                                        break;
                                }
                                break;
                            case MsbSectorFormat.POINT_PARAM_ST:
                                var regionType = PointParamSubtype.Points;
                                bin.StepIn(bin.Position + 0x0C);
                                {
                                    regionType = (PointParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (regionType)
                                {
                                    case PointParamSubtype.Points:
                                        var newMRP = new MsbRegionPoint();
                                        newMRP.Read(bin);
                                        Regions.Points.Add(newMRP);
                                        break;
                                    case PointParamSubtype.Spheres:
                                        var newMRS = new MsbRegionSphere();
                                        newMRS.Read(bin);
                                        Regions.Spheres.Add(newMRS);
                                        break;
                                    case PointParamSubtype.Cylinders:
                                        var newMRC = new MsbRegionCylinder();
                                        newMRC.Read(bin);
                                        Regions.Cylinders.Add(newMRC);
                                        break;
                                    case PointParamSubtype.Boxes:
                                        var newMRB = new MsbRegionBox();
                                        newMRB.Read(bin);
                                        Regions.Boxes.Add(newMRB);
                                        break;
                                }
                                break;

                            case MsbSectorFormat.PARTS_PARAM_ST:
                                var partsType = PartsParamSubtype.MapPieces;
                                bin.StepIn(bin.Position + 0x04);
                                {
                                    partsType = (PartsParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (partsType)
                                {
                                    case PartsParamSubtype.Collisions:
                                        var newMPC = new MsbPartsCollision();
                                        newMPC.Read(bin);
                                        Parts.Collisions.Add(newMPC);
                                        break;
                                    case PartsParamSubtype.MapPieces:
                                        var newMPMP = new MsbPartsMapPiece();
                                        newMPMP.Read(bin);
                                        Parts.MapPieces.Add(newMPMP);
                                        break;
                                    case PartsParamSubtype.Navimeshes:
                                        var newMPN = new MsbPartsNavimesh();
                                        newMPN.Read(bin);
                                        Parts.Navimeshes.Add(newMPN);
                                        break;
                                    case PartsParamSubtype.NPCs:
                                        var newMPNPC = new MsbPartsNPC();
                                        newMPNPC.Read(bin);
                                        Parts.NPCs.Add(newMPNPC);
                                        break;
                                    case PartsParamSubtype.Objects:
                                        var newMPO = new MsbPartsObject();
                                        newMPO.Read(bin);
                                        Parts.Objects.Add(newMPO);
                                        break;
                                    case PartsParamSubtype.Players:
                                        var newMPP = new MsbPartsPlayer();
                                        newMPP.Read(bin);
                                        Parts.Players.Add(newMPP);
                                        break;
                                    case PartsParamSubtype.UnusedCollisions:
                                        var newMPUC = new MsbPartsCollisionUnused();
                                        newMPUC.Read(bin);
                                        Parts.UnusedCollisions.Add(newMPUC);
                                        break;
                                    case PartsParamSubtype.UnusedNPCs:
                                        var newMPUNPC = new MsbPartsNPCUnused();
                                        newMPUNPC.Read(bin);
                                        Parts.UnusedNPCs.Add(newMPUNPC);
                                        break;
                                    case PartsParamSubtype.UnusedObjects:
                                        var newMPUO = new MsbPartsObjectUnused();
                                        newMPUO.Read(bin);
                                        Parts.UnusedObjects.Add(newMPUO);
                                        break;
                                }

                                break;
                        }
                    }
                    bin.StepOut();
                }

                if (currentSectorFormat == MsbSectorFormat.PARTS_PARAM_ST)
                {
                    return;
                }



                int sectionEndOffset = bin.ReadInt32();

                bin.Position = sectionEndOffset + 4;

                //if (sectionEndOffset == 0)
                //{
                //    //LAST SECTION YEET
                //    return;
                //}
                //else
                //{
                //    bin.Position = sectionEndOffset + 4;
                //}



            }
            while (true); //Maybe double check here so it doesnt keep reading on dumb files


        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            var LIST_EVENT = Events.GetAllEventsInOrder();
            var LIST_REGION = Regions.GetAllRegionsInOrder();
            var LIST_PARTS = Parts.GetAllPartsInOrder();

            bin.Write(Unknown1);


            bin.Placeholder("MODEL_PARAM_ST");
            bin.Write(Models.Count + 1);
            for (int i = 0; i < Models.Count; i++)
            {
                bin.Placeholder($"MODEL_PARAM_ST_{i}");
            }
            bin.Placeholder("MODEL_PARAM_ST_END");
            //bin.Write((int)0);

            bin.Replace("MODEL_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("MODEL_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < Models.Count; i++)
            {
                bin.Replace($"MODEL_PARAM_ST_{i}", (int)bin.Position);
                Models[i].Write(bin);
            }
            bin.Replace("MODEL_PARAM_ST_END", (int)bin.Position);
            bin.Write((int)0);


            


            bin.Placeholder("EVENT_PARAM_ST");
            bin.Write(LIST_EVENT.Count + 1);
            for (int i = 0; i < LIST_EVENT.Count; i++)
            {
                bin.Placeholder($"EVENT_PARAM_ST_{i}");
            }
            bin.Placeholder("EVENT_PARAM_ST_END");
            //bin.Write((int)0);

            bin.Replace("EVENT_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("EVENT_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < LIST_EVENT.Count; i++)
            {
                bin.Replace($"EVENT_PARAM_ST_{i}", (int)bin.Position);
                LIST_EVENT[i].Write(bin);
            }
            bin.Replace("EVENT_PARAM_ST_END", (int)bin.Position);
            bin.Write((int)0);






            bin.Placeholder("POINT_PARAM_ST");
            bin.Write(LIST_REGION.Count + 1);
            for (int i = 0; i < LIST_REGION.Count; i++)
            {
                bin.Placeholder($"POINT_PARAM_ST_{i}");
            }
            bin.Placeholder("POINT_PARAM_ST_END");
            //bin.Write((int)0);

            bin.Replace("POINT_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("POINT_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < LIST_REGION.Count; i++)
            {
                bin.Replace($"POINT_PARAM_ST_{i}", (int)bin.Position);
                LIST_REGION[i].Write(bin);
            }
            bin.Replace("POINT_PARAM_ST_END", (int)bin.Position);
            bin.Write((int)0);



            bin.Placeholder("PARTS_PARAM_ST");
            bin.Write(LIST_PARTS.Count + 1);
            for (int i = 0; i < LIST_PARTS.Count; i++)
            {
                bin.Placeholder($"PARTS_PARAM_ST_{i}");
            }
            //NO END MARKER BECAUSE LAST SECTION
            bin.Write((int)0);

            bin.Replace("PARTS_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("PARTS_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < LIST_PARTS.Count; i++)
            {
                bin.Replace($"PARTS_PARAM_ST_{i}", (int)bin.Position);
                LIST_PARTS[i].Write(bin);
            }
            bin.Write((int)0);
        }
    }
}

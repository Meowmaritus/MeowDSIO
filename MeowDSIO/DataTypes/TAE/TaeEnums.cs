﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.TAE
{
    public enum TimeActEventType
    {
        DoCommand = 000,
        DoBehaviorAtk = 001,
        DoBehaviorBullet = 002,
        DoBehaviorCommon = 005,
        Type008 = 008,
        SetEventEditorColors = 016,
        Type024 = 024,
        Type032 = 032,
        Type033 = 033,
        Type064 = 064,
        Type065 = 065,
        ApplySpEffect = 066,
        ApplySpEffectB = 067,
        SpawnSFXAtDmy = 096,
        SpawnSFXAtDmyB = 099,
        SpawnSFXAtDmyC = 100,
        DoSomethingToSFXSlot = 101,
        SpawnSFXAtDmyD = 104,
        SpawnSFXAtDmyAndFollow = 108,
        SpawnSFXAtDmyE = 109,
        DoSomethingToSFXSlotB = 110,
        Type112 = 112,
        Type114 = 114,
        Type115 = 115,
        Type116 = 116,
        WeaponTrail = 118,
        Type119 = 119,
        Type120 = 120,
        Type121 = 121,
        SoundBody = 128,
        SoundDmyPoly = 129,
        SoundOther = 130,
        CameraShakeSpecific = 144,
        CameraShakeGeneric = 145,
        SetChrOpacity = 193,
        SetChrTrackingSpeed = 224,
        Type225 = 225,
        Type226 = 226,
        Type228 = 228,
        Type229 = 229,
        Type231 = 231,
        Type232 = 232,
        Type233 = 233,
        Type236 = 236,
        Type300 = 300,
        Type301 = 301,
        Type302 = 302,
        PlayAnimation = 303,
        DoThrowDamage = 304,
        Type306 = 306,
        Type307 = 307,
        Type308 = 308,
        Type401 = 401,
        Type500 = 500,
    }

    public enum TaeGeneralCommandType : short
    {
        Command0 = 0,
        Command1 = 1,
        Command3 = 3,
        AllowCancelByRightHandAtk = 4,
        GetParriedWindow = 5,
        LockCharacterRotation = 7,
        InvincibilityFrames = 8,
        Command9 = 9,
        Command10 = 10,
        AllowCancelByTurningCamera = 11,
        Command12 = 12,
        Command13 = 13,
        Command15 = 15,
        AllowCancelByLeftHandAtk = 16,
        Command18 = 18,
        Command19 = 19,
        Command20 = 20,
        Command21 = 21,
        AllowCancelByGuard = 22,
        AllowCancelByComboAtk = 23,
        Command24 = 24,
        Command25 = 25,
        AllowCancelByDodge = 26,
        Command27 = 27,
        Command28 = 28,
        Command29 = 29,
        Command30 = 30,
        AllowCancelByGoodsUse = 31,
        AllowCancelByTwoHandingWeapon = 32,
        Command37 = 37,
        Command38 = 38,
        Command39 = 39,
        Command41 = 41,
        Command42 = 42,
        Command43 = 43,
        Command44 = 44,
        Command45 = 45,
        Command46 = 46,
        DisableBeingLockedOnto = 49,
        Command50 = 50,
        Command51 = 51,
        Command53 = 53,
        Command54 = 54,
        Command55 = 55,
        Command56 = 56,
        Command57 = 57,
        Command59 = 59,
        Command60 = 60,
        Command61 = 61,
        Command62 = 62,
        Command63 = 63,
        Command64 = 64,
        Command65 = 65,
        Command66 = 66,
        Command67 = 67,
        AllowCancelByMashingTriggersDuringThrow = 68,
        Command69 = 69,
        Command70 = 70,
        Command71 = 71,
        Command72 = 72,
        Command73 = 73,
        Command74 = 74,
        Command75 = 75,
        Command76 = 76,
        Command78 = 78,
        Command79 = 79,
        Command80 = 80,
        Command81 = 81,
        Command82 = 82,
        Command83 = 83,
        Command84 = 84,
        Command85 = 85,
        Command86 = 86,
        EnableAnimationCancelling = 87,
        Command88 = 88,
        Command89 = 89,
        Command90 = 90,
        Command91 = 91,
        Command92 = 92,
        Command94 = 94,
        Command95 = 95,
        Command97 = 97,
    }
}

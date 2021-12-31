using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor.Expressions
{
    using EMVD = ExpressionMemberValueDescription;
    using EMVDS = ExpressionMemberValueDescriptions;
    using EMOP = ExpressionMemberObjectProperty;

    public sealed class ExpressionMemberObjectProperty
    {
        /// <summary> Type of object this is a property of </summary>
        public EMVD ObjectDescription { get; private set; }

        /// <summary> Name of property as it appears in the XML </summary>
        public string Name { get; private set; }

        /// <summary> Type of this property's value </summary>
        public EMVD ValueDescription { get; private set; }

        /// <summary> True if this property cannot be set </summary>
        public Boolean IsReadOnly { get; private set; }

        /// <summary> Suffix string to show after value when displayed </summary>
        public string Units { get; private set; }

        public string ObsoletedByName { get; private set; }

        public ExpressionMemberObjectProperty(
            ExpressionMemberValueDescription objectDescription,
            string name,
            ExpressionMemberValueDescription valueDescription,
            string units = null,
            string obsoletedByName = null, 
            Boolean isReadOnly = false)
        {
            ObjectDescription = objectDescription;
            Name = name;
            ValueDescription = valueDescription;
            IsReadOnly = isReadOnly;
            Units = units;
            ObsoletedByName = obsoletedByName;
        }

        private static readonly string PercentUnits = "%";
        private static readonly string RadiansUnits = "radians";

        public static readonly List<EMOP> ObjectProperties = new List<EMOP>()
        {
            // Global properties.
            new EMOP(null, "nonPlayerSpeed",          EMVDS.Int_40_300, PercentUnits),
            new EMOP(null, "nebulaIsOpaque",          EMVDS.Bool_Yes_No),
            new EMOP(null, "sensorSetting",           EMVDS.SensorRange),
            new EMOP(null, "nonPlayerShield",         EMVDS.Int_40_300, PercentUnits),
            new EMOP(null, "nonPlayerWeapon",         EMVDS.Int_40_300, PercentUnits),
            new EMOP(null, "playerWeapon",            EMVDS.Int_40_300, PercentUnits),
            new EMOP(null, "playerShields",           EMVDS.Int_40_300, PercentUnits),
            new EMOP(null, "coopAdjustmentValue",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(null, "musicObjectMasterVolume", EMVDS.Flt_NegInf_PosInf),
            new EMOP(null, "commsObjectMasterVolume", EMVDS.Flt_NegInf_PosInf),
            new EMOP(null, "soundFXVolume",           EMVDS.Flt_NegInf_PosInf),
            new EMOP(null, "gameTimeLimit",           EMVDS.Flt_NegInf_PosInf),
            new EMOP(null, "networkTickSpeed",        EMVDS.Flt_NegInf_PosInf, "ms"),

            // Properties on all named objects.
            new EMOP(EMVDS.NameAll, "positionX",      EMVDS.Flt_0_100k, "m west of right edge"),
            new EMOP(EMVDS.NameAll, "positionY",      EMVDS.Flt_Minus100k_100k, "m"),
            new EMOP(EMVDS.NameAll, "positionZ",      EMVDS.Flt_0_100k, "m south of top edge"),
            new EMOP(EMVDS.NameAll, "deltaX",         EMVDS.Flt_Minus100k_100k),
            new EMOP(EMVDS.NameAll, "deltaY",         EMVDS.Flt_Minus100k_100k),
            new EMOP(EMVDS.NameAll, "deltaZ",         EMVDS.Flt_Minus100k_100k),
            new EMOP(EMVDS.NameAll, "angle",          EMVDS.Flt_NegInf_PosInf, "radians clockwise from south"),
            new EMOP(EMVDS.NameAll, "pitch",          EMVDS.Flt_NegInf_PosInf, RadiansUnits),
            new EMOP(EMVDS.NameAll, "roll",           EMVDS.Flt_NegInf_PosInf, RadiansUnits),
            new EMOP(EMVDS.NameAll, "sideValue",      EMVDS.SideValue),
            new EMOP(EMVDS.NameAll, "isTagged",       EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameAll, "tagOwnerSide",   EMVDS.SideValue),

            // GenericMesh properties.
            new EMOP(EMVDS.NameGenericMesh, "blocksShotFlag", EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameGenericMesh, "pushRadius",     EMVDS.Flt_NegInf_PosInf, "m"),
            new EMOP(EMVDS.NameGenericMesh, "pitchDelta",     EMVDS.Flt_NegInf_PosInf, RadiansUnits),
            new EMOP(EMVDS.NameGenericMesh, "rollDelta",      EMVDS.Flt_NegInf_PosInf, RadiansUnits),
            new EMOP(EMVDS.NameGenericMesh, "angleDelta",     EMVDS.Flt_NegInf_PosInf, RadiansUnits),
            new EMOP(EMVDS.NameGenericMesh, "artScale",       EMVDS.Flt_NegInf_PosInf),

            // Station properties.
            new EMOP(EMVDS.NameStation, "shieldState",         EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameStation, "canBuild",            EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameStation, "canLaunchFighters",   EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameStation, "canShoot",            EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameStation, "missileStoresHoming", EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresNuke",   EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresMine",   EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresECM",    EMVDS.Int_0_PosInf, null, "missileStoresEMP"),
            new EMOP(EMVDS.NameStation, "missileStoresEMP",    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresPShock", EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresBeacon", EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresProbe",  EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NameStation, "missileStoresTag",    EMVDS.Int_0_PosInf),

            // Shielded ship (player, enemy, neutral) properties.
            new EMOP(EMVDS.NameShip, "throttle",                EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "steering",                EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "topSpeed",                EMVDS.Flt_NegInf_PosInf, "hm/s"),
            new EMOP(EMVDS.NameShipOrMonster, "turnRate",       EMVDS.Flt_NegInf_PosInf, "radians/cycle"),
            new EMOP(EMVDS.NameShip, "shieldStateFront",        EMVDS.Int_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldMaxStateFront",     EMVDS.Int_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldStateBack",         EMVDS.Int_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldMaxStateBack",      EMVDS.Int_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldsOn",               EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameShip, "triggersMines",           EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameShip, "systemDamageBeam",        EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageTorpedo",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageTactical",    EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageTurning",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageImpulse",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageWarp",        EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageFrontShield", EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "systemDamageBackShield",  EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldBandStrength0",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldBandStrength1",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldBandStrength2",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldBandStrength3",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameShip, "shieldBandStrength4",     EMVDS.Flt_NegInf_PosInf),

            // Enemy properties.
            new EMOP(EMVDS.NameEnemy, "targetPointX",       EMVDS.Flt_0_100k, "m west of right edge"),
            new EMOP(EMVDS.NameEnemy, "targetPointY",       EMVDS.Flt_Minus100k_100k, "m"),
            new EMOP(EMVDS.NameEnemy, "targetPointZ",       EMVDS.Flt_0_100k, "m south of top edge"),
            new EMOP(EMVDS.NameEnemy, "hasSurrendered",     EMVDS.Bool_Yes_No),
            new EMOP(EMVDS.NameEnemy, "tauntImmunityIndex", EMVDS.tII1_3),
            new EMOP(EMVDS.NameEnemy, "eliteAIType",        EMVDS.EliteAIType),
            new EMOP(EMVDS.NameEnemy, "eliteAbilityBits",   EMVDS.EliteAbilityBits, null, "specialAbilityBits", true),
            new EMOP(EMVDS.NameEnemy, "specialAbilityBits", EMVDS.EliteAbilityBits, null, null, true),
            new EMOP(EMVDS.NameEnemy, "eliteAbilityState",  EMVDS.EliteAbilityBits, null, "specialAbilityState", true),
            new EMOP(EMVDS.NameEnemy, "specialAbilityState",EMVDS.EliteAbilityBits, null, null, true),
            new EMOP(EMVDS.NameEnemy, "surrenderChance",    EMVDS.Int_0_100, "%"),

            // Monster properties.
            new EMOP(EMVDS.NameMonster, "age",              EMVDS.MonsterAge),
            new EMOP(EMVDS.NameMonster, "health",           EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameMonster, "maxHealth",        EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameMonster, "size",             EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NameMonster, "speed",            EMVDS.Flt_NegInf_PosInf, "hm/s"),

            // Neutral properties.
            new EMOP(EMVDS.NameNeutral, "exitPointX",       EMVDS.Flt_0_100k, "m west of right edge"),
            new EMOP(EMVDS.NameNeutral, "exitPointY",       EMVDS.Flt_Minus100k_100k, "m"),
            new EMOP(EMVDS.NameNeutral, "exitPointZ",       EMVDS.Flt_0_100k, "m south of top edge"),
            new EMOP(EMVDS.NameNeutral, "willAcceptCommsOrders", EMVDS.Bool_Yes_No),

            // Player properties.
            new EMOP(EMVDS.NamePlayer, "countHoming",                 EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countNuke",                   EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countMine",                   EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countECM",                    EMVDS.Int_0_PosInf, null, "countEMP"),
            new EMOP(EMVDS.NamePlayer, "countEMP",                    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countPshock",                 EMVDS.Int_0_PosInf, null, "countShk"),
            new EMOP(EMVDS.NamePlayer, "countShk",                    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countBea",                    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countPro",                    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "countTag",                    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "energy",                      EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "warpState",                   EMVDS.Int_0_4),
            new EMOP(EMVDS.NamePlayer, "currentRealSpeed",            EMVDS.Flt_NegInf_PosInf, null, null, true),
            new EMOP(EMVDS.NamePlayer, "pirateRepWithStations",       EMVDS.Int_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "totalCoolant",                EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantBeam",        EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantTorpedo",     EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantTactical",    EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantTurning",     EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantImpulse",     EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantWarp",        EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantFrontShield", EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurCoolantBackShield",  EMVDS.Int_0_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatBeam",           EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatTorpedo",        EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatTactical",       EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatTurning",        EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatImpulse",        EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatWarp",           EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatFrontShield",    EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurHeatBackShield",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyBeam",         EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyTorpedo",      EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyTactical",     EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyTurning",      EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyImpulse",      EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyWarp",         EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyFrontShield",  EMVDS.Flt_NegInf_PosInf),
            new EMOP(EMVDS.NamePlayer, "systemCurEnergyBackShield",   EMVDS.Flt_NegInf_PosInf),
        };

        public static ExpressionMemberObjectProperty Find(string name)
        {
            string lowerName = name.ToLower();
            foreach (var property in ObjectProperties)
            {
                if (property.Name.ToLower() == lowerName)
                {
                    return property;
                }
            }
            return null;
        }
    }
}

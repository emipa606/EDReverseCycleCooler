using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler
{
    // Token: 0x02000002 RID: 2
    [StaticConstructorOnStartup]
    public class Building_ReverseCycleCooler : Building_Cooler
    {
        // Token: 0x04000005 RID: 5
        private const float HeatOutputMultiplier = 1.25f;

        // Token: 0x04000006 RID: 6
        private const float EfficiencyLossPerDegreeDifference = 0.007692308f;

        // Token: 0x04000001 RID: 1
        private static readonly Texture2D UI_ROTATE_RIGHT = ContentFinder<Texture2D>.Get("UI/RotRight");

        // Token: 0x04000002 RID: 2
        private static readonly Texture2D UI_TEMPERATURE_COOLING =
            ContentFinder<Texture2D>.Get("UI/Temperature_Cooling");

        // Token: 0x04000003 RID: 3
        private static readonly Texture2D UI_TEMPERATURE_HEATING =
            ContentFinder<Texture2D>.Get("UI/Temperature_Heating");

        // Token: 0x04000004 RID: 4
        private static readonly Texture2D UI_TEMPERATURE_AUTO = ContentFinder<Texture2D>.Get("UI/Temperature_Auto");

        // Token: 0x04000007 RID: 7
        private enumCoolerMode m_Mode;

        // Token: 0x06000002 RID: 2 RVA: 0x0000209D File Offset: 0x0000029D

        // Token: 0x06000003 RID: 3 RVA: 0x000020A8 File Offset: 0x000002A8
        public override void TickRare()
        {
            if (!compPowerTrader.PowerOn)
            {
                return;
            }

            var intVec = Position + IntVec3.South.RotatedBy(Rotation);
            var intVec2 = Position + IntVec3.North.RotatedBy(Rotation);
            var idle = false;
            if (!intVec2.Impassable(Map) && !intVec.Impassable(Map))
            {
                var temperature = intVec2.GetTemperature(Map);
                var temperature2 = intVec.GetTemperature(Map);
                var cooling = true;
                switch (m_Mode)
                {
                    case enumCoolerMode.Heating:
                        cooling = false;
                        break;
                    case enumCoolerMode.Auto:
                        cooling = temperature > compTempControl.targetTemperature;
                        break;
                }

                float num3;
                float num4;
                if (cooling)
                {
                    var num = temperature - temperature2;
                    if (temperature - 40.0 > num)
                    {
                        num = temperature - 40f;
                    }

                    var num2 = (float) (1.0 - (num * 0.0076923076923076927));
                    if (num2 < 0.0)
                    {
                        num2 = 0f;
                    }

                    num3 = (float) (compTempControl.Props.energyPerSecond * (double) num2 * 4.16666650772095);
                    num4 = GenTemperature.ControlTemperatureTempChange(intVec, Map, num3,
                        compTempControl.targetTemperature);
                    idle = !Mathf.Approximately(num4, 0f);
                }
                else
                {
                    var num5 = temperature - temperature2;
                    if (temperature + 40.0 > num5)
                    {
                        num5 = temperature + 40f;
                    }

                    var num6 = (float) (1.0 - (num5 * 0.0076923076923076927));
                    if (num6 < 0.0)
                    {
                        num6 = 0f;
                    }

                    num3 = (float) ((double) compTempControl.Props.energyPerSecond * -(float) (double) num6 *
                                    4.16666650772095);
                    num4 = GenTemperature.ControlTemperatureTempChange(intVec, Map, num3,
                        compTempControl.targetTemperature);
                    idle = !Mathf.Approximately(num4, 0f);
                }

                if (idle)
                {
                    intVec2.GetRoom(Map).Temperature -= num4;
                    GenTemperature.PushHeat(intVec, Map, (float) (num3 * 1.25));
                }
            }

            var props = compPowerTrader.Props;
            if (idle)
            {
                compPowerTrader.PowerOutput = -props.basePowerConsumption;
            }
            else
            {
                compPowerTrader.PowerOutput =
                    -props.basePowerConsumption * compTempControl.Props.lowPowerConsumptionFactor;
            }

            compTempControl.operatingAtHighPower = idle;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x0000235C File Offset: 0x0000055C
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            yield return new Command_Action
            {
                action = ChangeRotation,
                icon = UI_ROTATE_RIGHT,
                defaultLabel = "Rotate",
                defaultDesc = "Rotates",
                activateSound = SoundDef.Named("Click")
            };
            switch (m_Mode)
            {
                case enumCoolerMode.Cooling:
                    yield return new Command_Action
                    {
                        action = ChangeMode,
                        icon = UI_TEMPERATURE_COOLING,
                        defaultLabel = "Cooling",
                        defaultDesc = "Cooling",
                        activateSound = SoundDef.Named("Click")
                    };
                    break;
                case enumCoolerMode.Heating:
                    yield return new Command_Action
                    {
                        action = ChangeMode,
                        icon = UI_TEMPERATURE_HEATING,
                        defaultLabel = "Heating",
                        defaultDesc = "Heating",
                        activateSound = SoundDef.Named("Click")
                    };
                    break;
                case enumCoolerMode.Auto:
                    yield return new Command_Action
                    {
                        action = ChangeMode,
                        icon = UI_TEMPERATURE_AUTO,
                        defaultLabel = "Auto",
                        defaultDesc = "Auto",
                        activateSound = SoundDef.Named("Click")
                    };
                    break;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x0000236C File Offset: 0x0000056C
        public void ChangeRotation()
        {
            Rotation = new Rot4((Rotation.AsInt + 2) % 4);

            Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things, true, false);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000023CC File Offset: 0x000005CC
        public void ChangeMode()
        {
            switch (m_Mode)
            {
                case enumCoolerMode.Cooling:
                    m_Mode = enumCoolerMode.Heating;
                    return;
                case enumCoolerMode.Heating:
                    m_Mode = enumCoolerMode.Auto;
                    return;
                case enumCoolerMode.Auto:
                    m_Mode = enumCoolerMode.Cooling;
                    break;
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002400 File Offset: 0x00000600
        public override string GetInspectString()
        {
            var stringBuilder = new StringBuilder();
            switch (m_Mode)
            {
                case enumCoolerMode.Cooling:
                    stringBuilder.AppendLine("Mode: Cooling");
                    break;
                case enumCoolerMode.Heating:
                    stringBuilder.AppendLine("Mode: Heating");
                    break;
                case enumCoolerMode.Auto:
                    stringBuilder.AppendLine("Mode: Auto");
                    break;
            }

            stringBuilder.Append(base.GetInspectString());
            return stringBuilder.ToString();
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002468 File Offset: 0x00000668
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref m_Mode, "m_Mode");
        }
    }
}
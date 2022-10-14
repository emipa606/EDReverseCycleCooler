using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler;

[StaticConstructorOnStartup]
public class Building_ReverseCycleCooler : Building_Cooler
{
    private const float HeatOutputMultiplier = 1.25f;
    private const float EfficiencyLossPerDegreeDifference = 1f / 130f;

    private static readonly Texture2D UI_ROTATE_RIGHT = ContentFinder<Texture2D>.Get("UI/RotRight");
    private static readonly Texture2D UI_TEMPERATURE_COOLING = ContentFinder<Texture2D>.Get("UI/Temperature_Cooling");
    private static readonly Texture2D UI_TEMPERATURE_HEATING = ContentFinder<Texture2D>.Get("UI/Temperature_Heating");
    private static readonly Texture2D UI_TEMPERATURE_AUTO = ContentFinder<Texture2D>.Get("UI/Temperature_Auto");

    private enumCoolerMode m_Mode;

    public override void TickRare()
    {
        if (!compPowerTrader.PowerOn)
        {
            return;
        }

        var coldSide = Position + IntVec3.South.RotatedBy(Rotation); // formerly known as intVect
        var hotSide = Position + ReplaceStuffFix.adjustedNorth(this).RotatedBy(Rotation); // formerly known as intVect2

        var idle = false;
        if (!hotSide.Impassable(Map) && !coldSide.Impassable(Map))
        {
            var temperatureOnHot = hotSide.GetTemperature(Map); // formerly known as temperature
            var temperatureOnCold = coldSide.GetTemperature(Map); // formerly known as temperature2
            var cooling = true;

            switch (m_Mode)
            {
                case enumCoolerMode.Heating:
                    cooling = false;
                    break;
                case enumCoolerMode.Auto:
                    cooling = temperatureOnHot > compTempControl.targetTemperature;
                    break;
            }

            float energyLimit; // formerly known as num3

            var tempDif = temperatureOnHot - temperatureOnCold; // formerly known as num & num5

            if (cooling)
            {
                if (temperatureOnHot - 40.0 > tempDif)
                {
                    tempDif = temperatureOnHot - 40f;
                }

                var energyCost = 1f - (tempDif * EfficiencyLossPerDegreeDifference); // formerly known as num2
                if (energyCost < 0.0)
                {
                    energyCost = 0f;
                }

                energyLimit = compTempControl.Props.energyPerSecond * energyCost * 4.16666651f;
            }
            else
            {
                if (temperatureOnHot + 40.0 > tempDif)
                {
                    tempDif = temperatureOnHot + 40f;
                }

                var energyCost = 1f - (tempDif * EfficiencyLossPerDegreeDifference); // formerly known as num6
                if (energyCost < 0.0)
                {
                    energyCost = 0f;
                }

                energyLimit = compTempControl.Props.energyPerSecond * -energyCost * 4.16666651f;
            }

            var newTemp = GenTemperature.ControlTemperatureTempChange(coldSide, Map,
                energyLimit, // formerly known as num4
                compTempControl.targetTemperature);
            idle = !Mathf.Approximately(newTemp, 0f);

            if (idle)
            {
                hotSide.GetRoom(Map).Temperature -= newTemp;
                GenTemperature.PushHeat(coldSide, Map, energyLimit * HeatOutputMultiplier);
            }
        }

        var props = compPowerTrader.Props;
        if (idle)
        {
            compPowerTrader.PowerOutput = -props.PowerConsumption;
        }
        else
        {
            compPowerTrader.PowerOutput =
                -props.PowerConsumption * compTempControl.Props.lowPowerConsumptionFactor;
        }

        compTempControl.operatingAtHighPower = idle;
    }

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

    public void ChangeRotation()
    {
        Rotation = new Rot4((Rotation.AsInt + 2) % 4);

        if (ReplaceStuffFix.isWide(def.defName)) // So you can rotate the wide cooler
        {
            Position += IntVec3.South.RotatedBy(Rotation);
        }

        Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things, true, false);
    }

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

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref m_Mode, "m_Mode");
    }
}
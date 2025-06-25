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

    private static readonly Texture2D uiRotateRight = ContentFinder<Texture2D>.Get("UI/RotRight");
    private static readonly Texture2D uiTemperatureCooling = ContentFinder<Texture2D>.Get("UI/Temperature_Cooling");
    private static readonly Texture2D uiTemperatureHeating = ContentFinder<Texture2D>.Get("UI/Temperature_Heating");
    private static readonly Texture2D uiTemperatureAuto = ContentFinder<Texture2D>.Get("UI/Temperature_Auto");

    private EnumCoolerMode mMode;

    public override void TickRare()
    {
        if (!compPowerTrader.PowerOn)
        {
            return;
        }

        var coldSide = Position + IntVec3.South.RotatedBy(Rotation); // formerly known as intVect
        var hotSide = Position + ReplaceStuffFix.AdjustedNorth(this).RotatedBy(Rotation); // formerly known as intVect2

        var idle = false;
        if (!hotSide.Impassable(Map) && !coldSide.Impassable(Map))
        {
            var temperatureOnHot = hotSide.GetTemperature(Map); // formerly known as temperature
            var temperatureOnCold = coldSide.GetTemperature(Map); // formerly known as temperature2
            var cooling = true;

            switch (mMode)
            {
                case EnumCoolerMode.Heating:
                    cooling = false;
                    break;
                case EnumCoolerMode.Auto:
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
            action = changeRotation,
            icon = uiRotateRight,
            defaultLabel = "Rotate",
            defaultDesc = "Rotates",
            activateSound = SoundDef.Named("Click")
        };
        switch (mMode)
        {
            case EnumCoolerMode.Cooling:
                yield return new Command_Action
                {
                    action = changeMode,
                    icon = uiTemperatureCooling,
                    defaultLabel = "Cooling",
                    defaultDesc = "Cooling",
                    activateSound = SoundDef.Named("Click")
                };
                break;
            case EnumCoolerMode.Heating:
                yield return new Command_Action
                {
                    action = changeMode,
                    icon = uiTemperatureHeating,
                    defaultLabel = "Heating",
                    defaultDesc = "Heating",
                    activateSound = SoundDef.Named("Click")
                };
                break;
            case EnumCoolerMode.Auto:
                yield return new Command_Action
                {
                    action = changeMode,
                    icon = uiTemperatureAuto,
                    defaultLabel = "Auto",
                    defaultDesc = "Auto",
                    activateSound = SoundDef.Named("Click")
                };
                break;
        }
    }

    private void changeRotation()
    {
        Rotation = new Rot4((Rotation.AsInt + 2) % 4);

        if (ReplaceStuffFix.IsWide(def.defName)) // So you can rotate the wide cooler
        {
            Position += IntVec3.South.RotatedBy(Rotation);
        }

        Map.mapDrawer.MapMeshDirty(Position, MapMeshFlagDefOf.Things, true, false);
    }

    private void changeMode()
    {
        switch (mMode)
        {
            case EnumCoolerMode.Cooling:
                mMode = EnumCoolerMode.Heating;
                return;
            case EnumCoolerMode.Heating:
                mMode = EnumCoolerMode.Auto;
                return;
            case EnumCoolerMode.Auto:
                mMode = EnumCoolerMode.Cooling;
                break;
        }
    }

    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();
        switch (mMode)
        {
            case EnumCoolerMode.Cooling:
                stringBuilder.AppendLine("Mode: Cooling");
                break;
            case EnumCoolerMode.Heating:
                stringBuilder.AppendLine("Mode: Heating");
                break;
            case EnumCoolerMode.Auto:
                stringBuilder.AppendLine("Mode: Auto");
                break;
        }

        stringBuilder.Append(base.GetInspectString());
        return stringBuilder.ToString();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref mMode, "m_Mode");
    }
}
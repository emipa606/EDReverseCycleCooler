using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler;
/********************************************************************************************\
| This file contains everything necesary for Replace Stuff's Over wall coolers work properly |
\********************************************************************************************/

internal class ReplaceStuffFix
{
    public static bool isWide(string defName)
    {
        return defName == "Cooler_Over2W";
    }

    public static IntVec3 adjustedNorth(ThingDef thingDef)
    {
        return isWide(thingDef.defName) ? IntVec3.North * 2 : IntVec3.North;
    }

    public static IntVec3 adjustedNorth(Building building)
    {
        return isWide(building.def.defName) ? IntVec3.North * 2 : IntVec3.North;
    }

    public static IntVec3 adjustedNorth(BuildableDef buildableDef)
    {
        return isWide(buildableDef.defName) ? IntVec3.North * 2 : IntVec3.North;
    }
}
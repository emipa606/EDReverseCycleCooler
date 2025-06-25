using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler;
/********************************************************************************************\
| This file contains everything necesary for Replace Stuff's Over wall coolers work properly |
\********************************************************************************************/

internal class ReplaceStuffFix
{
    public static bool IsWide(string defName)
    {
        return defName == "Cooler_Over2W";
    }

    public static IntVec3 AdjustedNorth(ThingDef thingDef)
    {
        return IsWide(thingDef.defName) ? IntVec3.North * 2 : IntVec3.North;
    }

    public static IntVec3 AdjustedNorth(Building building)
    {
        return IsWide(building.def.defName) ? IntVec3.North * 2 : IntVec3.North;
    }

    public static IntVec3 AdjustedNorth(BuildableDef buildableDef)
    {
        return IsWide(buildableDef.defName) ? IntVec3.North * 2 : IntVec3.North;
    }
}
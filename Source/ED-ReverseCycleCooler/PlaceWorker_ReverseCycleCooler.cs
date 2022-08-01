using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler
{
    internal class PlaceWorker_ReverseCycleCooler : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            var currentMap = Find.CurrentMap;

            var coldSide = center + IntVec3.South.RotatedBy(rot); // formerly known as intVec
            var hotSide = center + ReplaceStuffFix.adjustedNorth(def).RotatedBy(rot); // formerly known as intVec2

            GenDraw.DrawFieldEdges(new List<IntVec3>
            {
                coldSide
            }, Color.magenta);

            GenDraw.DrawFieldEdges(new List<IntVec3>
            {
                hotSide
            }, GenTemperature.ColorSpotHot);

            var hotRoom = hotSide.GetRoom(currentMap); // formerly known as roomGroup
            var coldRoom = coldSide.GetRoom(currentMap); // formerly known as roomGroup2
            if (hotRoom == null || coldRoom == null)
            {
                return;
            }

            if (hotRoom == coldRoom && !hotRoom.UsesOutdoorTemperature)
            {
                GenDraw.DrawFieldEdges(hotRoom.Cells.ToList(), new Color(1f, 0.7f, 0f, 0.5f));
                return;
            }

            if (!hotRoom.UsesOutdoorTemperature)
            {
                GenDraw.DrawFieldEdges(hotRoom.Cells.ToList(), GenTemperature.ColorRoomHot);
            }

            if (!coldRoom.UsesOutdoorTemperature)
            {
                GenDraw.DrawFieldEdges(coldRoom.Cells.ToList(), Color.magenta);
            }
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            var coldSide = center + IntVec3.South.RotatedBy(rot); // formerly known as intVec
            var hotSide = center + ReplaceStuffFix.adjustedNorth(def).RotatedBy(rot); // formerly known as intVec2

            if (coldSide.Impassable(map) || hotSide.Impassable(map))
            {
                return "MustPlaceCoolerWithFreeSpaces".Translate();
            }

            var firstFrameOnCold = coldSide.GetFirstThing<Frame>(map); // formerly known as firstThing
            var firstFrameOnHot = hotSide.GetFirstThing<Frame>(map); // formerly known as firstThing2
            if (firstFrameOnCold != null && firstFrameOnCold.def.entityDefToBuild is {passability: Traversability.Impassable} ||
                firstFrameOnHot != null && firstFrameOnHot.def.entityDefToBuild is {passability: Traversability.Impassable})
            {
                return "MustPlaceCoolerWithFreeSpaces".Translate();
            }

            var firstBlueprintOnCold = coldSide.GetFirstThing<Blueprint>(map); // formerly known as firstThing3
            var firstBlueprintOnHot = hotSide.GetFirstThing<Blueprint>(map); // formerly known as firstThing4
            if (firstBlueprintOnCold != null && firstBlueprintOnCold.def.entityDefToBuild is {passability: Traversability.Impassable} ||
                firstBlueprintOnHot != null &&
                firstBlueprintOnHot.def.entityDefToBuild is {passability: Traversability.Impassable})
            {
                return "MustPlaceCoolerWithFreeSpaces".Translate();
            }

            return true;
        }
    }
}
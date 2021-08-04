using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler
{
    // Token: 0x02000004 RID: 4
    internal class PlaceWorker_ReverseCycleCooler : PlaceWorker
    {
        // Token: 0x0600000F RID: 15 RVA: 0x000024A4 File Offset: 0x000006A4
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            var currentMap = Find.CurrentMap;
            var intVec = center + IntVec3.South.RotatedBy(rot);
            var intVec2 = center + IntVec3.North.RotatedBy(rot);
            GenDraw.DrawFieldEdges(new List<IntVec3>
            {
                intVec
            }, Color.magenta);
            GenDraw.DrawFieldEdges(new List<IntVec3>
            {
                intVec2
            }, GenTemperature.ColorSpotHot);
            var roomGroup = intVec2.GetRoom(currentMap);
            var roomGroup2 = intVec.GetRoom(currentMap);
            if (roomGroup == null || roomGroup2 == null)
            {
                return;
            }

            if (roomGroup == roomGroup2 && !roomGroup.UsesOutdoorTemperature)
            {
                GenDraw.DrawFieldEdges(roomGroup.Cells.ToList(), new Color(1f, 0.7f, 0f, 0.5f));
                return;
            }

            if (!roomGroup.UsesOutdoorTemperature)
            {
                GenDraw.DrawFieldEdges(roomGroup.Cells.ToList(), GenTemperature.ColorRoomHot);
            }

            if (!roomGroup2.UsesOutdoorTemperature)
            {
                GenDraw.DrawFieldEdges(roomGroup2.Cells.ToList(), Color.magenta);
            }
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002594 File Offset: 0x00000794
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            var intVec = center + IntVec3.South.RotatedBy(rot);
            var intVec2 = center + IntVec3.North.RotatedBy(rot);
            if (intVec.Impassable(map) || intVec2.Impassable(map))
            {
                return "MustPlaceCoolerWithFreeSpaces".Translate();
            }

            var firstThing = intVec.GetFirstThing<Frame>(map);
            var firstThing2 = intVec2.GetFirstThing<Frame>(map);
            if (firstThing != null && firstThing.def.entityDefToBuild is {passability: Traversability.Impassable} ||
                firstThing2 != null &&
                firstThing2.def.entityDefToBuild is {passability: Traversability.Impassable})
            {
                return "MustPlaceCoolerWithFreeSpaces".Translate();
            }

            var firstThing3 = intVec.GetFirstThing<Blueprint>(map);
            var firstThing4 = intVec2.GetFirstThing<Blueprint>(map);
            if (firstThing3 != null && firstThing3.def.entityDefToBuild is {passability: Traversability.Impassable} ||
                firstThing4 != null &&
                firstThing4.def.entityDefToBuild is {passability: Traversability.Impassable})
            {
                return "MustPlaceCoolerWithFreeSpaces".Translate();
            }

            return true;
        }
    }
}
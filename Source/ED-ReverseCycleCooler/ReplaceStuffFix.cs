using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler
{
	/********************************************************************************************\
	| This file contains everything necesary for Replace Stuff's Over wall coolers work properly |
	\********************************************************************************************/

	internal class ReplaceStuffFix
	{
		public static bool isWide(object o)
		{
			ThingDef thingDef = o as ThingDef ?? (o as Thing)?.def;

			return thingDef == OverWallDef.Cooler_Over2W || thingDef.entityDefToBuild == OverWallDef.Cooler_Over2W;
		}

		public static IntVec3 adjustedNorth(object o)
		{
			return isWide(o) ? (IntVec3.North * 2) : IntVec3.North;
		}
	}

	[DefOf]
	public class OverWallDef
	{
		public static ThingDef Cooler_Over2W;
	}
}

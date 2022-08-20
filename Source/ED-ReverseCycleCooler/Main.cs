using Verse;

namespace EnhancedDevelopment.ReverseCycleCooler;

[StaticConstructorOnStartup]
public static class Main
{
    public static bool ReplaceStuffLoaded;

    static Main()
    {
        ReplaceStuffLoaded = ModLister.GetActiveModWithIdentifier("Uuugggg.ReplaceStuff") != null;
    }
}
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using ThePrismatic2.ThePrismatic2Code.Relics;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

public static class AncientRelicPatch
{
    [HarmonyPatch(typeof(Tanx), "BaseOptionPool",  MethodType.Getter)]
    public class TanxRelics {
        public static void Postfix(Tanx __instance, ref IEnumerable<EventOption> __result) {
            __result = __result.AddNewRelic<Tanx, Character.ThePrismatic2, FlamingSword>(__instance);
        }
    }
    
    [HarmonyPatch(typeof(Vakuu), "Pool1",  MethodType.Getter)]
    public class VakuuRelics {
        public static void Postfix(Vakuu __instance, ref IEnumerable<EventOption> __result) {
            __result = __result.AddNewRelic<Vakuu, Character.ThePrismatic2, GamblingDice>(__instance);
        }
    }
    
    [HarmonyPatch(typeof(Darv), "AllPossibleOptions",  MethodType.Getter)]
    public class DarvRelics {
        public static void Postfix(Darv __instance, ref IEnumerable<EventOption> __result) {
            __result = __result.AddNewRelic<Darv, Character.ThePrismatic2, DeadBranch2>(__instance);
            __result = __result.AddNewRelic<Darv, Character.ThePrismatic2, Brimstone2>(__instance);
        }
    }
    
    private static IEnumerable<EventOption> AddNewRelic<TAncient, TCharacter, TRelic>(this IEnumerable<EventOption> options, TAncient ancient) where TAncient : AncientEventModel where TCharacter : CharacterModel where TRelic : RelicModel {
        if (ancient is { IsMutable: true, Owner.Character: not TCharacter }) return options;
        MethodInfo? method = AccessTools.Method(typeof(TAncient), "RelicOption", generics: [typeof(TRelic)], parameters: [typeof(string), typeof(string)]);
        EventOption relic;
        try
        {
            relic = (method != null ? method.Invoke(ancient, ["INITIAL", null]) as EventOption : null) ?? throw new InvalidOperationException();
        }
        catch (TargetException)
        {
            return options;
        }
        IEnumerable<EventOption> result;
        try
        {
            // ReSharper disable once PossibleMultipleEnumeration
            result = options.ToList().Append(relic);
        }
        catch (ArgumentNullException)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            return options;
        }
        var addNewRelic = result as EventOption[] ?? result.ToArray();
        MainFile.Logger.Info($"RELIC_TEST: {ancient.GetType().ToString().Split(".").Last()} ({method == null} | {false}):\n - {String.Join("\n - ", addNewRelic.ToList())}");
        return addNewRelic;
    }
}
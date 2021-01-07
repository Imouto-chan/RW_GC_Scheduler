using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace GC_Scheduler
{
    // TickManager.TogglePaused
    [HarmonyPatch(typeof(TickManager), "TogglePaused")] // Attach to the game's TogglePaused method in the TickManager class
    internal class TickManager_TogglePaused_Patch
    {
        public static void Postfix(TimeSpeed ___curTimeSpeed) // Run after TogglePaused has executed
        {
            if (___curTimeSpeed == TimeSpeed.Paused)
                MainButtonWorker_GC_Scheduler.forceGC = true;
        }
    }

    [HarmonyPatch] // Tells Harmony that this class is a HarmonyPatch
    internal class MainTabWindow_Patch
    {
        public static IEnumerable<MethodBase> TargetMethods() // Tells Harmony to patch all methods returned by this method
        {
            foreach (Type subClass in typeof(Window).AllSubclassesNonAbstract()) // Gets all subclasses of the class MainTabWindow
            {
                // Need to exclude classes with Texture2D members as they will cause errors during Harmony Patching
                if (subClass.GetFields(BindingFlags.NonPublic | BindingFlags.Static).All(field => !field.FieldType.Equals(typeof(Texture2D)))
                    && subClass.GetFields(BindingFlags.Public | BindingFlags.Static).All(field => !field.FieldType.Equals(typeof(Texture2D))))
                    yield return subClass.GetMethod("PreOpen"); // Gets the method in the class with the name PreOpen
            }
        }

        // Postfix method is called after the patched method is ran
        public static void Postfix(Window __instance)
        {
            string classBaseType = __instance.GetType().BaseType.ToString();
            // Only run GC when these windows open
            if ((classBaseType.Contains("MainTabWindow") && ModSettings_GC_Scheduler.enableGC_OnOpenTab) 
                || (classBaseType.Contains("Dialog_NodeTree") && ModSettings_GC_Scheduler.enableGC_OnOpenMessage) 
                || (__instance.GetType().FullDescription().Contains("WorldInspectPane") && ModSettings_GC_Scheduler.enableGC_OnOpenMap))
                MainButtonWorker_GC_Scheduler.forceGC = true;
        }
    }
}

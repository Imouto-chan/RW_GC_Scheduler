using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace GC_Scheduler
{
    // TickManager.TogglePaused
    [HarmonyPatch(typeof(TickManager), "TogglePaused")] // Attach to the game's TogglePaused method in the TickManager class
    internal class TickManager_TogglePaused_Patch
    {
        public static void Postfix() // Run after TogglePaused has executed
        {
            Log.Warning("forceGC set to true");
            MainButtonWorker_GC_Scheduler.forceGC = true;
        }
    }
    
    // Window.PreOpen
    [HarmonyPatch(typeof(Window), "PreOpen")]
    internal class Window_PreOpen_Patch
    {
        public static void Postfix()
        {

        }
    }
}

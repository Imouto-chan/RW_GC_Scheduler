using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace GC_Scheduler
{
    class MainButtonWorker_GC_Scheduler : MainButtonWorker_ToggleTab
    {
        public static int updateTick = 0;
        public static float currentMemory = 0f;
        public static bool forceGC = false;

        public override float ButtonBarPercent
        {
            get
            {
                if (ModSettings_GC_Scheduler.gcScheduler)
                {
                    updateTick++;

                    if (updateTick > ModSettings_GC_Scheduler.updateInterval || forceGC) // only run on certain ticks so it does not constantly run
                    {
                        updateTick = 0;
                        currentMemory = (float)GC.GetTotalMemory(false) / 1024f / 1024f; // get current heap usage and convert into MB and then get percent of totalMemory

                        // Enable and then disable GC on next tick that memory is lowered in order to force the game to collect garbage
                        if (currentMemory >= ModSettings_GC_Scheduler.totalMemory || forceGC)
                        {
                            UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
                            forceGC = false;
                        }
                        else
                        {
                            UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
                        }

                        base.def.description = "lblCurrentMemoryUsage".Translate() + ": \n" 
                                                + currentMemory + " MB / " + ModSettings_GC_Scheduler.totalMemory + " MB"; // display memory information in tooltip/description of tab
                    }

                    return currentMemory / ModSettings_GC_Scheduler.totalMemory;
                }

                return 0f;
            }
        }
    }
}

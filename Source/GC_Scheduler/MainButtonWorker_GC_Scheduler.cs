using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace GC_Scheduler
{
    class MainButtonWorker_GC_Scheduler : MainButtonWorker_ToggleTab
    {
        public static int updateTick = 0;
        public static float currentMemory = 0f;

        public override float ButtonBarPercent
        {
            get
            {
                if (ModSettings_GC_Scheduler.gcScheduler)
                {
                    updateTick++;

                    if (updateTick > ModSettings_GC_Scheduler.updateInterval) // only run on certain ticks so it does not constantly run
                    {
                        updateTick = 0;
                        currentMemory = (float)GC.GetTotalMemory(false) / 1024f / 1024f; // get current heap usage and convert into MB and then get percent of totalMemory
                        base.def.description = "lblCurrentMemoryUsage".Translate() + ": \n" 
                                                + currentMemory + " MB / " + ModSettings_GC_Scheduler.totalMemory + " MB"; // display memory information in tooltip/description of tab
                    }

                    return currentMemory / ModSettings_GC_Scheduler.totalMemory;
                }

                return 0f;
            }
        }

        public override void DoButton(Rect rect)
        {
            if (ModSettings_GC_Scheduler.showMainTab)
                base.DoButton(rect);
            return;
        }
    }
}

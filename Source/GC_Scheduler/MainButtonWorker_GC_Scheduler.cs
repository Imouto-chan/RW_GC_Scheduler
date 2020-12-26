using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;

namespace GC_Scheduler
{
    class MainButtonWorker_GC_Scheduler : MainButtonWorker_ToggleTab
    {
        public override float ButtonBarPercent
        {
            get
            {
                if (gcScheduler)
                {
                    updateTick++;

                    if (updateTick > updateInterval) // only run on certain ticks so it does not constantly run
                    {
                        updateTick = 0;
                        currentMemory = (float)GC.GetTotalMemory(false) / 1024f / 1024f; // get current heap usage and convert into MB and then get percent of totalMemory
                        base.def.description = currentMemory + " MB / " + totalMemory + " MB"; // display memory information in tooltip/description of tab
                    }

                    return currentMemory / totalMemory;
                }

                return 0f;
            }
        }

        public static int updateTick = 0;
        public static int updateInterval = 360; // how often to run
        public static float currentMemory = 0f;
        public static float totalMemory = Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f);
        public static bool gcScheduler = false;
    }
}

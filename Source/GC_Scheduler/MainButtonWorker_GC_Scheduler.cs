﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;


namespace GC_Scheduler
{
    class MainButtonWorker_GC_Scheduler : MainButtonWorker_ToggleTab
    {
        public override float ButtonBarPercent
        {
            get
            {
                updateTick++;

                if (updateTick > updateInterval) // only run on certain ticks so it does not constantly run
                {
                    updateTick = 0;
                    float memory = (float)GC.GetTotalMemory(false) / 1024f / 1024f; // get current heap usage and convert into MB and then get percent of totalMemory
                    base.def.description = memory + " / " + totalMemory; // display memory information in tooltip/description of tab
                    return memory / totalMemory;
                }
                else 
                {
                    return 0f;
                }
            }
        }

        private static int updateTick = 0;
        private static int updateInterval = 360; // how often to run
        private static float totalMemory = 5000f;
    }
}

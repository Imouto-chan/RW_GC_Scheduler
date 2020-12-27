using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace GC_Scheduler
{
    public class ModSettings_GC_Scheduler : ModSettings
    {
        public static ModSettings_GC_Scheduler settings;
        public int updateInterval = 360; // how often to run memory updating
        public float totalMemory = Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f);
        public bool gcScheduler = false;
        public bool forcePause = true;

        public ModSettings_GC_Scheduler()
        {
            settings = this;
        }

        public override void ExposeData() // Method used to write settings so that they are saved
        {
            Scribe_Values.Look(ref updateInterval, "updateInterval", 360);
            Scribe_Values.Look(ref totalMemory, "totalMemory", Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f));
            Scribe_Values.Look(ref gcScheduler, "gcScheduler", false);
            Scribe_Values.Look(ref forcePause, "forcePause", true);
            base.ExposeData();
        }
    }
}

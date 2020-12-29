﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace GC_Scheduler
{
    class Mod_GC_Scheduler : Mod
    {
        public static ModSettings_GC_Scheduler settings;

        public Mod_GC_Scheduler(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModSettings_GC_Scheduler>(); // Load mod settings
            ModSettings_GC_Scheduler.thisMod = this;
        }

        public override string SettingsCategory()
        {
            return "GC Scheduler";
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            settings.DoSettingsWindow(rect);
        }
    }
}

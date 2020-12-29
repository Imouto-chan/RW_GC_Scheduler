using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace GC_Scheduler
{
    public class MainButton_GC_Scheduler : MainTabWindow
    {
        public MainButton_GC_Scheduler()
        {
            this.forcePause = ModSettings_GC_Scheduler.forcePause; // sets if to pause game when opened
        }

        public override Vector2 RequestedTabSize
        {
            get { return new Vector2(250f, 405f); } // Size of window tab to open
        }

        public override MainTabWindowAnchor Anchor
        {
            get { return MainTabWindowAnchor.Right; } // Anchor tab to right of screen
        }

        public override void PreClose()
        {
            // Writes settings to make sure they are saved just before menu is closed
            LoadedModManager.WriteModSettings(ModSettings_GC_Scheduler.thisMod.Content.FolderName, ModSettings_GC_Scheduler.thisMod.GetType().Name, Mod_GC_Scheduler.settings);
            base.PreClose();
        }

        public override void DoWindowContents(Rect rect)
        {
            Mod_GC_Scheduler.settings.DoInGameSettingsWindwo(rect);
            this.forcePause = ModSettings_GC_Scheduler.forcePause;
        }
    }
}

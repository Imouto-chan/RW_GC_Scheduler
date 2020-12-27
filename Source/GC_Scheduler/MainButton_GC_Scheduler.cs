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
            this.forcePause = ModSettings_GC_Scheduler.settings.forcePause; // Pause game when opened
        }

        public override Vector2 RequestedTabSize
        {
            get { return new Vector2(250f, 475f); } // Size of window tab to open
        }

        public override MainTabWindowAnchor Anchor
        {
            get { return MainTabWindowAnchor.Right; } // Anchor tab to right of screen
        }

        public override void PostClose()
        {
            base.PostClose();
        }

        public override void DoWindowContents(Rect rect)
        {
            base.DoWindowContents(rect);
            GUI.BeginGroup(rect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(rect);
            Text.Font = GameFont.Medium; // Writes following labels in medium font
            listing.Label("GCSchedulerTitle".Translate());
            Text.Font = GameFont.Small; // Writes following labels in small font

            if (UnityEngine.Scripting.GarbageCollector.GCMode == UnityEngine.Scripting.GarbageCollector.Mode.Enabled) // Display status to show if GC is enabled or disabled
            {
                listing.Label("GCStatusEnabled".Translate());
            }
            else
            {
                listing.Label("GCStatusDisabled".Translate());
            }

            listing.CheckboxLabeled("enableGCScheduler".Translate(), ref ModSettings_GC_Scheduler.settings.gcScheduler); // Display button whether to enable the GC Scheduler or not

            if (ModSettings_GC_Scheduler.settings.gcScheduler)
            {
                listing.Gap(5f); // Display current update tick the game is on
                listing.Label("lblCurrentTickInterval".Translate() + ": " + MainButtonWorker_GC_Scheduler.updateTick + " / " + ModSettings_GC_Scheduler.settings.updateInterval);
                listing.Gap(5f); // Display current memory usage
                listing.Label("lblCurrentMemoryUsage".Translate() + ": \n" + MainButtonWorker_GC_Scheduler.currentMemory + " MB / " + ModSettings_GC_Scheduler.settings.totalMemory + " MB");
                listing.Gap(5f); // Display option whether or not to force pause the game when the menu is opened
                listing.CheckboxLabeled("lblForcePause".Translate(), ref ModSettings_GC_Scheduler.settings.forcePause, "lblForcePauseDescription".Translate());
                this.forcePause = ModSettings_GC_Scheduler.settings.forcePause;
                listing.Gap(5f);

                listing.Label("lblSetTickInterval".Translate() + ": " + ModSettings_GC_Scheduler.settings.updateInterval + " " + "lblTicks".Translate(), tooltip: "lblSetTickIntervalDescription".Translate());
                Rect section2 = new Rect(rect.x, rect.y + listing.CurHeight + 10f, rect.width, 25f); // Bar to set amount of ticks to pass before updating memory
                ModSettings_GC_Scheduler.settings.updateInterval = (int)Widgets.HorizontalSlider(section2, ModSettings_GC_Scheduler.settings.updateInterval, 300f, 3600f, leftAlignedLabel: "300", rightAlignedLabel: "3600");

                listing.Gap(40f);
                listing.Label("lblMinGCMemory".Translate() + ": " + ModSettings_GC_Scheduler.settings.totalMemory.ToString() + " MB", tooltip: "lblMinGCMemoryDescription".Translate());
                Rect section3 = new Rect(rect.x, rect.y + listing.CurHeight + 10f, rect.width, 25f); // Bar to set total memory to keep track of and minimum memory before using GC
                ModSettings_GC_Scheduler.settings.totalMemory = Widgets.HorizontalSlider(section3, ModSettings_GC_Scheduler.settings.totalMemory,
                                                                                    1024f, Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f),
                                                                                    leftAlignedLabel: "1024 MB", rightAlignedLabel: Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f).ToString() + " MB");
                
                Rect section4 = new Rect(rect.x, rect.y + section3.y + section3.height + 20f, rect.width, 100f);
                List<ListableOption> list = new List<ListableOption>(); // List to add buttons to
                list.Add(new ListableOption("BtnStopGC".Translate(), delegate () // enable GC
                            {
                                UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
                                Messages.Message("GCDisabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                            }, null));
                GUI.color = new Color(0.85f, 0f, 0f); // Sets color to red
                list.Add(new ListableOption("BtnStartGC".Translate(), delegate () // disable GC
                            {
                                UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
                                Messages.Message("GCEnabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                            }, null));
                OptionListingUtility.DrawOptionListing(section4, list); // Add buttons to tab
            }

            listing.End();
            GUI.EndGroup();
        }
    }
}

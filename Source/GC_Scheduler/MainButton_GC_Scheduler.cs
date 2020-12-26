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
            this.forcePause = true; // Pause game when opened
        }

        public override Vector2 RequestedTabSize
        {
            get { return new Vector2(250f, 500f); } // Size of window tab to open
        }

        public override MainTabWindowAnchor Anchor
        {
            get { return MainTabWindowAnchor.Right; } // Anchor tab to right of screen
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

            listing.Gap(5f);
            listing.CheckboxLabeled("enableGCScheduler".Translate(), ref MainButtonWorker_GC_Scheduler.gcScheduler);

            if (MainButtonWorker_GC_Scheduler.gcScheduler)
            {
                listing.Gap(5f);
                listing.Label("lblSetTickInterval".Translate() + ": " + MainButtonWorker_GC_Scheduler.updateInterval + " ticks", tooltip: "lblSetTickIntervalDescription".Translate());
                Rect section2 = new Rect(rect.x, rect.y + listing.CurHeight + 10f, rect.width, 25f); // Bar to set amount of ticks to pass before updating memory
                MainButtonWorker_GC_Scheduler.updateInterval = (int)Widgets.HorizontalSlider(section2, MainButtonWorker_GC_Scheduler.updateInterval, 300f, 3600f, leftAlignedLabel: "300", rightAlignedLabel: "3600");

                listing.Gap(40f);
                listing.Label("lblMinGCMemory".Translate() + ": " + MainButtonWorker_GC_Scheduler.totalMemory.ToString() + " MB", tooltip: "lblMinGCMemoryDescription".Translate());
                Rect section3 = new Rect(rect.x, rect.y + listing.CurHeight + 10f, rect.width, 25f); // Bar to set total memory to keep track of and minimum memory before using GC
                MainButtonWorker_GC_Scheduler.totalMemory = Widgets.HorizontalSlider(section3, MainButtonWorker_GC_Scheduler.totalMemory,
                                                                                    1024f, Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f),
                                                                                    leftAlignedLabel: "1024 MB", rightAlignedLabel: Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f).ToString() + " MB");
                
                Rect section4 = new Rect(rect.x, rect.y + section3.y + section3.height + 35f, rect.width, 100f);
                List<ListableOption> list = new List<ListableOption>(); // List to add buttons to
                list.Add(new ListableOption("BtnStopGC".Translate(), delegate () // enable GC
                {
                    UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
                    Messages.Message("GCDisabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                }, null));
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

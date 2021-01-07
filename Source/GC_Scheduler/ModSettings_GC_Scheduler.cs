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
    public class ModSettings_GC_Scheduler : ModSettings
    {
        public static Mod thisMod;
        public static int updateInterval = 360; // how often to run memory updating
        public static float totalMemory = Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f);
        public static bool gcScheduler = false;
        public static bool forcePause = true;
        public static bool enableGC_OnOpenTab = true;
        public static bool enableGC_OnOpenMap = true;
        public static bool enableGC_OnOpenMessage = true;

        public override void ExposeData() // Method used to write settings so that they are saved
        {
            Scribe_Values.Look(ref updateInterval, "updateInterval", 360);
            Scribe_Values.Look(ref totalMemory, "totalMemory", Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f));
            Scribe_Values.Look(ref gcScheduler, "gcScheduler", false);
            Scribe_Values.Look(ref forcePause, "forcePause", true);
            base.ExposeData();
        }

        public void DoSettingsWindow(Rect rect)
        {
            GUI.BeginGroup(rect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(rect);
            Text.Font = GameFont.Small; // Writes following labels in small font
            listing.Label("lblStatsNote".Translate());
            listing.End();

            Rect section2 = new Rect(rect.x, rect.y + 20f, rect.width, 450f);
            DoInGameSettingsWindwo(section2, sectionOffsetY: -55f );

            GUI.EndGroup();
        }

        public void DoInGameSettingsWindwo(Rect rect, float sectionOffsetX = 0f, float sectionOffsetY = 0f)
        {
            GUI.BeginGroup(rect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(rect);
            Text.Font = GameFont.Medium; // Writes following labels in medium font
            listing.Label("GCSchedulerTitle".Translate());
            Text.Font = GameFont.Small; // Writes following labels in small font

            if (UnityEngine.Scripting.GarbageCollector.GCMode == UnityEngine.Scripting.GarbageCollector.Mode.Enabled) // Display status to show if GC is enabled or disabled
            {
                listing.Label("lblGCStatusEnabled".Translate());
            }
            else
            {
                listing.Label("lblGCStatusDisabled".Translate());
            }

            Rect sectionEnd;

            if (gcScheduler)
            {
                listing.GapLine(5f);
                // Display current update tick the game is on
                listing.Label("lblCurrentTickInterval".Translate() + ": " + MainButtonWorker_GC_Scheduler.updateTick + " / " + updateInterval);
                listing.Gap(5f);
                // Display current memory usage
                listing.Label("lblCurrentMemoryUsage".Translate() + ": \n" + MainButtonWorker_GC_Scheduler.currentMemory + " MB / " + totalMemory + " MB");
                listing.GapLine(5f);
                // Display option whether or not to force pause the game when the menu is opened
                listing.CheckboxLabeled("lblForcePause".Translate(), ref forcePause, "lblForcePauseDescription".Translate());
                listing.Gap(5f);

                listing.Label("lblSetTickInterval".Translate() + ": " + updateInterval + " " + "lblTicks".Translate(), tooltip: "lblSetTickIntervalDescription".Translate());
                Rect section2 = new Rect(rect.x + sectionOffsetX, rect.y + listing.CurHeight + 10f + sectionOffsetY, rect.width, 25f);
                // Bar to set amount of ticks to pass before updating memory
                updateInterval = (int)Widgets.HorizontalSlider(section2, updateInterval, 300f, 3600f, leftAlignedLabel: "300", rightAlignedLabel: "3600");

                listing.Gap(40f);
                listing.Label("lblMinGCMemory".Translate() + ": " + totalMemory.ToString() + " MB", tooltip: "lblMinGCMemoryDescription".Translate());
                Rect section3 = new Rect(rect.x + sectionOffsetX, rect.y + listing.CurHeight + 10f + sectionOffsetY, rect.width, 25f);
                // Bar to set total memory to keep track of and minimum memory before using GC
                totalMemory = Widgets.HorizontalSlider(section3, totalMemory,
                                                        1024f, Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f),
                                                        leftAlignedLabel: "1024 MB", rightAlignedLabel: Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f).ToString() + " MB");

                sectionEnd = new Rect(rect.x + sectionOffsetX, rect.y + section3.y + section3.height + 5f + sectionOffsetY, rect.width, 50f);
            }
            else
            {
                sectionEnd = new Rect(rect.x + sectionOffsetX, rect.y + listing.CurHeight + sectionOffsetY, rect.width, 50f);
            }

            List<ListableOption> list = new List<ListableOption>();
            if (UnityEngine.Scripting.GarbageCollector.GCMode == UnityEngine.Scripting.GarbageCollector.Mode.Enabled) // If GC is enabled, show disable button and vice versa
            {
                GUI.color = Color.red; // Sets color to red so proceeding GUI is in red
                list.Add(new ListableOption("BtnStopGC1".Translate() + "\n" + "BtnStopGC2".Translate(), delegate () // disable GC button
                {
                    UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
                    gcScheduler = true;
                    Messages.Message("GCDisabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                }, null));
            }
            else
            {
                GUI.color = Color.green; // Sets color to red so proceeding GUI is in green
                list.Add(new ListableOption("BtnStartGC1".Translate() + "\n" + "BtnStartGC2".Translate(), delegate () // enable GC button
                {
                    UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
                    gcScheduler = false;
                    Messages.Message("GCEnabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                }, null));
            }
            OptionListingUtility.DrawOptionListing(sectionEnd, list); // Adds buttons to section4

            GUI.color = Color.white;
            listing.End();
            GUI.EndGroup();
        }
    }
}

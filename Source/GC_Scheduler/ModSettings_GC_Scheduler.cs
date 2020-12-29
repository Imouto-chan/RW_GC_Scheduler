using RimWorld;
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
        public static Mod thisMod;
        public static int updateInterval = 360; // how often to run memory updating
        public static float totalMemory = Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f);
        public static bool gcScheduler = false;
        public static bool forcePause = true;
        public static bool showMainTab = true;

        public override void ExposeData() // Method used to write settings so that they are saved
        {
            Scribe_Values.Look(ref updateInterval, "updateInterval", 360);
            Scribe_Values.Look(ref totalMemory, "totalMemory", Math.Max(((float)SystemInfo.systemMemorySize - 4096f) / 2f, 1024f));
            Scribe_Values.Look(ref gcScheduler, "gcScheduler", false);
            Scribe_Values.Look(ref forcePause, "forcePause", true);
            Scribe_Values.Look(ref showMainTab, "showMainTab", true);
            base.ExposeData();
        }

        public void DoSettingsWindow(Rect rect)
        {
            GUI.BeginGroup(rect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(rect);
            Text.Font = GameFont.Small; // Writes following labels in small font
            listing.CheckboxLabeled("ShowMainTab".Translate(), ref showMainTab); // check box to decide whether or not to show the mod tab in-game
            listing.End();

            Rect section2 = new Rect((rect.x + rect.width) / 2 - rect.width / 2, rect.y + listing.CurHeight, rect.width, 450f);
            DoInGameSettingsWindwo(section2);

            GUI.EndGroup();
        }

        public void DoInGameSettingsWindwo(Rect rect)
        {
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

            listing.CheckboxLabeled("enableGCScheduler".Translate(), ref gcScheduler); // Display button whether to enable the GC Scheduler or not

            if (gcScheduler)
            {
                listing.Gap(5f); // Display current update tick the game is on
                listing.Label("lblCurrentTickInterval".Translate() + ": " + MainButtonWorker_GC_Scheduler.updateTick + " / " + updateInterval);
                listing.Gap(5f); // Display current memory usage
                listing.Label("lblCurrentMemoryUsage".Translate() + ": \n" + MainButtonWorker_GC_Scheduler.currentMemory + " MB / " + totalMemory + " MB");
                listing.Gap(5f); // Display option whether or not to force pause the game when the menu is opened
                listing.CheckboxLabeled("lblForcePause".Translate(), ref forcePause, "lblForcePauseDescription".Translate());
                listing.Gap(5f);

                listing.Label("lblSetTickInterval".Translate() + ": " + updateInterval + " " + "lblTicks".Translate(), tooltip: "lblSetTickIntervalDescription".Translate());
                Rect section2 = new Rect(rect.x, rect.y + listing.CurHeight + 10f, rect.width, 25f); // Bar to set amount of ticks to pass before updating memory
                updateInterval = (int)Widgets.HorizontalSlider(section2, updateInterval, 300f, 3600f, leftAlignedLabel: "300", rightAlignedLabel: "3600");

                listing.Gap(40f);
                listing.Label("lblMinGCMemory".Translate() + ": " + totalMemory.ToString() + " MB", tooltip: "lblMinGCMemoryDescription".Translate());
                Rect section3 = new Rect(rect.x, rect.y + listing.CurHeight + 10f, rect.width, 25f); // Bar to set total memory to keep track of and minimum memory before using GC
                totalMemory = Widgets.HorizontalSlider(section3, totalMemory,
                                                                                    1024f, Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f),
                                                                                    leftAlignedLabel: "1024 MB", rightAlignedLabel: Math.Max((float)SystemInfo.systemMemorySize - 4096f, 2048f).ToString() + " MB");

                Rect section4 = new Rect(rect.x, rect.y + section3.y + section3.height + 5f, rect.width, 50f);
                List<ListableOption> list = new List<ListableOption>();
                if (UnityEngine.Scripting.GarbageCollector.GCMode == UnityEngine.Scripting.GarbageCollector.Mode.Enabled) // If GC is enabled, show disable button and vice versa
                {
                    GUI.color = new Color(0.85f, 0f, 0f); // Sets color to red
                    list.Add(new ListableOption("BtnStopGC".Translate(), delegate () // disable GC button
                    {
                        UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
                        Messages.Message("GCDisabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                    }, null));
                }
                else
                {
                    GUI.color = new Color(0.4f, 85f, 0f); // Sets color to green
                    list.Add(new ListableOption("BtnStartGC".Translate(), delegate () // enable GC button
                    {
                        UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
                        Messages.Message("GCEnabled".Translate(), MessageTypeDefOf.PositiveEvent, false);
                    }, null));
                }
                OptionListingUtility.DrawOptionListing(section4, list); // Adds buttons to section4
            }

            listing.End();
            GUI.EndGroup();
        }
    }
}

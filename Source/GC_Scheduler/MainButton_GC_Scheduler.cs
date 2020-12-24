﻿using System;
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
            get { return new Vector2(500f, 500f); } // Size of window tab to open
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
            listing.Label("v1.0");
            if (UnityEngine.Scripting.GarbageCollector.GCMode == UnityEngine.Scripting.GarbageCollector.Mode.Enabled) // Display status to show if GC is enabled or disabled
            {
                listing.Label("GCStatusEnabled".Translate());
            }
            else
            {
                listing.Label("GCStatusDisabled".Translate());
            }

            Rect section2 = new Rect(rect.x, rect.y + listing.CurHeight + 25f, rect.width, 25f);
            Widgets.CheckboxLabeled(section2, "lblSetMinGCMemory".Translate(), ref setMinMem);
            if (setMinMem) // Bar to set total memory to keep track of and minimum memory before using GC
            {
                Rect section2sub = new Rect(rect.x, rect.y + section2.y + section2.height + 10f, rect.width, 25f);
                section2.height += 25f;
                MainButtonWorker_GC_Scheduler.totalMemory = Widgets.HorizontalSlider(section2sub, MainButtonWorker_GC_Scheduler.totalMemory,
                                                                2048f, (float)SystemInfo.systemMemorySize - 2048f,
                                                                leftAlignedLabel: "2048 MB", rightAlignedLabel: ((float)SystemInfo.systemMemorySize - 2048f).ToString() + " MB",
                                                                label: "lblMinGCMemory".Translate() + ": " + MainButtonWorker_GC_Scheduler.totalMemory.ToString() + " MB");
            }

            listing.End();

            Rect section3 = new Rect(rect.x, rect.y + section2.y + section2.height + 35f, rect.width, 100f);
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
            OptionListingUtility.DrawOptionListing(section3, list); // Add buttons to tab

            //// Bar to set amount of ticks to pass before updating memory
            //MainButtonWorker_GC_Scheduler.updateInterval = (int)Widgets.HorizontalSlider(subRect, MainButtonWorker_GC_Scheduler.updateInterval,
            //                                                    300f, 3600f, label: "lblSetTickInterval".Translate());

            GUI.EndGroup();
        }

        bool setMinMem = false;
    }
}

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
            get { return new Vector2(200f, 200f); } // Size of window tab to open
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
            listing.Label("GCSchedulerTitle".Translate());
            Text.Font = GameFont.Small;
            listing.Label("v1.0");
            listing.Gap(10f);
            listing.End();

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
            OptionListingUtility.DrawOptionListing(rect, list); // Add buttons to tab
            GUI.EndGroup();
        }
    }
}

using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GitBookConnect.Presets
{
    // GitBook's hint/callout styles, for Hint Block's Style input.
    public class HintStylePresetComponent : GH_ValueList
    {
        public HintStylePresetComponent()
        {
            Category = PluginUtilities.TabName;
            SubCategory = PluginUtilities.CategoryZYPresets;
            Name = "Hint Style";
            NickName = "HintStyle";
            Description = "GitBook hint/callout styles.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Info",    "info" },
            { "Warning", "warning" },
            { "Danger",  "danger" },
            { "Success", "success" },
        };

        protected override Bitmap Icon => Properties.Resources.GC_HintStylePreset;
        public override Guid ComponentGuid => new Guid("d8c66403-23b3-4b3d-b34d-468e4963ea98");
    }
}

using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GitBookConnect.Presets
{
    // GitBook color tokens, for Color Mark's Text Color and/or Background Color inputs -- background
    // is GitBook's "highlight" equivalent. Named colors plus semantic $-tokens resolved from a
    // published site's theme.
    public class TextColorPresetComponent : GH_ValueList
    {
        public TextColorPresetComponent()
        {
            Category = PluginUtilities.TabName;
            SubCategory = PluginUtilities.CategoryZYPresets;
            Name = "Text Color";
            NickName = "TextColor";
            Description = "GitBook color tokens, for Color Mark's Text Color / Background Color inputs.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Default", "default" },
            { "Green",   "green" },
            { "Blue",    "blue" },
            { "Red",     "red" },
            { "Orange",  "orange" },
            { "Yellow",  "yellow" },
            { "Purple",  "purple" },
            { "Pink",    "pink" },
            { "Violet",  "violet" },
            { "Cyan",    "cyan" },
            { "Primary (theme)", "$primary" },
            { "Info (theme)",    "$info" },
            { "Success (theme)", "$success" },
            { "Warning (theme)", "$warning" },
            { "Danger (theme)",  "$danger" },
            { "Tint (theme)",    "$tint" },
        };

        protected override Bitmap Icon => Properties.Resources.GC_TextColorPreset;
        public override Guid ComponentGuid => new Guid("6c569fdf-def3-4c28-8b85-5d4a9b0ef0b2");
    }
}

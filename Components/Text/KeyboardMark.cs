using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds a keyboard-key mark to a Text JSON leaf. Chain onto another mark's output to combine styles.
    public class KeyboardMarkComponent : GH_Component
    {
        public KeyboardMarkComponent()
            : base("Keyboard Mark", "Kbd",
                "Adds a keyboard-key mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with keyboard added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddKeyboard(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_KeyboardMark;
        public override Guid ComponentGuid => new Guid("82081d36-36ec-46c7-88c6-dc1492b60071");
    }
}

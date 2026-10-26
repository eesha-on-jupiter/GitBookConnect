using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // GitBook has no dedicated "card" block -- its editor's Cards feature is a columns block whose
    // column children each hold a heading/text/icon. Builds that structure from parallel lists, one
    // item per card.
    public class BuildCardsBlockComponent : GH_Component
    {
        public BuildCardsBlockComponent()
            : base("Cards Block", "Cards",
                "Builds a GitBook cards (columns) block from parallel lists, one item per card.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Card Title", "CTi", "Card titles.", GH_ParamAccess.list);
            pManager.AddTextParameter("Card Text", "CTx", "Card body text, index-matched to Card Title.", GH_ParamAccess.list);
            pManager.AddTextParameter("Card Icon", "CI", "Card icons, index-matched to Card Title.", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Cards (columns) block JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var titles = new List<string>();
            var texts = new List<string>();
            var icons = new List<string>();
            DA.GetDataList(0, titles);
            DA.GetDataList(1, texts);
            DA.GetDataList(2, icons);

            DA.SetData(0, BlockBuilders.BuildCardsBlock(titles, texts, icons));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildCardsBlock;
        public override Guid ComponentGuid => new Guid("75c52a40-2eeb-42f0-8666-acdf27cb3a81");
    }
}

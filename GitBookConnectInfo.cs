using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GitBookConnect
{
    public class GitBookConnectInfo : GH_AssemblyInfo
    {
        public override string Name => "GitBookConnect";
        // Same 16×16 logo as the category icon — shown in Grasshopper's loaded-libraries list
        public override Bitmap Icon => Properties.Resources.GC_GitBookConnectLogo;
        public override string Description => "Grasshopper components for the GitBook API — browse organizations, spaces and published content, and author GitBook pages through the Change Request flow from Rhino/Grasshopper. Made with the PleaseREST Claude Code plugin.";
        public override Guid Id => new Guid("9e1eba44-f233-47ca-895e-b47349d479a0");
        public override string AuthorName => "Eesha Jain";
        public override string AuthorContact => "toeesha@gmail.com";
        // Single source of truth is the .csproj <Version> — never a second hardcoded string here
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString(3);
    }

    public class GitBookConnectCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("GitBookConnect", Properties.Resources.GC_GitBookConnectLogo);
            Instances.ComponentServer.AddCategorySymbolName("GitBookConnect", 'G');
            return GH_LoadingInstruction.Proceed;
        }
    }
}

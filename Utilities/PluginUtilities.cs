using System;
using System.Collections.Generic;
using System.Linq;

namespace GitBookConnect
{
    public static class PluginUtilities
    {
        // Shared enum helper — used by any preset component backed by an enum
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        internal static readonly string TabName = "GitBookConnect";

        // Subcategory constants — one per subcategory, in display order
        // Content-type model: Auth, then all HTTP tabs, then all JSON content-type tabs, then Compose, then Presets/Utilities
        internal static readonly string CategoryAAAuth           = "1. Auth";
        internal static readonly string CategoryABWorkspace      = "2. Workspace";
        internal static readonly string CategoryACContent        = "3. Content";
        internal static readonly string CategoryADChangeRequests = "4. Change Requests";
        internal static readonly string CategoryAEPages          = "5. Pages";
        internal static readonly string CategoryAFText           = "6. Text";
        internal static readonly string CategoryAGRichContent    = "7. Rich Content";
        internal static readonly string CategoryAHCompose        = "8. Compose";
        internal static readonly string CategoryZYPresets        = "9. Presets";
        internal static readonly string CategoryZZUtilities      = "10. Utilities";
    }
}

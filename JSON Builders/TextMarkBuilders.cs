using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    public static class TextMarkBuilders
    {
        #region Private helpers

        private static JObject NewLeaf(string text)
        {
            return new JObject
            {
                ["object"] = "leaf",
                ["text"] = text ?? "",
                ["marks"] = new JArray()
            };
        }

        // Adds (or replaces) one mark on a leaf's marks array — re-applying the same mark type updates its data
        private static string AddMark(string textJson, string markType, JObject markData)
        {
            JObject leaf = string.IsNullOrWhiteSpace(textJson) ? NewLeaf("") : JObject.Parse(textJson);
            var marks = leaf["marks"] as JArray ?? new JArray();

            for (int i = marks.Count - 1; i >= 0; i--)
                if ((string)marks[i]["type"] == markType) marks.RemoveAt(i);

            var mark = new JObject { ["type"] = markType };
            if (markData != null && markData.Count > 0) mark["data"] = markData;
            marks.Add(mark);

            leaf["marks"] = marks;
            return leaf.ToString(Formatting.None);
        }

        #endregion

        #region Text leaf

        // leaf
        public static string BuildPlainText(string text)
        {
            return NewLeaf(text ?? "").ToString(Formatting.None);
        }

        #endregion

        #region Marks

        // mark
        public static string AddBold(string textJson) => AddMark(textJson, "bold", null);

        // mark
        public static string AddItalic(string textJson) => AddMark(textJson, "italic", null);

        // mark
        public static string AddStrikethrough(string textJson) => AddMark(textJson, "strikethrough", null);

        // mark
        public static string AddCode(string textJson) => AddMark(textJson, "code", null);

        // mark
        public static string AddKeyboard(string textJson) => AddMark(textJson, "keyboard", null);

        // mark
        public static string AddSuperscript(string textJson) => AddMark(textJson, "superscript", null);

        // mark
        public static string AddSubscript(string textJson) => AddMark(textJson, "subscript", null);

        // mark
        public static string AddColor(string textJson, string textColor, string backgroundColor)
        {
            var data = new JObject();
            if (!string.IsNullOrWhiteSpace(textColor) && textColor != "default") data["text"] = textColor;
            if (!string.IsNullOrWhiteSpace(backgroundColor) && backgroundColor != "default") data["background"] = backgroundColor;
            return AddMark(textJson, "color", data);
        }

        #endregion
    }
}

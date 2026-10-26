using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    public static class BlockBuilders
    {
        #region Private helpers

        // Wraps a list of Text JSON leaves (from the Text tab) into one text node holding all of them
        private static JArray TextNodeFromRuns(IEnumerable<string> textJsonRuns)
        {
            var leaves = new JArray();
            foreach (var t in textJsonRuns ?? Array.Empty<string>())
                if (!string.IsNullOrWhiteSpace(t)) leaves.Add(JObject.Parse(t));

            return new JArray(new JObject { ["object"] = "text", ["leaves"] = leaves });
        }

        private static JObject Paragraph(IEnumerable<string> textJsonRuns)
        {
            return new JObject { ["object"] = "block", ["type"] = "paragraph", ["nodes"] = TextNodeFromRuns(textJsonRuns) };
        }

        #endregion

        #region Blocks

        // block
        public static string BuildParagraphBlock(IEnumerable<string> textJsonRuns)
        {
            return Paragraph(textJsonRuns).ToString(Formatting.None);
        }

        // block
        public static string BuildHeadingBlock(IEnumerable<string> textJsonRuns, int level)
        {
            int lvl = level < 1 ? 1 : (level > 6 ? 6 : level);
            var block = new JObject { ["object"] = "block", ["type"] = "heading-" + lvl, ["nodes"] = TextNodeFromRuns(textJsonRuns) };
            return block.ToString(Formatting.None);
        }

        // block
        public static string BuildHintBlock(string style, IEnumerable<string> textJsonRuns)
        {
            var block = new JObject
            {
                ["object"] = "block",
                ["type"] = "hint",
                ["data"] = new JObject { ["style"] = string.IsNullOrWhiteSpace(style) ? "info" : style },
                ["nodes"] = new JArray(Paragraph(textJsonRuns))
            };
            return block.ToString(Formatting.None);
        }

        // block
        public static string BuildExpandableBlock(string title, IEnumerable<string> textJsonRuns, bool defaultExpanded)
        {
            var block = new JObject
            {
                ["object"] = "block",
                ["type"] = "expandable",
                ["data"] = new JObject { ["title"] = title ?? "", ["expanded"] = defaultExpanded },
                ["nodes"] = new JArray(Paragraph(textJsonRuns))
            };
            return block.ToString(Formatting.None);
        }

        // block
        public static string BuildCardsBlock(IList<string> cardTitles, IList<string> cardTexts, IList<string> cardIcons)
        {
            var columns = new JArray();
            int n = cardTitles?.Count ?? 0;

            for (int i = 0; i < n; i++)
            {
                string title = cardTitles[i] ?? "";
                string text = cardTexts != null && i < cardTexts.Count ? (cardTexts[i] ?? "") : "";
                string icon = cardIcons != null && i < cardIcons.Count ? (cardIcons[i] ?? "") : "";

                var nodes = new JArray { new JObject { ["object"] = "block", ["type"] = "heading-3", ["nodes"] = TextNodeFromRuns(new[] { TextMarkBuilders.BuildPlainText(title) }) } };
                if (!string.IsNullOrWhiteSpace(text))
                    nodes.Add(Paragraph(new[] { TextMarkBuilders.BuildPlainText(text) }));

                var columnData = string.IsNullOrWhiteSpace(icon) ? new JObject() : new JObject { ["icon"] = icon };
                columns.Add(new JObject { ["object"] = "block", ["type"] = "column", ["data"] = columnData, ["nodes"] = nodes });
            }

            var block = new JObject { ["object"] = "block", ["type"] = "columns", ["nodes"] = columns };
            return block.ToString(Formatting.None);
        }

        #endregion

        #region Parse helpers

        public static JObject Parse(string json)
            => string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

        #endregion
    }
}

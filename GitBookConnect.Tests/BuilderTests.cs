using System.Collections.Generic;
using System.Linq;
using GitBookConnect;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GitBookConnect.Tests
{
    // Pure builder-logic tests. The Markdown page path is checked against the validated
    // apply_change_request_content example in spec/call-library-validated.json; the JSON-block
    // path has no live-validated reference (dogfood F15), so those tests assert internal shape.
    public class TextMarkBuilderTests
    {
        [Fact]
        public void PlainText_IsLeafWithNoMarks()
        {
            var leaf = JObject.Parse(TextMarkBuilders.BuildPlainText("Level 3"));
            Assert.Equal("leaf", (string)leaf["object"]);
            Assert.Equal("Level 3", (string)leaf["text"]);
            Assert.Empty((JArray)leaf["marks"]);
        }

        [Fact]
        public void PlainText_NullBecomesEmptyString()
        {
            var leaf = JObject.Parse(TextMarkBuilders.BuildPlainText(null));
            Assert.Equal("", (string)leaf["text"]);
        }

        [Theory]
        [InlineData("bold")]
        [InlineData("italic")]
        [InlineData("strikethrough")]
        [InlineData("code")]
        [InlineData("keyboard")]
        [InlineData("superscript")]
        [InlineData("subscript")]
        public void EachMark_AddsItsType(string markType)
        {
            string leaf = TextMarkBuilders.BuildPlainText("x");
            string marked = markType switch
            {
                "bold" => TextMarkBuilders.AddBold(leaf),
                "italic" => TextMarkBuilders.AddItalic(leaf),
                "strikethrough" => TextMarkBuilders.AddStrikethrough(leaf),
                "code" => TextMarkBuilders.AddCode(leaf),
                "keyboard" => TextMarkBuilders.AddKeyboard(leaf),
                "superscript" => TextMarkBuilders.AddSuperscript(leaf),
                _ => TextMarkBuilders.AddSubscript(leaf),
            };
            var marks = (JArray)JObject.Parse(marked)["marks"];
            Assert.Single(marks);
            Assert.Equal(markType, (string)marks[0]["type"]);
            Assert.Null(marks[0]["data"]);
        }

        [Fact]
        public void Marks_ChainAndAccumulate()
        {
            string json = TextMarkBuilders.AddItalic(TextMarkBuilders.AddBold(TextMarkBuilders.BuildPlainText("x")));
            var types = ((JArray)JObject.Parse(json)["marks"]).Select(m => (string)m["type"]).ToList();
            Assert.Equal(new[] { "bold", "italic" }, types);
        }

        [Fact]
        public void SameMarkTwice_IsNotDuplicated()
        {
            string json = TextMarkBuilders.AddBold(TextMarkBuilders.AddBold(TextMarkBuilders.BuildPlainText("x")));
            Assert.Single((JArray)JObject.Parse(json)["marks"]);
        }

        [Fact]
        public void Mark_OnEmptyInput_StartsFromEmptyLeaf()
        {
            var leaf = JObject.Parse(TextMarkBuilders.AddBold(""));
            Assert.Equal("leaf", (string)leaf["object"]);
            Assert.Equal("", (string)leaf["text"]);
        }

        [Fact]
        public void Color_WritesTextAndBackground()
        {
            var mark = (JObject)((JArray)JObject.Parse(TextMarkBuilders.AddColor(TextMarkBuilders.BuildPlainText("x"), "green", "$warning"))["marks"])[0];
            Assert.Equal("color", (string)mark["type"]);
            Assert.Equal("green", (string)mark["data"]["text"]);
            Assert.Equal("$warning", (string)mark["data"]["background"]);
        }

        [Fact]
        public void Color_DefaultTokensAreOmitted()
        {
            var mark = (JObject)((JArray)JObject.Parse(TextMarkBuilders.AddColor(TextMarkBuilders.BuildPlainText("x"), "default", "default"))["marks"])[0];
            Assert.Equal("color", (string)mark["type"]);
            Assert.Null(mark["data"]);
        }

        [Fact]
        public void Color_ReappliedReplacesPrevious()
        {
            string once = TextMarkBuilders.AddColor(TextMarkBuilders.BuildPlainText("x"), "red", null);
            var marks = (JArray)JObject.Parse(TextMarkBuilders.AddColor(once, "blue", null))["marks"];
            Assert.Single(marks);
            Assert.Equal("blue", (string)marks[0]["data"]["text"]);
        }
    }

    public class BlockBuilderTests
    {
        private static readonly string[] Runs =
        {
            TextMarkBuilders.BuildPlainText("Plain "),
            TextMarkBuilders.AddBold(TextMarkBuilders.BuildPlainText("bold")),
        };

        [Fact]
        public void Paragraph_WrapsRunsInOneTextNode()
        {
            var block = JObject.Parse(BlockBuilders.BuildParagraphBlock(Runs));
            Assert.Equal("block", (string)block["object"]);
            Assert.Equal("paragraph", (string)block["type"]);
            var textNode = (JObject)((JArray)block["nodes"]).Single();
            Assert.Equal("text", (string)textNode["object"]);
            Assert.Equal(2, ((JArray)textNode["leaves"]).Count);
        }

        [Fact]
        public void Paragraph_SkipsBlankRuns()
        {
            var block = JObject.Parse(BlockBuilders.BuildParagraphBlock(new[] { "", Runs[0], "  " }));
            Assert.Single((JArray)block["nodes"][0]["leaves"]);
        }

        [Theory]
        [InlineData(2, "heading-2")]
        [InlineData(0, "heading-1")]
        [InlineData(9, "heading-6")]
        public void Heading_LevelIsClamped(int level, string expected)
        {
            var block = JObject.Parse(BlockBuilders.BuildHeadingBlock(Runs, level));
            Assert.Equal(expected, (string)block["type"]);
        }

        [Fact]
        public void Hint_HasStyleAndParagraph()
        {
            var block = JObject.Parse(BlockBuilders.BuildHintBlock("warning", Runs));
            Assert.Equal("hint", (string)block["type"]);
            Assert.Equal("warning", (string)block["data"]["style"]);
            Assert.Equal("paragraph", (string)block["nodes"][0]["type"]);
        }

        [Fact]
        public void Hint_BlankStyleFallsBackToInfo()
        {
            var block = JObject.Parse(BlockBuilders.BuildHintBlock("", Runs));
            Assert.Equal("info", (string)block["data"]["style"]);
        }

        [Fact]
        public void Expandable_CarriesTitleAndExpanded()
        {
            var block = JObject.Parse(BlockBuilders.BuildExpandableBlock("Details", Runs, true));
            Assert.Equal("expandable", (string)block["type"]);
            Assert.Equal("Details", (string)block["data"]["title"]);
            Assert.True((bool)block["data"]["expanded"]);
            Assert.Equal("paragraph", (string)block["nodes"][0]["type"]);
        }

        [Fact]
        public void Cards_OneColumnPerTitle_TextAndIconOptional()
        {
            var block = JObject.Parse(BlockBuilders.BuildCardsBlock(
                new List<string> { "A", "B" }, new List<string> { "about A" }, new List<string> { "", "star" }));
            Assert.Equal("columns", (string)block["type"]);
            var columns = (JArray)block["nodes"];
            Assert.Equal(2, columns.Count);
            Assert.All(columns, c => Assert.Equal("column", (string)c["type"]));
            Assert.Equal("heading-3", (string)columns[0]["nodes"][0]["type"]);
            Assert.Equal(2, ((JArray)columns[0]["nodes"]).Count);   // title + text
            Assert.Single((JArray)columns[1]["nodes"]);             // title only
            Assert.Null(columns[0]["data"]["icon"]);
            Assert.Equal("star", (string)columns[1]["data"]["icon"]);
        }

        [Fact]
        public void Cards_NoTitles_IsEmptyColumns()
        {
            var block = JObject.Parse(BlockBuilders.BuildCardsBlock(null, null, null));
            Assert.Empty((JArray)block["nodes"]);
        }
    }

    public class TableBuilderTests
    {
        private static readonly IList<string> Header = new List<string> { "Level", "Area" };
        private static readonly IList<IList<string>> Rows = new List<IList<string>>
        {
            new List<string> { "L1", "1200" },
            new List<string> { "L2", "1150" },
        };

        [Fact]
        public void Markdown_HeaderSeparatorAndRows()
        {
            string md = TableBuilders.BuildMarkdownTable(Header, Rows);
            Assert.Equal("| Level | Area |\n| --- | --- |\n| L1 | 1200 |\n| L2 | 1150 |\n", md);
        }

        [Fact]
        public void Markdown_EscapesPipesAndNewlines()
        {
            string md = TableBuilders.BuildMarkdownTable(new List<string> { "a|b" }, new List<IList<string>> { new List<string> { "x\ny" } });
            Assert.Contains("a\\|b", md);
            Assert.Contains("| x y |", md);
        }

        [Fact]
        public void TableBlock_HeaderRowFlaggedThenDataRows()
        {
            var block = JObject.Parse(TableBuilders.BuildTableBlock(Header, Rows));
            Assert.Equal("table", (string)block["type"]);
            Assert.Equal("grid", (string)block["data"]["view"]);
            var rows = (JArray)block["nodes"];
            Assert.Equal(3, rows.Count);
            Assert.True((bool)rows[0]["data"]["header"]);
            Assert.Null(rows[1]["data"]);
            Assert.Equal("1150", (string)rows[2]["nodes"][1]["nodes"][0]["leaves"][0]["text"]);
        }

        [Fact]
        public void TableBlock_NoHeader_OnlyDataRows()
        {
            var block = JObject.Parse(TableBuilders.BuildTableBlock(null, Rows));
            Assert.Equal(2, ((JArray)block["nodes"]).Count);
        }
    }

    public class PageBuilderTests
    {
        [Fact]
        public void MarkdownDocument_MatchesValidatedInsertPageShape()
        {
            // spec/call-library-validated.json -> apply_change_request_content.example_request.changes[0]
            var expected = JObject.Parse(@"{""operation"":""insert_page"",""title"":""API Test Page"",""document"":{""markdown"":""# API Test Page\n\nCreated by automated API test.\n""}}");
            string doc = PageBuilders.BuildDocumentFromMarkdown("API Test Page", "# API Test Page\n\nCreated by automated API test.\n", "");
            var op = JObject.Parse(PageBuilders.BuildInsertPageOp(doc, "", null, ""));
            Assert.True(JToken.DeepEquals(expected, op), op.ToString());
        }

        [Fact]
        public void MarkdownDocument_OmitsBlankTitleAndIcon()
        {
            var doc = JObject.Parse(PageBuilders.BuildDocumentFromMarkdown("", "body", " "));
            Assert.Null(doc["title"]);
            Assert.Null(doc["icon"]);
            Assert.Equal("body", (string)doc["document"]["markdown"]);
        }

        [Fact]
        public void BlocksDocument_WrapsNodesAndSkipsBlanks()
        {
            string para = BlockBuilders.BuildParagraphBlock(new[] { TextMarkBuilders.BuildPlainText("x") });
            var doc = JObject.Parse(PageBuilders.BuildDocumentFromBlocks("Rich", new[] { para, "", para }));
            Assert.Equal("Rich", (string)doc["title"]);
            Assert.Equal("document", (string)doc["document"]["object"]);
            Assert.Equal(2, ((JArray)doc["document"]["nodes"]).Count);
        }

        [Fact]
        public void InsertPage_OptionalPlacementFields()
        {
            string doc = PageBuilders.BuildDocumentFromMarkdown("T", "b", "📄");
            var op = JObject.Parse(PageBuilders.BuildInsertPageOp(doc, "parent123", 2, "ref-a"));
            Assert.Equal("insert_page", (string)op["operation"]);
            Assert.Equal("📄", (string)op["icon"]);
            Assert.Equal("parent123", (string)op["parent"]);
            Assert.Equal(2, (int)op["position"]);
            Assert.Equal("ref-a", (string)op["ref"]);
        }

        [Fact]
        public void UpdatePage_MoveOnly_HasNoDocument()
        {
            var op = JObject.Parse(PageBuilders.BuildUpdatePageOp("page1", "", "parent2", 0));
            Assert.Equal("update_page", (string)op["operation"]);
            Assert.Equal("page1", (string)op["page"]);
            Assert.Null(op["document"]);
            Assert.Equal("parent2", (string)op["parent"]);
            Assert.Equal(0, (int)op["position"]);
        }

        [Fact]
        public void UpdatePage_CopiesTitleAndDocument()
        {
            string doc = PageBuilders.BuildDocumentFromMarkdown("New title", "new body", "");
            var op = JObject.Parse(PageBuilders.BuildUpdatePageOp("page1", doc, "", null));
            Assert.Equal("New title", (string)op["title"]);
            Assert.Equal("new body", (string)op["document"]["markdown"]);
            Assert.Null(op["parent"]);
            Assert.Null(op["position"]);
        }
    }

    public class FileBuilderTests
    {
        [Fact]
        public void InsertFiles_UrlWinsOverContent()
        {
            var op = JObject.Parse(FileBuilders.BuildInsertFilesOp(
                new List<string> { "a.png", "b.png" },
                new List<string> { "ra", "" },
                new List<string> { "QUJD", "REVG" },
                new List<string> { "https://example.com/a.png" }));
            Assert.Equal("insert_files", (string)op["operation"]);
            var files = (JArray)op["files"];
            Assert.Equal(2, files.Count);
            Assert.Equal("https://example.com/a.png", (string)files[0]["url"]);
            Assert.Null(files[0]["content"]);
            Assert.Equal("ra", (string)files[0]["ref"]);
            Assert.Equal("REVG", (string)files[1]["content"]);
            Assert.Null(files[1]["ref"]);
        }

        [Fact]
        public void ImageBlock_ReferencesFile()
        {
            var block = JObject.Parse(FileBuilders.BuildImageBlock("ra"));
            Assert.Equal("images", (string)block["type"]);
            Assert.Equal("image", (string)block["nodes"][0]["type"]);
            Assert.Equal("ra", (string)block["nodes"][0]["data"]["ref"]);
        }
    }
}

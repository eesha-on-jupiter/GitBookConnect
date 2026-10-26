# GitBookConnect — Build TODO

Derived from `spec/plugin-spec.json` (49 components, 11 subcategories) and `spec/workflows.json` (8 confirmed workflows).
Nothing under `src/` exists yet — every item below starts unchecked.

Content-type model note: unlike a plain resource-model plugin, most of GitBookConnect's write surface funnels through
one batch endpoint (Apply Content Changes). Phases 2-4 below are organized by aggregation depth (unit -> aggregator ->
cross-resource batch), not strictly by REST resource -- see plugin-spec.json's `role` field per component.

---

## PHASE 1 — AUTH

- [x] GitBookConnectClient.cs        [CODE] base HTTP setup, Bearer auth header, NewRequest() helper, base URL https://api.gitbook.com/v1
- [x] GitBookAuth.cs                 [CODE] Token input -> Status + Response (GET /user). Note in-component: requires a Personal Access Token, not an integration token -- Change Request writes reject integration tokens outright (see feasibility.json auth_details.change_request_write_restriction)
- [x] GitBookConnectInfo.cs          [CODE] GH_AssemblyInfo + GH_AssemblyPriority
- [x] ButtonComponent.cs             [CODE] base class -- copy verbatim, namespace only
- [x] PluginUtilities.cs             [CODE] TabName + subcategory constants (Auth, Workspace, Content, Change Requests, Pages, Text, Rich Content, Compose, Presets, Utilities)
- [x] Auth icon                      [ASSET] generate_icon -> add_to_resources -- rendered in Phase 11 (ph:key glyph, blue/violet gradient background)
- [ ] Verify: authenticate with a GitBook Personal Access Token and get a valid GET /user response -- GATE 5, needs a live Rhino/Grasshopper test

## PHASE 2 — SIMPLE UNITS

### Workspace (read-only navigation)
- [x] GetAllOrganizations.cs         [CODE] Token -> Status + Response
- [x] GetOrganization.cs             [CODE] Token, Organization ID -> Status + Response
- [x] GetAllSpaces.cs                [CODE] Token, Organization ID -> Status + Response
- [x] GetSpace.cs                    [CODE] Token, Space ID -> Status + Response
- [x] GetAllCollections.cs           [CODE] Token, Organization ID -> Status + Response
- [x] GetCollection.cs               [CODE] Token, Collection ID -> Status + Response

### Content (read-only, published/live)
- [x] GetPublishedContent.cs         [CODE] Token, Space ID -> Status + Response
- [x] GetAllPages.cs                 [CODE] Token, Space ID -> Status + Response
- [x] GetPage.cs                     [CODE] Token, Space ID, Page ID -> Status + Response
- [x] GetPageByPath.cs               [CODE] Token, Space ID, Page Path -> Status + Response

### Change Requests -- direct-input lifecycle writes (no JSON builder pairing)
- [x] CreateChangeRequest.cs         [CODE] Token, Space ID, Subject, Description -> Change Request ID + URL + Status + Response (URL = urls.app from the response, confirmed live -- direct link to view/edit in the GitBook web app). NOTE: GitBook's create endpoint body only accepts 'subject' (validated); Description is set via a follow-up Update Change Request call to honor the approved I/O contract.
- [x] GetAllChangeRequests.cs        [CODE] Token, Space ID -> Status + Response
- [x] GetChangeRequest.cs            [CODE] Token, Space ID, Change Request ID -> URL + Status + Response
- [x] UpdateChangeRequest.cs         [CODE] Token, Space ID, Change Request ID, Subject, Description -> Status + Response
- [x] GetChangeRequestContent.cs     [CODE] Token, Space ID, Change Request ID -> Status + Response
- [x] SyncChangeRequest.cs           [CODE] Token, Space ID, Change Request ID -> Status + Response
- [x] RequestReviewers.cs            [CODE] Token, Space ID, Change Request ID, Reviewer User ID (list) -> Status + Response

### Pages -- secondary Markdown-path JSON units
- [x] BuildPageMarkdown.cs           [CODE] Title, Markdown Body, Icon -> Document JSON. Built PageBuilders.cs early (out of its Phase 3 listed order) since this component needs it -- covers both Markdown and JSON-block Document JSON paths plus insert_page/update_page op construction, all in one pass; Phase 3's BuildRichPage/InsertPage/UpdatePage/BuildPagesFromTree will consume the same file.
- [x] MarkdownTableFromGrid.cs       [CODE] Header Row, Grid (tree) -> Markdown Body (tabular benchmark, Markdown path). Also built TableBuilders.cs early (shared with Rich Content's TableBlock.cs, not separately listed in this todo but required by plugin-spec.json's builders[] list).

### Text -- chainable/recursive inline mark units
- [x] TextMarkBuilders.cs            [CODE] leaf/text-node JSON + all 8 DocumentTextMark constructors
- [x] PlainText.cs                   [CODE] Text -> Text JSON (base leaf, no marks)
- [x] BoldMark.cs                    [CODE] Text JSON -> Text JSON (+bold)
- [x] ItalicMark.cs                  [CODE] Text JSON -> Text JSON (+italic)
- [x] StrikethroughMark.cs           [CODE] Text JSON -> Text JSON (+strikethrough)
- [x] CodeMark.cs                    [CODE] Text JSON -> Text JSON (+code)
- [x] KeyboardMark.cs                [CODE] Text JSON -> Text JSON (+keyboard)
- [x] SuperscriptMark.cs             [CODE] Text JSON -> Text JSON (+superscript)
- [x] SubscriptMark.cs               [CODE] Text JSON -> Text JSON (+subscript)
- [x] ColorMark.cs                   [CODE] Text JSON, Text Color, Background Color -> Text JSON (+color; background = "highlight")

### Rich Content -- standalone JSON block units (no cross-block aggregation needed)
- [x] BuildHintBlock.cs              [CODE] Style, Text JSON (list) -> Block JSON (hint/callout). Built BlockBuilders.cs early (out of its Phase 3 listed order) since this component and Expandable Block both need it -- covers hint/expandable/paragraph/heading/cards block construction in one pass; Phase 3's Paragraph/Heading/Cards Block + Blocks Aggregator + Build Page (JSON) will consume the same file.
- [x] BuildExpandableBlock.cs        [CODE] Title, Text JSON (list), Default Expanded -> Block JSON
- [x] TableBlock.cs                  [CODE] Header Row, Grid (tree) -> Block JSON (tabular benchmark, JSON path)

### Compose -- standalone file/image unit
- [x] FileBuilders.cs                [CODE] insert_files op construction from File Path / Bitmap / Source URL / Base64 / Rhino viewport capture. NOTE: no HTTP endpoint in call-library-validated.json exercises insert_files directly (only apply_change_request_content's single insert_page example was validated) -- the op/file-entry shape and the per-item Image Block shape are a best-effort, internally-consistent construction, not a validated example. Flagged as a RUN-category note in the dogfood log.
- [x] BuildInsertFile.cs             [CODE] File Name, File Path, Bitmap, Source URL, Base64, Capture Rhino Viewport, View Name, Ref -> Change JSON + Block JSON (list)

- [x] Icons for every component above [ASSET] generate_icon -> add_to_resources -- rendered in Phase 11
- [ ] Verify: each simple unit returns valid JSON (JSON builders) or a valid direct HTTP response (HTTP components) against the live API

## PHASE 3 — AGGREGATOR

### Rich Content -- block-level aggregation
- [x] BlockBuilders.cs               [CODE] paragraph/heading/cards block construction + blocks-array wrapping (built early alongside BuildHintBlock.cs in Phase 2, see note there)
- [x] ParagraphBlock.cs              [CODE] Text JSON (list) -> Block JSON
- [x] HeadingBlock.cs                [CODE] Text JSON (list), Level -> Block JSON
- [x] BuildCardsBlock.cs             [CODE] Card Title/Text/Icon (list) -> Block JSON (columns-as-cards)
- [x] PageBlocksAggregator.cs        [CODE] Block JSON (list) -> Blocks JSON (Compose tab, feeds Build Page (JSON))

### Pages -- page-level assembly into a Change JSON op
- [x] PageBuilders.cs                [CODE] Document JSON construction (both Markdown and JSON-block paths) + insert_page/update_page op construction (built early alongside BuildPageMarkdown.cs in Phase 2, see note there)
- [x] BuildRichPage.cs               [CODE] Title, Blocks JSON (list) -> Document JSON (primary path)
- [x] InsertPage.cs                  [CODE] Document JSON (list), Into, At, Ref -> Change JSON (list) -- workflow 1
- [x] UpdatePage.cs                  [CODE] Page ID (list), Document JSON (list), Into, At -> Change JSON (list)
- [x] BuildPagesFromTree.cs          [CODE] Into (item), Page ID/Title/Body Text/Icon (tree) -> Change JSON (list) -- workflow 3, Dual Style Pattern parametric variant

- [x] Icons for every component above [ASSET] -- rendered in Phase 11
- [ ] Verify: a full Document JSON (via either the Markdown or JSON block path) resolves into a valid Insert/Update Page Change JSON operation

## PHASE 4 — LEVEL 2 AGGREGATOR

Cross-resource batching -- gathers Change JSON from Pages *and* Compose (Build Insert File) into the one call the API actually accepts.

- [x] ContentChangesAggregator.cs    [CODE] flattens every wired Change JSON into one ordered batch, capped at 50 (API limit)
- [x] ApplyContentChanges.cs         [CODE] Token, Space ID, Change Request ID, Change JSON (list) -> Status + Response -- the single batch write call (workflow 4)
- [x] MergeChangeRequest.cs          [CODE] Token, Space ID, Change Request ID -> Status + Response -- irreversible publish; kept though not currently needed (user declined removal)
- [x] Icons for the above            [ASSET] -- rendered in Phase 11
- [ ] Verify: a full batch (multiple pages + a file insert in one call) applies successfully to a live Change Request; confirm it's visible in the GitBook web app without merging

## PHASE 5 — CUSTOM / AEC FEATURES

SKIPPED — no AEC features selected at Gate 3. Version Watcher (Get Revision / Get Revision Changes) was proposed for
workflow 5's re-sync conflict-check and declined; add later via `/rest-add-feature` if a real overwrite conflict comes
up. In-GH Documentation Writer, Auth Manager, Error Reporter, and Batch Processor were found already delivered by
Phases 1-4 above (Text/Rich Content tabs, Connect to GitBook, Status/Response outputs, Content Changes Aggregator).

## PHASE 6 — PRESETS

- [x] HintStylePreset.cs             [CODE] GH_ValueList: info / warning / danger / success
- [x] TextColorPreset.cs             [CODE] GH_ValueList: default / green / blue / red / orange / yellow / purple / pink / violet / cyan / $primary / $info / $success / $warning / $danger / $tint
- [x] Preset icons                   [ASSET] generate_icon -> add_to_resources -- rendered in Phase 11 (neutral gray background, distinct from the API-call components)
- [ ] Verify: presets compose cleanly into Hint Block's Style input and Color Mark's Text Color / Background Color inputs

## PHASE 7 — TESTS

- [x] PageBuilderTests.cs            [TEST] Document JSON construction (Markdown + JSON paths), insert/update page op shape -- consolidated into GitBookConnect.Tests/BuilderTests.cs
- [x] TextMarkBuilderTests.cs        [TEST] each of the 8 marks, chaining (marks array accumulates correctly across composed calls)
- [x] BlockBuilderTests.cs           [TEST] paragraph/heading/hint/expandable/cards/blocks-array construction
- [x] TableBuilderTests.cs           [TEST] Markdown table + JSON table block construction from a tree grid
- [x] FileBuilderTests.cs            [TEST] insert_files op construction from each source type (path/bitmap/URL/base64/capture)
- [x] dotnet build                   [CODE] all green (net48 + net7.0-windows/net7.0) -- Release, 0 errors (35 CA1416 on the net7.0 Mac target, expected)
- [x] dotnet test                    [CODE] all green -- 43 passed (BuilderTests.cs + BulkAssemblyTests.cs)

### Demo canvases (derived from confirmed workflows)
- [ ] demos/01-batch-publish-pages.gh      [MCP] workflow 1 -- tree of records -> Insert Page (list) -> Apply Content Changes
- [ ] demos/02-tabular-table-page.gh       [MCP] workflow 2 -- grid tree -> Table Block / Markdown Table From Grid -> page
- [ ] demos/03-rich-styled-page.gh         [MCP] workflow 7 -- chained text marks -> Paragraph/Hint/Expandable/Cards blocks -> Build Page (JSON)
- [ ] demos/04-insert-image.gh             [MCP] workflow 6 -- Build Insert File from a file path, a Bitmap, a URL, and a Rhino viewport capture
- [ ] demos/05-change-request-lifecycle.gh [MCP] workflow 4 -- Create Change Request -> Apply Content Changes -> Get Change Request (no merge)
- [ ] Live MCP smoke test              [MCP] load .gha, solve each demo, auto-fix on failure, flag human only if no fix
- [x] Verify: all demos pass and demos/ is populated (or degrade gracefully to manual per pr-rhino-test) -- degraded: no Grasshopper bridge; demos specified by hand in demos/README.md and written up as docs/examples/*.md; .gh files pending

## PHASE 8 — DOCS

- [x] docs/index.md                       [CODE] plugin overview + install
- [x] docs/quickstart.md                  [CODE]
- [x] docs/authentication.md              [CODE] Personal Access Token requirement, space role requirement, integration-token restriction
- [x] docs/components/Auth/*.md           [CODE]
- [x] docs/components/Workspace/*.md      [CODE]
- [x] docs/components/Content/*.md        [CODE]
- [x] docs/components/Change Requests/*.md [CODE]
- [x] docs/components/Pages/*.md          [CODE]
- [x] docs/components/Text/*.md           [CODE]
- [x] docs/components/Rich Content/*.md   [CODE]
- [x] docs/components/Compose/*.md        [CODE]
- [x] docs/components/Presets/*.md        [CODE]
- [x] docs/examples/*.md                  [CODE] one per demo workflow
- [x] docs/changelog.md                   [CODE] starts at 1.0.0

## PHASE 9 — GITHUB

- [x] README.md                      [CODE] may link to docs/
- [x] CLAUDE.md                      [CODE]
- [x] LICENSE                        [CODE]
- [x] .gitignore                     [CODE]

## PHASE 10 — PACKAGE

- [x] dotnet build -c Release x3     [CODE] Rhino 7 (net48), Rhino 8 Win (net7.0-windows), Rhino 8 Mac (net7.0)
- [x] yak build x3                   [CODE]
- [x] .github/workflows/publish.yml  [CODE] + build.yml
- [x] Verify: .yak files in /dist/

[HUMAN GATE 6] Rhino manual test — demos pre-built & pre-checked; open demos/ and confirm live in Grasshopper
[HUMAN GATE 7] Cross-platform test — install .yak on Rh7 Win / Rh8 Win / Rh8 Mac

<!-- pr:begin header -->
# GitBookConnect — CLAUDE.md

Generated Grasshopper REST plugin for the GitBook API, built with the **PleaseREST** pipeline (`please-rest` plugin, v1.0.0).

- **Commands:** `/rest-status` (progress tree), `/rest-resume-plugin`, `/rest-add-feature`, `/rest-build-component <id>`, `/rest-icons`, `/rest-package`, `/rest-test-api`
- **Skills:** `pr-conventions` (always), `pr-component`, `pr-button`, `pr-builders`, `pr-client`, `pr-assembly`, `pr-preset`, `pr-utilities`, `pr-bulk-data`, `pr-unit-tests`, `pr-build`, `pr-docs`, `pr-gh-package`, `pr-yak-package`, `pr-ci-template`, `pr-rhino-test`, `pr-session`, `pr-dogfood`
- **Agents:** `pr-api-scout` (failing endpoint), `pr-aec-advisor` (new AEC feature), `pr-spec-critic` (new components via `/rest-add-feature`)
- **MCPs:** `pr-api-extractor`, `pr-api-tester`, `pr-icon-generator`
- **Build:** `dotnet build GitBookConnect.csproj` (net48 / net7.0-windows / net7.0). Grasshopper loads `bin/Debug/net7.0-windows/GitBookConnect.gha` (Rhino 8) or `bin/Debug/net48/` (Rhino 7) via its developer-folder setting — **close Rhino before building** (it locks the file). Packaging builds Release.
- **Tests:** `dotnet test GitBookConnect.Tests/GitBookConnect.Tests.csproj` — pure builder logic, net8.0, no Rhino needed.
<!-- pr:end header -->

<!-- pr:begin state -->
## Current build state

- **Version:** 1.0.0 — ready to ship; not yet pushed or tagged
- **Phase:** 15 — Package + Ship. Code (46 components), unit tests (43), `docs/`, README/LICENSE/.gitignore, `manifest.yml`, CI workflows and the six release artifacts in `dist/` are complete. Local git repo initialised with one commit; no remote yet.
- **Last completed step:** publish-readiness pass (2026-10-26) — csproj `<Version>1.0.0</Version>` + Release stripping + manifest/logo copy; `GitBookConnectInfo.cs` per `pr-assembly` (logo icon, version read from the assembly); `manifest.yml`; `.github/workflows/build.yml` + `publish.yml`; `GitBookConnect.Tests/BuilderTests.cs` + `BulkAssemblyTests.cs`; docs audited against the component source (Build Page (JSON) doc moved to the Rich Content tab, `docs/quickstart.md` and `docs/examples/` added, changelog 1.0.0 dated); Build Insert File's `Ref` input made optional (matches its description; no GUID or parameter-order change); `dist/` = 3 `.yak` + 3 Libraries-folder `.zip`.
- **Next step:** create https://github.com/eesha-on-jupiter/GitBookConnect, add it as `origin` and push `main` (completes Phase 14); Gate 6 live test in Rhino (build the demo canvases from `demos/README.md` / `docs/examples/`; check Build Page (JSON) end to end); Gate 7 install test of the `.yak` files (Mac untested); then tag `v1.0.0`, `yak push` x3, Food4Rhino listing, and backfill the README / `docs/index.md` placeholders.
- **Last active:** 2026-10-26
<!-- pr:end state -->

<!-- pr:begin platform -->
## Platform context

- **Base URL:** `https://api.gitbook.com/v1`
- **Auth method:** Bearer token — **must be a GitBook Personal Access Token**, not an integration token. Verified per-endpoint in GitBook's openapi.json: `create_change_request`, `apply_change_request_content`, and `request_reviewers` only accept the `user` security scheme; `integration`/`integration-installation` are explicitly absent for these three, unlike every read endpoint. `merge_change_request` additionally requires the `change-request:merge` OAuth scope specifically. A personal token still inherits the underlying user's actual space role (Admin/Editor/Reviewer/Viewer) — a Reviewer/Viewer-role user is still blocked from writes.
- **API version:** v1 (OpenAPI 0.0.1-beta), live spec at `https://api.gitbook.com/openapi.json`
- **Resource hierarchy:**
  ```
  Organization
    -> Collection (-> child Collections, -> Spaces)
    -> Space
         -> Content (published, read-only)
         -> ChangeRequest (the write branch)
              -> ChangeRequestComment (+ replies)
              -> Review
         -> Revision (published history, read-only)
    -> DocsSite (publishes one or more Spaces publicly)
  ```
- **Content write model:** GitBook has no direct "write a page" endpoint. Writes go through a Change Request flow — analogous to a git branch + PR + merge: `create_change_request` → `apply_change_request_content` (the one batch write call, up to 50 changes/request) → optional `request_reviewers` → `merge_change_request` (irreversible publish) or leave unmerged as a draft.
<!-- pr:end platform -->

<!-- pr:begin structure -->
## Plugin structure

9 ribbon tabs, 46 components (plus Utilities plumbing: client, button base class, plugin utilities):

| Subcategory | Components | Notes |
|---|---|---|
| Auth | 1 | `Connect to GitBook` — Token → Status + Response (`GET /user`) |
| Workspace | 6 | Organizations/Spaces/Collections list+get — read-only navigation |
| Content | 4 | Published/live content — read-only |
| Change Requests | 9 | Create/list/get/update/sync/request-reviewers/merge, draft content, Apply Content Changes (the one batch write) |
| Pages | 5 | Build Page (Markdown), Insert Page, Update Page, Build Pages From Tree, Markdown Table From Grid |
| Text | 9 | Plain Text + chainable marks — all 8 real `DocumentTextMark` values; GitBook has no underline |
| Rich Content | 7 | Paragraph, heading, hint, expandable, cards, table blocks + Build Page (JSON) (source file in `Components/Pages`) |
| Compose | 3 | Build Insert File (path / Bitmap / URL / base64 / viewport capture), Blocks Aggregator, Content Changes Aggregator |
| Presets | 2 | `Hint Style`, `Text Color` (GH_ValueList) |

Every HTTP component and Build Insert File extends `ButtonComponent`. The full component table (inputs/outputs) is generated into `README.md`.

**Skipped for v1** (see `spec/plugin-spec.json` → `skipped_for_v1`): underline mark (not in GitBook's schema); create/update space or collection; DELETE-class endpoints; comments/reviews; docs-site publishing; file browsing; ~326 of 365 GitBook operations with no stated workflow.
<!-- pr:end structure -->

<!-- pr:begin builders -->
## JSON builders (`JSON Builders/`)

| File | Covers | Used by |
|---|---|---|
| `PageBuilders.cs` | Page (Document JSON + insert/update-page ops) | BuildPageMarkdown, InsertPage, UpdatePage, BuildPagesFromTree, BuildRichPage |
| `FileBuilders.cs` | File (insert_files op) | BuildInsertFile |
| `TableBuilders.cs` | Table (Markdown + JSON block paths) | MarkdownTableFromGrid, TableBlock |
| `TextMarkBuilders.cs` | Text / Mark (leaf nodes + all 8 marks) | PlainText, BoldMark, ItalicMark, StrikethroughMark, CodeMark, KeyboardMark, SuperscriptMark, SubscriptMark, ColorMark |
| `BlockBuilders.cs` | Block (paragraph/heading/hint/expandable/cards + blocks-array wrapping) | ParagraphBlock, HeadingBlock, BuildHintBlock, BuildExpandableBlock, BuildCardsBlock, PageBlocksAggregator, BuildRichPage |
<!-- pr:end builders -->

<!-- pr:begin decisions -->
## Key design decisions

- **JSON-first content model.** Rich, styled JSON pages (via the block/mark builders) are the primary content path; Markdown is a secondary, simpler path (`BuildPageMarkdown`). Both converge on the same `PageBuilders.cs` Document JSON construction.
- **Two-level aggregation for writes.** Level-1 aggregators (`PageBlocksAggregator`, page builders) assemble one resource's Change JSON; the Level-2 `ContentChangesAggregator` then flattens Change JSON from *both* Pages and Compose (file inserts) into the single ordered, 50-item-capped batch `apply_change_request_content` actually accepts — this mirrors GitBook's real API shape (one batch call, not one call per change).
- **Build Insert File is source-agnostic.** File Path, Bitmap, Source URL, Base64, and Rhino viewport capture are all inputs on one component rather than five separate components (or a separate "Document Component" for screenshots) — decided at Gate 2 (F3 in dogfood log).
- **`URL` output on Create/Get Change Request.** GitBook's ChangeRequest response includes `urls.app`, a direct web link to view/edit the CR — added as a bonus output after live verification (dogfood F5).
- **`Merge Change Request` kept despite the user not currently needing it** — user chose to keep the component in scope rather than remove it.
<!-- pr:end decisions -->

<!-- pr:begin limitations -->
## Known limitations

- **Merge/write auth restriction is structural, not a plugin bug.** Change Request writes require a Personal Access Token; an integration token is rejected by GitBook itself regardless of scope. Documented in the Auth component and `docs/authentication.md`.
- **`Get Collection` may return nothing in workspaces with zero collections** — GitBook's "collections" grouping feature is optional and many orgs never use it; there's no `Create Collection` component in v1 scope to seed one from within the plugin (see `spec/todo.md` skipped_for_v1 rationale — no confirmed workflow needed it).
- **`Sync Change Request` returning HTTP 409 ("Branch is already up-to-date") is expected**, not an error — occurs whenever the change request has nothing new to rebase onto from the primary branch.
- **pr-api-tester tooling gaps found during Phase 7** (harvest logic, empty example bodies on write endpoints, non-2xx-as-valid-response support) were patched in PleaseREST's shared `pr-api-tester/server.py` this run but need an MCP server restart to take effect automatically — see `dogfood/GitBookConnect-run-01.md` findings F10–F13 for the full detail. Not a limitation of GitBookConnect itself.
- **Gate 4 diagnostics created real, permanent test data** in the live GitBook account used for validation: 1 collection ("API Test Collection"), 2 change requests, and 1 merged page ("API Test Page", path `api-test-page`) — flagged to the user, not yet cleaned up.
- **JSON-block page path not validated live.** Only the Markdown page path (`insert_page` with `document.markdown`) was exercised against the API; Build Page (JSON) and the block/mark builders follow GitBook's documented content model (dogfood F15). Gate 6 must check it end to end.
- **Rhino 8 Mac package is untested.** It builds (35 expected CA1416 warnings on the net7.0 target) but has not been installed on a Mac.
- **No demo `.gh` files yet.** Phase 12 ran without a Grasshopper bridge; `demos/README.md` (gitignored) and `docs/examples/*.md` specify the canvases.
<!-- pr:end limitations -->

<!-- pr:begin todo -->
## Todo (spec/todo.md)

```markdown
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
```
<!-- pr:end todo -->

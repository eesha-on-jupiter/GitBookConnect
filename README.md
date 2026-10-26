<!-- pr:begin header -->
<img src="GC_Plugin_Icon.png" alt="GitBookConnect" width="96" align="left" style="margin-right:16px">

# GitBookConnect
> Grasshopper plugin for the GitBook REST API — by Eesha Jain
>
> Built with the [PleaseREST](https://github.com/eesha-on-jupiter/PleaseREST) Claude Code plugin — an AI pipeline that turns a REST API reference into a Grasshopper plugin.

Publish GitBook documentation from a Grasshopper canvas: build pages from model data as Markdown or as rich, styled blocks, batch them into one Change Request, and review and merge them without leaving Grasshopper.

<br clear="left">

## What you can do with it

Keep project documentation in GitBook in step with the model, and regenerate it whenever the design changes:

- **One page per level, zone or element type** — a Grasshopper data tree becomes a set of GitBook pages, created or updated together in one Change Request.
- **Schedules and results tables** — a rows x columns grid (areas, takeoffs, load cases) becomes a table on a page, as GitBook table blocks or Markdown.
- **Styled pages** — headings, hints, expandables, cards and bold, coloured or code-formatted text, built from components rather than typed by hand.
- **Rhino views and images on the page** — capture a viewport, or use a file, Bitmap or URL, and place it in the page in the same batch.
- **Living docs with a review step** — find existing pages by path and rewrite them when the model changes; every change lands in a Change Request draft you review at its URL before merging.

Component chains for each of these are in [docs/workflows.md](docs/workflows.md); step-by-step examples are in [docs/examples/](docs/examples/index.md).

**Under the hood**

- Every write goes through GitBook's Change Request flow — Create Change Request → Apply Content Changes → Merge Change Request. Nothing is live until you merge, and a merge cannot be undone through the API.
- Pages, Text, Rich Content and Compose components only build JSON; **Content Changes Aggregator** gathers page and file changes into the single batch (up to 50) that Apply Content Changes sends.
- Every component that calls GitBook is a button — nothing hits your workspace on canvas recompute.
<!-- pr:end header -->

<!-- pr:begin install -->
## Installation

**Food4Rhino:** (pending first release)
**Yak:** `_PackageManager` → search `gitbookconnect` (pending first release)

**Rhino 8 note:** the Rhino 8 packages are built for .NET 7 and load only when Rhino runs on the .NET Core runtime (its default). If the GitBookConnect tab is missing, run `SetDotNetRuntime` in Rhino, choose **.NET Core**, and restart — Rhino.Inside Revit setups often switch this to .NET Framework.

**From source:** `dotnet build GitBookConnect.csproj`, then drop the `.gha` for your Rhino version into Grasshopper's `Libraries` folder (Rhino 7: `bin/Debug/net48`; Rhino 8 Windows: `bin/Debug/net7.0-windows`; Rhino 8 Mac: `bin/Debug/net7.0`) and restart Rhino — or point Grasshopper's developer-folder setting at the build folder. Close Rhino before rebuilding; it locks the `.gha`.
<!-- pr:end install -->

<!-- pr:begin quickstart -->
## Quick start

1. Create a GitBook **personal access token** (Settings → Developer settings → Personal access tokens) — integration tokens are rejected on writes. See [docs/authentication.md](docs/authentication.md).
2. Drop **Connect to GitBook** (Auth tab), plug the token into `Token` and click **Connect** — `Status` reads `Connected.`
3. **List Organizations** → **List Spaces** (Workspace tab) to find the `Space ID` of a space you can edit.
4. Author a page: **Build Page (Markdown)** (Pages tab), or Text marks → Rich Content blocks → **Blocks Aggregator** → **Build Page (JSON)**; then **Insert Page** for its `Change JSON`.
5. Wire every `Change JSON` into **Content Changes Aggregator** (Compose tab), in the order to apply.
6. **Create Change Request** (click **Create**), then **Apply Content Changes** with its `Change Request ID` and the aggregator output — click **Apply**, review at the Change Request `URL`, and **Merge Change Request** when ready.

Full walkthrough: [docs/quickstart.md](docs/quickstart.md). Common wirings as component chains: [docs/workflows.md](docs/workflows.md). Reference for every component: [docs/index.md](docs/index.md).
<!-- pr:end quickstart -->

<!-- pr:begin components -->
## Components

| Tab | Component | Inputs | Outputs | Description |
|---|---|---|---|---|
| Auth | Connect to GitBook | Token | Status, Response | Validates a GitBook Personal Access Token via GET /user and reports the connected account. |
| Workspace | Get Collection | Token, Collection ID | Status, Response | Retrieves one collection by ID. |
| Workspace | Get Organization | Token, Organization ID | Status, Response | Retrieves one organization by ID. |
| Workspace | Get Space | Token, Space ID | Status, Response | Retrieves one space by ID. |
| Workspace | List Collections | Token, Organization ID | Status, Response | Lists every collection in an organization. Returns an empty list if the organization never used GitBook's collections feature. |
| Workspace | List Organizations | Token | Status, Response | Lists every organization the token's account belongs to. |
| Workspace | List Spaces | Token, Organization ID | Status, Response | Lists every space in an organization. |
| Content | Find Page By Path | Token, Space ID, Page Path | Status, Response | Retrieves one published page by its path (slug) rather than its ID. |
| Content | Get Page | Token, Space ID, Page ID | Status, Response | Retrieves one published page by ID. |
| Content | Get Published Content | Token, Space ID | Status, Response | Retrieves a space's currently published (merged) content. |
| Content | List Pages | Token, Space ID | Status, Response | Lists every page in a space's published content. |
| Change Requests | Apply Content Changes | Token, Space ID, Change Request ID, Change JSON | Status, Response | Applies a batch of Change JSON operations to a Change Request. Wire the Content Changes Aggregator output here. |
| Change Requests | Create Change Request | Token, Space ID, Subject, Description | Change Request ID, URL, Status, Response | Opens a new Change Request (the write branch) on a space. A Description, if given, is set with a follow-up Update Change Request call since GitBook does not accept one at creation time. |
| Change Requests | Get Change Request | Token, Space ID, Change Request ID | URL, Status, Response | Retrieves one Change Request by ID. |
| Change Requests | Get Change Request Content | Token, Space ID, Change Request ID | Status, Response | Retrieves the draft content tree of a Change Request. |
| Change Requests | List Change Requests | Token, Space ID | Status, Response | Lists every Change Request on a space. |
| Change Requests | Merge Change Request | Token, Space ID, Change Request ID | Status, Response | Publishes a Change Request's draft content into the space's live content. Irreversible. |
| Change Requests | Request Reviewers | Token, Space ID, Change Request ID, Reviewer User ID | Status, Response | Requests one or more reviewers for a Change Request, by GitBook user ID. |
| Change Requests | Sync Change Request | Token, Space ID, Change Request ID | Status, Response | Rebases a Change Request on the space's latest live content before merging. |
| Change Requests | Update Change Request | Token, Space ID, Change Request ID, Subject, Description | Status, Response | Updates an existing Change Request's subject and/or description. |
| Pages | Build Page (Markdown) | Title, Markdown Body, Icon | Document JSON | Builds a page's Document JSON from a plain Markdown body. Prefer Build Page (JSON) for styled text, blocks, and tables. |
| Pages | Build Pages From Tree | Into, Page ID, Title, Body Text, Icon | Change JSON | Builds one Change JSON op per branch item from parallel trees of page properties — insert_page when Page ID is blank, update_page when it is set. |
| Pages | Insert Page | Document JSON, Into, At, Ref | Change JSON | Builds one insert_page Change JSON op per Document JSON item, index-matched against Into/At/Ref. |
| Pages | Markdown Table From Grid | Header Row, Grid | Markdown Body | Builds a Markdown table from a Header Row and a rows x columns Grid tree. Prefer Table Block for the JSON path. |
| Pages | Update Page | Page ID, Document JSON, Into, At | Change JSON | Builds one update_page Change JSON op per Page ID, index-matched against Document JSON/Into/At. |
| Text | Bold Mark | Text JSON | Text JSON | Adds a bold mark to a Text JSON leaf. |
| Text | Code Mark | Text JSON | Text JSON | Adds an inline code mark to a Text JSON leaf. |
| Text | Color Mark | Text JSON, Text Color, Background Color | Text JSON | Adds a color mark to a Text JSON leaf. Background Color is GitBook's equivalent of a highlight. |
| Text | Italic Mark | Text JSON | Text JSON | Adds an italic mark to a Text JSON leaf. |
| Text | Keyboard Mark | Text JSON | Text JSON | Adds a keyboard-key mark to a Text JSON leaf. |
| Text | Plain Text | Text | Text JSON | Wraps a raw string into a Text JSON leaf with no marks. Chain Mark components after it, or use it unstyled. |
| Text | Strikethrough Mark | Text JSON | Text JSON | Adds a strikethrough mark to a Text JSON leaf. |
| Text | Subscript Mark | Text JSON | Text JSON | Adds a subscript mark to a Text JSON leaf. |
| Text | Superscript Mark | Text JSON | Text JSON | Adds a superscript mark to a Text JSON leaf. |
| Rich Content | Build Page (JSON) | Title, Blocks JSON | Document JSON | Builds a page's Document JSON from assembled Blocks JSON. Primary page-authoring path. |
| Rich Content | Cards Block | Card Title, Card Text, Card Icon | Block JSON | Builds a GitBook cards (columns) block from parallel lists, one item per card. |
| Rich Content | Expandable Block | Title, Text JSON, Default Expanded | Block JSON | Builds an expandable/collapsible block from styled Text JSON runs. |
| Rich Content | Heading Block | Text JSON, Level | Block JSON | Wraps one or more styled Text JSON runs into a heading block. |
| Rich Content | Hint Block | Style, Text JSON | Block JSON | Builds a hint/callout block from styled Text JSON runs. |
| Rich Content | Paragraph Block | Text JSON | Block JSON | Wraps one or more styled Text JSON runs into a paragraph block. |
| Rich Content | Table Block | Header Row, Grid | Block JSON | Builds a GitBook grid-view table block from a Header Row and a rows x columns Grid tree. |
| Compose | Blocks Aggregator | Block JSON | Blocks JSON | Gathers Block JSON from any Rich Content block or Build Insert File into the Blocks JSON list that Build Page (JSON) consumes. |
| Compose | Build Insert File | File Name, File Path, Bitmap, Source URL, Base64, Capture Rhino Viewport, View Name, Ref | Change JSON, Block JSON | Builds an image/file insert from whichever source is provided per item (File Path, Bitmap, Source URL, Base64, or a captured Rhino viewport). Bundles all items into one insert_files Change JSON and emits one Image Block per item. |
| Compose | Content Changes Aggregator | Change JSON | Change JSON | Flattens every wired Change JSON into one ordered batch, capped at the API's 50-per-call limit. |
| Presets | Hint Style | — | Value list | GitBook hint/callout styles. |
| Presets | Text Color | — | Value list | GitBook color tokens, for Color Mark's Text Color / Background Color inputs. |
<!-- pr:end components -->

<!-- pr:begin requirements -->
## Requirements

- Rhino 7 or 8 (Windows or Mac)
- A GitBook account with access to the organization and space you want to write to
- A GitBook **personal access token** — no OAuth scopes to pick; it carries your own role on each space:
  - Any role — every read component (Workspace, Content, List/Get Change Request)
  - Editor or higher on the space — Create / Update Change Request, Apply Content Changes
  - Permission to merge on the space — Merge Change Request (OAuth tokens additionally need the `change-request:merge` scope)

## Documentation

- [docs/index.md](docs/index.md) — overview and every component by tab
- [docs/quickstart.md](docs/quickstart.md) — first page in seven steps
- [docs/workflows.md](docs/workflows.md) — common wirings as component chains
- [docs/authentication.md](docs/authentication.md) — tokens and permissions
- [docs/examples/](docs/examples/index.md) — step-by-step examples
- [docs/changelog.md](docs/changelog.md)

## Development

```
dotnet build GitBookConnect.csproj                               # net48, net7.0-windows, net7.0
dotnet test  GitBookConnect.Tests/GitBookConnect.Tests.csproj    # builder logic, no Rhino needed
```

This plugin was generated with the **PleaseREST** Claude Code plugin (`please-rest` v1.0.0); `CLAUDE.md` holds the build state and design notes.
<!-- pr:end requirements -->

<!-- pr:begin license -->
## License

GitBookConnect is released under the MIT License — Copyright (c) 2026 Eesha Jain. See [LICENSE](LICENSE).
<!-- pr:end license -->

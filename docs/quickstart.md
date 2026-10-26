# Quickstart

Seven components, one canvas: validate a token, find a space, build a page, and write it to a Change Request.

## 1. Get a token

Follow [authentication.md](authentication.md) to create a GitBook **personal access token** (integration tokens are rejected on writes). Put it on a Grasshopper Panel so you can wire it into every HTTP component.

## 2. Verify the connection

Drop **Auth > Connect to GitBook** ([GitBookAuth](components/Auth/GitBookAuth.md)). Wire the token into `Token` and click **Connect** — `Status` reads `Connected.`

## 3. Find your space

- **Workspace > List Organizations** ([docs](components/Workspace/GetAllOrganizations.md)) — click **Get**, read an organization `id` from `Response`.
- **Workspace > List Spaces** ([docs](components/Workspace/GetAllSpaces.md)) with that `Organization ID` — click **Get**, read the `id` of a space you can edit. That is your `Space ID`.

## 4. Build a page

- **Pages > Build Page (Markdown)** ([docs](components/Pages/BuildPageMarkdown.md)) — set `Title` and a `Markdown Body` such as `# Hello from Grasshopper`.
- Its `Document JSON` → **Pages > Insert Page** ([docs](components/Pages/InsertPage.md)) → `Change JSON`.
- `Change JSON` → **Compose > Content Changes Aggregator** ([docs](components/Compose/ContentChangesAggregator.md)).

None of these call GitBook; they recompute freely.

## 5. Open a Change Request

Drop **Change Requests > Create Change Request** ([docs](components/Change%20Requests/CreateChangeRequest.md)). Wire `Token`, `Space ID` and a `Subject`, then click **Create**. Keep its `Change Request ID` and `URL`.

## 6. Write the page

Drop **Change Requests > Apply Content Changes** ([docs](components/Change%20Requests/ApplyContentChanges.md)). Wire `Token`, `Space ID`, the `Change Request ID` from step 5 and the aggregator's `Change JSON`. Click **Apply** — `Status` reads `OK`.

Open the `URL` from step 5: the page is in the Change Request draft. Nothing is live yet.

## 7. Publish (optional)

**Change Requests > Merge Change Request** ([docs](components/Change%20Requests/MergeChangeRequest.md)) publishes the draft. A merge cannot be undone through the API, so review first.

## What's next

- Styled pages, tables, images and bulk pages: [workflows.md](workflows.md) and [examples/](examples/index.md).
- Every component, tab by tab: [index.md](index.md#components-by-tab).

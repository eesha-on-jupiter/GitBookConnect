# 05 - Change Request lifecycle

_Demo file pending; built from the plugin spec, not a saved canvas._

**Goal:** open, write, review and merge a Change Request from the canvas.

## Components, in wiring order

1. [Create Change Request](../components/Change%20Requests/CreateChangeRequest.md)
2. [Apply Content Changes](../components/Change%20Requests/ApplyContentChanges.md)
3. [Get Change Request Content](../components/Change%20Requests/GetChangeRequestContent.md) and [Get Change Request](../components/Change%20Requests/GetChangeRequest.md)
4. [Request Reviewers](../components/Change%20Requests/RequestReviewers.md) (optional)
5. [Sync Change Request](../components/Change%20Requests/SyncChangeRequest.md)
6. [Merge Change Request](../components/Change%20Requests/MergeChangeRequest.md)

## Wiring

- Create Change Request: `Token`, `Space ID`, `Subject`, `Description`. Click **Create**; read `Change Request ID` and `URL`.
- Wire any Change JSON (e.g. [example 01](01-batch-publish-pages.md)'s aggregator output) → Apply Content Changes `Change JSON`. Click **Apply**.
- Get Change Request Content with the same `Change Request ID` to read the draft; Get Change Request for its `URL`.
- Optional: user IDs → Request Reviewers `Reviewer User ID`. Click **Request**.
- Sync Change Request (click **Sync**; `Already up-to-date.` is fine), then Merge Change Request (click **Merge**).

## Inputs you set

- `Token`, `Space ID`, subject and description, reviewer user IDs.

## Result

- Applied changes stay in the draft until Merge; merging publishes them and cannot be undone through the API.

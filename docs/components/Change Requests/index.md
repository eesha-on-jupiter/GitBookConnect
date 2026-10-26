# Change Requests

GitBook's write path: open a Change Request, apply content to it, review, sync and merge. Every component is button-triggered.

| Component | Description |
|---|---|
| [Create Change Request](CreateChangeRequest.md) | Opens a new Change Request (the write branch) on a space. |
| [List Change Requests](GetAllChangeRequests.md) | Lists every Change Request on a space. |
| [Get Change Request](GetChangeRequest.md) | Retrieves one Change Request by ID. |
| [Update Change Request](UpdateChangeRequest.md) | Updates an existing Change Request's subject and/or description. |
| [Get Change Request Content](GetChangeRequestContent.md) | Retrieves a Change Request's draft content. |
| [Apply Content Changes](ApplyContentChanges.md) | Applies a batch of Change JSON operations to a Change Request in one call. |
| [Request Reviewers](RequestReviewers.md) | Requests one or more reviewers for a Change Request, by GitBook user ID. |
| [Sync Change Request](SyncChangeRequest.md) | Rebases a Change Request on the space's latest live content before merging. |
| [Merge Change Request](MergeChangeRequest.md) | Publishes a Change Request's draft content into the space's live content. |

Nothing reaches live content until [Merge Change Request](MergeChangeRequest.md). Every write needs a Personal Access Token, see [Authentication](../../authentication.md).

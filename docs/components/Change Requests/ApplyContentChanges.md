# Apply Content Changes

Applies a batch of Change JSON operations to a Change Request in one call.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |
| Change JSON | Text (list) | Yes | - | Change JSON operations to apply, in order (up to 50). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an error, `Nothing to apply.` when Change JSON is empty. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`POST /spaces/{spaceId}/change-requests/{changeRequestId}/content`

## Notes

- Button-triggered (button label `Apply`).
- This is the only content write in the plugin: pages and files reach GitBook only through this call.
- Each item must be a JSON object; the list is sent as `{"changes": [...]}`.
- GitBook accepts at most 50 changes per call: wire the output of Content Changes Aggregator here, which caps the batch at 50.
- Changes land in the Change Request draft only. Nothing goes live until Merge Change Request.

## Related

- [Content Changes Aggregator](../Compose/ContentChangesAggregator.md): builds the batch.
- [Insert Page](../Pages/InsertPage.md), [Update Page](../Pages/UpdatePage.md), [Build Pages From Tree](../Pages/BuildPagesFromTree.md), [Build Insert File](../Compose/BuildInsertFile.md): Change JSON sources.
- [Merge Change Request](MergeChangeRequest.md): publish the result.

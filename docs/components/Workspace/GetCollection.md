# Get Collection

Retrieves one GitBook collection by ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Collection ID | Text | Yes | - | ID of the collection (from List Collections). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /collections/{collectionId}`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- GitBookConnect cannot create collections (out of scope for v1).

## Related

- [List Collections](GetAllCollections.md)

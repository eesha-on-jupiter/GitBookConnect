# List Organizations

Lists every GitBook organization the token can access.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /orgs`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Read the organization `id` from Response and wire it into List Spaces or List Collections.

## Related

- [Get Organization](GetOrganization.md)
- [List Spaces](GetAllSpaces.md)
- [List Collections](GetAllCollections.md)

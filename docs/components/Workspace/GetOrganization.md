# Get Organization

Retrieves one GitBook organization by ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Organization ID | Text | Yes | - | ID of the GitBook organization (from List Organizations). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /orgs/{organizationId}`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Returns `Failed.` with `Organization ID is empty.` when no ID is wired.

## Related

- [List Organizations](GetAllOrganizations.md)

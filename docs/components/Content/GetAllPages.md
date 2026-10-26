# List Pages

Lists every page in a space's published content.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /spaces/{spaceId}/content/pages`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Use the page `id` values as `Into` (parent) on Insert Page or as `Page ID` on Update Page.

## Related

- [Get Page](GetPage.md)
- [Update Page](../Pages/UpdatePage.md)

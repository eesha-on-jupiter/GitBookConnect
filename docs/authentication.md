# Authentication

GitBookConnect authenticates every request with a GitBook **personal access token**, sent as a Bearer header. Integration tokens are not enough: GitBook only accepts a user token on the Change Request write endpoints.

## 1. Create a personal access token

1. Sign in to [app.gitbook.com](https://app.gitbook.com).
2. Open your account **Settings > Developer settings > Personal access tokens**.
3. Create a token, give it a name, and copy it. GitBook shows the full token once.

## 2. Access and permissions

| Need | Requirement |
|---|---|
| Read organizations, spaces, collections, published content | Any valid token with access to the organization |
| Create, update or apply content to a Change Request | Personal access token, plus an **Editor** (or higher) role on the space |
| Request reviewers | Personal access token |
| Merge a Change Request | Personal access token, plus permission to merge on the space |

- A personal token inherits your own role on each space: a **Reviewer** or **Viewer** role is still blocked from writes.
- An integration token (`integration` / `integration-installation` schemes) is rejected on Create Change Request, Apply Content Changes and Request Reviewers, whatever its scopes.
- If you use OAuth instead of a personal token, merging additionally needs the `change-request:merge` scope.

## 3. Wiring the token in

- Every HTTP component has a `Token` input. Wire one Panel holding the token into all of them.
- JSON-builder components (Pages, Text, Rich Content, Compose, Presets) take no token.
- Keep the token out of shared `.gh` files: internalised Panel text is saved with the definition.

## 4. Verifying it works

- Drop [Connect to GitBook](components/Auth/GitBookAuth.md) (Auth tab), wire the token into `Token`, and click **Connect**.
- `Status` reads `Connected.` and `Response` holds your account from `GET /user`.
- `Failed to authenticate.` means the token is wrong or revoked; `Response` carries the HTTP error.
- Next, run [List Organizations](components/Workspace/GetAllOrganizations.md) and [List Spaces](components/Workspace/GetAllSpaces.md) to find the `Space ID` the write components need.

## 5. Common errors

| Symptom | Cause |
|---|---|
| `HTTP 401` on any call | Token missing, mistyped or revoked |
| `HTTP 403` on Create Change Request or Apply Content Changes | Integration token used, or your space role is Reviewer/Viewer |
| `HTTP 404` | Wrong ID, or the space belongs to an organization your account is not in |
| `Already up-to-date.` from Sync Change Request | Not an error: GitBook's HTTP 409 when there is nothing to rebase |

## API version

All requests go to `https://api.gitbook.com/v1`. The version is fixed in the plugin and not configurable from the canvas.

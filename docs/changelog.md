# Changelog

## 1.0.0 (2026-10-26)

Initial release: 46 components on nine tabs.

- **Auth tab:** Connect to GitBook validates a personal access token against `GET /user`.
- **Workspace tab:** list and get organizations, spaces and collections.
- **Content tab:** read a space's published content, list its pages, get a page by ID or by path.
- **Change Requests tab:** create, list, get, update, sync and merge Change Requests, read their draft content, request reviewers, and apply a batch of up to 50 content changes in one call.
- **Pages tab:** build page Document JSON from Markdown, build insert_page / update_page Change JSON from lists or from a data tree, and turn a grid tree into a Markdown table.
- **Text tab:** Plain Text plus chainable marks for every GitBook text mark: bold, italic, strikethrough, code, keyboard, superscript, subscript and colour.
- **Rich Content tab:** paragraph, heading, hint, expandable, cards and table blocks, and Build Page (JSON) to assemble them into a page.
- **Compose tab:** Build Insert File (file path, Bitmap, URL, base64 or Rhino viewport capture), Blocks Aggregator and Content Changes Aggregator.
- **Presets tab:** Hint Style and Text Color value lists.
- Every HTTP component, and Build Insert File, runs only on a button click, never on canvas recompute.
- Packages for Rhino 7 Windows, Rhino 8 Windows and Rhino 8 Mac.

### Known limitations

- Writes need a personal access token: integration tokens are rejected by GitBook on Change Request writes.
- Merge Change Request is irreversible through the API.
- No underline mark: GitBook's text-mark schema has none.
- No create/update for spaces or collections, no delete endpoints, no comments or reviews, no docs-site publishing.
- Build Page (JSON)'s document schema was not validated live during the build (only the Markdown page path was): test a styled page end to end before batch use.
- Table Block covers plain-text grid tables only, not typed columns or card views.
- Base64 file content is capped at 1MB by the API.
- The Rhino 8 Mac package builds but has not been tested on a Mac.

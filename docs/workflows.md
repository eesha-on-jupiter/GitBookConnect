# Workflows

What you can do with GitBookConnect — each one turns Grasshopper data into GitBook pages, written through GitBook's Change Request flow.

- **Publish one page per tree branch** (one level, zone or element type each) in a single Change Request — see [example 01](examples/01-batch-publish-pages.md)
  - Build Pages From Tree → Content Changes Aggregator → Apply Content Changes
- **Turn a schedule, takeoff or load-case grid into a table** on a page — see [example 02](examples/02-tabular-table-page.md)
  - Table Block → Blocks Aggregator → Build Page (JSON) → Insert Page → Content Changes Aggregator → Apply Content Changes
  - or Markdown Table From Grid → Build Page (Markdown) → Insert Page → …
- **Write a richly styled page** with headings, hints, expandables, cards and coloured text — see [example 03](examples/03-rich-styled-page.md)
  - Plain Text → Bold Mark / Color Mark … → Paragraph / Heading / Hint / Expandable / Cards Block → Blocks Aggregator → Build Page (JSON) → Insert Page → …
- **Put an image on a page** from a file, a Bitmap, a URL or a live Rhino viewport capture — see [example 04](examples/04-insert-image.md)
  - Build Insert File → (`Block JSON`) Blocks Aggregator → Build Page (JSON) → Insert Page; (`Change JSON` first) → Content Changes Aggregator → Apply Content Changes
- **Open, write, review and merge a Change Request** from the canvas — see [example 05](examples/05-change-request-lifecycle.md)
  - Create Change Request → Apply Content Changes → Get Change Request (URL) → Request Reviewers → Sync Change Request → Merge Change Request
- **Update existing pages in place** when the model changes — see [example 06](examples/06-sync-existing-pages.md)
  - Find Page By Path → Update Page (or Build Pages From Tree with a `Page ID` tree) → Content Changes Aggregator → Apply Content Changes
- **Vary page content per item from computed values** (one spec page per element type, filled from analysis results)
  - compute titles and Markdown per branch → Build Pages From Tree → …, as in example 01

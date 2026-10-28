# 03 - Rich styled page

_Demo file pending; built from the plugin spec, not a saved canvas._

**Goal:** a page with styled inline text, a heading, a hint, an expandable and cards.

## Components, in wiring order

1. [Plain Text](../components/Text/PlainText.md) → [Bold Mark](../components/Text/BoldMark.md) → [Color Mark](../components/Text/ColorMark.md) (Text), with [Text Color](../components/Presets/TextColorPreset.md) (Presets)
2. [Paragraph Block](../components/Rich%20Content/ParagraphBlock.md), [Heading Block](../components/Rich%20Content/HeadingBlock.md), [Hint Block](../components/Rich%20Content/BuildHintBlock.md) with [Hint Style](../components/Presets/HintStylePreset.md), [Expandable Block](../components/Rich%20Content/BuildExpandableBlock.md), [Cards Block](../components/Rich%20Content/BuildCardsBlock.md) (Rich Content)
3. [Blocks Aggregator](../components/Compose/PageBlocksAggregator.md) (Compose)
4. [Build Page (JSON)](../components/Rich%20Content/BuildRichPage.md) (Rich Content)
5. [Insert Page](../components/Pages/InsertPage.md) (Pages) → [Content Changes Aggregator](../components/Compose/ContentChangesAggregator.md) → [Apply Content Changes](../components/Change%20Requests/ApplyContentChanges.md)

## Wiring

- Panel → Plain Text `Text`; its `Text JSON` → Bold Mark `Text JSON` → Color Mark `Text JSON`; a Text Color preset → `Text Color`.
- That run plus a second Plain Text run → Paragraph Block `Text JSON` (a list, in reading order).
- A Plain Text run → Heading Block `Text JSON`, `1` → `Level`.
- A run → Hint Block `Text JSON`, a Hint Style preset → `Style`.
- A run → Expandable Block `Text JSON`, a Panel → `Title`.
- Three-item lists → Cards Block `Card Title`, `Card Text`, `Card Icon`.
- Every `Block JSON`, in order → Blocks Aggregator `Block JSON` → Build Page (JSON) `Blocks JSON` → Insert Page → Content Changes Aggregator → Apply Content Changes.

## Inputs you set

- The text runs, colour and hint style presets, heading level, expandable title, card lists, page title.

## Result

- One styled page in the Change Request draft. GitBook's JSON document schema was not validated live during the build: check this page in the GitBook editor before reusing the chain in bulk.

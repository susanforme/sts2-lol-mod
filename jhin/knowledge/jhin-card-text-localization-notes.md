# Jhin Card Text Localization Notes

## Scope

This note records lessons from fixing Jhin's card-face text formatting and upgrade-preview behavior.

## Core Rules

1. Do not hardcode user-facing card text in C#.
2. Put all card-face text in `JhinMod/localization/*/cards.json`.
3. If a card sentence ends with a full stop and more text follows, use a real newline in JSON: `\n`.
4. For Exhaust cards, the final keyword line should be highlighted:
   - Chinese: `[gold]消耗。[/gold]`
   - English: `[gold]Exhaust.[/gold]`

## Upgrade Preview

Do not write card-face text like `升级后...` or `Upgrade: ...` for normal upgrade behavior.

Use the game's upgrade-preview localization syntax instead:

- `!D!` for damage diff preview
- `!B!` for block diff preview
- `-old-+new+` for text that changes on upgrade

Examples:

```text
造成 !D! 点伤害。
给予目标 -1-+2+ 层标记。
每当你触发华彩，获得 1 点能量-。-+并抽 1 张牌。+
```

```text
Deal !D! damage.
Apply -1-+2+ Mark.
Whenever you trigger Flourish, gain 1 Energy-. -+ and draw 1 card.+
```

## SimpleLoc Requirement

The project must enable BaseLib SimpleLoc so the shorthand localization syntax is processed.

Current required setup in `src/MainFile.cs`:

```csharp
SimpleLoc.EnableSimpleLoc(ModId);
```

Without that, `!D!`, `!B!`, and `-old-+new+` will not render correctly.

## Newline Pitfall

`NL` is not a supported newline token for this project.

Wrong:

```text
造成伤害。 NL 华彩：...
```

Right:

```text
造成伤害。\n华彩：...
```

## Pure-Damage Flourish Text

If a card's Flourish clause only adds extra damage, do not describe that extra damage on the card face.

Reason:

- That damage is better treated as part of the relic/system-level flourish synergy.
- Leaving it on the card text makes the card text misleading and too system-specific.

Also remove the corresponding card-specific extra damage code, not just the text.

Cards adjusted under this rule:

- `CommonShot`
- `WhisperBurst`
- `PiercingRound`
- `LongRangeSnipe`

Do not apply this rule to non-damage Flourish effects such as:

- gaining energy
- drawing cards
- applying Vulnerable
- other control or utility effects

## Upgrade Swap Syntax Pitfall

Be careful with upgrade-swap text around symbols like `+`.

Bad example:

```text
+-3-++4+
```

This can render incorrectly.

Prefer rewriting the sentence so the swapped number stands alone naturally.

Example:

```text
伤害额外提高 -3-+4+ 点。
If it kills, repeat on another random enemy with -3-+4+ more damage.
```

## Verification

After any card-text change, run:

```powershell
dotnet build "jhin.csproj" -c Debug
powershell -ExecutionPolicy Bypass -File "pack.ps1" -Deploy
```

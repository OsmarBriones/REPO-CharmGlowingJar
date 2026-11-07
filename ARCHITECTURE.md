# CharmGlowingJar Architecture

A Charm mod (layer 3 of the PocketCharms family): one valuable, one power. It depends on **Pockets** (storage) and **CharmCore** (registry, wear, cooldown display, host settings, debug mode).

## The power: Firefly Light

valuables near you are highlighted.

- `GlowingJarCharm : Charm` registers the prefab name `Valuable Wizard Glowing Jar`, the defaults (wear 1.5%, cooldown none s) and any extra host-synced settings.
- `FireflyLight` (a `MonoBehaviour` on the plugin object) detects the moment the power triggers and calls `Charm.TryUse()`.

TODO: describe how the trigger is detected and what game members the effect reads or writes.

## Game members used

TODO: list them; they are checked by `CharmGlowingJar.Tests/GameContractTests.cs`.

## Debug mode

Provided by CharmCore: the `Debug.DebugMode` setting (off by default) places the valuable on the floor at the start of each level and enables `Charm.LogDebug` output.

## Known limits

- Looked up by exact prefab name; a game update that renames the prefab needs a mod update.

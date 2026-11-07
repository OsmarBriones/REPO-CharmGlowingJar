# CharmGlowingJar Architecture

A Charm mod (layer 3 of the PocketCharms family): one valuable, one power. It depends on **Pockets** (storage) and **CharmCore** (registry, wear, cooldown display, host settings, debug mode).

## The power: Firefly Light

While the Glowing Jar is in the local player's pocket, valuables within `Radius` metres of the owner are highlighted on the owner's screen. It is purely local and visual; nothing is sent over the network.

- `GlowingJarCharm : Charm` registers the prefab name `Valuable Wizard Glowing Jar`, sets `WearsPerSecond = true` (default wear 1.5% per second, no cooldown) and declares the settings `Radius` and `RefreshSeconds`.
- `FireflyLight` (a `MonoBehaviour` on the plugin object) runs in `Update` on the owner's machine only while `Charm.IsActive`.
- `Domain/HighlightRules.cs` holds the game-free rules (`IsInRange`, `PickNearest`, `IsDue`, `ScanInterval`), unit tested in `HighlightRulesTests`.

### Flow
1. Every `RefreshSeconds` (a scan timer, not every frame) the effect walks `ValuableDirector.instance.valuableList`. It skips null entries (the game never removes destroyed valuables from that list), valuables without a `PhysGrabObject`, and valuables stored in any pocket (`PocketsApi.IsPocketed`).
2. Distances from `PlayerController.instance.transform.position` go to `HighlightRules.PickNearest`, which keeps those within `Radius`, nearest first, at most 12 per pulse.
3. For each picked valuable that has no live graphic (`IsDue`: at least 0.85 s since the last one, tracked in a dictionary by instance id) it calls `ValuableDiscover.instance.New(physGrabObject, ValuableDiscoverGraphic.State.Reminder)`.
4. Wear: `highlighting` is true when the last scan found at least one valuable in range. While it is true, `Update` calls `Charm.WearForSeconds(Time.deltaTime)`. It is false when the charm is inactive, there is no level (`ValuableDiscover.instance` is null) or nothing is in range.

### What the game does with `New(..., Reminder)`
Read from the decompiled `ValuableDiscover` and `ValuableDiscoverGraphic` and the graphic prefab:
- `New` only instantiates a UI graphic under the HUD canvas and follows the valuable's renderer bounds. It sends no RPC.
- `ValuableObject.Discover` (not used) would send `DiscoverRPC` to everyone, mark the valuable as discovered, add it to the map and award scouting points. We avoid all of that.
- A Reminder graphic lives about 0.8 s: intro 0.2 s (`introSpeed` 5), hold `waitTime` 1 x 0.5 s, outro 0.1 s (`outroSpeed` 10). It then destroys itself. With the default 1.5 s refresh the outline pulses; with `RefreshSeconds` below about 0.85 s the tracked set makes a valuable wait for its previous graphic to expire, so graphics never stack.
- Each new graphic plays a sound at low volume (0.3) at the valuable, hence the cap of 12 per pulse.

### Settings
| Key | Type | Range | Default | Meaning |
|---|---|---|---|---|
| `Radius` | float, metres | 1 to 50 | 15 | Highlight distance from the owner. 0 would make the power useless and is excluded. |
| `RefreshSeconds` | float, seconds | 0.2 to 10 | 1.5 | Time between pulses. 0 is excluded (a graphic every frame); values below 0.85 only mean "as soon as the previous graphic expires". |
| `WearPercent` (CharmCore) | float, percent per second | 0 to 100 | 1.5 | Wear per second while at least one valuable is highlighted. |

## Game members used

- `ValuableDirector.instance` and `ValuableDirector.valuableList` (read): every valuable in the level.
- `ValuableObject.physGrabObject` (read): the target of the graphic. `ValuableObject.dollarValueCurrent`: the value wear reduces (done by CharmCore).
- `ValuableDiscover.instance` and `ValuableDiscover.New(PhysGrabObject, ValuableDiscoverGraphic.State, ValuableDiscoverCustom)` (call): creates the local graphic. `ValuableDiscoverGraphic.State.Reminder` is the state used.
- `PlayerController.instance` (read): the owner's position.
- `PocketsApi.IsPocketed` (Pockets): skip stored valuables.

They are checked by `CharmGlowingJar.Tests/GameContractTests.cs`. The mod needs no Harmony patch.

## Debug mode

Provided by CharmCore: the `Debug.DebugMode` setting (off by default) places the valuable on the floor at the start of each level and enables `Charm.LogDebug` output.

## Known limits

- Looked up by exact prefab name; a game update that renames the prefab needs a mod update.
- Not verified in game: the build and unit tests prove the logic and that the game members exist, not the behaviour in a running game.
- Unverified: whether the outline is visible through walls. By the code it should be (it is a screen-space UI overlay placed from the renderer bounds, and `ValuableDiscoverGraphic` only checks that the point is on screen, with no line-of-sight test), but this was not seen in game.
- Unverified: the exact on-screen duration and look of the Reminder graphic, and how loud the repeated sound is with many valuables.
- The game's own sound plays at each new graphic, so the pulse repeats a quiet sound while valuables are near.
- Valuables already extracted are destroyed by the game and show up as null entries, which are skipped. A valuable in the truck or in a cart is not excluded.

## Decisions

- **No `Discover`:** it is global (RPC, map, scouting points). The effect calls `ValuableDiscover.New` directly so it stays local.
- **Pulse plus tracked set:** the scan runs every `RefreshSeconds`; a per-valuable timestamp (0.85 s) stops duplicate graphics when the host sets a very short refresh.
- **Cap of 12 graphics per pulse (constant, not a setting):** each graphic plays a sound and draws four corner images. Nearest valuables win. Raise it in code if it proves too low.
- **Wear follows the last scan:** wear runs per second between scans and stops within one refresh interval after the last valuable leaves range or is picked up.
- **Distance:** straight-line distance from the player's transform to the valuable's transform, the same measure the game uses for its own reminders. No line-of-sight filter, because seeing through walls is the point of the charm.
- **Pocketed valuables skipped:** including the owner's own jar and other players' pockets.

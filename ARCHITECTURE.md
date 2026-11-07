# CharmGlowingJar Architecture

A Charm mod (layer 3 of the PocketCharms family): one valuable, one power. It depends on **Pockets** (storage) and **CharmCore** (registry, wear, cooldown display, host settings, debug mode).

## The power: Firefly Light

While the Glowing Jar is in the local player's pocket, a flash highlights, on the owner's screen only, the valuables within `Radius` metres that the charm has not lit up in the last `RememberSeconds`. It is purely local and visual; nothing is sent over the network.

- `GlowingJarCharm : Charm` registers the prefab name `Valuable Wizard Glowing Jar`, a default wear of 3% per flash and a default cooldown of 8 s (both set through CharmCore, so the slot shows the cooldown veil), and declares the settings `Radius` and `RememberSeconds`.
- `FireflyLight` (a `MonoBehaviour` on the plugin object) runs in `Update` on the owner's machine only while `Charm.IsActive`.
- `Domain/HighlightRules.cs` holds the game-free rules (`IsInRange`, `PickNearest`, `IsDue`), unit tested in `HighlightRulesTests`.

### Flow
1. While the charm is active and ready (`Charm.IsReady`: the cooldown is over), once a second the effect walks `ValuableDirector.instance.valuableList`. It skips null entries (the game never removes destroyed valuables from that list), valuables without a `PhysGrabObject`, and valuables stored in any pocket (`PocketsApi.IsPocketed`). A valuable lit up less than `RememberSeconds` ago (tracked in a dictionary by instance id) gets a negative distance, which `PickNearest` skips.
2. `HighlightRules.PickNearest` keeps the candidates within `Radius`, nearest first, at most 12 per flash. The valuables left out are still new at the next flash.
3. If nothing is picked: nothing happens, no wear and no cooldown.
4. If something is picked: `Charm.TryUse()` starts the cooldown and wears the jar once (CharmCore sends the wear request to the host), the time is recorded for each picked valuable, and for each one `ValuableDiscover.instance.New(physGrabObject, ValuableDiscoverGraphic.State.Reminder)` creates the graphic.

### What the game does with `New(..., Reminder)`
Read from the decompiled `ValuableDiscover` and `ValuableDiscoverGraphic` and the graphic prefab:
- `New` only instantiates a UI graphic under the HUD canvas and follows the valuable's renderer bounds. It sends no RPC.
- `ValuableObject.Discover` (not used) would send `DiscoverRPC` to everyone, mark the valuable as discovered, add it to the map and award scouting points. We avoid all of that.
- A Reminder graphic lives about 0.8 s: intro 0.2 s, hold 0.5 s, outro 0.1 s. It then destroys itself, so the highlight is a brief flash by nature.
- Each new graphic plays a sound at low volume (0.3) at the valuable, hence the cap of 12 per flash.

### Settings
| Key | Type | Range | Default | Meaning |
|---|---|---|---|---|
| `Radius` | float, metres | 1 to 50 | 15 | Highlight distance from the owner. 0 would make the power useless and is excluded. |
| `RememberSeconds` | float, seconds | 0 to 600 | 60 | A lit-up valuable is skipped for this long. 0 means no memory (everything in range every flash). 600 is a sensible upper limit: a level rarely lasts longer. |
| `WearPercent` (CharmCore) | float, percent per flash | 0 to 100 | 3 | Wear for each flash that lit up something. |
| `CooldownSeconds` (CharmCore) | float, seconds | 0 to 600 | 8 | Wait between flashes. 0 means none. |

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
- Unverified: the exact on-screen duration and look of the Reminder graphic, and how loud a flash with many valuables is.
- The game's own sound plays at each new graphic, so a flash plays a quiet sound per valuable lit up (up to 12).
- Valuables already extracted are destroyed by the game and show up as null entries, which are skipped. A valuable in the truck or in a cart is not excluded.

## Decisions

- **No `Discover`:** it is global (RPC, map, scouting points). The effect calls `ValuableDiscover.New` directly so it stays local.
- **Flash, not constant glow (changed after the first test):** the first design redrew the highlight every `RefreshSeconds` and wore the jar per second while anything was in range. It emptied the jar in about 100 seconds, repeated the same highlight on the same valuables, and (with normal wear) ticked the game's break sound. Now a flash only lights valuables that are new to the player, costs wear only then, and has a cooldown.
- **Cap of 12 graphics per pulse (constant, not a setting):** each graphic plays a sound and draws four corner images. Nearest valuables win. Raise it in code if it proves too low.
- **Wear per useful flash:** one wear step per flash that lit up at least one valuable; a flash with nothing new is free.
- **Distance:** straight-line distance from the player's transform to the valuable's transform, the same measure the game uses for its own reminders. No line-of-sight filter, because seeing through walls is the point of the charm.
- **Pocketed valuables skipped:** including the owner's own jar and other players' pockets.

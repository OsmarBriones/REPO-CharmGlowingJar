# Charm Glowing Jar

Firefly Light: while the Glowing Jar is in your pocket, it flashes now and then and lights up the valuables around you that you have not seen lit up yet.

> **Everyone in the lobby must install this mod, together with Pockets and CharmCore.** The power changes what your own character can do and its state shows on your inventory slot, so friends without the mods would see an empty slot and be unable to use the pocket.

## How to Use

1. Find the **Glowing Jar** valuable and store it in a free inventory slot with [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/) (grab it, press the slot key).
2. Nothing else to do. Every few seconds (8 by default, and the slot shows the wait), if there are valuables within 15 metres of you that the jar has not lit up in the last minute, a flash outlines them on your screen, even behind walls or around corners (nearest first, up to 12 at a time). Only you see it. If everything near you was already shown, nothing happens and the jar loses nothing.
3. Take the valuable out of the pocket and the power stops.

## Features

- **Firefly Light:** valuables within 15 m (configurable) that are new to you flash with the same outline the game shows when you grab a valuable. It is only a visual aid for you: it does not reveal anything on the map, does not give scouting points and does not change what other players see. Valuables in a pocket (yours or a friend's) and valuables already extracted are skipped.
- **It costs the valuable, only when it shows you something:** each flash that lights up at least one new valuable wears the jar a little (3% of its original value by default); a flash that finds nothing new costs nothing, and a worn-out valuable breaks.
- **Everything is configurable:** wear, the wait between flashes, range and how long a valuable counts as already seen. The host's choices apply to everyone.

## Requirements
- [BepInEx Pack for R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
- [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/)
- [CharmCore](https://thunderstore.io/c/repo/p/OsmarBriones/CharmCore/)

## Installation
1. Install through r2modman / Thunderstore Mod Manager (Pockets and CharmCore come with it), or place `CharmGlowingJar.dll` next to `Pockets.dll` and `CharmCore.dll` in `BepInEx/plugins`. **All players need it.**
2. Launch the game once; the configuration file is created inside `BepInEx/config`.

## Configuration
Settings are in `com.osmar.CharmGlowingJar.cfg` inside `BepInEx/config`, section `Charm.GlowingJar`. The host's choices apply to everyone.

- `Enabled` (default `true`): turn this charm on or off.
- `WearPercent` (default `3`, 0 to 100): share of the jar's original value lost by each flash that lit something up.
- `CooldownSeconds` (default `8`, 0 to 600): wait between flashes. `0` means no wait.
- `Radius` (default `15`, 1 to 50): distance in metres around you in which valuables are highlighted.
- `RememberSeconds` (default `60`, 0 to 600): a valuable the jar lit up is skipped for this long, so a flash only shows new ones. `0` lights up everything in range at every flash.

Section `Debug`: `DebugMode` (default `false`) places a Glowing Jar on the floor in front of you when a level starts (host or singleplayer) and writes extra log lines. For testing. `SpawnNow` (default `false`, needs `DebugMode`): set it to `true` while a level is running to place one right away; it switches itself back off.

## Issues & Bug Reports
The official way to report issues, suggest improvements, or submit feedback is by opening an issue on the official GitHub repository:
👉 [GitHub Issues](https://github.com/OsmarBriones/REPO-CharmGlowingJar/issues)

## Credits
Developed by **Osmar Briones**

# Charm Glowing Jar

Firefly Light: while the Glowing Jar is in your pocket, valuables around you light up on your screen.

> **Everyone in the lobby must install this mod, together with Pockets and CharmCore.** The power changes what your own character can do and its state shows on your inventory slot, so friends without the mods would see an empty slot and be unable to use the pocket.

## How to Use

1. Find the **Glowing Jar** valuable and store it in a free inventory slot with [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/) (grab it, press the slot key).
2. Nothing else to do. Every second or so, the valuables within 15 metres of you are outlined on your screen, even behind walls or around corners (nearest first, up to 12 at a time). Only you see it.
3. Take the valuable out of the pocket and the power stops.

## Features

- **Firefly Light:** valuables within 15 m (configurable) pulse with the same outline the game shows when you grab a valuable. It is only a visual aid for you: it does not reveal anything on the map, does not give scouting points and does not change what other players see. Valuables in a pocket (yours or a friend's) and valuables already extracted are skipped.
- **It costs the valuable:** the jar loses a little value for every second the light works (only while at least one valuable is in range), and a worn-out valuable breaks.
- **Everything is configurable:** wear, range and how often the light pulses. The host's choices apply to everyone.

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
- `WearPercent` (default `1.5`, 0 to 100): share of the jar's original value lost per second while the light is working.
- `Radius` (default `15`, 1 to 50): distance in metres around you in which valuables are highlighted.
- `RefreshSeconds` (default `1.5`, 0.2 to 10): seconds between highlight pulses. Lower values keep the outlines on screen almost constantly but repeat the highlight sound more often.

Section `Debug`: `DebugMode` (default `false`) places a Glowing Jar on the floor in front of you when a level starts (host or singleplayer) and writes extra log lines. For testing.

## Issues & Bug Reports
The official way to report issues, suggest improvements, or submit feedback is by opening an issue on the official GitHub repository:
👉 [GitHub Issues](https://github.com/OsmarBriones/REPO-CharmGlowingJar/issues)

## Credits
Developed by **Osmar Briones**

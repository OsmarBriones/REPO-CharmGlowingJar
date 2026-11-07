# QA battery: Firefly Light (CharmGlowingJar)

Deploy: Pockets, CharmCore and CharmGlowingJar are in the r2modman `Debug` profile (compare hashes with the builds). Restart the game first.

## Quick start with debug mode
Set `DebugMode = true` in the `Debug` section of `BepInEx/config/com.osmar.CharmGlowingJar.cfg` (off by default). When a level starts, a Glowing Jar is placed on the floor about 2.5 m in front of you; the log then also shows the detailed lines. Any level works. To get another one at any moment, set `SpawnNow = true` in the same section while a level is running: the valuable is placed near you and the switch turns itself back off (the file is re-read about once a second).

## Single player
- [ ] Log shows `Charm registered: Glowing Jar - Firefly Light` and `CharmGlowingJar 1.0.0 loaded!`.
- [ ] Store the valuable in a slot: the slot shows its picture and value.
- [ ] With the jar in a slot and valuables within about 15 m (walk to a room with several), outlines pulse on them roughly every 1.5 s, with a soft sound. Log (DebugMode): `Firefly Light pulse: N valuables, M within 15 m, K highlighted`.
- [ ] Walk behind a wall so a valuable is in the next room: note whether its outline is still shown through the wall (unverified by code review).
- [ ] The jar loses value gradually while valuables are in range: about 1.5% of its original value per second. No cooldown veil appears.
- [ ] Walk to an empty area (nothing within 15 m): no outlines, and the jar stops losing value.
- [ ] A valuable stored in another slot (or the jar itself) is never outlined; extracted valuables are not outlined.
- [ ] The map does not gain the valuables you only saw outlined, and no scouting points are awarded.
- [ ] Take the valuable out: the power stops.
- [ ] Use it until the valuable breaks: the slot empties, the power stops, no errors in the log.

## Settings
- [ ] `WearPercent = 0`: outlines work, no wear.
- [ ] `Radius = 50`: valuables far away are outlined (at most 12, nearest first). `Radius = 1`: only valuables within 1 m.
- [ ] `RefreshSeconds = 0.2`: outlines are almost constant, graphics never stack up, the sound repeats often. `RefreshSeconds = 10`: one pulse every 10 s.
- [ ] Charm `Enabled = false`: no effect and no wear. CharmCore `Enabled = false`: nothing works.

## Multiplayer (host and client)
- [ ] The client sees the outlines (only on their own screen, other players see nothing from this jar) and the jar's value drops on both machines.
- [ ] Host settings override the client's.
- [ ] Client leaves and rejoins: no stuck outlines.

Log patterns: `Charm registered`, `threw`, `Exception`.

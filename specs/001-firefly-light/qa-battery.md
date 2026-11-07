# QA battery: Firefly Light (CharmGlowingJar)

Deploy: Pockets, CharmCore and CharmGlowingJar are in the r2modman `Debug` profile (compare hashes with the builds). Restart the game first.

## Quick start with debug mode
Set `DebugMode = true` in the `Debug` section of `BepInEx/config/com.osmar.CharmGlowingJar.cfg` (off by default). When a level starts, a Glowing Jar is placed on the floor about 2.5 m in front of you; the log then also shows the detailed lines. Any level works. To get another one at any moment, set `SpawnNow = true` in the same section while a level is running: the valuable is placed near you and the switch turns itself back off (the file is re-read about once a second).

## Single player
- [ ] Log shows `Charm registered: Glowing Jar - Firefly Light` and `CharmGlowingJar 1.0.0 loaded!`.
- [ ] Store the valuable in a slot: the slot shows its picture and value.
- [ ] With the jar in a slot and valuables within about 15 m (walk to a room with several), a flash outlines them once, with a soft sound. Log (DebugMode): `Firefly Light flash: K new valuable(s) within 15 m, N in the level`. The jar loses about 3% of its original value and a cooldown veil with seconds appears on the slot for 8 s.
- [ ] Walk behind a wall so a valuable is in the next room: note whether its outline is still shown through the wall (unverified by code review).
- [ ] Stay in the same place: after the cooldown the same valuables are NOT flashed again (log: `nothing new within 15 m`) and the jar loses nothing. The value stays put while you stand still, also with the pause menu open.
- [ ] Walk to another room with valuables you have not seen lit up: the next flash (after the cooldown) shows them and wears the jar once more.
- [ ] Walk to an empty area (nothing within 15 m): no flash, no wear.
- [ ] A valuable stored in another slot (or the jar itself) is never outlined; extracted valuables are not outlined.
- [ ] The map does not gain the valuables you only saw outlined, and no scouting points are awarded.
- [ ] Take the valuable out: the power stops.
- [ ] Use it until the valuable breaks: the slot empties, the power stops, no errors in the log.

## Settings
- [ ] `WearPercent = 0`: flashes work, no wear.
- [ ] `Radius = 50`: valuables far away are outlined (at most 12 per flash, nearest first; the rest come at the next flash). `Radius = 1`: only valuables within 1 m.
- [ ] `CooldownSeconds = 0`: no veil, a flash as soon as something new is in range. `CooldownSeconds = 30`: one flash every 30 s.
- [ ] `RememberSeconds = 0`: everything in range is lit up again at every flash (and wears the jar each time). `RememberSeconds = 600`: a valuable is lit up once and not again for ten minutes.
- [ ] Charm `Enabled = false`: no effect and no wear. CharmCore `Enabled = false`: nothing works.

## Multiplayer (host and client)
- [ ] The client sees the outlines (only on their own screen, other players see nothing from this jar) and the jar's value drops on both machines.
- [ ] Host settings override the client's.
- [ ] Client leaves and rejoins: no stuck outlines.

Log patterns: `Charm registered`, `threw`, `Exception`.

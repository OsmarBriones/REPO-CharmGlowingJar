# Changelog

## [Unreleased]

### Changed
- The charm clears its memory of lit-up valuables when a level ends and does not keep references to valuables between scans.

### Added
- Firefly Light: while the Glowing Jar is in the pocket, a flash (every `CooldownSeconds`, default 8, shown on the slot) highlights the valuables within `Radius` metres (default 15) that the charm did not light up in the last `RememberSeconds` (default 60). Only a flash that lights up something new wears the jar (3% by default) and starts the cooldown, so the jar does not drain while nothing new is around (the first design, a constant highlight with wear per second, emptied the jar in about 100 seconds and repeated the highlight on the same valuables).
- Settings `Radius` (1 to 50) and `RememberSeconds` (0 to 600).
- `Debug.DebugMode` (off by default): puts the Glowing Jar on the floor at the start of each level and writes detailed logs.

## User Scenarios

### User Story 1 - Firefly Light (Priority: P1)
A player stores the Glowing Jar in a pocket and gets this power while it stays there: valuables near you are highlighted.
**Acceptance**: with the valuable pocketed the power works; with it out of the pocket it does not; each use wears the valuable and shows the cooldown on the slot.

## Requirements
- FR-001: Registers a charm with CharmCore for the prefab Valuable Wizard Glowing Jar.
- FR-002: Settings through CharmCore: Enabled, WearPercent, CooldownSeconds, DebugMode; host-authoritative except DebugMode.
- FR-003: All players install it (Category 2); depends on Pockets and CharmCore.
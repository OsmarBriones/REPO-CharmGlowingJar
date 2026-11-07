## User Scenarios

### User Story 1 - Firefly Light (Priority: P1)
A player stores the Glowing Jar in a pocket and gets this power while it stays there: valuables within Radius metres of you are highlighted on your screen.
**Acceptance**: with the valuable pocketed the power works; with it out of the pocket it does not; the valuable wears per second while at least one valuable is highlighted (no cooldown).

## Requirements
- FR-001: Registers a charm with CharmCore for the prefab Valuable Wizard Glowing Jar.
- FR-002: Settings through CharmCore: Enabled, WearPercent (per second), Radius, RefreshSeconds, DebugMode; host-authoritative except DebugMode.
- FR-003: All players install it (Category 2); depends on Pockets and CharmCore.
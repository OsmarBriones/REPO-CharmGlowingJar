using CharmCore.Api;

namespace CharmGlowingJar
{
	/// <summary>Firefly Light: a flash that highlights the valuables near you that you have not seen lit up recently.</summary>
	internal sealed class GlowingJarCharm : Charm
	{
		internal const string PrefabName = "Valuable Wizard Glowing Jar";

		internal GlowingJarCharm()
			: base("GlowingJar", "Glowing Jar - Firefly Light", defaultWearPercent: 3f, defaultCooldownSeconds: 8f, PrefabName)
		{
			// WearPercent is per flash that really lit something up (a use), and CooldownSeconds is the pause between flashes.
			Radius = AddSetting("Radius", 15f, 1f, 50f, "How far from you, in metres, valuables are highlighted (1 to 50).");
			RememberSeconds = AddSetting("RememberSeconds", 60f, 0f, 600f, "Valuables this charm lit up in the last N seconds are skipped, so a flash only shows valuables that are new to you (0 to 600). 0 lights up everything in range at every flash.");
		}

		/// <summary>Highlight distance in metres (1 to 50).</summary>
		internal CharmSetting Radius { get; }

		/// <summary>Seconds a valuable stays "already seen" after a flash lit it up (0 to 600).</summary>
		internal CharmSetting RememberSeconds { get; }
	}
}

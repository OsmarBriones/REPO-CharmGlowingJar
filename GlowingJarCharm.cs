using CharmCore.Api;

namespace CharmGlowingJar
{
	/// <summary>Firefly Light: valuables near you are highlighted.</summary>
	internal sealed class GlowingJarCharm : Charm
	{
		internal const string PrefabName = "Valuable Wizard Glowing Jar";

		internal GlowingJarCharm()
			: base("GlowingJar", "Glowing Jar - Firefly Light", defaultWearPercent: 1.5f, defaultCooldownSeconds: null, PrefabName)
		{
			// WearPercent means percent per SECOND while the highlight is working.
			WearsPerSecond = true;
			Radius = AddSetting("Radius", 15f, 1f, 50f, "How far from you, in metres, valuables are highlighted (1 to 50).");
			RefreshSeconds = AddSetting("RefreshSeconds", 1.5f, 0.2f, 10f, "Seconds between highlight pulses (0.2 to 10). Lower values keep the highlight on screen almost constantly but repeat the highlight sound more often.");
		}

		/// <summary>Highlight distance in metres (1 to 50).</summary>
		internal CharmSetting Radius { get; }

		/// <summary>Seconds between highlight pulses (0.2 to 10).</summary>
		internal CharmSetting RefreshSeconds { get; }
	}
}

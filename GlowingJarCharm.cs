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
			// Extra host-synced numbers for this power go here, for example:
			// Amount = AddSetting("Amount", 30f, 1f, 100f, "Description for the config file.");
		}
	}
}

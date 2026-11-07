using System.Collections.Generic;
using CharmGlowingJar.Domain;
using Pockets.Api;
using UnityEngine;

namespace CharmGlowingJar
{
	/// <summary>
	/// Firefly Light: while the Glowing Jar is in the owner's pocket, valuables within <c>Radius</c> metres are highlighted on the
	/// owner's screen only. Every <c>RefreshSeconds</c> the valuables near the owner get a game "Reminder" graphic through
	/// <c>ValuableDiscover.New</c> (a local overlay; it does not mark the valuable as discovered and does not touch the map or scouting
	/// points). The valuable wears per second while there is at least one valuable in range.
	/// </summary>
	internal sealed class FireflyLight : MonoBehaviour
	{
		/// <summary>
		/// Approximate life of one Reminder graphic: intro 0.2 s + wait 0.5 s (waitTime 1 x 0.5) + outro 0.1 s, rounded up. Read from the
		/// graphic prefab; no new graphic is created for the same valuable before this time has passed.
		/// </summary>
		private const float GraphicLifetime = 0.85f;

		/// <summary>Most graphics created per pulse (nearest first): each one plays a sound, so a crowded room would be noisy.</summary>
		private const int MaxPerPulse = 12;

		private readonly List<ValuableObject> candidates = new List<ValuableObject>();
		private readonly List<float> distances = new List<float>();
		private readonly List<int> picked = new List<int>();
		private readonly Dictionary<int, float> lastShown = new Dictionary<int, float>();
		private float nextScan;
		private bool highlighting;

		internal GlowingJarCharm Charm { get; set; } = null!;

		private void Update()
		{
			if (Charm == null || !Charm.IsActive)
			{
				if (highlighting)
				{
					highlighting = false;
					Charm?.LogDebug("Firefly Light stopped (jar out of the pocket or charm off)");
				}

				nextScan = 0f;
				return;
			}

			ValuableDirector director = ValuableDirector.instance;
			PlayerController player = PlayerController.instance;
			if (ValuableDiscover.instance == null || director == null || player == null || !player.isActiveAndEnabled)
			{
				highlighting = false;
				return;
			}

			if (Time.time >= nextScan)
			{
				nextScan = Time.time + HighlightRules.ScanInterval(Charm.RefreshSeconds.Value);
				Scan(director, player.transform.position);
			}

			// Wear only while the power works: highlight on and at least one valuable to show.
			if (highlighting)
			{
				Charm.WearForSeconds(Time.deltaTime);
			}
		}

		private void Scan(ValuableDirector director, Vector3 origin)
		{
			float radius = Charm.Radius.Value;
			List<ValuableObject> all = director.valuableList;
			candidates.Clear();
			distances.Clear();
			for (int index = 0; index < all.Count; index++)
			{
				ValuableObject valuable = all[index];
				// Destroyed (extracted or broken) valuables stay in the game's list as null entries.
				if (valuable == null || valuable.physGrabObject == null || PocketsApi.IsPocketed(valuable))
				{
					continue;
				}

				candidates.Add(valuable);
				distances.Add(Vector3.Distance(origin, valuable.transform.position));
			}

			HighlightRules.PickNearest(distances, radius, MaxPerPulse, picked);
			highlighting = picked.Count > 0;

			int shown = 0;
			foreach (int index in picked)
			{
				ValuableObject valuable = candidates[index];
				int id = valuable.GetInstanceID();
				float last = lastShown.TryGetValue(id, out float value) ? value : float.NaN;
				if (!HighlightRules.IsDue(Time.time, last, GraphicLifetime))
				{
					continue;
				}

				lastShown[id] = Time.time;
				ValuableDiscover.instance.New(valuable.physGrabObject, ValuableDiscoverGraphic.State.Reminder);
				shown++;
			}

			if (lastShown.Count > 512)
			{
				lastShown.Clear();
			}

			Charm.LogDebug($"Firefly Light pulse: {all.Count} valuables, {picked.Count} within {radius:0.#} m, {shown} highlighted");
		}
	}
}

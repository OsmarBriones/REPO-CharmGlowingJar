using System.Collections.Generic;
using CharmGlowingJar.Domain;
using Pockets.Api;
using UnityEngine;

namespace CharmGlowingJar
{
	/// <summary>
	/// Firefly Light: while the Glowing Jar is in the owner's pocket and its cooldown is over, a flash highlights the valuables
	/// within <c>Radius</c> metres that the charm has not lit up in the last <c>RememberSeconds</c>. The highlight is the game's
	/// "Reminder" graphic created through <c>ValuableDiscover.New</c> (a local overlay: it does not mark the valuable as discovered
	/// and does not touch the map or scouting points). A flash that lights nothing new costs nothing and does not start the cooldown.
	/// </summary>
	internal sealed class FireflyLight : MonoBehaviour
	{
		/// <summary>Most graphics created per flash (nearest first): each one plays a sound. The valuables left out are new again at the next flash.</summary>
		private const int MaxPerFlash = 12;

		/// <summary>How often a ready charm looks for new valuables around the player.</summary>
		private const float ScanSeconds = 1f;

		private readonly List<ValuableObject> candidates = new List<ValuableObject>();
		private readonly List<float> distances = new List<float>();
		private readonly List<int> picked = new List<int>();
		private float nextScan;

		internal GlowingJarCharm Charm { get; set; } = null!;

		private void Update()
		{
			if (Charm == null || !Charm.IsActive)
			{
				nextScan = 0f;
				ReleaseScratch();
				return;
			}

			// Cooling down after the last flash: the slot shows the veil, nothing to do.
			if (!Charm.IsReady || Time.time < nextScan)
			{
				return;
			}

			ValuableDirector director = ValuableDirector.instance;
			PlayerController player = PlayerController.instance;
			if (ValuableDiscover.instance == null || director == null || player == null || !player.isActiveAndEnabled)
			{
				return;
			}

			nextScan = Time.time + ScanSeconds;
			Flash(director, player.transform.position);
		}

		private void Flash(ValuableDirector director, Vector3 origin)
		{
			float radius = Charm.Radius.Value;
			float remember = Charm.RememberSeconds.Value;
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

				float last = Charm.LastShown.TryGetValue(valuable.GetInstanceID(), out float value) ? value : float.NaN;
				candidates.Add(valuable);
				// A negative distance makes the valuable skipped: it was lit up recently.
				distances.Add(HighlightRules.IsDue(Time.time, last, remember) ? Vector3.Distance(origin, valuable.transform.position) : -1f);
			}

			HighlightRules.PickNearest(distances, radius, MaxPerFlash, picked);
			if (picked.Count == 0)
			{
				Charm.LogDebug($"Firefly Light: nothing new within {radius:0.#} m ({all.Count} valuables in the level)");
				ReleaseScratch();
				return;
			}

			// Something new to show: start the cooldown and wear the jar.
			if (!Charm.TryUse())
			{
				ReleaseScratch();
				return;
			}

			foreach (int index in picked)
			{
				ValuableObject valuable = candidates[index];
				Charm.LastShown[valuable.GetInstanceID()] = Time.time;
				ValuableDiscover.instance.New(valuable.physGrabObject, ValuableDiscoverGraphic.State.Reminder);
			}

			if (Charm.LastShown.Count > 512)
			{
				Charm.LastShown.Clear();
			}

			Charm.LogDebug($"Firefly Light flash: {picked.Count} new valuable(s) within {radius:0.#} m, {all.Count} in the level");
			ReleaseScratch();
		}

		/// <summary>Empties the working lists so they do not keep references to valuables between scans (or past the end of the level).</summary>
		private void ReleaseScratch()
		{
			candidates.Clear();
			distances.Clear();
			picked.Clear();
		}

		private void OnDestroy()
		{
			ReleaseScratch();
		}
	}
}

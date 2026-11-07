using System.Collections.Generic;

namespace CharmGlowingJar.Domain
{
	/// <summary>Game-free rules of Firefly Light: which valuables to highlight and when. Kept free of Unity and game types so it is unit tested.</summary>
	internal static class HighlightRules
	{
		/// <summary>True when something at <paramref name="distance"/> metres is inside <paramref name="radius"/> (the border counts). A negative distance or radius never matches.</summary>
		internal static bool IsInRange(float distance, float radius)
		{
			return distance >= 0f && radius >= 0f && distance <= radius;
		}

		/// <summary>
		/// Fills <paramref name="result"/> with the indices of <paramref name="distances"/> that are inside <paramref name="radius"/>,
		/// nearest first, at most <paramref name="maxCount"/> of them. Negative distances mark candidates that must be skipped
		/// (destroyed, pocketed, ...). <paramref name="result"/> is cleared first.
		/// </summary>
		internal static void PickNearest(IList<float> distances, float radius, int maxCount, List<int> result)
		{
			result.Clear();
			if (maxCount <= 0)
			{
				return;
			}

			for (int index = 0; index < distances.Count; index++)
			{
				if (!IsInRange(distances[index], radius))
				{
					continue;
				}

				// Insertion into a short sorted list: maxCount is small, so this is cheaper than sorting everything.
				int position = result.Count;
				while (position > 0 && distances[result[position - 1]] > distances[index])
				{
					position--;
				}

				if (position >= maxCount)
				{
					continue;
				}

				result.Insert(position, index);
				if (result.Count > maxCount)
				{
					result.RemoveAt(result.Count - 1);
				}
			}
		}

		/// <summary>
		/// True when a valuable may get a new highlight: it was never highlighted (<paramref name="lastShown"/> is NaN or below zero)
		/// or the previous graphic has expired, i.e. at least <paramref name="graphicLifetime"/> seconds have passed. This prevents
		/// stacking duplicate graphics when the refresh interval is shorter than the graphic lives.
		/// </summary>
		internal static bool IsDue(float now, float lastShown, float graphicLifetime)
		{
			if (float.IsNaN(lastShown) || lastShown < 0f)
			{
				return true;
			}

			return now - lastShown >= graphicLifetime;
		}

		/// <summary>Seconds until the next scan, never below the 0.2 s the setting allows.</summary>
		internal static float ScanInterval(float refreshSeconds)
		{
			return refreshSeconds < 0.2f ? 0.2f : refreshSeconds;
		}
	}
}

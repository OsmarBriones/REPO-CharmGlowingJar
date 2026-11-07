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
		/// True when a valuable may be lit up again: it never was (<paramref name="lastShown"/> is NaN or below zero) or at least
		/// <paramref name="rememberSeconds"/> have passed since the last time. With a memory of 0 every valuable is always due.
		/// </summary>
		internal static bool IsDue(float now, float lastShown, float rememberSeconds)
		{
			if (float.IsNaN(lastShown) || lastShown < 0f)
			{
				return true;
			}

			return now - lastShown >= rememberSeconds;
		}
	}
}

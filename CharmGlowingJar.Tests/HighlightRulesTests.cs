using System.Collections.Generic;
using CharmGlowingJar.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CharmGlowingJar.Tests;

[TestClass]
public class HighlightRulesTests
{
	[DataTestMethod]
	[DataRow(0f, 15f, true)]
	[DataRow(14.9f, 15f, true)]
	[DataRow(15f, 15f, true)]
	[DataRow(15.1f, 15f, false)]
	[DataRow(-1f, 15f, false)]
	[DataRow(5f, -1f, false)]
	public void IsInRange_IncludesTheBorderAndRejectsNegatives(float distance, float radius, bool expected)
	{
		Assert.AreEqual(expected, HighlightRules.IsInRange(distance, radius));
	}

	[TestMethod]
	public void PickNearest_ReturnsNearestFirstAndSkipsFarAndInvalid()
	{
		List<int> result = new List<int>();
		HighlightRules.PickNearest(new[] { 10f, 3f, 40f, -1f, 7f }, 15f, 10, result);
		CollectionAssert.AreEqual(new[] { 1, 4, 0 }, result);
	}

	[TestMethod]
	public void PickNearest_KeepsOnlyTheNearestWhenCapped()
	{
		List<int> result = new List<int>();
		HighlightRules.PickNearest(new[] { 9f, 2f, 8f, 1f, 5f }, 15f, 3, result);
		CollectionAssert.AreEqual(new[] { 3, 1, 4 }, result);
	}

	[TestMethod]
	public void PickNearest_ClearsTheResultAndHandlesZeroCap()
	{
		List<int> result = new List<int> { 99 };
		HighlightRules.PickNearest(new[] { 1f }, 15f, 0, result);
		Assert.AreEqual(0, result.Count);
		HighlightRules.PickNearest(new float[0], 15f, 5, result);
		Assert.AreEqual(0, result.Count);
	}

	[TestMethod]
	public void IsDue_TrueWhenNeverShownOrTheMemoryExpired()
	{
		Assert.IsTrue(HighlightRules.IsDue(100f, float.NaN, 60f));
		Assert.IsTrue(HighlightRules.IsDue(100f, -1f, 60f));
		Assert.IsTrue(HighlightRules.IsDue(100f, 40f, 60f));
		Assert.IsFalse(HighlightRules.IsDue(100f, 41f, 60f));
	}

	[TestMethod]
	public void IsDue_WithNoMemoryEverythingIsAlwaysDue()
	{
		Assert.IsTrue(HighlightRules.IsDue(100f, 100f, 0f));
		Assert.IsTrue(HighlightRules.IsDue(100f, 99.9f, 0f));
	}
}

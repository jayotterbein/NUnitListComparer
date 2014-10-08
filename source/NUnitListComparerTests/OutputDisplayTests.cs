using NUnit.Framework;
using NUnitListComparer;

namespace NUnitListComparerTests
{
    [TestFixture]
    [Ignore("These tests are designed to fail for the purposes of displaying the output from ListComparerConstaint.")]
    public class OutputDisplayTests
    {
        [Test]
        public void DisplayOnlyDifferences_test()
        {
            var actual = new[] {10, 1};
            var expected = new[] {10, 10};

            var listComparerConstraint = new ListComparerConstraint<int>(expected)
            {
                DisplayOnlyDifferences = true,
            };
            Assert.That(actual, !listComparerConstraint, "Evaulating the constraint.  This assert should not fail.");
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.EquivalentTo(new[] {1}), "Verifying all differences were found correctly.  This assert should not fail.");

            Assert.That(actual, listComparerConstraint, "This assert is expected to fail and is used to for the display of items");
        }

        [Test]
        public void Custom_output()
        {
            var actual = new[] {5, 1};
            var expected = new[] {10, 10};

            var listComparerConstraint = new ListComparerConstraint<int>(expected)
            {
                DisplayItem = item => item.ToString("####.00")
            };
            Assert.That(actual, !listComparerConstraint, "Evaulating the constraint.  This assert should not fail.");
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.EquivalentTo(new[] {0, 1}), "Verifying all differences were found correctly.  This assert should not fail.");

            Assert.That(actual, listComparerConstraint, "This assert is expected to fail and is used to for the display of items");
        }
    }
}
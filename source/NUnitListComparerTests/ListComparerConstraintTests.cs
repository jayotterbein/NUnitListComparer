using NUnit.Framework;
using NUnitListComparer;

namespace NUnitListComparerTests
{
    [TestFixture]
    public class ListComparerConstraintTests
    {
        [Test]
        public void Lists_equivalent()
        {
            var actual = new[] {1, 2, 3};
            var expected = new[] {1, 2, 3};

            var listComparerConstraint = new ListComparerConstraint<int>(expected);
            Assert.That(actual, listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.Empty);
        }

        [Test]
        public void Actual_list_smaller()
        {
            var actual = new[] {1};
            var expected = new[] {1, 2, 3};

            var listComparerConstraint = new ListComparerConstraint<int>(expected);
            Assert.That(actual, !listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.EquivalentTo(new[] {1, 2}));
        }

        [Test]
        public void Expected_list_smaller()
        {
            var actual = new[] {1, 2, 3};
            var expected = new[] {1};

            var listComparerConstraint = new ListComparerConstraint<int>(expected);
            Assert.That(actual, !listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.EquivalentTo(new[] {1, 2}));
        }
    }
}
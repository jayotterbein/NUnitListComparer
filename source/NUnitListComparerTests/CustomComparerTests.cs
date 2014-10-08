using System.Collections.Generic;
using NUnit.Framework;
using NUnitListComparer;

namespace NUnitListComparerTests
{
    [TestFixture]
    public class CustomComparerTests
    {
        [Test]
        public void Custom_comparer()
        {
            var actual = new[] {0, 0, 0};
            var expected = new[] {5, 5, 5};

            var listComparerConstraint = new ListComparerConstraint<int>(expected)
            {
                EqualityComparer = new IntsAlwaysEqualComparer()
            };

            Assert.That(actual, listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.Empty);
        }

        [Test]
        public void Custom_comparer_func()
        {
            var actual = new[] {0, 0, 0};
            var expected = new[] {1, 2, 3};

            var listComparerConstraint = new ListComparerConstraint<int>(expected)
            {
                EqualityComparerFunc = (x, y) => true
            };
            Assert.That(actual, listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.Empty);
        }

        private sealed class IntsAlwaysEqualComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return true;
            }

            public int GetHashCode(int obj)
            {
                return obj;
            }
        }
    }
}
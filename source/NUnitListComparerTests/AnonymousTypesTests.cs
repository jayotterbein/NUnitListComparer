using NUnit.Framework;
using NUnitListComparer;

namespace NUnitListComparerTests
{
    [TestFixture]
    public class AnonymousTypesTests
    {
        [Test]
        public void Anonymous_types_using_equalityfunc()
        {
            var actual = new[]
            {
                new {Value = 1},
                new {Value = 2}
            };
            var expected = new[]
            {
                new {Value = 1},
                new {Value = 5}
            };

            var listComparerConstraint = ListComparerConstraint.Create(expected);
            listComparerConstraint.EqualityComparerFunc = (x, y) => x.Value == y.Value;
            Assert.That(actual, !listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.EquivalentTo(new[] {1}));
        }

        [Test]
        public void Anonymous_types_using_default_comparer()
        {
            var actual = new[]
            {
                new {Value = 1},
                new {Value = 2}
            };
            var expected = new[]
            {
                actual[0],
                actual[1]
            };

            var listComparerConstraint = ListComparerConstraint.Create(expected);
            Assert.That(actual, listComparerConstraint);
            Assert.That(listComparerConstraint.DifferenceIndecies, Is.Empty);
        }
    }
}
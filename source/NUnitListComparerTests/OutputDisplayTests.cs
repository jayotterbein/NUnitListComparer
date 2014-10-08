using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnitListComparer;

namespace NUnitListComparerTests
{
    [TestFixture]
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

            OutputFailedResultMessage(listComparerConstraint);
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

            OutputFailedResultMessage(listComparerConstraint);
        }

        private static void OutputFailedResultMessage(Constraint listComparerConstraint, [CallerMemberName]string memberName = "")
        {
            using (var messageWriter = new TextMessageWriter())
            {
                listComparerConstraint.WriteMessageTo(messageWriter);
                Console.WriteLine("----- {0}", memberName);
                Console.WriteLine(messageWriter.ToString());
            }
        }
    }
}
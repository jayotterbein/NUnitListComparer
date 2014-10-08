using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace NUnitListComparer
{
    public static class ListComparerConstraint
    {
        /// <summary>
        ///     This allows a ListComparerConstraint to be easily created for anonymous types.
        /// </summary>
        public static ListComparerConstraint<T> Create<T>(IEnumerable<T> expected)
        {
            return new ListComparerConstraint<T>(expected);
        }
    }

    public class ListComparerConstraint<T> : Constraint
    {
        private readonly HashSet<int> _differentIndecies;
        private readonly IList<T> _expected;
        private IList<T> _actual;
        private Func<T, string> _displayItem;
        private bool? _displayOnlyDifferences;
        private IEqualityComparer<T> _equalityComparer;
        private Func<T, T, bool> _equalityComparerFunc;

        public ListComparerConstraint(IEnumerable<T> expected)
        {
            _expected = (expected == null) ? new T[0] : (expected as IList<T>) ?? expected.ToArray();
            _differentIndecies = new HashSet<int>();
        }

        /// <summary>
        ///     Set to true if only items that are different should be displayed.  Otherwise all items in both the actual and
        ///     expected lists are displayed.
        /// </summary>
        /// <value>false</value>
        public virtual bool DisplayOnlyDifferences
        {
            get { return _displayOnlyDifferences ?? false; }
            set { _displayOnlyDifferences = value; }
        }

        /// <summary> Set the comparer to be used to check equality of each item in the list. </summary>
        /// <remarks>Overwrites anything set to <see cref="EqualityComparerFunc" /> </remarks>
        /// <seealso cref="EqualityComparerFunc" />
        public virtual IEqualityComparer<T> EqualityComparer
        {
            get { return _equalityComparer ?? EqualityComparer<T>.Default; }
            set
            {
                _equalityComparer = value;
                _equalityComparerFunc = null;
            }
        }

        /// <summary> Set the comparer to be used to check equality of each item in the list. </summary>
        /// <remarks>Overwrites anything set to <see cref="EqualityComparer" /> </remarks>
        /// <seealso cref="EqualityComparer" />
        public virtual Func<T, T, bool> EqualityComparerFunc
        {
            get { return _equalityComparerFunc ?? EqualityComparer.Equals; }
            set
            {
                _equalityComparerFunc = value;
                _equalityComparer = null;
            }
        }

        /// <summary>
        ///     Choose how to display items; default is item.ToString().  nulls are possible to be sent in here.
        /// </summary>
        public virtual Func<T, string> DisplayItem
        {
            get { return _displayItem ?? (item => (ReferenceEquals(item, null)) ? "<null>" : item.ToString()); }
            set { _displayItem = value; }
        }

        /// <summary>
        ///     Returns the list of index differences in the actual and expected lists.  If one list is larger, then every index
        ///     past the bounds of the smaller array is included.
        /// </summary>
        public virtual int[] DifferenceIndecies
        {
            get { return _differentIndecies.ToArray(); }
        }

        public override bool Matches(object actualItem)
        {
            // allow null to be passed in and assumed the same as an empty enumerable.  if the item is non-null and non-enumerable, throw.
            var actualEnumerable = (actualItem == null) ? Enumerable.Empty<T>() : actualItem as IEnumerable<T>;
            if (actualEnumerable == null)
            {
                throw new ArgumentException("Non-enumerable passed into ListComparerConstraint.");
            }

            _actual = (actualEnumerable as IList<T>) ?? actualEnumerable.ToArray();

            int i;
            var min = Math.Min(_expected.Count, _actual.Count);
            var max = Math.Max(_expected.Count, _actual.Count);
            for (i = 0; i < min; i++)
            {
                if (!EqualityComparerFunc(_actual[i], _expected[i]))
                {
                    _differentIndecies.Add(i);
                }
            }
            for (; i < max; i++)
            {
                _differentIndecies.Add(i);
            }
            return !_differentIndecies.Any();
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("");
            for (var i = 0; i < _expected.Count; i++)
            {
                var item = _expected[i];
                WriteItem(writer, item, i, _differentIndecies.Contains(i));
            }
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteMessageLine("");
            for (var i = 0; i < _actual.Count; i++)
            {
                var item = _actual[i];
                WriteItem(writer, item, i, _differentIndecies.Contains(i));
            }
        }

        protected virtual void WriteItem(MessageWriter writer, T item, int index, bool isDifferent)
        {
            if (DisplayOnlyDifferences && !isDifferent)
            {
                return;
            }

            if (isDifferent)
            {
                writer.Write("* ");
            }
            writer.Write("[{0}] ", index);
            writer.WriteMessageLine(DisplayItem(item));
        }
    }
}
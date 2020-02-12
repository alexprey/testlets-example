using System;
using System.Collections.Generic;
using System.Linq;

namespace Testlets.Core.Models
{
    /// <summary>
    /// Describes the set of testlet items which should be used for candidate assessment.
    /// </summary>
    public class Testlet
    {
        /// <summary>
        /// The unique testlet identifier.
        /// </summary>
        public string TestletId { get; }

        private ICollection<TestletItem> _items;
        private int _initialPretestItemsCount;

        private ICollection<TestletItem> _pretestItems;
        private ICollection<TestletItem> _otherItems;

        /// <summary>
        /// Initialize a new instance of testlet.
        /// </summary>
        /// <param name="testletId">The unique testlet identifier. Can be <value>NULL</value> or empty.</param>
        /// <param name="initialPretestItemsCount">The count of items with pretest count that should be placed at beginning of resulting array.</param>
        /// <param name="items">The collection of testlet items.</param>
        /// <exception cref="System.ArgumentNullException">Throws when testlet identifier is null or empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Throws when initial pretest items count is not natural value (integer greater than zero).</exception>
        /// <exception cref="System.ArgumentNullException">Throws when items collection is null.</exception>
        /// <exception cref="System.ArgumentException">Throws when items collection is empty.</exception>
        /// <exception cref="System.InvalidOperationException">Throws when items in collection is not enough to correct handling of initial pretest items placement.</exception>
        public Testlet(string testletId, int initialPretestItemsCount, ICollection<TestletItem> items)
        {
            if (string.IsNullOrWhiteSpace(testletId))
            {
                throw new ArgumentNullException(nameof(testletId), "The identifier should be a non empty string.");
            }

            if (initialPretestItemsCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialPretestItemsCount), "The count of initial pretest items should be greater than zero.");
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (items.Count == 0)
            {
                throw new ArgumentException("Collection should have at least one item.", nameof(items));
            }

            TestletId = testletId;
            
            _items = items;
            _initialPretestItemsCount = initialPretestItemsCount;

            _pretestItems = items
                .Where(item => item.ItemType == TestletItemType.Pretest)
                .ToList();

            if (_pretestItems.Count < _initialPretestItemsCount)
            {
                throw new InvalidOperationException("Not enough pretest items in items collection.");
            }

            _otherItems = items
                .Where(item => item.ItemType != TestletItemType.Pretest)
                .ToList();
        }

        /// <summary>
        /// Randomize source collection of testlet items and provide it back for using it in the future.
        /// </summary>
        /// <returns>Randomized collection of testlet items.</returns>
        public ICollection<TestletItem> Randomize()
        {
            var result = new List<TestletItem>(_items.Count);

            var random = new Random();

            // Shuffle pretest items and place it in result collection
            var shuffledPretestItems = ShuffleCollection(random, _pretestItems);
            result.AddRange(shuffledPretestItems.Take(_initialPretestItemsCount));

            // Shuffle least items and place it in result collection
            var leastItems = new List<TestletItem>(_items.Count);
            leastItems.AddRange(shuffledPretestItems.Skip(_initialPretestItemsCount));
            leastItems.AddRange(_otherItems);

            result.AddRange(ShuffleCollection(random, leastItems));

            return result;
        }

        private static ICollection<TestletItem> ShuffleCollection(Random random, ICollection<TestletItem> items)
        {
            var result = new List<TestletItem>(items);
            for (var index = 0; index < items.Count; index++)
            {
                var newIndex = random.Next(0, items.Count);
                if (index != newIndex)
                {
                    var swappedItem = result[newIndex];
                    result[newIndex] = result[index];
                    result[index] = swappedItem;
                }
            }

            return result;
        }
    }
}

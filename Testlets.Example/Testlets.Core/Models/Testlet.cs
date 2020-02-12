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

        private ICollection<TestletItem> _pretestItems;
        private ICollection<TestletItem> _otherItems;

        /// <summary>
        /// Initialize a new instance of testlet.
        /// </summary>
        /// <param name="testletId">The unique testlet identifier. Can be <value>NULL</value> or empty.</param>
        /// <param name="items">The collection of testlet items.</param>
        /// <exception cref="System.ArgumentNullException">Throws when testlet identifier is null or empty.</exception>
        /// <exception cref="System.ArgumentNullException">Throws when items collection is null.</exception>
        /// <exception cref="System.ArgumentException">Throws when items collection is empty.</exception>
        public Testlet(string testletId, ICollection<TestletItem> items)
        {
            if (string.IsNullOrWhiteSpace(testletId))
            {
                throw new ArgumentNullException(nameof(testletId), "The identifier should be a non empty string.");
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
            _pretestItems = items
                .Where(item => item.ItemType == TestletItemType.Pretest)
                .ToList();

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

            var random = new Random((int)DateTime.Now.Ticks);

            var leastPretestItems = new List<TestletItem>(_pretestItems);
            result.AddRange(TakeRandomItemsFromCollection(random, 2, leastPretestItems));

            var leastItems = new List<TestletItem>(leastPretestItems.Count + _otherItems.Count);
            leastItems.AddRange(leastPretestItems);
            leastItems.AddRange(_otherItems);

            result.AddRange(TakeRandomItemsFromCollection(random, leastItems));

            return result;
        }

        private static IEnumerable<TestletItem> TakeRandomItemsFromCollection(Random random, List<TestletItem> items)
        {
            return TakeRandomItemsFromCollection(random, items.Count, items);
        }

        private static IEnumerable<TestletItem> TakeRandomItemsFromCollection(Random random, int count, List<TestletItem> items)
        {
            for (int step = 0; step < count; step++)
            {
                var itemIndex = random.Next(0, items.Count);
                var itemToReturn = items[itemIndex];
                items.RemoveAt(itemIndex);

                yield return itemToReturn;
            }
        }
    }
}

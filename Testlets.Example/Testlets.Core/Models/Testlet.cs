using System.Collections.Generic;

namespace Testlets.Core.Models
{
    /// <summary>
    /// Describes the set of testlet items which should be used for candidate assessment.
    /// </summary>
    public class Testlet
    {
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
            // TODO
        }

        /// <summary>
        /// Randomize source collection of testlet items and provide it back for using it in the future.
        /// </summary>
        /// <returns>Randomized collection of testlet items.</returns>
        public ICollection<TestletItem> Randomize()
        {
            // TODO
            return null;
        }
    }
}

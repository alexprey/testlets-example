namespace Testlets.Core.Models
{
    /// <summary>
    /// Describes the testlet item.
    /// </summary>
    public class TestletItem
    {
        /// <summary>
        /// The testlet item unique identifier.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// The type of the testlet item.
        /// </summary>
        public TestletItemType ItemType { get; set; }
    }
}

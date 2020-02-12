namespace Testlets.Core.Models
{
    /// <summary>
    /// Describes possibles testlet item types.
    /// </summary>
    public enum TestletItemType
    {
        /// <summary>
        /// Items of this type don't counted for a result score.
        /// </summary>
        Pretest = 0,
        /// <summary>
        /// Regular type of question for testlet.
        /// </summary>
        Operational = 1,
    }
}

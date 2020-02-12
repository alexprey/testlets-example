using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Testlets.Core.Models;

namespace Testlets.Core.UnitTests.Models
{
    [TestFixture]
    [TestOf(typeof(Testlet))]
    public class TestletUnitTests
    {
        private static readonly string ValidTestletId = "{2265353C-76A9-4010-BA30-9CE4DBC9FDE0}";
        private static readonly int ValidInitialPretestItemsCount = 2;

        private static readonly ICollection<TestletItem> ValidTestletItemsCollection = new[]
        {
            new TestletItem { ItemId = "pre-test-1", ItemType = TestletItemType.Pretest },
            new TestletItem { ItemId = "pre-test-2", ItemType = TestletItemType.Pretest },
            new TestletItem { ItemId = "pre-test-3", ItemType = TestletItemType.Pretest },
            new TestletItem { ItemId = "pre-test-4", ItemType = TestletItemType.Pretest },
            new TestletItem { ItemId = "pre-operational-1", ItemType = TestletItemType.Operational },
            new TestletItem { ItemId = "pre-operational-2", ItemType = TestletItemType.Operational },
            new TestletItem { ItemId = "pre-operational-3", ItemType = TestletItemType.Operational },
            new TestletItem { ItemId = "pre-operational-4", ItemType = TestletItemType.Operational },
            new TestletItem { ItemId = "pre-operational-5", ItemType = TestletItemType.Operational },
            new TestletItem { ItemId = "pre-operational-6", ItemType = TestletItemType.Operational },
        };

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void Construct_TestletIdEmpty_ShouldThrowException(string wrongTestletId)
        {
            Action act = () => new Testlet(wrongTestletId, ValidInitialPretestItemsCount, ValidTestletItemsCollection);

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("The identifier should be a non empty string. (Parameter 'testletId')");
        }

        [Test]
        public void Construct_ItemsCollectionIsNull_ShouldThrowException()
        {
            Action act = () => new Testlet(ValidTestletId, ValidInitialPretestItemsCount, null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Construct_WhenItemsCollectionIsEmpty_ShouldThrowException()
        {
            Action act = () => new Testlet(ValidTestletId, ValidInitialPretestItemsCount, new TestletItem[0]);

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Collection should have at least one item. (Parameter 'items')");
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void Construct_WhenInitialPretestItemsCountIsWrong_ShouldThrowException(int initialPretestItemsCount)
        {
            Action act = () => new Testlet(ValidTestletId, initialPretestItemsCount, ValidTestletItemsCollection);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("The count of initial pretest items should be greater than zero. (Parameter 'initialPretestItemsCount')");
        }

        [Test]
        public void Construct_WhenOverflowOfInitialPretestItemsCount_ShouldThrowException()
        {
            Action act = () => new Testlet(ValidTestletId, 5, ValidTestletItemsCollection);

            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Not enough pretest items in items collection.");
        }

        [Test]
        public void GetTestletId_WhenShouldReturnValidIdentifier()
        {
            var target = new Testlet(ValidTestletId, ValidInitialPretestItemsCount, ValidTestletItemsCollection);

            target.TestletId.Should().Be(ValidTestletId);
        }

        [Test]
        public void Randomize_ShouldReturnNewCollectionWithFirst2ItemsAsPretest()
        {
            var target = new Testlet(ValidTestletId, ValidInitialPretestItemsCount, ValidTestletItemsCollection);

            var randomizedCollection = target.Randomize();

            randomizedCollection.Should().NotBeNull();

            randomizedCollection.Should().NotBeSameAs(ValidTestletItemsCollection);
            randomizedCollection.Should().BeEquivalentTo(ValidTestletItemsCollection);

            randomizedCollection
                .Take(ValidInitialPretestItemsCount)
                .Should().OnlyContain(item => item.ItemType == TestletItemType.Pretest, "The first items of new randomized collection always should be a pretest");

            randomizedCollection
                .Should().OnlyHaveUniqueItems(item => item.ItemId, "New randomized collection should not have any duplications");
        }

        [Test]
        public void Randomize_ShouldReturnNewRandomizedCollection()
        {
            var target = new Testlet(ValidTestletId, ValidInitialPretestItemsCount, ValidTestletItemsCollection);

            var previousOrdering = new List<ICollection<string>>();

            for (int iterationId = 0; iterationId < 10; iterationId++)
            {
                var randomizedCollection = target.Randomize();

                randomizedCollection.Should().NotBeNull();

                var ordering = randomizedCollection.Select(item => item.ItemId).ToList();

                previousOrdering.Should().NotContain(ordering, "Each randomization should be unique.");

                previousOrdering.Add(ordering);
            }
        }
    }
}

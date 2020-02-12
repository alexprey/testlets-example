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
            Action act = () => new Testlet(wrongTestletId, ValidTestletItemsCollection);

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("The identifier should be a non empty string. (Parameter 'testletId')");
        }

        [Test]
        public void Construct_ItemsCollectionIsNull_ShouldThrowException()
        {
            Action act = () => new Testlet(ValidTestletId, null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Construct_ItemsCollectionIsEmpty_ShouldThrowException()
        {
            Action act = () => new Testlet(ValidTestletId, new TestletItem[0]);

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Collection should have at least one item. (Parameter 'items')");
        }

        [Test]
        public void TestletId_ShouldReturnValidIdentifier()
        {
            var target = new Testlet(ValidTestletId, ValidTestletItemsCollection);

            target.TestletId.Should().Be(ValidTestletId);
        }

        [Test]
        public void Randomize_ShouldReturnNewCollectionWithFirst2ItemsAsPretest()
        {
            var target = new Testlet(ValidTestletId, ValidTestletItemsCollection);

            var randomizedCollection = target.Randomize();

            randomizedCollection.Should().NotBeNull();

            randomizedCollection.Should().NotBeSameAs(ValidTestletItemsCollection);
            randomizedCollection.Should().BeEquivalentTo(ValidTestletItemsCollection);

            randomizedCollection
                .Take(2)
                .Should().OnlyContain(item => item.ItemType == TestletItemType.Pretest, "The first two items of new randomized collection always should be a pretest");

            randomizedCollection
                .Should().OnlyHaveUniqueItems(item => item.ItemId, "New randomized collection should not have any duplications");
        }

        [Test]
        public void Randomize_ShouldReturnNewRandomizedCollection()
        {
            var target = new Testlet(ValidTestletId, ValidTestletItemsCollection);

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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public class SegmentBuilderTests
    {
        [TestMethod]
        public void SegmentBuilder_CanBuildFields_Individually()
        {
            var builder = Message.Build()[1];
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .Field(3, field3)
                .Field(5, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .Field(5, field5)
                .Field(3, field3);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildFields_Sequentially()
        {
            var builder = Message.Build()[1];
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .Fields(3, field3, null, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build()[1];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(3, 1, repetition1)
                .FieldRepetition(3, 2, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(3, 2, repetition2)
                .FieldRepetition(3, 1, repetition1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetitions(3, repetition1, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(3, 1, 1, component1)
                .Component(3, 1, 2, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(3, 1, 2, component2)
                .Component(3, 1, 1, component1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Components(3, 1, component1, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(3, 1, 1, 1, subcomponent1)
                .Subcomponent(3, 1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(3, 1, 1, 2, subcomponent2)
                .Subcomponent(3, 1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponents(3, 1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1];
            Assert.AreEqual(builder.FieldDelimiter, '|');
            messageBuilder.FieldDelimiter = ':';
            Assert.AreEqual(builder.FieldDelimiter, ':');
        }

        [TestMethod]
        public void SegmentBuilder_ChangesEncodingCharactersIfMshSegmentChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1];
            Assert.AreEqual(builder.FieldDelimiter, '|');
            builder.Field(1, ":");
            Assert.AreEqual(builder.FieldDelimiter, ':');
        }
    }
}

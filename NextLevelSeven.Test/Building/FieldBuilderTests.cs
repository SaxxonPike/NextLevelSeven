using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    sealed public class FieldBuilderTests : BuildingTestFixture
    {
        [TestMethod]
        public void FieldBuilder_CanGetValue()
        {
            var val0 = Randomized.String();
            var val1 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", val0, val1))[1][3];
            Assert.AreEqual(builder.Value, string.Format("{0}~{1}", val0, val1));
        }

        [TestMethod]
        public void FieldBuilder_CanGetValues()
        {
            var val0 = Randomized.String();
            var val1 = Randomized.String();
            var val2 = Randomized.String();
            var val3 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                val0, val1, val2, val3))[1][3];
            AssertEnumerable.AreEqual(builder.Values, new[] {val0, string.Format("{0}^{1}&{2}", val1, val2, val3)});
        }

        [TestMethod]
        public void FieldBuilder_CanBeCloned()
        {
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Randomized.String(), Randomized.String(), Randomized.String(), Randomized.String()))[1][3];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(1, repetition1)
                .FieldRepetition(2, repetition2);
            Assert.AreEqual(string.Format("{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(2, repetition2)
                .FieldRepetition(1, repetition1);
            Assert.AreEqual(string.Format("{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetitions(3, repetition1, repetition2);
            Assert.AreEqual(string.Format("~~{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1][3];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, 1, component1)
                .Component(1, 2, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, 2, component2)
                .Component(1, 1, component1);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Components(1, component1, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(1, 1, 1, subcomponent1)
                .Subcomponent(1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(1, 1, 2, subcomponent2)
                .Subcomponent(1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponents(1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3];
            Assert.AreEqual(builder.FieldDelimiter, '|');
            messageBuilder.FieldDelimiter = ':';
            Assert.AreEqual(builder.FieldDelimiter, ':');
        }

        [TestMethod]
        public void FieldBuilder_ChangesEncodingCharactersIfSelfChanges()
        {
            var builder = Message.Build()[1][2];
            builder.Field("$~\\&");
            Assert.AreEqual(builder.ComponentDelimiter, '$');
        }
    }
}
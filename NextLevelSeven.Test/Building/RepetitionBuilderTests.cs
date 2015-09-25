using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class RepetitionBuilderTests : BuildingTestFixture
    {
        [TestMethod]
        public void RepetitionBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1];
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void RepetitionBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1] as IRepetition;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void RepetitionBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1] as IElement;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void RepetitionBuilder_CanGetComponent_ThroughField()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][1][2].Value);
            Assert.AreEqual(builder[1][3][1][2].Value, builder.Segment(1).Field(3).Component(2).Value,
                "Components returned differ.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanGetComponent()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][2][2].Value);
            Assert.AreEqual(builder[1][3][2][2].Value, builder.Segment(1).Field(3).Repetition(2).Component(2).Value,
                "Components returned differ.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanGetValue()
        {
            var val0 = Randomized.String();
            var val1 = Randomized.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{2}~{0}^{1}", val0, val1, Randomized.String()))[1][3][2];
            Assert.AreEqual(builder.Value, string.Format("{0}^{1}", val0, val1));
        }

        [TestMethod]
        public void RepetitionBuilder_CanGetValues()
        {
            var val1 = Randomized.String();
            var val2 = Randomized.String();
            var val3 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Randomized.String(), val1, val2, val3))[1][3][2];
            AssertEnumerable.AreEqual(builder.Values, new[] {val1, string.Format("{0}&{1}", val2, val3)});
        }

        [TestMethod]
        public void RepetitionBuilder_CanBeCloned()
        {
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Randomized.String(), Randomized.String(), Randomized.String(), Randomized.String()))[1][3][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .SetComponent(1, component1)
                .SetComponent(2, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .SetComponent(2, component2)
                .SetComponent(1, component1);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .SetComponents(3, component1, component2);
            Assert.AreEqual(string.Format("^^{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3][1];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .SetSubcomponent(1, 1, subcomponent1)
                .SetSubcomponent(1, 2, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .SetSubcomponent(1, 2, subcomponent2)
                .SetSubcomponent(1, 1, subcomponent1);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .SetSubcomponents(3, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("^^{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3][1];
            Assert.AreEqual(builder.Encoding.FieldDelimiter, '|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            Assert.AreEqual(builder.Encoding.FieldDelimiter, ':');
        }
    }
}
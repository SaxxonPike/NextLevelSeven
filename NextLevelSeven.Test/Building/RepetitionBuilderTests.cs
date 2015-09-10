using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public class RepetitionBuilderTests
    {
        [TestMethod]
        public void RepetitionBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, component1)
                .Component(2, component2);
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
                .Component(2, component2)
                .Component(1, component1);
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
                .Components(3, component1, component2);
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
                .Subcomponent(1, 1, subcomponent1)
                .Subcomponent(1, 2, subcomponent2);
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
                .Subcomponent(1, 2, subcomponent2)
                .Subcomponent(1, 1, subcomponent1);
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
                .Subcomponents(3, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("^^{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void RepetitionBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3][1];
            Assert.AreEqual(builder.FieldDelimiter, '|');
            messageBuilder.FieldDelimiter = ':';
            Assert.AreEqual(builder.FieldDelimiter, ':');
        }
    }
}
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class SegmentBuilderUnitTests : BuildingTestFixture
    {
        [TestMethod]
        public void SegmentBuilder_HasDelimiter()
        {
            var builder = Message.Build(Mock.Message());
            Assert.AreEqual('|', builder[1].Delimiter);
        }

        [TestMethod]
        public void SegmentBuilder_CanMoveFields()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            var val0 = Mock.String();
            builder[1][3].Value = val0;
            builder[1][3].Move(4);
            Assert.AreEqual(val0, builder[1][4].Value);
        }

        [TestMethod]
        public void SegmentBuilder_Throws_WhenDataIsTooShort()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1];
            AssertAction.Throws<ElementException>(() => builder.Value = "M");
        }

        [TestMethod]
        public void SegmentBuilder_Throws_WhenMshDataIsNull()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1];
            AssertAction.Throws<ElementException>(() => builder.Value = null);
        }

        [TestMethod]
        public void SegmentBuilder_CanNullifyNonMshSegment()
        {
            var builder = Message.Build(ExampleMessages.Standard)[2];
            builder.Value = null;
        }

        [TestMethod]
        public void SegmentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1];
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SegmentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1] as ISegment;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SegmentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1] as IElement;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SegmentBuilder_CanGetField()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3].Value);
            Assert.AreEqual(builder[1][3].Value, builder.Segment(1).Field(3).Value, "Fields returned differ.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBeCloned()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBeClonedGenerically()
        {
            IElement builder = Message.Build(ExampleMessages.Standard)[1];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void SegmentBuilder_CanGetValue()
        {
            var builder = Message.Build("MSH|^~\\&\rPID|1234")[1];
            Assert.AreEqual(builder.Value, "MSH|^~\\&");
        }

        [TestMethod]
        public void SegmentBuilder_CanGetValues()
        {
            var builder = Message.Build("MSH|^~\\&")[1];
            AssertEnumerable.AreEqual(builder.Values, new[] {"MSH", "|", "^~\\&"});
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildFields_Individually()
        {
            var builder = Message.Build()[1];
            var field3 = Mock.String();
            var field5 = Mock.String();

            builder
                .SetField(3, field3)
                .SetField(5, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var field3 = Mock.String();
            var field5 = Mock.String();

            builder
                .SetField(5, field5)
                .SetField(3, field3);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildFields_Sequentially()
        {
            var builder = Message.Build()[1];
            var field3 = Mock.String();
            var field5 = Mock.String();

            builder
                .SetFields(3, field3, null, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build()[1];
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetition(3, 1, repetition1)
                .SetFieldRepetition(3, 2, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetition(3, 2, repetition2)
                .SetFieldRepetition(3, 1, repetition1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1];
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetitions(3, repetition1, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1];
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponent(3, 1, 1, component1)
                .SetComponent(3, 1, 2, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponent(3, 1, 2, component2)
                .SetComponent(3, 1, 1, component1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1];
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponents(3, 1, component1, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(3, 1, 1, 1, subcomponent1)
                .SetSubcomponent(3, 1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(3, 1, 1, 2, subcomponent2)
                .SetSubcomponent(3, 1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponents(3, 1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void SegmentBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1];
            Assert.AreEqual(builder.Encoding.FieldDelimiter, '|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            Assert.AreEqual(builder.Encoding.FieldDelimiter, ':');
        }

        [TestMethod]
        public void SegmentBuilder_ChangesEncodingCharactersIfMshSegmentChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1];
            Assert.AreEqual(builder.Encoding.FieldDelimiter, '|');
            builder.SetField(1, ":");
            Assert.AreEqual(builder.Encoding.FieldDelimiter, ':');
        }

        [TestMethod]
        public void SegmentBuilder_CanGetType()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[2];
            var type = Mock.StringCaps(3);
            builder[0].Value = type;
            Assert.AreEqual(builder.Type, type);
        }

        [TestMethod]
        public void SegmentBuilder_CanSetType()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[2];
            var type = Mock.StringCaps(3);
            builder.Type = type;
            Assert.AreEqual(builder[0].Value, type);
        }
    }
}
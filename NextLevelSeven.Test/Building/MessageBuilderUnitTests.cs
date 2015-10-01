using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class MessageBuilderUnitTests : BuildingTestFixture
    {
        [TestMethod]
        public void MessageBuilder_ThrowsWithIncorrectFirstSegment()
        {
            AssertAction.Throws<ElementException>(() => Message.Build(Mock.String()));
        }

        [TestMethod]
        public void MessageBuilder_HasCorrectRootKey()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            Assert.AreEqual(ElementDefaults.RootElementKey, builder.Key);
        }

        [TestMethod]
        public void MessageBuilder_CanGetSegment()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1].Value);
            Assert.AreEqual(builder[1].Value, builder.Segment(1).Value, "Segments returned differ.");
        }

        [TestMethod]
        public void MessageBuilder_CanCreateNewMessageFromSegments()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var builder = message.Segments.OfType("PID").ToNewBuilder();
            Assert.AreEqual(3, builder.ValueCount);
            Assert.AreEqual(message[1].Value, builder[1].Value);
            Assert.AreEqual(message.Segments.OfType("PID").First().Value, builder[2].Value);
        }

        [TestMethod]
        public void MessageBuilder_HasNoAncestor()
        {
            var builder = Message.Build();
            Assert.IsNull(builder.Ancestor);
        }

        [TestMethod]
        public void MessageBuilder_HasCodec()
        {
            var builder = Message.Build();
            Assert.IsNotNull(builder.Codec);
        }

        [TestMethod]
        public void MessageBuilder_HasDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            Assert.AreEqual(13, builder.Descendants.Count());
        }

        [TestMethod]
        public void MessageBuilder_HasDetails()
        {
            var val0 = Mock.String();
            var message = string.Format("MSH|^~\\&\r{0}", val0);
            var builder = Message.Build(message);
            Assert.IsNotNull(builder.Details);
            builder.Details.Sender.Facility = val0;
            Assert.AreEqual(val0, builder.Segment(1).Field(4).Value);
        }

        [TestMethod]
        public void MessageBuilder_CanInsertElementBeforeDescendant()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var message0 = string.Format("MSH|^~\\&\r{0}", val0);
            var message1 = string.Format("MSH|^~\\&\r{0}", val1);
            var builder0 = Message.Build(message0);
            var builder1 = Message.Build(message1);
            var builder2 = builder0.Clone();
            builder0.Insert(2, builder1[2]);
            Assert.AreEqual(builder0[2].Value, builder1[2].Value);
            Assert.AreEqual(builder0[3].Value, builder2[2].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanInsertStringBeforeDescendant()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var message = string.Format("MSH|^~\\&\r{0}", val0);
            var builder = Message.Build(message);
            builder.Insert(2, val1);
            Assert.AreEqual(val1, builder[2].Value);
            Assert.AreEqual(val0, builder[3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanInsertElementBeforeSelf()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var message0 = string.Format("MSH|^~\\&\r{0}", val0);
            var message1 = string.Format("MSH|^~\\&\r{0}", val1);
            var builder0 = Message.Build(message0);
            var builder1 = Message.Build(message1);
            var builder2 = builder0.Clone();
            builder0[2].Insert(builder1[2]);
            Assert.AreEqual(builder0[2].Value, builder1[2].Value);
            Assert.AreEqual(builder0[3].Value, builder2[2].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanInsertStringBeforeSelf()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var message = string.Format("MSH|^~\\&\r{0}", val0);
            var builder = Message.Build(message);
            builder[2].Insert(val1);
            Assert.AreEqual(val1, builder[2].Value);
            Assert.AreEqual(val0, builder[3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanAddValue()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var message = string.Format("MSH|^~\\&\r{0}", val0);
            var builder = Message.Build(message);
            builder.Add(val1);
            Assert.AreEqual(val0, builder[2].Value);
            Assert.AreEqual(val1, builder[3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanAddValues()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            const string message = "MSH|^~\\&";
            var builder = Message.Build(message);
            builder.AddRange(val0, val1);
            Assert.AreEqual(val0, builder[2].Value);
            Assert.AreEqual(val1, builder[3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanAddValuesWithEnumerable()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            const string message = "MSH|^~\\&";
            var builder = Message.Build(message);
            builder.AddRange(new[] {val0, val1}.AsEnumerable());
            Assert.AreEqual(val0, builder[2].Value);
            Assert.AreEqual(val1, builder[3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanGetValue()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var message = string.Format("MSH|^~\\&\r{0}\r{1}", val0, val1);
            var builder = Message.Build(message);
            Assert.AreEqual(builder.Value, message);
        }

        [TestMethod]
        public void MessageBuilder_CanGetValues()
        {
            var val0 = Mock.StringCaps(3) + "|" + Mock.String();
            var val1 = Mock.StringCaps(3) + "|" + Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&\r{0}\r{1}",
                val0, val1));
            AssertEnumerable.AreEqual(builder.Values, new[] {"MSH|^~\\&", val0, val1});
        }

        [TestMethod]
        public void MessageBuilder_CanBeCloned()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_Individually()
        {
            var builder = Message.Build();
            var field3 = Mock.String();
            var field5 = Mock.String();

            builder
                .SetField(1, 3, field3)
                .SetField(1, 5, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = Message.Build();
            var field3 = Mock.String();
            var field5 = Mock.String();

            builder
                .SetField(1, 5, field5)
                .SetField(1, 3, field3);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_Sequentially()
        {
            var builder = Message.Build();
            var field3 = Mock.String();
            var field5 = Mock.String();

            builder
                .SetFields(1, 3, field3, null, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build();
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetition(1, 3, 1, repetition1)
                .SetFieldRepetition(1, 3, 2, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build();
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetition(1, 3, 2, repetition2)
                .SetFieldRepetition(1, 3, 1, repetition1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build();
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetitions(1, 3, repetition1, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_Individually()
        {
            var builder = Message.Build();
            var segment2 = "ZZZ|" + Mock.String();
            var segment3 = "ZAA|" + Mock.String();

            builder
                .SetSegment(2, segment2)
                .SetSegment(3, segment3);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_OutOfOrder()
        {
            var builder = Message.Build();
            var segment2 = "ZOT|" + Mock.String();
            var segment3 = "ZED|" + Mock.String();

            builder
                .SetSegment(4, segment3)
                .SetSegment(2, segment2);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_Sequentially()
        {
            var builder = Message.Build();
            var segment2 = "ZIP|" + Mock.String();
            var segment3 = "ZAP|" + Mock.String();

            builder
                .SetSegments(2, segment2, segment3);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build();
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponent(1, 3, 1, 1, component1)
                .SetComponent(1, 3, 1, 2, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build();
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponent(1, 3, 1, 2, component2)
                .SetComponent(1, 3, 1, 1, component1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build();
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponents(1, 3, 1, component1, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build();
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(1, 3, 1, 1, 1, subcomponent1)
                .SetSubcomponent(1, 3, 1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build();
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(1, 3, 1, 1, 2, subcomponent2)
                .SetSubcomponent(1, 3, 1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build();
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponents(1, 3, 1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsToParser()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            var beforeMessageString = builder.Value;
            var message = builder.ToParser();
            Assert.AreEqual(beforeMessageString, message.Value, "Conversion from builder to message failed.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsFromParser()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var beforeBuilderString = message.Value;
            var afterBuilder = Message.Build(message);
            Assert.AreEqual(beforeBuilderString, afterBuilder.Value, "Conversion from message to builder failed.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsMshCorrectly()
        {
            var builder = Message.Build(ExampleMessages.MshOnly);
            Assert.AreEqual(ExampleMessages.MshOnly, builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_UsesReasonableMemory_WhenParsingLargeMessages()
        {
            var before = GC.GetTotalMemory(true);
            var message = Message.Build();
            message.SetField(1000000, 1000000, Mock.String());
            var messageString = message.Value;
            var usage = GC.GetTotalMemory(false) - before;
            var overhead = usage - (messageString.Length << 1);
            var usePerCharacter = (overhead/(messageString.Length << 1));
            Assert.IsTrue(usePerCharacter < 10);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultFieldDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('|', builder.Encoding.FieldDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultComponentDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('^', builder.Encoding.ComponentDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultSubcomponentDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('&', builder.Encoding.SubcomponentDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultEscapeDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('\\', builder.Encoding.EscapeCharacter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultRepetitionDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('~', builder.Encoding.RepetitionDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_ContainsSegmentBuilders()
        {
            var builder = Message.Build();
            Assert.IsInstanceOfType(builder[1], typeof (ISegmentBuilder));
        }

        [TestMethod]
        public void MessageBuilder_ReturnsSegmentValues()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}\xDPID|{1}", id1, id2));
            var builderValues = builder.Values.ToList();
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id1), builderValues[0]);
            Assert.AreEqual(string.Format("PID|{0}", id2), builderValues[1]);
        }

        [TestMethod]
        public void MessageBuilder_ReturnsSegmentValuesAsArray()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}\xDPID|{1}", id1, id2));
            var builderValues = builder.Values.ToArray();
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id1), builderValues[0]);
            Assert.AreEqual(string.Format("PID|{0}", id2), builderValues[1]);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh1()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 1, ":");
            Assert.AreEqual("MSH:^~\\&:", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 2, "@#$%");
            Assert.AreEqual("MSH|@#$%|", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetTypeToMsh()
        {
            var builder = Message.Build();
            builder.SetField(2, 0, "MSH");
        }

        [TestMethod]
        public void MessageBuilder_CanNotChangeTypeFromMsh()
        {
            // NOTE: by design.
            // (change this test if the design changes.)
            var builder = Message.Build();
            AssertAction.Throws<BuilderException>(() => builder.SetField(1, 0, "PID"));
        }

        [TestMethod]
        public void MessageBuilder_CanNotChangeTypeToMsh()
        {
            // NOTE: by design.
            // (change this test if the design changes.)
            var builder = Message.Build();
            builder.SetField(2, 0, "PID");
            AssertAction.Throws<BuilderException>(() => builder.SetField(2, 0, "MSH"));
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Component()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            builder.SetField(1, 2, "$~\\&");
            Assert.AreEqual(string.Format("MSH|$~\\&|{0}${1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Repetition()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            builder.SetField(1, 2, "^$\\&");
            Assert.AreEqual(string.Format("MSH|^$\\&|{0}${1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Escape()
        {
            // NOTE: changing escape code does not affect anything but MSH-2 for design reasons.
            // (change this message if the functionality is ever added and this test updated.)
            var id1 = Mock.String();
            var id2 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|\\H\\{0}\\N\\{1}", id1, id2));
            builder.SetField(1, 2, "^~$&");
            Assert.AreEqual(string.Format("MSH|^~$&|\\H\\{0}\\N\\{1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Subcomponent()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            builder.SetField(1, 2, "^~\\$");
            Assert.AreEqual(string.Format("MSH|^~\\$|{0}${1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Partially()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 2, "$");
            Assert.AreEqual("MSH|$|", builder.Value);
            Assert.AreEqual(builder.Encoding.EscapeCharacter, '\0');
            Assert.AreEqual(builder.Encoding.RepetitionDelimiter, '\0');
            Assert.AreEqual(builder.Encoding.SubcomponentDelimiter, '\0');
        }

        [TestMethod]
        public void MessageBuilder_CanUseDifferentFieldDelimiter()
        {
            var id = Mock.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH{0}^~\\&{0}{1}", delimiter, id));
            Assert.AreEqual(delimiter, builder.Encoding.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanChangeFieldDelimiter()
        {
            var id = Mock.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            builder.Encoding.FieldDelimiter = delimiter;
            Assert.AreEqual(delimiter, builder.Encoding.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanUseDifferentEscapeDelimiter()
        {
            var id = Mock.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH|^~{0}&|{1}", delimiter, id));
            Assert.AreEqual(delimiter, builder.Encoding.EscapeCharacter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanChangeEscapeDelimiter()
        {
            var id = Mock.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            builder.Encoding.FieldDelimiter = delimiter;
            Assert.AreEqual(delimiter, builder.Encoding.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanMapSegments()
        {
            var id = Mock.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id), tree.GetValue(1));
        }

        [TestMethod]
        public void MessageBuilder_CanMapFields()
        {
            var id = Mock.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(id, tree.GetValue(1, 3));
        }

        [TestMethod]
        public void MessageBuilder_CanMapRepetitions()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 2));
        }

        [TestMethod]
        public void MessageBuilder_CanMapComponents()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 2));
        }

        [TestMethod]
        public void MessageBuilder_CanMapSubcomponents()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 1, 2));
        }
    }
}
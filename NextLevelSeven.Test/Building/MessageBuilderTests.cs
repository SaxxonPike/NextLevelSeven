using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public class MessageBuilderTests
    {
        [TestMethod]
        public void MessageBuilder_CanBuildFields_Individually()
        {
            var builder = new MessageBuilder();
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .Field(1, 3, field3)
                .Field(1, 5, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = new MessageBuilder();
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .Field(1, 5, field5)
                .Field(1, 3, field3);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_Sequentially()
        {
            var builder = new MessageBuilder();
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .Fields(1, 3, field3, null, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_Individually()
        {
            var builder = new MessageBuilder();
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(1, 3, 1, repetition1)
                .FieldRepetition(1, 3, 2, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = new MessageBuilder();
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(1, 3, 2, repetition2)
                .FieldRepetition(1, 3, 1, repetition1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = new MessageBuilder();
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetitions(1, 3, repetition1, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_Individually()
        {
            var builder = new MessageBuilder();
            var segment2 = "ZZZ|" + Randomized.String();
            var segment3 = "ZAA|" + Randomized.String();

            builder
                .Segment(2, segment2)
                .Segment(3, segment3);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_OutOfOrder()
        {
            var builder = new MessageBuilder();
            var segment2 = "ZOT|" + Randomized.String();
            var segment3 = "ZED|" + Randomized.String();

            builder
                .Segment(4, segment3)
                .Segment(2, segment2);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_Sequentially()
        {
            var builder = new MessageBuilder();
            var segment2 = "ZIP|" + Randomized.String();
            var segment3 = "ZAP|" + Randomized.String();

            builder
                .Segments(2, segment2, segment3);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_Individually()
        {
            var builder = new MessageBuilder();
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, 3, 1, 1, component1)
                .Component(1, 3, 1, 2, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = new MessageBuilder();
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, 3, 1, 2, component2)
                .Component(1, 3, 1, 1, component1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_Sequentially()
        {
            var builder = new MessageBuilder();
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Components(1, 3, 1, component1, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = new MessageBuilder();
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(1, 3, 1, 1, 1, subcomponent1)
                .Subcomponent(1, 3, 1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = new MessageBuilder();
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(1, 3, 1, 1, 2, subcomponent2)
                .Subcomponent(1, 3, 1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = new MessageBuilder();
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponents(1, 3, 1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsToMessage()
        {
            var builder = new MessageBuilder(ExampleMessages.Standard);
            var beforeMessageString = builder.ToString();
            var message = builder.ToMessage();
            Assert.AreEqual(beforeMessageString, message.ToString(), "Conversion from builder to message failed.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsFromMessage()
        {
            var message = new Message(ExampleMessages.Standard);
            var beforeBuilderString = message.ToString();
            var afterBuilder = new MessageBuilder(message);
            Assert.AreEqual(beforeBuilderString, afterBuilder.ToString(), "Conversion from message to builder failed.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsMshCorrectly()
        {
            var builder = new MessageBuilder(ExampleMessages.MshOnly);
            Assert.AreEqual(ExampleMessages.MshOnly, builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_UsesReasonableMemory_WhenParsingLargeMessages()
        {
            var before = GC.GetTotalMemory(true);
            var message = new MessageBuilder();
            message.Field(1000000, 1000000, Randomized.String());
            var messageString = message.ToString();
            var usage = GC.GetTotalMemory(false) - before;
            var overhead = usage - (messageString.Length << 1);
            var usePerCharacter = (overhead / (messageString.Length << 1));
            Assert.IsTrue(usePerCharacter < 10);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultFieldDelimiter()
        {
            var builder = new MessageBuilder();
            Assert.AreEqual('|', builder.FieldDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultComponentDelimiter()
        {
            var builder = new MessageBuilder();
            Assert.AreEqual('^', builder.ComponentDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultSubcomponentDelimiter()
        {
            var builder = new MessageBuilder();
            Assert.AreEqual('&', builder.SubcomponentDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultEscapeDelimiter()
        {
            var builder = new MessageBuilder();
            Assert.AreEqual('\\', builder.EscapeDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultRepetitionDelimiter()
        {
            var builder = new MessageBuilder();
            Assert.AreEqual('~', builder.RepetitionDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_ContainsSegmentBuilders()
        {
            var builder = new MessageBuilder();
            Assert.IsInstanceOfType(builder[1], typeof(SegmentBuilder));
        }

        [TestMethod]
        public void MessageBuilder_ReturnsSegmentValues()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}\xDPID|{1}", id1, id2));
            var builderValues = builder.Values;
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id1), builderValues[1]);
            Assert.AreEqual(string.Format("PID|{0}", id2), builderValues[2]);
        }

        [TestMethod]
        public void MessageBuilder_ReturnsSegmentValuesAsArray()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}\xDPID|{1}", id1, id2));
            var builderValues = builder.Values.ToArray();
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id1), builderValues[0]);
            Assert.AreEqual(string.Format("PID|{0}", id2), builderValues[1]);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh1()
        {
            var builder = new MessageBuilder("MSH|^~\\&|");
            builder.Field(1, 1, ":");
            Assert.AreEqual("MSH:^~\\&:", builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh1ToDefaultWithNull()
        {
            var builder = new MessageBuilder("MSH|^~\\&|");
            builder.Field(1, 1, null);
            Assert.AreEqual("MSH|^~\\&|", builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2()
        {
            var builder = new MessageBuilder("MSH|^~\\&|");
            builder.Field(1, 2, "@#$%");
            Assert.AreEqual("MSH|@#$%|", builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetTypeToMsh()
        {
            var builder = new MessageBuilder();
            builder.Field(2, 0, "MSH");
        }

        [TestMethod]
        public void MessageBuilder_CanNotChangeTypeFromMsh()
        {
            var builder = new MessageBuilder();
            It.Throws<BuilderException>(() => builder.Field(1, 0, "PID"));
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Component()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            builder.Field(1, 2, "$~\\&");
            Assert.AreEqual(string.Format("MSH|$~\\&|{0}${1}", id1, id2), builder.ToString());            
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Repetition()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            builder.Field(1, 2, "^$\\&");
            Assert.AreEqual(string.Format("MSH|^$\\&|{0}${1}", id1, id2), builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Escape()
        {
            // note: changing escape code does not affect anything but MSH-2 for design reasons.
            // (change this message if the functionality is ever added and this test updated.)
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|\\H\\{0}\\N\\{1}", id1, id2));
            builder.Field(1, 2, "^~$&");
            Assert.AreEqual(string.Format("MSH|^~$&|\\H\\{0}\\N\\{1}", id1, id2), builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Subcomponent()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            builder.Field(1, 2, "^~\\$");
            Assert.AreEqual(string.Format("MSH|^~\\$|{0}${1}", id1, id2), builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2ToDefaultWithNull()
        {
            var builder = new MessageBuilder("MSH|^~\\&|");
            builder.Field(1, 2, null);
            Assert.AreEqual("MSH|^~\\&|", builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Partially()
        {
            var builder = new MessageBuilder("MSH|^~\\&|");
            builder.Field(1, 2, "$");
            Assert.AreEqual("MSH|$~\\&|", builder.ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanUseDifferentFieldDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = new MessageBuilder(string.Format("MSH{0}^~\\&{0}{1}", delimiter, id));
            Assert.AreEqual(delimiter, builder.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanChangeFieldDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}", id)) {FieldDelimiter = delimiter};
            Assert.AreEqual(delimiter, builder.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanUseDifferentEscapeDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = new MessageBuilder(string.Format("MSH|^~{0}&|{1}", delimiter, id));
            Assert.AreEqual(delimiter, builder.EscapeDelimiter);
            Assert.AreEqual(id, builder[1][3].ToString());
        }

        [TestMethod]
        public void MessageBuilder_CanChangeEscapeDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = new MessageBuilder(string.Format("MSH|^~\\&|{0}", id)) { FieldDelimiter = delimiter };
            Assert.AreEqual(delimiter, builder.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].ToString());
        }
    }
}

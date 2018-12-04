using System;
using System.Linq;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Test.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class MessageBuilderFunctionalTestFixture : ElementBuilderBaseTestFixture<IMessageBuilder, IMessage>
    {
        protected override IMessageBuilder BuildBuilder()
        {
            return Message.Build(ExampleMessageRepository.Standard);
        }

        [Test]
        public void MessageBuilder_RetrievalMethodsAreIdentical()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            message.GetValue(1).Should().Be(message[1].RawValue);
            message.GetValue(1, 3).Should().Be(message[1][3].RawValue);
            message.GetValue(1, 3, 1).Should().Be(message[1][3][1].RawValue);
            message.GetValue(1, 3, 1, 1).Should().Be(message[1][3][1][1].RawValue);
            message.GetValue(1, 3, 1, 1, 1).Should().Be(message[1][3][1][1][1].RawValue);
        }

        [Test]
        public void MessageBuilder_MultiRetrievalMethodsAreIdentical()
        {
            var message = Message.Build(ExampleMessageRepository.Variety);
            message.GetValues(1).Should().Equal(message[1].RawValues);
            message.GetValues(1, 3).Should().Equal(message[1][3].RawValues);
            message.GetValues(1, 3, 1).Should().Equal(message[1][3][1].RawValues);
            message.GetValues(1, 3, 1, 1).Should().Equal(message[1][3][1][1].RawValues);
            message.GetValues(1, 3, 1, 1, 1).Should().Equal(message[1][3][1][1][1].RawValues);
        }

        [Test]
        public void MessageBuilder_ThrowsWithIncorrectFirstSegment()
        {
            Action act = () => Message.Build(Any.String());
            act.Should().Throw<ElementException>();
        }

        [Test]
        public void MessageBuilder_HasCorrectRootKey()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard);
            builder.Key.Should().Be(ElementDefaults.RootElementKey);
        }

        [Test]
        public void MessageBuilder_CanGetSegment()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1].RawValue.Should().NotBeNull();
            builder[1].RawValue.Should().Be(builder.Segment(1).RawValue);
        }

        [Test]
        public void MessageBuilder_CanCreateNewMessageFromSegments()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var builder = message.Segments.OfType("PID").ToNewBuilder();
            builder.ValueCount.Should().Be(3);
            message[1].RawValue.Should().Be(builder[1].RawValue);
            message.Segments.OfType("PID").First().RawValue.Should().Be(builder[2].RawValue);
        }

        [Test]
        public void MessageBuilder_HasNoAncestor()
        {
            var builder = Message.Build();
            builder.Ancestor.Should().BeNull();
        }

        [Test]
        public void MessageBuilder_HasConverter()
        {
            var builder = Message.Build();
            builder.As.Should().NotBeNull();
        }

        [Test]
        public void MessageBuilder_HasDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard);
            builder.Descendants.Count().Should().Be(13);
        }

        [Test]
        public void MessageBuilder_HasDetails()
        {
            var val0 = Any.String();
            var message = $"MSH|^~\\&\r{val0}";
            var builder = Message.Build(message);
            builder.Details.Should().NotBeNull();
            builder.Details.Sender.Facility = val0;
            builder.Segment(1).Field(4).RawValue.Should().Be(val0);
        }

        [Test]
        public void MessageBuilder_CanInsertElementBeforeDescendant()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var message0 = $"MSH|^~\\&\r{val0}";
            var message1 = $"MSH|^~\\&\r{val1}";
            var builder0 = Message.Build(message0);
            var builder1 = Message.Build(message1);
            var builder2 = builder0.Clone();
            builder0.Insert(2, builder1[2]);
            builder0[2].RawValue.Should().Be(builder1[2].RawValue);
            builder0[3].RawValue.Should().Be(builder2[2].RawValue);
        }

        [Test]
        public void MessageBuilder_CanInsertStringBeforeDescendant()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var message = $"MSH|^~\\&\r{val0}";
            var builder = Message.Build(message);
            builder.Insert(2, val1);
            builder[2].RawValue.Should().Be(val1);
            builder[3].RawValue.Should().Be(val0);
        }

        [Test]
        public void MessageBuilder_CanInsertElementBeforeSelf()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var message0 = $"MSH|^~\\&\r{val0}";
            var message1 = $"MSH|^~\\&\r{val1}";
            var builder0 = Message.Build(message0);
            var builder1 = Message.Build(message1);
            var builder2 = builder0.Clone();
            builder0[2].Insert(builder1[2]);
            builder0[2].RawValue.Should().Be(builder1[2].RawValue);
            builder0[3].RawValue.Should().Be(builder2[2].RawValue);
        }

        [Test]
        public void MessageBuilder_CanInsertStringBeforeSelf()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var message = $"MSH|^~\\&\r{val0}";
            var builder = Message.Build(message);
            builder[2].Insert(val1);
            builder[2].RawValue.Should().Be(val1);
            builder[3].RawValue.Should().Be(val0);
        }

        [Test]
        public void MessageBuilder_CanAddValue()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var message = $"MSH|^~\\&\r{val0}";
            var builder = Message.Build(message);
            builder.Add(val1);
            builder[2].RawValue.Should().Be(val0);
            builder[3].RawValue.Should().Be(val1);
        }

        [Test]
        public void MessageBuilder_CanAddValues()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            const string message = "MSH|^~\\&";
            var builder = Message.Build(message);
            builder.AddRange(val0, val1);
            builder[2].RawValue.Should().Be(val0);
            builder[3].RawValue.Should().Be(val1);
        }

        [Test]
        public void MessageBuilder_CanAddValuesWithEnumerable()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            const string message = "MSH|^~\\&";
            var builder = Message.Build(message);
            builder.AddRange(new[] {val0, val1}.AsEnumerable());
            builder[2].RawValue.Should().Be(val0);
            builder[3].RawValue.Should().Be(val1);
        }

        [Test]
        public void MessageBuilder_CanGetValue()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var message = $"MSH|^~\\&\r{val0}\r{val1}";
            var builder = Message.Build(message);
            builder.RawValue.Should().Be(message);
        }

        [Test]
        public void MessageBuilder_CanGetValues()
        {
            var val0 = Any.StringCaps(3) + "|" + Any.String();
            var val1 = Any.StringCaps(3) + "|" + Any.String();
            var builder = Message.Build($"MSH|^~\\&\r{val0}\r{val1}");
            builder.RawValues.Should().Equal("MSH|^~\\&", val0, val1);
        }

        [Test]
        public void MessageBuilder_CanBuildFields_Individually()
        {
            var builder = Message.Build();
            var field3 = Any.String();
            var field5 = Any.String();

            builder
                .SetField(1, 3, field3)
                .SetField(1, 5, field5);
            builder.RawValue.Should().Be($"MSH|^~\\&|{field3}||{field5}");
        }

        [Test]
        public void MessageBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = Message.Build();
            var field3 = Any.String();
            var field5 = Any.String();

            builder
                .SetField(1, 5, field5)
                .SetField(1, 3, field3);
            builder.RawValue.Should().Be($"MSH|^~\\&|{field3}||{field5}");
        }

        [Test]
        public void MessageBuilder_CanBuildFields_Sequentially()
        {
            var builder = Message.Build();
            var field3 = Any.String();
            var field5 = Any.String();

            builder
                .SetFields(1, 3, field3, null, field5);
            builder.RawValue.Should().Be($"MSH|^~\\&|{field3}||{field5}");
        }

        [Test]
        public void MessageBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build();
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetition(1, 3, 1, repetition1)
                .SetFieldRepetition(1, 3, 2, repetition2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{repetition1}~{repetition2}");
        }

        [Test]
        public void MessageBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build();
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetition(1, 3, 2, repetition2)
                .SetFieldRepetition(1, 3, 1, repetition1);
            builder.RawValue.Should().Be($"MSH|^~\\&|{repetition1}~{repetition2}");
        }

        [Test]
        public void MessageBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build();
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetitions(1, 3, repetition1, repetition2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{repetition1}~{repetition2}");
        }

        [Test]
        public void MessageBuilder_CanBuildSegments_Individually()
        {
            var builder = Message.Build();
            var segment2 = "ZZZ|" + Any.String();
            var segment3 = "ZAA|" + Any.String();

            builder
                .SetSegment(2, segment2)
                .SetSegment(3, segment3);
            builder.RawValue.Should().Be($"MSH|^~\\&\xD{segment2}\xD{segment3}");
        }

        [Test]
        public void MessageBuilder_CanBuildSegments_OutOfOrder()
        {
            var builder = Message.Build();
            var segment2 = "ZOT|" + Any.String();
            var segment3 = "ZED|" + Any.String();

            builder
                .SetSegment(4, segment3)
                .SetSegment(2, segment2);
            builder.RawValue.Should().Be($"MSH|^~\\&\xD{segment2}\xD{segment3}");
        }

        [Test]
        public void MessageBuilder_CanBuildSegments_Sequentially()
        {
            var builder = Message.Build();
            var segment2 = "ZIP|" + Any.String();
            var segment3 = "ZAP|" + Any.String();

            builder
                .SetSegments(2, segment2, segment3);
            builder.RawValue.Should().Be($"MSH|^~\\&\xD{segment2}\xD{segment3}");
        }

        [Test]
        public void MessageBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build();
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(1, 3, 1, 1, component1)
                .SetComponent(1, 3, 1, 2, component2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{component1}^{component2}");
        }

        [Test]
        public void MessageBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build();
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(1, 3, 1, 2, component2)
                .SetComponent(1, 3, 1, 1, component1);
            builder.RawValue.Should().Be($"MSH|^~\\&|{component1}^{component2}");
        }

        [Test]
        public void MessageBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build();
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponents(1, 3, 1, component1, component2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{component1}^{component2}");
        }

        [Test]
        public void MessageBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build();
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, 3, 1, 1, 1, subcomponent1)
                .SetSubcomponent(1, 3, 1, 1, 2, subcomponent2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void MessageBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build();
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, 3, 1, 1, 2, subcomponent2)
                .SetSubcomponent(1, 3, 1, 1, 1, subcomponent1);
            builder.RawValue.Should().Be($"MSH|^~\\&|{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void MessageBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build();
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponents(1, 3, 1, 1, 1, subcomponent1, subcomponent2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void MessageBuilder_ConvertsToParser()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard);
            var beforeMessageString = builder.RawValue;
            var message = builder.ToParser();
            message.RawValue.Should().Be(beforeMessageString);
        }

        [Test]
        public void MessageBuilder_ConvertsFromParser()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var beforeBuilderString = message.RawValue;
            var afterBuilder = Message.Build(message);
            afterBuilder.RawValue.Should().Be(beforeBuilderString);
        }

        [Test]
        public void MessageBuilder_ConvertsMshCorrectly()
        {
            var builder = Message.Build(ExampleMessageRepository.MshOnly);
            builder.RawValue.Should().Be(ExampleMessageRepository.MshOnly);
        }

        [Test]
        public void MessageBuilder_UsesReasonableMemory_WhenParsingLargeMessages()
        {
            var before = GC.GetTotalMemory(true);
            var message = Message.Build();
            message.SetField(1000000, 1000000, Any.String());
            var messageString = message.RawValue;
            var usage = GC.GetTotalMemory(false) - before;
            var overhead = usage - (messageString.Length << 1);
            var usePerCharacter = overhead/(messageString.Length << 1);
            usePerCharacter.Should().BeLessThan(10);
        }

        [Test]
        public void MessageBuilder_HasProperDefaultFieldDelimiter()
        {
            var builder = Message.Build();
            builder.Encoding.FieldDelimiter.Should().Be('|');
        }

        [Test]
        public void MessageBuilder_HasProperDefaultComponentDelimiter()
        {
            var builder = Message.Build();
            builder.Encoding.ComponentDelimiter.Should().Be('^');
        }

        [Test]
        public void MessageBuilder_HasProperDefaultSubcomponentDelimiter()
        {
            var builder = Message.Build();
            builder.Encoding.SubcomponentDelimiter.Should().Be('&');
        }

        [Test]
        public void MessageBuilder_HasProperDefaultEscapeDelimiter()
        {
            var builder = Message.Build();
            builder.Encoding.EscapeCharacter.Should().Be('\\');
        }

        [Test]
        public void MessageBuilder_HasProperDefaultRepetitionDelimiter()
        {
            var builder = Message.Build();
            builder.Encoding.RepetitionDelimiter.Should().Be('~');
        }

        [Test]
        public void MessageBuilder_ContainsSegmentBuilders()
        {
            var builder = Message.Build();
            builder[1].Should().BeAssignableTo<ISegmentBuilder>();
        }

        [Test]
        public void MessageBuilder_ReturnsSegmentValues()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{id1}\xDPID|{id2}");
            var builderValues = builder.RawValues.ToList();
            builderValues[0].Should().Be($"MSH|^~\\&|{id1}");
            builderValues[1].Should().Be($"PID|{id2}");
        }

        [Test]
        public void MessageBuilder_ReturnsSegmentValuesAsArray()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{id1}\xDPID|{id2}");
            var builderValues = builder.RawValues.ToArray();
            builderValues[0].Should().Be($"MSH|^~\\&|{id1}");
            builderValues[1].Should().Be($"PID|{id2}");
        }

        [Test]
        public void MessageBuilder_CanSetMsh1()
        {
            var builder = Message.Build(ExampleMessageRepository.Minimum + "|");
            builder.SetField(1, 1, ":");
            builder.RawValue.Should().Be("MSH:^~\\&:");
        }

        [Test]
        public void MessageBuilder_CanSetMsh2()
        {
            var builder = Message.Build(ExampleMessageRepository.Minimum + "|");
            builder.SetField(1, 2, "@#$%");
            builder.RawValue.Should().Be("MSH|@#$%|");
        }

        [Test]
        public void MessageBuilder_CanSetTypeToMsh()
        {
            var builder = Message.Build();
            builder.SetField(2, 0, "MSH");
            builder[2][0].RawValue.Should().Be("MSH");
        }

        [Test]
        public void MessageBuilder_CanNotChangeTypeFromMsh()
        {
            var builder = Message.Build();
            builder.Invoking(b => b.SetField(1, 0, "PID")).Should().Throw<ElementException>();
        }

        [Test]
        public void MessageBuilder_CanNotChangeTypeToMsh()
        {
            // NOTE: by design.
            // (change this test if the design changes.)
            var builder = Message.Build();
            builder.SetField(2, 0, "PID");
            builder.Invoking(b => b.SetField(2, 0, "MSH")).Should().Throw<ElementException>();
        }

        [Test]
        public void MessageBuilder_CanSetMsh2Component()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{id1}^{id2}");
            builder.SetField(1, 2, "$~\\&");
            builder.RawValue.Should().Be($"MSH|$~\\&|{id1}${id2}");
        }

        [Test]
        public void MessageBuilder_CanSetMsh2Repetition()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{id1}~{id2}");
            builder.SetField(1, 2, "^$\\&");
            builder.RawValue.Should().Be($"MSH|^$\\&|{id1}${id2}");
        }

        [Test]
        public void MessageBuilder_CanSetMsh2Escape()
        {
            // NOTE: changing escape code does not affect anything but MSH-2 for design reasons.
            // (change this message if the functionality is ever added and this test updated.)
            var id1 = Any.String();
            var id2 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|\\H\\{id1}\\N\\{id2}");
            builder.SetField(1, 2, "^~$&");
            builder.RawValue.Should().Be($"MSH|^~$&|\\H\\{id1}\\N\\{id2}");
        }

        [Test]
        public void MessageBuilder_CanSetMsh2Subcomponent()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{id1}&{id2}");
            builder.SetField(1, 2, "^~\\$");
            builder.RawValue.Should().Be($"MSH|^~\\$|{id1}${id2}");
        }

        [Test]
        public void MessageBuilder_CanSetMsh2Partially()
        {
            var builder = Message.Build(ExampleMessageRepository.Minimum + "|");
            builder.SetField(1, 2, "$");
            builder.RawValue.Should().Be("MSH|$|");
            builder.Encoding.EscapeCharacter.Should().Be('\0');
            builder.Encoding.RepetitionDelimiter.Should().Be('\0');
            builder.Encoding.SubcomponentDelimiter.Should().Be('\0');
        }

        [Test]
        public void MessageBuilder_CanUseDifferentFieldDelimiter()
        {
            var id = Any.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH{0}^~\\&{0}{1}", delimiter, id));
            builder.Encoding.FieldDelimiter.Should().Be(delimiter);
            builder[1][3].RawValue.Should().Be(id);
        }

        [Test]
        public void MessageBuilder_CanChangeFieldDelimiter()
        {
            var id = Any.String();
            const char delimiter = ':';
            var builder = Message.Build($"MSH|^~\\&|{id}");
            builder.Encoding.FieldDelimiter = delimiter;
            builder.Encoding.FieldDelimiter.Should().Be(delimiter);
            builder[1][3].RawValue.Should().Be(id);
        }

        [Test]
        public void MessageBuilder_CanUseDifferentEscapeDelimiter()
        {
            var id = Any.String();
            const char delimiter = ':';
            var builder = Message.Build($"MSH|^~{delimiter}&|{id}");
            builder.Encoding.EscapeCharacter.Should().Be(delimiter);
            builder[1][3].RawValue.Should().Be(id);
        }

        [Test]
        public void MessageBuilder_CanChangeEscapeDelimiter()
        {
            var id = Any.String();
            const char delimiter = ':';
            var builder = Message.Build($"MSH|^~\\&|{id}");
            builder.Encoding.FieldDelimiter = delimiter;
            builder.Encoding.FieldDelimiter.Should().Be(delimiter);
            builder[1][3].RawValue.Should().Be(id);
        }

        [Test]
        public void MessageBuilder_CanMapSegments()
        {
            var id = Any.String();
            IMessage tree = Message.Build($"MSH|^~\\&|{id}");
            tree.GetValue(1).Should().Be($"MSH|^~\\&|{id}");
        }

        [Test]
        public void MessageBuilder_CanMapFields()
        {
            var id = Any.String();
            IMessage tree = Message.Build($"MSH|^~\\&|{id}");
            tree.GetValue(1, 3).Should().Be(id);
        }

        [Test]
        public void MessageBuilder_CanMapRepetitions()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            IMessage tree = Message.Build($"MSH|^~\\&|{id1}~{id2}");
            tree.GetValue(1, 3, 1).Should().Be(id1);
            tree.GetValue(1, 3, 2).Should().Be(id2);
        }

        [Test]
        public void MessageBuilder_CanMapComponents()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            IMessage tree = Message.Build($"MSH|^~\\&|{id1}^{id2}");
            tree.GetValue(1, 3, 1, 1).Should().Be(id1);
            tree.GetValue(1, 3, 1, 2).Should().Be(id2);
        }

        [Test]
        public void MessageBuilder_CanMapSubcomponents()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            IMessage tree = Message.Build($"MSH|^~\\&|{id1}&{id2}");
            tree.GetValue(1, 3, 1, 1, 1).Should().Be(id1);
            tree.GetValue(1, 3, 1, 1, 2).Should().Be(id2);
        }
    }
}
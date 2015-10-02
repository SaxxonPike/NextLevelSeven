using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class FieldBuilderUnitTests : BuildingTestFixture
    {
        [TestMethod]
        public void FieldBuilder_Type_CannotMoveDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][0];
            AssertAction.Throws<ElementException>(() => builder.Move(1, 2));
        }

        [TestMethod]
        public void FieldBuilder_Type_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][0];
            Assert.AreEqual(0, builder.Descendants.Count());
        }

        [TestMethod]
        public void FieldBuilder_Type_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][0];
            Assert.AreEqual('\0', builder.Delimiter);
        }

        [TestMethod]
        public void FieldBuilder_Type_ThrowsOnIndex()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][0];
            AssertAction.Throws<ElementException>(() => Assert.Inconclusive(builder[1].Value));
        }

        [TestMethod]
        public void FieldBuilder_Type_SetsRepetitionOne()
        {
            var builder = Message.Build(ExampleMessages.Standard)[2][0];
            var value = Mock.String();
            builder.SetFieldRepetition(1, value);
            Assert.AreEqual(value, builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_Type_ThrowsOnRepetitionsOtherThanOne()
        {
            var builder = Message.Build(ExampleMessages.Standard)[2][0];
            var value = Mock.String();
            AssertAction.Throws<ElementException>(() => builder.SetFieldRepetition(0, value));
            AssertAction.Throws<ElementException>(() => builder.SetFieldRepetition(2, value));
        }

        [TestMethod]
        public void FieldBuilder_Encoding_Exists()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            Assert.IsTrue(builder.Exists);
        }

        [TestMethod]
        public void FieldBuilder_Encoding_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            Assert.AreEqual(0, builder.Descendants.Count());
        }

        [TestMethod]
        public void FieldBuilder_Encoding_HasMultipleValues()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            Assert.AreEqual(4, builder.ValueCount);
        }

        [TestMethod]
        public void FieldBuilder_Encoding_GetsValues()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            builder.Value = "$%^&";
            AssertArray.AreEqual(new[] {"$", "%", "^", "&"}, builder.Values.ToArray());
        }

        [TestMethod]
        public void FieldBuilder_Encoding_SetsValue()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            builder.Value = "$%^&";
            Assert.AreEqual(builder.Value, "$%^&");
        }

        [TestMethod]
        public void FieldBuilder_Encoding_SetsValues()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            builder.Values = new[] {"$", "#", "~", "@"};
            Assert.AreEqual(builder.Value, "$#~@");
        }

        [TestMethod]
        public void FieldBuilder_Encoding_CanNullify()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            builder.Nullify();
            Assert.IsNull(builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_Encoding_ThrowsOnSubdivision()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            IElement test = null;
            AssertAction.Throws<BuilderException>(() => test = builder[1]);
            Assert.IsNull(test);
        }

        [TestMethod]
        public void FieldBuilder_Encoding_ThrowsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            IElement test = null;
            AssertAction.Throws<BuilderException>(() => test = builder.SetFieldRepetition(0, "~@#$"));
            Assert.IsNull(test);
        }

        [TestMethod]
        public void Fieldbuilder_Encoding_SetsOversizedValue()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var builder = message[1][2];
            var value = "!@#$%";
            builder.Value = value;
            Assert.AreEqual('!', message.Encoding.ComponentDelimiter);
            Assert.AreEqual('@', message.Encoding.RepetitionDelimiter);
            Assert.AreEqual('#', message.Encoding.EscapeCharacter);
            Assert.AreEqual('$', message.Encoding.SubcomponentDelimiter);
            Assert.AreEqual(value, builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_Encoding_SetsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            builder.SetFieldRepetition(1, "~@#$");
            Assert.AreEqual(builder.Value, "~@#$");
        }

        [TestMethod]
        public void FieldBuilder_Encoding_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][2];
            Assert.AreEqual('\0', builder.Delimiter);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_HasOneValue()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            Assert.AreEqual(1, builder.ValueCount);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            Assert.AreEqual('\0', builder.Delimiter);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            Assert.AreEqual(0, builder.Descendants.Count());
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_GetsValue()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            Assert.AreEqual("|", builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_GetsValues()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            Assert.AreEqual(1, builder.Values.Count());
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_SetsSingleValueFromOnlyFirstCharacter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            builder.Value = "$%^&";
            Assert.AreEqual(builder.Value, "$");
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_SetsValuesFromOnlyFirstCharacter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            builder.Values = new[] {"$", "#"};
            Assert.AreEqual(builder.Value, "$");
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_CanNullify()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            builder.Nullify();
            Assert.IsNull(builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_ThrowsOnSubdivision()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            IElement test = null;
            AssertAction.Throws<BuilderException>(() => test = builder[1]);
            Assert.IsNull(test);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_ThrowsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            IElement test = null;
            AssertAction.Throws<BuilderException>(() => test = builder.SetFieldRepetition(0, "$"));
            Assert.IsNull(test);
        }

        [TestMethod]
        public void FieldBuilder_Delimiter_SetsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            builder.SetFieldRepetition(1, "$");
            Assert.AreEqual(builder.Value, "$");
        }

        [TestMethod]
        public void FieldBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1];
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void FieldBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1] as IField;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void FieldBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][1] as IElement;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void FieldBuilder_CanGetRepetition()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][2].Value);
            Assert.AreEqual(builder[1][3][2].Value, builder.Segment(1).Field(3).Repetition(2).Value,
                "Repetitions returned differ.");
        }

        [TestMethod]
        public void FieldBuilder_CanGetValue()
        {
            var val0 = Mock.String();
            var val1 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", val0, val1))[1][3];
            Assert.AreEqual(builder.Value, string.Format("{0}~{1}", val0, val1));
        }

        [TestMethod]
        public void FieldBuilder_CanGetValues()
        {
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            var val3 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                val0, val1, val2, val3))[1][3];
            AssertEnumerable.AreEqual(builder.Values, new[] {val0, string.Format("{0}^{1}&{2}", val1, val2, val3)});
        }

        [TestMethod]
        public void FieldBuilder_CanBeCloned()
        {
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Mock.String(), Mock.String(), Mock.String(), Mock.String()))[1][3];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetition(1, repetition1)
                .SetFieldRepetition(2, repetition2);
            Assert.AreEqual(string.Format("{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetition(2, repetition2)
                .SetFieldRepetition(1, repetition1);
            Assert.AreEqual(string.Format("{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Mock.String();
            var repetition2 = Mock.String();

            builder
                .SetFieldRepetitions(3, repetition1, repetition2);
            Assert.AreEqual(string.Format("~~{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1][3];
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponent(1, 1, component1)
                .SetComponent(1, 2, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponent(1, 2, component2)
                .SetComponent(1, 1, component1);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var component1 = Mock.String();
            var component2 = Mock.String();

            builder
                .SetComponents(1, component1, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(1, 1, 1, subcomponent1)
                .SetSubcomponent(1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(1, 1, 2, subcomponent2)
                .SetSubcomponent(1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponents(1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanSetAllSubcomponents()
        {
            var builder = Message.Build()[1][3];
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder.SetSubcomponents(1, 1, 1, val0, val1, val2);
            Assert.AreEqual(string.Join("&", val0, val1, val2), builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_CanSetAllComponents()
        {
            var builder = Message.Build()[1][3];
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder.SetComponents(1, 1, val0, val1, val2);
            Assert.AreEqual(string.Join("^", val0, val1, val2), builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_CanSetAllRepetitions()
        {
            var builder = Message.Build()[1][3];
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder.SetFieldRepetitions(1, val0, val1, val2);
            Assert.AreEqual(string.Join("~", val0, val1, val2), builder.Value);
        }

        [TestMethod]
        public void FieldBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3];
            Assert.AreEqual(builder.Encoding.FieldDelimiter, '|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            Assert.AreEqual(builder.Encoding.FieldDelimiter, ':');
        }

        [TestMethod]
        public void FieldBuilder_ChangesEncodingCharactersIfSelfChanges()
        {
            var builder = Message.Build()[1][2];
            builder.SetField("$~\\&");
            Assert.AreEqual(builder.Encoding.ComponentDelimiter, '$');
        }
    }
}
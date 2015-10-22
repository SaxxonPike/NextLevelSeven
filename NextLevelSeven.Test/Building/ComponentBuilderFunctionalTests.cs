using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class ComponentBuilderFunctionalTests : BuildingTestFixture
    {
        [TestMethod]
        public void ComponentBuilder_CanMoveDescendants()
        {
            var builder = Message.Build(ExampleMessages.Variety)[1][3][1][1];
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder[1].Value = val1;
            builder[2].Value = val2;
            builder.Move(1, 2);
            Assert.AreEqual(builder[1].Value, val2);
            Assert.AreEqual(builder[2].Value, val1);
        }

        [TestMethod]
        public void ComponentBuilder_ExistsWithValue()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1];
            builder.Value = Mock.String();
            Assert.IsTrue(builder.Exists);
        }

        [TestMethod]
        public void ComponentBuilder_DoesNotExistWithNullValue()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1];
            builder.Value = null;
            Assert.IsFalse(builder.Exists);
        }

        [TestMethod]
        public void ComponentBuilder_DoesNotExistAfterErasing()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1];
            builder.Erase();
            Assert.IsFalse(builder.Exists);
        }

        [TestMethod]
        public void ComponentBuilder_HasDelimiter()
        {
            var builder = Message.Build()[1][3][1][1];
            Assert.AreEqual('&', builder.Delimiter);
        }

        [TestMethod]
        public void ComponentBuilder_HasSubcomponents()
        {
            var builder = Message.Build(ExampleMessages.Variety)[1][3][1][1];
            Assert.AreEqual(2, builder.Subcomponents.Count());
        }

        [TestMethod]
        public void ComponentBuilder_ClearsSubcomponentsWithNoParameters()
        {
            var builder = Message.Build()[1][3][1][1];
            builder.Values = new[] {Mock.String(), Mock.String()};
            builder.SetSubcomponents();
            Assert.IsNull(builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_DoesNotClearSubcomponentsWithNoParametersAndIndex()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Mock.String();
            var val1 = Mock.String();
            builder.Values = new[] { val0, val1 };
            builder.SetSubcomponents(1);
            Assert.AreEqual(string.Format("{0}&{1}", val0, val1), builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_CanSetComponent()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Mock.String();
            builder.SetComponent(val0);
            Assert.AreEqual(val0, builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_CanSetSubcomponent()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Mock.String();
            builder.SetSubcomponent(2, val0);
            Assert.AreEqual(string.Format("&{0}", val0), builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_CanSetSubcomponents()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder.SetSubcomponents(val0, val1, val2);
            Assert.AreEqual(string.Format("{0}&{1}&{2}", val0, val1, val2), builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_CanSetSubcomponentsAtIndex()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder.SetSubcomponents(2, val0, val1, val2);
            Assert.AreEqual(string.Join("&", "", val0, val1, val2), builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_CanSetValues()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Mock.String();
            var val1 = Mock.String();
            var val2 = Mock.String();
            builder.Values = new[] {val0, val1, val2};
            Assert.AreEqual(string.Format("{0}&{1}&{2}", val0, val1, val2), builder.Value);
        }

        [TestMethod]
        public void ComponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1];
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void ComponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1] as IComponent;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void ComponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1] as IElement;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void ComponentBuilder_CanGetSubcomponent()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][2][2][2].Value);
            Assert.AreEqual(builder[1][3][2][2][2].Value,
                builder.Segment(1).Field(3).Repetition(2).Component(2).Subcomponent(2).Value,
                "Subcomponents returned differ.");
        }

        [TestMethod]
        public void ComponentBuilder_CanGetValue()
        {
            var val0 = Mock.String();
            var val1 = Mock.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{2}^{0}&{1}", val0, val1, Mock.String()))[1][3][1][2];
            Assert.AreEqual(builder.Value, string.Format("{0}&{1}", val0, val1));
        }

        [TestMethod]
        public void ComponentBuilder_CanGetValues()
        {
            var val1 = Mock.String();
            var val2 = Mock.String();
            var val3 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Mock.String(), val1, val2, val3))[1][3][2][2];
            AssertEnumerable.AreEqual(builder.Values, new[] {val2, val3});
        }

        [TestMethod]
        public void ComponentBuilder_CanBeCloned()
        {
            var builder = Message.Build(Mock.Message())[1][3][2][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void ComponentBuilder_CanBeClonedGenerically()
        {
            IElement builder = Message.Build(Mock.Message())[1][3][2][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void ComponentBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(1, subcomponent1)
                .SetSubcomponent(2, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void ComponentBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponent(2, subcomponent2)
                .SetSubcomponent(1, subcomponent1);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void ComponentBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = Mock.String();
            var subcomponent2 = Mock.String();

            builder
                .SetSubcomponents(3, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("&&{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void ComponentBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3][1][1];
            Assert.AreEqual(builder.Encoding.FieldDelimiter, '|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            Assert.AreEqual(builder.Encoding.FieldDelimiter, ':');
        }
    }
}
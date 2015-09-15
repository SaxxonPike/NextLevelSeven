using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Conversion;
using NextLevelSeven.Core.Specification;

namespace NextLevelSeven.Test.Conversion
{
    [TestClass]
    public class AddressTypeConverterTests
    {
        [TestMethod]
        public void AddressTypeConverter_EncodesCurrent()
        {
            Assert.AreEqual("C", AddressTypeConverter.ConvertFromTable(AddressType.CurrentOrTemporary));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesPermanent()
        {
            Assert.AreEqual("P", AddressTypeConverter.ConvertFromTable(AddressType.Permanent));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesMailing()
        {
            Assert.AreEqual("M", AddressTypeConverter.ConvertFromTable(AddressType.Mailing));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesFirmOrBusiness()
        {
            Assert.AreEqual("B", AddressTypeConverter.ConvertFromTable(AddressType.FirmOrBusiness));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesOffice()
        {
            Assert.AreEqual("O", AddressTypeConverter.ConvertFromTable(AddressType.Office));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesHome()
        {
            Assert.AreEqual("H", AddressTypeConverter.ConvertFromTable(AddressType.Home));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesBirth()
        {
            Assert.AreEqual("N", AddressTypeConverter.ConvertFromTable(AddressType.Birth));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesCountryOfOrigin()
        {
            Assert.AreEqual("F", AddressTypeConverter.ConvertFromTable(AddressType.CountryOfOrigin));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesInvalidAsNull()
        {
            Assert.AreEqual(null, AddressTypeConverter.ConvertFromTable((AddressType) (-1)));
        }

        [TestMethod]
        public void AddressTypeConverter_EncodesNullAsNull()
        {
            Assert.AreEqual(null, AddressTypeConverter.ConvertFromTable(AddressType.Null));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesLowercaseCharacters()
        {
            Assert.AreEqual(AddressType.CurrentOrTemporary, AddressTypeConverter.ConvertToTable("c"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesCurrent()
        {
            Assert.AreEqual(AddressType.CurrentOrTemporary, AddressTypeConverter.ConvertToTable("C"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesPermanent()
        {
            Assert.AreEqual(AddressType.Permanent, AddressTypeConverter.ConvertToTable("P"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesMailing()
        {
            Assert.AreEqual(AddressType.Mailing, AddressTypeConverter.ConvertToTable("M"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesFirmOrBusiness()
        {
            Assert.AreEqual(AddressType.FirmOrBusiness, AddressTypeConverter.ConvertToTable("B"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesOffice()
        {
            Assert.AreEqual(AddressType.Office, AddressTypeConverter.ConvertToTable("O"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesHome()
        {
            Assert.AreEqual(AddressType.Home, AddressTypeConverter.ConvertToTable("H"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesBirth()
        {
            Assert.AreEqual(AddressType.Birth, AddressTypeConverter.ConvertToTable("N"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesCountryOfOrigin()
        {
            Assert.AreEqual(AddressType.CountryOfOrigin, AddressTypeConverter.ConvertToTable("F"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesInvalidAsNull()
        {
            Assert.AreEqual(AddressType.Null, AddressTypeConverter.ConvertToTable("\x1"));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesEmptyAsNull()
        {
            Assert.AreEqual(AddressType.Null, AddressTypeConverter.ConvertToTable(string.Empty));
        }

        [TestMethod]
        public void AddressTypeConverter_DecodesNullAsNull()
        {
            Assert.AreEqual(AddressType.Null, AddressTypeConverter.ConvertToTable(null));
        }
    }
}
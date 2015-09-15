using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Specification;

namespace NextLevelSeven.Test.Core.Specification
{
    [TestClass]
    public class AddressTests : SpecificationTestFixture
    {
        private string _streetAddress;
        private string _otherDesignation;
        private string _city;
        private string _stateOrProvince;
        private string _zipOrPostalCode;
        private string _country;
        private string _addressType;
        private string _otherGeographicDesignation;
        private string _message;
        private IAddress _address;

        [TestInitialize]
        public void Initialize()
        {
            _streetAddress = Randomized.String();
            _otherDesignation = Randomized.String();
            _city = Randomized.String();
            _stateOrProvince = Randomized.String();
            _zipOrPostalCode = Randomized.String();
            _country = Randomized.String();
            _addressType = "C";
            _otherGeographicDesignation = Randomized.String();
            _message = string.Format("MSH|^~\\&|{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}|{8}",
                _streetAddress, _otherDesignation, _city, _stateOrProvince,
                _zipOrPostalCode, _country, _addressType, _otherGeographicDesignation,
                Randomized.String());
            _address = Message.Create(_message)[1][3][1].AsAddress();
        }

        [TestMethod]
        public void Address_Validate_Fails_WithInvalidAddressType()
        {
            _address.AddressTypeData = "\x1";
            It.Throws<ValidationException>(_address.Validate, "Validation must fail with invalid address type.");
        }

        [TestMethod]
        public void Address_Validate_Succeeds_WithValidAddressType()
        {
            _address.Validate();
        }

        [TestMethod]
        public void Address_Validate_Succeeds_WithNullAddressType()
        {
            _address.AddressTypeData = null;
            _address.Validate();
        }

        [TestMethod]
        public void Address_Parses_StreetAddress()
        {
            Assert.AreEqual(_streetAddress, _address.StreetAddress);
        }

        [TestMethod]
        public void Address_Parses_OtherDesignation()
        {
            Assert.AreEqual(_otherDesignation, _address.OtherDesignation);
        }

        [TestMethod]
        public void Address_Parses_City()
        {
            Assert.AreEqual(_city, _address.City);
        }

        [TestMethod]
        public void Address_Parses_State()
        {
            Assert.AreEqual(_stateOrProvince, _address.StateOrProvince);
        }

        [TestMethod]
        public void Address_Parses_Zip()
        {
            Assert.AreEqual(_zipOrPostalCode, _address.ZipOrPostalCode);
        }

        [TestMethod]
        public void Address_Parses_Country()
        {
            Assert.AreEqual(_country, _address.Country);
        }

        [TestMethod]
        public void Address_Parses_AddressType()
        {
            Assert.AreEqual(AddressType.CurrentOrTemporary, _address.AddressType);
        }

        [TestMethod]
        public void Address_Parses_AddressTypeData()
        {
            Assert.AreEqual(_addressType, _address.AddressTypeData);
        }

        [TestMethod]
        public void Address_Parses_OtherGeographicDesignation()
        {
            Assert.AreEqual(_otherGeographicDesignation, _address.OtherGeographicDesignation);
        }

    }
}

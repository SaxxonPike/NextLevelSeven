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
            _message = string.Format("MSH|^~\\&|{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}",
                _streetAddress, _otherDesignation, _city, _stateOrProvince,
                _zipOrPostalCode, _country, _addressType, _otherGeographicDesignation);
        }

        [TestMethod]
        public void Address_Parses_StreetAddress()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_streetAddress, address.StreetAddress);
        }

        [TestMethod]
        public void Address_Parses_OtherDesignation()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_otherDesignation, address.OtherDesignation);
        }

        [TestMethod]
        public void Address_Parses_City()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_city, address.City);
        }

        [TestMethod]
        public void Address_Parses_State()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_stateOrProvince, address.StateOrProvince);
        }

        [TestMethod]
        public void Address_Parses_Zip()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_zipOrPostalCode, address.ZipOrPostalCode);
        }

        [TestMethod]
        public void Address_Parses_Country()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_country, address.Country);
        }

        [TestMethod]
        public void Address_Parses_AddressType()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(AddressType.CurrentOrTemporary, address.AddressType);
        }

        [TestMethod]
        public void Address_Parses_AddressTypeData()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_addressType, address.AddressTypeData);
        }

        [TestMethod]
        public void Address_Parses_OtherGeographicDesignation()
        {
            var message = Message.Create(_message);
            var address = message[1][3][1].AsAddress();
            Assert.AreEqual(_otherGeographicDesignation, address.OtherGeographicDesignation);
        }

    }
}

using NextLevelSeven.Core;
using NextLevelSeven.Specification.Conversion;

namespace NextLevelSeven.Specification.Elements
{
    internal class AddressElement : SpecificationElementBase, IAddress
    {
        public AddressElement(IElement element) : base(element)
        {
        }

        public string StreetAddress
        {
            get { return Element[1].Value; }
            set { Element[1].Value = value; }
        }

        public string OtherDesignation
        {
            get { return Element[2].Value; }
            set { Element[2].Value = value; }
        }

        public string City
        {
            get { return Element[3].Value; }
            set { Element[3].Value = value; }
        }

        public string StateOrProvince
        {
            get { return Element[4].Value; }
            set { Element[4].Value = value; }
        }

        public string ZipOrPostalCode
        {
            get { return Element[5].Value; }
            set { Element[5].Value = value; }
        }

        public string Country
        {
            get { return Element[6].Value; }
            set { Element[6].Value = value; }
        }

        public AddressType AddressType
        {
            get { return AddressTypeConverter.ConvertToTable(Element[7].Value); }
            set { Element[7].Value = AddressTypeConverter.ConvertFromTable(value); }
        }

        public string AddressTypeData
        {
            get { return Element[7].Value; }
            set { Element[7].Value = value; }
        }

        public string OtherGeographicDesignation
        {
            get { return Element[8].Value; }
            set { Element[8].Value = value; }
        }

        public override void Validate()
        {
            AssertTypedValueOrNull(AddressType, AddressTypeData, "AddressType");
        }
    }
}
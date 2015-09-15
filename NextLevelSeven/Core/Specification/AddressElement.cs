using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Conversion;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core.Specification
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

        override public void Validate()
        {
            AssertTypedValueOrNull(AddressType, AddressTypeData, "AddressType");
        }
    }
}

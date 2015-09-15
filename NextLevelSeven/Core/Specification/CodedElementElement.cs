using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core.Specification
{
    internal class CodedElementElement : SpecificationElementBase, ICodedElement
    {
        public CodedElementElement(IElement element)
            : base(element)
        {
        }

        public string Identifier
        {
            get { return Element[1].Value; }
            set { Element[1].Value = value; }
        }

        public string Text
        {
            get { return Element[2].Value; }
            set { Element[2].Value = value; }
        }

        public string CodingSystemName
        {
            get { return Element[3].Value; }
            set { Element[3].Value = value; }
        }

        public string AlternateIdentifier
        {
            get { return Element[4].Value; }
            set { Element[4].Value = value; }
        }

        public string AlternateText
        {
            get { return Element[5].Value; }
            set { Element[5].Value = value; }
        }

        public string AlternateCodingSystemName
        {
            get { return Element[6].Value; }
            set { Element[6].Value = value; }
        }

        override public void Validate()
        {
            // there are no required fields.
        }
    }
}

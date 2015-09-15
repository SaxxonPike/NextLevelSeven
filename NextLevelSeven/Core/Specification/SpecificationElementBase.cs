using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core.Specification
{
    abstract internal class SpecificationElementBase : ISpecificationElement
    {
        protected SpecificationElementBase(IElement element)
        {
            Element = element;
        }

        public IElement Element
        {
            get;
            private set;
        }

        public bool IsValid
        {
            get
            {
                try
                {
                    Validate();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public abstract void Validate();

        /// <summary>
        ///     Assert that either the value is null, or the typed value is not default (which should be a null default).
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="typedValue">Typed value.</param>
        /// <param name="value">Value.</param>
        /// <param name="fieldName">Name of the field that's being checked.</param>
        protected void AssertTypedValueOrNull<T>(T typedValue, string value, string fieldName)
        {
            if (value == null)
            {
                return;
            }
            if (!typedValue.Equals(default(T)))
            {
                return;
            }
            throw new ValidationException(ErrorCode.DataIsInvalidForType, fieldName);
        }

        /// <summary>
        ///     Assert that the length of a string is less than or equal to the specified length.
        /// </summary>
        /// <param name="length">Length limit.</param>
        /// <param name="value">String to validate.</param>
        protected void AssertTotalLengthIsWithin(int length, string value)
        {
            if (value == null || value.Length <= length)
            {
                return;
            }
            throw new ValidationException(ErrorCode.DataTypeIsTooLong);
        }
    }
}

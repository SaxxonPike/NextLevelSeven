using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core.Specification
{
    /// <summary>
    ///     Base class for HL7 specification element wrappers.
    /// </summary>
    abstract internal class SpecificationElementBase : ISpecificationElement
    {
        /// <summary>
        ///     Create a specification element wrapper.
        /// </summary>
        /// <param name="element"></param>
        protected SpecificationElementBase(IElement element)
        {
            Element = element;
        }

        /// <summary>
        ///     Element that is being wrapped.
        /// </summary>
        public IElement Element
        {
            get;
            private set;
        }

        /// <summary>
        ///     Return true if validation passes, otherwise false.
        /// </summary>
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

        /// <summary>
        ///     Perform validation. This throws a ValidationException when validation fails.
        /// </summary>
        public abstract void Validate();

        /// <summary>
        ///     Assert that a value is not null.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="fieldName">Name of the field that's being checked.</param>
        protected void AssertNotNull(string value, string fieldName)
        {
            if (value != null)
            {
                return;
            }
            throw new ValidationException(ErrorCode.FieldCannotBeNull, fieldName);
        }

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
        /// <param name="fieldName">Name of the field that's being checked.</param>
        protected void AssertTotalLengthIsWithin(int length, string value, string fieldName)
        {
            if (value == null || value.Length <= length)
            {
                return;
            }
            throw new ValidationException(ErrorCode.DataTypeIsTooLong, fieldName);
        }
    }
}

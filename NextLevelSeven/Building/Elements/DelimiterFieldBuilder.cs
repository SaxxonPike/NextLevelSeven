using System.Linq;
using NextLevelSeven.Core;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>A fixed field builder that notifies a segment builder when its value has changed.</summary>
    internal sealed class DelimiterFieldBuilder : StaticValueFieldBuilder
    {
        /// <summary>Create a field builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal DelimiterFieldBuilder(Builder builder, int index)
            : base(builder, index)
        {
        }

        /// <summary>Get or set the field type value.</summary>
        public override string Value
        {
            get => FieldDelimiter == '\0' ? null : new string(FieldDelimiter, 1);
            set
            {
                if (string.IsNullOrEmpty(value) || HL7.NullValues.Contains(value))
                {
                    FieldDelimiter = '\0';
                    return;
                }
                FieldDelimiter = value[0];
            }
        }
    }
}
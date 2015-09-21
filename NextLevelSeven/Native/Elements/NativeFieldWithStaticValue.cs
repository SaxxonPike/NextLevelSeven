using System.Collections.Generic;
using NextLevelSeven.Native.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    internal abstract class NativeFieldWithStaticValue : NativeField
    {
        /// <summary>
        ///     Create a field delimiter descendant.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index within the ancestor.</param>
        /// <param name="externalIndex">Exposed index.</param>
        protected NativeFieldWithStaticValue(NativeElement ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        /// <summary>
        ///     Delimiter is invalid for a field delimiter field.
        /// </summary>
        public override sealed char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>
        ///     False, as MSH-1 cannot have descendants.
        /// </summary>
        public override sealed bool HasSignificantDescendants
        {
            get { return false; }
        }

        /// <summary>
        ///     Get the field delimiter value.
        /// </summary>
        /// <param name="repetition">Not used.</param>
        /// <param name="component">Not used.</param>
        /// <param name="subcomponent">Not used.</param>
        /// <returns>Field delimiter value.</returns>
        public override sealed string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value;
        }

        /// <summary>
        ///     Get the field delimiter value.
        /// </summary>
        /// <param name="repetition">Not used.</param>
        /// <param name="component">Not used.</param>
        /// <param name="subcomponent">Not used.</param>
        /// <returns>Field delimiter value.</returns>
        public override sealed IEnumerable<string> GetValues(int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return Value.Yield();
        }

        /// <summary>
        ///     Get the descendant divider for this field.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the raw value.</param>
        /// <returns>Descendant divider within the specified index.</returns>
        protected override sealed IStringDivider GetDescendantDivider(NativeElement ancestor, int index)
        {
            return index <= 1
                ? new ProxyStringDivider(() => Value, v => Value = v)
                : new ProxyStringDivider(() => null, v => { });
        }

        /// <summary>
        ///     Get a repetition division of this field.
        /// </summary>
        /// <returns>Repetition descendant.</returns>
        protected override sealed NativeRepetition CreateRepetition(int index)
        {
            return new NativeRepetition(this, index - 1, index)
            {
                EncodingConfiguration = Core.Encoding.EncodingConfiguration.Empty
            };
        }

        /// <summary>
        ///     Deep clone this field.
        /// </summary>
        /// <returns>Cloned field.</returns>
        protected override sealed NativeField CloneInternal()
        {
            return new NativeField(Value, EncodingConfiguration) {Index = Index};
        }
    }
}
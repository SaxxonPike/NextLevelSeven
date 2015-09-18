using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a component level element of an HL7 message.
    /// </summary>
    internal sealed class NativeComponent : NativeElement, INativeComponent
    {
        /// <summary>
        ///     Internal subcomponent cache.
        /// </summary>
        private readonly IndexedCache<int, NativeSubcomponent> _cache;

        /// <summary>
        ///     Create a component.
        /// </summary>
        /// <param name="ancestor">Ancestor element to pull encoding information from.</param>
        /// <param name="parentIndex">Zero-based index within the parent divider.</param>
        /// <param name="externalIndex">HL7 index to identify as.</param>
        public NativeComponent(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _cache = new IndexedCache<int, NativeSubcomponent>(CreateSubcomponent);
        }

        /// <summary>
        ///     Create a detached component.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <param name="config">Encoding configuration.</param>
        private NativeComponent(string value, EncodingConfigurationBase config)
            : base(value, config)
        {
            _cache = new IndexedCache<int, NativeSubcomponent>(CreateSubcomponent);
        }

        /// <summary>
        ///     Delimiter to use for descendants.
        /// </summary>
        public override char Delimiter
        {
            get { return EncodingConfiguration.SubcomponentDelimiter; }
        }

        /// <summary>
        ///     Get the value at the specified index.
        /// </summary>
        /// <param name="subcomponent">Optional: Subcomponent index.</param>
        /// <returns>Value at the specified index.</returns>
        public string GetValue(int subcomponent = -1)
        {
            return subcomponent < 0
                ? Value
                : _cache[subcomponent].Value;
        }

        /// <summary>
        ///     Get the values at the specified index.
        /// </summary>
        /// <param name="subcomponent">Optional: Subcomponent index.</param>
        /// <returns>Values at the specified index.</returns>
        public IEnumerable<string> GetValues(int subcomponent = -1)
        {
            return subcomponent < 0
                ? Values
                : _cache[subcomponent].Value.Yield();
        }

        /// <summary>
        ///     Get a descendant element at the specified index.
        /// </summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Element at the specified index.</returns>
        public new INativeSubcomponent this[int index]
        {
            get { return _cache[index]; }
        }

        /// <summary>
        ///     Create a deep clone of the element.
        /// </summary>
        /// <returns>The cloned element.</returns>
        public override IElement Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Create a deep clone of the component.
        /// </summary>
        /// <returns>The cloned component.</returns>
        IComponent IComponent.Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Element at the specified index.</returns>
        public override INativeElement GetDescendant(int index)
        {
            return _cache[index];
        }

        /// <summary>
        ///     Create a subcomponent object.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Subcomponent.</returns>
        private NativeSubcomponent CreateSubcomponent(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SubcomponentIndexMustBeGreaterThanZero));
            }

            return new NativeSubcomponent(this, index - 1, index);
        }

        /// <summary>
        ///     Create a deep clone of the component.
        /// </summary>
        /// <returns>Cloned component.</returns>
        private NativeComponent CloneInternal()
        {
            return new NativeComponent(Value, EncodingConfiguration) {Index = Index};
        }
    }
}
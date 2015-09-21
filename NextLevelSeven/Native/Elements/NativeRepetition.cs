using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a repetition-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeRepetition : NativeElement, INativeRepetition
    {
        /// <summary>
        ///     Internal component cache.
        /// </summary>
        private readonly IndexedCache<int, NativeComponent> _cache;

        /// <summary>
        ///     Create a repetition with the specified ancestor, ancestor index, and exposed index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Index in the parent splitter.</param>
        /// <param name="externalIndex">Exposed index.</param>
        public NativeRepetition(NativeElement ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _cache = new IndexedCache<int, NativeComponent>(CreateComponent);
        }

        /// <summary>
        ///     Create a detached repetition with the specified initial value and encoding configuration.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <param name="config">Encoding configuration.</param>
        private NativeRepetition(string value, EncodingConfigurationBase config)
            : base(value, config)
        {
            _cache = new IndexedCache<int, NativeComponent>(CreateComponent);
        }

        /// <summary>
        ///     Component delimiter.
        /// </summary>
        public override char Delimiter
        {
            get { return EncodingConfiguration.ComponentDelimiter; }
        }

        /// <summary>
        ///     Get the value at the specified indices.
        /// </summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified indices.</returns>
        public string GetValue(int component = -1, int subcomponent = -1)
        {
            return component < 0
                ? Value
                : GetComponent(component).GetValue(subcomponent);
        }

        /// <summary>
        ///     Get the values at the specified indices.
        /// </summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Values at the specified indices.</returns>
        public IEnumerable<string> GetValues(int component = -1, int subcomponent = -1)
        {
            return component < 0
                ? Values
                : GetComponent(component).GetValues(subcomponent);
        }

        /// <summary>
        ///     Get the descendant component at the specified index.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Descendant component at the specified index.</returns>
        public new INativeComponent this[int index]
        {
            get { return _cache[index]; }
        }

        /// <summary>
        ///     Deep clone this repetition.
        /// </summary>
        /// <returns>Clone of this repetition.</returns>
        public override IElement Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Deep clone this repetition.
        /// </summary>
        /// <returns>Clone of this repetition.</returns>
        IRepetition IRepetition.Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Descendant element at the specified index.</returns>
        public override INativeElement GetDescendant(int index)
        {
            return _cache[index];
        }

        /// <summary>
        ///     Get the descendant component at the specified index.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Descendant component at the specified index.</returns>
        private INativeComponent GetComponent(int index)
        {
            return _cache[index];
        }

        /// <summary>
        ///     Create a descendant component object.
        /// </summary>
        /// <param name="index">Index of the component.</param>
        /// <returns>Component object.</returns>
        private NativeComponent CreateComponent(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.ComponentIndexMustBeGreaterThanZero));
            }

            var result = new NativeComponent(this, index - 1, index);
            return result;
        }

        /// <summary>
        ///     Deep clone this repetition.
        /// </summary>
        /// <returns>Clone of this repetition.</returns>
        private NativeRepetition CloneInternal()
        {
            return new NativeRepetition(Value, EncodingConfiguration) {Index = Index};
        }

        /// <summary>
        ///     Get all components.
        /// </summary>
        public IEnumerable<INativeComponent> Components
        {
            get
            {
                return new WrapperEnumerable<INativeComponent>(i => _cache[i],
                    (i, v) => { },
                    () => ValueCount,
                    1);
            }
        }

        /// <summary>
        ///     Get all components.
        /// </summary>
        IEnumerable<IComponent> IRepetition.Components
        {
            get
            {
                return Components;
            }
        }
    }
}
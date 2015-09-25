using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a repetition-level element in an HL7 message.</summary>
    internal sealed class RepetitionParser : DescendantParser, IRepetitionParser
    {
        /// <summary>Internal component cache.</summary>
        private readonly IndexedCache<int, ComponentParser> _components;

        /// <summary>Create a repetition with the specified ancestor, ancestor index, and exposed index.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Index in the parent splitter.</param>
        /// <param name="externalIndex">Exposed index.</param>
        public RepetitionParser(Parser ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _components = new IndexedCache<int, ComponentParser>(CreateComponent);
        }

        /// <summary>Create a repetition with the specified ancestor, ancestor index, and exposed index, with an alternative encoding configuration.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Index in the parent splitter.</param>
        /// <param name="externalIndex">Exposed index.</param>
        /// <param name="config">Encoding configuration to use.</param>
        public RepetitionParser(Parser ancestor, int parentIndex, int externalIndex, EncodingConfiguration config)
            : base(ancestor, parentIndex, externalIndex, config)
        {
            _components = new IndexedCache<int, ComponentParser>(CreateComponent);
        }

        /// <summary>Create a detached repetition with the specified initial value and encoding configuration.</summary>
        /// <param name="config">Encoding configuration.</param>
        private RepetitionParser(EncodingConfiguration config)
            : base(config)
        {
            _components = new IndexedCache<int, ComponentParser>(CreateComponent);
        }

        /// <summary>Component delimiter.</summary>
        public override char Delimiter
        {
            get { return EncodingConfiguration.ComponentDelimiter; }
        }

        /// <summary>Get the value at the specified indices.</summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified indices.</returns>
        public string GetValue(int component = -1, int subcomponent = -1)
        {
            return component < 0 ? Value : GetComponent(component).GetValue(subcomponent);
        }

        /// <summary>Get the values at the specified indices.</summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Values at the specified indices.</returns>
        public IEnumerable<string> GetValues(int component = -1, int subcomponent = -1)
        {
            return component < 0 ? Values : GetComponent(component).GetValues(subcomponent);
        }

        /// <summary>Get the descendant component at the specified index.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Descendant component at the specified index.</returns>
        public new IComponentParser this[int index]
        {
            get { return _components[index]; }
        }

        /// <summary>Deep clone this repetition.</summary>
        /// <returns>Clone of this repetition.</returns>
        public override IElement Clone()
        {
            return CloneInternal();
        }

        /// <summary>Deep clone this repetition.</summary>
        /// <returns>Clone of this repetition.</returns>
        IRepetition IRepetition.Clone()
        {
            return CloneInternal();
        }

        /// <summary>Get all components.</summary>
        public IEnumerable<IComponentParser> Components
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _components[i];
                }
            }
        }

        /// <summary>Get all components.</summary>
        IEnumerable<IComponent> IRepetition.Components
        {
            get { return Components; }
        }

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Descendant element at the specified index.</returns>
        public override IElementParser GetDescendant(int index)
        {
            return _components[index];
        }

        /// <summary>Get the descendant component at the specified index.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Descendant component at the specified index.</returns>
        private IComponentParser GetComponent(int index)
        {
            return _components[index];
        }

        /// <summary>Create a descendant component object.</summary>
        /// <param name="index">Index of the component.</param>
        /// <returns>Component object.</returns>
        private ComponentParser CreateComponent(int index)
        {
            if (index < 1)
            {
                throw new ParserException(ErrorCode.ComponentIndexMustBeGreaterThanZero);
            }

            var result = new ComponentParser(this, index - 1, index);
            return result;
        }

        /// <summary>Deep clone this repetition.</summary>
        /// <returns>Clone of this repetition.</returns>
        private RepetitionParser CloneInternal()
        {
            return new RepetitionParser(EncodingConfiguration)
            {
                Index = Index,
                Value = Value
            };
        }

        /// <summary>
        ///     Get this element's heirarchy-specific ancestor.
        /// </summary>
        IField IRepetition.Ancestor
        {
            get { return Ancestor as IField; }
        }
    }
}
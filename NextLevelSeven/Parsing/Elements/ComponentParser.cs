using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a component level element of an HL7 message.</summary>
    internal sealed class ComponentParser : DescendantParser, IComponentParser
    {
        /// <summary>Internal subcomponent cache.</summary>
        private readonly IndexedCache<SubcomponentParser> _subcomponents;

        /// <summary>Create a component.</summary>
        /// <param name="ancestor">Ancestor element to pull encoding information from.</param>
        /// <param name="parentIndex">Zero-based index within the parent divider.</param>
        /// <param name="externalIndex">HL7 index to identify as.</param>
        public ComponentParser(Parser ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
            _subcomponents = new WeakReferenceCache<SubcomponentParser>(CreateSubcomponent);
        }

        /// <summary>Create a detached component.</summary>
        /// <param name="config">Encoding configuration.</param>
        private ComponentParser(ReadOnlyEncodingConfiguration config)
            : base(config)
        {
            _subcomponents = new WeakReferenceCache<SubcomponentParser>(CreateSubcomponent);
        }

        /// <summary>Delimiter to use for descendants.</summary>
        public override char Delimiter => EncodingConfiguration.SubcomponentDelimiter;

        /// <summary>Get the value at the specified index.</summary>
        /// <param name="subcomponent">Optional: Subcomponent index.</param>
        /// <returns>Value at the specified index.</returns>
        public string GetValue(int subcomponent = -1)
        {
            return subcomponent < 0 ? Value : _subcomponents[subcomponent].Value;
        }

        /// <summary>Get the values at the specified index.</summary>
        /// <param name="subcomponent">Optional: Subcomponent index.</param>
        /// <returns>Values at the specified index.</returns>
        public IEnumerable<string> GetValues(int subcomponent = -1)
        {
            return subcomponent < 0 ? Values : _subcomponents[subcomponent].Value.Yield();
        }

        /// <summary>Get a descendant element at the specified index.</summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Element at the specified index.</returns>
        public new ISubcomponentParser this[int index] => _subcomponents[index];

        /// <summary>Create a deep clone of the element.</summary>
        /// <returns>The cloned element.</returns>
        public override IElement Clone()
        {
            return CloneComponent();
        }

        /// <summary>Create a deep clone of the component.</summary>
        /// <returns>The cloned component.</returns>
        IComponent IComponent.Clone()
        {
            return CloneComponent();
        }

        /// <summary>Get all subcomponents.</summary>
        public IEnumerable<ISubcomponentParser> Subcomponents
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _subcomponents[i];
                }
            }
        }

        /// <summary>Get all subcomponents.</summary>
        IEnumerable<ISubcomponent> IComponent.Subcomponents => Subcomponents;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IRepetition IComponent.Ancestor => Ancestor as IRepetition;

        /// <summary>Get this element's heirarchy-specific ancestor parser.</summary>
        IRepetitionParser IComponentParser.Ancestor => Ancestor as IRepetitionParser;

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Element at the specified index.</returns>
        protected override IElementParser GetDescendant(int index)
        {
            return _subcomponents[index];
        }

        /// <summary>Create a subcomponent object.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Subcomponent.</returns>
        private SubcomponentParser CreateSubcomponent(int index)
        {
            if (index < 1)
            {
                throw new ElementException(ErrorCode.SubcomponentIndexMustBeGreaterThanZero);
            }

            return new SubcomponentParser(this, index - 1, index);
        }

        /// <summary>Create a deep clone of the component.</summary>
        /// <returns>Cloned component.</returns>
        private ComponentParser CloneComponent()
        {
            return new ComponentParser(EncodingConfiguration)
            {
                Index = Index,
                Value = Value
            };
        }
    }
}
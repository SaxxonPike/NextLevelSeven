using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    public sealed class ComponentBuilder
    {
        private readonly EncodingConfiguration _encodingConfiguration;

        private readonly Dictionary<int, SubcomponentBuilder> _subcomponentBuilders =
            new Dictionary<int, SubcomponentBuilder>();

        internal ComponentBuilder(EncodingConfiguration encodingConfiguration)
        {
            _encodingConfiguration = encodingConfiguration;
        }

        public SubcomponentBuilder this[int index]
        {
            get
            {
                if (!_subcomponentBuilders.ContainsKey(index))
                {
                    _subcomponentBuilders[index] = new SubcomponentBuilder();
                }
                return _subcomponentBuilders[index];
            }
        }

        public int Count
        {
            get { return _subcomponentBuilders.Max(kv => kv.Key); }
        }

        public IEnumerableIndexable<int, string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].ToString(),
                    (index, data) => Subcomponent(index, data),
                    () => Count,
                    1);
            }
        }

        public ComponentBuilder Component(string value)
        {
            _subcomponentBuilders.Clear();
            var index = 1;

            value = value ?? string.Empty;
            foreach (var subcomponent in value.Split(_encodingConfiguration.SubcomponentDelimiter))
            {
                Subcomponent(index++, subcomponent);
            }

            return this;
        }

        public ComponentBuilder Subcomponent(int subcomponentIndex, string value)
        {
            this[subcomponentIndex].Subcomponent(value);
            return this;
        }

        public ComponentBuilder Subcomponents(params string[] subcomponents)
        {
            _subcomponentBuilders.Clear();
            var index = 1;
            foreach (var subcomponent in subcomponents)
            {
                Subcomponent(index++, subcomponent);
            }
            return this;
        }

        public ComponentBuilder Subcomponents(int startIndex, params string[] subcomponents)
        {
            var index = startIndex;
            foreach (var subcomponent in subcomponents)
            {
                Subcomponent(index++, subcomponent);
            }
            return this;
        }

        public override string ToString()
        {
            var index = 1;
            var result = new StringBuilder();

            foreach (var subcomponent in _subcomponentBuilders.OrderBy(i => i.Key))
            {
                while (index < subcomponent.Key)
                {
                    result.Append(_encodingConfiguration.SubcomponentDelimiter);
                    index++;
                }

                if (subcomponent.Key > 0)
                {
                    result.Append(subcomponent.Value);
                }
            }

            return result.ToString();
        }
    }
}
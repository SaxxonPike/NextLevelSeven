using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    public sealed class RepetitionBuilder
    {
        private readonly Dictionary<int, ComponentBuilder> _componentBuilders = new Dictionary<int, ComponentBuilder>();
        private readonly EncodingConfiguration _encodingConfiguration;
        private readonly bool _split;

        internal RepetitionBuilder(EncodingConfiguration encodingConfiguration, bool split)
        {
            _encodingConfiguration = encodingConfiguration;
            _split = split;
        }

        public ComponentBuilder this[int index]
        {
            get
            {
                if (!_componentBuilders.ContainsKey(index))
                {
                    _componentBuilders[index] = new ComponentBuilder(_encodingConfiguration);
                }
                return _componentBuilders[index];
            }
        }

        public int Count
        {
            get { return _componentBuilders.Max(kv => kv.Key); }
        }

        public IEnumerableIndexable<int, string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].ToString(),
                    (index, data) => Component(index, data),
                    () => Count,
                    1);
            }
        }

        public RepetitionBuilder Component(int componentIndex, string value)
        {
            this[componentIndex].Component(value);
            return this;
        }

        public RepetitionBuilder Components(params string[] components)
        {
            _componentBuilders.Clear();
            var index = 1;
            foreach (var component in components)
            {
                Component(index++, component);
            }
            return this;
        }

        public RepetitionBuilder Components(int startIndex, params string[] components)
        {
            var index = startIndex;
            foreach (var component in components)
            {
                Component(index++, component);
            }
            return this;
        }

        public RepetitionBuilder Subcomponent(int componentIndex, int subcomponentIndex, string value)
        {
            this[componentIndex].Subcomponent(subcomponentIndex, value);
            return this;
        }

        public RepetitionBuilder Subcomponents(int componentIndex, params string[] subcomponents)
        {
            this[componentIndex].Subcomponents(subcomponents);
            return this;
        }

        public RepetitionBuilder Subcomponents(int componentIndex, int startIndex, params string[] subcomponents)
        {
            this[componentIndex].Subcomponents(startIndex, subcomponents);
            return this;
        }

        public override string ToString()
        {
            var index = 1;
            var result = new StringBuilder();

            foreach (var component in _componentBuilders.OrderBy(i => i.Key))
            {
                while (index < component.Key)
                {
                    result.Append(_encodingConfiguration.ComponentDelimiter);
                    index++;
                }

                if (component.Key > 0)
                {
                    result.Append(component.Value);
                }
            }

            return result.ToString();
        }
    }
}
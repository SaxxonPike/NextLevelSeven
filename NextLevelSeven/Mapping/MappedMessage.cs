using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Mapping
{
    public class MappedMessage<TKey>
    {
        public MappedMessage(IMapConfiguration<TKey> configuration, IElementTree message)
        {
            Configuration = configuration;
            Message = message;
        }

        public IMapConfiguration<TKey> Configuration
        {
            get;
            private set;
        }

        public IEnumerable<string> GetAllValues(TKey key)
        {
            if (!Configuration.ContainsKey(key))
            {
                return Enumerable.Empty<string>();
            }

            var target = Configuration[key];
            if (!target.IsValid)
            {
                return Enumerable.Empty<string>();
            }

            if (target.Segment < 0)
            {
                return Message.GetValues(target.SegmentName, target.Field, target.Repetition, target.Component,
                    target.Subcomponent);
            }
            return Message.GetValue(target.Segment, target.Field, target.Repetition, target.Component,
                target.Subcomponent).Yield();
        }

        public string GetFirstValue(TKey key)
        {
            return GetAllValues(key).FirstOrDefault();
        }

        public IElementTree Message
        {
            get;
            private set;
        }
    }
}

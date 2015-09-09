using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Represents the highest level of an HL7 message.
    /// </summary>
    internal sealed class Message : Element
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();

        private readonly EncodingConfiguration _encodingConfiguration;
        private Guid _keyGuid;

        public Message(string message)
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(@"message");
            }

            _encodingConfiguration = new NativeMessageEncodingConfiguration(this);
        }

        public override char Delimiter
        {
            get { return '\xD'; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfiguration; }
        }

        public string Escape(string data)
        {
            return _encodingConfiguration.Escape(data);
        }

        public override string Key
        {
            get { return KeyGuid.ToString(); }
        }

        private Guid KeyGuid
        {
            get
            {
                if (_keyGuid == Guid.Empty)
                {
                    _keyGuid = Guid.NewGuid();
                }
                return _keyGuid;
            }
        }

        public IEnumerable<INativeSegment> Segments
        {
            get { return new SegmentEnumerable(this); }
        }

        public string UnEscape(string data)
        {
            return _encodingConfiguration.UnEscape(data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override INativeElement CloneDetached()
        {
            return new Message(Value);
        }

        public override INativeElement GetDescendant(int index)
        {
            return GetSegment(index);
        }

        public INativeSegment GetSegment(int index)
        {
            if (_cache.ContainsKey(index))
            {
                return _cache[index] as INativeSegment;
            }

            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SegmentIndexMustBeGreaterThanZero));
            }

            var result = new Segment(this, index - 1, index);
            _cache[index] = result;
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    /// Represents the highest level of an HL7 message.
    /// </summary>
    sealed internal class Message : Element
    {
        public Message(string message)
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(@"message");
            }

            _encodingConfiguration = new MessageEncodingConfiguration(this);
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

        public override IElement CloneDetached()
        {
            return new Message(Value);
        }

        public override char Delimiter
        {
            get { return '\xD'; }
        }

        private readonly EncodingConfiguration _encodingConfiguration;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return GetSegment(index);
        }

        public ISegment GetSegment(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SegmentIndexMustBeGreaterThanZero));
            }
            return new Segment(this, index - 1, index);
        }

        public override string Key
        {
            get { return KeyGuid.ToString(); }
        }

        private Guid _keyGuid;
        Guid KeyGuid
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

        public IEnumerable<ISegment> Segments
        {
            get { return new SegmentEnumerable(this); }
        }
    }
}

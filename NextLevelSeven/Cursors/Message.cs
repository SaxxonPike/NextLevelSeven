using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class Message : Element
    {
        public Message(string message)
            : base(message)
        {
            _encodingConfiguration = new EncodingConfiguration(this);
        }

        protected override char Delimiter
        {
            get { return '\n'; }
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
            return new Segment(this, index - 1, index);
        }

        public IEnumerable<ISegment> Segments
        {
            get { return new SegmentEnumerable(this); }
        }
    }
}

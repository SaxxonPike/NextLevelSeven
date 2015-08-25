using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Codecs;

namespace NextLevelSeven.Core
{
    public class Message : IMessage
    {
        public Message(string message = null)
        {
            _message = new Cursors.Message(message ?? @"MSH|^~\&|");
        }

        internal Message(Cursors.Message internalMessage)
        {
            _message = internalMessage;
        }

        private readonly Cursors.Message _message;

        public IElement this[int index]
        {
            get { return _message[index]; }
        }

        public IEnumerable<ISegment> this[string segmentType]
        {
            get { return Segments.Where(s => s.Type == segmentType); }
        }

        public IEnumerable<ISegment> this[IEnumerable<string> segmentTypes]
        {
            get { return Segments.Where(s => segmentTypes.Contains(s.Type)); }
        }

        public ICodec As
        {
            get { return _message.As; }
        }

        public IElement AncestorElement
        {
            get { return null; }
        }

        public IMessage Clone()
        {
            return new Message(Value);
        }

        public string ControlId
        {
            get { return Msh[10].Value; }
            set { Msh[10].Value = value; }
        }

        public void Delete()
        {
            throw new ElementException(@"The root element of a message cannot be deleted.");
        }

        public int DescendantCount
        {
            get { return _message.DescendantCount; }
        }

        public IEnumerable<IElement> DescendantElements
        {
            get { return _message.DescendantElements; }
        }

        public void Erase()
        {
            throw new ElementException(@"The root element of a message cannot be erased.");
        }

        public bool Exists
        {
            get { return true; }
        }

        public IElement GetField(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            var segmentElement = _message[segment];
            if (field < 0)
            {
                return segmentElement;
            }
            if (repetition < 0)
            {
                return segmentElement[field];
            }
            if (component < 0)
            {
                return segmentElement[field][repetition];
            }
            if (subcomponent < 0)
            {
                return segmentElement[field][repetition][component];
            }
            return segmentElement[field][repetition][component][subcomponent];
        }

        public IElement GetField(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            var segment = _message.FirstOrDefault(s => s.Value != null && s.Value.StartsWith(segmentName));
            if (field < 0 || segment == null)
            {
                return segment;
            }
            if (repetition < 0)
            {
                return segment[field];
            }
            if (component < 0)
            {
                return segment[field][repetition];
            }
            if (subcomponent < 0)
            {
                return segment[field][repetition][component];
            }
            return segment[field][repetition][component][subcomponent];
        }

        public int Index
        {
            get { return _message.Index; }
        }

        public string Key
        {
            get { return _message.Key; }
        }

        IMessage IElement.Message
        {
            get { return this; }
        }

        IElement Msh
        {
            get { return this["MSH"].First(); }
        }

        public void Nullify()
        {
            Value = null;
        }

        public string ProcessingId
        {
            get { return Msh[11].Value; }
            set { Msh[11].Value = value; }
        }

        public string ReceivingApplication
        {
            get { return Msh[5].Value; }
            set { Msh[5].Value = value; }
        }

        public string ReceivingFacility
        {
            get { return Msh[6].Value; }
            set { Msh[6].Value = value; }
        }

        public string Security
        {
            get { return Msh[8].Value; }
            set { Msh[8].Value = value; }
        }

        public IEnumerable<ISegment> Segments
        {
            get { return _message.Segments; }
        }

        public string SendingApplication
        {
            get { return Msh[3].Value; }
            set { Msh[3].Value = value; }
        }

        public string SendingFacility
        {
            get { return Msh[4].Value; }
            set { Msh[4].Value = value; }
        }

        public DateTimeOffset? Time
        {
            get { return Codec.ConvertToDateTime(Msh[7].Value); }
            set { Msh[7].Value = Codec.ConvertFromDateTime(value); }
        }

        public string TriggerEvent
        {
            get { return Msh[9][0][2].Value; }
            set { Msh[9][0][2].Value = value; }
        }

        public string Type
        {
            get { return Msh[9][0][1].Value; }
            set { Msh[9][0][1].Value = value; }
        }

        public bool Validate()
        {
            if (!Value.StartsWith("MSH"))
            {
                return false;
            }
            return true;
        }

        public string Value
        {
            get
            {
                return _message.Value;
            }
            set
            {
                _message.Value = value;
            }
        }

        public string[] Values
        {
            get
            {
                return _message.Values;
            }
            set
            {
                _message.Values = value;
            }
        }

        public string Version
        {
            get
            {
                return Msh[12].Value;
            }
            set
            {
                Msh[12].Value = value;
            }
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            return _message.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _message.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Codecs;
using NextLevelSeven.Core;

#pragma warning disable 0067

namespace NextLevelSeven.Test
{
    /// <summary>
    ///     A null message that contains nothing. This exists solely to implement IMessage without extra plumbing.
    /// </summary>
    sealed public class NullMessage : IMessage
    {
        /// <summary>
        ///     Get the singleton instance of NullMessage.
        /// </summary>
        public static readonly NullMessage Instance = new NullMessage();

        public ISegment this[int index]
        {
            get { return null; }
        }

        public IEnumerable<ISegment> this[string segmentType]
        {
            get { return Enumerable.Empty<ISegment>(); }
        }

        public IEnumerable<ISegment> this[IEnumerable<string> segmentTypes]
        {
            get { return Enumerable.Empty<ISegment>(); }
        }

        public string ControlId
        {
            get { return null; }
            set { }
        }

        public string Escape(string data)
        {
            return data;
        }

        public string ProcessingId
        {
            get { return null; }
            set { }
        }

        public IIdentity Receiver
        {
            get { return null; }
        }

        public string Security
        {
            get { return null; }
            set { }
        }

        public IEnumerable<ISegment> Segments
        {
            get { return Enumerable.Empty<ISegment>(); }
        }

        public IIdentity Sender
        {
            get { return null; }
        }

        public DateTimeOffset? Time
        {
            get { return null; }
            set { }
        }

        public string TriggerEvent
        {
            get { return null; }
            set { }
        }

        public string Type
        {
            get { return null; }
            set { }
        }

        public string UnEscape(string data)
        {
            return data;
        }

        public string Version
        {
            get { return null; }
        }

        public IMessage Clone()
        {
            return new NullMessage();
        }

        public IElement GetField(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return null;
        }

        public IElement GetField(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return null;
        }

        public bool Validate()
        {
            return true;
        }

        IElement IElement.this[int index]
        {
            get { return null; }
        }

        public IElement AncestorElement
        {
            get { return null; }
        }

        public ICodec As
        {
            get { return null; }
        }

        public char Delimiter
        {
            get { return '\0'; }
        }

        public int DescendantCount
        {
            get { return 0; }
        }

        public IEnumerable<IElement> DescendantElements
        {
            get { return Enumerable.Empty<IElement>(); }
        }

        public bool Exists
        {
            get { return false; }
        }

        public bool HasSignificantDescendants
        {
            get { return false; }
        }

        public int Index
        {
            get { return 0; }
        }

        public string Key
        {
            get { return null; }
        }

        public IMessage Message
        {
            get { return this; }
        }

        public string Value
        {
            get { return null; }
            set { }
        }

        public string[] Values
        {
            get { return new string[0]; }
            set { }
        }

        public event EventHandler ValueChanged;

        public IElement CloneDetached()
        {
            return new NullMessage();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Erase()
        {
        }

        public void Nullify()
        {
        }

        public string GetValue(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return null;
        }

        public string GetValue(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return null;
        }

        public IEnumerable<IElement> GetFields(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Enumerable.Empty<IElement>();
        }

        public IEnumerable<string> GetValues(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Enumerable.Empty<string>();
        }
    }
}

#pragma warning restore 0067

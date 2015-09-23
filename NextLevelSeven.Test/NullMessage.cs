using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Properties;
using NextLevelSeven.Parsing;

#pragma warning disable 0067

namespace NextLevelSeven.Test
{
    /// <summary>
    ///     A null message that contains nothing. This exists solely to implement IMessage without extra plumbing.
    /// </summary>
    public sealed class NullMessage : IMessageParser
    {
        /// <summary>
        ///     Get the singleton instance of NullMessage.
        /// </summary>
        public static readonly NullMessage Instance = new NullMessage();

        public int DescendantCount
        {
            get { return 0; }
        }

        public string ControlId
        {
            get { return null; }
            set { Debug.WriteLine("Write to ControlId: {0}", value); }
        }

        public ISegmentParser this[int index]
        {
            get { return null; }
        }

        public IEnumerable<ISegmentParser> this[string segmentType]
        {
            get { return Enumerable.Empty<ISegmentParser>(); }
        }

        public IEnumerable<ISegmentParser> this[IEnumerable<string> segmentTypes]
        {
            get { return Enumerable.Empty<ISegmentParser>(); }
        }

        public string Escape(string data)
        {
            return data;
        }

        public IEnumerable<ISegmentParser> Segments
        {
            get { return Enumerable.Empty<ISegmentParser>(); }
        }

        public string UnEscape(string data)
        {
            return data;
        }

        public IElementParser GetElement(int segment, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return null;
        }

        public bool Validate()
        {
            return true;
        }

        IElementParser IElementParser.this[int index]
        {
            get { return null; }
        }

        public IElementParser AncestorElement
        {
            get { return null; }
        }

        public IEncodedTypeConverter As
        {
            get { return null; }
        }

        public char Delimiter
        {
            get { return '\0'; }
        }

        public IEnumerable<IElementParser> DescendantElements
        {
            get { return Enumerable.Empty<IElementParser>(); }
        }

        public bool Exists
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

        public IMessageParser Message
        {
            get { return this; }
        }

        public string Value
        {
            get { return null; }
            set { }
        }

        public IEnumerable<string> Values
        {
            get { return new string[0]; }
            set { }
        }

        public event EventHandler ValueChanged;

        public IEnumerable<string> GetValues(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return Enumerable.Empty<string>();
        }

        public string GetValue(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return null;
        }

        IElement IElement.Clone()
        {
            return CloneInternal();
        }

        IMessage IMessage.Clone()
        {
            return CloneInternal();
        }

        IElement IElement.this[int index]
        {
            get { return null; }
        }


        public int ValueCount
        {
            get { return 0; }
        }


        public IMessageDetails Details
        {
            get { return null; }
        }


        public string FormattedValue
        {
            get { return null; }
            set { }
        }

        IElement IElement.Ancestor
        {
            get { return null; }
        }

        IEnumerable<IElement> IElement.Descendants
        {
            get { return Enumerable.Empty<IElement>(); }
        }

        IEnumerable<ISegment> IMessage.Segments
        {
            get { return Enumerable.Empty<ISegment>(); }
        }

        public void Delete()
        {
        }

        public void Erase()
        {
        }

        public void Nullify()
        {
        }

        public IMessageParser Clone()
        {
            return CloneInternal();
        }

        public IElementParser GetField(string segmentName, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return null;
        }

        private static NullMessage CloneInternal()
        {
            var result = new NullMessage();
            return result;
        }
    }
}

#pragma warning restore 0067
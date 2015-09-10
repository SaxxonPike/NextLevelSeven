using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Specification;
using NextLevelSeven.Native;

#pragma warning disable 0067

namespace NextLevelSeven.Test
{
    /// <summary>
    ///     A null message that contains nothing. This exists solely to implement IMessage without extra plumbing.
    /// </summary>
    public sealed class NullMessage : INativeMessage
    {
        /// <summary>
        ///     Get the singleton instance of NullMessage.
        /// </summary>
        public static readonly NullMessage Instance = new NullMessage();

        public INativeSegment this[int index]
        {
            get { return null; }
        }

        public IEnumerable<INativeSegment> this[string segmentType]
        {
            get { return Enumerable.Empty<INativeSegment>(); }
        }

        public IEnumerable<INativeSegment> this[IEnumerable<string> segmentTypes]
        {
            get { return Enumerable.Empty<INativeSegment>(); }
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

        public IEnumerable<INativeSegment> Segments
        {
            get { return Enumerable.Empty<INativeSegment>(); }
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

        public INativeMessage Clone()
        {
            return new NullMessage();
        }

        public INativeElement GetField(int segment, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return null;
        }

        public bool Validate()
        {
            return true;
        }

        INativeElement INativeElement.this[int index]
        {
            get { return null; }
        }

        public INativeElement AncestorElement
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

        public IEnumerable<INativeElement> DescendantElements
        {
            get { return Enumerable.Empty<INativeElement>(); }
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

        public INativeMessage Message
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

        public INativeElement CloneDetached()
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

        public INativeElement GetField(string segmentName, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return null;
        }
    }
}

#pragma warning restore 0067
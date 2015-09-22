using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    /// <summary>
    ///     A comparer for multiple parsers.
    /// </summary>
    public class ParserComparer
    {
        /// <summary>
        ///     Parsers to compare.
        /// </summary>
        private readonly List<IMessage> _parsers = new List<IMessage>();

        /// <summary>
        ///     Create a parser comparer with the specified parsers. Minimum two parsers required.
        /// </summary>
        public ParserComparer(IMessage parserA, IMessage parserB, params IMessage[] additionalParsers)
        {
            _parsers.Add(parserA);
            _parsers.Add(parserB);
            _parsers.AddRange(additionalParsers);
        }

        /// <summary>
        ///     Assert that a message parses identically on all parsers.
        /// </summary>
        /// <param name="message">Message to compare.</param>
        public void AssertParseEquivalent(string message)
        {
            // preload parsers.
            foreach (var parser in _parsers)
            {
                parser.Value = message;
            }

            // begin parsing at the top level.
            AssertParseEquivalent(_parsers.Select(p => (IElement) p).ToArray());
        }

        /// <summary>
        ///     Assert that all elements parse identically.
        /// </summary>
        /// <param name="elements"></param>
        private void AssertParseEquivalent(params IElement[] elements)
        {
            if (elements.Count() <= 1)
            {
                return;
            }

            // verify they have the same string representation.
            AssertEqual("String representations differ.", elements.Select(p => p.Value).ToArray());

            // verify they all have the same value counts
            AssertEqual("Value counts differ.", _parsers.Select(p => p.ValueCount).ToArray());

            // verify array contents are identical
            ArrayComparer.AssertCompare(_parsers.Select(p => p.Values.ToList()).ToArray());

            // verify descendant counts are identical
            AssertEqual("Enumerable descendant counts differ.", _parsers.Select(p => p.Descendants.Count()).ToArray());

            // verify descendants are also individually equivalent
            var referenceAncestor = elements.First();
            if (referenceAncestor is ISubcomponent) return;

            var referenceDescendants = referenceAncestor.Descendants.ToList();
            for (var i = 0; i < referenceDescendants.Count; i++)
            {
                try
                {
                    AssertParseEquivalent(elements.Skip(1).Select(e => e.Descendants.Skip(i).Take(1).First()).ToArray());
                }
                catch (Exception)
                {
                    Debug.WriteLine("Failed at a descendant of {0} ({1}).", GetElementLevel(referenceAncestor),
                        GetElementLocation(referenceAncestor));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Get a string representation of the HL7 element level.
        /// </summary>
        private static string GetElementLevel(IElement element)
        {
            if (element is IMessage)
            {
                return "Message";
            }
            if (element is ISegment)
            {
                return "Segment";
            }
            if (element is IField)
            {
                return "Field";
            }
            if (element is IRepetition)
            {
                return "Repetition";
            }
            if (element is IComponent)
            {
                return "Component";
            }
            if (element is ISubcomponent)
            {
                return "Subcomponent";
            }
            return "Element";
        }

        /// <summary>
        ///     Get a string representation of the HL7 element location.
        /// </summary>
        private static string GetElementLocation(IElement element)
        {
            var result = new StringBuilder();
            var cursor = element;
            while (cursor.Ancestor != null)
            {
                if (result.Length > 0)
                {
                    result.Insert(0, ".");
                }
                result.Insert(0, cursor.Index);
                cursor = cursor.Ancestor;
            }
            return result.ToString();
        }

        /// <summary>
        ///     Assert values are equal and print detailed report.
        /// </summary>
        private static void AssertEqual<T>(string errorMessage = null, params T[] values)
        {
            Assert.AreEqual(1, values.Distinct().Count(), string.Format(
                "{2}{1}{1}{0}", string.Join(Environment.NewLine, values.Select((p, i) => string.Format(
                    "{0}:{2}{1}{2}", i, p.ToString().Replace("\n", "\\n").Replace("\r", "\\r"), Environment.NewLine))),
                Environment.NewLine,
                errorMessage ?? "Values differ."));
        }

        /// <summary>
        ///     Assert that a message parses identically on all parsers.
        /// </summary>
        /// <param name="message">Message to compare.</param>
        public void AssertParseEquivalent(IMessage message)
        {
            AssertParseEquivalent(message.Value);
        }
    }
}
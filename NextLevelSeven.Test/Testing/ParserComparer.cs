using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Testing
{
    /// <summary>A comparer for multiple parsers.</summary>
    public class ParserComparer
    {
        /// <summary>Parsers to compare.</summary>
        private readonly List<IMessage> _parsers = new List<IMessage>();

        /// <summary>Create a parser comparer with the specified parsers. Minimum two parsers required.</summary>
        public ParserComparer(IMessage parserA, IMessage parserB, params IMessage[] additionalParsers)
        {
            _parsers.Add(parserA);
            _parsers.Add(parserB);
            _parsers.AddRange(additionalParsers);
        }

        /// <summary>Assert that a message parses identically on all parsers.</summary>
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

        /// <summary>Assert that all elements parse identically.</summary>
        /// <param name="elements"></param>
        private void AssertParseEquivalent(params IElement[] elements)
        {
            if (elements.Length <= 1)
            {
                return;
            }

            // verify contents are identical
            foreach (var parser in _parsers.Skip(1))
            {
                parser.Values.ShouldAllBeEquivalentTo(_parsers.First().Values);
                parser.Value.Should().Be(_parsers.First().Value);
            }

            // verify descendants are also individually equivalent
            var referenceAncestor = elements.First();
            if (referenceAncestor is ISubcomponent) return;

            var referenceDescendants = referenceAncestor.Descendants.ToList();
            for (var i = 0; i < referenceDescendants.Count; i++)
            {
                AssertParseEquivalent(elements.Skip(1).Select(e => e.Descendants.Skip(i).Take(1).First()).ToArray());
            }
        }
    }
}
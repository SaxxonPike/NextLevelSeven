using System.Globalization;
using System.Linq;

namespace NextLevelSeven.Core
{
    /// <summary>A collection of useful internal operations that are common to elements of all types.</summary>
    internal static class ElementOperations
    {
        /// <summary>Get a key unique to this element in the tree.</summary>
        public static string GetKey(IElement target)
        {
            // root element?
            var ancestor = target.Ancestor;
            if (ancestor == null)
            {
                return ElementDefaults.RootElementKey;
            }

            // anything more precise than segment?
            var segment = target as ISegment;
            if (segment == null)
            {
                return string.Concat(GetKey(ancestor), ".", target.Index.ToString(CultureInfo.InvariantCulture));
            }

            // segment? (these are named like so: MSH1, OBR3, etc)
            var message = (IMessage) ancestor;
            var type = segment.Type;
            var index =
                message.Segments.Where(s => s.Type == type).Select(s => s.Index).ToList().IndexOf(segment.Index) + 1;
            return string.Concat(type, index.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>Determine if the element is a field that contains encoding characters of some kind. (MSH-1, MSH-2)</summary>
        /// <param name="element">Element to check.</param>
        /// <returns>True, if the element contains encoding characters of some kind.</returns>
        public static bool IsEncodingCharacterField(IElement element)
        {
            return element is IField && element.Ancestor != null && ((IField) element).Ancestor.Type == "MSH" &&
                   element.Index >= 1 && element.Index <= 2;
        }
    }
}
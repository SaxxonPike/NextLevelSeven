using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NextLevelSeven.Building;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Extensions to the IElement interface.
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        ///     Add a string as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">String to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this IElement target, string elementToAdd)
        {
            target.Value = String.Join(new string(target.Delimiter, 1), target.Value, elementToAdd);
        }

        /// <summary>
        ///     Add an element as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this IElement target, IElement elementToAdd)
        {
            target.Value = String.Join(new string(target.Delimiter, 1), target.Value ?? string.Empty, elementToAdd.Value ?? string.Empty);
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<string> elementsToAdd)
        {
            target.Value = String.Join(new string(target.Delimiter, 1), (new[] {target.Value}).Concat(elementsToAdd));
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<IElement> elementsToAdd)
        {
            target.Value = String.Join(new string(target.Delimiter, 1),
                (new[] {target.Value}).Concat(elementsToAdd.Select(e => e.Value ?? string.Empty)));
        }

        /// <summary>
        ///     Delete an element from its ancestor. Throws an exception if the element is a root element.
        /// </summary>
        /// <param name="target">Element to delete.</param>
        public static void Delete(this IElement target)
        {
            if (target.Ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }
            target.Ancestor.Delete(target.Index);
        }

        /// <summary>
        ///     Delete a descendant element.
        /// </summary>
        /// <param name="target">Element to delete from.</param>
        /// <param name="index">Index of descendant to delete.</param>
        public static void Delete(this IElement target, int index)
        {
            if (index >= 1 && index <= target.ValueCount)
            {
                var values = new List<string>();
                var indexMap = (target is ISegment) ? index : index - 1;
                values.AddRange(target.Descendants.Where((d, i) => i != indexMap).Select(d => d.Value));
                target.Values = values;
            }
        }

        /// <summary>
        ///     Delete elements in the enumerable. All elements must share a direct ancestor.
        /// </summary>
        /// <param name="targets">Elements to delete.</param>
        public static void Delete(this IEnumerable<IElement> targets)
        {
            var elements = targets.ToList();
            if (elements.Count <= 0)
            {
                return;
            }

            var ancestor = elements.First().Ancestor;
            if (elements.Any(e => e == null || !ReferenceEquals(e.Ancestor, ancestor)))
            {
                throw new ElementException(ErrorCode.ElementsMustShareDirectAncestors);
            }

            // delete them in reverse order so that the parent isn't removed out
            // from under the values we're deleting.
            foreach (var element in elements.Select(e => e.Index).Distinct().OrderByDescending(i => i).ToList())
            {
                ancestor.Delete(element);
            }
        }

        /// <summary>
        ///     Get segments that do not match a specific segment type.
        /// </summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentType">Segment type to filter out.</param>
        /// <returns>Segments that do not match the filtered segment type.</returns>
        public static IEnumerable<ISegment> ExceptType(this IEnumerable<ISegment> segments, string segmentType)
        {
            return segments.Where(s => s.Type != segmentType);
        }

        /// <summary>
        ///     Get segments that do not match any of the specified segment types.
        /// </summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentTypes">Segment types to filter out.</param>
        /// <returns>Segments that do not match the filtered segment types.</returns>
        public static IEnumerable<ISegment> ExceptTypes(this IEnumerable<ISegment> segments,
            IEnumerable<string> segmentTypes)
        {
            return segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>
        ///     Get meaningful content from descendants, excluding null/empty fields and segment type.
        /// </summary>
        public static IEnumerable<IElement> GetDescendantContent(this IElement element)
        {
            return element.Descendants.Where(d => d.Index > 0 && !string.IsNullOrEmpty(d.Value));
        }

        /// <summary>
        ///     If true, the element has meaningful descendants (not necessarily direct ones.)
        /// </summary>
        public static bool HasSignificantDescendants(this IElement element)
        {
            if (element is ISubcomponent)
            {
                return false;
            }

            if (element is IField && element.Ancestor != null)
            {
                var segment = (element as IField).Ancestor as ISegment;
                if (segment != null && segment.Type == "MSH" && element.Index >= 1 && element.Index <= 2)
                {
                    return false;
                }
            }

            return (element.ValueCount > 1) || element.Descendants.Any(HasSignificantDescendants);
        }

        /// <summary>
        ///     Insert element data after the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data after.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertAfter(this IElement target, int index, IElement elementToInsert)
        {
            target[index].InsertAfter(elementToInsert);
        }

        /// <summary>
        ///     Insert element data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertAfter(this IElement target, IElement elementToInsert)
        {
            if (target.Ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index;
            var ancestor = target.Ancestor;
            var strings = ancestor.Values.ToList();
            while (strings.Count < index)
            {
                strings.Add(null);
            }
            strings.Insert(index, elementToInsert.Value);
            ancestor.Values = strings.ToArray();
        }

        /// <summary>
        ///     Insert string data after the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        public static void InsertAfter(this IElement target, int index, string dataToInsert)
        {
            target[index].InsertAfter(dataToInsert);
        }

        /// <summary>
        ///     Insert string data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        public static void InsertAfter(this IElement target, string dataToInsert)
        {
            if (target.Ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index;
            var ancestor = target.Ancestor;
            var strings = ancestor.Values.ToList();
            while (strings.Count < index)
            {
                strings.Add(null);
            }
            strings.Insert(index, dataToInsert);
            ancestor.Values = strings.ToArray();
        }

        /// <summary>
        ///     Insert element data before the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, int index, IElement elementToInsert)
        {
            target[index].InsertBefore(elementToInsert);
        }

        /// <summary>
        ///     Insert element data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, IElement elementToInsert)
        {
            if (target.Ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index - 1;
            var ancestor = target.Ancestor;
            var strings = ancestor.Values.ToList();
            while (strings.Count < index)
            {
                strings.Add(null);
            }
            strings.Insert(index, elementToInsert.Value);
            ancestor.Values = strings.ToArray();
        }

        /// <summary>
        ///     Insert string data before the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, int index, string dataToInsert)
        {
            target[index].InsertBefore(dataToInsert);
        }

        /// <summary>
        ///     Insert string data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, string dataToInsert)
        {
            if (target.Ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index - 1;
            var ancestor = target.Ancestor;
            var strings = ancestor.Values.ToList();
            while (strings.Count < index)
            {
                strings.Add(null);
            }
            strings.Insert(index, dataToInsert);
            ancestor.Values = strings.ToArray();
        }

        /// <summary>
        ///     Move element within its ancestor. Returns the new element reference.
        /// </summary>
        /// <param name="target">Element to move.</param>
        /// <param name="targetIndex">Target index.</param>
        /// <returns>Element in its new place.</returns>
        public static IElement MoveToIndex(this IElement target, int targetIndex)
        {
            var ancestor = target.Ancestor;
            if (ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            if (targetIndex < 0)
            {
                throw new ElementException(ErrorCode.ElementIndexMustBeZeroOrGreater);
            }

            if (targetIndex == target.Index)
            {
                return target;
            }

            if (ancestor is ISegment)
            {
                if (target.Index < 1)
                {
                    throw new ElementException(ErrorCode.SegmentTypeCannotBeMoved);
                }

                if (target.Index <= 2 && (ancestor as ISegment).Type == "MSH")
                {
                    throw new ElementException(ErrorCode.EncodingElementCannotBeMoved);
                }
            }

            var values = new List<string>(ancestor.Values);
            var index = targetIndex - 1;
            var replacedValue = (target.Index > values.Count) ? target.Value : values[target.Index - 1];

            while (values.Count < targetIndex)
            {
                values.Add(null);
            }

            values.RemoveAt(target.Index - 1);
            values.Insert(index, replacedValue);
            ancestor.Values = values.ToArray();
            return ancestor[targetIndex];
        }

        /// <summary>
        ///     Set the value of a field to null.
        /// </summary>
        /// <param name="target"></param>
        public static void Nullify(this IElement target)
        {
            target.Value = null;
        }

        /// <summary>
        ///     Get only segments that match the specified segment type.
        /// </summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentType">Segment type to get.</param>
        /// <returns>Segments that match the specified segment type.</returns>
        public static IEnumerable<ISegment> OfType(this IEnumerable<ISegment> segments, string segmentType)
        {
            return segments.Where(s => s.Type == segmentType);
        }

        /// <summary>
        ///     Get only segments that match any of the specified segment types.
        /// </summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentTypes">Segment types to get.</param>
        /// <returns>Segments that match one of the specified segment types.</returns>
        public static IEnumerable<ISegment> OfTypes(this IEnumerable<ISegment> segments,
            IEnumerable<string> segmentTypes)
        {
            return segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>
        ///     Copy the contents of this message to a new message builder.
        /// </summary>
        /// <param name="message">Message to get data from.</param>
        /// <returns>Converted message.</returns>
        public static IMessageBuilder ToBuilder(this IMessage message)
        {
            return Message.Build(message.Value);
        }

        /// <summary>
        ///     Copy the contents of this message to a new HL7 message parser.
        /// </summary>
        /// <param name="message">Message to get data from.</param>
        /// <returns>Converted message.</returns>
        public static IMessageParser ToParser(this IMessage message)
        {
            return Message.Parse(message.Value);
        }

        /// <summary>
        ///     Get the segment at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static ISegment Segment(this IMessage ancestor, int index)
        {
            return (ISegment)ancestor[index];
        }

        /// <summary>
        ///     Get the field at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IField Field(this ISegment ancestor, int index)
        {
            return (IField)ancestor[index];
        }

        /// <summary>
        ///     Get the field repetition at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IRepetition Repetition(this IField ancestor, int index)
        {
            return (IRepetition)ancestor[index];
        }

        /// <summary>
        ///     Get the component at the specified index, assuming repetition 1.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IComponent Component(this IField ancestor, int index)
        {
            return (IComponent)ancestor[1][index];
        }

        /// <summary>
        ///     Get the component at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IComponent Component(this IRepetition ancestor, int index)
        {
            return (IComponent)ancestor[index];
        }

        /// <summary>
        ///     Get the subcomponent at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static ISubcomponent Subcomponent(this IComponent ancestor, int index)
        {
            return (ISubcomponent)ancestor[index];
        }
    }
}
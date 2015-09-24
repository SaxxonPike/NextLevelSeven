using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NextLevelSeven.Building;
using NextLevelSeven.Building.Elements;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing;
using NextLevelSeven.Parsing.Elements;

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
            target[target.NextIndex].Value = elementToAdd;
        }

        /// <summary>
        ///     Add an element as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this IElement target, IElement elementToAdd)
        {
            CopyOver(elementToAdd, target[target.NextIndex]);
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, params string[] elementsToAdd)
        {
            AddRange(target, elementsToAdd.AsEnumerable());
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<string> elementsToAdd)
        {
            foreach (var element in elementsToAdd)
            {
                target.Add(element);
            }
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<IElement> elementsToAdd)
        {
            foreach (var element in elementsToAdd)
            {
                target.Add(element);
            }
        }

        /// <summary>
        ///     Copy elements and sub-elements one by one into the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private static void CopyOver(IElement source, IElement target)
        {
            if (!(target is ISubcomponent) && source.HasSignificantDescendants())
            {
                foreach (var descendant in source.Descendants.ToList())
                {
                    CopyOver(descendant, target[descendant.Index]);
                }
            }
            else
            {
                target.Value = source.Value;
            }
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
        /// <param name="indices">Indices of descendants to delete.</param>
        public static void Delete(this IElement target, params int[] indices)
        {
            // convert to list first so we don't squash our input values.
            target.Values = target.Descendants.Where((d, i) => !indices.Contains(d.Index)).Select(d => d.Value).ToList();
        }

        /// <summary>
        ///     Delete elements in the enumerable. All elements must share a direct ancestor.
        /// </summary>
        /// <param name="targets">Elements to delete.</param>
        public static void Delete(this IEnumerable<IElement> targets)
        {
            // cache targets
            var elements = targets.ToList();
            if (elements.Count <= 0)
            {
                return;
            }

            // determine if we have a single common direct ancestor
            if (elements.Select(t => t.Ancestor).Distinct().Count() > 1)
            {
                throw new ElementException(ErrorCode.ElementsMustShareDirectAncestors);
            }
            
            // perform deletion
            elements.First().Ancestor.Delete(elements.Select(e => e.Index).ToArray());
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

            // check for field delimiter and encoding character fields
            if (ElementOperations.HasEncodingCharacters(element))
            {
                return false;
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
            InsertBefore(target, index + 1, elementToInsert);
        }

        /// <summary>
        ///     Insert element data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertAfter(this IElement target, IElement elementToInsert)
        {
            target.Ancestor.InsertBefore(target.Index + 1, elementToInsert);
        }

        /// <summary>
        ///     Insert string data after the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        public static void InsertAfter(this IElement target, int index, string dataToInsert)
        {
            InsertBefore(target, index + 1, dataToInsert);
        }

        /// <summary>
        ///     Insert string data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        public static void InsertAfter(this IElement target, string dataToInsert)
        {
            target.Ancestor.InsertBefore(target.Index + 1, dataToInsert);
        }

        /// <summary>
        ///     Insert element data before the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, int index, IElement elementToInsert)
        {
            InsertEmpty(target, index);
            CopyOver(elementToInsert, target[index]);
        }

        /// <summary>
        ///     Insert element data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, IElement elementToInsert)
        {
            target.Ancestor.InsertBefore(target.Index, elementToInsert);
        }

        /// <summary>
        ///     Insert string data before the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, int index, string dataToInsert)
        {
            InsertEmpty(target, index);
            target[index].Value = dataToInsert;
        }

        /// <summary>
        ///     Insert string data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this IElement target, string dataToInsert)
        {
            target.Ancestor.InsertBefore(target.Index, dataToInsert);
        }

        /// <summary>
        ///     Move elements forward one index, starting at the specified index.
        /// </summary>
        /// <param name="target">Element to modify.</param>
        /// <param name="index">Index to begin at.</param>
        private static void InsertEmpty(IElement target, int index)
        {
            var values = target.Descendants.Where(e => e.Index >= index).ToDictionary(e => e.Index, e => e.Value);
            foreach (var v in values.OrderByDescending(kv => kv.Key).ToList())
            {
                target[v.Key + 1].Nullify();
                CopyOver(target[v.Key], target[v.Key + 1]);
                if (!values.ContainsKey(v.Key - 1))
                {
                    target[v.Key].Nullify();
                }
            }            
        }

        /// <summary>
        ///     Determine if the value is either not present or the HL7 standard null value.
        /// </summary>
        /// <param name="target">Element to verify value for.</param>
        /// <returns>True, if the element's value is null equivalent.</returns>
        public static bool IsNull(this IElement target)
        {
            return HL7.NullValues.Contains(target.Value);
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
        ///     Set the value of a field to existant null.
        /// </summary>
        /// <param name="target">Element to nullify.</param>
        public static void Nullify(this IElement target)
        {
            // don't change what's already null.
            if (target == null || target.Value == null)
            {
                return;
            }

            // messages can't be nullified.
            if (target is IMessage)
            {
                throw new ElementException(ErrorCode.MessageDataMustNotBeNull);
            }

            // segment nullability doesn't work well, so we just clear out all fields.
            var segment = target as ISegment;
            if (segment != null)
            {
                segment.Value = string.Concat(segment.Type, segment.Delimiter);
                return;
            }

            target.Value = HL7.Null;
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
            return (ISegment) ancestor[index];
        }

        /// <summary>
        ///     Get the field at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IField Field(this ISegment ancestor, int index)
        {
            return (IField) ancestor[index];
        }

        /// <summary>
        ///     Get the field repetition at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IRepetition Repetition(this IField ancestor, int index)
        {
            return (IRepetition) ancestor[index];
        }

        /// <summary>
        ///     Get the component at the specified index, assuming repetition 1.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IComponent Component(this IField ancestor, int index)
        {
            return (IComponent) ancestor[1][index];
        }

        /// <summary>
        ///     Get the component at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IComponent Component(this IRepetition ancestor, int index)
        {
            return (IComponent) ancestor[index];
        }

        /// <summary>
        ///     Get the subcomponent at the specified index.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static ISubcomponent Subcomponent(this IComponent ancestor, int index)
        {
            return (ISubcomponent) ancestor[index];
        }

        /// <summary>
        ///     Create a new message.
        /// </summary>
        /// <typeparam name="T">Any IMessage type.</typeparam>
        /// <returns>New message builder/parser.</returns>
        private static T ToNewMessage<T>(IEnumerable<ISegment> segments) where T : IMessage, new()
        {
            // build empty message when there are no segments
            var sourceSegments = segments.ToList();
            if (sourceSegments.Count == 0)
            {
                return new T();
            }

            // for message metadata
            var newMessage = new T();

            // determine where to pull message metadata from
            var childSegment = sourceSegments.FirstOrDefault(s => s.Type == "MSH")
                               ??
                               sourceSegments.Where(s => s.Ancestor is IMessage)
                                   .Select(s => (ISegment) (s.Ancestor[1]))
                                   .FirstOrDefault();
            if (childSegment != null)
            {
                CopyOver(childSegment, newMessage[1]);
            }

            // build message
            foreach (var segment in sourceSegments)
            {
                newMessage.Add(segment);
            }

            return newMessage;
        }

        /// <summary>
        ///     Create a new message builder with the selected segments.
        /// </summary>
        /// <param name="segments">Segments to make a new message for.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder ToNewBuilder(this IEnumerable<ISegment> segments)
        {
            return ToNewMessage<MessageBuilder>(segments);
        }

        /// <summary>
        ///     Create a new message parser with the selected segments.
        /// </summary>
        /// <param name="segments">Segments to make a new message for.</param>
        /// <returns>New message parser.</returns>
        public static IMessageParser ToNewParser(this IEnumerable<ISegment> segments)
        {
            return ToNewMessage<MessageParser>(segments);
        }
    }
}
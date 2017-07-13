using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Building;
using NextLevelSeven.Building.Elements;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing;
using NextLevelSeven.Parsing.Elements;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Core
{
    /// <summary>Extensions to the IElement interface.</summary>
    public static class ElementExtensions
    {
        /// <summary>Add a string as a descendant.</summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">String to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this IElement target, string elementToAdd)
        {
            target[target.NextIndex].Value = elementToAdd;
        }

        /// <summary>Add an element as a descendant.</summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this IElement target, IElement elementToAdd)
        {
            CopyTo(elementToAdd, target[target.NextIndex]);
        }

        /// <summary>Add elements as descendants.</summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, params string[] elementsToAdd)
        {
            AddRange(target, elementsToAdd.AsEnumerable());
        }

        /// <summary>Add elements as descendants.</summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<string> elementsToAdd)
        {
            foreach (var element in elementsToAdd)
            {
                target.Add(element);
            }
        }

        /// <summary>Add elements as descendants.</summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<IElement> elementsToAdd)
        {
            foreach (var element in elementsToAdd)
            {
                target.Add(element);
            }
        }

        /// <summary>Copy elements and sub-elements one by one into the target.</summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyTo(this IElement source, IElement target)
        {
            var segment = target as ISegment;
            if (segment != null && segment.Type == "MSH")
            {
                target.Value = string.Join(source[1].Value,
                    source.Descendants.Where(d => d.Index != 1).Select(d => d.Value));
                return;
            }

            if (!(target is ISubcomponent) && source.HasSignificantDescendants())
            {
                foreach (var descendant in source.Descendants.ToList())
                {
                    CopyTo(descendant, target[descendant.Index]);
                }
            }
            else
            {
                target.Value = source.Value;
            }
        }

        /// <summary>Delete an element from its ancestor. Throws an exception if the element is a root element.</summary>
        /// <param name="target">Element to delete.</param>
        public static void Delete(this IElement target)
        {
            var ancestor = target.Ancestor;
            if (ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }
            ancestor.Delete(target.Index);
        }

        /// <summary>Delete descendant elements.</summary>
        /// <param name="target">Element to delete from.</param>
        /// <param name="indices">Indices of descendants to delete.</param>
        public static void Delete(this IElement target, params int[] indices)
        {
            foreach (var index in indices.OrderByDescending(i => i))
            {
                target.Delete(index);
            }
        }

        /// <summary>Delete elements in the enumerable. All elements must share a direct ancestor.</summary>
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
            var firstAncestor = elements.First().Ancestor;
            if (elements.Skip(1).Any(t => !ReferenceEquals(t.Ancestor, firstAncestor)))
            {
                throw new ElementException(ErrorCode.ElementsMustShareDirectAncestors);
            }

            // perform deletion
            elements.First().Ancestor.Delete(elements.Select(e => e.Index).ToArray());
        }

        /// <summary>Get segments that do not match a specific segment type.</summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentType">Segment type to filter out.</param>
        /// <returns>Segments that do not match the filtered segment type.</returns>
        public static IEnumerable<ISegment> ExceptType(this IEnumerable<ISegment> segments, string segmentType)
        {
            return segments.Where(s => s.Type != segmentType);
        }

        /// <summary>Get segments that do not match any of the specified segment types.</summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentTypes">Segment types to filter out.</param>
        /// <returns>Segments that do not match the filtered segment types.</returns>
        public static IEnumerable<ISegment> ExceptTypes(this IEnumerable<ISegment> segments,
            IEnumerable<string> segmentTypes)
        {
            return segments.Where(s => !segmentTypes.Contains(s.Type));
        }

        /// <summary>Get segments that do not match any of the specified segment types.</summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentTypes">Segment types to filter out.</param>
        /// <returns>Segments that do not match the filtered segment types.</returns>
        public static IEnumerable<ISegment> ExceptTypes(this IEnumerable<ISegment> segments,
            params string[] segmentTypes)
        {
            return ExceptTypes(segments, segmentTypes.AsEnumerable());
        }

        /// <summary>Get meaningful content from descendants, excluding null/empty fields and segment type.</summary>
        public static IEnumerable<IElement> Simplified(this IElement element)
        {
            return element.Descendants.Where(d => d.Index > 0 && !string.IsNullOrEmpty(d.Value));
        }

        /// <summary>If true, the element has meaningful descendants (not necessarily direct ones.)</summary>
        public static bool HasSignificantDescendants(this IElement element)
        {
            if (element is ISubcomponent)
            {
                return false;
            }

            // check for field delimiter and encoding character fields
            if (ElementOperations.IsEncodingCharacterField(element))
            {
                return false;
            }

            return element.ValueCount > 1 || element.Descendants.Any(HasSignificantDescendants);
        }

        /// <summary>Insert element data before the specified element.</summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void Insert(this IElement target, IElement elementToInsert)
        {
            var ancestor = target.Ancestor;
            if (ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }
            ancestor.Insert(target.Index, elementToInsert);
        }

        /// <summary>Insert string data before the specified element.</summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        public static void Insert(this IElement target, string dataToInsert)
        {
            var ancestor = target.Ancestor;
            if (ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }
            ancestor.Insert(target.Index, dataToInsert);
        }

        /// <summary>Move element within its ancestor. Returns the new element reference.</summary>
        /// <param name="target">Element to move.</param>
        /// <param name="targetIndex">Target index.</param>
        /// <returns>Element in its new place.</returns>
        public static IElement Move(this IElement target, int targetIndex)
        {
            if (target.Index == targetIndex)
            {
                return target;
            }

            var ancestor = target.Ancestor;
            if (ancestor == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }
            ancestor.Move(target.Index, targetIndex);
            return ancestor[targetIndex];
        }

        /// <summary>Set the value of a field to existant null.</summary>
        /// <param name="target">Element to nullify.</param>
        public static void Nullify(this IElement target)
        {
            // messages can't be nullified.
            if (target is IMessage)
            {
                throw new ElementException(ErrorCode.MessageDataMustNotBeNull);
            }

            // segment nullability doesn't work well, so we just clear out all fields.
            var segment = target as ISegment;
            if (segment != null)
            {
                segment.Values = segment.Type == "MSH"
                    ? segment.Values.Take(3).ToList()
                    : segment.Type.Yield();
                return;
            }

            target.Value = HL7.Null;
        }

        /// <summary>Get only segments that match the specified segment type.</summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentType">Segment type to get.</param>
        /// <returns>Segments that match the specified segment type.</returns>
        public static IEnumerable<ISegment> OfType(this IEnumerable<ISegment> segments, string segmentType)
        {
            return segments.Where(s => s.Type == segmentType);
        }

        /// <summary>Get only segments that match any of the specified segment types.</summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentTypes">Segment types to get.</param>
        /// <returns>Segments that match one of the specified segment types.</returns>
        public static IEnumerable<ISegment> OfTypes(this IEnumerable<ISegment> segments,
            IEnumerable<string> segmentTypes)
        {
            return segments.Where(s => segmentTypes.Contains(s.Type));
        }

        /// <summary>Get only segments that match any of the specified segment types.</summary>
        /// <param name="segments">Segments to query.</param>
        /// <param name="segmentTypes">Segment types to get.</param>
        /// <returns>Segments that match one of the specified segment types.</returns>
        public static IEnumerable<ISegment> OfTypes(this IEnumerable<ISegment> segments,
            params string[] segmentTypes)
        {
            return OfTypes(segments, segmentTypes.AsEnumerable());
        }

        /// <summary>Copy the contents of this message to a new message builder.</summary>
        /// <param name="message">Message to get data from.</param>
        /// <returns>Converted message.</returns>
        public static IMessageBuilder ToBuilder(this IMessage message)
        {
            return Message.Build(message.Value);
        }

        /// <summary>Copy the contents of this message to a new HL7 message parser.</summary>
        /// <param name="message">Message to get data from.</param>
        /// <returns>Converted message.</returns>
        public static IMessageParser ToParser(this IMessage message)
        {
            return Message.Parse(message.Value);
        }

        /// <summary>Get the segment at the specified index.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static ISegment Segment(this IMessage ancestor, int index)
        {
            return ancestor[index] as ISegment;
        }

        /// <summary>Get the field at the specified index.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IField Field(this ISegment ancestor, int index)
        {
            return ancestor[index] as IField;
        }

        /// <summary>Get the field repetition at the specified index.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IRepetition Repetition(this IField ancestor, int index)
        {
            return ancestor[index] as IRepetition;
        }

        /// <summary>Get the component at the specified index, assuming repetition 1.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IComponent Component(this IField ancestor, int index)
        {
            return ancestor[1][index] as IComponent;
        }

        /// <summary>Get the component at the specified index.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static IComponent Component(this IRepetition ancestor, int index)
        {
            return ancestor[index] as IComponent;
        }

        /// <summary>Get the subcomponent at the specified index.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index of the descendant.</param>
        /// <returns>Descendant element.</returns>
        public static ISubcomponent Subcomponent(this IComponent ancestor, int index)
        {
            return ancestor[index] as ISubcomponent;
        }

        /// <summary>Create a new message.</summary>
        /// <typeparam name="T">Any IMessage type.</typeparam>
        /// <returns>New message builder/parser.</returns>
        private static T ToNewMessage<T>(IEnumerable<ISegment> segments) where T : IMessage, new()
        {
            // for message metadata
            var newMessage = new T();

            // build empty message when there are no segments
            var sourceSegments = segments.ToList();
            if (sourceSegments.Count == 0)
            {
                return newMessage;
            }

            // determine where to pull message metadata from
            var childSegment = sourceSegments.FirstOrDefault(s => s.Type == "MSH") ??
                               sourceSegments.Where(s => s.Ancestor != null)
                                   .Select(s => s.Ancestor[1])
                                   .FirstOrDefault();
            if (childSegment != null)
            {
                CopyTo(childSegment, newMessage[1]);
            }

            // build message
            foreach (var segment in sourceSegments)
            {
                newMessage.Add(segment);
            }

            return newMessage;
        }

        /// <summary>Create a new message builder with the selected segments.</summary>
        /// <param name="segments">Segments to make a new message for.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder ToNewBuilder(this IEnumerable<ISegment> segments)
        {
            return ToNewMessage<MessageBuilder>(segments);
        }

        /// <summary>Create a new message parser with the selected segments.</summary>
        /// <param name="segments">Segments to make a new message for.</param>
        /// <returns>New message parser.</returns>
        public static IMessageParser ToNewParser(this IEnumerable<ISegment> segments)
        {
            return ToNewMessage<MessageParser>(segments);
        }
    }
}
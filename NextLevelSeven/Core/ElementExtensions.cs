using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Diagnostics;

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
            target.Value = string.Join(new string(target.Delimiter, 1), target.Value, elementToAdd);
        }

        /// <summary>
        ///     Add an element as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this IElement target, IElement elementToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), target.Value, elementToAdd.ToString());
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<string> elementsToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), (new[] { target.Value }).Concat(elementsToAdd));
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this IElement target, IEnumerable<IElement> elementsToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1),
                (new[] { target.Value }).Concat(elementsToAdd.Select(e => e.ToString())));
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
    }
}
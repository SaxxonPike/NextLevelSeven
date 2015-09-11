using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native
{
    /// <summary>
    ///     Extensions for generic element functionality.
    /// </summary>
    public static class NativeElementExtensions
    {
        /// <summary>
        ///     Add a string as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">String to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this INativeElement target, string elementToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), target.Value, elementToAdd);
        }

        /// <summary>
        ///     Add an element as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        public static void Add(this INativeElement target, INativeElement elementToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), target.Value, elementToAdd.ToString());
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this INativeElement target, IEnumerable<string> elementsToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), (new[] {target.Value}).Concat(elementsToAdd));
        }

        /// <summary>
        ///     Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        public static void AddRange(this INativeElement target, IEnumerable<INativeElement> elementsToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1),
                (new[] {target.Value}).Concat(elementsToAdd.Select(e => e.ToString())));
        }

        /// <summary>
        ///     Delete a descendant element.
        /// </summary>
        /// <param name="target">Element to delete from.</param>
        /// <param name="index">Index of descendant to delete.</param>
        public static void Delete(this INativeElement target, int index)
        {
            if (index >= 1 && index <= target.ValueCount)
            {
                target[index].Delete();
            }
        }

        /// <summary>
        ///     Delete elements in the enumerable. All elements must share a direct ancestor.
        /// </summary>
        /// <param name="targets">Elements to delete.</param>
        public static void Delete(this IEnumerable<INativeElement> targets)
        {
            var elements = targets.ToList();
            if (elements.Count <= 0)
            {
                return;
            }

            var ancestor = elements.First().AncestorElement;
            if (elements.Any(e => e == null || !ReferenceEquals(e.AncestorElement, ancestor)))
            {
                throw new ElementException(ErrorCode.ElementsMustShareDirectAncestors);
            }

            // delete them in reverse order so that the parent isn't removed out
            // from under the values we're deleting.
            foreach (var element in elements.Select(e => e.Index).Distinct().OrderByDescending(i => i))
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
        public static void InsertAfter(this INativeElement target, int index, INativeElement elementToInsert)
        {
            target[index].InsertAfter(elementToInsert);
        }

        /// <summary>
        ///     Insert element data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertAfter(this INativeElement target, INativeElement elementToInsert)
        {
            if (target.AncestorElement == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index;
            var ancestor = target.AncestorElement;
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
        public static void InsertAfter(this INativeElement target, int index, string dataToInsert)
        {
            target[index].InsertAfter(dataToInsert);
        }

        /// <summary>
        ///     Insert string data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        public static void InsertAfter(this INativeElement target, string dataToInsert)
        {
            if (target.AncestorElement == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index;
            var ancestor = target.AncestorElement;
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
        public static void InsertBefore(this INativeElement target, int index, INativeElement elementToInsert)
        {
            target[index].InsertBefore(elementToInsert);
        }

        /// <summary>
        ///     Insert element data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this INativeElement target, INativeElement elementToInsert)
        {
            if (target.AncestorElement == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index - 1;
            var ancestor = target.AncestorElement;
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
        public static void InsertBefore(this INativeElement target, int index, string dataToInsert)
        {
            target[index].InsertBefore(dataToInsert);
        }

        /// <summary>
        ///     Insert string data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        public static void InsertBefore(this INativeElement target, string dataToInsert)
        {
            if (target.AncestorElement == null)
            {
                throw new ElementException(ErrorCode.AncestorDoesNotExist);
            }

            var index = target.Index - 1;
            var ancestor = target.AncestorElement;
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
        public static INativeElement MoveToIndex(this INativeElement target, int targetIndex)
        {
            var ancestor = target.AncestorElement;
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

            if (ancestor is INativeSegment)
            {
                if (target.Index < 1)
                {
                    throw new ElementException(ErrorCode.SegmentTypeCannotBeMoved);
                }

                if (target.Index <= 2 && (ancestor as INativeSegment).Type == "MSH")
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
    }
}
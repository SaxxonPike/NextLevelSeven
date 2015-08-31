using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Transformation;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Extensions for generic element functionality.
    /// </summary>
    static public class ElementExtensions
    {
        /// <summary>
        /// Add a string as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">String to be added.</param>
        /// <returns>The newly added element.</returns>
        static public void Add(this IElement target, string elementToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), target.Value, elementToAdd);
        }

        /// <summary>
        /// Add an element as a descendant.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        static public void Add(this IElement target, IElement elementToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), target.Value, elementToAdd.ToString());
        }

        /// <summary>
        /// Add a string as a descendant and get the descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">String to be added.</param>
        /// <returns>The newly added element.</returns>
        static public IElement AddAndRetrieve(this IElement target, string elementToAdd)
        {
            Add(target, elementToAdd);
            return target[target.DescendantCount + 1];
        }
        
        /// <summary>
        /// Add an element as a descendant and get the descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementToAdd">Element to be added.</param>
        /// <returns>The newly added element.</returns>
        static public IElement AddAndRetrieve(this IElement target, IElement elementToAdd)
        {
            Add(target, elementToAdd);
            return target[target.DescendantCount + 1];
        }

        /// <summary>
        /// Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        static public void AddRange(this IElement target, IEnumerable<string> elementsToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), (new[]{target.Value}).Concat(elementsToAdd));
        }

        /// <summary>
        /// Add elements as descendants.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="elementsToAdd">Elements to be added.</param>
        static public void AddRange(this IElement target, IEnumerable<IElement> elementsToAdd)
        {
            target.Value = string.Join(new string(target.Delimiter, 1), (new[] { target.Value }).Concat(elementsToAdd.Select(e => e.ToString())));
        }

        /// <summary>
        /// Insert element data after the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data after.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        static public void InsertAfter(this IElement target, int index, IElement elementToInsert)
        {
            target[index].InsertAfter(elementToInsert);
        }

        /// <summary>
        /// Insert element data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        static public void InsertAfter(this IElement target, IElement elementToInsert)
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
        /// Insert string data after the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        static public void InsertAfter(this IElement target, int index, string dataToInsert)
        {
            target[index].InsertAfter(dataToInsert);
        }

        /// <summary>
        /// Insert string data after the specified element.
        /// </summary>
        /// <param name="target">Element to add data after.</param>
        /// <param name="dataToInsert">Descendant string to insert.</param>
        static public void InsertAfter(this IElement target, string dataToInsert)
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
        /// Insert element data before the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        static public void InsertBefore(this IElement target, int index, IElement elementToInsert)
        {
            target[index].InsertBefore(elementToInsert);
        }

        /// <summary>
        /// Insert element data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="elementToInsert">Descendant data to insert.</param>
        static public void InsertBefore(this IElement target, IElement elementToInsert)
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
        /// Insert string data before the specified descendant element.
        /// </summary>
        /// <param name="target">Element to add to.</param>
        /// <param name="index">Index of the descendant to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        static public void InsertBefore(this IElement target, int index, string dataToInsert)
        {
            target[index].InsertBefore(dataToInsert);
        }

        /// <summary>
        /// Insert string data before the specified element.
        /// </summary>
        /// <param name="target">Element to add data before.</param>
        /// <param name="dataToInsert">Descendant data to insert.</param>
        static public void InsertBefore(this IElement target, string dataToInsert)
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
        /// Move element within its ancestor. Returns the new element reference.
        /// </summary>
        /// <param name="target">Element to move.</param>
        /// <param name="targetIndex">Target index.</param>
        /// <returns>Element in its new place.</returns>
        static public IElement MoveToIndex(this IElement target, int targetIndex)
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
        /// Apply a transformation for each element.
        /// </summary>
        /// <param name="element">Element to transform.</param>
        /// <param name="transform">A transform to clone.</param>
        /// <returns>Transformed element.</returns>
        static public ElementTransform Transform(this IElement element, ElementTransform transform)
        {
            return transform.CloneTransform(element);
        }

        /// <summary>
        /// Apply a transformation for each element.
        /// </summary>
        /// <param name="element">Element to transform.</param>
        /// <param name="transformFactory">Factory that generates a transform for this element.</param>
        /// <returns>Transformed element.</returns>
        static public ElementTransform Transform(this IElement element, IElementTransformFactory transformFactory)
        {
            return transformFactory.CreateTransform(element);
        }

        /// <summary>
        /// Apply a transformation for each element.
        /// </summary>
        /// <param name="elements">Elements to transform.</param>
        /// <param name="transform">A transform to clone.</param>
        /// <returns>Transformed elements.</returns>
        static public IEnumerable<ElementTransform> TransformAll(this IEnumerable<IElement> elements, ElementTransform transform)
        {
            return elements.Select(transform.CloneTransform);
        }

        /// <summary>
        /// Apply a transformation for each element.
        /// </summary>
        /// <param name="elements">Elements to transform.</param>
        /// <param name="transformFactory">Factory that generates a transform for these elements.</param>
        /// <returns>Transformed elements.</returns>
        static public IEnumerable<ElementTransform> TransformAll(this IEnumerable<IElement> elements, IElementTransformFactory transformFactory)
        {
            return elements.Select(transformFactory.CreateTransform);
        }
    }
}

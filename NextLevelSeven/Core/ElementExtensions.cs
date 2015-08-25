using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    static public class ElementExtensions
    {
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
                return;
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
                return;
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
                return;
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
                return;
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
        /// Move element within its ancestor.
        /// </summary>
        /// <param name="target">Element to move.</param>
        /// <param name="targetIndex">Target index.</param>
        static public void MoveToIndex(this IElement target, int targetIndex)
        {
            
        }
    }
}

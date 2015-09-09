using NextLevelSeven.Core;

namespace NextLevelSeven.Transformation
{
    /// <summary>
    ///     Interface for a factory to generate element transforms.
    /// </summary>
    public interface IElementTransformFactory
    {
        /// <summary>
        ///     Create a transform for an element.
        /// </summary>
        /// <param name="element">Element to apply the transform to.</param>
        /// <returns>A transform for the specified element.</returns>
        ElementTransform CreateTransform(INativeElement element);
    }
}
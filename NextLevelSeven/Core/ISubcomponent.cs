namespace NextLevelSeven.Core
{
    /// <summary>Represents a subcomponent element in an HL7 message.</summary>
    public interface ISubcomponent : IElement
    {
        /// <summary>Get the ancestor component. Null if the element is an orphan.</summary>
        new IComponent Ancestor { get; }

        /// <summary>Get a copy of the segment.</summary>
        new ISubcomponent Clone();
    }
}
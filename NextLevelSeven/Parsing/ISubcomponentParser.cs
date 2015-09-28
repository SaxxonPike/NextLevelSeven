using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    /// <summary>A parser for HL7 messages at the subcomponent level.</summary>
    public interface ISubcomponentParser : IElementParser, ISubcomponent
    {
        /// <summary>Get the ancestor component. Null if the element is an orphan.</summary>
        new IComponentParser Ancestor { get; }
    }
}
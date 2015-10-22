namespace NextLevelSeven.Core.Properties
{
    /// <summary>Interface that provides Application and Facility information.</summary>
    public interface IIdentity
    {
        /// <summary>Get or set the application name.</summary>
        string Application { get; set; }

        /// <summary>Get or set the facility name.</summary>
        string Facility { get; set; }
    }
}
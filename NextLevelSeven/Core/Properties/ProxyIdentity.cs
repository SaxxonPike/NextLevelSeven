namespace NextLevelSeven.Core.Properties
{
    internal sealed class ProxyIdentity : IIdentity
    {
        /// <summary>Get the application data index.</summary>
        private readonly int _applicationIndex;

        /// <summary>Get the referenced builder.</summary>
        private readonly IElement _element;

        /// <summary>Get the facility data index.</summary>
        private readonly int _facilityIndex;

        /// <summary>Create an Identity reference via HL7 element.</summary>
        /// <param name="element">Element to reference.</param>
        /// <param name="applicationIndex">Index of the application data.</param>
        /// <param name="facilityIndex">Index of the facility data.</param>
        public ProxyIdentity(IElement element, int applicationIndex, int facilityIndex)
        {
            _applicationIndex = applicationIndex;
            _element = element;
            _facilityIndex = facilityIndex;
        }

        /// <summary>Get or set the application name.</summary>
        public string Application
        {
            get => _element[_applicationIndex].Value;
            set => _element[_applicationIndex].Value = value;
        }

        /// <summary>Get or set the facility name.</summary>
        public string Facility
        {
            get => _element[_facilityIndex].Value;
            set => _element[_facilityIndex].Value = value;
        }
    }
}
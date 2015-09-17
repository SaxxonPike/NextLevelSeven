namespace NextLevelSeven.Core.Properties
{
    internal sealed class ProxyIdentity : IIdentity
    {
        /// <summary>
        ///     Create an Identity reference via HL7 element.
        /// </summary>
        /// <param name="element">Element to reference.</param>
        /// <param name="applicationIndex">Index of the application data.</param>
        /// <param name="facilityIndex">Index of the facility data.</param>
        public ProxyIdentity(IElement element, int applicationIndex, int facilityIndex)
        {
            ApplicationIndex = applicationIndex;
            Element = element;
            FacilityIndex = facilityIndex;
        }

        /// <summary>
        ///     Get the application data index.
        /// </summary>
        private int ApplicationIndex { get; set; }

        /// <summary>
        ///     Get the referenced builder.
        /// </summary>
        private IElement Element { get; set; }

        /// <summary>
        ///     Get the facility data index.
        /// </summary>
        private int FacilityIndex { get; set; }

        /// <summary>
        ///     Get or set the application name.
        /// </summary>
        public string Application
        {
            get { return Element[ApplicationIndex].Value; }
            set { Element[ApplicationIndex].Value = value; }
        }

        /// <summary>
        ///     Get or set the facility name.
        /// </summary>
        public string Facility
        {
            get { return Element[FacilityIndex].Value; }
            set { Element[FacilityIndex].Value = value; }
        }
    }
}
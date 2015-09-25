namespace NextLevelSeven.Specification
{
    /// <summary>Stores information about calibration parameters for a channel in the HL7 specification. (CCP)</summary>
    public interface IChannelCalibrationParameters : ISpecificationElement
    {
        /// <summary>Channel calibration sensitivity correction factor, as a number. (CCP.1)</summary>
        decimal? SensitivityCorrectionFactor
        {
            get;
            set;
        }

        /// <summary>Channel calibration sensitivity correction factor, as a string. (CCP.1)</summary>
        string SensitivityCorrectionFactorData
        {
            get;
            set;
        }

        /// <summary>Channel calibration baseline, as a number. (CCP.2)</summary>
        decimal? Baseline
        {
            get;
            set;
        }

        /// <summary>Channel calibration baseline, as a string. (CCP.2)</summary>
        string BaselineData
        {
            get;
            set;
        }

        /// <summary>Channel calibration time skew, as a number. (CCP.3)</summary>
        decimal? TimeSkew
        {
            get;
            set;
        }

        /// <summary>Channel calibration time skew, as a string. (CCP.3)</summary>
        string TimeSkewData
        {
            get;
            set;
        }
    }
}
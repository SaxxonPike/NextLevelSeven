namespace NextLevelSeven.Core.Specification
{
    /// <summary>
    ///     Contains information about a channel in the HL7 specification. (CD)
    /// </summary>
    public interface IChannelDefinition : ISpecificationElement
    {
        /// <summary>
        ///     Channel identifier, as a WVI type. (CD.1)
        /// </summary>
        IChannelIdentifier ChannelIdentifier { get; }

        /// <summary>
        ///     Channel identifier, as a string. (CD.1)
        /// </summary>
        string ChannelIdentifierData { get; set; }

        /// <summary>
        ///     Waveform source, as a WVS type. (CD.2)
        /// </summary>
        IWaveformSource WaveformSource { get; }

        /// <summary>
        ///     Waveform source, as a string. (CD.2)
        /// </summary>
        string WaveformSourceData { get; set; }

        /// <summary>
        ///     Channel sensitivity/units, as a CSU type. (CD.3)
        /// </summary>
        IChannelSensitivityAndUnits ChannelSensitivityAndUnits { get; }

        /// <summary>
        ///     Channel sensitivity/units, as a string. (CD.3)
        /// </summary>
        string ChannelSensitivityAndUnitsData { get; set; }

        /// <summary>
        ///     Channel calibration parameters, as a CCP type. (CD.4)
        /// </summary>
        IChannelCalibrationParameters ChannelCalibrationParameters { get; }

        /// <summary>
        ///     Channel calibration parameters, as a string. (CD.4)
        /// </summary>
        string ChannelCalibrationParamtersData { get; set; }

        /// <summary>
        ///     Channel sampling frequency, as a number. (CD.5)
        /// </summary>
        decimal? ChannelSamplingFrequency { get; set; }

        /// <summary>
        ///     Channel sampling frequency, as a string. (CD.5)
        /// </summary>
        string ChannelSamplingFrequencyData { get; set; }

        /// <summary>
        ///     Minimum and maximum data values, as a NR type. (CD.6)
        /// </summary>
        INumberRange MinimumMaximumValues { get; }

        /// <summary>
        ///     Minimum and maximum data values, as a string. (CD.6)
        /// </summary>
        string MinimumMaximumValuesData { get; set; }
    }
}
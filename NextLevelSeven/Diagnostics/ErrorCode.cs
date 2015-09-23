namespace NextLevelSeven.Diagnostics
{
    /// <summary>
    ///     An enumeration of possible error messages.
    /// </summary>
    public enum ErrorCode
    {
        // NOTE: Do not change error code numbers once they have been added.
        // It is okay to add new ones, but once a number has been assigned, it
        // should never be reused. Also, do not remove items or reorder this list.

        /* 0000 */
        Unspecified,
        /* 0001 */
        ComponentIndexMustBeGreaterThanZero,
        /* 0002 */
        DescendantElementsCannotBeModified,
        /* 0003 */
        ExceededRetriesForMessage,
        /* 0004 */
        FieldIndexMustBeZeroOrGreater,
        /* 0005 */
        HeaderByteIsIncorrect,
        /* 0006 */
        MessageDataMustNotBeNull,
        /* 0007 */
        MessageDataMustStartWithMsh,
        /* 0008 */
        MessageDataIsTooShort,
        /* 0009 */
        RepetitionIndexMustBeZeroOrGreater,
        /* 0010 */
        RootElementCannotBeDeleted,
        /* 0011 */
        RootElementCannotBeErased,
        /* 0012 */
        SegmentIndexMustBeGreaterThanZero,
        /* 0013 */
        SubcomponentCannotHaveDescendants,
        /* 0014 */
        SubcomponentIndexMustBeGreaterThanZero,
        /* 0015 */
        TimedOutWaitingForTransportToBecomeReady,
        /* 0016 */
        UnableToParseDate,
        /* 0017 */
        ReachedEndOfMlpStream,
        /* 0018 */
        MlpDataEndedPrematurely,
        /* 0019 */
        InvalidMlpCharacter,
        /* 0020 */
        DoNotTranslateThisMessageForTestingPurposes,
        /* 0021 */
        AncestorDoesNotExist,
        /* 0022 */
        ElementIndexMustBeZeroOrGreater,
        /* 0023 */
        EncodingElementCannotBeMoved,
        /* 0024 */
        SegmentTypeCannotBeMoved,
        /* 0025 */
        ElementsMustShareDirectAncestors,
        /* 0026 */
        SegmentBuilderHasInvalidSegmentType,
        /* 0027 */
        TransportNotStarted,
        /* 0028 */
        FixedFieldsCannotBeDivided,
        /* 0029 */
        ChangingSegmentTypesToAndFromMshIsNotSupported,
        /* 0030 */
        InvalidAddressType,
        /* 0031 */
        DataTypeIsTooLong,
        /* 0032 */
        DataIsInvalidForType,
        /* 0033 */
        FieldCannotBeNull,
        /* 0034 */
        SegmentDataIsTooShort,
        /* 0035 */
        InvalidCompare,
        /* 0036 */
        MessageTypeIsInvalid,
        /* 0037 */
        MessageTriggerEventIsInvalid,
    }
}
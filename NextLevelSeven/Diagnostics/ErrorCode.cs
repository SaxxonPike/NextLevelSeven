namespace NextLevelSeven.Diagnostics
{
    /// <summary>An enumeration of possible error messages.</summary>
    public enum ErrorCode
    {
        // NOTE: Do not change error code numbers once they have been added.
        // It is okay to add new ones, but once a number has been assigned, it
        // should never be reused. Also, do not remove items or reorder this list.

        /* 0000 */
        Unspecified,
        /* 0001 */
        DoNotTranslateThisMessageForTestingPurposes,
        /* 0002 */
        ComponentIndexMustBeGreaterThanZero,
        /* 0003 */
        FieldIndexMustBeZeroOrGreater,
        /* 0004 */
        HeaderByteIsIncorrect,
        /* 0005 */
        MessageDataMustNotBeNull,
        /* 0006 */
        MessageDataMustStartWithMsh,
        /* 0007 */
        MessageDataIsTooShort,
        /* 0008 */
        RepetitionIndexMustBeGreaterThanZero,
        /* 0009 */
        SegmentIndexMustBeGreaterThanZero,
        /* 0010 */
        SubcomponentCannotHaveDescendants,
        /* 0011 */
        SubcomponentIndexMustBeGreaterThanZero,
        /* 0012 */
        UnableToParseDate,
        /* 0013 */
        MlpDataEndedPrematurely,
        /* 0014 */
        AncestorDoesNotExist,
        /* 0015 */
        ElementIndexMustBeZeroOrGreater,
        /* 0016 */
        EncodingElementCannotBeMoved,
        /* 0017 */
        SegmentTypeCannotBeMoved,
        /* 0018 */
        ElementsMustShareDirectAncestors,
        /* 0019 */
        FixedFieldsCannotBeDivided,
        /* 0020 */
        ChangingSegmentTypesToAndFromMshIsNotSupported,
        /* 0021 */
        DataTypeIsTooLong,
        /* 0022 */
        DataIsInvalidForType,
        /* 0023 */
        FieldCannotBeNull,
        /* 0024 */
        SegmentDataIsTooShort,
        /* 0025 */
        SegmentDataMustNotBeNull,
        /* 0026 */
        ElementIndexMustBeGreaterThanZero,
        /* 0027 */
        ElementValueCannotBeChanged,
    }
}
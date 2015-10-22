using System.Diagnostics.CodeAnalysis;

namespace NextLevelSeven.Diagnostics
{
    public static partial class ErrorMessages
    {
        /// <summary>English language translation.</summary>
        [ExcludeFromCodeCoverage]
        private sealed class English : ErrorMessageLanguage
        {
            public override string GetMessage(ErrorCode code)
            {
                switch (code)
                {
                    case ErrorCode.AncestorDoesNotExist:
                        return "Ancestor does not exist.";
                    case ErrorCode.ChangingSegmentTypesToAndFromMshIsNotSupported:
                        return "Changing segment types to and from MSH is not currently supported.";
                    case ErrorCode.ComponentIndexMustBeGreaterThanZero:
                        return "Component index must be greater than zero.";
                    case ErrorCode.DataIsInvalidForType:
                        return "Data is invalid for type: {0}";
                    case ErrorCode.DoNotTranslateThisMessageForTestingPurposes:
                        return "This message is not to be translated to any other language for testing purposes.";
                    case ErrorCode.ElementIndexMustBeGreaterThanZero:
                        return "Element index must be greater than zero.";
                    case ErrorCode.ElementIndexMustBeZeroOrGreater:
                        return "Element index must be zero or greater.";
                    case ErrorCode.ElementsMustShareDirectAncestors:
                        return "Elements must all share a direct ancestor.";
                    case ErrorCode.ElementValueCannotBeChanged:
                        return "Element value cannot be changed.";
                    case ErrorCode.EncodingElementCannotBeMoved:
                        return "Encoding element cannot be moved.";
                    case ErrorCode.FieldCannotBeNull:
                        return "Field cannot be null.";
                    case ErrorCode.FieldIndexMustBeZeroOrGreater:
                        return "Field index must be zero or greater.";
                    case ErrorCode.FixedFieldsCannotBeDivided:
                        return "Fixed fields (such as field delimiter and encoding characters) cannot be divided.";
                    case ErrorCode.HeaderByteIsIncorrect:
                        return "Header byte is incorrect.";
                    case ErrorCode.MessageDataIsTooShort:
                        return "Message data is too short.";
                    case ErrorCode.MessageDataMustNotBeNull:
                        return "Message data must not be null.";
                    case ErrorCode.MessageDataMustStartWithMsh:
                        return "Message data must start with 'MSH'.";
                    case ErrorCode.MlpDataEndedPrematurely:
                        return "Reached end of stream before MLP end-of-message was found.";
                    case ErrorCode.RepetitionIndexMustBeGreaterThanZero:
                        return "Repetition index must be greater than zero.";
                    case ErrorCode.SegmentDataIsTooShort:
                        return "Segment data must be at least 4 characters in length.";
                    case ErrorCode.SegmentDataMustNotBeNull:
                        return "Segment data must not be null.";
                    case ErrorCode.SegmentIndexMustBeGreaterThanZero:
                        return "Segment index must be greater than zero.";
                    case ErrorCode.SegmentTypeCannotBeMoved:
                        return "Segment type cannot be moved.";
                    case ErrorCode.SubcomponentCannotHaveDescendants:
                        return "Subcomponent cannot have descendants.";
                    case ErrorCode.SubcomponentIndexMustBeGreaterThanZero:
                        return "Subcomponent index must be greater than zero.";
                    case ErrorCode.UnableToParseDate:
                        return "Unable to parse date.";
                    case ErrorCode.Unspecified:
                        return "Unspecified error.";
                }

                // if no other message is given, return the generic error message.
                return null;
            }
        }
    }
}
namespace NextLevelSeven.Diagnostics
{
    public static partial class ErrorMessages
    {
        /// <summary>
        ///     English language translation.
        /// </summary>
        private class English : ErrorMessageLanguage
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
                    case ErrorCode.DescendantElementsCannotBeModified:
                        return "Descendant elements cannot be modified.";
                    case ErrorCode.DoNotTranslateThisMessageForTestingPurposes:
                        return "This message is not to be translated to any other language for testing purposes.";
                    case ErrorCode.ElementIndexMustBeZeroOrGreater:
                        return "Element index must be zero or greater.";
                    case ErrorCode.ElementsMustShareDirectAncestors:
                        return "Elements must all share a direct ancestor.";
                    case ErrorCode.EncodingElementCannotBeMoved:
                        return "Encoding element cannot be moved.";
                    case ErrorCode.ExceededRetriesForMessage:
                        return "Exceeded retries for message.";
                    case ErrorCode.FieldIndexMustBeZeroOrGreater:
                        return "Field index must be zero or greater.";
                    case ErrorCode.FixedFieldsCannotBeDivided:
                        return "Fixed fields (such as field delimiter and encoding characters) cannot be divided.";
                    case ErrorCode.HeaderByteIsIncorrect:
                        return "Header byte is incorrect.";
                    case ErrorCode.InvalidAddressType:
                        return "Address type is not valid.";
                    case ErrorCode.InvalidMlpCharacter:
                        return "Invalid character found in MLP stream.";
                    case ErrorCode.MessageDataIsTooShort:
                        return "Message data is too short.";
                    case ErrorCode.MessageDataMustNotBeNull:
                        return "Message data must not be null.";
                    case ErrorCode.MessageDataMustStartWithMsh:
                        return "Message data must start with 'MSH'.";
                    case ErrorCode.MlpDataEndedPrematurely:
                        return "Reached end of stream before MLP end-of-message was found.";
                    case ErrorCode.ReachedEndOfMlpStream:
                        return "Reached end of stream before MLP data could be read.";
                    case ErrorCode.RepetitionIndexMustBeZeroOrGreater:
                        return "Repetition index must be zero or greater.";
                    case ErrorCode.RootElementCannotBeDeleted:
                        return "Root element cannot be deleted.";
                    case ErrorCode.RootElementCannotBeErased:
                        return "Root element cannot be erased.";
                    case ErrorCode.SegmentBuilderHasInvalidSegmentType:
                        return "Segment builder has invalid segment type. Field 0 must have a three-letter code.";
                    case ErrorCode.SegmentIndexMustBeGreaterThanZero:
                        return "Segment index must be greater than zero.";
                    case ErrorCode.SegmentTypeCannotBeMoved:
                        return "Segment type cannot be moved.";
                    case ErrorCode.SubcomponentCannotHaveDescendants:
                        return "Subcomponent cannot have descendants.";
                    case ErrorCode.SubcomponentIndexMustBeGreaterThanZero:
                        return "Subcomponent index must be greater than zero.";
                    case ErrorCode.TimedOutWaitingForTransportToBecomeReady:
                        return "Timed out waiting for transport to become ready.";
                    case ErrorCode.TransportNotStarted:
                        return "The transport is not running.";
                    case ErrorCode.UnableToParseDate:
                        return "Unable to parse date.";
                    case ErrorCode.Unspecified:
                        return "Unspecified error.";
                }
                return null;
            }
        }
    }
}
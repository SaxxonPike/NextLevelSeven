using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Mapping.Maps
{
    public enum MshSegmentMap
    {
        FieldSeparator,
        EncodingCharacters,
        SendingApplication,
        SendingFacility,
        ReceivingApplication,
        ReceivingFacility,
        MessageDateTime,
        Security,
        Type,
        TypeCode,
        TypeTriggerEvent,
        TypeStructure,
        ControlId,
        ProcessingId,
        VersionId,
        SequenceNumber,
        ContinuationPointer,
        AcceptAcknowledgementType,
        ApplicationAcknowledgementType,
        CountryCode,
        CharacterSet,
        PrincipalLanguage,
        AlternateCharacterSet,
        ProfileIdentifier,
        SendingResponsibleOrganization,
        ReceivingResponsibleOrganization,
        SendingNetworkAddress,
        ReceivingNetworkAddress,
    }
}

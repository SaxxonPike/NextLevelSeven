using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Mapping.Maps
{
    public class MshSegmentConfiguration : MapConfiguration<MshSegmentMap>
    {
        public MshSegmentConfiguration()
        {
            this[MshSegmentMap.FieldSeparator] = new MapLocation("MSH", 1);
            this[MshSegmentMap.EncodingCharacters] = new MapLocation("MSH", 2);
            this[MshSegmentMap.SendingApplication] = new MapLocation("MSH", 3);
            this[MshSegmentMap.SendingFacility] = new MapLocation("MSH", 4);
            this[MshSegmentMap.ReceivingApplication] = new MapLocation("MSH", 5);
            this[MshSegmentMap.ReceivingFacility] = new MapLocation("MSH", 6);
            this[MshSegmentMap.MessageDateTime] = new MapLocation("MSH", 7);
            this[MshSegmentMap.Security] = new MapLocation("MSH", 8);
            this[MshSegmentMap.Type] = new MapLocation("MSH", 9);
            this[MshSegmentMap.TypeCode] = new MapLocation("MSH", 9, 1, 1);
            this[MshSegmentMap.TypeTriggerEvent] = new MapLocation("MSH", 9, 1, 2);
            this[MshSegmentMap.TypeStructure] = new MapLocation("MSH", 9, 1, 3);
            this[MshSegmentMap.ControlId] = new MapLocation("MSH", 10);
            this[MshSegmentMap.ProcessingId] = new MapLocation("MSH", 11);
            this[MshSegmentMap.VersionId] = new MapLocation("MSH", 12);
            this[MshSegmentMap.SequenceNumber] = new MapLocation("MSH", 13);
            this[MshSegmentMap.ContinuationPointer] = new MapLocation("MSH", 14);
            this[MshSegmentMap.AcceptAcknowledgementType] = new MapLocation("MSH", 15);
            this[MshSegmentMap.ApplicationAcknowledgementType] = new MapLocation("MSH", 16);
            this[MshSegmentMap.CountryCode] = new MapLocation("MSH", 17);
            this[MshSegmentMap.CharacterSet] = new MapLocation("MSH", 18);
            this[MshSegmentMap.PrincipalLanguage] = new MapLocation("MSH", 19);
            this[MshSegmentMap.AlternateCharacterSet] = new MapLocation("MSH", 20);
            this[MshSegmentMap.SendingResponsibleOrganization] = new MapLocation("MSH", 21);
            this[MshSegmentMap.ReceivingResponsibleOrganization] = new MapLocation("MSH", 22);
            this[MshSegmentMap.SendingNetworkAddress] = new MapLocation("MSH", 23);
            this[MshSegmentMap.ReceivingNetworkAddress] = new MapLocation("MSH", 24);
        }
    }
}

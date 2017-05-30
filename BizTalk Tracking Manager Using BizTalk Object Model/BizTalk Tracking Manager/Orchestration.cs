using System.ComponentModel;

namespace BizTalk_Tracking_Manager
{
    public class Orchestration
    {
        public string Name { get; set; }

        [DisplayName("Track Events\r\nOrchestrations start and end")]
        public bool ServiceStartEnd { get; set; }

        [DisplayName("Track Events\r\nMessage send and receive")]
        public bool MessageSendReceive { get; set; }

        [DisplayName("Track Events\r\nShape start and end")]
        public bool OrchestrationEvents { get; set; }

        [DisplayName("Track Message Bodies\r\nBefore orchestration processing")]
        public bool InboundMessageBody { get; set; }

        [DisplayName("Track Message Bodies\r\nAfter orchestration processing")]
        public bool OutboundMessageBody { get; set; }

        [DisplayName("Track Message Properties\r\nIncoming messages")]
        public bool TrackPropertiesForIncomingMessages { get; set; }

        [DisplayName("Track Message Properties\r\nOutgoing messages")]
        public bool TrackPropertiesForOutgoingMessages { get; set; }
    }
}
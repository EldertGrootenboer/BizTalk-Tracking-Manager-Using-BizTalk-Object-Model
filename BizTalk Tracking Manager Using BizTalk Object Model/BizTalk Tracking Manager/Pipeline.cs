using System.ComponentModel;

namespace BizTalk_Tracking_Manager
{
    public class Pipeline
    {
        public string Name { get; set; }

        [DisplayName("Track Events\r\nPipeline events")]
        public bool PipelineEvents { get; set; }

        [DisplayName("Track Events\r\nPort start and end events")]
        public bool ServiceStartEnd { get; set; }

        [DisplayName("Track Events\r\nMessage send and receive events")]
        public bool MessageSendReceive { get; set; }

        [DisplayName("Track Message Bodies\r\nMessage before pipeline processing")]
        public bool InboundMessageBody { get; set; }

        [DisplayName("Track Message Bodies\r\nMessage after pipeline processing")]
        public bool OutboundMessageBody { get; set; }
    }
}
using System.ComponentModel;

namespace BizTalk_Tracking_Manager
{
    public class SendPort
    {
        public string Name { get; set; }

        [DisplayName("Track Message Bodies\r\nRequest message before port processing")]
        public bool BeforeSendPipeline { get; set; }

        [DisplayName("Track Message Bodies\r\nRequest message after port processing")]
        public bool AfterSendPipeline { get; set; }

        [DisplayName("Track Message Bodies\r\nResponse message before port processing")]
        public bool BeforeReceivePipeline { get; set; }

        [DisplayName("Track Message Bodies\r\nResponse message after port processing")]
        public bool AfterReceivePipeline { get; set; }

        [DisplayName("Track Message Properties\r\nRequest message before port processing")]
        public bool TrackPropertiesBeforeSendPipeline { get; set; }

        [DisplayName("Track Message Properties\r\nRequest message after port processing")]
        public bool TrackPropertiesAfterSendPipeline { get; set; }

        [DisplayName("Track Message Properties\r\nResponse message before port processing")]
        public bool TrackPropertiesBeforeReceivePipeline { get; set; }

        [DisplayName("Track Message Properties\r\nResponse message after port processing")]
        public bool TrackPropertiesAfterReceivePipeline { get; set; }

        public bool IsTwoWay;

    }
}
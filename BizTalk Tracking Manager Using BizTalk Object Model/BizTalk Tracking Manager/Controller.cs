using System.Collections.Generic;
using System.Linq;

using Microsoft.BizTalk.ExplorerOM;

namespace BizTalk_Tracking_Manager
{
    /// <summary>
    /// This class acts as as a proxy between the UI and data.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Create helper for communicating with data model.
        /// </summary>
        private readonly BizTalkObjectModelHelper _bizTalkObjectModelHelper = new BizTalkObjectModelHelper();

        /// <summary>
        /// Get artifacts from data model.
        /// </summary>
        public IEnumerable<Orchestration> GetOrchestrations()
        {
            return
                _bizTalkObjectModelHelper.GetOrchestrations()
                    .Select(
                        orchestration =>
                            new Orchestration
                            {
                                Name = orchestration.FullName,
                                ServiceStartEnd = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.ServiceStartEnd),
                                MessageSendReceive = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.MessageSendReceive),
                                OrchestrationEvents = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.OrchestrationEvents),
                                InboundMessageBody = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.InboundMessageBody),
                                OutboundMessageBody = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.OutboundMessageBody),
                                TrackPropertiesForIncomingMessages = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.TrackPropertiesForIncomingMessages),
                                TrackPropertiesForOutgoingMessages = orchestration.Tracking.HasFlag(OrchestrationTrackingTypes.TrackPropertiesForOutgoingMessages)
                            });
        }

        /// <summary>
        /// Get artifacts from data model.
        /// </summary>
        public IEnumerable<SendPort> GetSendPorts()
        {
            return
                _bizTalkObjectModelHelper.GetSendPorts()
                    .Select(
                        sendPort =>
                            new SendPort
                            {
                                Name = sendPort.Name,
                                BeforeSendPipeline = sendPort.Tracking.HasFlag(TrackingTypes.BeforeSendPipeline),
                                AfterSendPipeline = sendPort.Tracking.HasFlag(TrackingTypes.AfterSendPipeline),
                                BeforeReceivePipeline = sendPort.Tracking.HasFlag(TrackingTypes.BeforeReceivePipeline),
                                AfterReceivePipeline = sendPort.Tracking.HasFlag(TrackingTypes.AfterReceivePipeline),
                                TrackPropertiesBeforeSendPipeline = sendPort.Tracking.HasFlag(TrackingTypes.TrackPropertiesBeforeSendPipeline),
                                TrackPropertiesAfterSendPipeline = sendPort.Tracking.HasFlag(TrackingTypes.TrackPropertiesAfterSendPipeline),
                                TrackPropertiesBeforeReceivePipeline = sendPort.Tracking.HasFlag(TrackingTypes.TrackPropertiesBeforeReceivePipeline),
                                TrackPropertiesAfterReceivePipeline = sendPort.Tracking.HasFlag(TrackingTypes.TrackPropertiesAfterReceivePipeline),
                                IsTwoWay = sendPort.IsTwoWay
                            });
        }

        /// <summary>
        /// Get artifacts from data model.
        /// </summary>
        public IEnumerable<ReceivePort> GetReceivePorts()
        {
            return
                _bizTalkObjectModelHelper.GetReceivePorts()
                    .Select(
                        receivePort =>
                            new ReceivePort
                            {
                                Name = receivePort.Name,
                                BeforeSendPipeline = receivePort.Tracking.HasFlag(TrackingTypes.BeforeSendPipeline),
                                AfterSendPipeline = receivePort.Tracking.HasFlag(TrackingTypes.AfterSendPipeline),
                                BeforeReceivePipeline = receivePort.Tracking.HasFlag(TrackingTypes.BeforeReceivePipeline),
                                AfterReceivePipeline = receivePort.Tracking.HasFlag(TrackingTypes.AfterReceivePipeline),
                                TrackPropertiesBeforeSendPipeline = receivePort.Tracking.HasFlag(TrackingTypes.TrackPropertiesBeforeSendPipeline),
                                TrackPropertiesAfterSendPipeline = receivePort.Tracking.HasFlag(TrackingTypes.TrackPropertiesAfterSendPipeline),
                                TrackPropertiesBeforeReceivePipeline = receivePort.Tracking.HasFlag(TrackingTypes.TrackPropertiesBeforeReceivePipeline),
                                TrackPropertiesAfterReceivePipeline = receivePort.Tracking.HasFlag(TrackingTypes.TrackPropertiesAfterReceivePipeline),
                                IsTwoWay = receivePort.IsTwoWay
                            });
        }

        /// <summary>
        /// Get artifacts from data model.
        /// </summary>
        public IEnumerable<Pipeline> GetPipelines()
        {
            return
                _bizTalkObjectModelHelper.GetPipelines()
                    .Select(
                        pipeline =>
                            new Pipeline
                            {
                                Name = pipeline.FullName,
                                PipelineEvents = pipeline.Tracking.HasFlag(PipelineTrackingTypes.PipelineEvents),
                                ServiceStartEnd = pipeline.Tracking.HasFlag(PipelineTrackingTypes.ServiceStartEnd),
                                MessageSendReceive = pipeline.Tracking.HasFlag(PipelineTrackingTypes.MessageSendReceive),
                                InboundMessageBody = pipeline.Tracking.HasFlag(PipelineTrackingTypes.InboundMessageBody),
                                OutboundMessageBody = pipeline.Tracking.HasFlag(PipelineTrackingTypes.OutboundMessageBody)
                            });
        }

        /// <summary>
        /// Queue saving tracking to the specified artifact.
        /// </summary>
        public void QueueOrchestrationTracking(string orchestrationName, OrchestrationTrackingTypes tracking)
        {
            _bizTalkObjectModelHelper.GetOrchestrations().First(orchestration => orchestration.FullName == orchestrationName).Tracking = tracking;
        }

        /// <summary>
        /// Queue saving tracking to the specified artifact.
        /// </summary>
        public void QueueSendPortTracking(string sendPortName, TrackingTypes tracking)
        {
            _bizTalkObjectModelHelper.GetSendPorts().First(sendPort => sendPort.Name == sendPortName).Tracking = tracking;
        }

        /// <summary>
        /// Queue saving tracking to the specified artifact.
        /// </summary>
        public void QueueReceivePortTracking(string receivePortName, TrackingTypes tracking)
        {
            _bizTalkObjectModelHelper.GetReceivePorts().First(receivePort => receivePort.Name == receivePortName).Tracking = tracking;
        }

        /// <summary>
        /// Queue saving tracking to the specified artifact.
        /// </summary>
        public void QueuePipelineTracking(string pipelineName, PipelineTrackingTypes tracking)
        {
            _bizTalkObjectModelHelper.GetPipelines().First(pipeline => pipeline.FullName == pipelineName).Tracking = tracking;
        }

        /// <summary>
        /// Save all queued changes.
        /// </summary>
        public void SaveChanges()
        {
            _bizTalkObjectModelHelper.SaveChanges();
        }
    }
}

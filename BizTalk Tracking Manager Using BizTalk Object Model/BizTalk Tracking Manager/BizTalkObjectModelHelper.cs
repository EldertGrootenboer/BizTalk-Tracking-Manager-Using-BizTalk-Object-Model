using System.Collections.Generic;
using System.Linq;

using BizTalk_Tracking_Manager.Properties;

using Microsoft.BizTalk.ExplorerOM;

namespace BizTalk_Tracking_Manager
{
    /// <summary>
    /// Class used to communicate with the BizTalk object model.
    /// </summary>
    internal class BizTalkObjectModelHelper
    {
        /// <summary>
        /// Catalog explorer for working with BizTalk objects.
        /// </summary>
        private static BtsCatalogExplorer _btsCatalogExplorer;

        # region Caching

        private static ApplicationCollection _applications;
        private static IEnumerable<BtsOrchestration> _orchestrations;
        private static IEnumerable<Microsoft.BizTalk.ExplorerOM.ReceivePort> _receivePorts;
        private static IEnumerable<Microsoft.BizTalk.ExplorerOM.SendPort> _sendPorts;
        private static IEnumerable<Microsoft.BizTalk.ExplorerOM.Pipeline> _pipelines;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public BizTalkObjectModelHelper()
        {
            _btsCatalogExplorer = new BtsCatalogExplorer { ConnectionString = Settings.Default.BizTalkManagmentDatabaseConnectionString };
        }

        /// <summary>
        /// Get all BizTalk applications.
        /// </summary>
        internal ApplicationCollection GetApplications()
        {
            return _applications ?? (_applications = _btsCatalogExplorer.Applications);
        }

        /// <summary>
        /// Save changes to the catalog explorer.
        /// </summary>
        public void SaveChanges()
        {
            _btsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Get orchestrations for all applications.
        /// </summary>
        public IEnumerable<BtsOrchestration> GetOrchestrations()
        {
            return _orchestrations ?? (_orchestrations = GetApplications().Cast<Application>().SelectMany(application => application.Orchestrations.Cast<BtsOrchestration>()));
        }

        /// <summary>
        /// Get receive ports for all applications.
        /// </summary>
        public IEnumerable<Microsoft.BizTalk.ExplorerOM.ReceivePort> GetReceivePorts()
        {
            return _receivePorts ?? (_receivePorts = GetApplications().Cast<Application>().SelectMany(application => application.ReceivePorts.Cast<Microsoft.BizTalk.ExplorerOM.ReceivePort>()));
        }
        
        /// <summary>
        /// Get send ports for all applications.
        /// </summary>
        public IEnumerable<Microsoft.BizTalk.ExplorerOM.SendPort> GetSendPorts()
        {
            return _sendPorts ?? (_sendPorts = GetApplications().Cast<Application>().SelectMany(application => application.SendPorts.Cast<Microsoft.BizTalk.ExplorerOM.SendPort>()));
        }
        
        /// <summary>
        /// Get pipelines for all applications.
        /// </summary>
        public IEnumerable<Microsoft.BizTalk.ExplorerOM.Pipeline> GetPipelines()
        {
            return _pipelines ?? (_pipelines = GetApplications().Cast<Application>().SelectMany(application => application.Pipelines.Cast<Microsoft.BizTalk.ExplorerOM.Pipeline>()));
        }
    }
}

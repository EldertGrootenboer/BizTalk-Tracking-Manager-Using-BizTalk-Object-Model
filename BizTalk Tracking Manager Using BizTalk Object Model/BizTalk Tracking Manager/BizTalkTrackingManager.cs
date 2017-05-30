using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BizTalk_Tracking_Manager.Properties;

namespace BizTalk_Tracking_Manager
{
    /// <summary>
    /// This application is used to manage BizTalk tracking settings.
    /// </summary>
    public partial class BizTalkTrackingManager : Form
    {
        /// <summary>
        /// Controller used as abstraction between UI and data.
        /// </summary>
        private readonly Controller _controller = new Controller();

        /// <summary>
        /// Constructor.
        /// </summary>
        public BizTalkTrackingManager()
        {
            // Initialize components
            InitializeComponent();
            Show();

            #region Create orchestrations tab

            var trackingManagerPanelOrchestrations = new TrackingManagerPanel(ArtifactType.Orchestration, ref _controller);
            
            var tabPageOrchestrations = new TabPage("Orchestrations");
            tabPageOrchestrations.Controls.Add(trackingManagerPanelOrchestrations);
            tabControl.TabPages.Add(tabPageOrchestrations);

            #endregion

            #region Create send ports tab

            var trackingManagerPanelSendPorts = new TrackingManagerPanel(ArtifactType.SendPort, ref _controller);

            var tabPageSendPorts = new TabPage("Send Ports");
            tabPageSendPorts.Controls.Add(trackingManagerPanelSendPorts);
            tabControl.TabPages.Add(tabPageSendPorts);

            #endregion

            #region Create receive ports tab

            var trackingManagerPanelReceivePorts = new TrackingManagerPanel(ArtifactType.ReceivePort, ref _controller);

            var tabPageReceivePorts = new TabPage("Receive Ports");
            tabPageReceivePorts.Controls.Add(trackingManagerPanelReceivePorts);
            tabControl.TabPages.Add(tabPageReceivePorts);

            #endregion

            #region Create pipelines tab

            var trackingManagerPanelPipelines = new TrackingManagerPanel(ArtifactType.Pipeline, ref _controller);

            var tabPagePipelines = new TabPage("Pipelines");
            tabPagePipelines.Controls.Add(trackingManagerPanelPipelines);
            tabControl.TabPages.Add(tabPagePipelines);

            #endregion
            
        }

        /// <summary>
        /// Save changed tracking.
        /// </summary>
        private void ButtonSaveClick(object sender, EventArgs e)
        {
            // Check if user wants to save
            if (MessageBox.Show(Resources.TrackingManagerPanel_Are_you_sure_you_want_to_save_these_changes, Resources.BizTalkTrackingManager_Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            // Set cursor
            Cursor.Current = Cursors.WaitCursor;

            // Loop through all panels
            foreach (var trackingManagerPanel in tabControl.TabPages.Cast<TabPage>().SelectMany(tabPage => tabPage.Controls.OfType<TrackingManagerPanel>()))
            {
                // Queue changes
                trackingManagerPanel.QueueChanges();
            }

            // Save all changes
            _controller.SaveChanges();
            MessageBox.Show(Resources.BizTalkTrackingManager_All_changes_have_been_saved, Resources.BizTalkTrackingManager_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
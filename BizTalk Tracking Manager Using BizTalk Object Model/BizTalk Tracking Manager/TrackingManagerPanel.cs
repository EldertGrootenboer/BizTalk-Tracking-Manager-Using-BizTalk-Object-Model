using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using BizTalk_Tracking_Manager.Properties;

using Microsoft.BizTalk.ExplorerOM;

namespace BizTalk_Tracking_Manager
{
    /// <summary>
    /// User control to show tracking for a specific group of components (f.e. orchestrations or ports).
    /// </summary>
    public sealed partial class TrackingManagerPanel : UserControl
    {
        /// <summary>
        /// Type of artifact for this panel.
        /// </summary>
        private readonly ArtifactType _artifactType;

        /// <summary>
        /// Controller used for communication with data layer.
        /// </summary>
        private readonly Controller _controller;

        /// <summary>
        /// Table to show tracking.
        /// </summary>
        private readonly DataGridView _dataGridView = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false };

        /// <summary>
        /// Color of cells for which values have been changed.
        /// </summary>
        private readonly Color _editedCellColor = Color.Yellow;

        /// <summary>
        /// Color of cells which are read only.
        /// </summary>
        private readonly Color _readOnlyCellColor = Color.WhiteSmoke;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TrackingManagerPanel(ArtifactType artifactType, ref Controller controller)
        {
            // Set properties
            _artifactType = artifactType;
            _controller = controller;

            // Initialize components
            InitializeComponent();
            Dock = DockStyle.Fill;

            // Bind to the artifacts
            switch (artifactType)
            {
                case ArtifactType.Orchestration:
                    _dataGridView.DataSource = new BindingSource(new BindingList<Orchestration>(controller.GetOrchestrations().OrderBy(orchestration => orchestration.Name).ToList()), null);
                    break;
                case ArtifactType.SendPort:
                    _dataGridView.DataSource = new BindingSource(new BindingList<SendPort>(controller.GetSendPorts().OrderBy(sendPort => sendPort.Name).ToList()), null);
                    break;
                case ArtifactType.ReceivePort:
                    _dataGridView.DataSource = new BindingSource(new BindingList<ReceivePort>(controller.GetReceivePorts().OrderBy(receivePort => receivePort.Name).ToList()), null);
                    break;
                case ArtifactType.Pipeline:
                    _dataGridView.DataSource = new BindingSource(new BindingList<Pipeline>(controller.GetPipelines().OrderBy(pipeline => pipeline.Name).ToList()), null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
            }

            // Add event handlers
            _dataGridView.DataBindingComplete += OnDataGridViewOnDataBindingComplete;
            _dataGridView.ColumnHeaderMouseClick += OnDataGridViewOnColumnHeaderMouseClick;
            _dataGridView.RowHeaderMouseClick += OnDataGridViewOnRowHeaderMouseClick;
            _dataGridView.CellValueChanged += OnDataGridViewOnCellValueChanged;
            _dataGridView.CellMouseUp += OnDataGridViewOnCellMouseUp;

            // Add table to panel
            Controls.Add(_dataGridView);
        }

        /// <summary>
        /// Save changes to the data layer.
        /// </summary>
        public void QueueChanges()
        {
            try
            {
                // Loop through rows
                foreach (var row in _dataGridView.Rows.Cast<DataGridViewRow>().Where(dataGridViewRow => dataGridViewRow.Cells.Cast<DataGridViewCell>().Any(cell => cell.Style.BackColor == _editedCellColor)))
                {
                    // Check what type of artifact we are working on
                    switch (_artifactType)
                    {
                        case ArtifactType.Orchestration:
                            // Set no tracking
                            OrchestrationTrackingTypes orchestrationTracking = 0;

                            // Loop through checkbox cells which have been checked
                            foreach (var cell in row.Cells.Cast<DataGridViewCell>().Where(cell => cell.ColumnIndex != 0))
                            {
                                // Check if cell is ticked
                                if (Convert.ToBoolean(cell.Value))
                                {
                                    // Used to parse tracking
                                    OrchestrationTrackingTypes columnTracking;

                                    // Parse tracking
                                    Enum.TryParse(cell.OwningColumn.Name, out columnTracking);

                                    // Update tracking
                                    orchestrationTracking = orchestrationTracking | columnTracking;
                                }

                                // Reset cell
                                SetCellNotEdited(cell);
                                cell.Tag = null;
                            }

                            // Queue tracking to be saved
                            _controller.QueueOrchestrationTracking(row.Cells[0].Value.ToString(), orchestrationTracking);
                            break;
                        case ArtifactType.SendPort:
                            // Set no tracking
                            TrackingTypes sendPortTracking = 0;

                            // Loop through checkbox cells which have been checked
                            foreach (var cell in row.Cells.Cast<DataGridViewCell>().Where(cell => cell.ColumnIndex != 0))
                            {
                                // Check if cell is ticked
                                if (Convert.ToBoolean(cell.Value))
                                {
                                    // Used to parse tracking
                                    TrackingTypes columnTracking;

                                    // Parse tracking
                                    Enum.TryParse(cell.OwningColumn.Name, out columnTracking);

                                    // Update tracking
                                    sendPortTracking = sendPortTracking | columnTracking;
                                }

                                // Reset cell
                                SetCellNotEdited(cell);
                                cell.Tag = null;
                            }

                            // Queue tracking to be saved
                            _controller.QueueSendPortTracking(row.Cells[0].Value.ToString(), sendPortTracking);
                            break;
                        case ArtifactType.ReceivePort:
                            // Set no tracking
                            TrackingTypes receivePortTracking = 0;

                            // Loop through checkbox cells which have been checked
                            foreach (var cell in row.Cells.Cast<DataGridViewCell>().Where(cell => cell.ColumnIndex != 0))
                            {
                                // Check if cell is ticked
                                if (Convert.ToBoolean(cell.Value))
                                {
                                    // Used to parse tracking
                                    TrackingTypes columnTracking;

                                    // Parse tracking
                                    Enum.TryParse(cell.OwningColumn.Name, out columnTracking);

                                    // Update tracking
                                    receivePortTracking = receivePortTracking | columnTracking;
                                }

                                // Reset cell
                                SetCellNotEdited(cell);
                                cell.Tag = null;
                            }
                            
                            // Queue tracking to be saved
                            _controller.QueueReceivePortTracking(row.Cells[0].Value.ToString(), receivePortTracking);
                            break;
                        case ArtifactType.Pipeline:
                            // Set no tracking
                            PipelineTrackingTypes pipelineTracking = 0;

                            // Loop through checkbox cells which have been checked
                            foreach (var cell in row.Cells.Cast<DataGridViewCell>().Where(cell => cell.ColumnIndex != 0))
                            {
                                // Check if cell is ticked
                                if (Convert.ToBoolean(cell.Value))
                                {
                                    // Used to parse tracking
                                    PipelineTrackingTypes columnTracking;

                                    // Parse tracking
                                    Enum.TryParse(cell.OwningColumn.Name, out columnTracking);

                                    // Update tracking
                                    pipelineTracking = pipelineTracking | columnTracking;
                                }

                                // Reset cell
                                SetCellNotEdited(cell);
                                cell.Tag = null;
                            }

                            // Queue tracking to be saved
                            _controller.QueuePipelineTracking(row.Cells[0].Value.ToString(), pipelineTracking);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), Resources.TrackingManager_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Once binding is completed, make sure columns are resized.
        /// </summary>
        private void OnDataGridViewOnDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs args)
        {
            // Do not allow artifact names to be edited
            _dataGridView.Columns[0].ReadOnly = true;

            // Loop through all columns
            for (var index = 0; index < _dataGridView.ColumnCount; index++)
            {
                // Column with the artifact names should be full size, the rest of the columns should fill the form
                _dataGridView.Columns[index].AutoSizeMode = index == 0 ? DataGridViewAutoSizeColumnMode.AllCells : DataGridViewAutoSizeColumnMode.Fill;

                // Align headers in the middle
                _dataGridView.Columns[index].HeaderCell.Style = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
            }

            // Make sure column headers are auto sized correctly
            _dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Make sure cells for unused one-way ports are disabled
            if (_artifactType == ArtifactType.ReceivePort)
            {
                // Loop through cells, where the port is not two way
                // For these cells, determine which tracking type they belong to
                foreach (var cell in _dataGridView.Rows.Cast<DataGridViewRow>().Where(row => !((ReceivePort)row.DataBoundItem).IsTwoWay).SelectMany(row => row.Cells.Cast<DataGridViewCell>()
                    .Where(
                        cell =>
                            cell.OwningColumn.Name == "BeforeSendPipeline" || cell.OwningColumn.Name == "AfterSendPipeline" ||
                            cell.OwningColumn.Name == "TrackPropertiesBeforeSendPipeline" || cell.OwningColumn.Name == "TrackPropertiesAfterSendPipeline")))
                {
                    // Make sure cells for unused tracking are disabled
                    SetCellReadOnly(cell);
                }
            }
            else if (_artifactType == ArtifactType.SendPort)
            {
                // Loop through cells, where the port is not two way
                // For these cells, determine which tracking type they belong to
                foreach (var cell in _dataGridView.Rows.Cast<DataGridViewRow>().Where(row => !((SendPort)row.DataBoundItem).IsTwoWay).SelectMany(row => row.Cells.Cast<DataGridViewCell>()
                    .Where(
                        cell =>
                            cell.OwningColumn.Name == "BeforeReceivePipeline" || cell.OwningColumn.Name == "AfterReceivePipeline" ||
                            cell.OwningColumn.Name == "TrackPropertiesBeforeReceivePipeline" || cell.OwningColumn.Name == "TrackPropertiesAfterReceivePipeline")))
                {
                    // Make sure cells for unused tracking are disabled
                    SetCellReadOnly(cell);
                }
            }
        }

        /// <summary>
        /// When clicking column headers, toggle state of that tracking for all artifacts.
        /// </summary>
        private void OnDataGridViewOnColumnHeaderMouseClick(object o, DataGridViewCellMouseEventArgs args)
        {
            // Ignore column with artifact names
            if (args.ColumnIndex == 0)
            {
                return;
            }

            // Get value of first row
            var current = Convert.ToBoolean(_dataGridView[args.ColumnIndex, 1].Value);

            // Set values for all rows
            foreach (var cell in _dataGridView.Rows.Cast<DataGridViewRow>().Select(row => row.Cells[args.ColumnIndex]).Where(cell => !cell.ReadOnly))
            {
                cell.Value = !current;
            }
        }

        /// <summary>
        /// When clicking row headers, toggle state for all tracking of that artifact.
        /// </summary>
        private void OnDataGridViewOnRowHeaderMouseClick(object o, DataGridViewCellMouseEventArgs args)
        {
            // Get value of first column
            var current = Convert.ToBoolean(_dataGridView[1, args.RowIndex].Value);

            // Set values for all columns
            foreach (var cell in _dataGridView.Columns.Cast<DataGridViewColumn>().Where(column => column.Index != 0).Select(column => _dataGridView[column.Index, args.RowIndex]).Where(cell => !cell.ReadOnly))
            {
                cell.Value = !current;
            }
        }

        /// <summary>
        /// Handle check box checked event.
        /// </summary>
        private void OnDataGridViewOnCellValueChanged(object sender, DataGridViewCellEventArgs args)
        {
            // Ignore first column
            if (args.ColumnIndex > 0 && args.RowIndex != -1)
            {
                // Get changed cell
                var cell = _dataGridView[args.ColumnIndex, args.RowIndex];

                // Check if tag was allready set
                if (cell.Tag == null)
                {
                    // If not, set the value to the original value
                    cell.Tag = !Convert.ToBoolean(cell.Value);
                }

                // Check if value has been changed from original value
                if ((bool)cell.Value != (bool)cell.Tag)
                {
                    // Value has been changed
                    SetCellEdited(cell);
                }
                else
                {
                    // Value is same as original value
                    SetCellNotEdited(cell);
                }
            }
        }

        /// <summary>
        /// Set the specified cell as readonly.
        /// </summary>
        private void SetCellReadOnly(DataGridViewCell cell)
        {
            cell.ReadOnly = true;
            ((DataGridViewCheckBoxCell)cell).ThreeState = true;
            ((DataGridViewCheckBoxCell)cell).IndeterminateValue = false;
            SetCellColors(cell, _readOnlyCellColor, _readOnlyCellColor);
        }

        /// <summary>
        /// Set the specified cell as edited.
        /// </summary>
        private void SetCellEdited(DataGridViewCell cell)
        {
            SetCellColors(cell, _editedCellColor, _editedCellColor);
        }

        /// <summary>
        /// Set the specified cell as not edited.
        /// </summary>
        private void SetCellNotEdited(DataGridViewCell cell)
        {
            SetCellColors(cell, _dataGridView[0, 0].Style.BackColor, _dataGridView[0, 0].Style.SelectionBackColor);
        }

        /// <summary>
        /// Set the colors of the specified cell.
        /// </summary>
        private static void SetCellColors(DataGridViewCell cell, Color backColor, Color selectionBackColor)
        {
            cell.Style.BackColor = backColor;
            cell.Style.SelectionBackColor = selectionBackColor;
        }

        /// <summary>
        /// Needed to handle check box events.
        /// </summary>
        private void OnDataGridViewOnCellMouseUp(object sender, DataGridViewCellMouseEventArgs args)
        {
            // End of editing on each click on column of checkbox
            if (args.ColumnIndex > 0 && args.RowIndex != -1)
            {
                _dataGridView.EndEdit();
            }
        }
    }
}

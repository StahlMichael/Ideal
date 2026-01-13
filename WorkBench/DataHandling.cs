
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WorkBench
{
    public partial class DataHandling : Form
    {
        private GilboaAutomation gilboaAutomation = new GilboaAutomation();
        private Process? nodeProcess;
        private static SocketIOClient.SocketIO? socket; // Make static to share across all instances
        internal static FlaUI.Core.Application? app;
        private List<(IntPtr hWnd, string Title)> windows = new List<(IntPtr, string)>();
        internal static int doket = 0;
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        internal DataHandling()
        {
            InitializeComponent();
        }
        private void DataHandling_Load(object sender, EventArgs e)
        {
            bool serverRunning = IsServerAlreadyRunning();
            //btnStartServer.Enabled = !serverRunning;
            SetupRequestsGrid();

            // Connect to server if it's already running
            if (serverRunning)
            {
                btnStopServer.Enabled = true;
                ConnectToServer();
            }
        }

        #region node
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (IsServerAlreadyRunning())
                return;
            LaunchNodeProcess();
            HandleServerStartup();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            var taskkill = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = "/F /IM node.exe /T",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            taskkill.Start();
            taskkill.WaitForExit();
            if (taskkill.ExitCode != 0)
            {
                //btnStartServer.Enabled = true;
                //btnStopServer.Enabled = false;
            }

        }

        private async void ConnectToServer()
        {
            // Prevent multiple connections - check if already connected
            if (socket != null && socket.Connected)
                return;

            // Prevent race condition with lock
            lock (typeof(DataHandling))
            {
                if (socket != null && socket.Connected)
                    return;

                // Disconnect any existing socket before creating a new one
                if (socket != null)
                {
                    socket.DisconnectAsync().Wait();
                    socket.Dispose();
                }

                var options = new SocketIOClient.SocketIOOptions();
                socket = new SocketIOClient.SocketIO("http://localhost:3000", options);

                socket.On("newOrder", response =>
                {
                    var data = response.GetValue<JsonElement>(0);

                    // Extract the new request ID if available
                    string? newRequestId = null;
                    if (data.TryGetProperty("requestId", out JsonElement requestIdElement))
                    {
                        newRequestId = requestIdElement.GetString();
                    }

                    // Find all open DataHandling forms and update them
                    foreach (Form openForm in Application.OpenForms)
                    {
                        if (openForm is DataHandling dh && !dh.IsDisposed)
                        {
                            dh.Invoke((MethodInvoker)delegate
                            {
                                dh.LoadRequestLog(newRequestId);
                            });
                        }
                    }
                });
            }

            await socket.ConnectAsync();
        }

        private void DataHandling_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Only disconnect if no other DataHandling forms are open
            bool hasOtherDataHandlingForms = false;
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is DataHandling && openForm != this && !openForm.IsDisposed)
                {
                    hasOtherDataHandlingForms = true;
                    break;
                }
            }

            if (!hasOtherDataHandlingForms)
            {
                socket?.DisconnectAsync();
            }
        }

        private bool IsServerAlreadyRunning()
        {
            var existingNodeProcesses = Process.GetProcessesByName("node");
            return (existingNodeProcesses.Length > 0);

        }

        private void LaunchNodeProcess()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\nodejs\npm.cmd",
                Arguments = "run dev",
                WorkingDirectory = @"G:\michaelstahl\current_programs\WorkBench\node project\application-api",
                CreateNoWindow = false,
                UseShellExecute = true
            };

            nodeProcess = Process.Start(startInfo);
        }

        private void HandleServerStartup()
        {
            if (nodeProcess == null)
                return;

            // Wait a moment for the server to start
            System.Threading.Thread.Sleep(2000);

            if (nodeProcess.HasExited)
            {
                MessageBox.Show("Server failed to start! Check the console window for error details.",
                    "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //btnStopServer.Enabled = true;
                //btnStartServer.Enabled = false;
                Thread.Sleep(2000);
                ConnectToServer();
            }
        }

        private PaymentInfo GetPaymentInfo()
        {
            return new PaymentInfo
            {
                Token = txtPaymentToken.Text,
                ApprovalNum = txtApprovalNum.Text,
                CardName = txtCardName.Text,
                ExpiryDate = txtExpiryDate.Text,
                Amount = txtAmount.Text,
                PaymentDateTime = DateTime.TryParse(txtPaymentDateTime.Text, out var dt) ? dt : DateTime.Now
            };
        }

        private void HandleError(Exception ex, string userMessage)
        {
            var stackTrace = new System.Diagnostics.StackTrace(ex, true);
            var frame = stackTrace.GetFrame(0);
            int lineNumber = frame?.GetFileLineNumber() ?? 0;

            // Get the calling method name (go up 1 frame from this helper method)
            var callingFrame = new System.Diagnostics.StackTrace(true).GetFrame(1);
            string methodName = callingFrame?.GetMethod()?.Name ?? "Unknown";

            string errorInfo = $"Method: {methodName}\nLine: {lineNumber}\nError: {ex.Message}";
            Clipboard.SetText(errorInfo);
            MessageBox.Show(userMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region dataControl
        private void SetupRequestsGrid()
        {
            dgvRequestsStatus.Columns.Clear();
            
            // Add columns with sorting disabled to preserve database order
            DataGridViewColumn colRequestId = new DataGridViewTextBoxColumn();
            colRequestId.Name = "RequestId";
            colRequestId.HeaderText = "Request ID";
            colRequestId.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvRequestsStatus.Columns.Add(colRequestId);
            
            DataGridViewColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "Status";
            colStatus.HeaderText = "Status";
            colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvRequestsStatus.Columns.Add(colStatus);
            
            DataGridViewColumn colDateTime = new DataGridViewTextBoxColumn();
            colDateTime.Name = "RequestDateTime";
            colDateTime.HeaderText = "Date/Time";
            colDateTime.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvRequestsStatus.Columns.Add(colDateTime);
            
            DataGridViewColumn colUser = new DataGridViewTextBoxColumn();
            colUser.Name = "User";
            colUser.HeaderText = "User";
            colUser.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvRequestsStatus.Columns.Add(colUser);
            
            dgvRequestsStatus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadRequestLog();
        }

        private void LoadRequestLog(string? highlightRequestId = null)
        {
            try
            {
                // Store current selection if any (but don't restore if we're highlighting a new request)
                string? selectedRequestId = null;
                if (highlightRequestId == null && dgvRequestsStatus.CurrentRow != null && dgvRequestsStatus.CurrentRow.Cells["RequestId"].Value != null)
                {
                    selectedRequestId = dgvRequestsStatus.CurrentRow.Cells["RequestId"].Value.ToString();
                }

                string connectionString = "Server=SQL-SERVER\\GILBOA;Database=SKI;User Id=Michael;Password=2585;TrustServerCertificate=True;";
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT RequestId, Status, RequestDateTime, [User] 
                                    FROM tblRequestLog 
                                    ORDER BY CONVERT(DATETIME, RequestDateTime, 103) DESC";

                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    using (var adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvRequestsStatus.SuspendLayout();
                        dgvRequestsStatus.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            int rowIndex = dgvRequestsStatus.Rows.Add(
                                row["RequestId"],
                                row["Status"],
                                row["RequestDateTime"],
                                row["User"]
                            );

                            // Color code based on status
                            string status = row["Status"]?.ToString()?.ToLower() ?? "";
                            if (status == "accepted")
                            {
                                dgvRequestsStatus.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                                dgvRequestsStatus.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
                            }
                            else if (status == "denied")
                            {
                                dgvRequestsStatus.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                                dgvRequestsStatus.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.Red;
                            }
                        }
                        dgvRequestsStatus.ResumeLayout();
                        dgvRequestsStatus.Refresh();
                    }
                }

                // Highlight the new request if specified
                if (!string.IsNullOrEmpty(highlightRequestId))
                {
                    foreach (DataGridViewRow row in dgvRequestsStatus.Rows)
                    {
                        if (row.Cells["RequestId"].Value?.ToString() == highlightRequestId)
                        {
                            row.Selected = true;
                            dgvRequestsStatus.CurrentCell = row.Cells[0];
                            dgvRequestsStatus.FirstDisplayedScrollingRowIndex = row.Index;
                            // Trigger selection changed to update other grids
                            dgvRequestsStatus_SelectionChanged(dgvRequestsStatus, EventArgs.Empty);
                            break;
                        }
                    }
                }
                // Restore previous selection if no new highlight
                else if (!string.IsNullOrEmpty(selectedRequestId))
                {
                    foreach (DataGridViewRow row in dgvRequestsStatus.Rows)
                    {
                        if (row.Cells["RequestId"].Value?.ToString() == selectedRequestId)
                        {
                            row.Selected = true;
                            dgvRequestsStatus.CurrentCell = row.Cells[0];
                            break;
                        }
                    }
                }
                // If no highlight and no previous selection, select the first row (newest request)
                else if (dgvRequestsStatus.Rows.Count > 0)
                {
                    dgvRequestsStatus.Rows[0].Selected = true;
                    dgvRequestsStatus.CurrentCell = dgvRequestsStatus.Rows[0].Cells[0];
                    dgvRequestsStatus.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error loading request log: {ex.Message}");
            }
        }
        private void dgvRequestsStatus_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Check if the selected request is denied
                if (dgvRequestsStatus.CurrentRow != null && dgvRequestsStatus.CurrentRow.Cells["Status"].Value != null)
                {
                    string status = dgvRequestsStatus.CurrentRow.Cells["Status"].Value.ToString().ToLower();

                    if (status == "denied")
                    {
                        // Clear the data when request is denied
                        ClearOrderAndPaymentInfo();
                        dgvPax.DataSource = null;
                        dgvFiles.DataSource = null;
                        ClearPaxDetails();
                        ClearFileDetails();
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(f.GetSkiConnectionString()))
                {
                    conn.Open();

                    // Check if a row is selected
                    if (dgvRequestsStatus.CurrentRow != null && dgvRequestsStatus.CurrentRow.Cells["RequestId"].Value != null)
                    {
                        // Get the selected RequestId
                        string requestId = dgvRequestsStatus.CurrentRow.Cells["RequestId"].Value.ToString();

                        // Load order information
                        LoadOrderInfo(requestId, conn);

                        // Load pax and files
                        LoadPaxAndFiles(requestId, conn);

                        // Load payment information
                        LoadPaymentInfo(requestId, conn);
                    }
                    else
                    {
                        ClearOrderAndPaymentInfo();
                        dgvPax.DataSource = null;
                        dgvFiles.DataSource = null;
                    }
                }
            }
            catch { }
        }

        private void LoadOrderInfo(string requestId, SqlConnection conn)
        {
            string sql = "SELECT * FROM tblOrders WHERE RequestId = @RequestId";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtOrderRequestId.Text = reader["RequestId"].ToString();
                        txtVacationDate.Text = reader["vacationDate"].ToString();
                        txtTotalRooms.Text = reader["totalRooms"].ToString();
                        txtRoomType.Text = reader["roomType"].ToString();
                        txtAddress.Text = reader["address"].ToString();
                        txtPhone.Text = reader["phone"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        txtNotes.Text = reader["notes"].ToString();
                        chkTermsAccepted.Checked = reader["termsAccepted"] != DBNull.Value && (bool)reader["termsAccepted"];
                        txtSubmissionDate.Text = reader["submissionDate"].ToString();
                    }
                    else
                    {
                        ClearOrderAndPaymentInfo();
                    }
                }
            }
        }

        private void LoadPaxAndFiles(string requestId, SqlConnection conn)
        {
            // tblPax
            DataTable dtPax = new DataTable();
            string sqlPax = "SELECT * FROM tblPax WHERE RequestId = @RequestId";
            using (SqlCommand cmd = new SqlCommand(sqlPax, conn))
            {
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                    dtPax.Load(reader);
            }
            dgvPax.DataSource = dtPax;
            FormatPaxGrid();

            // Store dtPax for later use (store in Tag property)
            dgvPax.Tag = dtPax;
            
            // Clear passenger details if no rows
            if (dtPax.Rows.Count == 0)
            {
                ClearPaxDetails();
            }

            // tblFiles
            DataTable dtFiles = new DataTable();
            string sqlFiles = "SELECT * FROM tblFiles WHERE RequestId = @RequestId";
            using (SqlCommand cmd = new SqlCommand(sqlFiles, conn))
            {
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                    dtFiles.Load(reader);
            }
            dgvFiles.DataSource = dtFiles;
            FormatFilesGrid();
            
            // Clear file details if no rows
            if (dtFiles.Rows.Count == 0)
            {
                ClearFileDetails();
            }
        }

        private void LoadPaymentInfo(string requestId, SqlConnection conn)
        {
            string sql = "SELECT * FROM tblCC_Payment WHERE RequestId = @RequestId";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtPaymentToken.Text = reader["token"].ToString();
                        txtApprovalNum.Text = reader["approveNum"].ToString();
                        txtCardName.Text = reader["cardName"].ToString();
                        txtExpiryDate.Text = reader["validDate"].ToString();
                        txtAmount.Text = reader["amount"] != DBNull.Value ? ((decimal)reader["amount"]).ToString("N2") : "";
                        txtPaymentDateTime.Text = reader["paymentDateTime"] != DBNull.Value ? ((DateTime)reader["paymentDateTime"]).ToString("dd/MM/yyyy HH:mm") : "";
                    }
                    else
                    {
                        ClearPaymentInfo();
                    }
                }
            }
        }

        private void ClearOrderAndPaymentInfo()
        {
            txtOrderRequestId.Text = "";
            txtVacationDate.Text = "";
            txtTotalRooms.Text = "";
            txtRoomType.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtNotes.Text = "";
            chkTermsAccepted.Checked = false;
            txtSubmissionDate.Text = "";
            ClearPaymentInfo();
        }

        private void ClearPaymentInfo()
        {
            txtPaymentToken.Text = "";
            txtApprovalNum.Text = "";
            txtCardName.Text = "";
            txtExpiryDate.Text = "";
            txtAmount.Text = "";
            txtPaymentDateTime.Text = "";
        }

        private void FormatPaxGrid()
        {
            if (dgvPax.Columns.Count == 0) return;

            // Hide all columns except first and last name
            dgvPax.Columns["PaxId"].Visible = false;
            dgvPax.Columns["RequestId"].Visible = false;
            dgvPax.Columns["tempId"].Visible = false;
            dgvPax.Columns["lastName"].HeaderText = "Last Name";
            dgvPax.Columns["firstName"].HeaderText = "First Name";
            dgvPax.Columns["birthDate"].Visible = false;
            dgvPax.Columns["passportNumber"].Visible = false;
            dgvPax.Columns["gender"].Visible = false;
            dgvPax.Columns["noFlight"].Visible = false;
            dgvPax.Columns["skiPass"].Visible = false;
            dgvPax.Columns["skiLevel"].Visible = false;
            dgvPax.Columns["hasNumber"].Visible = false;
            dgvPax.Columns["frequentFlyerNumber"].Visible = false;

            dgvPax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void FormatFilesGrid()
        {
            if (dgvFiles.Columns.Count == 0) return;

            // Hide all columns except fileName
            dgvFiles.Columns["FileId"].Visible = false;
            dgvFiles.Columns["RequestId"].Visible = false;
            dgvFiles.Columns["fileName"].HeaderText = "File Name";
            dgvFiles.Columns["fileType"].Visible = false;
            dgvFiles.Columns["fileSize"].Visible = false;

            if (dgvFiles.Columns.Contains("folderUrl"))
            {
                dgvFiles.Columns["folderUrl"].Visible = false;
            }

            dgvFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void dgvPax_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvPax.CurrentRow != null && dgvPax.CurrentRow.DataBoundItem is DataRowView rowView)
                {
                    DataRow row = rowView.Row;
                    
                    // Populate textboxes with passenger details
                    if (row["birthDate"] != DBNull.Value && DateTime.TryParse(row["birthDate"]?.ToString(), out DateTime birthDate))
                    {
                        txtPaxBirthDate.Text = birthDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtPaxBirthDate.Text = "";
                    }
                    txtPaxPassport.Text = row["passportNumber"]?.ToString() ?? "";
                    txtPaxGender.Text = row["gender"]?.ToString() ?? "";
                    chkPaxNoFlight.Checked = row["noFlight"] != DBNull.Value && Convert.ToBoolean(row["noFlight"]);
                    chkPaxSkiPass.Checked = row["skiPass"] != DBNull.Value && Convert.ToBoolean(row["skiPass"]);
                    txtPaxSkiLevel.Text = row["skiLevel"]?.ToString() ?? "";
                    chkPaxHasFFNumber.Checked = row["hasNumber"] != DBNull.Value && Convert.ToBoolean(row["hasNumber"]);
                    txtPaxFFNumber.Text = row["frequentFlyerNumber"]?.ToString() ?? "";
                }
                else
                {
                    ClearPaxDetails();
                }
            }
            catch { }
        }

        private void ClearPaxDetails()
        {
            txtPaxBirthDate.Text = "";
            txtPaxPassport.Text = "";
            txtPaxGender.Text = "";
            chkPaxNoFlight.Checked = false;
            chkPaxSkiPass.Checked = false;
            txtPaxSkiLevel.Text = "";
            chkPaxHasFFNumber.Checked = false;
            txtPaxFFNumber.Text = "";
        }

        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvFiles.CurrentRow != null && dgvFiles.CurrentRow.DataBoundItem is DataRowView rowView)
                {
                    DataRow row = rowView.Row;
                    
                    // Populate textboxes with file details
                    txtFileType.Text = row["fileType"]?.ToString() ?? "";
                    txtFileSize.Text = row["fileSize"]?.ToString() ?? "";
                    txtFileFolderUrl.Text = row["folderUrl"]?.ToString() ?? "";
                    txtFileFolderUrl.ForeColor = Color.Blue;
                }
                else
                {
                    ClearFileDetails();
                }
            }
            catch { }
        }

        private void ClearFileDetails()
        {
            txtFileType.Text = "";
            txtFileSize.Text = "";
            txtFileFolderUrl.Text = "";
        }

        private void txtFileFolderUrl_Click(object sender, EventArgs e)
        {
            string url = txtFileFolderUrl.Text;
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Check if the clicked column is folderUrl
            if (dgvFiles.Columns[e.ColumnIndex].Name == "folderUrl")
            {
                var url = dgvFiles.Rows[e.RowIndex].Cells["folderUrl"].Value?.ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        HandleError(ex, $"Failed to open URL: {ex.Message}");
                    }
                }
            }
        }

        #endregion

        #region gilboa
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            windows = gilboaAutomation.GetOpenWindows();
            listBox1.Items.Clear();
            for (int i = 0; i < windows.Count; i++)
            {
                listBox1.Items.Add($"{i + 1}. {windows[i].Title} - {windows[i].hWnd}");
            }
        }

        private void btnOpenGilboa_Click(object sender, EventArgs e)
        {
            try
            {
                gilboaAutomation.OpenOrAttachGilboa();
                txtDoket.Text = gilboaAutomation.CurrentDoket.ToString();
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error opening Gilboa: {ex.Message}");
            }
        }

        private void btnShowCoordinates_Click(object sender, EventArgs e)
        {
            MouseTrackerForm mouseTracker = new MouseTrackerForm();
            mouseTracker.ShowDialog();
        }

        private void btnShowControls_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex >= 0)
            {
                var selectedWindow = windows[selectedIndex];
                txtResult.Text = gilboaAutomation.GetControlsInfo(selectedWindow.hWnd);
            }
        }

        private void btnOpenNewDoketsWithPax_Click(object sender, EventArgs e)
        {
            try
            {
                var passengers = new List<string>();

                // Get passenger data from dgvPax Tag (stored DataTable)
                if (dgvPax.Tag is DataTable dtPax && dtPax.Rows.Count > 0)
                {
                    foreach (DataRow row in dtPax.Rows)
                    {
                        string lastName = row["lastName"]?.ToString() ?? "";
                        string firstName = row["firstName"]?.ToString() ?? "";
                        string birthDateStr = row["birthDate"]?.ToString() ?? "";
                        string passportNumber = row["passportNumber"]?.ToString() ?? "";
                        
                        // Format birthdate as dd/MM/yy
                        string formattedBirthDate = "";
                        if (!string.IsNullOrWhiteSpace(birthDateStr) && DateTime.TryParse(birthDateStr, out DateTime birthDate))
                        {
                            formattedBirthDate = birthDate.ToString("dd/MM/yy");
                        }
                        
                        // Format: LastName/FirstName|BirthDate|PassportNumber
                        passengers.Add($"{lastName.Trim()}/{firstName.Trim()}|{formattedBirthDate}|{passportNumber.Trim()}");
                    }
                }
                else
                {
                    MessageBox.Show("No passengers found. Please select a request first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gilboaAutomation.OpenNewDoketWithPax("MARGANIT", passengers);
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error opening new doket: {ex.Message}");
            }
        }

        private void btnOpenExistingDoket_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtDoket.Text, out int doketNumber))
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }

            try
            {
                gilboaAutomation.OpenExistingDoket(doketNumber);
                txtDoket.Text = txtDoket.Text;
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error opening doket: {ex.Message}");
            }
        }
        private void txtDoket_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnOpenExistingDoket.PerformClick();
        }
        private void btnCreateVoucher_Click(object sender, EventArgs e)
        {
            try
            {
                string seasonVoucher = "SKI WINTER";
                string packageName = txtVacationDate.Text;
                var parts = packageName.Split('-');
                string startDate = DateTime.ParseExact(parts[0].Trim(), "dd/MM/yyyy", null).ToString("ddMMMyy");
                string endDate = DateTime.ParseExact(parts[1].Trim(), "dd/MM/yyyy", null).ToString("ddMMMyy");
                gilboaAutomation.CreateVoucher($"{packageName} חבילה", startDate, endDate, seasonVoucher, seasonVoucher);
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error creating voucher: {ex.Message}");
            }
        }

        #endregion

        private List<int> lstAutomation = new List<int>();
        private void btnFindAllAutomationId_Click(object sender, EventArgs e)
        {
            var matches = Regex.Matches(txtResult.Text, @"AutomationId:\s*(\d{5})");
            foreach (Match match in matches)
            {
                lstAutomation.Add(Convert.ToInt32(match.Groups[1].Value)); // Only the digits
                lstAutomation.RemoveAll(x => x == 11634 || x == 65535);
            }

        }

        private void btnEnterInDoket_Click(object sender, EventArgs e)
         {
            try
            {
                // Open Gilboa if not already open
                gilboaAutomation.OpenOrAttachGilboa();
                btnOpenNewDoketsWithPax.PerformClick();
                btnCreateVoucher.PerformClick();
                 gilboaAutomation.CreatePayment(GetPaymentInfo());
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error: {ex.Message}");
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelToSqlExample.ImportFolderToOneTable();
                MessageBox.Show("Excel import completed successfully!", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing Excel files: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

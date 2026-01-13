

namespace WorkBench
{
    partial class DataHandling
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStartServer = new Button();
            btnStopServer = new Button();
            btnImportExcel = new Button();
            dgvRequestsStatus = new DataGridView();
            dgvPax = new DataGridView();
            dgvFiles = new DataGridView();
            btnCreateVoucher = new Button();
            btnOpenGilboa = new Button();
            btnOpenNewDoketsWithPax = new Button();
            btnShowCoordinates = new Button();
            label1 = new Label();
            btnRefresh = new Button();
            txtDoket = new TextBox();
            listBox1 = new ListBox();
            btnOpenExistingDoket = new Button();
            btnShowControls = new Button();
            txtResult = new TextBox();
            txtOrderRequestId = new TextBox();
            txtVacationDate = new TextBox();
            txtTotalRooms = new TextBox();
            txtRoomType = new TextBox();
            txtAddress = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtNotes = new TextBox();
            chkTermsAccepted = new CheckBox();
            txtSubmissionDate = new TextBox();
            txtPaymentToken = new TextBox();
            txtApprovalNum = new TextBox();
            txtCardName = new TextBox();
            txtExpiryDate = new TextBox();
            txtAmount = new TextBox();
            txtPaymentDateTime = new TextBox();
            grpOrderInfo = new GroupBox();
            lblRequestId = new Label();
            lblVacationDate = new Label();
            lblTotalRooms = new Label();
            lblRoomType = new Label();
            lblAddress = new Label();
            lblPhone = new Label();
            lblEmail = new Label();
            lblNotes = new Label();
            lblSubmissionDate = new Label();
            grpPaymentInfo = new GroupBox();
            lblToken = new Label();
            lblApprovalNum = new Label();
            lblCardName = new Label();
            lblValidDate = new Label();
            lblAmount = new Label();
            lblPaymentDateTime = new Label();
            functions = new GroupBox();
            btnFindAllAutomationId = new Button();
            btnCreatePayment = new Button();
            btnEnterInDoket = new Button();
            txtPaxBirthDate = new TextBox();
            txtPaxPassport = new TextBox();
            txtPaxGender = new TextBox();
            chkPaxNoFlight = new CheckBox();
            chkPaxSkiPass = new CheckBox();
            txtPaxSkiLevel = new TextBox();
            chkPaxHasFFNumber = new CheckBox();
            txtPaxFFNumber = new TextBox();
            grpPaxDetails = new GroupBox();
            lblPaxBirthDate = new Label();
            lblPaxPassport = new Label();
            lblPaxGender = new Label();
            lblPaxSkiLevel = new Label();
            txtFileType = new TextBox();
            txtFileSize = new TextBox();
            txtFileFolderUrl = new TextBox();
            grpFileDetails = new GroupBox();
            lblFileType = new Label();
            lblFileSize = new Label();
            lblFileFolderUrl = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRequestsStatus).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvFiles).BeginInit();
            grpOrderInfo.SuspendLayout();
            grpPaymentInfo.SuspendLayout();
            functions.SuspendLayout();
            grpPaxDetails.SuspendLayout();
            grpFileDetails.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartServer
            // 
            btnStartServer.BackColor = Color.FromArgb(255, 192, 128);
            btnStartServer.Font = new Font("Segoe UI", 15F);
            btnStartServer.Location = new Point(54, 12);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(194, 45);
            btnStartServer.TabIndex = 1;
            btnStartServer.Text = "START SERVER";
            btnStartServer.UseVisualStyleBackColor = false;
            btnStartServer.Click += btnStartServer_Click;
            // 
            // btnStopServer
            // 
            btnStopServer.BackColor = Color.FromArgb(255, 192, 128);
            btnStopServer.Font = new Font("Segoe UI", 15F);
            btnStopServer.Location = new Point(254, 12);
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new Size(194, 45);
            btnStopServer.TabIndex = 2;
            btnStopServer.Text = "STOP SERVER";
            btnStopServer.UseVisualStyleBackColor = false;
            btnStopServer.Click += btnStopServer_Click;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = Color.FromArgb(128, 255, 128);
            btnImportExcel.Font = new Font("Segoe UI", 15F);
            btnImportExcel.Location = new Point(454, 12);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(263, 45);
            btnImportExcel.TabIndex = 3;
            btnImportExcel.Text = "IMPORT EXCEL FOLDER";
            btnImportExcel.UseVisualStyleBackColor = false;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // dgvRequestsStatus
            // 
            dgvRequestsStatus.BackgroundColor = Color.FromArgb(255, 224, 192);
            dgvRequestsStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRequestsStatus.Location = new Point(8, 79);
            dgvRequestsStatus.Name = "dgvRequestsStatus";
            dgvRequestsStatus.Size = new Size(721, 489);
            dgvRequestsStatus.TabIndex = 8;
            dgvRequestsStatus.SelectionChanged += dgvRequestsStatus_SelectionChanged;
            // 
            // dgvPax
            // 
            dgvPax.BackgroundColor = Color.FromArgb(255, 224, 192);
            dgvPax.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPax.Location = new Point(735, 12);
            dgvPax.Name = "dgvPax";
            dgvPax.Size = new Size(300, 213);
            dgvPax.TabIndex = 11;
            dgvPax.SelectionChanged += dgvPax_SelectionChanged;
            // 
            // dgvFiles
            // 
            dgvFiles.BackgroundColor = Color.FromArgb(255, 224, 192);
            dgvFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFiles.Location = new Point(735, 241);
            dgvFiles.Name = "dgvFiles";
            dgvFiles.Size = new Size(300, 114);
            dgvFiles.TabIndex = 10;
            dgvFiles.CellContentClick += dgvFiles_CellContentClick;
            dgvFiles.SelectionChanged += dgvFiles_SelectionChanged;
            // 
            // btnCreateVoucher
            // 
            btnCreateVoucher.Font = new Font("Times New Roman", 12F);
            btnCreateVoucher.Location = new Point(6, 93);
            btnCreateVoucher.Name = "btnCreateVoucher";
            btnCreateVoucher.Size = new Size(150, 29);
            btnCreateVoucher.TabIndex = 16;
            btnCreateVoucher.Text = "Create Voucher";
            btnCreateVoucher.UseVisualStyleBackColor = true;
            btnCreateVoucher.Click += btnCreateVoucher_Click;
            // 
            // btnOpenGilboa
            // 
            btnOpenGilboa.Font = new Font("Times New Roman", 12F);
            btnOpenGilboa.Location = new Point(6, 23);
            btnOpenGilboa.Name = "btnOpenGilboa";
            btnOpenGilboa.Size = new Size(150, 29);
            btnOpenGilboa.TabIndex = 14;
            btnOpenGilboa.Text = "Open Gilboa";
            btnOpenGilboa.UseVisualStyleBackColor = true;
            btnOpenGilboa.Click += btnOpenGilboa_Click;
            // 
            // btnOpenNewDoketsWithPax
            // 
            btnOpenNewDoketsWithPax.Font = new Font("Times New Roman", 12F);
            btnOpenNewDoketsWithPax.Location = new Point(6, 58);
            btnOpenNewDoketsWithPax.Name = "btnOpenNewDoketsWithPax";
            btnOpenNewDoketsWithPax.Size = new Size(150, 29);
            btnOpenNewDoketsWithPax.TabIndex = 15;
            btnOpenNewDoketsWithPax.Text = "Open New Doket";
            btnOpenNewDoketsWithPax.UseVisualStyleBackColor = true;
            btnOpenNewDoketsWithPax.Click += btnOpenNewDoketsWithPax_Click;
            // 
            // btnShowCoordinates
            // 
            btnShowCoordinates.Font = new Font("Segoe UI", 10F);
            btnShowCoordinates.Location = new Point(312, 22);
            btnShowCoordinates.Name = "btnShowCoordinates";
            btnShowCoordinates.Size = new Size(226, 42);
            btnShowCoordinates.TabIndex = 6;
            btnShowCoordinates.Text = "SHOW COORDINATES";
            btnShowCoordinates.UseVisualStyleBackColor = true;
            btnShowCoordinates.Click += btnShowCoordinates_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(255, 224, 192);
            label1.Font = new Font("Times New Roman", 12F);
            label1.Location = new Point(162, 28);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 0, 10, 0);
            label1.Size = new Size(110, 19);
            label1.TabIndex = 11;
            label1.Text = "Current Doket:";
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.Location = new Point(338, 70);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(200, 36);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh List";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // txtDoket
            // 
            txtDoket.BackColor = Color.FromArgb(255, 224, 192);
            txtDoket.Font = new Font("Segoe UI", 13F);
            txtDoket.Location = new Point(192, 56);
            txtDoket.Name = "txtDoket";
            txtDoket.Size = new Size(73, 31);
            txtDoket.TabIndex = 10;
            txtDoket.TextAlign = HorizontalAlignment.Center;
            txtDoket.KeyPress += txtDoket_KeyPress;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(338, 112);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(200, 64);
            listBox1.TabIndex = 2;
            // 
            // btnOpenExistingDoket
            // 
            btnOpenExistingDoket.BackColor = Color.FromArgb(255, 224, 192);
            btnOpenExistingDoket.Font = new Font("Times New Roman", 12F);
            btnOpenExistingDoket.Location = new Point(171, 92);
            btnOpenExistingDoket.Name = "btnOpenExistingDoket";
            btnOpenExistingDoket.Size = new Size(138, 30);
            btnOpenExistingDoket.TabIndex = 9;
            btnOpenExistingDoket.Text = "Open Exist. Doket";
            btnOpenExistingDoket.TextAlign = ContentAlignment.TopCenter;
            btnOpenExistingDoket.UseVisualStyleBackColor = false;
            btnOpenExistingDoket.Click += btnOpenExistingDoket_Click;
            // 
            // btnShowControls
            // 
            btnShowControls.Font = new Font("Times New Roman", 12F);
            btnShowControls.Location = new Point(9, 195);
            btnShowControls.Name = "btnShowControls";
            btnShowControls.Size = new Size(256, 36);
            btnShowControls.TabIndex = 7;
            btnShowControls.Text = "Show all controls for this page";
            btnShowControls.UseVisualStyleBackColor = true;
            btnShowControls.Click += btnShowControls_Click;
            // 
            // txtResult
            // 
            txtResult.Font = new Font("Times New Roman", 12F);
            txtResult.Location = new Point(9, 237);
            txtResult.Multiline = true;
            txtResult.Name = "txtResult";
            txtResult.Size = new Size(529, 64);
            txtResult.TabIndex = 5;
            // 
            // txtOrderRequestId
            // 
            txtOrderRequestId.Location = new Point(16, 48);
            txtOrderRequestId.Name = "txtOrderRequestId";
            txtOrderRequestId.ReadOnly = true;
            txtOrderRequestId.Size = new Size(100, 23);
            txtOrderRequestId.TabIndex = 19;
            // 
            // txtVacationDate
            // 
            txtVacationDate.Location = new Point(126, 48);
            txtVacationDate.Name = "txtVacationDate";
            txtVacationDate.ReadOnly = true;
            txtVacationDate.Size = new Size(208, 23);
            txtVacationDate.TabIndex = 20;
            // 
            // txtTotalRooms
            // 
            txtTotalRooms.Location = new Point(355, 48);
            txtTotalRooms.Name = "txtTotalRooms";
            txtTotalRooms.ReadOnly = true;
            txtTotalRooms.Size = new Size(80, 23);
            txtTotalRooms.TabIndex = 21;
            // 
            // txtRoomType
            // 
            txtRoomType.Location = new Point(445, 48);
            txtRoomType.Name = "txtRoomType";
            txtRoomType.ReadOnly = true;
            txtRoomType.Size = new Size(100, 23);
            txtRoomType.TabIndex = 22;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(19, 98);
            txtAddress.Name = "txtAddress";
            txtAddress.ReadOnly = true;
            txtAddress.Size = new Size(200, 23);
            txtAddress.TabIndex = 23;
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(229, 98);
            txtPhone.Name = "txtPhone";
            txtPhone.ReadOnly = true;
            txtPhone.Size = new Size(120, 23);
            txtPhone.TabIndex = 24;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(359, 98);
            txtEmail.Name = "txtEmail";
            txtEmail.ReadOnly = true;
            txtEmail.Size = new Size(200, 23);
            txtEmail.TabIndex = 25;
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(19, 143);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ReadOnly = true;
            txtNotes.Size = new Size(706, 45);
            txtNotes.TabIndex = 26;
            // 
            // chkTermsAccepted
            // 
            chkTermsAccepted.AutoSize = true;
            chkTermsAccepted.Enabled = false;
            chkTermsAccepted.Location = new Point(606, 100);
            chkTermsAccepted.Name = "chkTermsAccepted";
            chkTermsAccepted.Size = new Size(110, 19);
            chkTermsAccepted.TabIndex = 27;
            chkTermsAccepted.Text = "Terms Accepted";
            chkTermsAccepted.UseVisualStyleBackColor = true;
            // 
            // txtSubmissionDate
            // 
            txtSubmissionDate.Location = new Point(566, 48);
            txtSubmissionDate.Name = "txtSubmissionDate";
            txtSubmissionDate.ReadOnly = true;
            txtSubmissionDate.Size = new Size(150, 23);
            txtSubmissionDate.TabIndex = 28;
            // 
            // txtPaymentToken
            // 
            txtPaymentToken.Location = new Point(16, 43);
            txtPaymentToken.Name = "txtPaymentToken";
            txtPaymentToken.ReadOnly = true;
            txtPaymentToken.Size = new Size(119, 23);
            txtPaymentToken.TabIndex = 29;
            // 
            // txtApprovalNum
            // 
            txtApprovalNum.Location = new Point(141, 43);
            txtApprovalNum.Name = "txtApprovalNum";
            txtApprovalNum.ReadOnly = true;
            txtApprovalNum.Size = new Size(88, 23);
            txtApprovalNum.TabIndex = 30;
            // 
            // txtCardName
            // 
            txtCardName.Location = new Point(240, 43);
            txtCardName.Name = "txtCardName";
            txtCardName.ReadOnly = true;
            txtCardName.Size = new Size(164, 23);
            txtCardName.TabIndex = 31;
            // 
            // txtExpiryDate
            // 
            txtExpiryDate.Location = new Point(421, 43);
            txtExpiryDate.Name = "txtExpiryDate";
            txtExpiryDate.ReadOnly = true;
            txtExpiryDate.Size = new Size(69, 23);
            txtExpiryDate.TabIndex = 32;
            txtExpiryDate.TextAlign = HorizontalAlignment.Right;
            // 
            // txtAmount
            // 
            txtAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtAmount.Location = new Point(519, 43);
            txtAmount.Name = "txtAmount";
            txtAmount.ReadOnly = true;
            txtAmount.Size = new Size(55, 23);
            txtAmount.TabIndex = 33;
            txtAmount.TextAlign = HorizontalAlignment.Right;
            // 
            // txtPaymentDateTime
            // 
            txtPaymentDateTime.Location = new Point(607, 43);
            txtPaymentDateTime.Name = "txtPaymentDateTime";
            txtPaymentDateTime.ReadOnly = true;
            txtPaymentDateTime.Size = new Size(115, 23);
            txtPaymentDateTime.TabIndex = 34;
            txtPaymentDateTime.TextAlign = HorizontalAlignment.Right;
            // 
            // grpOrderInfo
            // 
            grpOrderInfo.BackColor = Color.FromArgb(255, 255, 192);
            grpOrderInfo.Controls.Add(lblRequestId);
            grpOrderInfo.Controls.Add(lblVacationDate);
            grpOrderInfo.Controls.Add(lblTotalRooms);
            grpOrderInfo.Controls.Add(lblRoomType);
            grpOrderInfo.Controls.Add(lblAddress);
            grpOrderInfo.Controls.Add(lblPhone);
            grpOrderInfo.Controls.Add(lblEmail);
            grpOrderInfo.Controls.Add(lblNotes);
            grpOrderInfo.Controls.Add(lblSubmissionDate);
            grpOrderInfo.Controls.Add(txtOrderRequestId);
            grpOrderInfo.Controls.Add(txtVacationDate);
            grpOrderInfo.Controls.Add(txtTotalRooms);
            grpOrderInfo.Controls.Add(txtRoomType);
            grpOrderInfo.Controls.Add(txtAddress);
            grpOrderInfo.Controls.Add(txtPhone);
            grpOrderInfo.Controls.Add(txtEmail);
            grpOrderInfo.Controls.Add(txtNotes);
            grpOrderInfo.Controls.Add(chkTermsAccepted);
            grpOrderInfo.Controls.Add(txtSubmissionDate);
            grpOrderInfo.Location = new Point(735, 361);
            grpOrderInfo.Name = "grpOrderInfo";
            grpOrderInfo.Size = new Size(741, 207);
            grpOrderInfo.TabIndex = 35;
            grpOrderInfo.TabStop = false;
            grpOrderInfo.Text = "Order Information";
            // 
            // lblRequestId
            // 
            lblRequestId.AutoSize = true;
            lblRequestId.Location = new Point(16, 30);
            lblRequestId.Name = "lblRequestId";
            lblRequestId.Size = new Size(66, 15);
            lblRequestId.TabIndex = 37;
            lblRequestId.Text = "Request ID:";
            // 
            // lblVacationDate
            // 
            lblVacationDate.AutoSize = true;
            lblVacationDate.Location = new Point(126, 30);
            lblVacationDate.Name = "lblVacationDate";
            lblVacationDate.Size = new Size(82, 15);
            lblVacationDate.TabIndex = 38;
            lblVacationDate.Text = "Vacation Date:";
            // 
            // lblTotalRooms
            // 
            lblTotalRooms.AutoSize = true;
            lblTotalRooms.Location = new Point(355, 30);
            lblTotalRooms.Name = "lblTotalRooms";
            lblTotalRooms.Size = new Size(75, 15);
            lblTotalRooms.TabIndex = 39;
            lblTotalRooms.Text = "Total Rooms:";
            // 
            // lblRoomType
            // 
            lblRoomType.AutoSize = true;
            lblRoomType.Location = new Point(445, 30);
            lblRoomType.Name = "lblRoomType";
            lblRoomType.Size = new Size(69, 15);
            lblRoomType.TabIndex = 40;
            lblRoomType.Text = "Room Type:";
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(19, 80);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(52, 15);
            lblAddress.TabIndex = 41;
            lblAddress.Text = "Address:";
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(229, 80);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(44, 15);
            lblPhone.TabIndex = 42;
            lblPhone.Text = "Phone:";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(359, 80);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 43;
            lblEmail.Text = "Email:";
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(19, 126);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(41, 15);
            lblNotes.TabIndex = 44;
            lblNotes.Text = "Notes:";
            // 
            // lblSubmissionDate
            // 
            lblSubmissionDate.AutoSize = true;
            lblSubmissionDate.Location = new Point(566, 30);
            lblSubmissionDate.Name = "lblSubmissionDate";
            lblSubmissionDate.Size = new Size(98, 15);
            lblSubmissionDate.TabIndex = 45;
            lblSubmissionDate.Text = "Submission Date:";
            // 
            // grpPaymentInfo
            // 
            grpPaymentInfo.BackColor = Color.FromArgb(255, 255, 192);
            grpPaymentInfo.Controls.Add(lblToken);
            grpPaymentInfo.Controls.Add(lblApprovalNum);
            grpPaymentInfo.Controls.Add(lblCardName);
            grpPaymentInfo.Controls.Add(lblValidDate);
            grpPaymentInfo.Controls.Add(lblAmount);
            grpPaymentInfo.Controls.Add(lblPaymentDateTime);
            grpPaymentInfo.Controls.Add(txtPaymentToken);
            grpPaymentInfo.Controls.Add(txtApprovalNum);
            grpPaymentInfo.Controls.Add(txtCardName);
            grpPaymentInfo.Controls.Add(txtExpiryDate);
            grpPaymentInfo.Controls.Add(txtAmount);
            grpPaymentInfo.Controls.Add(txtPaymentDateTime);
            grpPaymentInfo.Location = new Point(740, 576);
            grpPaymentInfo.Name = "grpPaymentInfo";
            grpPaymentInfo.Size = new Size(736, 82);
            grpPaymentInfo.TabIndex = 36;
            grpPaymentInfo.TabStop = false;
            grpPaymentInfo.Text = "Payment Information";
            // 
            // lblToken
            // 
            lblToken.AutoSize = true;
            lblToken.Location = new Point(16, 25);
            lblToken.Name = "lblToken";
            lblToken.Size = new Size(41, 15);
            lblToken.TabIndex = 46;
            lblToken.Text = "Token:";
            // 
            // lblApprovalNum
            // 
            lblApprovalNum.AutoSize = true;
            lblApprovalNum.Location = new Point(141, 25);
            lblApprovalNum.Name = "lblApprovalNum";
            lblApprovalNum.Size = new Size(88, 15);
            lblApprovalNum.TabIndex = 47;
            lblApprovalNum.Text = "Approval Num:";
            // 
            // lblCardName
            // 
            lblCardName.AutoSize = true;
            lblCardName.Location = new Point(240, 25);
            lblCardName.Name = "lblCardName";
            lblCardName.Size = new Size(70, 15);
            lblCardName.TabIndex = 48;
            lblCardName.Text = "Card Name:";
            // 
            // lblValidDate
            // 
            lblValidDate.AutoSize = true;
            lblValidDate.Location = new Point(421, 25);
            lblValidDate.Name = "lblValidDate";
            lblValidDate.Size = new Size(69, 15);
            lblValidDate.TabIndex = 49;
            lblValidDate.Text = "Expiry Date:";
            // 
            // lblAmount
            // 
            lblAmount.AutoSize = true;
            lblAmount.Location = new Point(519, 25);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(54, 15);
            lblAmount.TabIndex = 50;
            lblAmount.Text = "Amount:";
            // 
            // lblPaymentDateTime
            // 
            lblPaymentDateTime.AutoSize = true;
            lblPaymentDateTime.Location = new Point(607, 25);
            lblPaymentDateTime.Name = "lblPaymentDateTime";
            lblPaymentDateTime.Size = new Size(115, 15);
            lblPaymentDateTime.TabIndex = 51;
            lblPaymentDateTime.Text = "Payment Date/Time:";
            // 
            // functions
            // 
            functions.BackColor = Color.IndianRed;
            functions.Controls.Add(btnFindAllAutomationId);
            functions.Controls.Add(btnCreatePayment);
            functions.Controls.Add(btnOpenGilboa);
            functions.Controls.Add(btnRefresh);
            functions.Controls.Add(listBox1);
            functions.Controls.Add(btnShowCoordinates);
            functions.Controls.Add(btnOpenExistingDoket);
            functions.Controls.Add(txtResult);
            functions.Controls.Add(btnOpenNewDoketsWithPax);
            functions.Controls.Add(btnShowControls);
            functions.Controls.Add(txtDoket);
            functions.Controls.Add(btnCreateVoucher);
            functions.Controls.Add(label1);
            functions.Location = new Point(1482, 633);
            functions.Name = "functions";
            functions.Size = new Size(568, 329);
            functions.TabIndex = 37;
            functions.TabStop = false;
            functions.Text = "functions";
            functions.Visible = false;
            // 
            // btnFindAllAutomationId
            // 
            btnFindAllAutomationId.BackColor = Color.FromArgb(255, 224, 192);
            btnFindAllAutomationId.Font = new Font("Times New Roman", 12F);
            btnFindAllAutomationId.Location = new Point(271, 200);
            btnFindAllAutomationId.Name = "btnFindAllAutomationId";
            btnFindAllAutomationId.Size = new Size(267, 30);
            btnFindAllAutomationId.TabIndex = 18;
            btnFindAllAutomationId.Text = "Find all automation id";
            btnFindAllAutomationId.TextAlign = ContentAlignment.TopCenter;
            btnFindAllAutomationId.UseVisualStyleBackColor = false;
            btnFindAllAutomationId.Click += btnFindAllAutomationId_Click;
            // 
            // btnCreatePayment
            // 
            btnCreatePayment.Font = new Font("Times New Roman", 12F);
            btnCreatePayment.Location = new Point(9, 128);
            btnCreatePayment.Name = "btnCreatePayment";
            btnCreatePayment.Size = new Size(150, 29);
            btnCreatePayment.TabIndex = 17;
            btnCreatePayment.Text = "Create Payment";
            btnCreatePayment.UseVisualStyleBackColor = true;
            btnCreatePayment.Click += btnCreatePayment_Click;
            // 
            // btnEnterInDoket
            // 
            btnEnterInDoket.Font = new Font("Times New Roman", 20F);
            btnEnterInDoket.Location = new Point(8, 576);
            btnEnterInDoket.Name = "btnEnterInDoket";
            btnEnterInDoket.Size = new Size(709, 82);
            btnEnterInDoket.TabIndex = 19;
            btnEnterInDoket.Text = "Open and Enter Into Doket";
            btnEnterInDoket.UseVisualStyleBackColor = true;
            btnEnterInDoket.Click += btnEnterInDoket_Click;
            // 
            // txtPaxBirthDate
            // 
            txtPaxBirthDate.Font = new Font("Segoe UI", 9F);
            txtPaxBirthDate.Location = new Point(120, 22);
            txtPaxBirthDate.Name = "txtPaxBirthDate";
            txtPaxBirthDate.ReadOnly = true;
            txtPaxBirthDate.Size = new Size(76, 23);
            txtPaxBirthDate.TabIndex = 1;
            // 
            // txtPaxPassport
            // 
            txtPaxPassport.Font = new Font("Segoe UI", 9F);
            txtPaxPassport.Location = new Point(120, 51);
            txtPaxPassport.Name = "txtPaxPassport";
            txtPaxPassport.ReadOnly = true;
            txtPaxPassport.Size = new Size(300, 23);
            txtPaxPassport.TabIndex = 3;
            // 
            // txtPaxGender
            // 
            txtPaxGender.Font = new Font("Segoe UI", 9F);
            txtPaxGender.Location = new Point(120, 80);
            txtPaxGender.Name = "txtPaxGender";
            txtPaxGender.ReadOnly = true;
            txtPaxGender.Size = new Size(100, 23);
            txtPaxGender.TabIndex = 5;
            // 
            // chkPaxNoFlight
            // 
            chkPaxNoFlight.AutoSize = true;
            chkPaxNoFlight.Enabled = false;
            chkPaxNoFlight.Font = new Font("Segoe UI", 9F);
            chkPaxNoFlight.Location = new Point(10, 112);
            chkPaxNoFlight.Name = "chkPaxNoFlight";
            chkPaxNoFlight.Size = new Size(75, 19);
            chkPaxNoFlight.TabIndex = 6;
            chkPaxNoFlight.Text = "No Flight";
            chkPaxNoFlight.UseVisualStyleBackColor = true;
            // 
            // chkPaxSkiPass
            // 
            chkPaxSkiPass.AutoSize = true;
            chkPaxSkiPass.Enabled = false;
            chkPaxSkiPass.Font = new Font("Segoe UI", 9F);
            chkPaxSkiPass.Location = new Point(10, 140);
            chkPaxSkiPass.Name = "chkPaxSkiPass";
            chkPaxSkiPass.Size = new Size(67, 19);
            chkPaxSkiPass.TabIndex = 7;
            chkPaxSkiPass.Text = "Ski Pass";
            chkPaxSkiPass.UseVisualStyleBackColor = true;
            // 
            // txtPaxSkiLevel
            // 
            txtPaxSkiLevel.Font = new Font("Segoe UI", 9F);
            txtPaxSkiLevel.Location = new Point(228, 137);
            txtPaxSkiLevel.Name = "txtPaxSkiLevel";
            txtPaxSkiLevel.ReadOnly = true;
            txtPaxSkiLevel.Size = new Size(100, 23);
            txtPaxSkiLevel.TabIndex = 9;
            // 
            // chkPaxHasFFNumber
            // 
            chkPaxHasFFNumber.AutoSize = true;
            chkPaxHasFFNumber.Enabled = false;
            chkPaxHasFFNumber.Font = new Font("Segoe UI", 9F);
            chkPaxHasFFNumber.Location = new Point(10, 170);
            chkPaxHasFFNumber.Name = "chkPaxHasFFNumber";
            chkPaxHasFFNumber.Size = new Size(108, 19);
            chkPaxHasFFNumber.TabIndex = 10;
            chkPaxHasFFNumber.Text = "Has FF Number";
            chkPaxHasFFNumber.UseVisualStyleBackColor = true;
            // 
            // txtPaxFFNumber
            // 
            txtPaxFFNumber.Font = new Font("Segoe UI", 9F);
            txtPaxFFNumber.Location = new Point(124, 168);
            txtPaxFFNumber.Name = "txtPaxFFNumber";
            txtPaxFFNumber.ReadOnly = true;
            txtPaxFFNumber.Size = new Size(285, 23);
            txtPaxFFNumber.TabIndex = 12;
            // 
            // grpPaxDetails
            // 
            grpPaxDetails.BackColor = Color.FromArgb(255, 224, 192);
            grpPaxDetails.Controls.Add(lblPaxBirthDate);
            grpPaxDetails.Controls.Add(txtPaxBirthDate);
            grpPaxDetails.Controls.Add(lblPaxPassport);
            grpPaxDetails.Controls.Add(txtPaxPassport);
            grpPaxDetails.Controls.Add(lblPaxGender);
            grpPaxDetails.Controls.Add(txtPaxGender);
            grpPaxDetails.Controls.Add(chkPaxNoFlight);
            grpPaxDetails.Controls.Add(chkPaxSkiPass);
            grpPaxDetails.Controls.Add(lblPaxSkiLevel);
            grpPaxDetails.Controls.Add(txtPaxSkiLevel);
            grpPaxDetails.Controls.Add(chkPaxHasFFNumber);
            grpPaxDetails.Controls.Add(txtPaxFFNumber);
            grpPaxDetails.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grpPaxDetails.Location = new Point(1041, 12);
            grpPaxDetails.Name = "grpPaxDetails";
            grpPaxDetails.Size = new Size(435, 213);
            grpPaxDetails.TabIndex = 21;
            grpPaxDetails.TabStop = false;
            grpPaxDetails.Text = "Passenger Details";
            // 
            // lblPaxBirthDate
            // 
            lblPaxBirthDate.AutoSize = true;
            lblPaxBirthDate.Font = new Font("Segoe UI", 9F);
            lblPaxBirthDate.Location = new Point(10, 25);
            lblPaxBirthDate.Name = "lblPaxBirthDate";
            lblPaxBirthDate.Size = new Size(62, 15);
            lblPaxBirthDate.TabIndex = 0;
            lblPaxBirthDate.Text = "Birth Date:";
            // 
            // lblPaxPassport
            // 
            lblPaxPassport.AutoSize = true;
            lblPaxPassport.Font = new Font("Segoe UI", 9F);
            lblPaxPassport.Location = new Point(10, 54);
            lblPaxPassport.Name = "lblPaxPassport";
            lblPaxPassport.Size = new Size(55, 15);
            lblPaxPassport.TabIndex = 2;
            lblPaxPassport.Text = "Passport:";
            // 
            // lblPaxGender
            // 
            lblPaxGender.AutoSize = true;
            lblPaxGender.Font = new Font("Segoe UI", 9F);
            lblPaxGender.Location = new Point(10, 83);
            lblPaxGender.Name = "lblPaxGender";
            lblPaxGender.Size = new Size(48, 15);
            lblPaxGender.TabIndex = 4;
            lblPaxGender.Text = "Gender:";
            // 
            // lblPaxSkiLevel
            // 
            lblPaxSkiLevel.AutoSize = true;
            lblPaxSkiLevel.Font = new Font("Segoe UI", 9F);
            lblPaxSkiLevel.Location = new Point(118, 140);
            lblPaxSkiLevel.Name = "lblPaxSkiLevel";
            lblPaxSkiLevel.Size = new Size(55, 15);
            lblPaxSkiLevel.TabIndex = 8;
            lblPaxSkiLevel.Text = "Ski Level:";
            // 
            // txtFileType
            // 
            txtFileType.Font = new Font("Segoe UI", 9F);
            txtFileType.Location = new Point(120, 22);
            txtFileType.Name = "txtFileType";
            txtFileType.ReadOnly = true;
            txtFileType.Size = new Size(300, 23);
            txtFileType.TabIndex = 1;
            // 
            // txtFileSize
            // 
            txtFileSize.Font = new Font("Segoe UI", 9F);
            txtFileSize.Location = new Point(120, 51);
            txtFileSize.Name = "txtFileSize";
            txtFileSize.ReadOnly = true;
            txtFileSize.Size = new Size(300, 23);
            txtFileSize.TabIndex = 3;
            // 
            // txtFileFolderUrl
            // 
            txtFileFolderUrl.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            txtFileFolderUrl.ForeColor = Color.FromArgb(0, 0, 192);
            txtFileFolderUrl.Location = new Point(120, 80);
            txtFileFolderUrl.Name = "txtFileFolderUrl";
            txtFileFolderUrl.Size = new Size(300, 23);
            txtFileFolderUrl.TabIndex = 5;
            txtFileFolderUrl.Click += txtFileFolderUrl_Click;
            // 
            // grpFileDetails
            // 
            grpFileDetails.BackColor = Color.FromArgb(255, 224, 192);
            grpFileDetails.Controls.Add(lblFileType);
            grpFileDetails.Controls.Add(txtFileType);
            grpFileDetails.Controls.Add(lblFileSize);
            grpFileDetails.Controls.Add(txtFileSize);
            grpFileDetails.Controls.Add(lblFileFolderUrl);
            grpFileDetails.Controls.Add(txtFileFolderUrl);
            grpFileDetails.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grpFileDetails.Location = new Point(1045, 241);
            grpFileDetails.Name = "grpFileDetails";
            grpFileDetails.Size = new Size(431, 114);
            grpFileDetails.TabIndex = 22;
            grpFileDetails.TabStop = false;
            grpFileDetails.Text = "File Details";
            // 
            // lblFileType
            // 
            lblFileType.AutoSize = true;
            lblFileType.Font = new Font("Segoe UI", 9F);
            lblFileType.Location = new Point(10, 25);
            lblFileType.Name = "lblFileType";
            lblFileType.Size = new Size(34, 15);
            lblFileType.TabIndex = 0;
            lblFileType.Text = "Type:";
            // 
            // lblFileSize
            // 
            lblFileSize.AutoSize = true;
            lblFileSize.Font = new Font("Segoe UI", 9F);
            lblFileSize.Location = new Point(10, 54);
            lblFileSize.Name = "lblFileSize";
            lblFileSize.Size = new Size(55, 15);
            lblFileSize.TabIndex = 2;
            lblFileSize.Text = "Size (KB):";
            // 
            // lblFileFolderUrl
            // 
            lblFileFolderUrl.AutoSize = true;
            lblFileFolderUrl.Font = new Font("Segoe UI", 9F);
            lblFileFolderUrl.Location = new Point(10, 83);
            lblFileFolderUrl.Name = "lblFileFolderUrl";
            lblFileFolderUrl.Size = new Size(67, 15);
            lblFileFolderUrl.TabIndex = 4;
            lblFileFolderUrl.Text = "Folder URL:";
            // 
            // DataHandling
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1502, 662);
            Controls.Add(grpFileDetails);
            Controls.Add(grpPaxDetails);
            Controls.Add(functions);
            Controls.Add(btnEnterInDoket);
            Controls.Add(grpPaymentInfo);
            Controls.Add(grpOrderInfo);
            Controls.Add(dgvPax);
            Controls.Add(dgvFiles);
            Controls.Add(dgvRequestsStatus);
            Controls.Add(btnImportExcel);
            Controls.Add(btnStopServer);
            Controls.Add(btnStartServer);
            Name = "DataHandling";
            Text = "Data Handling";
            FormClosing += DataHandling_FormClosing;
            Load += DataHandling_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRequestsStatus).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPax).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvFiles).EndInit();
            grpOrderInfo.ResumeLayout(false);
            grpOrderInfo.PerformLayout();
            grpPaymentInfo.ResumeLayout(false);
            grpPaymentInfo.PerformLayout();
            functions.ResumeLayout(false);
            functions.PerformLayout();
            grpPaxDetails.ResumeLayout(false);
            grpPaxDetails.PerformLayout();
            grpFileDetails.ResumeLayout(false);
            grpFileDetails.PerformLayout();
            ResumeLayout(false);
        }

        private void btnCreatePayment_Click(object sender, EventArgs e)
        {
            try
            {
                // Open Gilboa if not already open
                gilboaAutomation.OpenOrAttachGilboa();
                
                // Open new doket if no doket is currently open
                if (gilboaAutomation.CurrentDoket == 0)
                {
                    btnOpenNewDoketsWithPax.PerformClick();
                }
                 var paymentInfo = GetPaymentInfo();
                gilboaAutomation.CreatePayment(paymentInfo);
            }
            catch (Exception ex)
            {
                HandleError(ex, $"Error creating payment: {ex.Message}");
            }
        }

        #endregion
        private Button btnStartServer;
        private Button btnStopServer;
        private Button btnImportExcel;
        private DataGridView dgvRequestsStatus;
        private DataGridView dgvPax;
        private DataGridView dgvFiles;
        private Button btnCreateVoucher;
        private Button btnOpenGilboa;
        private Button btnOpenNewDoketsWithPax;
        private Button btnShowCoordinates;
        private Label label1;
        private Button btnRefresh;
        private TextBox txtDoket;
        private ListBox listBox1;
        private Button btnOpenExistingDoket;
        private Button btnShowControls;
        private TextBox txtResult;
        private TextBox txtOrderRequestId;
        private TextBox txtVacationDate;
        private TextBox txtTotalRooms;
        private TextBox txtRoomType;
        private TextBox txtAddress;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtNotes;
        private CheckBox chkTermsAccepted;
        private TextBox txtSubmissionDate;
        private TextBox txtPaymentToken;
        private TextBox txtApprovalNum;
        private TextBox txtCardName;
        private TextBox txtExpiryDate;
        private TextBox txtAmount;
        private TextBox txtPaymentDateTime;
        private GroupBox grpOrderInfo;
        private GroupBox grpPaymentInfo;
        private GroupBox functions;
        private Label lblRequestId;
        private Label lblVacationDate;
        private Label lblTotalRooms;
        private Label lblRoomType;
        private Label lblAddress;
        private Label lblPhone;
        private Label lblEmail;
        private Label lblNotes;
        private Label lblSubmissionDate;
        private Label lblToken;
        private Label lblApprovalNum;
        private Label lblCardName;
        private Label lblValidDate;
        private Label lblAmount;
        private Label lblPaymentDateTime;
        private Button btnCreatePayment;
        private Button btnFindAllAutomationId;
        private Button button1;
        private Button btnEnterInDoket;
        private GroupBox grpPaxDetails;
        private TextBox txtPaxBirthDate;
        private TextBox txtPaxPassport;
        private TextBox txtPaxGender;
        private CheckBox chkPaxNoFlight;
        private CheckBox chkPaxSkiPass;
        private TextBox txtPaxSkiLevel;
        private CheckBox chkPaxHasFFNumber;
        private TextBox txtPaxFFNumber;
        private Label lblPaxBirthDate;
        private Label lblPaxPassport;
        private Label lblPaxGender;
        private Label lblPaxSkiLevel;
        private GroupBox grpFileDetails;
        private TextBox txtFileType;
        private TextBox txtFileSize;
        private TextBox txtFileFolderUrl;
        private Label lblFileType;
        private Label lblFileSize;
        private Label lblFileFolderUrl;
    }
}
namespace Demo.SysWork.Data
{
    partial class FrmDemoSysworkData
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.GrpParameters = new System.Windows.Forms.GroupBox();
            this.BtnUnconnect = new System.Windows.Forms.Button();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.BtnGetParameters = new System.Windows.Forms.Button();
            this.TxtConnectionString = new System.Windows.Forms.TextBox();
            this.CmbDataBaseEngine = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GrpDetails = new System.Windows.Forms.GroupBox();
            this.BtnEntityClassFromDb = new System.Windows.Forms.Button();
            this.BtnGetByLambdaExpression = new System.Windows.Forms.Button();
            this.BtnGetAll = new System.Windows.Forms.Button();
            this.BtnDeleteByLambdaExpression = new System.Windows.Forms.Button();
            this.BtnCLearLog = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtLog = new System.Windows.Forms.TextBox();
            this.BtnTestGenericViewManager = new System.Windows.Forms.Button();
            this.BtnSqlLAMTest = new System.Windows.Forms.Button();
            this.BtnLogger = new System.Windows.Forms.Button();
            this.BtnExecuteNonQueryWparam = new System.Windows.Forms.Button();
            this.BtnExecuteNonQuery = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.TxtPassport = new System.Windows.Forms.TextBox();
            this.BtnGetByPassport = new System.Windows.Forms.Button();
            this.TxtId = new System.Windows.Forms.TextBox();
            this.BtnGetByID = new System.Windows.Forms.Button();
            this.TxtDelId = new System.Windows.Forms.TextBox();
            this.DeleteByID = new System.Windows.Forms.Button();
            this.BtnFind5 = new System.Windows.Forms.Button();
            this.BtnUpdateRange = new System.Windows.Forms.Button();
            this.BtnDeleteAll = new System.Windows.Forms.Button();
            this.BtnAddRange = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BtnSimpleQuery = new System.Windows.Forms.Button();
            this.BtnExistsColumn = new System.Windows.Forms.Button();
            this.BtnExistsTable = new System.Windows.Forms.Button();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.GrpParameters.SuspendLayout();
            this.GrpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpParameters
            // 
            this.GrpParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrpParameters.Controls.Add(this.BtnUnconnect);
            this.GrpParameters.Controls.Add(this.BtnConnect);
            this.GrpParameters.Controls.Add(this.BtnGetParameters);
            this.GrpParameters.Controls.Add(this.TxtConnectionString);
            this.GrpParameters.Controls.Add(this.CmbDataBaseEngine);
            this.GrpParameters.Controls.Add(this.label2);
            this.GrpParameters.Controls.Add(this.label1);
            this.GrpParameters.Location = new System.Drawing.Point(9, 1);
            this.GrpParameters.Name = "GrpParameters";
            this.GrpParameters.Size = new System.Drawing.Size(877, 107);
            this.GrpParameters.TabIndex = 92;
            this.GrpParameters.TabStop = false;
            this.GrpParameters.Text = "DB Parameters";
            // 
            // BtnUnconnect
            // 
            this.BtnUnconnect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnUnconnect.Location = new System.Drawing.Point(501, 78);
            this.BtnUnconnect.Name = "BtnUnconnect";
            this.BtnUnconnect.Size = new System.Drawing.Size(129, 21);
            this.BtnUnconnect.TabIndex = 6;
            this.BtnUnconnect.Text = "&Desconectar";
            this.BtnUnconnect.UseVisualStyleBackColor = true;
            this.BtnUnconnect.Click += new System.EventHandler(this.BtnUnconnect_Click);
            // 
            // BtnConnect
            // 
            this.BtnConnect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnConnect.Location = new System.Drawing.Point(321, 78);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(129, 21);
            this.BtnConnect.TabIndex = 5;
            this.BtnConnect.Text = "&Conectar";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // BtnGetParameters
            // 
            this.BtnGetParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnGetParameters.Location = new System.Drawing.Point(850, 49);
            this.BtnGetParameters.Name = "BtnGetParameters";
            this.BtnGetParameters.Size = new System.Drawing.Size(18, 20);
            this.BtnGetParameters.TabIndex = 4;
            this.BtnGetParameters.Text = "...";
            this.BtnGetParameters.UseVisualStyleBackColor = true;
            this.BtnGetParameters.Click += new System.EventHandler(this.BtnGetParameters_Click);
            // 
            // TxtConnectionString
            // 
            this.TxtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtConnectionString.Location = new System.Drawing.Point(105, 49);
            this.TxtConnectionString.Name = "TxtConnectionString";
            this.TxtConnectionString.Size = new System.Drawing.Size(739, 20);
            this.TxtConnectionString.TabIndex = 3;
            // 
            // CmbDataBaseEngine
            // 
            this.CmbDataBaseEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbDataBaseEngine.FormattingEnabled = true;
            this.CmbDataBaseEngine.Location = new System.Drawing.Point(107, 19);
            this.CmbDataBaseEngine.Name = "CmbDataBaseEngine";
            this.CmbDataBaseEngine.Size = new System.Drawing.Size(761, 21);
            this.CmbDataBaseEngine.TabIndex = 2;
            this.CmbDataBaseEngine.SelectedValueChanged += new System.EventHandler(this.CmbDataBaseEngine_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ConnectionString";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Base Engine";
            // 
            // GrpDetails
            // 
            this.GrpDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrpDetails.Controls.Add(this.button1);
            this.GrpDetails.Controls.Add(this.BtnEntityClassFromDb);
            this.GrpDetails.Controls.Add(this.BtnGetByLambdaExpression);
            this.GrpDetails.Controls.Add(this.BtnGetAll);
            this.GrpDetails.Controls.Add(this.BtnDeleteByLambdaExpression);
            this.GrpDetails.Controls.Add(this.BtnCLearLog);
            this.GrpDetails.Controls.Add(this.label3);
            this.GrpDetails.Controls.Add(this.TxtLog);
            this.GrpDetails.Controls.Add(this.BtnTestGenericViewManager);
            this.GrpDetails.Controls.Add(this.BtnSqlLAMTest);
            this.GrpDetails.Controls.Add(this.BtnLogger);
            this.GrpDetails.Controls.Add(this.BtnExecuteNonQueryWparam);
            this.GrpDetails.Controls.Add(this.BtnExecuteNonQuery);
            this.GrpDetails.Controls.Add(this.txtQuery);
            this.GrpDetails.Controls.Add(this.TxtPassport);
            this.GrpDetails.Controls.Add(this.BtnGetByPassport);
            this.GrpDetails.Controls.Add(this.TxtId);
            this.GrpDetails.Controls.Add(this.BtnGetByID);
            this.GrpDetails.Controls.Add(this.TxtDelId);
            this.GrpDetails.Controls.Add(this.DeleteByID);
            this.GrpDetails.Controls.Add(this.BtnFind5);
            this.GrpDetails.Controls.Add(this.BtnUpdateRange);
            this.GrpDetails.Controls.Add(this.BtnDeleteAll);
            this.GrpDetails.Controls.Add(this.BtnAddRange);
            this.GrpDetails.Controls.Add(this.dataGridView1);
            this.GrpDetails.Controls.Add(this.BtnSimpleQuery);
            this.GrpDetails.Controls.Add(this.BtnExistsColumn);
            this.GrpDetails.Controls.Add(this.BtnExistsTable);
            this.GrpDetails.Controls.Add(this.BtnUpdate);
            this.GrpDetails.Controls.Add(this.BtnAdd);
            this.GrpDetails.Location = new System.Drawing.Point(9, 114);
            this.GrpDetails.Name = "GrpDetails";
            this.GrpDetails.Size = new System.Drawing.Size(876, 438);
            this.GrpDetails.TabIndex = 93;
            this.GrpDetails.TabStop = false;
            // 
            // BtnEntityClassFromDb
            // 
            this.BtnEntityClassFromDb.Location = new System.Drawing.Point(250, 173);
            this.BtnEntityClassFromDb.Name = "BtnEntityClassFromDb";
            this.BtnEntityClassFromDb.Size = new System.Drawing.Size(238, 19);
            this.BtnEntityClassFromDb.TabIndex = 94;
            this.BtnEntityClassFromDb.Text = "Test EntityClassFromDb";
            this.BtnEntityClassFromDb.UseVisualStyleBackColor = true;
            this.BtnEntityClassFromDb.Click += new System.EventHandler(this.BtnTestEntityClassFromDb_Click);
            // 
            // BtnGetByLambdaExpression
            // 
            this.BtnGetByLambdaExpression.Location = new System.Drawing.Point(88, 198);
            this.BtnGetByLambdaExpression.Name = "BtnGetByLambdaExpression";
            this.BtnGetByLambdaExpression.Size = new System.Drawing.Size(156, 21);
            this.BtnGetByLambdaExpression.TabIndex = 139;
            this.BtnGetByLambdaExpression.Text = "Get List By Lambda Expression";
            this.BtnGetByLambdaExpression.UseVisualStyleBackColor = true;
            this.BtnGetByLambdaExpression.Click += new System.EventHandler(this.BtnGetByLambdaExpression_Click);
            // 
            // BtnGetAll
            // 
            this.BtnGetAll.Location = new System.Drawing.Point(6, 198);
            this.BtnGetAll.Name = "BtnGetAll";
            this.BtnGetAll.Size = new System.Drawing.Size(76, 21);
            this.BtnGetAll.TabIndex = 138;
            this.BtnGetAll.Text = "Get All ";
            this.BtnGetAll.UseVisualStyleBackColor = true;
            this.BtnGetAll.Click += new System.EventHandler(this.BtnGetAll_Click);
            // 
            // BtnDeleteByLambdaExpression
            // 
            this.BtnDeleteByLambdaExpression.Location = new System.Drawing.Point(88, 171);
            this.BtnDeleteByLambdaExpression.Name = "BtnDeleteByLambdaExpression";
            this.BtnDeleteByLambdaExpression.Size = new System.Drawing.Size(156, 21);
            this.BtnDeleteByLambdaExpression.TabIndex = 137;
            this.BtnDeleteByLambdaExpression.Text = "Delete By Lambda Expression";
            this.BtnDeleteByLambdaExpression.UseVisualStyleBackColor = true;
            this.BtnDeleteByLambdaExpression.Click += new System.EventHandler(this.BtnDeleteByLambdaExpression_Click);
            // 
            // BtnCLearLog
            // 
            this.BtnCLearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCLearLog.Location = new System.Drawing.Point(815, 10);
            this.BtnCLearLog.Name = "BtnCLearLog";
            this.BtnCLearLog.Size = new System.Drawing.Size(53, 20);
            this.BtnCLearLog.TabIndex = 136;
            this.BtnCLearLog.Text = "&Clear";
            this.BtnCLearLog.UseVisualStyleBackColor = true;
            this.BtnCLearLog.Click += new System.EventHandler(this.BtnCLearLog_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(568, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "Log";
            // 
            // TxtLog
            // 
            this.TxtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtLog.Location = new System.Drawing.Point(571, 34);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.Size = new System.Drawing.Size(297, 237);
            this.TxtLog.TabIndex = 134;
            // 
            // BtnTestGenericViewManager
            // 
            this.BtnTestGenericViewManager.Location = new System.Drawing.Point(250, 146);
            this.BtnTestGenericViewManager.Name = "BtnTestGenericViewManager";
            this.BtnTestGenericViewManager.Size = new System.Drawing.Size(238, 21);
            this.BtnTestGenericViewManager.TabIndex = 133;
            this.BtnTestGenericViewManager.Text = "Test generic View";
            this.BtnTestGenericViewManager.UseVisualStyleBackColor = true;
            // 
            // BtnSqlLAMTest
            // 
            this.BtnSqlLAMTest.Location = new System.Drawing.Point(250, 119);
            this.BtnSqlLAMTest.Name = "BtnSqlLAMTest";
            this.BtnSqlLAMTest.Size = new System.Drawing.Size(238, 21);
            this.BtnSqlLAMTest.TabIndex = 132;
            this.BtnSqlLAMTest.Text = "SQLLam Test";
            this.BtnSqlLAMTest.UseVisualStyleBackColor = true;
            this.BtnSqlLAMTest.Click += new System.EventHandler(this.BtnSqlLAMTest_Click);
            // 
            // BtnLogger
            // 
            this.BtnLogger.Location = new System.Drawing.Point(250, 90);
            this.BtnLogger.Name = "BtnLogger";
            this.BtnLogger.Size = new System.Drawing.Size(238, 23);
            this.BtnLogger.TabIndex = 131;
            this.BtnLogger.Text = "Logger";
            this.BtnLogger.UseVisualStyleBackColor = true;
            // 
            // BtnExecuteNonQueryWparam
            // 
            this.BtnExecuteNonQueryWparam.Location = new System.Drawing.Point(113, 279);
            this.BtnExecuteNonQueryWparam.Name = "BtnExecuteNonQueryWparam";
            this.BtnExecuteNonQueryWparam.Size = new System.Drawing.Size(131, 21);
            this.BtnExecuteNonQueryWparam.TabIndex = 129;
            this.BtnExecuteNonQueryWparam.Text = "ExecuteNonQuery W/P";
            this.BtnExecuteNonQueryWparam.UseVisualStyleBackColor = true;
            // 
            // BtnExecuteNonQuery
            // 
            this.BtnExecuteNonQuery.Location = new System.Drawing.Point(6, 279);
            this.BtnExecuteNonQuery.Name = "BtnExecuteNonQuery";
            this.BtnExecuteNonQuery.Size = new System.Drawing.Size(105, 21);
            this.BtnExecuteNonQuery.TabIndex = 128;
            this.BtnExecuteNonQuery.Text = "ExecuteNonQuery";
            this.BtnExecuteNonQuery.UseVisualStyleBackColor = true;
            this.BtnExecuteNonQuery.Click += new System.EventHandler(this.BtnExecuteNonQuery_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Location = new System.Drawing.Point(6, 308);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(862, 80);
            this.txtQuery.TabIndex = 127;
            // 
            // TxtPassport
            // 
            this.TxtPassport.Location = new System.Drawing.Point(142, 251);
            this.TxtPassport.Name = "TxtPassport";
            this.TxtPassport.Size = new System.Drawing.Size(102, 20);
            this.TxtPassport.TabIndex = 126;
            this.TxtPassport.Text = "0";
            // 
            // BtnGetByPassport
            // 
            this.BtnGetByPassport.Location = new System.Drawing.Point(6, 252);
            this.BtnGetByPassport.Name = "BtnGetByPassport";
            this.BtnGetByPassport.Size = new System.Drawing.Size(130, 21);
            this.BtnGetByPassport.TabIndex = 125;
            this.BtnGetByPassport.Text = "Get By Passport";
            this.BtnGetByPassport.UseVisualStyleBackColor = true;
            // 
            // TxtId
            // 
            this.TxtId.Location = new System.Drawing.Point(196, 226);
            this.TxtId.Name = "TxtId";
            this.TxtId.Size = new System.Drawing.Size(48, 20);
            this.TxtId.TabIndex = 124;
            this.TxtId.Text = "0";
            // 
            // BtnGetByID
            // 
            this.BtnGetByID.Location = new System.Drawing.Point(6, 225);
            this.BtnGetByID.Name = "BtnGetByID";
            this.BtnGetByID.Size = new System.Drawing.Size(184, 21);
            this.BtnGetByID.TabIndex = 123;
            this.BtnGetByID.Text = "Get By Id";
            this.BtnGetByID.UseVisualStyleBackColor = true;
            // 
            // TxtDelId
            // 
            this.TxtDelId.Location = new System.Drawing.Point(196, 145);
            this.TxtDelId.Name = "TxtDelId";
            this.TxtDelId.Size = new System.Drawing.Size(48, 20);
            this.TxtDelId.TabIndex = 122;
            this.TxtDelId.Text = "0";
            // 
            // DeleteByID
            // 
            this.DeleteByID.Location = new System.Drawing.Point(6, 144);
            this.DeleteByID.Name = "DeleteByID";
            this.DeleteByID.Size = new System.Drawing.Size(184, 21);
            this.DeleteByID.TabIndex = 121;
            this.DeleteByID.Text = "Delete By Id";
            this.DeleteByID.UseVisualStyleBackColor = true;
            this.DeleteByID.Click += new System.EventHandler(this.DeleteByID_Click);
            // 
            // BtnFind5
            // 
            this.BtnFind5.Location = new System.Drawing.Point(6, 117);
            this.BtnFind5.Name = "BtnFind5";
            this.BtnFind5.Size = new System.Drawing.Size(238, 21);
            this.BtnFind5.TabIndex = 120;
            this.BtnFind5.Text = "Find 5 ";
            this.BtnFind5.UseVisualStyleBackColor = true;
            this.BtnFind5.Click += new System.EventHandler(this.BtnFind5_Click);
            // 
            // BtnUpdateRange
            // 
            this.BtnUpdateRange.Location = new System.Drawing.Point(6, 90);
            this.BtnUpdateRange.Name = "BtnUpdateRange";
            this.BtnUpdateRange.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdateRange.TabIndex = 119;
            this.BtnUpdateRange.Text = "UpdateRange ";
            this.BtnUpdateRange.UseVisualStyleBackColor = true;
            this.BtnUpdateRange.Click += new System.EventHandler(this.BtnUpdateRange_Click);
            // 
            // BtnDeleteAll
            // 
            this.BtnDeleteAll.Location = new System.Drawing.Point(6, 171);
            this.BtnDeleteAll.Name = "BtnDeleteAll";
            this.BtnDeleteAll.Size = new System.Drawing.Size(76, 21);
            this.BtnDeleteAll.TabIndex = 118;
            this.BtnDeleteAll.Text = "Delete All ";
            this.BtnDeleteAll.UseVisualStyleBackColor = true;
            this.BtnDeleteAll.Click += new System.EventHandler(this.BtnDeleteAll_Click);
            // 
            // BtnAddRange
            // 
            this.BtnAddRange.Location = new System.Drawing.Point(6, 36);
            this.BtnAddRange.Name = "BtnAddRange";
            this.BtnAddRange.Size = new System.Drawing.Size(238, 21);
            this.BtnAddRange.TabIndex = 117;
            this.BtnAddRange.Text = "AddRange";
            this.BtnAddRange.UseVisualStyleBackColor = true;
            this.BtnAddRange.Click += new System.EventHandler(this.BtnAddRange_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 394);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(862, 35);
            this.dataGridView1.TabIndex = 116;
            // 
            // BtnSimpleQuery
            // 
            this.BtnSimpleQuery.Location = new System.Drawing.Point(250, 61);
            this.BtnSimpleQuery.Name = "BtnSimpleQuery";
            this.BtnSimpleQuery.Size = new System.Drawing.Size(238, 21);
            this.BtnSimpleQuery.TabIndex = 115;
            this.BtnSimpleQuery.Text = "SimpleQuery (Selecciona Todos)";
            this.BtnSimpleQuery.UseVisualStyleBackColor = true;
            // 
            // BtnExistsColumn
            // 
            this.BtnExistsColumn.Location = new System.Drawing.Point(250, 34);
            this.BtnExistsColumn.Name = "BtnExistsColumn";
            this.BtnExistsColumn.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsColumn.TabIndex = 114;
            this.BtnExistsColumn.Text = "Exists Column ";
            this.BtnExistsColumn.UseVisualStyleBackColor = true;
            // 
            // BtnExistsTable
            // 
            this.BtnExistsTable.Location = new System.Drawing.Point(250, 7);
            this.BtnExistsTable.Name = "BtnExistsTable";
            this.BtnExistsTable.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsTable.TabIndex = 113;
            this.BtnExistsTable.Text = "Exists Table ";
            this.BtnExistsTable.UseVisualStyleBackColor = true;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(6, 63);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdate.TabIndex = 112;
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(6, 9);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(238, 21);
            this.BtnAdd.TabIndex = 111;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 26);
            this.button1.TabIndex = 140;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmDemoSysworkData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 557);
            this.Controls.Add(this.GrpDetails);
            this.Controls.Add(this.GrpParameters);
            this.Name = "FrmDemoSysworkData";
            this.Text = "Test SysWork.Data";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTestDataV2_FormClosing);
            this.Load += new System.EventHandler(this.FrmTestDataV2_Load);
            this.GrpParameters.ResumeLayout(false);
            this.GrpParameters.PerformLayout();
            this.GrpDetails.ResumeLayout(false);
            this.GrpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox GrpParameters;
        private System.Windows.Forms.Button BtnUnconnect;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.Button BtnGetParameters;
        private System.Windows.Forms.TextBox TxtConnectionString;
        private System.Windows.Forms.ComboBox CmbDataBaseEngine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GrpDetails;
        private System.Windows.Forms.Button BtnTestGenericViewManager;
        private System.Windows.Forms.Button BtnSqlLAMTest;
        private System.Windows.Forms.Button BtnLogger;
        private System.Windows.Forms.Button BtnExecuteNonQueryWparam;
        private System.Windows.Forms.Button BtnExecuteNonQuery;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.TextBox TxtPassport;
        private System.Windows.Forms.Button BtnGetByPassport;
        private System.Windows.Forms.TextBox TxtId;
        private System.Windows.Forms.Button BtnGetByID;
        private System.Windows.Forms.TextBox TxtDelId;
        private System.Windows.Forms.Button DeleteByID;
        private System.Windows.Forms.Button BtnFind5;
        private System.Windows.Forms.Button BtnUpdateRange;
        private System.Windows.Forms.Button BtnDeleteAll;
        private System.Windows.Forms.Button BtnAddRange;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BtnSimpleQuery;
        private System.Windows.Forms.Button BtnExistsColumn;
        private System.Windows.Forms.Button BtnExistsTable;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnCLearLog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtLog;
        private System.Windows.Forms.Button BtnDeleteByLambdaExpression;
        private System.Windows.Forms.Button BtnGetByLambdaExpression;
        private System.Windows.Forms.Button BtnGetAll;
        private System.Windows.Forms.Button BtnEntityClassFromDb;
        private System.Windows.Forms.Button button1;
    }
}


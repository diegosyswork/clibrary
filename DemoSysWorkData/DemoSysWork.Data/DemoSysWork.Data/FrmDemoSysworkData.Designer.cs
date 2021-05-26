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
            this.CmbDatabaseEngine = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GrpDetails = new System.Windows.Forms.GroupBox();
            this.BtnAddRangeAsync = new System.Windows.Forms.Button();
            this.BtnGetAllAsync = new System.Windows.Forms.Button();
            this.BtnTestMapper = new System.Windows.Forms.Button();
            this.BtnGetByGenericWhere = new System.Windows.Forms.Button();
            this.BtnGetListByGenericWhere = new System.Windows.Forms.Button();
            this.BtnDeleteByGenericWhere = new System.Windows.Forms.Button();
            this.BtnTestWhereFilter = new System.Windows.Forms.Button();
            this.BtnTestDbExecutor = new System.Windows.Forms.Button();
            this.BtnRepositoryClassFromDb = new System.Windows.Forms.Button();
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
            this.GrpParameters.Controls.Add(this.CmbDatabaseEngine);
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
            // CmbDatabaseEngine
            // 
            this.CmbDatabaseEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbDatabaseEngine.FormattingEnabled = true;
            this.CmbDatabaseEngine.Location = new System.Drawing.Point(107, 19);
            this.CmbDatabaseEngine.Name = "CmbDatabaseEngine";
            this.CmbDatabaseEngine.Size = new System.Drawing.Size(761, 21);
            this.CmbDatabaseEngine.TabIndex = 2;
            this.CmbDatabaseEngine.SelectedValueChanged += new System.EventHandler(this.CmbDatabaseEngine_SelectedValueChanged);
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
            this.GrpDetails.Controls.Add(this.BtnAddRangeAsync);
            this.GrpDetails.Controls.Add(this.BtnGetAllAsync);
            this.GrpDetails.Controls.Add(this.BtnTestMapper);
            this.GrpDetails.Controls.Add(this.BtnGetByGenericWhere);
            this.GrpDetails.Controls.Add(this.BtnGetListByGenericWhere);
            this.GrpDetails.Controls.Add(this.BtnDeleteByGenericWhere);
            this.GrpDetails.Controls.Add(this.BtnTestWhereFilter);
            this.GrpDetails.Controls.Add(this.BtnTestDbExecutor);
            this.GrpDetails.Controls.Add(this.BtnRepositoryClassFromDb);
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
            // BtnAddRangeAsync
            // 
            this.BtnAddRangeAsync.Location = new System.Drawing.Point(250, 326);
            this.BtnAddRangeAsync.Name = "BtnAddRangeAsync";
            this.BtnAddRangeAsync.Size = new System.Drawing.Size(102, 21);
            this.BtnAddRangeAsync.TabIndex = 149;
            this.BtnAddRangeAsync.Text = "AddRange Async";
            this.BtnAddRangeAsync.UseVisualStyleBackColor = true;
            this.BtnAddRangeAsync.Click += new System.EventHandler(this.BtnAddRangeAsync_Click);
            // 
            // BtnGetAllAsync
            // 
            this.BtnGetAllAsync.Location = new System.Drawing.Point(250, 299);
            this.BtnGetAllAsync.Name = "BtnGetAllAsync";
            this.BtnGetAllAsync.Size = new System.Drawing.Size(102, 21);
            this.BtnGetAllAsync.TabIndex = 148;
            this.BtnGetAllAsync.Text = "Get All Async";
            this.BtnGetAllAsync.UseVisualStyleBackColor = true;
            this.BtnGetAllAsync.Click += new System.EventHandler(this.BtnGetAllAsync_Click);
            // 
            // BtnTestMapper
            // 
            this.BtnTestMapper.Location = new System.Drawing.Point(250, 252);
            this.BtnTestMapper.Name = "BtnTestMapper";
            this.BtnTestMapper.Size = new System.Drawing.Size(238, 21);
            this.BtnTestMapper.TabIndex = 147;
            this.BtnTestMapper.Text = "Test Mapper";
            this.BtnTestMapper.UseVisualStyleBackColor = true;
            this.BtnTestMapper.Click += new System.EventHandler(this.BtnTestMapper_Click);
            // 
            // BtnGetByGenericWhere
            // 
            this.BtnGetByGenericWhere.Location = new System.Drawing.Point(6, 326);
            this.BtnGetByGenericWhere.Name = "BtnGetByGenericWhere";
            this.BtnGetByGenericWhere.Size = new System.Drawing.Size(238, 21);
            this.BtnGetByGenericWhere.TabIndex = 146;
            this.BtnGetByGenericWhere.Text = "Get By Generic Where";
            this.BtnGetByGenericWhere.UseVisualStyleBackColor = true;
            // 
            // BtnGetListByGenericWhere
            // 
            this.BtnGetListByGenericWhere.Location = new System.Drawing.Point(88, 247);
            this.BtnGetListByGenericWhere.Name = "BtnGetListByGenericWhere";
            this.BtnGetListByGenericWhere.Size = new System.Drawing.Size(156, 21);
            this.BtnGetListByGenericWhere.TabIndex = 145;
            this.BtnGetListByGenericWhere.Text = "Get List by Generic Where";
            this.BtnGetListByGenericWhere.UseVisualStyleBackColor = true;
            // 
            // BtnDeleteByGenericWhere
            // 
            this.BtnDeleteByGenericWhere.Location = new System.Drawing.Point(88, 193);
            this.BtnDeleteByGenericWhere.Name = "BtnDeleteByGenericWhere";
            this.BtnDeleteByGenericWhere.Size = new System.Drawing.Size(156, 21);
            this.BtnDeleteByGenericWhere.TabIndex = 144;
            this.BtnDeleteByGenericWhere.Text = "Delete By Generic Where";
            this.BtnDeleteByGenericWhere.UseVisualStyleBackColor = true;
            // 
            // BtnTestWhereFilter
            // 
            this.BtnTestWhereFilter.Location = new System.Drawing.Point(250, 225);
            this.BtnTestWhereFilter.Name = "BtnTestWhereFilter";
            this.BtnTestWhereFilter.Size = new System.Drawing.Size(238, 21);
            this.BtnTestWhereFilter.TabIndex = 143;
            this.BtnTestWhereFilter.Text = "Test GenericWhereFilter";
            this.BtnTestWhereFilter.UseVisualStyleBackColor = true;
            this.BtnTestWhereFilter.Click += new System.EventHandler(this.BtnTestWhereFilter_Click);
            // 
            // BtnTestDbExecutor
            // 
            this.BtnTestDbExecutor.Location = new System.Drawing.Point(250, 61);
            this.BtnTestDbExecutor.Name = "BtnTestDbExecutor";
            this.BtnTestDbExecutor.Size = new System.Drawing.Size(238, 20);
            this.BtnTestDbExecutor.TabIndex = 142;
            this.BtnTestDbExecutor.Text = "TestDbExecutor";
            this.BtnTestDbExecutor.UseVisualStyleBackColor = true;
            this.BtnTestDbExecutor.Click += new System.EventHandler(this.BtnTestDbExecutor_Click);
            // 
            // BtnRepositoryClassFromDb
            // 
            this.BtnRepositoryClassFromDb.Location = new System.Drawing.Point(250, 115);
            this.BtnRepositoryClassFromDb.Name = "BtnRepositoryClassFromDb";
            this.BtnRepositoryClassFromDb.Size = new System.Drawing.Size(238, 21);
            this.BtnRepositoryClassFromDb.TabIndex = 141;
            this.BtnRepositoryClassFromDb.Text = "Test RepositoryClassFromDb";
            this.BtnRepositoryClassFromDb.UseVisualStyleBackColor = true;
            this.BtnRepositoryClassFromDb.Click += new System.EventHandler(this.BtnRepositoryClassFromDb_Click);
            // 
            // BtnEntityClassFromDb
            // 
            this.BtnEntityClassFromDb.Location = new System.Drawing.Point(250, 89);
            this.BtnEntityClassFromDb.Name = "BtnEntityClassFromDb";
            this.BtnEntityClassFromDb.Size = new System.Drawing.Size(238, 20);
            this.BtnEntityClassFromDb.TabIndex = 94;
            this.BtnEntityClassFromDb.Text = "Test EntityClassFromDb";
            this.BtnEntityClassFromDb.UseVisualStyleBackColor = true;
            this.BtnEntityClassFromDb.Click += new System.EventHandler(this.BtnTestEntityClassFromDb_Click);
            // 
            // BtnGetByLambdaExpression
            // 
            this.BtnGetByLambdaExpression.Location = new System.Drawing.Point(88, 220);
            this.BtnGetByLambdaExpression.Name = "BtnGetByLambdaExpression";
            this.BtnGetByLambdaExpression.Size = new System.Drawing.Size(156, 21);
            this.BtnGetByLambdaExpression.TabIndex = 139;
            this.BtnGetByLambdaExpression.Text = "Get List By Lambda Expression";
            this.BtnGetByLambdaExpression.UseVisualStyleBackColor = true;
            this.BtnGetByLambdaExpression.Click += new System.EventHandler(this.BtnGetByLambdaExpression_Click);
            // 
            // BtnGetAll
            // 
            this.BtnGetAll.Location = new System.Drawing.Point(6, 220);
            this.BtnGetAll.Name = "BtnGetAll";
            this.BtnGetAll.Size = new System.Drawing.Size(76, 21);
            this.BtnGetAll.TabIndex = 138;
            this.BtnGetAll.Text = "Get All ";
            this.BtnGetAll.UseVisualStyleBackColor = true;
            this.BtnGetAll.Click += new System.EventHandler(this.BtnGetAll_Click);
            // 
            // BtnDeleteByLambdaExpression
            // 
            this.BtnDeleteByLambdaExpression.Location = new System.Drawing.Point(88, 169);
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
            this.label3.Location = new System.Drawing.Point(498, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "Log";
            // 
            // TxtLog
            // 
            this.TxtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtLog.Location = new System.Drawing.Point(501, 34);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.Size = new System.Drawing.Size(367, 266);
            this.TxtLog.TabIndex = 134;
            // 
            // BtnTestGenericViewManager
            // 
            this.BtnTestGenericViewManager.Location = new System.Drawing.Point(250, 198);
            this.BtnTestGenericViewManager.Name = "BtnTestGenericViewManager";
            this.BtnTestGenericViewManager.Size = new System.Drawing.Size(238, 21);
            this.BtnTestGenericViewManager.TabIndex = 133;
            this.BtnTestGenericViewManager.Text = "Test generic View";
            this.BtnTestGenericViewManager.UseVisualStyleBackColor = true;
            this.BtnTestGenericViewManager.Click += new System.EventHandler(this.BtnTestGenericViewManager_Click);
            // 
            // BtnSqlLAMTest
            // 
            this.BtnSqlLAMTest.Location = new System.Drawing.Point(250, 171);
            this.BtnSqlLAMTest.Name = "BtnSqlLAMTest";
            this.BtnSqlLAMTest.Size = new System.Drawing.Size(238, 21);
            this.BtnSqlLAMTest.TabIndex = 132;
            this.BtnSqlLAMTest.Text = "SQLLam Test";
            this.BtnSqlLAMTest.UseVisualStyleBackColor = true;
            this.BtnSqlLAMTest.Click += new System.EventHandler(this.BtnSqlLAMTest_Click);
            // 
            // BtnLogger
            // 
            this.BtnLogger.Location = new System.Drawing.Point(250, 142);
            this.BtnLogger.Name = "BtnLogger";
            this.BtnLogger.Size = new System.Drawing.Size(238, 23);
            this.BtnLogger.TabIndex = 131;
            this.BtnLogger.Text = "Logger";
            this.BtnLogger.UseVisualStyleBackColor = true;
            this.BtnLogger.Click += new System.EventHandler(this.BtnLogger_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Location = new System.Drawing.Point(6, 353);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(862, 35);
            this.txtQuery.TabIndex = 127;
            // 
            // TxtPassport
            // 
            this.TxtPassport.Location = new System.Drawing.Point(142, 300);
            this.TxtPassport.Name = "TxtPassport";
            this.TxtPassport.Size = new System.Drawing.Size(102, 20);
            this.TxtPassport.TabIndex = 126;
            this.TxtPassport.Text = "0";
            // 
            // BtnGetByPassport
            // 
            this.BtnGetByPassport.Location = new System.Drawing.Point(6, 299);
            this.BtnGetByPassport.Name = "BtnGetByPassport";
            this.BtnGetByPassport.Size = new System.Drawing.Size(130, 21);
            this.BtnGetByPassport.TabIndex = 125;
            this.BtnGetByPassport.Text = "Get By Passport";
            this.BtnGetByPassport.UseVisualStyleBackColor = true;
            this.BtnGetByPassport.Click += new System.EventHandler(this.BtnGetByPassport_Click);
            // 
            // TxtId
            // 
            this.TxtId.Location = new System.Drawing.Point(196, 275);
            this.TxtId.Name = "TxtId";
            this.TxtId.Size = new System.Drawing.Size(48, 20);
            this.TxtId.TabIndex = 124;
            this.TxtId.Text = "0";
            // 
            // BtnGetByID
            // 
            this.BtnGetByID.Location = new System.Drawing.Point(6, 274);
            this.BtnGetByID.Name = "BtnGetByID";
            this.BtnGetByID.Size = new System.Drawing.Size(184, 21);
            this.BtnGetByID.TabIndex = 123;
            this.BtnGetByID.Text = "Get By Id";
            this.BtnGetByID.UseVisualStyleBackColor = true;
            this.BtnGetByID.Click += new System.EventHandler(this.BtnGetByID_Click);
            // 
            // TxtDelId
            // 
            this.TxtDelId.Location = new System.Drawing.Point(196, 143);
            this.TxtDelId.Name = "TxtDelId";
            this.TxtDelId.Size = new System.Drawing.Size(48, 20);
            this.TxtDelId.TabIndex = 122;
            this.TxtDelId.Text = "0";
            // 
            // DeleteByID
            // 
            this.DeleteByID.Location = new System.Drawing.Point(6, 142);
            this.DeleteByID.Name = "DeleteByID";
            this.DeleteByID.Size = new System.Drawing.Size(184, 20);
            this.DeleteByID.TabIndex = 121;
            this.DeleteByID.Text = "Delete By Id";
            this.DeleteByID.UseVisualStyleBackColor = true;
            this.DeleteByID.Click += new System.EventHandler(this.DeleteByID_Click);
            // 
            // BtnFind5
            // 
            this.BtnFind5.Location = new System.Drawing.Point(6, 115);
            this.BtnFind5.Name = "BtnFind5";
            this.BtnFind5.Size = new System.Drawing.Size(238, 21);
            this.BtnFind5.TabIndex = 120;
            this.BtnFind5.Text = "Find 5 ";
            this.BtnFind5.UseVisualStyleBackColor = true;
            this.BtnFind5.Click += new System.EventHandler(this.BtnFind5_Click);
            // 
            // BtnUpdateRange
            // 
            this.BtnUpdateRange.Location = new System.Drawing.Point(6, 88);
            this.BtnUpdateRange.Name = "BtnUpdateRange";
            this.BtnUpdateRange.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdateRange.TabIndex = 119;
            this.BtnUpdateRange.Text = "UpdateRange ";
            this.BtnUpdateRange.UseVisualStyleBackColor = true;
            this.BtnUpdateRange.Click += new System.EventHandler(this.BtnUpdateRange_Click);
            // 
            // BtnDeleteAll
            // 
            this.BtnDeleteAll.Location = new System.Drawing.Point(6, 169);
            this.BtnDeleteAll.Name = "BtnDeleteAll";
            this.BtnDeleteAll.Size = new System.Drawing.Size(76, 21);
            this.BtnDeleteAll.TabIndex = 118;
            this.BtnDeleteAll.Text = "Delete All ";
            this.BtnDeleteAll.UseVisualStyleBackColor = true;
            this.BtnDeleteAll.Click += new System.EventHandler(this.BtnDeleteAll_Click);
            // 
            // BtnAddRange
            // 
            this.BtnAddRange.Location = new System.Drawing.Point(6, 33);
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
            // BtnExistsColumn
            // 
            this.BtnExistsColumn.Location = new System.Drawing.Point(250, 34);
            this.BtnExistsColumn.Name = "BtnExistsColumn";
            this.BtnExistsColumn.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsColumn.TabIndex = 114;
            this.BtnExistsColumn.Text = "Exists Column ";
            this.BtnExistsColumn.UseVisualStyleBackColor = true;
            this.BtnExistsColumn.Click += new System.EventHandler(this.BtnExistsColumn_Click);
            // 
            // BtnExistsTable
            // 
            this.BtnExistsTable.Location = new System.Drawing.Point(250, 7);
            this.BtnExistsTable.Name = "BtnExistsTable";
            this.BtnExistsTable.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsTable.TabIndex = 113;
            this.BtnExistsTable.Text = "Exists Table ";
            this.BtnExistsTable.UseVisualStyleBackColor = true;
            this.BtnExistsTable.Click += new System.EventHandler(this.BtnExistsTable_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(6, 61);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdate.TabIndex = 112;
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(6, 7);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(238, 21);
            this.BtnAdd.TabIndex = 111;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(431, 311);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(18, 15);
            this.button1.TabIndex = 150;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDemoSysworkData_FormClosing);
            this.Load += new System.EventHandler(this.FrmDemoSysworkData_Load);
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
        private System.Windows.Forms.ComboBox CmbDatabaseEngine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GrpDetails;
        private System.Windows.Forms.Button BtnTestGenericViewManager;
        private System.Windows.Forms.Button BtnSqlLAMTest;
        private System.Windows.Forms.Button BtnLogger;
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
        private System.Windows.Forms.Button BtnRepositoryClassFromDb;
        private System.Windows.Forms.Button BtnTestDbExecutor;
        private System.Windows.Forms.Button BtnTestWhereFilter;
        private System.Windows.Forms.Button BtnDeleteByGenericWhere;
        private System.Windows.Forms.Button BtnGetListByGenericWhere;
        private System.Windows.Forms.Button BtnGetByGenericWhere;
        private System.Windows.Forms.Button BtnTestMapper;
        private System.Windows.Forms.Button BtnGetAllAsync;
        private System.Windows.Forms.Button BtnAddRangeAsync;
        private System.Windows.Forms.Button button1;
    }
}


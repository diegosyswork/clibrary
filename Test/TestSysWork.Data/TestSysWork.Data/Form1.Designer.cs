namespace TestDaoModelDataCommon
{
    partial class Form1
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
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.BtnExistsTable = new System.Windows.Forms.Button();
            this.BtnExistsColumn = new System.Windows.Forms.Button();
            this.BtnSimpleQuery = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BtnAddRange = new System.Windows.Forms.Button();
            this.BtnDeleteAll = new System.Windows.Forms.Button();
            this.BtnUpdateRange = new System.Windows.Forms.Button();
            this.BtnFind5 = new System.Windows.Forms.Button();
            this.DeleteByID = new System.Windows.Forms.Button();
            this.txtDELId = new System.Windows.Forms.TextBox();
            this.txtGETId = new System.Windows.Forms.TextBox();
            this.BtnGetByID = new System.Windows.Forms.Button();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.BtnGetByDni = new System.Windows.Forms.Button();
            this.BtnAddSQL = new System.Windows.Forms.Button();
            this.BtnAddRangeSQL = new System.Windows.Forms.Button();
            this.BtnUpdateSQL = new System.Windows.Forms.Button();
            this.BtnUpdateRangeSQL = new System.Windows.Forms.Button();
            this.BtnFind5SQL = new System.Windows.Forms.Button();
            this.txtDELSQLId = new System.Windows.Forms.TextBox();
            this.BtnDeleteById = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtGETSQLId = new System.Windows.Forms.TextBox();
            this.BtnGetByIDSQL = new System.Windows.Forms.Button();
            this.txtDNISQL = new System.Windows.Forms.TextBox();
            this.BtnGetByDniSQL = new System.Windows.Forms.Button();
            this.BtnSimpleQuerySQL = new System.Windows.Forms.Button();
            this.BtnExistsColumnSQL = new System.Windows.Forms.Button();
            this.BtnExistsTableSQL = new System.Windows.Forms.Button();
            this.BtnSimpleQueryOleDb = new System.Windows.Forms.Button();
            this.BtnExistsColumnOleDb = new System.Windows.Forms.Button();
            this.BtnExistsTableOleDb = new System.Windows.Forms.Button();
            this.txtDNIOLEDB = new System.Windows.Forms.TextBox();
            this.GetByDniOleDb = new System.Windows.Forms.Button();
            this.txtGETOLEDBId = new System.Windows.Forms.TextBox();
            this.BtnGetByIdOleDb = new System.Windows.Forms.Button();
            this.BtnDeleteAllOleDb = new System.Windows.Forms.Button();
            this.txtDELOLEDBId = new System.Windows.Forms.TextBox();
            this.BtnDeleteByIdOleDb = new System.Windows.Forms.Button();
            this.BtnFind5OleDb = new System.Windows.Forms.Button();
            this.UpdateRangeOleDb = new System.Windows.Forms.Button();
            this.BtnUpdateOleDb = new System.Windows.Forms.Button();
            this.BtnAddRangeOleDb = new System.Windows.Forms.Button();
            this.BtnAddOleDb = new System.Windows.Forms.Button();
            this.BtnAddRangeCRepetidosOleDb = new System.Windows.Forms.Button();
            this.BtnAddRangeCRepetidosSQL = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(12, 12);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(238, 21);
            this.BtnAdd.TabIndex = 0;
            this.BtnAdd.Text = "Add SQLite";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAddSQLite_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(12, 66);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdate.TabIndex = 1;
            this.BtnUpdate.Text = "Update SQLite";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdateSQLite_Click);
            // 
            // BtnExistsTable
            // 
            this.BtnExistsTable.Location = new System.Drawing.Point(15, 253);
            this.BtnExistsTable.Name = "BtnExistsTable";
            this.BtnExistsTable.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsTable.TabIndex = 2;
            this.BtnExistsTable.Text = "Exists Table SQLite";
            this.BtnExistsTable.UseVisualStyleBackColor = true;
            this.BtnExistsTable.Click += new System.EventHandler(this.BtnExistsTableSQLite_Click);
            // 
            // BtnExistsColumn
            // 
            this.BtnExistsColumn.Location = new System.Drawing.Point(15, 280);
            this.BtnExistsColumn.Name = "BtnExistsColumn";
            this.BtnExistsColumn.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsColumn.TabIndex = 3;
            this.BtnExistsColumn.Text = "Exists Column SQLite";
            this.BtnExistsColumn.UseVisualStyleBackColor = true;
            this.BtnExistsColumn.Click += new System.EventHandler(this.BtnExistsColumnSQLite_Click);
            // 
            // BtnSimpleQuery
            // 
            this.BtnSimpleQuery.Location = new System.Drawing.Point(15, 307);
            this.BtnSimpleQuery.Name = "BtnSimpleQuery";
            this.BtnSimpleQuery.Size = new System.Drawing.Size(238, 21);
            this.BtnSimpleQuery.TabIndex = 4;
            this.BtnSimpleQuery.Text = "SimpleQuery SQLite (Selecciona Todos)";
            this.BtnSimpleQuery.UseVisualStyleBackColor = true;
            this.BtnSimpleQuery.Click += new System.EventHandler(this.BtnSimpleQuerySQLite_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 334);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(878, 134);
            this.dataGridView1.TabIndex = 5;
            // 
            // BtnAddRange
            // 
            this.BtnAddRange.Location = new System.Drawing.Point(12, 39);
            this.BtnAddRange.Name = "BtnAddRange";
            this.BtnAddRange.Size = new System.Drawing.Size(238, 21);
            this.BtnAddRange.TabIndex = 6;
            this.BtnAddRange.Text = "AddRange SQLite";
            this.BtnAddRange.UseVisualStyleBackColor = true;
            this.BtnAddRange.Click += new System.EventHandler(this.BtnAddRangeSQLite_Click);
            // 
            // BtnDeleteAll
            // 
            this.BtnDeleteAll.Location = new System.Drawing.Point(12, 174);
            this.BtnDeleteAll.Name = "BtnDeleteAll";
            this.BtnDeleteAll.Size = new System.Drawing.Size(238, 21);
            this.BtnDeleteAll.TabIndex = 7;
            this.BtnDeleteAll.Text = "Delete All SQLite";
            this.BtnDeleteAll.UseVisualStyleBackColor = true;
            this.BtnDeleteAll.Click += new System.EventHandler(this.BtnDeleteAllSQLite_Click);
            // 
            // BtnUpdateRange
            // 
            this.BtnUpdateRange.Location = new System.Drawing.Point(12, 93);
            this.BtnUpdateRange.Name = "BtnUpdateRange";
            this.BtnUpdateRange.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdateRange.TabIndex = 8;
            this.BtnUpdateRange.Text = "UpdateRange SQLite";
            this.BtnUpdateRange.UseVisualStyleBackColor = true;
            this.BtnUpdateRange.Click += new System.EventHandler(this.BtnUpdateRangeSQLite_Click);
            // 
            // BtnFind5
            // 
            this.BtnFind5.Location = new System.Drawing.Point(12, 120);
            this.BtnFind5.Name = "BtnFind5";
            this.BtnFind5.Size = new System.Drawing.Size(238, 21);
            this.BtnFind5.TabIndex = 9;
            this.BtnFind5.Text = "Find 5 SQLite";
            this.BtnFind5.UseVisualStyleBackColor = true;
            this.BtnFind5.Click += new System.EventHandler(this.BtnFind5SQLite_Click);
            // 
            // DeleteByID
            // 
            this.DeleteByID.Location = new System.Drawing.Point(12, 147);
            this.DeleteByID.Name = "DeleteByID";
            this.DeleteByID.Size = new System.Drawing.Size(184, 21);
            this.DeleteByID.TabIndex = 10;
            this.DeleteByID.Text = "Delete By Id SQLIte";
            this.DeleteByID.UseVisualStyleBackColor = true;
            this.DeleteByID.Click += new System.EventHandler(this.BtnDeleteByIDSQLite_Click);
            // 
            // txtDELId
            // 
            this.txtDELId.Location = new System.Drawing.Point(202, 148);
            this.txtDELId.Name = "txtDELId";
            this.txtDELId.Size = new System.Drawing.Size(48, 20);
            this.txtDELId.TabIndex = 11;
            this.txtDELId.Text = "0";
            // 
            // txtGETId
            // 
            this.txtGETId.Location = new System.Drawing.Point(202, 202);
            this.txtGETId.Name = "txtGETId";
            this.txtGETId.Size = new System.Drawing.Size(48, 20);
            this.txtGETId.TabIndex = 13;
            this.txtGETId.Text = "0";
            // 
            // BtnGetByID
            // 
            this.BtnGetByID.Location = new System.Drawing.Point(12, 201);
            this.BtnGetByID.Name = "BtnGetByID";
            this.BtnGetByID.Size = new System.Drawing.Size(184, 21);
            this.BtnGetByID.TabIndex = 12;
            this.BtnGetByID.Text = "Get By Id SQLite";
            this.BtnGetByID.UseVisualStyleBackColor = true;
            this.BtnGetByID.Click += new System.EventHandler(this.BtnGetByIDSQLite_Click);
            // 
            // txtDNI
            // 
            this.txtDNI.Location = new System.Drawing.Point(148, 227);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(102, 20);
            this.txtDNI.TabIndex = 15;
            this.txtDNI.Text = "0";
            // 
            // BtnGetByDni
            // 
            this.BtnGetByDni.Location = new System.Drawing.Point(12, 226);
            this.BtnGetByDni.Name = "BtnGetByDni";
            this.BtnGetByDni.Size = new System.Drawing.Size(130, 21);
            this.BtnGetByDni.TabIndex = 14;
            this.BtnGetByDni.Text = "Get By DNI SQLite";
            this.BtnGetByDni.UseVisualStyleBackColor = true;
            this.BtnGetByDni.Click += new System.EventHandler(this.BtnGetByDniSQLite_Click);
            // 
            // BtnAddSQL
            // 
            this.BtnAddSQL.Location = new System.Drawing.Point(279, 10);
            this.BtnAddSQL.Name = "BtnAddSQL";
            this.BtnAddSQL.Size = new System.Drawing.Size(238, 21);
            this.BtnAddSQL.TabIndex = 16;
            this.BtnAddSQL.Text = "Add SQLServer";
            this.BtnAddSQL.UseVisualStyleBackColor = true;
            this.BtnAddSQL.Click += new System.EventHandler(this.BtnAddSQL_Click);
            // 
            // BtnAddRangeSQL
            // 
            this.BtnAddRangeSQL.Location = new System.Drawing.Point(279, 37);
            this.BtnAddRangeSQL.Name = "BtnAddRangeSQL";
            this.BtnAddRangeSQL.Size = new System.Drawing.Size(121, 21);
            this.BtnAddRangeSQL.TabIndex = 17;
            this.BtnAddRangeSQL.Text = "AddRange SQLServer";
            this.BtnAddRangeSQL.UseVisualStyleBackColor = true;
            this.BtnAddRangeSQL.Click += new System.EventHandler(this.BtnAddRangeSQL_Click);
            // 
            // BtnUpdateSQL
            // 
            this.BtnUpdateSQL.Location = new System.Drawing.Point(279, 64);
            this.BtnUpdateSQL.Name = "BtnUpdateSQL";
            this.BtnUpdateSQL.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdateSQL.TabIndex = 18;
            this.BtnUpdateSQL.Text = "Update SQLServer";
            this.BtnUpdateSQL.UseVisualStyleBackColor = true;
            this.BtnUpdateSQL.Click += new System.EventHandler(this.BtnUpdatSQL_Click);
            // 
            // BtnUpdateRangeSQL
            // 
            this.BtnUpdateRangeSQL.Location = new System.Drawing.Point(279, 91);
            this.BtnUpdateRangeSQL.Name = "BtnUpdateRangeSQL";
            this.BtnUpdateRangeSQL.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdateRangeSQL.TabIndex = 19;
            this.BtnUpdateRangeSQL.Text = "UpdateRange SQLServer";
            this.BtnUpdateRangeSQL.UseVisualStyleBackColor = true;
            this.BtnUpdateRangeSQL.Click += new System.EventHandler(this.BtnUpdateRangeSQL_Click);
            // 
            // BtnFind5SQL
            // 
            this.BtnFind5SQL.Location = new System.Drawing.Point(279, 118);
            this.BtnFind5SQL.Name = "BtnFind5SQL";
            this.BtnFind5SQL.Size = new System.Drawing.Size(238, 21);
            this.BtnFind5SQL.TabIndex = 20;
            this.BtnFind5SQL.Text = "Find 5 SQLServer";
            this.BtnFind5SQL.UseVisualStyleBackColor = true;
            this.BtnFind5SQL.Click += new System.EventHandler(this.BtnFind5SQL_Click);
            // 
            // txtDELSQLId
            // 
            this.txtDELSQLId.Location = new System.Drawing.Point(469, 146);
            this.txtDELSQLId.Name = "txtDELSQLId";
            this.txtDELSQLId.Size = new System.Drawing.Size(48, 20);
            this.txtDELSQLId.TabIndex = 22;
            this.txtDELSQLId.Text = "0";
            // 
            // BtnDeleteById
            // 
            this.BtnDeleteById.Location = new System.Drawing.Point(279, 145);
            this.BtnDeleteById.Name = "BtnDeleteById";
            this.BtnDeleteById.Size = new System.Drawing.Size(184, 21);
            this.BtnDeleteById.TabIndex = 21;
            this.BtnDeleteById.Text = "Delete By Id SQLServer";
            this.BtnDeleteById.UseVisualStyleBackColor = true;
            this.BtnDeleteById.Click += new System.EventHandler(this.BtnDeleteByIDSQL_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(279, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(238, 21);
            this.button1.TabIndex = 23;
            this.button1.Text = "Delete All SQLServer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnDeleteAllSQL_Click);
            // 
            // txtGETSQLId
            // 
            this.txtGETSQLId.Location = new System.Drawing.Point(469, 200);
            this.txtGETSQLId.Name = "txtGETSQLId";
            this.txtGETSQLId.Size = new System.Drawing.Size(48, 20);
            this.txtGETSQLId.TabIndex = 25;
            this.txtGETSQLId.Text = "0";
            // 
            // BtnGetByIDSQL
            // 
            this.BtnGetByIDSQL.Location = new System.Drawing.Point(279, 199);
            this.BtnGetByIDSQL.Name = "BtnGetByIDSQL";
            this.BtnGetByIDSQL.Size = new System.Drawing.Size(184, 21);
            this.BtnGetByIDSQL.TabIndex = 24;
            this.BtnGetByIDSQL.Text = "Get By Id SQLServer";
            this.BtnGetByIDSQL.UseVisualStyleBackColor = true;
            this.BtnGetByIDSQL.Click += new System.EventHandler(this.BtnGetByIDSQL_Click);
            // 
            // txtDNISQL
            // 
            this.txtDNISQL.Location = new System.Drawing.Point(415, 227);
            this.txtDNISQL.Name = "txtDNISQL";
            this.txtDNISQL.Size = new System.Drawing.Size(102, 20);
            this.txtDNISQL.TabIndex = 27;
            this.txtDNISQL.Text = "0";
            // 
            // BtnGetByDniSQL
            // 
            this.BtnGetByDniSQL.Location = new System.Drawing.Point(279, 226);
            this.BtnGetByDniSQL.Name = "BtnGetByDniSQL";
            this.BtnGetByDniSQL.Size = new System.Drawing.Size(130, 21);
            this.BtnGetByDniSQL.TabIndex = 26;
            this.BtnGetByDniSQL.Text = "Get By DNI SQLServer";
            this.BtnGetByDniSQL.UseVisualStyleBackColor = true;
            this.BtnGetByDniSQL.Click += new System.EventHandler(this.BtnGetByDniSQL_Click);
            // 
            // BtnSimpleQuerySQL
            // 
            this.BtnSimpleQuerySQL.Location = new System.Drawing.Point(279, 307);
            this.BtnSimpleQuerySQL.Name = "BtnSimpleQuerySQL";
            this.BtnSimpleQuerySQL.Size = new System.Drawing.Size(238, 21);
            this.BtnSimpleQuerySQL.TabIndex = 30;
            this.BtnSimpleQuerySQL.Text = "SimpleQuery SQLServer (Selecciona Todos)";
            this.BtnSimpleQuerySQL.UseVisualStyleBackColor = true;
            this.BtnSimpleQuerySQL.Click += new System.EventHandler(this.BtnSimpleQuerySQL_Click);
            // 
            // BtnExistsColumnSQL
            // 
            this.BtnExistsColumnSQL.Location = new System.Drawing.Point(279, 280);
            this.BtnExistsColumnSQL.Name = "BtnExistsColumnSQL";
            this.BtnExistsColumnSQL.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsColumnSQL.TabIndex = 29;
            this.BtnExistsColumnSQL.Text = "Exists Column SQLServer";
            this.BtnExistsColumnSQL.UseVisualStyleBackColor = true;
            this.BtnExistsColumnSQL.Click += new System.EventHandler(this.BtnExistsColumnSQL_Click);
            // 
            // BtnExistsTableSQL
            // 
            this.BtnExistsTableSQL.Location = new System.Drawing.Point(279, 253);
            this.BtnExistsTableSQL.Name = "BtnExistsTableSQL";
            this.BtnExistsTableSQL.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsTableSQL.TabIndex = 28;
            this.BtnExistsTableSQL.Text = "Exists Table SQLServer";
            this.BtnExistsTableSQL.UseVisualStyleBackColor = true;
            this.BtnExistsTableSQL.Click += new System.EventHandler(this.BtnExistsTableSQL_Click);
            // 
            // BtnSimpleQueryOleDb
            // 
            this.BtnSimpleQueryOleDb.Location = new System.Drawing.Point(545, 307);
            this.BtnSimpleQueryOleDb.Name = "BtnSimpleQueryOleDb";
            this.BtnSimpleQueryOleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnSimpleQueryOleDb.TabIndex = 45;
            this.BtnSimpleQueryOleDb.Text = "SimpleQuery OleDb (Selecciona Todos)";
            this.BtnSimpleQueryOleDb.UseVisualStyleBackColor = true;
            this.BtnSimpleQueryOleDb.Click += new System.EventHandler(this.BtnSimpleQueryOleDb_Click);
            // 
            // BtnExistsColumnOleDb
            // 
            this.BtnExistsColumnOleDb.Location = new System.Drawing.Point(545, 280);
            this.BtnExistsColumnOleDb.Name = "BtnExistsColumnOleDb";
            this.BtnExistsColumnOleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsColumnOleDb.TabIndex = 44;
            this.BtnExistsColumnOleDb.Text = "Exists Column OleDb";
            this.BtnExistsColumnOleDb.UseVisualStyleBackColor = true;
            this.BtnExistsColumnOleDb.Click += new System.EventHandler(this.BtnExistsColumnOleDb_Click);
            // 
            // BtnExistsTableOleDb
            // 
            this.BtnExistsTableOleDb.Location = new System.Drawing.Point(545, 253);
            this.BtnExistsTableOleDb.Name = "BtnExistsTableOleDb";
            this.BtnExistsTableOleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnExistsTableOleDb.TabIndex = 43;
            this.BtnExistsTableOleDb.Text = "Exists Table OleDb";
            this.BtnExistsTableOleDb.UseVisualStyleBackColor = true;
            this.BtnExistsTableOleDb.Click += new System.EventHandler(this.BtnExistsTableOleDb_Click);
            // 
            // txtDNIOLEDB
            // 
            this.txtDNIOLEDB.Location = new System.Drawing.Point(681, 227);
            this.txtDNIOLEDB.Name = "txtDNIOLEDB";
            this.txtDNIOLEDB.Size = new System.Drawing.Size(102, 20);
            this.txtDNIOLEDB.TabIndex = 42;
            this.txtDNIOLEDB.Text = "0";
            // 
            // GetByDniOleDb
            // 
            this.GetByDniOleDb.Location = new System.Drawing.Point(545, 226);
            this.GetByDniOleDb.Name = "GetByDniOleDb";
            this.GetByDniOleDb.Size = new System.Drawing.Size(130, 21);
            this.GetByDniOleDb.TabIndex = 41;
            this.GetByDniOleDb.Text = "Get By DNI OleDb";
            this.GetByDniOleDb.UseVisualStyleBackColor = true;
            this.GetByDniOleDb.Click += new System.EventHandler(this.BtnGetByDniOleDb_Click);
            // 
            // txtGETOLEDBId
            // 
            this.txtGETOLEDBId.Location = new System.Drawing.Point(735, 200);
            this.txtGETOLEDBId.Name = "txtGETOLEDBId";
            this.txtGETOLEDBId.Size = new System.Drawing.Size(48, 20);
            this.txtGETOLEDBId.TabIndex = 40;
            this.txtGETOLEDBId.Text = "0";
            // 
            // BtnGetByIdOleDb
            // 
            this.BtnGetByIdOleDb.Location = new System.Drawing.Point(545, 199);
            this.BtnGetByIdOleDb.Name = "BtnGetByIdOleDb";
            this.BtnGetByIdOleDb.Size = new System.Drawing.Size(184, 21);
            this.BtnGetByIdOleDb.TabIndex = 39;
            this.BtnGetByIdOleDb.Text = "Get By Id OleDb";
            this.BtnGetByIdOleDb.UseVisualStyleBackColor = true;
            this.BtnGetByIdOleDb.Click += new System.EventHandler(this.BtnGetByIDOleDb_Click);
            // 
            // BtnDeleteAllOleDb
            // 
            this.BtnDeleteAllOleDb.Location = new System.Drawing.Point(545, 172);
            this.BtnDeleteAllOleDb.Name = "BtnDeleteAllOleDb";
            this.BtnDeleteAllOleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnDeleteAllOleDb.TabIndex = 38;
            this.BtnDeleteAllOleDb.Text = "Delete All OleDb";
            this.BtnDeleteAllOleDb.UseVisualStyleBackColor = true;
            this.BtnDeleteAllOleDb.Click += new System.EventHandler(this.BtnDeleteAllOleDb_Click);
            // 
            // txtDELOLEDBId
            // 
            this.txtDELOLEDBId.Location = new System.Drawing.Point(735, 146);
            this.txtDELOLEDBId.Name = "txtDELOLEDBId";
            this.txtDELOLEDBId.Size = new System.Drawing.Size(48, 20);
            this.txtDELOLEDBId.TabIndex = 37;
            this.txtDELOLEDBId.Text = "0";
            // 
            // BtnDeleteByIdOleDb
            // 
            this.BtnDeleteByIdOleDb.Location = new System.Drawing.Point(545, 145);
            this.BtnDeleteByIdOleDb.Name = "BtnDeleteByIdOleDb";
            this.BtnDeleteByIdOleDb.Size = new System.Drawing.Size(184, 21);
            this.BtnDeleteByIdOleDb.TabIndex = 36;
            this.BtnDeleteByIdOleDb.Text = "Delete By Id OleDb";
            this.BtnDeleteByIdOleDb.UseVisualStyleBackColor = true;
            this.BtnDeleteByIdOleDb.Click += new System.EventHandler(this.BtnDeleteByIDOleDb_Click);
            // 
            // BtnFind5OleDb
            // 
            this.BtnFind5OleDb.Location = new System.Drawing.Point(545, 118);
            this.BtnFind5OleDb.Name = "BtnFind5OleDb";
            this.BtnFind5OleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnFind5OleDb.TabIndex = 35;
            this.BtnFind5OleDb.Text = "Find 5 OleDb";
            this.BtnFind5OleDb.UseVisualStyleBackColor = true;
            this.BtnFind5OleDb.Click += new System.EventHandler(this.BtnFind5OleDb_Click);
            // 
            // UpdateRangeOleDb
            // 
            this.UpdateRangeOleDb.Location = new System.Drawing.Point(545, 91);
            this.UpdateRangeOleDb.Name = "UpdateRangeOleDb";
            this.UpdateRangeOleDb.Size = new System.Drawing.Size(238, 21);
            this.UpdateRangeOleDb.TabIndex = 34;
            this.UpdateRangeOleDb.Text = "UpdateRange OleDb";
            this.UpdateRangeOleDb.UseVisualStyleBackColor = true;
            this.UpdateRangeOleDb.Click += new System.EventHandler(this.BtnUpdateRangeOleDb_Click);
            // 
            // BtnUpdateOleDb
            // 
            this.BtnUpdateOleDb.Location = new System.Drawing.Point(545, 64);
            this.BtnUpdateOleDb.Name = "BtnUpdateOleDb";
            this.BtnUpdateOleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnUpdateOleDb.TabIndex = 33;
            this.BtnUpdateOleDb.Text = "Update OleDb";
            this.BtnUpdateOleDb.UseVisualStyleBackColor = true;
            this.BtnUpdateOleDb.Click += new System.EventHandler(this.BtnUpdateOleDb_Click);
            // 
            // BtnAddRangeOleDb
            // 
            this.BtnAddRangeOleDb.Location = new System.Drawing.Point(545, 37);
            this.BtnAddRangeOleDb.Name = "BtnAddRangeOleDb";
            this.BtnAddRangeOleDb.Size = new System.Drawing.Size(99, 21);
            this.BtnAddRangeOleDb.TabIndex = 32;
            this.BtnAddRangeOleDb.Text = "AddRange OleDb";
            this.BtnAddRangeOleDb.UseVisualStyleBackColor = true;
            this.BtnAddRangeOleDb.Click += new System.EventHandler(this.BtnAddRangeOleDb_Click);
            // 
            // BtnAddOleDb
            // 
            this.BtnAddOleDb.Location = new System.Drawing.Point(545, 10);
            this.BtnAddOleDb.Name = "BtnAddOleDb";
            this.BtnAddOleDb.Size = new System.Drawing.Size(238, 21);
            this.BtnAddOleDb.TabIndex = 31;
            this.BtnAddOleDb.Text = "Add OleDb";
            this.BtnAddOleDb.UseVisualStyleBackColor = true;
            this.BtnAddOleDb.Click += new System.EventHandler(this.BtnAddOleDb_Click);
            // 
            // BtnAddRangeCRepetidosOleDb
            // 
            this.BtnAddRangeCRepetidosOleDb.Location = new System.Drawing.Point(650, 37);
            this.BtnAddRangeCRepetidosOleDb.Name = "BtnAddRangeCRepetidosOleDb";
            this.BtnAddRangeCRepetidosOleDb.Size = new System.Drawing.Size(133, 21);
            this.BtnAddRangeCRepetidosOleDb.TabIndex = 46;
            this.BtnAddRangeCRepetidosOleDb.Text = "AddRange C/Repetidos";
            this.BtnAddRangeCRepetidosOleDb.UseVisualStyleBackColor = true;
            this.BtnAddRangeCRepetidosOleDb.Click += new System.EventHandler(this.BtnAddRangeCRepetidosOleDb_Click);
            // 
            // BtnAddRangeCRepetidosSQL
            // 
            this.BtnAddRangeCRepetidosSQL.Location = new System.Drawing.Point(406, 37);
            this.BtnAddRangeCRepetidosSQL.Name = "BtnAddRangeCRepetidosSQL";
            this.BtnAddRangeCRepetidosSQL.Size = new System.Drawing.Size(111, 21);
            this.BtnAddRangeCRepetidosSQL.TabIndex = 47;
            this.BtnAddRangeCRepetidosSQL.Text = "AddRange C/Repet";
            this.BtnAddRangeCRepetidosSQL.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 480);
            this.Controls.Add(this.BtnAddRangeCRepetidosSQL);
            this.Controls.Add(this.BtnAddRangeCRepetidosOleDb);
            this.Controls.Add(this.BtnSimpleQueryOleDb);
            this.Controls.Add(this.BtnExistsColumnOleDb);
            this.Controls.Add(this.BtnExistsTableOleDb);
            this.Controls.Add(this.txtDNIOLEDB);
            this.Controls.Add(this.GetByDniOleDb);
            this.Controls.Add(this.txtGETOLEDBId);
            this.Controls.Add(this.BtnGetByIdOleDb);
            this.Controls.Add(this.BtnDeleteAllOleDb);
            this.Controls.Add(this.txtDELOLEDBId);
            this.Controls.Add(this.BtnDeleteByIdOleDb);
            this.Controls.Add(this.BtnFind5OleDb);
            this.Controls.Add(this.UpdateRangeOleDb);
            this.Controls.Add(this.BtnUpdateOleDb);
            this.Controls.Add(this.BtnAddRangeOleDb);
            this.Controls.Add(this.BtnAddOleDb);
            this.Controls.Add(this.BtnSimpleQuerySQL);
            this.Controls.Add(this.BtnExistsColumnSQL);
            this.Controls.Add(this.BtnExistsTableSQL);
            this.Controls.Add(this.txtDNISQL);
            this.Controls.Add(this.BtnGetByDniSQL);
            this.Controls.Add(this.txtGETSQLId);
            this.Controls.Add(this.BtnGetByIDSQL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDELSQLId);
            this.Controls.Add(this.BtnDeleteById);
            this.Controls.Add(this.BtnFind5SQL);
            this.Controls.Add(this.BtnUpdateRangeSQL);
            this.Controls.Add(this.BtnUpdateSQL);
            this.Controls.Add(this.BtnAddRangeSQL);
            this.Controls.Add(this.BtnAddSQL);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.BtnGetByDni);
            this.Controls.Add(this.txtGETId);
            this.Controls.Add(this.BtnGetByID);
            this.Controls.Add(this.txtDELId);
            this.Controls.Add(this.DeleteByID);
            this.Controls.Add(this.BtnFind5);
            this.Controls.Add(this.BtnUpdateRange);
            this.Controls.Add(this.BtnDeleteAll);
            this.Controls.Add(this.BtnAddRange);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.BtnSimpleQuery);
            this.Controls.Add(this.BtnExistsColumn);
            this.Controls.Add(this.BtnExistsTable);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.BtnAdd);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Button BtnExistsTable;
        private System.Windows.Forms.Button BtnExistsColumn;
        private System.Windows.Forms.Button BtnSimpleQuery;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BtnAddRange;
        private System.Windows.Forms.Button BtnDeleteAll;
        private System.Windows.Forms.Button BtnUpdateRange;
        private System.Windows.Forms.Button BtnFind5;
        private System.Windows.Forms.Button DeleteByID;
        private System.Windows.Forms.TextBox txtDELId;
        private System.Windows.Forms.TextBox txtGETId;
        private System.Windows.Forms.Button BtnGetByID;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Button BtnGetByDni;
        private System.Windows.Forms.Button BtnAddSQL;
        private System.Windows.Forms.Button BtnAddRangeSQL;
        private System.Windows.Forms.Button BtnUpdateSQL;
        private System.Windows.Forms.Button BtnUpdateRangeSQL;
        private System.Windows.Forms.Button BtnFind5SQL;
        private System.Windows.Forms.TextBox txtDELSQLId;
        private System.Windows.Forms.Button BtnDeleteById;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtGETSQLId;
        private System.Windows.Forms.Button BtnGetByIDSQL;
        private System.Windows.Forms.TextBox txtDNISQL;
        private System.Windows.Forms.Button BtnGetByDniSQL;
        private System.Windows.Forms.Button BtnSimpleQuerySQL;
        private System.Windows.Forms.Button BtnExistsColumnSQL;
        private System.Windows.Forms.Button BtnExistsTableSQL;
        private System.Windows.Forms.Button BtnSimpleQueryOleDb;
        private System.Windows.Forms.Button BtnExistsColumnOleDb;
        private System.Windows.Forms.Button BtnExistsTableOleDb;
        private System.Windows.Forms.TextBox txtDNIOLEDB;
        private System.Windows.Forms.Button GetByDniOleDb;
        private System.Windows.Forms.TextBox txtGETOLEDBId;
        private System.Windows.Forms.Button BtnGetByIdOleDb;
        private System.Windows.Forms.Button BtnDeleteAllOleDb;
        private System.Windows.Forms.TextBox txtDELOLEDBId;
        private System.Windows.Forms.Button BtnDeleteByIdOleDb;
        private System.Windows.Forms.Button BtnFind5OleDb;
        private System.Windows.Forms.Button UpdateRangeOleDb;
        private System.Windows.Forms.Button BtnUpdateOleDb;
        private System.Windows.Forms.Button BtnAddRangeOleDb;
        private System.Windows.Forms.Button BtnAddOleDb;
        private System.Windows.Forms.Button BtnAddRangeCRepetidosOleDb;
        private System.Windows.Forms.Button BtnAddRangeCRepetidosSQL;
    }
}


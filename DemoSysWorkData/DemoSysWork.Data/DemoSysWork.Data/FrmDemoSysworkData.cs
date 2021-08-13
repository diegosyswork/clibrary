using Demo.SysWork.Data.Entities;
using Demo.SysWork.Data.Repositories;
using InterfaceB2B.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SysWork.Data.Common.DbConnector;
using SysWork.Data.Common.Mapper;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository.CodeWriter;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.LoggerDb;

namespace Demo.SysWork.Data
{
    public partial class FrmDemoSysworkData : Form
    {
        const string _defaultConnectionStringMSSQL = @"Data Source =NT-SYSWORK\SQLEXPRESS; Initial Catalog=TEST; User ID=TEST; Password=TEST";
        const string _defaultConnectionStringOleDb = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\SWSISTEMAS\C#Library\DemoSysWorkData\DemoSysWork.Data\DemoSysWork.Data\Data\TEST.accdb;Persist Security Info=False";
        const string _defaultConnectionStringMySql = @"Server=localhost;Database=test;Uid=root;Pwd=@#!Sw58125812;persistsecurityinfo=True;";

        PersonRepository _personRepository;
        StateRepository _stateRepository;

        private string _defaultConnectionStringSQLite()
        {
            string SqliteLoggerPath = @"c:\SWSISTEMAS\C#Library\DemoSysWorkData\DemoSysWork.Data\DemoSysWork.Data\Data\TEST.sqlite";
            string SqlConnectionStringBuilder = "Data Source = {0}; Version = 3; New = {1}; Compress = True;PRAGMA jounal_mode=WAL;";
            string SqLiteConnectionString = string.Format(SqlConnectionStringBuilder, SqliteLoggerPath, "False");

            return SqLiteConnectionString;
        }
        public FrmDemoSysworkData()
        {
            InitializeComponent();
        }

        private void FrmDemoSysworkData_Load(object sender, EventArgs e)
        {
            CmbDatabaseEngine.DataSource = Enum.GetValues(typeof(EDatabaseEngine));
            ControlFormState(EState.Unconnected);
            LoadConfig();
        }

        private void FrmDemoSysworkData_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void LoadConfig()
        {
            TxtConnectionString.Text = Properties.Settings.Default.CfgConnectrionString;
            string cfgDatabaseEngine = Properties.Settings.Default.CfgDatabaseEngine;
            if (string.IsNullOrEmpty(cfgDatabaseEngine))
                cfgDatabaseEngine = EDatabaseEngine.MSSqlServer.ToString();


            if (Enum.TryParse<EDatabaseEngine>(cfgDatabaseEngine, out EDatabaseEngine DatabaseEngine))
                CmbDatabaseEngine.SelectedItem = DatabaseEngine;

        }

        private void SaveConfig()
        {
            Properties.Settings.Default.CfgConnectrionString = TxtConnectionString.Text;
            Properties.Settings.Default.CfgDatabaseEngine = ((EDatabaseEngine)CmbDatabaseEngine.SelectedValue).ToString();
            Properties.Settings.Default.Save();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            var DatabaseEngine = (EDatabaseEngine)CmbDatabaseEngine.SelectedValue;

            if (!DbUtil.ConnectionSuccess(DatabaseEngine, TxtConnectionString.Text, out string errMessage))
                MessageBox.Show($"Ha ocurrido el siguiente error {errMessage}", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                ControlFormState(EState.Connected);

            switch (DatabaseEngine)
            {
                case EDatabaseEngine.MSSqlServer:
                    Properties.Settings.Default.MSSQLConnectionString = TxtConnectionString.Text;
                    break;
                case EDatabaseEngine.SqLite:
                    Properties.Settings.Default.SQLiteConnectionString = TxtConnectionString.Text;
                    break;
                case EDatabaseEngine.OleDb:
                    Properties.Settings.Default.OleDbConnectionString = TxtConnectionString.Text;
                    break;
                case EDatabaseEngine.MySql:
                    Properties.Settings.Default.MySQLConnectionString = TxtConnectionString.Text;
                    break;
            }
            Properties.Settings.Default.Save();


            LogText("Verifying Tables...");
            if (!DbUtil.ExistsTable(DatabaseEngine, TxtConnectionString.Text, "States"))
                DbUtil.ExecuteBatchNonQuery(DatabaseEngine, GetScriptStates(DatabaseEngine), TxtConnectionString.Text);

            if (!DbUtil.ExistsTable(DatabaseEngine, TxtConnectionString.Text, "Persons"))
                DbUtil.ExecuteBatchNonQuery(DatabaseEngine, GetScriptPersons(DatabaseEngine), TxtConnectionString.Text);

            LogText("Tables ok");

            DataManager.DatabaseEngine = DatabaseEngine;
            DataManager.ConnectionString = TxtConnectionString.Text;
            LogText("DataManager Setted");

            DbLogger.DatabaseEngine = DataManager.DatabaseEngine;
            DbLogger.ConnectionString = DataManager.ConnectionString;
            DbLogger.AppUserName = "Diego Martinez";
            DbLogger.DbUserName = "TEST_SYSWORK_DATA";
            LogText("DbLogger Setted");

            _personRepository = DataManager.GetInstance().PersonRepository;
            _stateRepository = DataManager.GetInstance().StateRepository;
        }

        private void BtnGetParameters_Click(object sender, EventArgs e)
        {
            var dbConnector = new DataBaseConnector((EDatabaseEngine)CmbDatabaseEngine.SelectedValue);
            dbConnector.ConnectionString = TxtConnectionString.Text;
            dbConnector.PromptUser = true;
            dbConnector.BeforeConnectShowDefaultsParameters = true;
            dbConnector.DefaultDatabase = "TEST_SYSWORK_DATA";

            dbConnector.Connect();
            if (dbConnector.IsConnectionSuccess)
                TxtConnectionString.Text = dbConnector.ConnectionString;
        }

        enum EState
        {
            Connected,
            Unconnected
        }
        void ControlFormState(EState state)
        {
            BtnConnect.Enabled = (state == EState.Unconnected);
            CmbDatabaseEngine.Enabled = BtnConnect.Enabled;
            TxtConnectionString.Enabled = BtnConnect.Enabled;
            BtnGetParameters.Enabled = BtnConnect.Enabled;

            BtnUnconnect.Enabled = !BtnConnect.Enabled;
            GrpDetails.Enabled = !BtnConnect.Enabled;
        }


        private void BtnUnconnect_Click(object sender, EventArgs e)
        {
            ControlFormState(EState.Unconnected);
        }

        private void CmbDatabaseEngine_SelectedValueChanged(object sender, EventArgs e)
        {
            EDatabaseEngine DatabaseEngine = (EDatabaseEngine)CmbDatabaseEngine.SelectedValue;
            string defaultConnectionString = "";
            switch (DatabaseEngine)
            {
                case EDatabaseEngine.MSSqlServer:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.MSSQLConnectionString) ? _defaultConnectionStringMSSQL : Properties.Settings.Default.MSSQLConnectionString;
                    break;
                case EDatabaseEngine.SqLite:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.SQLiteConnectionString) ? _defaultConnectionStringSQLite() : Properties.Settings.Default.SQLiteConnectionString;
                    break;
                case EDatabaseEngine.OleDb:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.OleDbConnectionString) ? _defaultConnectionStringOleDb : Properties.Settings.Default.OleDbConnectionString;
                    break;
                case EDatabaseEngine.MySql:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.MySQLConnectionString) ? _defaultConnectionStringMySql : Properties.Settings.Default.MySQLConnectionString;
                    break;
            }

            TxtConnectionString.Text = defaultConnectionString;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START Add Method DEMO         ///");

            long idPerson = 0;
            string errMessage = "";

            IDbConnection conn;
            IDbTransaction transaction;

            //*************************************************************************
            //  Add, CATCH EXCEPTION
            //*************************************************************************
            LogText(Environment.NewLine + "Add, CATCH EXCEPTION");
            Person person = GetRandomPerson();
            try
            {
                idPerson = _personRepository.Add(person);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, $"Person with id {idPerson} was added");
                LogText($"Person with id {idPerson} was added");
            }
            catch (RepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
                LogText("Catch GenericRepositoryException ");
            }

            //*************************************************************************
            //  Add (DUPLICATE PERSON), CATCH EXCEPTION
            //*************************************************************************
            LogText(Environment.NewLine + "Add (DUPLICATE PERSON), CATCH EXCEPTION");
            try
            {
                LogText("Add duplicate Person id: {idPerson}");
                _personRepository.Add(person);
            }
            catch (RepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
                LogText("Catch GenericRepositoryException ");
            }


            //*************************************************************************
            //  Add, WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //*************************************************************************
            LogText(Environment.NewLine + "Add, WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE");
            person = GetRandomPerson();
            idPerson = _personRepository.Add(person, out errMessage);
            LogText($"Add Person without exception capture, id: {idPerson}");
            if (idPerson == -1)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, errMessage);
            }
            else
            {
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, $"se Insert el id {idPerson}");
            }

            //*************************************************************************
            //  Add (DUPLICATE PERSON), WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //*************************************************************************
            LogText(Environment.NewLine + "Add (DUPLICATE PERSON), WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE");
            idPerson = _personRepository.Add(person, out errMessage);
            LogText("Try Add duplicate Person without exception capture");
            if (idPerson == -1)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, errMessage);
                LogText($"The following error has occurred: {errMessage}");
            }

            //*************************************************************************
            //  Add USING CONNNECTION, CAPTURE EXCEPTION
            //*************************************************************************
            LogText(Environment.NewLine + "Add USING CONNNECTION, CAPTURE EXCEPTION");
            LogText("Create a new Connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();
            person = GetRandomPerson();
            try
            {
                idPerson = _personRepository.Add(person, conn);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, $"Add Person using a connection, person id: {idPerson}");
                LogText($"Add Person using a connection, person id: {idPerson}");
            }
            catch (RepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
                LogText("catch GenericRepositoryException");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            //*************************************************************************
            //  Add (DUPLICATE PERSON) USING CONNNECTION, CAPTURE EXCEPTION
            //*************************************************************************
            LogText(Environment.NewLine + "Add (DUPLICATE PERSON) USING CONNNECTION, CAPTURE EXCEPTION");
            LogText("Create a new connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();
            try
            {
                LogText($"Try add duplicate Person using a connection, person id: {idPerson}");
                _personRepository.Add(person, conn);
            }
            catch (RepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
                LogText("Catch GenericRepositoryException");

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            //*************************************************************************
            //  Add USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //*************************************************************************
            LogText(Environment.NewLine + "Add USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION");
            LogText("Create a new connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();

            LogText("Create transaction");
            transaction = conn.BeginTransaction();

            try
            {
                person = GetRandomPerson();
                idPerson = _personRepository.Add(person, conn, transaction);
                LogText($"Add new person Id: {idPerson}");

                person = GetRandomPerson();
                idPerson = _personRepository.Add(person, conn, transaction);
                LogText($"Add new person Id: {idPerson}");

                transaction.Commit();
                LogText("Commit the transaction");

                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "2 persons was added correctly");
            }
            catch (RepositoryException repoException)
            {
                transaction.Rollback();
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
                LogText("Catch GenericRepositoryException and rollback transaction");

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }


            //********************************************************************************
            //  Add (DUPLICATE PERSON) USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "Add (DUPLICATE PERSON) USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION");
            LogText("Create new Connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();

            LogText("Create transaction");
            transaction = conn.BeginTransaction();
            try
            {
                person = GetRandomPerson();
                _personRepository.Add(person, conn, transaction);
                LogText($"Add new person Id: {idPerson}");

                // Add the same person.
                LogText($"try add duplicate person Id: {idPerson}");
                _personRepository.Add(person, conn, transaction);

                transaction.Commit();
            }
            catch (RepositoryException repoException)
            {

                LogText("catch GenericRepositoryException and Rollback Transaction");
                transaction.Rollback();
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            LogText(Environment.NewLine + "///      END Add Method DEMO         ///");

            // ALLS Method work like this!!!!!!!!

        }

        private void BtnAddRange_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START AddRange Method DEMO         ///");

            string errMessage = "";
            IDbConnection conn;
            IDbTransaction transaction;

            //********************************************************************************
            //  AddRange, CATCH EXCEPTION
            //********************************************************************************
            var list = new List<Person>();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            LogText(Environment.NewLine + "AddRange, CATCH EXCEPTION");
            try
            {
                _personRepository.AddRange(list);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
            }


            //********************************************************************************
            //  AddRange(DUPLICATE LIST), CATCH EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "AddRange(DUPLICATE LIST), CATCH EXCEPTION");
            try
            {
                LogText("Try add dupicate list");
                _personRepository.AddRange(list);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");
            }

            //********************************************************************************
            //  AddRange, WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //********************************************************************************
            LogText(Environment.NewLine + "AddRange, WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE");

            list.Clear();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());
            if (_personRepository.AddRange(list, out errMessage))
            {
                LogText("100 persons was added");
            }
            else
            {
                LogText($"the following error has occurred: {errMessage}");
            }

            //********************************************************************************
            //  AddRange, (DUPLICATE LIST), WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //********************************************************************************
            LogText(Environment.NewLine + "AddRange, (DUPLICATE LIST), WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE");
            LogText("Try add 100 persons (Duplicate list) without capture exception");
            if (_personRepository.AddRange(list, out errMessage))
            {
                LogText("100 persons was added");
            }
            else
            {
                LogText($"the following error has occurred: {errMessage}");
            }

            //********************************************************************************
            //  AddRange, USING CONNNECTION, CAPTURE EXCEPTION
            //********************************************************************************
            list.Clear();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            LogText(Environment.NewLine + "AddRange, USING CONNNECTION, CAPTURE EXCEPTION");
            conn = _personRepository.GetDbConnection();
            conn.Open();
            try
            {
                _personRepository.AddRange(list, conn);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");
            }
            conn.Close();
            conn.Dispose();

            //********************************************************************************
            //  AddRange, (DUPLICATE LIST) USING CONNNECTION, CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "AddRange, (DUPLICATE LIST) USING CONNNECTION, CAPTURE EXCEPTION");
            conn = _personRepository.GetDbConnection();
            conn.Open();
            try
            {
                LogText("Try add duplicate list");
                _personRepository.AddRange(list, conn);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");
            }
            conn.Close();
            conn.Dispose();

            //********************************************************************************
            //  AddRange, USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "AddRange, USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION");

            list.Clear();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            LogText("Create connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();

            LogText("Create Transaction");
            transaction = conn.BeginTransaction();
            try
            {
                _personRepository.AddRange(list, conn, transaction);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");

                transaction.Commit();
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Commit transaction");
                LogText("Commit transaction");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");

                transaction.Rollback();
                LogText("Rollback transaction");
            }

            conn.Close();
            conn.Dispose();

            //********************************************************************************
            //  AddRange, (DUPLICATE LIST) USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "AddRange, (DUPLICATE LIST) USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION");

            LogText("Create Connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();

            LogText("Create Transaction");
            transaction = conn.BeginTransaction();
            try
            {
                LogText("Try add duplicate list with transaction");
                _personRepository.AddRange(list, conn, transaction);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");

                transaction.Commit();
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Commit transaction");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");

                transaction.Rollback();
                LogText("Rollback transaction");
            }

            conn.Close();
            conn.Dispose();

            LogText(Environment.NewLine + "///      END AddRange Method DEMO         ///");
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START Update Method DEMO         ///");

            string errMessage = "";
            IDbConnection conn;
            IDbTransaction transaction;
            Person person = null;

            long idPerson;

            //********************************************************************************
            //  Update, CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "Update, CAPTURE EXCEPTION");

            try
            {
                idPerson = _personRepository.Add(GetRandomPerson());
                person = _personRepository.GetById(idPerson);
                LogText("Create a Person and get Person by ID");

                LogText($"Person FirstName: {person.FirstName}");
                LogText($"Person LastName: {person.LastName}");
                LogText($"Person Passport: {person.Passport}");

                person.FirstName = "UP" + person.FirstName;
                person.LastName = "UP" + person.LastName;
                person.Passport = "UP" + person.Passport;

                _personRepository.Update(person);
                LogText("Change person data and Update ");

                LogText("Get Person Again and show modified data");
                person = _personRepository.GetById(idPerson);

                LogText($"Person FirstName: {person.FirstName}");
                LogText($"Person LastName: {person.LastName}");
                LogText($"Person Passport: {person.Passport}");

                DbLogger.LogInfo(EDbInfoTag.UpdateInfo, $"Person id {person.IdPerson} was updated");
                LogText($"Person id {person.IdPerson} was updated");

            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");
            }

            //********************************************************************************
            //  Update, WITHOUT CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "Update, WITHOUT CAPTURE EXCEPTION");
            idPerson = _personRepository.Add(GetRandomPerson());
            person = _personRepository.GetById(idPerson);
            LogText("Create a Person and get Person by ID");

            LogText($"Person FirstName: {person.FirstName}");
            LogText($"Person LastName: {person.LastName}");
            LogText($"Person Passport: {person.Passport}");

            person.FirstName = "UP" + person.FirstName;
            person.LastName = "UP" + person.LastName;
            person.Passport = "UP" + person.Passport;
            LogText("Change person data and Update ");

            if (_personRepository.Update(person, out errMessage))
            {
                LogText("Get Person Again and show modified data");
                person = _personRepository.GetById(idPerson);

                LogText($"Person FirstName: {person.FirstName}");
                LogText($"Person LastName: {person.LastName}");
                LogText($"Person Passport: {person.Passport}");

                DbLogger.LogInfo(EDbInfoTag.UpdateInfo, $"Person id {person.IdPerson} was updated");
                LogText($"Person id {person.IdPerson} was updated");
            }
            else
            {
                DbLogger.LogError(EDbErrorTag.UpdateError, $"The following error has ocurred: {errMessage}");
                LogText($"Person id {person.IdPerson} was not updated");
            }

            //********************************************************************************
            //  Update, USING A CONNECTION AND TRANSACTION CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "Update, USING A CONNECTION AND TRANSACTION CAPTURE EXCEPTION");
            idPerson = _personRepository.Add(GetRandomPerson());
            person = _personRepository.GetById(idPerson);
            LogText("Create a Person and get Person by ID");

            LogText($"Person FirstName: {person.FirstName}");
            LogText($"Person LastName: {person.LastName}");
            LogText($"Person Passport: {person.Passport}");

            person.FirstName = "UP" + person.FirstName;
            person.LastName = "UP" + person.LastName;
            person.Passport = "UP" + person.Passport;
            LogText("Change person data and Update ");

            LogText("Create a connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();

            LogText("Create transaction");
            transaction = conn.BeginTransaction();

            try
            {
                _personRepository.Update(person, conn, transaction);
                transaction.Commit();
                LogText("Commit transaction");

                LogText("Get Person Again and show modified data");
                person = _personRepository.GetById(idPerson);

                LogText($"Person FirstName: {person.FirstName}");
                LogText($"Person LastName: {person.LastName}");
                LogText($"Person Passport: {person.Passport}");

                DbLogger.LogInfo(EDbInfoTag.UpdateInfo, $"Person id {person.IdPerson} was updated");
                LogText($"Person id {person.IdPerson} was updated");
            }
            catch (RepositoryException gre)
            {
                transaction.Rollback();
                LogText("Rollback transaction");

                DbLogger.LogError(EDbErrorTag.UpdateError, "Error Update", gre);
                LogText($"Capture GenericRepositoryException {gre.Message}");
            }

            LogText(Environment.NewLine + "///      END Update Method DEMO         ///");
        }

        private void BtnUpdateRange_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START UpdateRange Method DEMO         ///");

            IDbConnection conn;
            IDbTransaction transaction;

            //********************************************************************************
            //  UpdateRange, CATCH EXCEPTION
            //********************************************************************************
            var list = new List<Person>();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            LogText(Environment.NewLine + "UpdateRange, CATCH EXCEPTION");
            try
            {
                _personRepository.AddRange(list, out IEnumerable<object> idList);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");

                list = _personRepository.Find(idList).ToList();
                foreach (var item in list)
                {
                    item.FirstName = "UP" + item.FirstName;
                    item.LastName = "UP" + item.LastName;
                    item.Passport = "UP" + item.Passport;
                }
                _personRepository.UpdateRange(list);
                DbLogger.LogInfo(EDbInfoTag.UpdateInfo, "100 persons was Updated");
                LogText("100 was updated");


                LogText("Gets the first 5 persons and show modifiers");
                var first5Persons = _personRepository.Find(idList);
                for (int i = 0; i < 5; i++)
                {
                    LogText($"IdPerson {first5Persons[i].IdPerson} FN: {first5Persons[i].FirstName}, LN: {first5Persons[i].LastName}, PASSPORT: {first5Persons[i].Passport}");
                }
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
            }

            //********************************************************************************
            //  UpdateRange, USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //********************************************************************************
            LogText(Environment.NewLine + "UpdateRange, USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION");

            list.Clear();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            LogText("Create connection");
            conn = _personRepository.GetDbConnection();
            conn.Open();

            LogText("Create Transaction");
            transaction = conn.BeginTransaction();

            try
            {
                _personRepository.AddRange(list, conn, transaction, out IEnumerable<object> listIds);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");

                list = _personRepository.Find(listIds, conn, transaction).ToList();
                foreach (var item in list)
                {
                    item.FirstName = "UPTR" + item.FirstName;
                    item.LastName = "UPTR" + item.LastName;
                    item.Passport = "UPTR" + item.Passport;
                }
                _personRepository.UpdateRange(list, conn, transaction);
                DbLogger.LogInfo(EDbInfoTag.UpdateInfo, "100 persons was Updated");
                LogText("100 was updated");

                transaction.Commit();
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Commit transaction");
                LogText("Commit transaction");

                LogText("Get the first 5 updated persons and show modified data");
                var first5Persons = _personRepository.Find(listIds, conn);
                for (int i = 0; i < 5; i++)
                {
                    LogText($"IdPerson {first5Persons[i].IdPerson} FN: {first5Persons[i].FirstName}, LN: {first5Persons[i].LastName}, PASSPORT: {first5Persons[i].Passport}");
                }


            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
                LogText("Capture GenericRepositoryException ");

                transaction.Rollback();
                LogText("Rollback transaction");
            }

            conn.Close();
            conn.Dispose();

            LogText(Environment.NewLine + "///      END UpdateRange Method DEMO         ///");
        }

        private void BtnFind5_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START Find Method DEMO         ///");

            var list = new List<Person>();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            try
            {
                LogText("Create Connection");
                var conn = _personRepository.GetDbConnection();


                _personRepository.AddRange(list, conn, out IEnumerable<object> listIds);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");

                var first5Persons = _personRepository.Find(listIds, conn);
                for (int i = 0; i < 5; i++)
                {
                    LogText($"IdPerson {first5Persons[i].IdPerson} FN: {first5Persons[i].FirstName}, LN: {first5Persons[i].LastName}, PASSPORT: {first5Persons[i].Passport}");
                }

            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.ReadError, "", gre);
                LogText($"The following error has occurred: {gre.OriginalException.Message}");
            }

            LogText(Environment.NewLine + "///      END Find Method DEMO         ///");
        }
        private void DeleteByID_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START DeleteById Method DEMO         ///");

            long idPerson = long.Parse(TxtId.Text);
            LogText($"Verify the specified Id {idPerson}, exists in DB");

            if (_personRepository.GetById(idPerson) != null)
            {
                if (_personRepository.DeleteById(idPerson, out string errMessage))
                {
                    DbLogger.LogInfo(EDbInfoTag.DeleteInfo, $"The id: {idPerson} was deleted correctly");
                    LogText($"The id: {idPerson} was deleted correctly");
                }
                else
                {
                    DbLogger.LogError(EDbErrorTag.DeleteError, $"The id: {idPerson} was not deleted correctly. The following error has ocurred: {errMessage}");
                    LogText($"The id: {idPerson} was not deleted correctly. The following error has ocurred: {errMessage}");
                }
            }
            else
            {
                LogText($"Id {idPerson}, no exists in db");
            }

            LogText(Environment.NewLine + "///      END DeleteById Method DEMO         ///");
        }
        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START DeleteAll Method DEMO         ///");

            try
            {
                long recordsAffected = _personRepository.DeleteAll();
                LogText($"{recordsAffected} was deleted succefully");

            }
            catch (RepositoryException gre)
            {
                LogText($"The following erros has ocurred: {gre.OriginalException.Message}");
            }

            LogText(Environment.NewLine + "///      END DeleteAll Method DEMO         ///");

        }
        private void BtnDeleteByLambdaExpression_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START DeleteByLambdaExpressionFilter Method DEMO         ///");

            LogText("Create 50 persons, name starts with @TEST1");
            var list = new List<Person>();
            for (int p = 0; p < 50; p++)
                list.Add(GetRandomPerson("@TEST1", ""));

            LogText("Create50 persons, name starts with @TEST2");
            for (int p = 0; p < 50; p++)
                list.Add(GetRandomPerson("@TEST2", ""));

            try
            {

                _personRepository.AddRange(list);
                LogText("Adds 100 person to db, 50 firstName starts with @TEST1 and 50 starts with @TEST2");

                LogText("delete only persons witn name starts with @TEST2");
                long recordsAffected = _personRepository.DeleteByLambdaExpressionFilter(p => p.FirstName.StartsWith("@TEST2"));
                LogText($"{recordsAffected} was deleted succefully");

            }
            catch (RepositoryException gre)
            {
                LogText($"The following erros has ocurred: {gre.OriginalException.Message}");
            }
            LogText(Environment.NewLine + "///      END DeleteByLambdaExpressionFilter Method DEMO         ///");

        }
        private void BtnGetAll_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(_personRepository.GetAll().ToList());
            dataGridView1.Refresh();
        }
        private void BtnGetByLambdaExpression_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START GetListByLambdaExpressionFilter Method DEMO         ///");

            LogText("Create 50 persons, name starts with @P1");
            var list = new List<Person>();
            for (int p = 0; p < 50; p++)
                list.Add(GetRandomPerson("@P1", ""));

            LogText("Create50 persons, name starts with @P2");
            for (int p = 0; p < 50; p++)
                list.Add(GetRandomPerson("@P2", ""));

            try
            {
                _personRepository.AddRange(list);
                LogText("Adds 100 person to db, 50 firstName starts with @P1 and 50 starts with @P2");

                LogText("Get where idState == null");

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(_personRepository.GetListByLambdaExpressionFilter(p => p.FirstName.StartsWith("@P1")).ToList());
                dataGridView1.Refresh();
                LogText($"showing persons startsWith @p1 result {dataGridView1.Rows.Count - 1} persons");
                MessageBox.Show("showing persons startsWith @p1", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(_personRepository.GetListByLambdaExpressionFilter(p => p.IdState != null).ToList());
                MessageBox.Show("showing persons with IdState <> null", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogText($"showing persons with IdState <> null, result {dataGridView1.Rows.Count - 1}  persons");

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(_personRepository.GetListByLambdaExpressionFilter(p => (p.Active == true) && (p.FirstName.Contains("FN")) && (p.IdState == null)).ToList());
                MessageBox.Show("showing persons with IdState == null and active == 1 and First name cotains (FN)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogText($"showing persons with IdState == null and active == 1 and First name cotains (FN), result {dataGridView1.Rows.Count - 1}  persons");

            }
            catch (RepositoryException gre)
            {
                LogText($"The following erros has ocurred: {gre.OriginalException.Message}");
            }

            LogText(Environment.NewLine + "///      END GetListByLambdaExpressionFilter Method DEMO         ///");

        }

        public Person GetRandomPerson()
        {
            return GetRandomPerson("", "");
        }
        public Person GetRandomPerson(string FirstNamePrefix, string LastNamePrefix)
        {
            Person person = new Person();
            person.FirstName = FirstNamePrefix + "FName " + RandomString(10);
            person.LastName = LastNamePrefix + "LName " + RandomString(10);
            person.Passport = "AR" + RandomNumber(10);
            person.LongNameField = "Field With Long Name";
            person.Address = RandomString(20) + " " + RandomNumber(4);
            person.BirthDate = DateTime.Today.AddDays(int.Parse(RandomNumber(4)) * -1);
            person.Active = true;
            //person.GUID = Guid.NewGuid();
            return person;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void LogText(string message)
        {
            TxtLog.AppendText(message + Environment.NewLine);
        }

        private void BtnCLearLog_Click(object sender, EventArgs e)
        {
            TxtLog.Text = "";
        }

        private void BtnExecuteNonQuery_Click(object sender, EventArgs e)
        {
        }

        private void BtnSqlLAMTest_Click(object sender, EventArgs e)
        {

            var result = from person in _personRepository.Table()
                         join state in _stateRepository.Table()
                         on person.IdState equals state.IdState                       
                         select new {person.FirstName, state.Description };

            result.ToList();

            var @for = "";


            /*
            LogText(Environment.NewLine + "///      START SqlLam Method DEMO         ///");

            switch (DataManager.DatabaseEngine)
            {
                case EDatabaseEngine.MSSqlServer:
                    SqlLam<Person>.SetAdapter(SqlAdapter.SqlServer2012);
                    break;
                case EDatabaseEngine.SqLite:
                    SqlLam<Person>.SetAdapter(SqlAdapter.SQLite);
                    break;
                case EDatabaseEngine.OleDb:
                    SqlLam<Person>.SetAdapter(SqlAdapter.SqlServer2008);
                    break;
                case EDatabaseEngine.MySql:
                    SqlLam<Person>.SetAdapter(SqlAdapter.MySql);
                    break;
                default:
                    break;
            }

            DateTime bornDate = new DateTime(2010, 1, 1);

            LogText("SqlLam SelectCount()");
            var querySelectCount = new SqlLam<Person>().SelectCount(p => p.IdPerson).Where(p => (p.IdState == null) && p.BirthDate >= bornDate);

            LogText($"Generated Query {querySelectCount.QueryString}");
            var getCount = _personRepository.GetDbExecutor()
                            .Query(querySelectCount.QueryString)
                            .AddParameters(querySelectCount.QueryParameters)
                            .ExecuteScalar();

            LogText($"Result {getCount}");


            LogText("SqlLam SelectDistict()");
            var distinctSelect = new SqlLam<Person>().SelectDistinct(p => p.BirthDate).Where(p => ((p.IdState == null)) && (p.BirthDate >= bornDate));
            LogText($"Generated Select Distinct Query {distinctSelect.QueryString}");

            DataTable dt = new DataTable();
            dt.Load(_personRepository.GetDbExecutor()
                            .Query(distinctSelect.QueryString)
                            .AddParameters(distinctSelect.QueryParameters)
                            .ExecuteReader());

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            var test = new SqlLam<Person>().Where(p => ((p.FirstName.StartsWith("A")) || (p.FirstName.StartsWith("b")) || (p.FirstName.StartsWith("E"))));

            LogText("Result in datagridView");
            LogText(Environment.NewLine + "///      END SqlLam Method DEMO         ///");
        }

        private void BtnTestEntityClassFromDb_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START EntityClassFromDb Method DEMO         ///" + Environment.NewLine);

            SyntaxProvider syntaxProvider = new SyntaxProvider(DataManager.DatabaseEngine);

            var entityClassFrom = new EntityClassFromTable(DataManager.DatabaseEngine, DataManager.ConnectionString, "Persons", "Persons", "TestNamespace");
            var textClass = entityClassFrom.ToString();
            LogText(textClass);
            Clipboard.SetText(textClass);

            MessageBox.Show("Se copio la clase al portapapeles", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LogText(Environment.NewLine + "///      END EntityClassFromDb Method DEMO         ///" + Environment.NewLine);
        */
        }

        private string GetScriptStates(EDatabaseEngine DatabaseEngine)
        {
            switch (DatabaseEngine)
            {
                case EDatabaseEngine.MSSqlServer:
                    return GetScriptStatesMSSqlServer();
                case EDatabaseEngine.SqLite:
                    return GetScriptStatesSQLite();
                case EDatabaseEngine.OleDb:
                    return GetScriptStatesOleDb();
                case EDatabaseEngine.MySql:
                    return GetScriptStatesMySql();
                default:
                    throw new ArgumentOutOfRangeException("The DatabaseEngine is not supported by this method GetScriptStates()");
            }
        }
        private string GetScriptPersons(EDatabaseEngine DatabaseEngine)
        {
            switch (DatabaseEngine)
            {
                case EDatabaseEngine.MSSqlServer:
                    return GetScriptPersonMSSqlServer();
                case EDatabaseEngine.SqLite:
                    return GetScriptPersonSQLite();
                case EDatabaseEngine.OleDb:
                    return GetScriptPersonOleDb();
                case EDatabaseEngine.MySql:
                    return GetScriptPersonMySql();
                default:
                    throw new ArgumentOutOfRangeException("The DatabaseEngine is not supported by this method GetScriptStates()");
            }
        }
        private string GetScriptStatesOleDb()
        {
            var result = "CREATE TABLE [States] " + Environment.NewLine;
            result += "(" + Environment.NewLine;
            result += " [IdState] AUTOINCREMENT NOT NULL," + Environment.NewLine;
            result += " [StateCode] TEXT NOT NULL," + Environment.NewLine;
            result += " [Description] TEXT NOT NULL," + Environment.NewLine;
            result += " CONSTRAINT[PK_States] PRIMARY KEY" + Environment.NewLine;
            result += "  (" + Environment.NewLine;
            result += "     [IdState]" + Environment.NewLine;
            result += "  )" + Environment.NewLine;
            result += ")" + Environment.NewLine;
            result += "GO" + Environment.NewLine;
            result += "CREATE UNIQUE INDEX IX_STATE_CODE_UNIQUE ON States (StateCode) WITH DISALLOW NULL" + Environment.NewLine;
            result += "GO" + Environment.NewLine;

            return result;
        }

        private string GetScriptPersonOleDb()
        {
            var result = " CREATE TABLE [Persons] (" + Environment.NewLine;
            result += " [IdPerson] AUTOINCREMENT NOT NULL," + Environment.NewLine;
            result += " [FirstName] TEXT NOT NULL," + Environment.NewLine;
            result += " [LastName] TEXT NOT NULL," + Environment.NewLine;
            result += " [Passport] TEXT NOT NULL ," + Environment.NewLine;
            result += " [Address] TEXT," + Environment.NewLine;
            result += " [IdState] INTEGER," + Environment.NewLine;
            result += " [BirthDate] DATETIME," + Environment.NewLine;
            result += " [Active] BIT," + Environment.NewLine;
            result += " [Long Name Field] TEXT, " + Environment.NewLine;
            result += " CONSTRAINT[PK_Persons] PRIMARY KEY" + Environment.NewLine;
            result += "  (" + Environment.NewLine;
            result += "     [IdPerson]" + Environment.NewLine;
            result += "  )" + Environment.NewLine;
            result += " )" + Environment.NewLine;
            result += "GO" + Environment.NewLine;
            result += "CREATE UNIQUE INDEX IX_PERSON_PASSPORT_UNIQUE ON Persons (Passport) WITH DISALLOW NULL" + Environment.NewLine;
            result += "GO" + Environment.NewLine;

            return result;
        }


        private string GetScriptPersonSQLite()
        {
            var result = " CREATE TABLE [Persons] (" + Environment.NewLine;
            result += " [IdPerson] INTEGER PRIMARY KEY AUTOINCREMENT," + Environment.NewLine;
            result += " [FirstName] TEXT NOT NULL," + Environment.NewLine;
            result += " [LastName] TEXT NOT NULL," + Environment.NewLine;
            result += " [Passport] TEXT NOT NULL UNIQUE," + Environment.NewLine;
            result += " [Address] TEXT," + Environment.NewLine;
            result += " [IdState] INTEGER," + Environment.NewLine;
            result += " [BirthDate] TEXT," + Environment.NewLine;
            result += " [Active] INTEGER," + Environment.NewLine;
            result += " [Long Name Field] TEXT " + Environment.NewLine;
            result += " )" + Environment.NewLine;

            return result;
        }
        private string GetScriptStatesSQLite()
        {

            var result = "CREATE TABLE [States](" + Environment.NewLine;
            result += " [IdState] INTEGER PRIMARY KEY AUTOINCREMENT," + Environment.NewLine;
            result += " [StateCode] TEXT NOT NULL UNIQUE," + Environment.NewLine;
            result += " [Description] TEXT NOT NULL" + Environment.NewLine;
            result += " )" + Environment.NewLine;
            return result;
        }

        private string GetScriptPersonMySql()
        {
            var result = "CREATE TABLE `persons` (" + Environment.NewLine;
            result += " `IdPerson` INT NOT NULL AUTO_INCREMENT," + Environment.NewLine;
            result += " `FirstName` NVARCHAR(50) NOT NULL," + Environment.NewLine;
            result += " `LastName` NVARCHAR(50) NOT NULL," + Environment.NewLine;
            result += " `Passport` NVARCHAR(50) NOT NULL," + Environment.NewLine;
            result += " `Address` NVARCHAR(200) NULL," + Environment.NewLine;
            result += " `IdState` INT NULL," + Environment.NewLine;
            result += " `BirthDate` DATETIME NULL," + Environment.NewLine;
            result += " `Active` bit(1) DEFAULT NULL," + Environment.NewLine;
            result += " `Long Name Field` NVARCHAR(50) NULL," + Environment.NewLine;
            result += " PRIMARY KEY(`IdPerson`)," + Environment.NewLine;
            result += " UNIQUE INDEX `Passport_UNIQUE` (`Passport` ASC) VISIBLE);" + Environment.NewLine;

            return result;
        }
        private string GetScriptStatesMySql()
        {

            var result = "CREATE TABLE `states` ( " + Environment.NewLine;
            result += " `IdState` INT NOT NULL AUTO_INCREMENT," + Environment.NewLine;
            result += " `StateCode` NVARCHAR(50) NOT NULL," + Environment.NewLine;
            result += " `Description` NVARCHAR(50) NOT NULL," + Environment.NewLine;
            result += "   PRIMARY KEY(`IdState`)," + Environment.NewLine;
            result += " UNIQUE KEY `StateCode_UNIQUE` (`StateCode` ASC) VISIBLE); " + Environment.NewLine;

            return result;
        }
        private string GetScriptPersonMSSqlServer()
        {
            var result = " CREATE TABLE[Persons]( " + Environment.NewLine;
            result += " [IdPerson][int] IDENTITY(1, 1) NOT NULL, " + Environment.NewLine;
            result += " [FirstName] [nvarchar] (50) NOT NULL, " + Environment.NewLine;
            result += " [LastName] [nvarchar] (50) NOT NULL, " + Environment.NewLine;
            result += " [Passport] [nvarchar] (50) NOT NULL, " + Environment.NewLine;
            result += " [Address] [nvarchar] (200) NULL, " + Environment.NewLine;
            result += " [IdState] [int] NULL, " + Environment.NewLine;
            result += " [BirthDate] [datetime] NULL, " + Environment.NewLine;
            result += " [Active] [bit] NULL, " + Environment.NewLine;
            result += " [Long Name Field] [nvarchar] (50) NULL, " + Environment.NewLine;
            result += " CONSTRAINT[PK_Persons] PRIMARY KEY CLUSTERED " + Environment.NewLine;
            result += " ([IdPerson] ASC) ON[PRIMARY], " + Environment.NewLine;
            result += " CONSTRAINT[IX_Persons] UNIQUE NONCLUSTERED([Passport] ASC) ON[PRIMARY]) ON[PRIMARY] " + Environment.NewLine;
            result += " GO " + Environment.NewLine;
            result += "  " + Environment.NewLine;
            result += " ALTER TABLE[dbo].[Persons] ADD CONSTRAINT[DF_Persons_BitField]  DEFAULT((0)) FOR[Active] " + Environment.NewLine;
            result += " GO " + Environment.NewLine;
            result += "  " + Environment.NewLine;
            result += " ALTER TABLE[dbo].[Persons] WITH CHECK ADD CONSTRAINT[FK_Persons_States] FOREIGN KEY([IdState]) " + Environment.NewLine;
            result += " REFERENCES[dbo].[States] " + Environment.NewLine;
            result += " ([IdState]) " + Environment.NewLine;
            result += " GO " + Environment.NewLine;
            result += "  " + Environment.NewLine;
            result += " ALTER TABLE[dbo].[Persons] " + Environment.NewLine;
            result += "         CHECK CONSTRAINT[FK_Persons_States] " + Environment.NewLine;
            result += " GO " + Environment.NewLine;

            return result;
        }
        private string GetScriptStatesMSSqlServer()
        {
            var result = " CREATE TABLE[States] " + Environment.NewLine;
            result += " ([IdState][int] IDENTITY(1, 1) NOT NULL, " + Environment.NewLine;
            result += " [StateCode] [nvarchar] (6) NOT NULL, " + Environment.NewLine;
            result += " [Description] [nvarchar] (250) NOT NULL, " + Environment.NewLine;
            result += " CONSTRAINT[PK_States_1] PRIMARY KEY CLUSTERED " + Environment.NewLine;
            result += " ([IdState] ASC)ON[PRIMARY], " + Environment.NewLine;
            result += "  CONSTRAINT[IX_States] UNIQUE NONCLUSTERED " + Environment.NewLine;
            result += " ([StateCode] ASC)ON[PRIMARY]) ON[PRIMARY] " + Environment.NewLine;
            result += " GO " + Environment.NewLine;

            return result;
        }

        private void BtnRepositoryClassFromDb_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START RepositoryClassFromDb Method DEMO         ///" + Environment.NewLine);

            SyntaxProvider syntaxProvider = new SyntaxProvider(DataManager.DatabaseEngine);

            var repositoryClassFromDb = new RepositoryClassFromTable(DataManager.DatabaseEngine, DataManager.ConnectionString, "Person", "TestNamespace", "Persons");
            var textClass = repositoryClassFromDb.ToString();
            LogText(textClass);
            Clipboard.SetText(textClass);

            MessageBox.Show("Se copio la clase al portapapeles", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LogText(Environment.NewLine + "///      END RepositoryClassFromDb Method DEMO         ///" + Environment.NewLine);
        }

        private void BtnExistsTable_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START ExistsTable Method DEMO         ///" + Environment.NewLine);

            string tableName = "Persons";
            LogText(string.Format("Table {0} exists ={1} ", tableName, DbUtil.ExistsTable(DataManager.DatabaseEngine, DataManager.ConnectionString, tableName)));

            tableName = "States";
            LogText(string.Format("Table {0} exists ={1} ", tableName, DbUtil.ExistsTable(DataManager.DatabaseEngine, DataManager.ConnectionString, tableName)));

            tableName = "State";
            LogText(string.Format("Table {0} exists ={1} ", tableName, DbUtil.ExistsTable(DataManager.DatabaseEngine, DataManager.ConnectionString, tableName)));

            tableName = "OtherTable";
            LogText(string.Format("Table {0} exists ={1} ", tableName, DbUtil.ExistsTable(DataManager.DatabaseEngine, DataManager.ConnectionString, tableName)));

            LogText(Environment.NewLine + "///      END ExistsTable Method DEMO         ///" + Environment.NewLine);
        }

        private void BtnExistsColumn_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START ExistsColumn Method DEMO         ///" + Environment.NewLine);

            string table = "Persons";
            string column = "IdPerson";
            LogText(string.Format("Table: {0} Column: {1} exists ={2} ", table, column, DbUtil.ExistsColumn(DataManager.DatabaseEngine, DataManager.ConnectionString, table, column)));

            table = "Persons";
            column = "Long Name Field";
            LogText(string.Format("Table: {0} Column: {1} exists ={2} ", table, column, DbUtil.ExistsColumn(DataManager.DatabaseEngine, DataManager.ConnectionString, table, column)));

            table = "Persons";
            column = "Other Field";
            LogText(string.Format("Table: {0} Column: {1} exists ={2} ", table, column, DbUtil.ExistsColumn(DataManager.DatabaseEngine, DataManager.ConnectionString, table, column)));

            LogText(Environment.NewLine + "///      END ExistsColumn Method DEMO         ///" + Environment.NewLine);
        }

        private void BtnGetByID_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START GetById Method DEMO         ///" + Environment.NewLine);

            Person p;
            p = _personRepository.GetById(-1);
            if (p == null)
                LogText("Person with Id -1 NO exists ");


            if (long.TryParse(TxtId.Text, out long idPerson))
            {
                p = _personRepository.GetById(idPerson);
                if (p == null)
                {
                    LogText($"Person with Id {idPerson} NO exists ");
                }
                else
                {
                    LogText($"FirstName: {p.FirstName}, LastName: {p.LastName}, Passport: {p.Passport} ");
                }
            }
            else
            {
                LogText($" {TxtId.Text} is not a valid ID ");
            }

            LogText(Environment.NewLine + "///      END GetById Method DEMO         ///" + Environment.NewLine);
        }

        private void BtnGetByPassport_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START GetByPassport Method DEMO         ///" + Environment.NewLine);

            Person p;
            p = _personRepository.GetByPassport(TxtPassport.Text);
            if (p == null)
            {
                LogText($"Person with Passport {TxtPassport.Text} NO exists ");
            }
            else
            {
                LogText($"FirstName: {p.FirstName}, LastName: {p.LastName}, Passport: {p.Passport} ");
            }


            LogText(Environment.NewLine + "///      END GetByPassport Method DEMO         ///" + Environment.NewLine);
        }

        private void BtnTestDbExecutor_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START DbExecutor Method DEMO         ///" + Environment.NewLine);

            //SELECT
            LogText("Select persons born between 01/01/2000 and 31/12/2010");

            var reader = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
               .Query($"SELECT {_personRepository.ColumnsForSelect} FROM Persons WHERE BirthDate BETWEEN @FromDate AND @ToDate")
               .AddParameter("@FromDate", new DateTime(2000, 01, 01), DbType.DateTime)
               .AddParameter("@ToDate", new DateTime(2010, 12, 31), DbType.DateTime)
               .ExecuteReader();

            var mappedList = new MapDataReaderToEntity().Map<Person>(reader);


            /*SUPER ABBREVIATED
                var mappedList = new MapDataReaderToEntity().Map<Person>(new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                    .Query($"SELECT {_personRepository.ColumnsForSelect} FROM Persons WHERE BirthDate BETWEEN @FromDate AND @ToDate")
                    .AddParameter("@FromDate", new DateTime(2000, 01, 01))
                    .AddParameter("@ToDate", new DateTime(2010, 12, 31))
                .ExecuteReader());
             */

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = mappedList;
            dataGridView1.Refresh();

            /*
            LogText("Select persons born between 01/01/2000 and 31/12/2010 using GetTypedList<Person>()");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                .Query($"SELECT {_personRepository.ColumnsForSelect} FROM Persons WHERE BirthDate BETWEEN @FromDate AND @ToDate")
                .AddParameter("@FromDate", new DateTime(2000, 01, 01))
                .AddParameter("@ToDate", new DateTime(2010, 12, 31))
                .GetTypedList<Person>();
            */
            //dataGridView1.Refresh();

            // INSERT
            LogText("Insert a new Person and Get the ID");
            long idPerson;
            var result = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                .InsertQuery("Persons")
                .AddFieldWithValue("FirstName", "Diego")
                .AddFieldWithValue("LastName", "Martinez")
                .AddFieldWithValue("Passport", "AR" + RandomNumber(8))
                .AddFieldWithValue("BirthDate", new DateTime(1980, 5, 24))
                .AddFieldWithValue("Active", true)
                .AddFieldWithValue("Long Name Field", "Test")
            .ExecuteScalar();

            idPerson = DbUtil.ParseToLong(result);
            LogText($"The idPerson {idPerson} was inserted");

            // UPDATE
            var updateQuery = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                .UpdateQuery("Persons", " IdPerson = @IdPerson")
                .AddFieldWithValue("FirstName", "Updated-Diego")
                .AddFieldWithValue("LastName", "Updated-Martinez")
                .AddParameter("@IdPerson", idPerson)
                .ExecuteNonQuery();
            LogText($"The idPerson {idPerson} was Updated");

            var updateQuery2 = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                .UpdateQuery("Persons")
                .AddFieldWithValue("Long Name Field", "UPDATED!!!!")
                .ExecuteNonQuery();
            LogText("Long Name Field was Updated for All Persons ");

            var deleteQuery = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                .Query("DELETE FROM Persons WHERE BirthDate BETWEEN @FromDate AND @ToDate")
                .AddParameter("@FromDate", new DateTime(2000, 01, 01))
                .AddParameter("@ToDate", new DateTime(2010, 12, 31))
                .ExecuteNonQuery();
            LogText($"Delete persons born between 01/01/2000 and 31/12/2010 recordAffected = {deleteQuery}");


            var storeExecDelete = new DbExecutor(DataManager.ConnectionString, DataManager.DatabaseEngine)
                .Query("DELETE_Person")
                .AddParameter("@IdPerson", 3252)
                .AddParameter("@Name", "Jhon Perez", DbType.String, 200, ParameterDirection.Input)
                .AddOutputParameter("@DateTime", DbType.DateTime)
                .AddOutputParameter("@Exists", DbType.Boolean)
                .AddParameter("@ErrorMsg", "", DbType.String, 200);

            var resultStoreExecDelete = storeExecDelete.ExecuteNonQuery(CommandType.StoredProcedure);
            foreach (var item in storeExecDelete.DbParameters)
            {

            }

            LogText(Environment.NewLine + "///      END DbExecutor Method DEMO         ///" + Environment.NewLine);
        }

        private void BtnTestGenericViewManager_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START ViewManager DEMO         ///" + Environment.NewLine);
            var VManagerPersonsWithStates = DataManager.GetInstance().VManagerPersonsWithStates;

            LogText("Select all from view VW_PersonsWithStates");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = VManagerPersonsWithStates.GetAll().ToList();
            dataGridView1.Refresh();

            LogText("Select from view VW_PersonsWithStates person with passport starts with 'AR2'");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = VManagerPersonsWithStates.GetListByLambdaExpressionFilter(p => p.Passport.StartsWith("AR2")).ToList();
            dataGridView1.Refresh();

            LogText("Get list using manual where clause, using parameters");

            string whereClause = "  (FirstName LIKE '%Name%') AND " +
                " (BirthDate >= @BirthDate) AND " +
                " (IdState IN (1,2,3,4,5) OR (IdState IS NULL)) AND " +
                " Active = 1 ";

            var filter = VManagerPersonsWithStates.GetGenericWhereFilter();
            filter.SetWhere(whereClause)
                .AddParameter("@BirthDate", new DateTime(2014, 1, 1), DbType.DateTime);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = VManagerPersonsWithStates.GetListByGenericWhereFilter(filter).ToList();
            dataGridView1.Refresh();
            LogText(Environment.NewLine + "///      END ViewManager DEMO         ///" + Environment.NewLine);

        }

        private void BtnLogger_Click(object sender, EventArgs e)
        {

        }

        private void BtnTestWhereFilter_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START GenericWhereFilter DEMO         ///" + Environment.NewLine);

            string textFilter = "  (FirstName LIKE '%Name%') AND " +
                " (BirthDate >= @pBirthDate) AND " +
                " (IdState IN (1,2,3,4,5) OR (IdState IS NULL)) AND " +
                " Active = @pActive ";
            LogText("Select persons with FirstName contains 'Name' AND birthDate >= 01/01/2014 AND IdState in List AND Active" + Environment.NewLine);

            var selectFilter = _personRepository.GetGenericWhereFilter();
            selectFilter.SetWhere(textFilter)
                .AddParameter("@pBirthDate", new DateTime(2014, 1, 1), DbType.DateTime)
                .AddParameter("@pActive", 1, DbType.Boolean);
            LogText($"SelectQueryString = {selectFilter.SelectQueryString} " + Environment.NewLine);

            LogText("Update persons, set 'LONG FIELD NAME' = 'UPDATED' with FirstName contains 'Name' AND birthDate >= 01/01/2014 AND IdState in List AND Active" + Environment.NewLine);
            var updatefilter = _personRepository.GetGenericWhereFilter();
            updatefilter.SetWhere(textFilter)
                .AddParameter("@pBirthDate", new DateTime(2014, 1, 1), DbType.DateTime)
                .AddParameter("@pActive", 1, DbType.Boolean)
                .AddFieldWithValue("LONG FIELD NAME", "UPDATED");

            LogText($"UpdateQueryString = {updatefilter.UpdateQueryString} " + Environment.NewLine);

            LogText("Delete persons with IdState = 1 AND Active" + Environment.NewLine);
            string deleteFilter = "WHERE (IdState = @pIdState) AND Active = @pActive";
            var deletefilter = _personRepository.GetGenericWhereFilter();
            deletefilter.SetWhere(deleteFilter)
                .AddParameter("@pIdState", 1)
                .AddParameter("@pActive", 1, DbType.Boolean);

            LogText($"DeleteQueryString = {deletefilter.DeleteQueryString} " + Environment.NewLine);

            LogText(Environment.NewLine + "///      START GenericWhereFilter DEMO         ///" + Environment.NewLine);
        }

        private void BtnTestMapper_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START MapperDemo         ///" + Environment.NewLine);

            var list = new List<Person>();
            for (int p = 0; p < 10000; p++)
                list.Add(GetRandomPerson());

            LogText(Environment.NewLine + "AddRange 10.000");
            try
            {
                _personRepository.AddRange(list);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("10.000 persons was added");
            }
            catch (RepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error AddRange", gre);
            }

            list = _personRepository.GetAll().ToList();
        }

        private async void BtnGetAllAsync_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            var cronoSync = new Stopwatch();
            cronoSync.Start();
            var result2 = _stateRepository.GetAll();
            var result1 = _personRepository.GetAll();
            var result = _personRepository.GetAll();
            dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(result.ToList());
            dataGridView1.Refresh();

            cronoSync.Stop();
            Console.WriteLine(cronoSync.Elapsed);


            var cronoAsync = new Stopwatch();
            cronoAsync.Start();

            var resultAsync2 = _stateRepository.GetAllAsync();
            var resultAsync1 = _personRepository.GetAllAsync();
            var resultAsync = _personRepository.GetAllAsync();

            dataGridView1.DataSource = null;

            await resultAsync2;
            await resultAsync1;
            await resultAsync;

            dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(resultAsync.Result.ToList());
            dataGridView1.Refresh();
            cronoAsync.Stop();

            Console.WriteLine(cronoAsync.Elapsed);

        }

        private async void BtnAddRangeAsync_Click(object sender, EventArgs e)
        {

            var personList = new List<Person>();
            /*for (int i = 0; i < 10000; i++)
                personList.Add(GetRandomPerson());

            var dbConnection = _personRepository.GetDbConnection();

            var chrono = new Stopwatch();
            chrono.Start();

            dbConnection.Open();
            _personRepository.AddRange(personList, dbConnection);
            dbConnection.Close();
            chrono.Stop();

            Console.WriteLine($"Elapsed: {chrono.ElapsedMilliseconds}");
            */
            LogText("Create a list of 10.000 persons");
            personList.Clear();
            for (int i = 0; i < 10000; i++)
                personList.Add(GetRandomPerson());

            LogText("List Created");

            var dbConnectionAsync = _personRepository.GetDbConnection();

            var chronoAsync = new Stopwatch();
            chronoAsync.Start();

            await dbConnectionAsync.OpenAsync();

            LogText("AddRange");
            var task = _personRepository.AddRangeAsync(personList, dbConnectionAsync);

            await task;

            dbConnectionAsync.Close();
            chronoAsync.Stop();
            LogText($"Added 10.000 persons in: { chronoAsync.ElapsedMilliseconds} {Environment.NewLine} milliseconds");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //_personRepository.GetByLambdaExpressionFilter(p => p.FirstName.Substring(1,2) == "Di");
            //_personRepository.GetByLambdaExpressionFilter(p => p.FirstName.Contains( "Diego"));
            //_personRepository.GetByLambdaExpressionFilter(p => p.FirstName == "Diego");
            var xx = _personRepository.GetByLambdaExpressionFilter(p => p.FirstName.Trim() == "Diego");

            var r = new V_MA_ARTICULOSRepository(connectionString: TxtConnectionString.Text, EDatabaseEngine.MSSqlServer);
            var x = r.Table().Where(a => a.ABM== "").FirstOrDefault();
            var x2 = r.GetByLambdaExpressionFilter(a => a.CODIGOARTPROVEEDOR.Trim() == null);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var person = _personRepository.GetById()
        }
    }
}
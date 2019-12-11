using Demo.SysWork.Data.Entities;
using Demo.SysWork.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using SysWork.Data.Common;
using SysWork.Data.Common.DbConnector;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepostory.CodeWriter;
using SysWork.Data.GenericRepostory.Exceptions;
using SysWork.Data.LoggerDb;
using SysWork.Data.Syntax;

namespace Demo.SysWork.Data
{
    public partial class FrmDemoSysworkData : Form
    {
        const string _defaultConnectionStringMSSQL = @"Data Source =NT-SYSWORK\SQLEXPRESS; Initial Catalog=TEST; User ID=TEST; Password=TEST";
        const string _defaultConnectionStringOleDb = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\SWSISTEMAS\C#Library\DemoSysWorkData\DemoSysWork.Data\DemoSysWork.Data\Data\TEST.accdb;Persist Security Info=False";
        const string _defaultConnectionStringMySql = @"Server=localhost;Database=test;Uid=root;Pwd=@#!Sw58125812;persistsecurityinfo=True;";

        PersonRepository _personRepository;
        
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
        private void FrmTestDataV2_Load(object sender, EventArgs e)
        {
            CmbDataBaseEngine.DataSource = Enum.GetValues(typeof(EDataBaseEngine));
            ControlFormState(EState.Unconnected);
            LoadConfig();
        }
        private void FrmTestDataV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void LoadConfig()
        {
            TxtConnectionString.Text = Properties.Settings.Default.CfgConnectrionString;
            string cfgDatabaseEngine = Properties.Settings.Default.CfgDatabaseEngine;
            if (string.IsNullOrEmpty(cfgDatabaseEngine))
                cfgDatabaseEngine = EDataBaseEngine.MSSqlServer.ToString();


            if (Enum.TryParse<EDataBaseEngine>(cfgDatabaseEngine, out EDataBaseEngine dataBaseEngine))
                CmbDataBaseEngine.SelectedItem = dataBaseEngine;

        }

        private void SaveConfig()
        {
            Properties.Settings.Default.CfgConnectrionString = TxtConnectionString.Text ;
            Properties.Settings.Default.CfgDatabaseEngine = ((EDataBaseEngine)CmbDataBaseEngine.SelectedValue).ToString();
            Properties.Settings.Default.Save();
        }


        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (!DbUtil.ConnectionSuccess((EDataBaseEngine)CmbDataBaseEngine.SelectedValue, TxtConnectionString.Text, out string errMessage))
            {
                MessageBox.Show($"Ha ocurrido el siguiente error {errMessage}", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ControlFormState(EState.Connected);
            }

            var databaseEngine = (EDataBaseEngine)CmbDataBaseEngine.SelectedValue;
            switch (databaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    Properties.Settings.Default.MSSQLConnectionString = TxtConnectionString.Text;
                    break;
                case EDataBaseEngine.SqLite:
                    Properties.Settings.Default.SQLiteConnectionString = TxtConnectionString.Text;
                    break;
                case EDataBaseEngine.OleDb:
                    Properties.Settings.Default.OleDbConnectionString = TxtConnectionString.Text;
                    break;
                case EDataBaseEngine.MySql:
                    Properties.Settings.Default.MySQLConnectionString = TxtConnectionString.Text;
                    break;
            }
            Properties.Settings.Default.Save();


            LogText("Verifying Tables...");
            if (!DbUtil.ExistsTable(databaseEngine, TxtConnectionString.Text, "States"))
                DbUtil.ExecuteBatchNonQuery(databaseEngine, GetScriptStates(databaseEngine), TxtConnectionString.Text);

            if (!DbUtil.ExistsTable(databaseEngine, TxtConnectionString.Text, "Persons"))
                DbUtil.ExecuteBatchNonQuery(databaseEngine, GetScriptPersons(databaseEngine), TxtConnectionString.Text);

            LogText("Tables ok");

            RepositoryManager.DataBaseEngine = databaseEngine;
            RepositoryManager.ConnectionString = TxtConnectionString.Text;
            LogText("RepositoryManager Setted");

            DbLogger.DataBaseEngine = RepositoryManager.DataBaseEngine;
            DbLogger.ConnectionString = RepositoryManager.ConnectionString;
            DbLogger.AppUserName = "Diego Martinez";
            DbLogger.DbUserName = "TEST_SYSWORK_DATA";
            LogText("DbLogger Setted");

            _personRepository = RepositoryManager.GetInstance().PersonRepository;
        }

        private void BtnGetParameters_Click(object sender, EventArgs e)
        {
            var dbConnector = new DataBaseConnector((EDataBaseEngine)CmbDataBaseEngine.SelectedValue);
            dbConnector.ConnectionString = TxtConnectionString.Text;
            dbConnector.PromptUser = true;
            dbConnector.BeforeConnectShowDefaultsParameters = true;
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
            CmbDataBaseEngine.Enabled = BtnConnect.Enabled;
            TxtConnectionString.Enabled = BtnConnect.Enabled;
            BtnGetParameters.Enabled = BtnConnect.Enabled;

            BtnUnconnect.Enabled = !BtnConnect.Enabled;
            GrpDetails.Enabled = !BtnConnect.Enabled;
        }


        private void BtnUnconnect_Click(object sender, EventArgs e)
        {
            ControlFormState(EState.Unconnected);
        }

        private void CmbDataBaseEngine_SelectedValueChanged(object sender, EventArgs e)
        {
            EDataBaseEngine databaseEngine = (EDataBaseEngine)CmbDataBaseEngine.SelectedValue;
            string defaultConnectionString = "";
            switch (databaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.MSSQLConnectionString) ? _defaultConnectionStringMSSQL : Properties.Settings.Default.MSSQLConnectionString;
                    break;
                case EDataBaseEngine.SqLite:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.SQLiteConnectionString) ? _defaultConnectionStringSQLite() : Properties.Settings.Default.SQLiteConnectionString;
                    break;
                case EDataBaseEngine.OleDb:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.OleDbConnectionString) ? _defaultConnectionStringOleDb : Properties.Settings.Default.OleDbConnectionString;
                    break;
                case EDataBaseEngine.MySql:
                    defaultConnectionString = string.IsNullOrEmpty(Properties.Settings.Default.MySQLConnectionString) ? _defaultConnectionStringMySql : Properties.Settings.Default.MySQLConnectionString;
                    break;
            }

            TxtConnectionString.Text = defaultConnectionString;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START Add Method DEMO         ///");

            long idPerson = 0;
            string errMessage="";
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
            catch (GenericRepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError,"", repoException);
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
            catch (GenericRepositoryException repoException)
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
            catch (GenericRepositoryException repoException)
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
            catch (GenericRepositoryException repoException)
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
            catch (GenericRepositoryException repoException)
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
            catch (GenericRepositoryException repoException)
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
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
                _personRepository.AddRange(list, conn,transaction);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");

                transaction.Commit();
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Commit transaction");
                LogText("Commit transaction");
            }
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
                _personRepository.Update(person,conn,transaction);
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
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
                var first5Persons = _personRepository.Find(listIds,conn);
                for (int i = 0; i < 5; i++)
                {
                    LogText($"IdPerson {first5Persons[i].IdPerson} FN: {first5Persons[i].FirstName}, LN: {first5Persons[i].LastName}, PASSPORT: {first5Persons[i].Passport}");
                }


            }
            catch (GenericRepositoryException gre)
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


                _personRepository.AddRange(list, conn,  out IEnumerable<object> listIds);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "100 persons was added");
                LogText("100 persons was added");

                var first5Persons = _personRepository.Find(listIds, conn);
                for (int i = 0; i < 5; i++)
                {
                    LogText($"IdPerson {first5Persons[i].IdPerson} FN: {first5Persons[i].FirstName}, LN: {first5Persons[i].LastName}, PASSPORT: {first5Persons[i].Passport}");
                }

            }
            catch (GenericRepositoryException gre)
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
            catch (GenericRepositoryException gre)
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
                list.Add(GetRandomPerson("@TEST1",""));

            LogText("Create50 persons, name starts with @TEST2");
            for (int p = 0; p < 50; p++)
                list.Add(GetRandomPerson("@TEST2",""));

            try
            {

                _personRepository.AddRange(list);
                LogText("Adds 100 person to db, 50 firstName starts with @TEST1 and 50 starts with @TEST2");

                LogText("delete only persons witn name starts with @TEST2");
                long recordsAffected = _personRepository.DeleteByLambdaExpressionFilter(p => p.FirstName.StartsWith("@TEST2"));
                LogText($"{recordsAffected} was deleted succefully");

            }
            catch (GenericRepositoryException gre)
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
                LogText($"showing persons startsWith @p1 result {dataGridView1.Rows.Count-1} persons" );
                MessageBox.Show("showing persons startsWith @p1", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = DbUtil.ConvertToDatatable<Person>(_personRepository.GetListByLambdaExpressionFilter(p => p.IdState != null).ToList());
                MessageBox.Show("showing persons with IdState <> null", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogText($"showing persons with IdState <> null, result {dataGridView1.Rows.Count-1}  persons");

            }
            catch (GenericRepositoryException gre)
            {
                LogText($"The following erros has ocurred: {gre.OriginalException.Message}");
            }

            LogText(Environment.NewLine + "///      END GetListByLambdaExpressionFilter Method DEMO         ///");

        }

        public Person GetRandomPerson()
        {
            return GetRandomPerson("","");
        }
        public Person GetRandomPerson(string FirstNamePrefix,string LastNamePrefix)
        {
            Person person = new Person();
            person.FirstName = FirstNamePrefix + "FName " + RandomString(10);
            person.LastName = LastNamePrefix + "LName " + RandomString(10);
            person.Passport = "AR" + RandomNumber(10);
            person.LongNameField = "Field With Long Name";
            person.Address = RandomString(20) + " " + RandomNumber(4);
            person.BirthDate = DateTime.Today.AddDays(int.Parse(RandomNumber(4)) * -1);
            person.Active = true;

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
            SqlLam<Person>.SetAdapter(SqlAdapter.SqlServer2012);
            var querySelectCount = new SqlLam<Person>().SelectCount(p => p.IdPerson).Where(p => p.IdState != null);

            var cantMySQL = _personRepository.GetDbExecutor()
                            .Query(querySelectCount.QueryString)
                            .AddParameters(querySelectCount.QueryParameters)
                            .ExecuteScalar();
        }

        private void BtnTestEntityClassFromDb_Click(object sender, EventArgs e)
        {
            LogText(Environment.NewLine + "///      START EntityClassFromDb Method DEMO         ///"+ Environment.NewLine  );

            SyntaxProvider syntaxProvider = new SyntaxProvider(RepositoryManager.DataBaseEngine);

            var entityClassFrom = new EntityClassFromDb(RepositoryManager.DataBaseEngine, RepositoryManager.ConnectionString, "Persons", "Persons", "TestNamespace");
            var textClass = entityClassFrom.ToString();
            LogText(textClass);
            Clipboard.SetText(textClass);

            MessageBox.Show("Se copio la clase al portapapeles", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LogText(Environment.NewLine + "///      END EntityClassFromDb Method DEMO         ///" + Environment.NewLine);

        }

        private string GetScriptStates(EDataBaseEngine dataBaseEngine)
        {
            switch (dataBaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    return GetScriptStatesMSSqlServer();
                case EDataBaseEngine.SqLite:
                    return GetScriptStatesSQLite();
                case EDataBaseEngine.OleDb:
                    return GetScriptStatesOleDb();
                case EDataBaseEngine.MySql:
                    return GetScriptStatesMySql();
                default:
                    throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method GetScriptStates()");
            }
        }
        private string GetScriptPersons(EDataBaseEngine dataBaseEngine)
        {
            switch (dataBaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    return GetScriptPersonMSSqlServer();
                case EDataBaseEngine.SqLite:
                    return GetScriptPersonSQLite();
                case EDataBaseEngine.OleDb:
                    return GetScriptPersonOleDb();
                case EDataBaseEngine.MySql:
                    return GetScriptPersonMySql();
                default:
                    throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method GetScriptStates()");
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
            result += " ([IdState][int] NOT NULL, " + Environment.NewLine;
            result += " [StateCode] [nvarchar] (6) NOT NULL, " + Environment.NewLine;
            result += " [Description] [nvarchar] (250) NOT NULL, " + Environment.NewLine;
            result += " CONSTRAINT[PK_States_1] PRIMARY KEY CLUSTERED " + Environment.NewLine;
            result += " ([IdState] ASC)ON[PRIMARY], " + Environment.NewLine;
            result += "  CONSTRAINT[IX_States] UNIQUE NONCLUSTERED " + Environment.NewLine;
            result += " ([StateCode] ASC)ON[PRIMARY]) ON[PRIMARY] " + Environment.NewLine;
            result += " GO " + Environment.NewLine;

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection cn = new OleDbConnection(TxtConnectionString.Text);
            cn.Open();

            //Retrieve schema information
            DataTable columns = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { null, null, null, null, "Persons" });
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = columns;
            dataGridView1.Refresh();

            foreach (DataRow row in columns.Rows)
            {
                Console.WriteLine(row["COLUMN_NAME"].ToString());
                Console.WriteLine(row["TABLE_NAME"].ToString());
                Console.WriteLine(row["UNIQUE"].ToString());
            }

            cn.Close();
        }
    }
}
using Demo.SysWork.Data.Entities;
using Demo.SysWork.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SysWork.Data.Common;
using SysWork.Data.Common.DbConnector;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepostory.Exceptions;
using SysWork.Data.LoggerDb;

namespace Demo.SysWork.Data
{
    public partial class FrmDemoSysworkData : Form
    {
        const string _defaultConnectionStringMSSQL = @"Data Source =NT-SYSWORK\SQLEXPRESS; Initial Catalog=TEST; User ID=TEST; Password=TEST";
        const string _defaultConnectionStringOleDb = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\SWSISTEMAS\C#Library\DemoSysWorkData\DemoSysWork.Data\DemoSysWork.Data\Data\TEST.accdb;Persist Security Info=False";
        const string _defaultConnectionStringMySql = @"Server=localhost;Database=test;Uid=root;Pwd=@#!Sw58125812;persistsecurityinfo=True;";

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

            RepositoryManager.DataBaseEngine = databaseEngine;
            RepositoryManager.ConnectionString = TxtConnectionString.Text;

            DbLogger.DataBaseEngine = RepositoryManager.DataBaseEngine;
            DbLogger.ConnectionString = RepositoryManager.ConnectionString;
            DbLogger.AppUserName = "Diego Martinez";
            DbLogger.DbUserName = "TEST_SYSWORK_DATA";
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

            PersonRepository personRepository = RepositoryManager.GetInstance().PersonRepository;

            long idPerson = 0;
            string errMessage="";
            IDbConnection conn;
            IDbTransaction transaction;

            //
            //  INSERT, CAPTURE ERROR
            //
            Person person = GetRandomPerson();
            try
            {
                idPerson = personRepository.Add(person);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, $"se inserto el id {idPerson}");
            }
            catch (GenericRepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError,"", repoException);
            }

            //
            //  INSERT (DUPLICATE PERSON), CAPTURE ERROR
            //
            try
            {
                personRepository.Add(person);
            }
            catch (GenericRepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
            }


            //
            //  INSERT, WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //
            person = GetRandomPerson();
            idPerson = personRepository.Add(person, out errMessage);
            if (idPerson == -1)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, errMessage);
                MessageBox.Show($"Ha ocurrido el siguiente error: {errMessage}", "Aviso al operador!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, $"se inserto el id {idPerson}");
            }

            //
            //  INSERT (DUPLICATE PERSON), WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //
            idPerson = personRepository.Add(person, out errMessage);
            if (idPerson == -1)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, errMessage);
                MessageBox.Show($"Ha ocurrido el siguiente error: {errMessage}", "Aviso al operador!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //
            //  INSERT USING CONNNECTION, CAPTURE EXCEPTION
            //
            conn = personRepository.GetDbConnection();
            conn.Open();
            person = GetRandomPerson();
            try
            {
                idPerson = personRepository.Add(person, conn);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, $"se inserto el id {idPerson}");
            }
            catch (GenericRepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            //
            //  INSERT (DUPLICATE PERSON) USING CONNNECTION, CAPTURE EXCEPTION
            //
            conn = personRepository.GetDbConnection();
            conn.Open();
            try
            {
                personRepository.Add(person, conn);
            }
            catch (GenericRepositoryException repoException)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }


            //
            //  INSERT USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //
            conn = personRepository.GetDbConnection();
            conn.Open();
            
            transaction = conn.BeginTransaction();
            //transaction = conn.BeginTransaction();
            try
            {

                person = GetRandomPerson();
                idPerson = personRepository.Add(person, conn, transaction);

                person = GetRandomPerson();
                idPerson = personRepository.Add(person, conn, transaction);

                transaction.Commit();

                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Se insertaron 2 personas correctmente");
            }
            catch (GenericRepositoryException repoException)
            {
                transaction.Rollback();
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }


            //
            //  INSERT (DUPLICATE PERSON) USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //
            conn = personRepository.GetDbConnection();
            conn.Open();
            transaction = conn.BeginTransaction();
            try
            {
                person = GetRandomPerson();
                personRepository.Add(person, conn, transaction);

                // insert the same person.
                personRepository.Add(person, conn, transaction);

                transaction.Commit();
            }
            catch (GenericRepositoryException repoException)
            {
                transaction.Rollback();
                DbLogger.LogError(EDbErrorTag.InsertError, "", repoException);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            MessageBox.Show("Fin de las pruebas", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void BtnAddRange_Click(object sender, EventArgs e)
        {
            PersonRepository personRepository = RepositoryManager.GetInstance().PersonRepository;

            long idPerson = 0;
            string errMessage = "";
            IDbConnection conn;
            IDbTransaction transaction;

            //
            //  INSERT, CAPTURE ERROR
            //
            var list = new List<Person>();
            for (int p = 0; p < 100; p++)
                list.Add(GetRandomPerson());

            try
            {
                personRepository.AddRange(list);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Se agregaron 100 personas");
            }
            catch (GenericRepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error insertando 100 registros", gre);
            }

            //
            //  INSERT (DUPLICATE), CAPTURE ERROR
            //
            try
            {
                personRepository.AddRange(list);
                DbLogger.LogInfo(EDbInfoTag.InsertInfo, "Se agregaron 100 personas");
            }
            catch (GenericRepositoryException gre)
            {
                DbLogger.LogError(EDbErrorTag.InsertError, "Error insertando 100 registros", gre);
            }


            //
            //  INSERT RANGE, CAPTURE ERROR
            //

            //
            //  INSERT RANGE, (DUPLICATE PERSON), CAPTURE ERROR
            //

            //
            //  INSERT RANGE, WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //

            //
            //  INSERT RANGE, (DUPLICATE PERSON), WITHOUT EXCEPTION CAPTURE, ONLY SHOW MESSAGE
            //

            //
            //  INSERT RANGE, USING CONNNECTION, CAPTURE EXCEPTION
            //

            //
            //  INSERT RANGE, (DUPLICATE PERSON) USING CONNNECTION, CAPTURE EXCEPTION
            //

            //
            //  INSERT RANGE, USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //

            //
            //  INSERT RANGE, (DUPLICATE PERSON) USING CONNNECTION AND TRANSACTION, CAPTURE EXCEPTION
            //

        }




        public Person GetRandomPerson()
        {
            Person person = new Person();
            person.FirstName = "FName " + RandomString(10);
            person.LastName = "LName " + RandomString(10);
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

    }
}
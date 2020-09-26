using GerdannaDataManager.Daos;
using GerdannaDataManager.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SysWork.Data.Common;
using SysWork.Data.Common.SimpleQuery;
using SysWork.Data.Common.Utilities;
using Microsoft.VisualBasic;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;
using SysWork.Data.Common.DbConnector;
using SysWork.Data.LoggerDb;
using SysWork.Data.Common.ValueObjects;

namespace TestSysWorkData
{
    public partial class Form1 : Form
    {
        const string ConnectionStringSQL = @"Data Source =NT-SYSWORK\SQLEXPRESS; Initial Catalog=TEST; User ID=TEST; Password=TEST";
        const string ConnectionStringOleDb = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST.accdb;Persist Security Info=False";
        const string ConnectionStringMySql = @"Server=localhost;Database=test;Uid=root;Pwd=@#!Sw58125812;persistsecurityinfo=True;";

        private DaoPersona _daoPersonaSQLite;
        private DaoPersona _daoPersonaMSSQL;
        private DaoPersona _daoPersonaMySql;
        private DaoPersona _daoPersonaOleDb;

        public Form1()
        {
            InitializeComponent();
            
            _daoPersonaSQLite = new DaoPersona (GetSqliteConnectionString(), EDataBaseEngine.SqLite);

            _daoPersonaMSSQL = new DaoPersona(ConnectionStringSQL, EDataBaseEngine.MSSqlServer);

            _daoPersonaOleDb = new DaoPersona(ConnectionStringOleDb,EDataBaseEngine.OleDb);

            //_daoPersonaMySql = new DaoPersona(ConnectionStringMySql,EDataBaseEngine.MySql);
            

            /*
            DataManagerSQLite.ConnectionString = GetSqliteConnectionString();
            DataManagerSQLite.DataBaseEngine = EDataBaseEngine.SqLite;
            _daoPersonaSQLite = DataManagerSQLite.GetInstance().DaoPersonaSqlite;


            DataManagerOleDb.ConnectionString = ConnectionStringOleDb;
            DataManagerOleDb.DataBaseEngine = EDataBaseEngine.OleDb;
            _daoPersonaOleDb = DataManagerOleDb.GetInstance().DaoPersonaOleDb;
            
            DataManagerMySQL.ConnectionString = ConnectionStringMySql;
            DataManagerMySQL.DataBaseEngine = EDataBaseEngine.MySql;
            _daoPersonaMySql = DataManagerMySQL.GetInstance().DaoPersonaMySql;

            DataManagerMSSQL.ConnectionString = ConnectionStringSQL;
            DataManagerMSSQL.DataBaseEngine = EDataBaseEngine.MSSqlServer;
            _daoPersonaSQL = DataManagerMSSQL.GetInstance().DaoPersonaMSSql;
            */


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

        private string GetSqliteConnectionString()
        {
            string SqliteLoggerPath = @"C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST.sqlite";
            string SqlConnectionStringBuilder = "Data Source = {0}; Version = 3; New = {1}; Compress = True;";
            string SqLiteConnectionString = string.Format(SqlConnectionStringBuilder, SqliteLoggerPath, "False");

            return SqLiteConnectionString;
        }
        private void BtnAddSQLite_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            MessageBox.Show("Inserto una nueva persona");

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaSQLite.Add(p,out string errMessage);

            p = new Persona();
            p.Apellido = RandomString(500);
            p.Nombre = RandomString(500);
            p.Dni = RandomNumber(500);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(500);
            idGenerado = _daoPersonaSQLite.Add(p);
            MessageBox.Show("Inserto uno que Excede los maximos de los textos (para probar parametros) idGenerado : " + idGenerado);
        }
        private void BtnAddSQL_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaMSSQL.Add(p);
            MessageBox.Show("Inserto uno idGenerado : " + idGenerado);

            p = new Persona();
            p.Apellido = RandomString(500);
            p.Nombre = RandomString(500);
            p.Dni = RandomNumber(500);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(500);
            idGenerado = _daoPersonaMSSQL.Add(p);
            MessageBox.Show("Inserto uno que Excede los maximos de los textos (para probar parametros) idGenerado : " + idGenerado);

            try
            {
                p = new Persona();
                p.Apellido = RandomString(500);
                p.Nombre = RandomString(500);
                p.Dni = RandomNumber(500);
                p.FechaNacimiento = DateTime.Parse("24/05/1980");
                p.Telefono = "11" + RandomNumber(500);
                idGenerado = _daoPersonaMSSQL.Add(p);
                idGenerado = _daoPersonaMSSQL.Add(p);
                MessageBox.Show("Inserto repetido: " + idGenerado);

            }
            catch (Exception)
            {
                throw;
            }




        }
        private void BtnAddOleDb_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaOleDb.Add(p);
            MessageBox.Show("Inserto uno idGenerado : " + idGenerado);

            p = new Persona();
            p.Apellido = RandomString(500);
            p.Nombre = RandomString(500);
            p.Dni = RandomNumber(500);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(500);
            idGenerado = _daoPersonaOleDb.Add(p);
            MessageBox.Show("Inserto uno que Excede los maximos de los textos (para probar parametros) idGenerado : " + idGenerado);
        }

        private void BtnUpdateSQLite_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaSQLite.Add(p);
            MessageBox.Show("Genera una nueva Persona con el id: " + idGenerado);

            p = _daoPersonaSQLite.GetById(idGenerado);
            p.Apellido = p.Apellido + " ** modificado";
            p.Nombre = p.Nombre + " ** modificado";
            p.Dni = p.Dni + " ** modificado ";
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = p.Telefono + "** modificado";

            bool correcto = _daoPersonaSQLite.Update(p);
            MessageBox.Show("La modifica correctamente " + correcto);
        }
        private void BtnUpdatSQL_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaMSSQL.Add(p);
            MessageBox.Show("Genera una nueva Persona con el id: " + idGenerado);

            p = _daoPersonaMSSQL.GetById(idGenerado);
            p.Apellido = p.Apellido + " ** modificado";
            p.Nombre = p.Nombre + " ** modificado";
            p.Dni = p.Dni + " ** modificado ";
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = p.Telefono + "** modificado";

            bool correcto = _daoPersonaMSSQL.Update(p);
            MessageBox.Show("La modifica correctamente " + correcto);
        }
        private void BtnUpdateOleDb_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaOleDb.Add(p);
            MessageBox.Show("Genera una nueva Persona con el id: " + idGenerado);

            p = _daoPersonaOleDb.GetById(idGenerado);
            p.Apellido = " qwerty zzzz** modificado";
            p.Nombre = " **modificado";
            p.Dni = "modificado ";
            p.FechaNacimiento = DateTime.Now;
            p.Telefono = "** modificado";

            bool correcto = _daoPersonaOleDb.Update(p);
            MessageBox.Show("La modifica correctamente " + correcto);
        }

        private void BtnExistsTableSQLite_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsTable(EDataBaseEngine.SqLite, GetSqliteConnectionString(), "Personas"));
            MessageBox.Show("Existe Tabla Personas : " + existe);

            existe = (DbUtil.ExistsTable(EDataBaseEngine.SqLite, GetSqliteConnectionString(), "UnaQueNoExiste"));
            MessageBox.Show("Existe Tabla UnaQueNoExiste: " + existe);

        }
        private void BtnExistsTableSQL_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsTable(ConnectionStringSQL, "Personas"));
            MessageBox.Show("Existe Tabla Personas : " + existe);

            existe = (DbUtil.ExistsTable(ConnectionStringSQL, "UnaQueNoExiste"));
            MessageBox.Show("Existe Tabla UnaQueNoExiste: " + existe);

        }
        private void BtnExistsTableOleDb_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsTable(EDataBaseEngine.OleDb, ConnectionStringOleDb, "Personas"));
            MessageBox.Show("Existe Tabla Personas : " + existe);

            existe = (DbUtil.ExistsTable(EDataBaseEngine.OleDb, ConnectionStringOleDb, "UnaQueNoExiste"));
            MessageBox.Show("Existe Tabla UnaQueNoExiste: " + existe);
        }
        private void BtnExistsColumnSQLite_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsColumn(EDataBaseEngine.SqLite, GetSqliteConnectionString(), "Personas", "Dni"));
            MessageBox.Show("Existe Personas DNI " + existe);

            existe = (DbUtil.ExistsColumn(EDataBaseEngine.SqLite, GetSqliteConnectionString(), "Personas", "mail"));
            MessageBox.Show("Existe Personas mail " + existe);
        }
        private void BtnExistsColumnSQL_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsColumn(EDataBaseEngine.MSSqlServer, ConnectionStringSQL, "Personas", "Dni"));
            MessageBox.Show("Existe Personas DNI " + existe);

            existe = (DbUtil.ExistsColumn(EDataBaseEngine.MSSqlServer, ConnectionStringSQL, "Personas", "mail"));
            MessageBox.Show("Existe Personas mail " + existe);
        }
        private void BtnExistsColumnOleDb_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsColumn(EDataBaseEngine.OleDb, ConnectionStringOleDb, "Personas", "Dni"));
            MessageBox.Show("Existe Personas DNI " + existe);

            existe = (DbUtil.ExistsColumn(EDataBaseEngine.OleDb, ConnectionStringOleDb, "Personas", "mail"));
            MessageBox.Show("Existe Personas mail " + existe);
        }

        private void BtnSimpleQuerySQLite_Click(object sender, EventArgs e)
        {
            var resut = SimpleQuery.Execute(EDataBaseEngine.SqLite, GetSqliteConnectionString(), "SELECT * FROM PERSONAS");
            dataGridView1.DataSource = null;

            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in resut)
            {
                listaPersona.Add(new Persona((long)item.IdPersona, item.Apellido.ToString(), item.Nombre.ToString(), item.Dni.ToString(), DateTime.Parse(item.FechaNacimiento), item.Telefono.ToString()));
            }

            dataGridView1.DataSource = listaPersona;
            dataGridView1.Refresh();
        }
        private void BtnSimpleQuerySQL_Click(object sender, EventArgs e)
        {
            var resut = SimpleQuery.Execute(EDataBaseEngine.MSSqlServer, ConnectionStringSQL, "SELECT * FROM PERSONAS");
            dataGridView1.DataSource = null;

            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in resut)
            {
                listaPersona.Add(new Persona((long)item.IdPersona, item.Apellido.ToString(), item.Nombre.ToString(), item.Dni.ToString(), DateTime.Parse(item.FechaNacimiento.ToString()), item.Telefono.ToString()));
            }

            dataGridView1.DataSource = listaPersona;
            dataGridView1.Refresh();
        }
        private void BtnSimpleQueryOleDb_Click(object sender, EventArgs e)
        {
            var resut = SimpleQuery.Execute(EDataBaseEngine.OleDb, ConnectionStringOleDb, "SELECT * FROM PERSONAS");
            dataGridView1.DataSource = null;

            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in resut)
            {
                listaPersona.Add(new Persona((long)item.IdPersona, item.Apellido.ToString(), item.Nombre.ToString(), item.Dni.ToString(), DateTime.Parse(item.FechaNacimiento.ToString()), item.Telefono.ToString()));
            }

            dataGridView1.DataSource = listaPersona;
            dataGridView1.Refresh();
        }

        private void BtnAddRangeSQLite_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            for (int i = 0; i < 100; i++)
            {
                listaPersona.Add(new Persona(RandomString(50), RandomString(50), RandomNumber(8), (DateTime.Today).AddDays(i), "11" + RandomNumber(8)));
            }
            _daoPersonaSQLite.AddRange(listaPersona);
            MessageBox.Show("Se han insertado " + listaPersona.Count + " Personas");
        }
        private void BtnAddRangeSQL_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            for (int i = 0; i < 100; i++)
            {
                listaPersona.Add(new Persona(RandomString(50), RandomString(50), RandomNumber(8), (DateTime.Today).AddDays(i), "11" + RandomNumber(8)));
            }
            _daoPersonaMSSQL.AddRange(listaPersona);
            MessageBox.Show("Se han insertado " + listaPersona.Count + " Personas");
        }
        private void BtnAddRangeOleDb_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            for (int i = 0; i < 100; i++)
            {
                listaPersona.Add(new Persona(RandomString(50), RandomString(50), RandomNumber(8), (DateTime.Today).AddDays(i), "11" + RandomNumber(8)));
            }
            _daoPersonaOleDb.AddRange(listaPersona);
            MessageBox.Show("Se han insertado " + listaPersona.Count + " Personas");
        }

        private void BtnUpdateRangeSQLite_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in _daoPersonaSQLite.GetAll().ToList())
            {

                listaPersona.Add(new Persona(item.IdPersona, "updateRANGE" + item.Apellido, "updateRANGE" + item.Nombre, "updateRANGE" + item.Dni, item.FechaNacimiento, "updateRANGE" + item.Telefono));
            }

            _daoPersonaSQLite.UpdateRange(listaPersona);
            MessageBox.Show("Se han actualizado " + listaPersona.Count + " Personas");

        }
        private void BtnUpdateRangeSQL_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in _daoPersonaMSSQL.GetAll().ToList())
            {

                listaPersona.Add(new Persona(item.IdPersona, "updateRANGE" + item.Apellido, "updateRANGE" + item.Nombre, item.Dni, item.FechaNacimiento, "updateRANGE" + item.Telefono));
            }

            _daoPersonaMSSQL.UpdateRange(listaPersona);
            MessageBox.Show("Se han actualizado " + listaPersona.Count + " Personas");

        }
        private void BtnUpdateRangeOleDb_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in _daoPersonaOleDb.GetAll().ToList())
            {

                listaPersona.Add(new Persona(item.IdPersona, "updateRANGE" + item.Apellido, "updateRANGE" + item.Nombre, item.Dni, item.FechaNacimiento, "updateRANGE" + item.Telefono));
            }

            _daoPersonaOleDb.UpdateRange(listaPersona);
            MessageBox.Show("Se han actualizado " + listaPersona.Count + " Personas");

        }


        private void BtnDeleteAllSQLite_Click(object sender, EventArgs e)
        {
            long recordsAffecteds = _daoPersonaSQLite.DeleteAll();
            MessageBox.Show($"consulta ejecutada con Exito se eliminaron {recordsAffecteds} registros");
        }
        private void BtnDeleteAllSQL_Click(object sender, EventArgs e)
        {
            long recordsAffecteds = _daoPersonaMSSQL.DeleteAll();
            MessageBox.Show($"consulta ejecutada con Exito se eliminaron {recordsAffecteds} registros");
        }
        private void BtnDeleteAllOleDb_Click(object sender, EventArgs e)
        {
            long recordsAffecteds = _daoPersonaOleDb.DeleteAll();
            MessageBox.Show($"consulta ejecutada con Exito se eliminaron {recordsAffecteds} registros");

        }

        private void BtnFind5SQLite_Click(object sender, EventArgs e)
        {
            List<Persona> lista = _daoPersonaSQLite.GetAll().ToList();
            if (lista.Count < 5)
            {
                MessageBox.Show("Hay menos de 5 items");
                return;
            }
            List<object> ids = new List<object>();
            for (int i = 0; i < 5; i++)
                ids.Add(lista[i].IdPersona);

            dataGridView1.DataSource = null;

            dataGridView1.DataSource = _daoPersonaSQLite.Find(ids);

        }
        private void BtnFind5SQL_Click(object sender, EventArgs e)
        {
            List<Persona> lista = _daoPersonaMSSQL.GetAll().ToList();
            if (lista.Count < 5)
            {
                MessageBox.Show("Hay menos de 5 items");
                return;
            }
            List<object> ids = new List<object>();
            for (int i = 0; i < 5; i++)
                ids.Add(lista[i].IdPersona);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _daoPersonaMSSQL.Find(ids);

        }
        private void BtnFind5OleDb_Click(object sender, EventArgs e)
        {
            List<Persona> lista = _daoPersonaOleDb.GetAll().ToList();
            if (lista.Count < 5)
            {
                MessageBox.Show("Hay menos de 5 items");
                return;
            }
            List<object> ids = new List<object>();
            for (int i = 0; i < 5; i++)
                ids.Add(lista[i].IdPersona);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _daoPersonaOleDb.Find(ids);

        }

        private void BtnDeleteByIDSQLite_Click(object sender, EventArgs e)
        {
            bool correcto = _daoPersonaSQLite.DeleteById(long.Parse(txtDELId.Text));
            MessageBox.Show("Correcto + " + correcto);
        }
        private void BtnDeleteByIDSQL_Click(object sender, EventArgs e)
        {
            bool correcto = _daoPersonaMSSQL.DeleteById(long.Parse(txtDELSQLId.Text));
            MessageBox.Show("Correcto + " + correcto);
        }
        private void BtnDeleteByIDOleDb_Click(object sender, EventArgs e)
        {
            bool correcto = _daoPersonaOleDb.DeleteById(long.Parse(txtDELOLEDBId.Text));
            MessageBox.Show("Correcto + " + correcto);
        }

        private void BtnGetByIDSQLite_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaSQLite.GetById(long.Parse(txtGETId.Text));
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }
        private void BtnGetByIDSQL_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaMSSQL.GetById(long.Parse(txtGETSQLId.Text));
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }
        private void BtnGetByIDOleDb_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaOleDb.GetById(long.Parse(txtGETOLEDBId.Text));
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }

        private void BtnGetByDniSQLite_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaSQLite.GetByDni(txtDNI.Text);
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }
        private void BtnGetByDniSQL_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaMSSQL.GetByDni(txtDNISQL.Text);
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }
        private void BtnGetByDniOleDb_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaOleDb.GetByDni(txtDNIOLEDB.Text);
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }

        private void BtnAddRangeCRepetidosOleDb_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();

            for (int i = 0; i < 5; i++)
            {
                listaPersona.Add(new Persona(RandomString(50), RandomString(50), RandomNumber(8), (DateTime.Today).AddDays(i), "11" + RandomNumber(8)));
            }
            listaPersona.Add(new Persona(RandomString(50), RandomString(50), "00000000", (DateTime.Today), "11" + RandomNumber(8)));
            listaPersona.Add(new Persona(RandomString(50), RandomString(50), "00000000", (DateTime.Today), "11" + RandomNumber(8)));

        }

        private void BtnAddRangeCRepetidosSQL_Click(object sender, EventArgs e)
        {

        }

        private void BtnExecuteNonQuery_Click(object sender, EventArgs e)
        {
            long recordsAffected = _daoPersonaSQLite.GetDBExecute().Query(txtQuery.Text).ExecuteNonQuery(); 
            MessageBox.Show($"records affected = {recordsAffected}");
        }

        private void BtnExecuteNonQuerySQL_Click(object sender, EventArgs e)
        {
            long recordsAffected = _daoPersonaMSSQL.GetDBExecute().Query(txtQuerySQL.Text).ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");
        }

        private void BtnExecuteNonQueryOleDB_Click(object sender, EventArgs e)
        {
            long recordsAffected = _daoPersonaOleDb.GetDBExecute().Query(txtQueryOleDb.Text).ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");

        }

        private void BtnExecuteNonQueryWparam_Click(object sender, EventArgs e)
        {
            DbExecutor dbExecute = _daoPersonaSQLite.GetDBExecute() ;
            dbExecute.Query(txtQuery.Text);

            string paramName;
            string paramValue;
            bool  seguir = true;
            while (seguir)
            {
                paramName = Interaction.InputBox("Ingrese el nombre del parametro", "Parametro");
                paramValue = Interaction.InputBox("Ingrese el valor del parametro", "Parametro");
                seguir = (!string.IsNullOrEmpty(paramName.Trim()) && !string.IsNullOrEmpty(paramValue.Trim()));

                if (seguir)
                {
                    dbExecute.AddParameter(paramName, paramValue);
                }
            }
            long recordsAffected = dbExecute.ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");
        }

        private void btnVerificaSQLServer_Click(object sender, EventArgs e)
        {

            var DataBaseConnector = new DataBaseConnector();
            DataBaseConnector.ConnectionStringName = "testSqlServer";
            DataBaseConnector.DefaultDataSource = @"NT-SYSWORK\SQLEXPRESS";
            DataBaseConnector.DefaultUser = "TEST";
            DataBaseConnector.DefaultPassword = "TEST-MAL";
            DataBaseConnector.DefaultDatabase = "TEST";
            DataBaseConnector.PromptUser = true;
            DataBaseConnector.IsEncryptedData = true;
            DataBaseConnector.Connect();

            if (DataBaseConnector.IsConnectionSuccess)
            {
                MessageBox.Show("Conexion Correcta");
            }
        }

        private void btnVerificaSQLite_Click(object sender, EventArgs e)
        {

            string _SqliteDbPath = @"C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST-mall.sqlite";
            string _defaultSqliteConnectionString = "Data Source = {0}; Version = 3; New = {1}; Compress = True;";
            _defaultSqliteConnectionString = string.Format(_defaultSqliteConnectionString, _SqliteDbPath, "false");

            /*
            if (DbUtil.VerifySQLiteConnectionStringOrGetParams("testSqlite", _defaultSqliteConnectionString))
            {
                MessageBox.Show("Conexion Correcta");
            }
            */
        }

        private void btnVerifyOleDb_Click(object sender, EventArgs e)
        {
            /*
            string _defaultOleDbConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST-mal.accdb;Persist Security Info=False;";
            if (DbUtil.VerifyOleDbConnectionStringOrGetParams("TestOleDb", _defaultOleDbConnectionString))
            {
                MessageBox.Show("Conexion Correcta");
            }
            */

            var DataBaseConnector = new DataBaseConnector(EDataBaseEngine.OleDb );
            DataBaseConnector.ConnectionStringName = "TestOleDbNew";
            DataBaseConnector.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST-mal.accdb;Persist Security Info=False;";
            DataBaseConnector.PromptUser = true;
            DataBaseConnector.IsEncryptedData = true;
            DataBaseConnector.Connect();

            if (DataBaseConnector.IsConnectionSuccess)
            {
                MessageBox.Show("Conexion Correcta");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Persona p = new Persona();
            p.Apellido = "Martinez";
            p.Nombre = "Diego";
            p.Dni = "27926043222";
            p.FechaNacimiento = DateTime.Parse("01/05/1980");
            p.Telefono = "1151825180";

            _daoPersonaMySql.Add(p);
            _daoPersonaMySql.GetAll();

        }

        private void BtnAddMySQL_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaMySql.Add(p);
            MessageBox.Show("Inserto uno idGenerado : " + idGenerado);

            p = new Persona();
            p.Apellido = RandomString(500);
            p.Nombre = RandomString(500);
            p.Dni = RandomNumber(500);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(500);
            idGenerado = _daoPersonaMySql.Add(p);
            MessageBox.Show("Inserto uno que Excede los maximos de los textos (para probar parametros) idGenerado : " + idGenerado);

        }

        private void BtnAddRangeMySQL_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            for (int i = 0; i < 100; i++)
            {
                listaPersona.Add(new Persona(RandomString(50), RandomString(50), RandomNumber(8), (DateTime.Today).AddDays(i), "11" + RandomNumber(8)));
            }
            _daoPersonaMySql.AddRange(listaPersona);
            MessageBox.Show("Se han insertado " + listaPersona.Count + " Personas");

        }

        private void BtnUpdateMySQL_Click(object sender, EventArgs e)
        {
            Persona p;
            long idGenerado;

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaMySql.Add(p);
            MessageBox.Show("Genera una nueva Persona con el id: " + idGenerado);

            p = _daoPersonaMySql.GetById(idGenerado);
            p.Apellido = p.Apellido + " ** modificado";
            p.Nombre = p.Nombre + " ** modificado";
            p.Dni = p.Dni + " ** modificado ";
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = p.Telefono + "** modificado";

            bool correcto = _daoPersonaMySql.Update(p);
            MessageBox.Show("La modifica correctamente " + correcto);
        }

        private void BtnUpdateRangeMySQL_Click(object sender, EventArgs e)
        {
            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in _daoPersonaMySql.GetAll().ToList())
            {

                listaPersona.Add(new Persona(item.IdPersona, "updateRANGE" + item.Apellido, "updateRANGE" + item.Nombre, item.Dni, item.FechaNacimiento, "updateRANGE" + item.Telefono));
            }

            _daoPersonaMySql.UpdateRange(listaPersona);
            MessageBox.Show("Se han actualizado " + listaPersona.Count + " Personas");

        }

        private void BtnFind5MySQL_Click(object sender, EventArgs e)
        {
            List<Persona> lista = _daoPersonaMySql.GetAll().ToList();
            if (lista.Count < 5)
            {
                MessageBox.Show("Hay menos de 5 items");
                return;
            }
            List<object> ids = new List<object>();
            for (int i = 0; i < 5; i++)
                ids.Add(lista[i].IdPersona);

            dataGridView1.DataSource = null;

            dataGridView1.DataSource = _daoPersonaMySql.Find(ids);


        }

        private void DeleteByIDMySQL_Click(object sender, EventArgs e)
        {
            bool correcto = _daoPersonaMySql.DeleteById(long.Parse(txtDELIdMySQL.Text));
            MessageBox.Show("Correcto + " + correcto);
        }

        private void BtnDeleteAllMySQL_Click(object sender, EventArgs e)
        {
            long recordsAffecteds = _daoPersonaMySql.DeleteAll();
            MessageBox.Show($"consulta ejecutada con Exito se eliminaron {recordsAffecteds} registros");

        }

        private void BtnGetByIDMySQL_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaMySql.GetById(long.Parse(txtGETIdMySQL.Text));
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }

        private void BtnGetByDniMySQL_Click(object sender, EventArgs e)
        {
            Persona p = _daoPersonaMySql.GetByDni(txtDNIMySQL.Text);
            MessageBox.Show($" {p.Apellido}\r\n {p.Nombre}\r\n {p.Dni} ");
        }

        private void BtnExistsTableMySQL_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsTable(EDataBaseEngine.MySql, ConnectionStringMySql, "Personas"));
            MessageBox.Show("Existe Tabla Personas : " + existe);

            existe = (DbUtil.ExistsTable(EDataBaseEngine.MySql, ConnectionStringMySql, "UnaQueNoExiste"));
            MessageBox.Show("Existe Tabla UnaQueNoExiste: " + existe);
        }

        private void BtnExistsColumnMySQL_Click(object sender, EventArgs e)
        {
            bool existe = (DbUtil.ExistsColumn(EDataBaseEngine.MySql, ConnectionStringMySql, "Personas", "Dni"));
            MessageBox.Show("Existe Personas DNI " + existe);

            existe = (DbUtil.ExistsColumn(EDataBaseEngine.MySql, ConnectionStringMySql, "Personas", "mail"));
            MessageBox.Show("Existe Personas mail " + existe);
        }

        private void BtnSimpleQueryMySQL_Click(object sender, EventArgs e)
        {
            var resut = SimpleQuery.Execute(EDataBaseEngine.MySql, ConnectionStringMySql, "SELECT * FROM PERSONAS");
            dataGridView1.DataSource = null;

            List<Persona> listaPersona = new List<Persona>();
            foreach (var item in resut)
            {
                listaPersona.Add(new Persona((long)item.IdPersona, item.Apellido.ToString(), item.Nombre.ToString(), item.Dni.ToString(), DateTime.Parse(item.FechaNacimiento.ToString()), item.Telefono.ToString()));
            }

            dataGridView1.DataSource = listaPersona;
            dataGridView1.Refresh();

        }

        private void BtnExecuteNonQueryMySQL_Click(object sender, EventArgs e)
        {
            long recordsAffected = _daoPersonaMySql.GetDBExecute().Query(txtQueryMySQL.Text).ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");

        }

        private void BtnExecuteNonQueryWparamOleDb_Click(object sender, EventArgs e)
        {
            DbExecutor dbExecute = _daoPersonaOleDb.GetDBExecute();
            dbExecute.Query(txtQueryOleDb.Text);

            string paramName;
            string paramValue;
            bool seguir = true;
            while (seguir)
            {
                paramName = Interaction.InputBox("Ingrese el nombre del parametro", "Parametro");
                paramValue = Interaction.InputBox("Ingrese el valor del parametro", "Parametro");
                seguir = (!string.IsNullOrEmpty(paramName.Trim()) && !string.IsNullOrEmpty(paramValue.Trim()));

                if (seguir)
                {
                    dbExecute.AddParameter(paramName, paramValue);
                }
            }
            long recordsAffected = dbExecute.ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");

        }

        private void BtnExecuteNonQueryWparamSQL_Click(object sender, EventArgs e)
        {
            DbExecutor dbExecute = _daoPersonaMSSQL.GetDBExecute();
            dbExecute.Query(txtQuerySQL.Text);

            string paramName;
            string paramValue;
            bool seguir = true;
            while (seguir)
            {
                paramName = Interaction.InputBox("Ingrese el nombre del parametro", "Parametro");
                paramValue = Interaction.InputBox("Ingrese el valor del parametro", "Parametro");
                seguir = (!string.IsNullOrEmpty(paramName.Trim()) && !string.IsNullOrEmpty(paramValue.Trim()));

                if (seguir)
                {
                    dbExecute.AddParameter(paramName, paramValue);
                }
            }

            long recordsAffected = dbExecute.ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");
        }

        private void BtnExecuteNonQueryWparamMySQL_Click(object sender, EventArgs e)
        {
            DbExecutor daoExecuteNonQuery = _daoPersonaMySql.GetDBExecute();
            daoExecuteNonQuery.Query(txtQueryMySQL.Text);

            string paramName;
            string paramValue;
            bool seguir = true;
            while (seguir)
            {
                paramName = Interaction.InputBox("Ingrese el nombre del parametro", "Parametro");
                paramValue = Interaction.InputBox("Ingrese el valor del parametro", "Parametro");
                seguir = (!string.IsNullOrEmpty(paramName.Trim()) && !string.IsNullOrEmpty(paramValue.Trim()));

                if (seguir)
                {
                    daoExecuteNonQuery.AddParameter(paramName, paramValue);
                }
            }
            long recordsAffected = daoExecuteNonQuery.ExecuteNonQuery();
            MessageBox.Show($"records affected = {recordsAffected}");

        }

        private void btnVerifyMySQL_Click(object sender, EventArgs e)
        {
            var DataBaseConnector = new DataBaseConnector(EDataBaseEngine.MySql);
            DataBaseConnector.ConnectionStringName = "testMySql";
            DataBaseConnector.DefaultDataSource = @"LOCALHOST";
            DataBaseConnector.DefaultUser = "TEST";
            DataBaseConnector.DefaultPassword = "TEST-MAL";
            DataBaseConnector.DefaultDatabase = "TEST";
            DataBaseConnector.PromptUser = true;
            DataBaseConnector.IsEncryptedData = true;
            DataBaseConnector.Connect();

            if (DataBaseConnector.IsConnectionSuccess)
            {
                MessageBox.Show("Conexion Correcta");
            }
        }

        private void BtnLoggerMySQL_Click(object sender, EventArgs e)
        {
            DbLogger.ConnectionString = ConnectionStringMySql;
            DbLogger.DataBaseEngine = EDataBaseEngine.MySql;
            DbLogger.LogInfo(EDbInfoTag.Info,"Test MySQL");
        }

        private void BtnLoggerOleDb_Click(object sender, EventArgs e)
        {
            //LoggerDb.ConnectionString = ConnectionStringOleDb;
            //LoggerDb.DataBaseEngine = EDataBaseEngine.OleDb;
            //LoggerDb.Log("Test OleDb");

            DbLogger.ConnectionString = ConnectionStringOleDb;
            DbLogger.DataBaseEngine = EDataBaseEngine.OleDb;
            DbLogger.LogInfo(EDbInfoTag.Info,"Test OleDb");



        }

        private void BtnLoggerSQL_Click(object sender, EventArgs e)
        {
            //LoggerDb.ConnectionString = ConnectionStringSQL;
            //LoggerDb.DataBaseEngine = EDataBaseEngine.MSSqlServer;
            //LoggerDb.Log("Test Sql");

            DbLogger.ConnectionString = ConnectionStringSQL;
            DbLogger.DataBaseEngine = EDataBaseEngine.MSSqlServer;
            DbLogger.LogInfo(EDbInfoTag.Info, "resumen");


        }

        private void BtnLogger_Click(object sender, EventArgs e)
        {
            DbLogger.ConnectionString = GetSqliteConnectionString();
            DbLogger.DataBaseEngine = EDataBaseEngine.SqLite;
            DbLogger.LogInfo(EDbInfoTag.Info,"Test Sqlite");

            DbLogger.ConnectionString = GetSqliteConnectionString();
            DbLogger.DataBaseEngine = EDataBaseEngine.SqLite;
            DbLogger.LogInfo(EDbInfoTag.Info, "resumen");


        }

        private void btnSQLLAMTest_Click(object sender, EventArgs e)
        {
            SqlLam<Persona>.SetAdapter(SqlAdapter.MySql);
            var querySelectCountMySQL = new SqlLam<Persona>().SelectCount(p => p.IdPersona);//.Where(p => p.Dni == "27926043");
            // Obtiene la cantidad
            var cantMySQL = _daoPersonaMySql.GetDBExecute()
                            .Query(querySelectCountMySQL.QueryString)
                            .AddParameters(querySelectCountMySQL.QueryParameters)
                            .ExecuteScalar();

            SqlLam<Persona>.SetAdapter(SqlAdapter.SqlServer2012);
            var querySelectCountMSSql = new SqlLam<Persona>().SelectCount(p => p.IdPersona);//.Where(p => p.Dni == "27926043");
            // Obtiene la cantidad
            var cantMSSql = _daoPersonaMSSQL.GetDBExecute()
                            .Query(querySelectCountMSSql.QueryString)
                            .AddParameters(querySelectCountMSSql.QueryParameters)
                            .ExecuteScalar();

            SqlLam<Persona>.SetAdapter(SqlAdapter.SQLite);
            var querySelectCountSQLite = new SqlLam<Persona>().SelectCount(p => p.IdPersona);//.Where(p => p.Dni == "27926043");
            // Obtiene la cantidad
            var cantSQLite = _daoPersonaSQLite.GetDBExecute()
                            .Query(querySelectCountSQLite.QueryString)
                            .AddParameters(querySelectCountSQLite.QueryParameters)
                            .ExecuteScalar();

        }

        private void btnTestMSSqlServerConnector_Click(object sender, EventArgs e)
        {
            var MsSqlConn = new DbConnectorMSSqlServer();
            MsSqlConn.TryGetConnectionStringFromConfig = true;
            MsSqlConn.WriteInConfigFile = true;
            MsSqlConn.ConnectionStringName = "SyworkTestDbModel";

            MsSqlConn.DefaultDatabase = "SysworkDbModel";
            MsSqlConn.DefaultDataSource = "NT-SYSWORK\\SQLEXPRESS";
            MsSqlConn.DefaultUser = "SA";
            MsSqlConn.IsEncryptedData = true;
            MsSqlConn.DefaultPassword = "Dm58125812";

            MsSqlConn.PromptUser = true;

            MsSqlConn.Connect();


            
            var MsSqlConn2 = new DbConnectorMSSqlServer();
            MsSqlConn2.TryGetConnectionStringFromConfig = true;
            MsSqlConn2.WriteInConfigFile = true;
            MsSqlConn2.ConnectionStringName = "SyworkTestDbModelMA2L";

            MsSqlConn2.DefaultDatabase = "SysworkDbModel";
            MsSqlConn2.DefaultDataSource = "MAL";
            MsSqlConn2.DefaultUser = "MAL";
            MsSqlConn2.DefaultPassword = "MAL";

            MsSqlConn2.PromptUser = true;

            MsSqlConn2.Connect();


            var MsSqlConn3 = new DbConnectorMSSqlServer();
            MsSqlConn3.DefaultDatabase = "SysworkDbModel";

            MsSqlConn3.PromptUser = false;

            MsSqlConn3.Connect();
            if (!MsSqlConn3.IsConnectionSuccess)
            {
                MessageBox.Show($"{MsSqlConn3.ConnectionError}");
            }


            var MsSqlConn4 = new DbConnectorMSSqlServer();

            MsSqlConn4.DefaultDatabase = "SysworkDbModel";
            MsSqlConn4.DefaultDataSource = "NT-SYSWORK\\SQLEXPRESS";
            MsSqlConn4.DefaultUser = "SA";
            MsSqlConn4.IsEncryptedData = true;
            MsSqlConn4.DefaultPassword = "Dm58125812";
            MsSqlConn4.PromptUser = true;

            MsSqlConn4.Connect();



        }

        private void btnOleDbConnector_Click(object sender, EventArgs e)
        {
            var oleDbConn = new DbConnectorOleDb();
            //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST-mal.accdb;Persist Security Info=False;"
            oleDbConn.WriteInConfigFile = true;
            oleDbConn.TryGetConnectionStringFromConfig = true;
            oleDbConn.ConnectionStringName = "TestOldDbOk";
            oleDbConn.IsEncryptedData = true;
            oleDbConn.PromptUser = true;

            oleDbConn.Connect(); 

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dbCnonector = new DataBaseConnector();
            dbCnonector.DefaultDatabase = "SYSWORK2005";
            dbCnonector.DefaultUser = "SYSWORK2005";
            dbCnonector.DefaultPassword = "SYSWORK2005";
            dbCnonector.DefaultDataSource= @".\SQLEXPRESS";
            dbCnonector.Connect();
            if (dbCnonector.IsConnectionSuccess)
            {
                var connectionString = dbCnonector.ConnectionString;

            }

        }
    }
}

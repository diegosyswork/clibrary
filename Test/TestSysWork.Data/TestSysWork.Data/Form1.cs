using GerdannaDataManager.Daos;
using GerdannaDataManager.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using SysWork.Data.Common;
using SysWork.Data.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.SimpleQuery;
using SysWork.Data.Common.Utilities;

namespace TestDaoModelDataCommon
{
    public partial class Form1 : Form
    {
        const string ConnectionStringSQL = @"Data Source =NT-SYSWORK\SQLEXPRESS; Initial Catalog=TEST; User ID=TEST; Password=TEST";
        const string ConnectionStringOleDb = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SWSISTEMAS\C#Library\Test\TestSysWork.Data\TestSysWork.Data\Data\TEST.accdb;Persist Security Info=False";

        private DaoPersonaSqlite _daoPersonaSQLite;
        private DaoPersonaSql _daoPersonaSQL;

        private DaoPersonaOleDb _daoPersonaOleDb;

        public Form1()
        {
            InitializeComponent();
            _daoPersonaSQLite = new DaoPersonaSqlite(GetSqliteConnectionString());
            _daoPersonaSQL = new DaoPersonaSql(ConnectionStringSQL);
            _daoPersonaOleDb = new DaoPersonaOleDb(ConnectionStringOleDb);
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

            p = new Persona();
            p.Apellido = RandomString(20);
            p.Nombre = RandomString(20);
            p.Dni = RandomNumber(8);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(8);
            idGenerado = _daoPersonaSQLite.Add(p);
            MessageBox.Show("Inserto uno idGenerado : " + idGenerado);

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
            idGenerado = _daoPersonaSQL.Add(p);
            MessageBox.Show("Inserto uno idGenerado : " + idGenerado);

            p = new Persona();
            p.Apellido = RandomString(500);
            p.Nombre = RandomString(500);
            p.Dni = RandomNumber(500);
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = "11" + RandomNumber(500);
            idGenerado = _daoPersonaSQL.Add(p);
            MessageBox.Show("Inserto uno que Excede los maximos de los textos (para probar parametros) idGenerado : " + idGenerado);
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
            idGenerado = _daoPersonaSQL.Add(p);
            MessageBox.Show("Genera una nueva Persona con el id: " + idGenerado);

            p = _daoPersonaSQL.GetById(idGenerado);
            p.Apellido = p.Apellido + " ** modificado";
            p.Nombre = p.Nombre + " ** modificado";
            p.Dni = p.Dni + " ** modificado ";
            p.FechaNacimiento = DateTime.Parse("24/05/1980");
            p.Telefono = p.Telefono + "** modificado";

            bool correcto = _daoPersonaSQL.Update(p);
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
            _daoPersonaSQL.AddRange(listaPersona);
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
            foreach (var item in _daoPersonaSQL.GetAll().ToList())
            {

                listaPersona.Add(new Persona(item.IdPersona, "updateRANGE" + item.Apellido, "updateRANGE" + item.Nombre, item.Dni, item.FechaNacimiento, "updateRANGE" + item.Telefono));
            }

            _daoPersonaSQL.UpdateRange(listaPersona);
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
            DbConnection conn = _daoPersonaSQLite.GetConnection();
            conn.Open();

            DbCommand dBcommand = conn.CreateCommand();
            dBcommand.CommandText = "DELETE FROM personas";
            dBcommand.ExecuteNonQuery();

            MessageBox.Show("consulta ejecutada con Exito");

        }
        private void BtnDeleteAllSQL_Click(object sender, EventArgs e)
        {
            DbConnection conn = _daoPersonaSQL.GetConnection();
            conn.Open();

            DbCommand dBcommand = conn.CreateCommand();
            dBcommand.CommandText = "DELETE FROM personas";
            dBcommand.ExecuteNonQuery();

            MessageBox.Show("consulta ejecutada con Exito");

        }
        private void BtnDeleteAllOleDb_Click(object sender, EventArgs e)
        {
            DbConnection conn = _daoPersonaOleDb.GetConnection();
            conn.Open();

            DbCommand dBcommand = conn.CreateCommand();
            dBcommand.CommandText = "DELETE FROM personas";
            dBcommand.ExecuteNonQuery();

            MessageBox.Show("consulta ejecutada con Exito");

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
            List<Persona> lista = _daoPersonaSQL.GetAll().ToList();
            if (lista.Count < 5)
            {
                MessageBox.Show("Hay menos de 5 items");
                return;
            }
            List<object> ids = new List<object>();
            for (int i = 0; i < 5; i++)
                ids.Add(lista[i].IdPersona);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _daoPersonaSQL.Find(ids);

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
            bool correcto = _daoPersonaSQL.DeleteById(long.Parse(txtDELSQLId.Text));
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
            Persona p = _daoPersonaSQL.GetById(long.Parse(txtGETSQLId.Text));
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
            Persona p = _daoPersonaSQL.GetByDni(txtDNISQL.Text);
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


            DbConnection connPersistent = _daoPersonaOleDb.Get_PersistentConnection();
            connPersistent.Open();

            DbTransaction tt = connPersistent.BeginTransaction();
            try
            {
                _daoPersonaOleDb.AddRange(listaPersona,connPersistent,tt);
                tt.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("sin transaction ocurrio el siguiente error" + ex.Message);
                tt.Rollback();
            }


            IDbConnection oleDbConn = _daoPersonaOleDb.Get_IDBConnection();
            oleDbConn.Open();

            IDbTransaction transaction = oleDbConn.BeginTransaction();
            try
            {
                
                _daoPersonaOleDb.AddRange(listaPersona, oleDbConn, transaction);
                transaction.Commit();
            }
            catch(Exception exx)
            {
                transaction.Rollback();
            }
        }

        private void BtnAddRangeCRepetidosSQL_Click(object sender, EventArgs e)
        {

        }
    }
}

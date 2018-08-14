using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysWork.Data.DaoModel;
using SysWork.Data.ObjectResolver;
using SysWork.Forms.FormsABM;
using SysWork.Util.Logger;
using TestFormsABM.Data;

namespace TestFormsABM
{
    public partial class Form1 : FormABM<Cliente>
    {

        public bool _noValidarFormulario;
        private DaoCliente _daoCliente;
        private const string connectionString = "Data Source=54.94.227.132;Initial Catalog=ManosALaObra;User ID=ManosALaObra;Password=ManosALaObra$%&;";

        public Form1()
        {
            InitializeComponent();
            ABMErrorProvider = errorProvider1;
            ABMToolBarABM = toolBarABM1;
            UniqueKeyControls = "codClienteTextBox";

            SeteaLogger();

            InitABMControls();

            _daoCliente  = new DaoCliente(connectionString);
            DaoEntity = _daoCliente;

            codClienteTextBox.Validating += ValidaControl;
            razonSocialTextBox.Validating += ValidaControl;
        }

        private void ValidaControl(object sender, CancelEventArgs e)
        {
            if (NoValidarFormulario)
                e.Cancel = false;
            else
            {
                var control = (Control)sender;
                switch (control.Name)
                {
                    case "codClienteTextBox":
                        e.Cancel = !ValidaCodClienteTextBox();
                        break;

                    case "razonSocialTextBox":
                        e.Cancel = !ValidaRazonSocialTextBox();
                        break;
                }
            }
        }

        private bool ValidaCodClienteTextBox()
        {
            string mensajeError = "";
            ABMErrorProvider.SetError(codClienteTextBox, mensajeError);

            if (string.IsNullOrEmpty(codClienteTextBox.Text))
            {
                mensajeError = "Debe informar el codigo del Cliente";
                ABMErrorProvider.SetError(codClienteTextBox, mensajeError);
                return false;
            }

            if (!ValidacionFinal)
            {
                try
                {
                    Entity = _daoCliente.GetByCodCliente(codClienteTextBox.Text.Trim());

                    if (Entity!= null)
                        AsignarDatos();

                    ModoEdicion(true);
                }
                catch (DaoModelException daoModelException)
                {
                    //LoggerDb.Log(ELoggerDbTagError.ErrorDeLectura.ToString(), daoModelException.DbCommand, daoModelException.OriginalException);
                    MessageBox.Show("Ha ocurrido el siguiente error al intentar acceder al registro: \r\n" + daoModelException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private bool ValidaRazonSocialTextBox()
        {
            return true;
        }

        public override void EntityValuesToControls()
        {
            codClienteTextBox.Text = Entity.codCliente;
            razonSocialTextBox.Text = Entity.razonSocial;
            activoCheckBox.Checked = Entity.activo;
        }

        public override void ControlsValuesToEntity()
        {
            Entity.codCliente = codClienteTextBox.Text;
            Entity.razonSocial = razonSocialTextBox.Text;
            Entity.activo = activoCheckBox.Checked;
        }

        public override bool DatosValidos()
        {
            ValidacionFinal = true;

            bool valido = ValidaCodClienteTextBox();
            valido = valido && ValidaRazonSocialTextBox();

            ValidacionFinal = false;
            return valido;
        }

        void SeteaLogger()
        {
            string SqliteLoggerPath = Path.Combine(Application.StartupPath, ".LoggerSQLite.sqlite");
            string SqlConnectionStringBuilder = "Data Source = {0}; Version = 3; New = {1}; Compress = True;";
            string SQLiteConnectionString = string.Format(SqlConnectionStringBuilder, SqliteLoggerPath, "False");

            if (!File.Exists(SqliteLoggerPath))
            {
                try
                {
                    var dbConnection = DataObjectResolver.GetDbConnection(EResolverDataBaseEngine.SqLite, string.Format(SqlConnectionStringBuilder, SqliteLoggerPath, "True"));

                    dbConnection.Open();
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e);
                }
            }

            LoggerDb.ResolverDbType = EResolverDataBaseEngine.SqLite;
            LoggerDb.ConnectionString = SQLiteConnectionString;

            LoggerDb.Log("Loguea en la base SQLite");
            LoggerDb.Log(DateTime.Today.ToString());
        }
    }
}
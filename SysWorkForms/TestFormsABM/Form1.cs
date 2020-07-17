using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.ObjectResolver;
using SysWork.Util.Logger;
using TestFormsABM.Data;

namespace TestFormsABM
{
    public partial class Form1 : FormABM<Cliente>
    {

        public bool _noValidarFormulario;
        private ClienteRepository _daoCliente;
        private const string connectionString = "Data Source=54.94.227.132;Initial Catalog=ManosALaObra;User ID=ManosALaObra;Password=ManosALaObra$%&;";

        public Form1()
        {
            InitializeComponent();

            ABMErrorProvider = errorProvider1;
            ABMToolBarABM = toolBarABM1;
            UniqueKeyControls = "codClienteTextBox";
            
            SeteaLogger();

            _daoCliente = new ClienteRepository(connectionString);
            Repository = _daoCliente;

            InitFormControls();

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

            if (!IsFinalValidation)
            {
                try
                {
                    Entity = _daoCliente.GetByCodCliente(codClienteTextBox.Text.Trim());

                    if (Entity!= null)
                        AssignData();

                    SetEditMode(true);
                }
                catch (RepositoryException repositoryException)
                {
                    //LoggerDb.Log(ELoggerDbTagError.ErrorDeLectura.ToString(), daoModelException.DbCommand, daoModelException.OriginalException);
                    MessageBox.Show("Ha ocurrido el siguiente error al intentar acceder al registro: \r\n" + repositoryException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private bool ValidaRazonSocialTextBox()
        {
            return true;
        }

        protected override void EntityValuesToControls()
        {
            codClienteTextBox.Text = Entity.codCliente;
            razonSocialTextBox.Text = Entity.razonSocial;
            activoCheckBox.Checked = Entity.activo;
        }

        protected override void ControlsValuesToEntity()
        {
            Entity.codCliente = codClienteTextBox.Text;
            Entity.razonSocial = razonSocialTextBox.Text;
            Entity.activo = activoCheckBox.Checked;
            base.ControlsValuesToEntity();
        }

        protected override bool IsValidData()
        {
            IsFinalValidation = true;

            bool valido = ValidaCodClienteTextBox();
            valido = valido && ValidaRazonSocialTextBox();

            IsFinalValidation = false;
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
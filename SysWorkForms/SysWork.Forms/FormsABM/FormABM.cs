using System;
using System.ComponentModel;
using System.Windows.Forms;
using SysWork.Controls.Toolbars;
using SysWork.Data.DaoModel;
using SysWork.Data.DaoModel.Exceptions;
using SysWork.Data.DaoModel.Interfaces;
using SysWork.Data.Logger;
using SysWork.Forms.FormsABM.FormsUtil;
using static SysWork.Data.Logger.LoggerDb;

namespace SysWork.Forms.FormsABM
{
    [TypeDescriptionProvider(typeof(AbstractCommunicatorProvider))]
    public abstract partial class FormABM<T>:Form where T:class,new()
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        private bool _validacionFinal;
        public bool ValidacionFinal
        {
            get { return _validacionFinal; }
            set { _validacionFinal = value; }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        private bool _noLanzarEventosCombos;
        public bool NoLanzarEventosCombos
        {
            get { return _noLanzarEventosCombos; }
            set { _noLanzarEventosCombos = value; }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        private T _entity = default(T);
        public T Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        private IBaseDao<T> _daoEntity = null;
        public IBaseDao<T> DaoEntity
        {
            get { return _daoEntity; }
            set { _daoEntity = value; }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        public ToolBarABM _toolBarABM = null;
        public ToolBarABM ABMToolBarABM
        {
            get { return _toolBarABM; }
            set { _toolBarABM = value; }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public ErrorProvider _errorProvider = null;
        public ErrorProvider ABMErrorProvider
        {
            get { return _errorProvider; }
            set { _errorProvider = value; }
        }

        private bool _noValidarFormulario;

        public bool NoValidarFormulario
        {
            get { return _noValidarFormulario; }
            set { _noValidarFormulario = value; }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        public string UniqueKeyControls { get; set; }

        private bool _ABMloggerHabilitado = false;
        public bool ABMLoggerHabilitado
        {
            get { return _ABMloggerHabilitado; }
            set { _ABMloggerHabilitado = value; }
        }
        public LoggerDb ABMLoggerDb { get; private set; }

        public void InitFormABM()
        {
            if (_errorProvider == null)
                throw new NullReferenceException("No se ha referenciado un control del tipo ErrorProvider");

            if (_toolBarABM == null)
                throw new NullReferenceException("No se ha referenciado un control del tipo ToolBarABM");

            if (_daoEntity== null)
                throw new NullReferenceException("No se ha referenciado un IBaseDao<T>");

            if (_ABMloggerHabilitado)
                if (string.IsNullOrEmpty(LoggerDb.ConnectionString) )
                    throw new ArgumentException("No se ha Informado la cadena de conexion del Logger");

            SetearPosicion();

            this.KeyPreview = true;
            this.KeyDown += FormABM_KeyDown;
            this.Load += FormABM_Load;
            this.Shown += FormABM_Shown;
            
            _toolBarABM.ToolBarABMClick += _toolBarABM_ToolBarABMClick;
        }

        private void FormABM_Shown(object sender, EventArgs e)
        {
            SetearFocoUniqueControl();
        }

        private void SetearPosicion()
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void FormABM_Load(object sender, EventArgs e)
        {
            InicializarFormulario();
        }

        private void FormABM_KeyDown(object sender, KeyEventArgs e)
        {
            _toolBarABM.analizaTecla(e.KeyCode);
        }

        private void _toolBarABM_ToolBarABMClick(object sender, ToolBarABMClickEventArgs e)
        {
            switch (e.opcion)
            {
                case EOpcionToolBarABM.nuevo:

                    BuscaNuevoCodigo();
                    break;

                case EOpcionToolBarABM.eliminar:

                    if (MessageBox.Show("¿Realmente desea dar de baja el registro seleccionado?", "Aviso al operador", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (DatosValidosEliminacion())
                            if (EliminarRegistro())
                                InicializarFormulario();
                    }
                    break;

                case EOpcionToolBarABM.refresh:

                    InicializarFormulario();
                    break;

                case EOpcionToolBarABM.consultar:

                    Consulta();
                    break;

                case EOpcionToolBarABM.cancelar:

                    _noValidarFormulario = true;
                    if (MessageBox.Show("¿Realmente desea abandonar la carga de datos?", "Aviso al operador", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        InicializarFormulario();

                    break;

                case EOpcionToolBarABM.reporte:
                    LlamarReporte();
                    break;

                case EOpcionToolBarABM.grabar:

                    if (DatosValidos())
                        if (ActualizarRegistro())
                            InicializarFormulario();

                    break;

                case EOpcionToolBarABM.salir:

                    _noValidarFormulario = true;
                    this.Dispose();
                    break;
            }
        }

        protected virtual bool ActualizarRegistro()
        {
            bool esAlta = false;

            if (_entity == null)
            {
                esAlta = true;
                _entity = new T();
            }

            ControlsValuesToEntity();

            if (esAlta)
            {
                try
                {
                    long id = _daoEntity.Add(_entity);
                }
                catch (DaoModelException daoModelException)
                {
                    _entity = default(T);
                    if (_ABMloggerHabilitado)
                        LoggerDb.Log(ELoggerDbTagError.ErrorDeInsercion.ToString(), daoModelException.DbCommand, daoModelException.OriginalException);

                    MessageBox.Show("No pudo grabarse el registro correctamente, intentenlo nuevamente por favor.\r\n\r\n " + daoModelException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
            }
            else
            {
                bool resultadoOk = false;
                try
                {
                    resultadoOk = _daoEntity.Update(_entity);
                }
                catch (DaoModelException daoModelException)
                {
                    if (_ABMloggerHabilitado)
                        LoggerDb.Log(ELoggerDbTagError.ErrorDeActualizacion.ToString(), daoModelException.DbCommand, daoModelException.OriginalException);
                }

                if (!resultadoOk)
                {
                    MessageBox.Show("No pudo actualizarse el registro correctamente, intentenlo nuevamente por favor.", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
            }

            return true;
        }

        protected virtual void ControlsValuesToEntity()
        {
            throw new NotImplementedException();
        }
        protected virtual void EntityValuesToControls()
        {
            throw new NotImplementedException();
        }

        protected virtual bool DatosValidos()
        {
            throw new NotImplementedException();
        }

        protected virtual void LlamarReporte()
        {
            throw new NotImplementedException();
        }

        protected virtual void  Consulta()
        {
            throw new NotImplementedException();
        }

        protected virtual void InicializarFormulario()
        {
            _entity = default(T);

            _validacionFinal = false;
            _noLanzarEventosCombos = true;
            _noValidarFormulario = true;

            FormUtil.CleanControls(this);
            
            ModoEdicion(false);

            SetearFocoUniqueControl();

            _errorProvider.Clear();
            _noLanzarEventosCombos = false;
            _noValidarFormulario = false;
        }

        protected virtual void SetearFocoUniqueControl()
        {
            if (!string.IsNullOrEmpty(UniqueKeyControls))
            {
                string[] controlesUnique = UniqueKeyControls.Split(',');
                this.Controls.Find(controlesUnique[0], true)[0].Focus();
            }
        }

        protected virtual void AsignarDatos()
        {
            _noLanzarEventosCombos = true;

            EntityValuesToControls();

            _noLanzarEventosCombos = false;
        }


        protected virtual void ModoEdicion(bool permiteEdicion)
        {
            FormUtil.EditModeControls(this, permiteEdicion, "", UniqueKeyControls);

            _toolBarABM.setModo(modo: permiteEdicion ? ToolBarABM.eModo.edicion : ToolBarABM.eModo.normal);
            _toolBarABM.btnEliminarHabilitado = (_entity != null);
            _toolBarABM.btnConsultarHabilitado = (!permiteEdicion);
            _toolBarABM.btnReporteHabilitado = true;
        }

        protected virtual bool EliminarRegistro()
        {
            bool result = false;
            try
            {
                result = _daoEntity.DeleteById(GetIdEntity());
            }
            catch (DaoModelException daoModelException)
            {
                if (_ABMloggerHabilitado)
                    LoggerDb.Log(ELoggerDbTagError.ErrorDeEliminacion.ToString(), daoModelException.DbCommand, daoModelException.OriginalException);

                MessageBox.Show("No pudo eliminarse el registro correctamente, ocurrio el siguiente error: \r\n\r\n" + daoModelException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            return result;
        }

        protected virtual long GetIdEntity()
        {
            throw new NotImplementedException();
        }

        public virtual bool DatosValidosEliminacion()
        {
            return true;
        }

        protected virtual void BuscaNuevoCodigo()
        {
            throw new NotImplementedException();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && this.AcceptButton == null)
            {
                TextBoxBase box = this.ActiveControl as TextBoxBase;
                if (box == null || !box.Multiline)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormABM
            // 
            this.ClientSize = new System.Drawing.Size(554, 340);
            this.Name = "FormABM";
            this.ResumeLayout(false);
        }
    }
}

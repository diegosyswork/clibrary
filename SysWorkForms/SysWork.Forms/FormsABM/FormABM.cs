using System;
using System.ComponentModel;
using System.Windows.Forms;
using SysWork.Controls.Toolbars;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.Interfaces;
using SysWork.Data.LoggerDb;
using SysWork.Forms.FormsABM.FormsUtil;

namespace SysWork.Forms.FormsABM
{
    [TypeDescriptionProvider(typeof(AbstractCommunicatorProvider))]
    public abstract partial class FormABM<T>:Form where T:class,new()
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        private bool _isFinalValidation;
        public bool IsFinalValidation
        {
            get { return _isFinalValidation; }
            set { _isFinalValidation = value; }
        }
        
        /// <summary>
        /// 
        /// 
        /// </summary>
        private bool _noThrowComboEvents;
        public bool NoThrowComboEvents
        {
            get { return _noThrowComboEvents; }
            set { _noThrowComboEvents = value; }
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
        private IBaseRepository<T> _repository = null;
        public IBaseRepository<T> Repository
        {
            get { return _repository; }
            set { _repository = value; }
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

        private bool _ABMloggerEnabled = false;
        public bool ABMLoggerEnabled
        {
            get { return _ABMloggerEnabled; }
            set { _ABMloggerEnabled = value; }
        }

        public DbLogger ABMDbLogger { get; private set; }

        public void InitFormABM()
        {
            if (_errorProvider == null)
                throw new NullReferenceException("No se ha referenciado un control del tipo ErrorProvider");

            if (_toolBarABM == null)
                throw new NullReferenceException("No se ha referenciado un control del tipo ToolBarABM");

            if (_repository== null)
                throw new NullReferenceException("No se ha referenciado un IBaseDao<T>");

            if (_ABMloggerEnabled)
            {
                if (string.IsNullOrEmpty(DbLogger.ConnectionString))
                    throw new ArgumentException("No se ha Informado la cadena de conexion del Logger");
            }

            SetPosition();

            this.KeyPreview = true;
            this.KeyDown += FormABM_KeyDown;
            this.Load += FormABM_Load;
            this.Shown += FormABM_Shown;
            
            _toolBarABM.ToolBarABMClick += _toolBarABM_ToolBarABMClick;
        }

        private void FormABM_Load(object sender, EventArgs e)
        {
            InitFormControls();
        }

        private void FormABM_Shown(object sender, EventArgs e)
        {
            SetFocusUniqueControl();
        }
        private void FormABM_KeyDown(object sender, KeyEventArgs e)
        {
            _toolBarABM.AnalizaTecla(e.KeyCode);
        }

        private void SetPosition()
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void _toolBarABM_ToolBarABMClick(object sender, ToolBarABMClickEventArgs e)
        {
            switch (e.opcion)
            {
                case EOpcionToolBarABM.NUEVO:

                    GetNewCode();
                    break;

                case EOpcionToolBarABM.ELIMINAR:

                    if (MessageBox.Show("¿Realmente desea dar de baja el registro seleccionado?", "Aviso al operador", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (IsValidDataForDelete())
                            if (DeleteRecord())
                                InitFormControls();
                    }
                    break;

                case EOpcionToolBarABM.REFRESH:

                    InitFormControls();
                    break;

                case EOpcionToolBarABM.CONSULTAR:

                    QueryData();
                    break;

                case EOpcionToolBarABM.CANCELAR:

                    _noValidarFormulario = true;
                    if (MessageBox.Show("¿Realmente desea abandonar la carga de datos?", "Aviso al operador", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        InitFormControls();

                    break;

                case EOpcionToolBarABM.REPORTE:
                    Report();
                    break;

                case EOpcionToolBarABM.GRABAR:

                    if (IsValidData())
                        if (AddEditRecord())
                            InitFormControls();

                    break;

                case EOpcionToolBarABM.SALIR:

                    _noValidarFormulario = true;
                    this.Dispose();
                    break;
            }
        }

        protected virtual bool AddEditRecord()
        {
            bool isNewRecord = false;

            if (_entity == null)
            {
                isNewRecord = true;
                _entity = new T();
            }

            ControlsValuesToEntity();

            if (isNewRecord)
            {
                try
                {
                    long id = _repository.Add(_entity);
                }
                catch (RepositoryException repositoryException)
                {
                    _entity = default(T);
                    if (_ABMloggerEnabled)
                        DbLogger.LogError(EDbErrorTag.InsertError , "", repositoryException.DbCommand, repositoryException.OriginalException);

                    MessageBox.Show("No pudo grabarse el registro correctamente, intentenlo nuevamente por favor.\r\n\r\n " + repositoryException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
            }
            else
            {
                bool resultadoOk = false;
                try
                {
                    resultadoOk = _repository.Update(_entity);
                }
                catch (RepositoryException repositoryException)
                {
                    if (_ABMloggerEnabled)
                        DbLogger.LogError(EDbErrorTag.UpdateError, "", repositoryException.DbCommand, repositoryException.OriginalException);
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

        protected virtual bool IsValidData()
        {
            throw new NotImplementedException();
        }

        protected virtual void Report()
        {
            throw new NotImplementedException();
        }

        protected virtual void  QueryData()
        {
            throw new NotImplementedException();
        }

        protected virtual void InitFormControls()
        {
            _entity = default(T);

            _isFinalValidation = false;
            _noThrowComboEvents = true;
            _noValidarFormulario = true;

            FormUtil.CleanControls(this);
            
            SetEditMode(false);

            SetFocusUniqueControl();

            _errorProvider.Clear();
            _noThrowComboEvents = false;
            _noValidarFormulario = false;
        }

        protected virtual void SetFocusUniqueControl()
        {
            if (!string.IsNullOrEmpty(UniqueKeyControls))
            {
                string[] controlesUnique = UniqueKeyControls.Split(',');
                this.Controls.Find(controlesUnique[0], true)[0].Focus();
            }
        }

        protected virtual void AssignData()
        {
            _noThrowComboEvents = true;

            EntityValuesToControls();

            _noThrowComboEvents = false;
        }


        protected virtual void SetEditMode(bool permiteEdicion)
        {
            FormUtil.EditModeControls(this, permiteEdicion, "", UniqueKeyControls);

            _toolBarABM.SetModo(modo: permiteEdicion ? ToolBarABM.EModoToolBar.EDICION: ToolBarABM.EModoToolBar.NORMAL);
            _toolBarABM.BtnEliminarHabilitado = (_entity != null);
            _toolBarABM.BtnConsultarHabilitado = (!permiteEdicion);
            _toolBarABM.BtnReporteHabilitado = true;
        }

        protected virtual bool DeleteRecord()
        {
            bool result = false;
            try
            {
                result = _repository.DeleteById(GetIdEntity());
            }
            catch (RepositoryException repositoryException)
            {
                if (_ABMloggerEnabled)
                    DbLogger.LogError(EDbErrorTag.DeleteError, "",repositoryException.DbCommand, repositoryException.OriginalException);

                MessageBox.Show("No pudo eliminarse el registro correctamente, ocurrio el siguiente error: \r\n\r\n" + repositoryException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            return result;
        }

        protected virtual long GetIdEntity()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsValidDataForDelete()
        {
            return true;
        }

        protected virtual void GetNewCode()
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

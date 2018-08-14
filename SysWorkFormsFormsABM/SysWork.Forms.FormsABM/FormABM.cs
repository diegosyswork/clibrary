using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysWork.Controls.Toolbars;
using SysWork.Data.DaoModel;
using SysWork.Util.Logger;
using static SysWork.Util.Logger.LoggerDb;

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

        private bool _loggerHabilitado = false;
        public bool LoggerHabilitado
        {
            get { return _loggerHabilitado; }
            set { _loggerHabilitado = true; }
        }
        public LoggerDb LoggerDb { get; private set; }

        public void InitABMControls()
        {
            if (_errorProvider == null)
                throw new NullReferenceException("No se ha referenciado un control del tipo ErrorProvider");

            if (_toolBarABM == null)
                throw new NullReferenceException("No se ha referenciado un control del tipo ToolBarABM");

            if (_daoEntity== null)
                throw new NullReferenceException("No se ha referenciado un IBaseDao<T>");

            if (_loggerHabilitado)
                if (string.IsNullOrEmpty(LoggerDb.ConnectionString) )
                    throw new ArgumentException("No se ha Informado la cadena de conexion del Logger");

            SetearPosicion();

            this.KeyPreview = true;
            this.KeyDown += FormABM_KeyDown;
            this.Load += FormABM_Load;

            _toolBarABM.ToolBarABMClick += _toolBarABM_ToolBarABMClick;
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

        public virtual bool ActualizarRegistro()
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
                    if (_loggerHabilitado)
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
                    if (_loggerHabilitado)
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

        public virtual void ControlsValuesToEntity()
        {
            throw new NotImplementedException();
        }
        public virtual void EntityValuesToControls()
        {
            throw new NotImplementedException();
        }

        public virtual bool DatosValidos()
        {
            throw new NotImplementedException();
        }

        public virtual void LlamarReporte()
        {
            throw new NotImplementedException();
        }

        public virtual void  Consulta()
        {
            throw new NotImplementedException();
        }

        public virtual void InicializarFormulario()
        {
            _entity = default(T);

            _validacionFinal = false;
            _noLanzarEventosCombos = true;
            _noValidarFormulario = true;

            CleanControls(this);

            ModoEdicion(false);

            _errorProvider.Clear();

            _noLanzarEventosCombos = false;
            _noValidarFormulario = false;
        }

        public virtual void AsignarDatos()
        {
            _noLanzarEventosCombos = true;

            EntityValuesToControls();

            _noLanzarEventosCombos = false;

        }


        public virtual void ModoEdicion(bool permiteEdicion)
        {
            EditModeControls(this, permiteEdicion, "", UniqueKeyControls);

            _toolBarABM.setModo(modo: permiteEdicion ? ToolBarABM.eModo.edicion : ToolBarABM.eModo.normal);
            _toolBarABM.btnEliminarHabilitado = (_entity != null);
            _toolBarABM.btnConsultarHabilitado = (!permiteEdicion);
            _toolBarABM.btnReporteHabilitado = true;
        }

        public virtual bool EliminarRegistro()
        {
            bool result = false;
            try
            {
                result = _daoEntity.DeleteById(GetIdEntity());
            }
            catch (DaoModelException daoModelException)
            {
                if (_loggerHabilitado)
                    LoggerDb.Log(ELoggerDbTagError.ErrorDeEliminacion.ToString(), daoModelException.DbCommand, daoModelException.OriginalException);

                MessageBox.Show("No pudo eliminarse el registro correctamente, ocurrio el siguiente error: \r\n\r\n" + daoModelException.Message, "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            return result;
        }

        public virtual long GetIdEntity()
        {
            throw new NotImplementedException();
        }

        public virtual bool DatosValidosEliminacion()
        {
            return true;
        }

        public virtual void BuscaNuevoCodigo()
        {
            throw new NotImplementedException();
        }

        public static void EditModeControls(Control container, bool allowEdit, string excludedNameControls, string inverseStateNameControls)
        {

            string[] localExcludedNameControls = new string[1];
            string[] localInverseStateNameControls = new string[1];

            if (!string.IsNullOrEmpty(excludedNameControls))
                localExcludedNameControls = excludedNameControls.Split(',');

            if (!string.IsNullOrEmpty(inverseStateNameControls))
                localInverseStateNameControls = inverseStateNameControls.Split(',');

            var listExcluded = localExcludedNameControls.ToList();
            var listInverse = localInverseStateNameControls.ToList();

            foreach (Control control in container.Controls)
            {
                if (!listExcluded.Exists(c => control.Name == c))
                {
                    if (control.Controls.Count > 0)
                    {
                        EditModeControls(control, allowEdit, excludedNameControls, inverseStateNameControls);
                    }
                    else
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).ReadOnly = (listInverse.Exists(i => control.Name == i)) ? allowEdit : !allowEdit;
                        }
                        else if (control is RadioButton)
                        {
                            ((RadioButton)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is MaskedTextBox)
                        {
                            ((MaskedTextBox)control).ReadOnly = (listInverse.Exists(i => control.Name == i)) ? allowEdit : !allowEdit;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Limpia los controles (TextBox, ComboBox,CheckBox,RadioButton)
        /// </summary>
        /// <param name="container">Control contenedor que se recorrera</param>
        /// <param name="excludedNamesControl">lista de controles separada por comas que no seran tenidos en cuenta</param>
        public virtual void CleanControls(Control container, params string[] excludedNamesControl)
        {
            var list = excludedNamesControl.ToList();

            foreach (Control control in container.Controls)
            {
                if (!list.Exists(c => control.Name == c))
                {
                    if (control.Controls.Count > 0)
                    {
                        CleanControls(control, excludedNamesControl);
                    }
                    else
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).Clear();
                        }
                        else if (control is RadioButton)
                        {
                            ((RadioButton)control).Checked = false;
                        }
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Text = "";
                            ((ComboBox)control).SelectedItem = null;
                        }
                        else if (control is MaskedTextBox)
                        {
                            ((MaskedTextBox)control).Clear();
                        }
                    }
                }
            }
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
    }
}

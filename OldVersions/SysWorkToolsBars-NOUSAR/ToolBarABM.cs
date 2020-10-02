using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Controls
{
    public partial class ToolBarABM : UserControl
    {
        public enum EModoToolBar 
        {
            NORMAL,
            EDICION
        }

        public EModoToolBar _estadoModoEdicion { get; private set; }
        public event ToolBarABMClickEventHandler ToolBarABMClick;
        public delegate void ToolBarABMClickEventHandler(Object sender, ToolBarABMClickEventArgs e);

        public bool BtnNuevoHabilitado
        {
            get
            {
                return btnNuevo.Enabled;
            }
            set
            {
                btnNuevo.Enabled = value;
            }
        }
        public bool BtnEliminarHabilitado
        {
            get
            {
                return btnEliminar.Enabled;
            }
            set
            {
                btnEliminar.Enabled = value;
            }
        }
        public bool BtnRefreshHabilitado
        {
            get
            {
                return btnRefresh.Enabled;
            }
            set
            {
                btnRefresh.Enabled = value;
            }
        }
        public bool BtnConsultarHabilitado
        {
            get
            {
                return btnConsultar.Enabled;
            }
            set
            {
                btnConsultar.Enabled = value;
            }
        }
        public bool BtnCancelarHabilitado
        {
            get
            {
                return btnCancelar.Enabled;
            }
            set
            {
                btnCancelar.Enabled = value;
            }
        }
        public bool BtnGrabarHabilitado
        {
            get
            {
                return btnGrabar.Enabled;
            }
            set
            {
                btnGrabar.Enabled = value;
            }
        }
        public bool BtnExportarHabilitado
        {
            get
            {
                return btnExportar.Enabled;
            }
            set
            {
                btnExportar.Enabled = value;
            }
        }
        public bool BtnReporteHabilitado
        {
            get
            {
                return btnReporte.Enabled;
            }
            set
            {
                btnReporte.Enabled = value;
            }
        }
        public bool BtnSalirHabilitado
        {
            get
            {
                return btnSalir.Enabled;
            }
            set
            {
                btnSalir.Enabled = value;
            }
        }


        public ToolBarABM()
        {
            InitializeComponent();
            this._estadoModoEdicion = EModoToolBar.NORMAL;
        }
        
        public void SetModo(EModoToolBar modo)
        {
            this._estadoModoEdicion = modo;

            switch (modo) 
            {
                case EModoToolBar.NORMAL:
                    btnNuevo.Enabled = true;
                    btnEliminar.Enabled = false;
                    btnRefresh.Enabled = true;
                    btnConsultar.Enabled = true;
                    btnCancelar.Enabled = false;
                    btnGrabar.Enabled = false;
                    btnSalir.Enabled = true;
                    break;

                case EModoToolBar.EDICION:
                    btnNuevo.Enabled = false;
                    btnEliminar.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnConsultar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnGrabar.Enabled = true;
                    btnSalir.Enabled = false;
                    break;
            
            }
        }
        public void AnalizaTecla(Keys keyCode)
        {
        
            switch (keyCode)
            {
                case Keys.F3:
                    if (btnNuevo.Enabled)
                        LanzarEventoNuevo();
                    break;
                case Keys.F4:
                    if (btnReporte.Enabled)
                        LanzarEventoReporte();
                    break;
                case Keys.F10:
                    if (btnEliminar.Enabled)
                        LanzarEventoEliminar();
                    break;
                case Keys.F12:
                    if (btnRefresh.Enabled)
                        LanzarEventoRefresh();
                    break;
                case Keys.F5:
                    if (btnConsultar.Enabled)
                        LanzarEventoConsultar();
                    break;
                
                case Keys.F2:
                    if (btnGrabar.Enabled)
                        LanzarEventoGrabar();
                    break;
                
                case Keys.Escape:
                    if (btnCancelar.Enabled)
                        LanzarEventoCancelar();
                    else if (btnSalir.Enabled)
                        LanzarEventoSalir();

                    break;

            }
        }

        protected virtual void OnToolBarABMClick(ToolBarABMClickEventArgs e)
        {
            ToolBarABMClickEventHandler handler = ToolBarABMClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LanzarEventoNuevo();
        }

        private void LanzarEventoNuevo() 
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.NUEVO;
            OnToolBarABMClick(args);
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            LanzarEventoEliminar();
        }
        private void LanzarEventoEliminar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.ELIMINAR;
            OnToolBarABMClick(args);
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LanzarEventoRefresh();
        }

        private void LanzarEventoRefresh()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.REFRESH;
            OnToolBarABMClick(args);
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            LanzarEventoConsultar();
        }

        private void LanzarEventoConsultar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.CONSULTAR;
            OnToolBarABMClick(args);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LanzarEventoCancelar();
        }

        private void LanzarEventoCancelar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.CANCELAR;
            OnToolBarABMClick(args);

        }

        private void BtnGrabar_Click(object sender, EventArgs e)
        {
            LanzarEventoGrabar();
        }

        private void LanzarEventoGrabar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.GRABAR;
            OnToolBarABMClick(args);

        }
        
        private void BtnSalir_Click(object sender, EventArgs e)
        {
            LanzarEventoSalir();
        }

        private void LanzarEventoSalir()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.SALIR;
            OnToolBarABMClick(args);
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {

            LanzarEventoExportar();
        }

        private void LanzarEventoExportar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.EXPORTAR;
            OnToolBarABMClick(args);
        }

        private void BtnReporte_Click(object sender, EventArgs e)
        {

            LanzarEventoReporte();
        }

        private void LanzarEventoReporte()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.REPORTE;
            OnToolBarABMClick(args);
        }
    }

    public class ToolBarABMClickEventArgs : EventArgs
    {
        public EOpcionToolBarABM opcion;
    }
    public enum EOpcionToolBarABM
    {
        NUEVO,
        ELIMINAR,
        REFRESH,
        CONSULTAR,
        EXPORTAR,
        REPORTE,
        CANCELAR,
        GRABAR,
        SALIR
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Controls.Toolbars
{
    public partial class ToolBarABM : UserControl
    {
        public enum eModo 
        {
            normal,
            edicion
        }

        public eModo estadoModoEdicion { get; private set; }
        public event ToolBarABMClickEventHandler ToolBarABMClick;
        public delegate void ToolBarABMClickEventHandler(Object sender, ToolBarABMClickEventArgs e);

        public bool btnNuevoHabilitado
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
        public bool btnEliminarHabilitado
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
        public bool btnRefreshHabilitado
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
        public bool btnConsultarHabilitado
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
        public bool btnCancelarHabilitado
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
        public bool btnGrabarHabilitado
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
        public bool btnExportarHabilitado
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
        public bool btnReporteHabilitado
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
        public bool btnSalirHabilitado
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
            this.estadoModoEdicion = eModo.normal;
        }
        
        public void setModo(eModo modo)
        {
            this.estadoModoEdicion = modo;

            switch (modo) 
            {
                case eModo.normal:
                    btnNuevo.Enabled = true;
                    btnEliminar.Enabled = false;
                    btnRefresh.Enabled = true;
                    btnConsultar.Enabled = true;
                    btnCancelar.Enabled = false;
                    btnGrabar.Enabled = false;
                    btnSalir.Enabled = true;
                    break;

                case eModo.edicion:
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
        public void analizaTecla(Keys keyCode)
        {
        
            switch (keyCode)
            {
                case Keys.F3:
                    if (btnNuevo.Enabled)
                        lanzarEventoNuevo();
                    break;
                case Keys.F4:
                    if (btnReporte.Enabled)
                        lanzarEventoReporte();
                    break;
                case Keys.F10:
                    if (btnEliminar.Enabled)
                        lanzarEventoEliminar();
                    break;
                case Keys.F12:
                    if (btnRefresh.Enabled)
                        lanzarEventoRefresh();
                    break;
                case Keys.F5:
                    if (btnConsultar.Enabled)
                        lanzarEventoConsultar();
                    break;
                
                case Keys.F2:
                    if (btnGrabar.Enabled)
                        lanzarEventoGrabar();
                    break;
                
                case Keys.Escape:
                    if (btnCancelar.Enabled)
                        lanzarEventoCancelar();
                    else if (btnSalir.Enabled)
                        lanzarEventoSalir();

                    break;

            }
        }

        protected virtual void onToolBarABMClick(ToolBarABMClickEventArgs e)
        {
            ToolBarABMClickEventHandler handler = ToolBarABMClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lanzarEventoNuevo();
        }

        private void lanzarEventoNuevo() 
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.nuevo;
            onToolBarABMClick(args);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            lanzarEventoEliminar();
        }
        private void lanzarEventoEliminar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.eliminar;
            onToolBarABMClick(args);
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lanzarEventoRefresh();
        }

        private void lanzarEventoRefresh()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.refresh;
            onToolBarABMClick(args);
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lanzarEventoConsultar();
        }

        private void lanzarEventoConsultar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.consultar;
            onToolBarABMClick(args);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            lanzarEventoCancelar();
        }

        private void lanzarEventoCancelar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.cancelar;
            onToolBarABMClick(args);

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            lanzarEventoGrabar();
        }

        private void lanzarEventoGrabar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.grabar;
            onToolBarABMClick(args);

        }
        
        private void btnSalir_Click(object sender, EventArgs e)
        {
            lanzarEventoSalir();
        }

        private void lanzarEventoSalir()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.salir;
            onToolBarABMClick(args);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {

            lanzarEventoExportar();
        }

        private void lanzarEventoExportar()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.exportar;
            onToolBarABMClick(args);
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {

            lanzarEventoReporte();
        }

        private void lanzarEventoReporte()
        {
            ToolBarABMClickEventArgs args = new ToolBarABMClickEventArgs();
            args.opcion = EOpcionToolBarABM.reporte;
            onToolBarABMClick(args);
        }
    }

    public class ToolBarABMClickEventArgs : EventArgs
    {
        public EOpcionToolBarABM opcion;
    }
    public enum EOpcionToolBarABM
    {
        nuevo,
        eliminar,
        refresh,
        consultar,
        exportar,
        reporte,
        cancelar,
        grabar,
        salir
    }

}

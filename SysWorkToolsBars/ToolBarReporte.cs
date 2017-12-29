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
    public partial class ToolBarReporte : UserControl
    {

        public class ToolBarReporteClickEventArgs : EventArgs
        {
            public eOpcionToolBarReportes opcion;
        }
        public enum eOpcionToolBarReportes
        {
            pantalla,
            impresora,
            exportar,
            refresh,
            consultar,
            salir
        }


        public event ToolBarReporteClickEventHandler ToolBarReporteClick;
        public delegate void ToolBarReporteClickEventHandler(Object sender, ToolBarReporteClickEventArgs e);

        public bool btnPantallaHabilitado
        {
            get
            {
                return btnPantalla.Enabled;
            }
            set
            {
                btnPantalla.Enabled = value;
            }
        }
        public bool btnImpresoraHabilitado
        {
            get
            {
                return btnImprimir.Enabled;
            }
            set
            {
                btnImprimir.Enabled = value;
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

        public ToolBarReporte()
        {
            InitializeComponent();
        }

        protected virtual void onToolBarReporteClick(ToolBarReporteClickEventArgs e)
        {
            ToolBarReporteClickEventHandler handler = ToolBarReporteClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void btnPantalla_Click(object sender, EventArgs e)
        {
            lanzarEventoPantalla();
        }
        private void lanzarEventoPantalla()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = eOpcionToolBarReportes.pantalla;
            onToolBarReporteClick(args);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            lanzarEventoImpresora();
        }
        private void lanzarEventoImpresora()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = eOpcionToolBarReportes.impresora;
            onToolBarReporteClick(args);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            lanzarEventoExportar();
        }
        private void lanzarEventoExportar()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = eOpcionToolBarReportes.exportar;
            onToolBarReporteClick(args);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lanzarEventoRefresh();
        }
        private void lanzarEventoRefresh()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = eOpcionToolBarReportes.refresh;
            onToolBarReporteClick(args);
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lanzarEventoConsultar();
        }
        private void lanzarEventoConsultar()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = eOpcionToolBarReportes.consultar;
            onToolBarReporteClick(args);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            lanzarEventoSalir();
        }
        private void lanzarEventoSalir()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = eOpcionToolBarReportes.salir;
            onToolBarReporteClick(args);
        }
    }
}

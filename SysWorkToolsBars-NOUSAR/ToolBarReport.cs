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
    public partial class ToolBarReporte : UserControl
    {

        public class ToolBarReporteClickEventArgs : EventArgs
        {
            public EOpcionToolBarReportes opcion;
        }
        public enum EOpcionToolBarReportes
        {
            PANTALLA,
            IMPRESORA,
            EXPORTAR,
            REFRESH,
            CONSULTAR,
            SALIR
        }


        public event ToolBarReporteClickEventHandler ToolBarReporteClick;
        public delegate void ToolBarReporteClickEventHandler(Object sender, ToolBarReporteClickEventArgs e);

        public bool BtnPantallaHabilitado
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
        public bool BtnImpresoraHabilitado
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

        public ToolBarReporte()
        {
            InitializeComponent();
        }

        protected virtual void OnToolBarReporteClick(ToolBarReporteClickEventArgs e)
        {
            ToolBarReporteClickEventHandler handler = ToolBarReporteClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void BtnPantalla_Click(object sender, EventArgs e)
        {
            LanzarEventoPantalla();
        }
        private void LanzarEventoPantalla()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = EOpcionToolBarReportes.PANTALLA;
            OnToolBarReporteClick(args);
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            LanzarEventoImpresora();
        }
        private void LanzarEventoImpresora()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = EOpcionToolBarReportes.IMPRESORA;
            OnToolBarReporteClick(args);
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            LanzarEventoExportar();
        }
        private void LanzarEventoExportar()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = EOpcionToolBarReportes.EXPORTAR;
            OnToolBarReporteClick(args);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LanzarEventoRefresh();
        }
        private void LanzarEventoRefresh()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = EOpcionToolBarReportes.REFRESH;
            OnToolBarReporteClick(args);
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            LanzarEventoConsultar();
        }
        private void LanzarEventoConsultar()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = EOpcionToolBarReportes.CONSULTAR;
            OnToolBarReporteClick(args);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            lanzarEventoSalir();
        }
        private void lanzarEventoSalir()
        {
            ToolBarReporteClickEventArgs args = new ToolBarReporteClickEventArgs();
            args.opcion = EOpcionToolBarReportes.SALIR;
            OnToolBarReporteClick(args);
        }

        public void AnalizaTecla(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.F2:
                    if (btnPantalla.Enabled)
                        LanzarEventoPantalla();
                    break;
                case Keys.F3:
                    if (btnImprimir.Enabled)
                        LanzarEventoImpresora();
                    break;
                case Keys.F4:
                    if (btnExportar.Enabled)
                        LanzarEventoExportar();
                    break;
                case Keys.F5:
                    if (btnConsultar.Enabled)
                        LanzarEventoConsultar();
                    break;
                case Keys.F8:
                    if (btnRefresh.Enabled)
                        LanzarEventoRefresh();
                    break;
                case Keys.Escape:
                    if (btnSalir.Enabled)
                        lanzarEventoSalir();
                    break;
            }
        }
    }
}

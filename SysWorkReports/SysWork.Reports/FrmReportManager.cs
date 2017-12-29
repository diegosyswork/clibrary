using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Reports
{
    public partial class FrmReportManager : Form
    {
        public FrmReportManager()
        {
            InitializeComponent();
        }

        public ReportViewer GetReportViewer() 
        {
            return this.reportViewer1;
        }
        public BindingSource GetBindingSource() 
        {
            return BindingSource;
        }

        public void imprimir() 
        {
            ReportPrintDocument reportPrintDocument = new ReportPrintDocument(reportViewer1.LocalReport);
            reportPrintDocument.Print();
        }

        private void FrmReportManager_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.reportViewer1.RefreshReport();
        }
    }
}

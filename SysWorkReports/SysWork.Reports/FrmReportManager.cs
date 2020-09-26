using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace SysWork.Reports
{
    public partial class FrmReportManager : Form
    {
        public ReportViewer ReportViewer { get { return this.reportViewer1; } }
        public BindingSource BindingSource { get { return this.LocalBindingSource; } }

        public FrmReportManager()
        {
            InitializeComponent();
        }
        private void FrmReportManager_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.reportViewer1.RefreshReport();
        }

        public void Print() 
        {
            ReportPrintDocument reportPrintDocument = new ReportPrintDocument(reportViewer1.LocalReport);
            reportPrintDocument.Print();
        }

    }
}

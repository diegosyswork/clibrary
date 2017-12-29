using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Reports
{
    public class ReportManager
    {
        private string _reportPath;
        private ReportViewer _reportViewer;
        private FrmReportManager _frmReportManager;
        
        public String Titulo { get; set; }
        public EDestino Destino { get; set; }
        public EFormato Formato { get; set; }
        public List<ReportDataSource> ReportDataSources { get; set; }
        public BindingSource BindingSource { get; set; }
        public bool MostrarPrintDialog { get; set; }
        public PrinterSettings PrinterSettings { get; set; }
        public PrintController PrintController { get; set; }
        public enum EDestino
        {
            pantalla,
            impresora,
            exportar
        }
        public enum EFormato
        {
            excel,
            word,
            pdf,
            png,
            emf
        }

        public ReportManager(string reportPath)
        {
            this._reportPath = reportPath;
            instanciarControles();
            inicializarVariables(); 
        }
        private void inicializarVariables()
        {
            ReportDataSources = new List<ReportDataSource>();
        }

        private void instanciarControles() 
        {
            _frmReportManager = new FrmReportManager();
            _reportViewer = _frmReportManager.GetReportViewer();
            BindingSource = _frmReportManager.GetBindingSource();
        }

        public void Ejecutar()
        {

            _reportViewer.ProcessingMode = ProcessingMode.Local;
            _reportViewer.LocalReport.ReportPath = this._reportPath;
            
            foreach (ReportDataSource rds in ReportDataSources) 
            {
                _reportViewer.LocalReport.DataSources.Add(rds);
            }
            
            _reportViewer.RefreshReport();

            switch (Destino) 
            {
                case EDestino.pantalla:
                    
                    _frmReportManager.Show();
                    break;

                case EDestino.impresora:
                    
                    ReportPrintDocument reportPrintDocument = new ReportPrintDocument(_reportViewer.LocalReport);

                    if (PrinterSettings != null)
                        reportPrintDocument.PrinterSettings = PrinterSettings;
                    
                    if (PrinterSettings != null)
                        reportPrintDocument.PrintController = PrintController;

                    if (MostrarPrintDialog)
                    {
                        PrintDialog p = new PrintDialog();
                        reportPrintDocument.PrinterSettings = p.PrinterSettings;
                    }

                    reportPrintDocument.Print();
                    break;
                
                case EDestino.exportar:

                    break;
            }
        }
        public ReportViewer GetReportViewer()
        {
            return _reportViewer;
        }
    }

    public class ReportManagerException : Exception 
    {
        public ReportManagerException(string message) : base(message) 
        {

        }
    }
}

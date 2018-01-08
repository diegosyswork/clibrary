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
            InstanciarControles();
            InicializarVariables(); 
        }
        private void InicializarVariables()
        {
            ReportDataSources = new List<ReportDataSource>();
        }

        private void InstanciarControles() 
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

    /* 
    
    TODO: Exportacion a PDF
    string _sPathFilePDF = String.Empty;
    String v_mimetype;
    String v_encoding;
    String v_filename_extension;
    String[] v_streamids;
    Microsoft.Reporting.WinForms.Warning[] warnings;
    string _sSuggestedName = String.Empty;

    Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    Microsoft.Reporting.WinForms.LocalReport objRDLC = new Microsoft.Reporting.WinForms.LocalReport();
    reportViewer1.LocalReport.ReportEmbeddedResource = "reportViewer1.rdlc";
    reportViewer1.LocalReport.DisplayName  = _sSuggestedName;

    objRDLC.DataSources.Clear();
    byte[] byteViewer = rptvFlightPlan.LocalReport.Render("PDF", null, out v_mimetype, out v_encoding, out v_filename_extension, out v_streamids, out warnings);

    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

    saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf";
    saveFileDialog1.FilterIndex = 2;
    saveFileDialog1.RestoreDirectory = true;
    saveFileDialog1.FileName = _sSuggestedName;
    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
    {
        FileStream newFile = new FileStream(saveFileDialog1.FileName, FileMode.Create);
        newFile.Write(byteViewer, 0, byteViewer.Length);
        newFile.Close();
    }
     
     
     
     
     
     */


}

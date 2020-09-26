using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SysWork.Reports
{
    /// <summary>
    /// The destination of the report
    /// </summary>
    public enum ReportDestination
    {
        Screen,
        Printer,
        ExportToFile
    }

    /// <summary>
    /// Format to export
    /// </summary>
    public enum ReportExportFormat
    {
        excel,
        word,
        pdf,
        png,
        emf
    }

    public class ReportManager
    {
        private readonly string _reportPath;
        private ReportViewer _reportViewer;
        private FrmReportManager _frmReportManager;

        /// <summary>
        /// Gets or sets the title of the form.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the destination of Report (Screen, Printer, Export).
        /// </summary>
        /// <value>
        /// Screen, Printer, Export.
        /// </value>
        public ReportDestination ReportDestination { get; set; }

        /// <summary>
        /// Gets or sets the format for export.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public ReportExportFormat ReportExportFormat { get; set; }

        /// <summary>
        /// Gets or sets the report data sources.
        /// </summary>
        /// <value>
        /// The report data sources.
        /// </value>
        public List<ReportDataSource> ReportDataSources { get; set; }

        /// <summary>
        /// Gets or sets the binding source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        public BindingSource BindingSource { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether [show printer dialog].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show printer dialog]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowPrinterDialog { get; set; }

        /// <summary>
        /// Gets or sets the printer settings.
        /// </summary>
        /// <value>
        /// The printer settings.
        /// </value>
        public PrinterSettings PrinterSettings { get; set; }

        /// <summary>
        /// Gets or sets the print controller.
        /// </summary>
        /// <value>
        /// The print controller.
        /// </value>
        public PrintController PrintController { get; set; }

        /// <summary>
        /// Gets the report viewer.
        /// </summary>
        /// <value>
        /// The report viewer.
        /// </value>
        public ReportViewer ReportViewer { get { return _reportViewer; }  }

        public ReportManager(string reportPath)
        {
            _reportPath = reportPath;
            InitControls();
            Init(); 
        }

        private void Init()
        {
            ReportDataSources = new List<ReportDataSource>();
        }

        private void InitControls() 
        {
            _frmReportManager = new FrmReportManager();
            _reportViewer = _frmReportManager.ReportViewer;
            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            BindingSource = _frmReportManager.BindingSource;
            _frmReportManager.Text = Title;
        }

        public void SetDisplayMode(DisplayMode displayMode)
        {
            _reportViewer.SetDisplayMode(displayMode);
        }

        public void RunReport()
        {
            _reportViewer.ProcessingMode = ProcessingMode.Local;
            _reportViewer.LocalReport.ReportPath = _reportPath;

            foreach (ReportDataSource rds in ReportDataSources) 
                _reportViewer.LocalReport.DataSources.Add(rds);
            
            _reportViewer.RefreshReport();

            switch (ReportDestination) 
            {
                case ReportDestination.Screen:
                    
                    _frmReportManager.Show();
                    break;

                case ReportDestination.Printer:
                    
                    ReportPrintDocument reportPrintDocument = new ReportPrintDocument(_reportViewer.LocalReport);

                    if (PrinterSettings != null)
                        reportPrintDocument.PrinterSettings = PrinterSettings;
                    
                    if (PrinterSettings != null)
                        reportPrintDocument.PrintController = PrintController;

                    if (ShowPrinterDialog)
                    {
                        PrintDialog p = new PrintDialog();
                        reportPrintDocument.PrinterSettings = p.PrinterSettings;
                    }

                    reportPrintDocument.Print();
                    break;
                
                case ReportDestination.ExportToFile:
                    throw new NotImplementedException();
            }
        }
    }

    public class ReportManagerException : Exception 
    {
        public ReportManagerException(string message) : base(message) 
        {

        }
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

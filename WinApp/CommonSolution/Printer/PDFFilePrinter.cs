/********************************************************************************************
 * Project Name - PDFFilePrinter
 * Description  - Class to print pdf file on selected printer
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.50.00     20-Jan-2009      Guru S A       Created  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Semnox.Parafait.Printer
{
    public class PDFFilePrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PrintDocument printDocument;
        private string reportFileName; 
        public PDFFilePrinter(PrintDocument printDocument, string reportFileName)
        {
            log.LogMethodEntry(printDocument, reportFileName);
            this.printDocument = printDocument;
            this.reportFileName = reportFileName;
            log.LogMethodExit();
        }

        public void SendPDFToPrinter()
        {
            log.LogMethodEntry();
            try
            {
                using (RadPdfViewer rViewer = new RadPdfViewer())
                {
                    rViewer.DocumentLoaded += RViewer_DocumentLoaded;
                    rViewer.LoadDocument(reportFileName);
                    rViewer.LoadElementTree();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 1819, ex.Message));
            }
            log.LogMethodExit();
        }

        private void RViewer_DocumentLoaded(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                RadPrintDocument document = new RadPrintDocument();
                if (string.IsNullOrEmpty(printDocument.PrinterSettings.PrinterName) != true)
                {
                    document.PrinterSettings.PrinterName = printDocument.PrinterSettings.PrinterName;
                }
                document.DocumentName = printDocument.DocumentName;
                document.Landscape = printDocument.PrinterSettings.DefaultPageSettings.Landscape;
                document.DefaultPageSettings = printDocument.DefaultPageSettings; 
                document.AssociatedObject = (sender as RadPdfViewerElement);
                document.Print();
                document.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 1819, ex.Message));
            }
            log.LogMethodExit();
        }
    }
}

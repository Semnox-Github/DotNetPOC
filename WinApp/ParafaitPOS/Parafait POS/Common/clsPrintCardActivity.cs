/********************************************************************************************
 * Project Name - Common
 * Description  -  Class for clsPrintCardActivity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar        Modified : Added Logger methods and Removed unused namespace's 
 *********************************************************************************************/
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Common
{
    public class clsPrintCardActivity
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PrintDocument MyPrintDocument = new PrintDocument();
        DataGridViewPrinter MyDataGridViewPrinter;
        public void Print(Card Card, DataGridView dgvTransactions, DataGridView dgvGamePlays)
        {
            log.LogMethodEntry(Card);
            if (Card == null)
            {
                log.LogMethodExit("Card == null");
                return;
            }

            if (SetupThePrinting())
            {
                try
                {
                    if (dgvTransactions.Rows.Count > 0)
                    {
                        DataGridView lclTrxDGV = new DataGridView();
                        lclTrxDGV.AllowUserToAddRows = false;
                        foreach (DataGridViewColumn dc in dgvTransactions.Columns)
                            lclTrxDGV.Columns.Add(dc.Clone() as DataGridViewColumn);
                        foreach (DataGridViewRow dr in dgvTransactions.Rows)
                            lclTrxDGV.Rows.Add(CloneWithValues(dr));

                        lclTrxDGV.Columns["POS"].Visible =
                        lclTrxDGV.Columns["Username"].Visible = false;
                        lclTrxDGV.Columns["Refid"].Visible = false;
                        lclTrxDGV.Columns["dcBtnCardActivityTrxPrint"].Visible = false;
                        POSStatic.Utilities.setupDataGridProperties(ref lclTrxDGV);

                        MyPrintDocument.DocumentName = "Transaction Details for Card: " + Card.CardNumber + (Card.customerDTO != null ? " / " + Card.customerDTO.FirstName : "");
                        MyDataGridViewPrinter = new DataGridViewPrinter(lclTrxDGV,
                                    MyPrintDocument, true, true, MyPrintDocument.DocumentName, new System.Drawing.Font("Tahoma", 12,
                                    System.Drawing.FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
                        MyPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(MyPrintDocument_PrintPage);
                        MyPrintDocument.EndPrint += MyPrintDocument_EndPrint;
                        MyPrintDocument.Print();
                        while (trxPrintComplete == false)
                            System.Threading.Thread.Sleep(10);
                    }

                    if (dgvGamePlays.Rows.Count > 0)
                    {
                        DataGridView lclGamePlayDGV = new DataGridView();
                        lclGamePlayDGV.AllowUserToAddRows = false;
                        foreach (DataGridViewColumn dc in dgvGamePlays.Columns)
                            lclGamePlayDGV.Columns.Add(dc.Clone() as DataGridViewColumn);
                        foreach (DataGridViewRow dr in dgvGamePlays.Rows)
                            lclGamePlayDGV.Rows.Add(CloneWithValues(dr));

                        lclGamePlayDGV.Columns[0].Visible = false;
                        POSStatic.Utilities.setupDataGridProperties(ref lclGamePlayDGV);

                        MyPrintDocument.DocumentName = "Game Play Details for Card: " + Card.CardNumber + (Card.customerDTO != null ? " / " + Card.customerDTO.FirstName : "");
                        MyDataGridViewPrinter = new DataGridViewPrinter(lclGamePlayDGV,
                                    MyPrintDocument, true, true, MyPrintDocument.DocumentName, new System.Drawing.Font("Tahoma", 12,
                                    System.Drawing.FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
                        MyPrintDocument.Print();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception :", ex);
                    log.LogMethodExit(null, "Exception at Print() method :" + ex.Message);
                    POSUtils.ParafaitMessageBox(ex.Message, "Print Error");
                }
                log.LogMethodExit();
            }
        }

        DataGridViewRow CloneWithValues(DataGridViewRow row)
        {
            log.LogMethodEntry();
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                clonedRow.Cells[index].Value = row.Cells[index].Value;
            }
            log.LogMethodExit(clonedRow);
            return clonedRow;
        }

        bool trxPrintComplete = false;
        void MyPrintDocument_EndPrint(object sender, PrintEventArgs e)
        {
            log.LogMethodEntry();
            trxPrintComplete = true;
            log.LogMethodExit();
        }

        void MyPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
            log.LogMethodExit();
        }

        // The printing setup function
        bool SetupThePrinting()
        {
            log.LogMethodEntry();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.UseEXDialog = true;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = true;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;

            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins.Left = 0;
            MyPrintDocument.DefaultPageSettings.Margins.Right = 40;
            MyPrintDocument.DefaultPageSettings.Margins.Top = 30;
            MyPrintDocument.DefaultPageSettings.Margins.Bottom = 40;
            log.LogMethodExit(true);
            return true;
        }
    }
}

/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - POS application
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.70.0       24-Jun-2019     Mathew Ninan   Changed External Interface call to use FacilityTableDTO
*2.70.0       17-Jul-2019     Divya A        Filter for My Transactions Tab and Bookings Tab
*2.70         26-Mar-2019     Guru S A       Booking phase 2 enhancement changes
*2.80         20-Sep-2019     Deeksha        Modified to display Consumption Rule message in POS.
*2.80.0      15-Jun-2020      Nitin Pai      Added new function to return Reservation DTO for a given transaction
*2.90.0       24-Jun-2020     Girish Kundar   Modified: Bowa pagas Viber receipt changes
*2.90.0       4-Sep-2020      Girish Kundar   Modified: CardCoreDTO removal issue fix
*2.110.0     22-Dec-2020      Girish Kundar   Modified :FiscalTrust changes - Shift open/Close/PayIn/PayOut to be fiscalized
*2.110.0     10-May-2021      Girish Kundar   Modified :FiscalTrust changes - PayIn/PayOut to be Card count issue Fixes
*2.130.0     19-July-2021     Girish Kundar   Modified : Virtual point column added part of Arcade changes
*2.130.2     24-Dec-2021      Mathew Ninan   Increased size of Message entry form --> Tasks-Message entry form 
*2.140.0     14-Sep-2021      Deeksha         Modified : Provisional Shift and Cash drawer changes
*2.140.0     27-Jun-2021      Fiona Lishal    Modified for Delivery Order enhancements for F&B
*2.140.0     16-Feb-2022      Girish Kundar   Modified : Issue Fixes - Object reference error while Print button is clicked
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.Drawing.Printing;
using System.ComponentModel;
using System.Reflection;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.POS;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Category;
using QRCoder;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    static class POSUtils
    {
        //Begin: Added the parameters to get he value to print on Dec-14-2015//
        static System.Drawing.Printing.PrintDocument MyPrintDocument;
        static ShiftBL shiftBL;
        static double amount = 0;
        static int count = 0;
        internal static DataTable dtCardPurchaseDetails;
        internal static DataTable dtCardPurchaseLoaded;
        private static int dgvPurchasesPageNo = 2;
        private static int dgvPurchasesPageSize = 100;

        internal static DataTable dtCardGamePlayDetails;
        internal static DataTable dtCardGamePlayLoaded;
        private static int dgvGamePlayPageNo = 2;
        private static int dgvGamePlayPageSize = 400;
        private static string fiscalSignature = string.Empty;
        public static List<ShiftDTO> OpenShiftListDTOList;
        public static DateTime? shiftModuleLastUpdatedTime;
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Added the parameters to get he value to print on Dec-14-2015//

        internal static RemotingClient CardRoamingRemotingClient = null;
        public static void displayCardActivity(Card CurrentCard, DataGridView dataGridViewPurchases, DataGridView dataGridViewGamePlay, bool Consolidated, bool Extended)
        {
            log.LogMethodEntry(CurrentCard, dataGridViewPurchases, dataGridViewGamePlay, Consolidated, Extended);
            dataGridViewPurchases.DataSource = null;
            dataGridViewGamePlay.DataSource = null;

            if (CurrentCard == null)
            {
                log.LogMethodExit("CurrentCard == null");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            DataTable PurchaseTbl = new DataTable();
            DataTable GamePlayTbl = new DataTable();

            dtCardPurchaseDetails = new DataTable();
            dtCardPurchaseLoaded = new DataTable();
            dgvPurchasesPageNo = 2;
            dtCardGamePlayDetails = new DataTable();
            dtCardGamePlayLoaded = new DataTable();
            dgvGamePlayPageNo = 2;

            Utilities Utilities = POSStatic.Utilities;

            SqlCommand PurchaseCmd = Utilities.getCommand();
            PurchaseCmd.CommandText = @"select * from CardActivityView  
                                        where card_id = @card_id  
                                        order by date desc, product desc ";

            PurchaseCmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            SqlDataAdapter da = new SqlDataAdapter(PurchaseCmd);
            da.Fill(PurchaseTbl);
            da.Dispose();

            SqlCommand GamePlayCmd = Utilities.getCommand();
            if (!Extended)
                GamePlayCmd.CommandText = @"select card_id, gameplay_id, [Date], Machine, Game, Credits, Courtesy, Bonus, [Time], Tickets, [e-Tickets], [Manual Tickets], [T.Eater Tickets] , Mode, [Site], task_id
                                            from GameMetricViewExtendedForDisplay
                                            where card_id = @card_id 
                                            order by date desc";
            else
                GamePlayCmd.CommandText = @"select *
                                            from GameMetricViewExtendedForDisplay 
                                            where card_id = @card_id 
                                            order by date desc";

            GamePlayCmd.Parameters.Clear();
            GamePlayCmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            SqlDataAdapter dag = new SqlDataAdapter(GamePlayCmd);
            dag.Fill(GamePlayTbl);

            if (Consolidated)
            {
                DataSet ds = null;

                if (CardRoamingRemotingClient != null)
                {
                    try
                    {
                        ds = CardRoamingRemotingClient.GetServerCardActivity(CurrentCard.CardNumber, POSStatic.ParafaitEnv.SiteId);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("fault"))
                        {
                            try
                            {
                                CardRoamingRemotingClient = new RemotingClient();
                            }
                            catch (Exception exe)
                            {
                                log.Error(exe);
                                POSUtils.ParafaitMessageBox(exe.Message);
                            }
                        }
                        else
                            POSUtils.ParafaitMessageBox(ex.Message, "Remoting Error");
                    }
                }

                if (ds != null)
                {
                    DataTable centralPurchaseTbl = ds.Tables[0];
                    DataTable centralGamePlayTbl;

                    if (!Extended)
                        centralGamePlayTbl = ds.Tables[1];
                    else
                        centralGamePlayTbl = ds.Tables[2];

                    if (!centralPurchaseTbl.Columns.Contains("task_id"))
                        centralPurchaseTbl.Columns.Add("task_id");

                    bool found;
                    for (int i = 0; i < PurchaseTbl.Rows.Count; i++)
                    {
                        found = false;
                        for (int j = 0; j < centralPurchaseTbl.Rows.Count; j++)
                        {
                            if (PurchaseTbl.Rows[i]["Date"].Equals(centralPurchaseTbl.Rows[j]["Date"]))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            PurchaseTbl.Rows[i].SetModified(); // flag the row to add it later. setmodified is used to flag as there is no other way              
                    }

                    for (int i = 0; i < PurchaseTbl.Rows.Count; i++)
                    {
                        if (PurchaseTbl.Rows[i].RowState == DataRowState.Modified)
                            centralPurchaseTbl.ImportRow(PurchaseTbl.Rows[i]);
                    }

                    PurchaseTbl = centralPurchaseTbl;
                    PurchaseTbl.DefaultView.Sort = "Date desc, Product desc";

                    for (int i = 0; i < GamePlayTbl.Rows.Count; i++)
                    {
                        found = false;
                        for (int j = 0; j < centralGamePlayTbl.Rows.Count; j++)
                        {
                            if (GamePlayTbl.Rows[i]["Date"].Equals(centralGamePlayTbl.Rows[j]["Date"]))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            GamePlayTbl.Rows[i].SetModified(); // flag the row to add it later. setmodified is used to flag as there is no other way
                    }

                    for (int i = 0; i < GamePlayTbl.Rows.Count; i++)
                    {
                        if (GamePlayTbl.Rows[i].RowState == DataRowState.Modified)
                            centralGamePlayTbl.ImportRow(GamePlayTbl.Rows[i]);
                    }

                    GamePlayTbl = centralGamePlayTbl;
                    GamePlayTbl.DefaultView.Sort = "Date desc";
                }
            }

            PurchaseTbl.Columns.Add("Dummy");
            DataTable dt = Utilities.getReportGridTable(PurchaseTbl, PurchaseTbl.Columns.Count - 1, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 15, 20, 21 });
            dt.Columns.Remove("Dummy");

            if (PurchaseTbl.Rows.Count > 0)
            {
                dt.Rows[dt.Rows.Count - 2].Delete();
                dt.Rows[dt.Rows.Count - 2].Delete();
                dt.Rows[dt.Rows.Count - 1][2] = "Grand Total";

                DataRow dr = dt.NewRow();
                dt.Rows.InsertAt(dr, 0);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Rows[0][i] = dt.Rows[dt.Rows.Count - 1][i];
                }
                dt.Rows[dt.Rows.Count - 1].Delete();
            }
            dtCardPurchaseDetails = dt;

            IEnumerable<DataRow> firstSetOfRows = dt.AsEnumerable().Skip(0).Take(dgvPurchasesPageSize);
            if (firstSetOfRows.Any())
            {
                dtCardPurchaseLoaded = firstSetOfRows.CopyToDataTable();
            }
            else
            {
                dtCardPurchaseLoaded = dtCardPurchaseDetails;
            }
            dataGridViewPurchases.DataSource = dtCardPurchaseLoaded;

            dataGridViewPurchases.Columns["card_id"].Visible = false;
            dataGridViewPurchases.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dataGridViewPurchases.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dataGridViewPurchases.Columns["Courtesy"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Bonus"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Time"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Loyalty Points"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Virtual Points"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Price"].DefaultCellStyle =
            dataGridViewPurchases.Columns["CounterItems"].DefaultCellStyle =
            dataGridViewPurchases.Columns["PlayCredits"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Credits"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();

            dataGridViewPurchases.Columns["Quantity"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Tokens"].DefaultCellStyle =
            dataGridViewPurchases.Columns["Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dataGridViewPurchases.Columns["Tickets"].HeaderText = POSStatic.TicketTermVariant;


            ResetDGVPurchaseRowPrinterImage(dataGridViewPurchases, 0);

            dataGridViewPurchases.Refresh();

            DataTable dtg;
            GamePlayTbl.Columns.Add("dummy");
            if (!Extended)
                dtg = Utilities.getReportGridTable(GamePlayTbl, GamePlayTbl.Columns.Count - 1, new int[] { 5, 6, 7, 8, 9, 10, 11, 12 });
            else
                dtg = Utilities.getReportGridTable(GamePlayTbl, GamePlayTbl.Columns.Count - 1, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });

            dtg.Columns.Remove("dummy");

            if (GamePlayTbl.Rows.Count > 0)
            {
                dtg.Rows[dtg.Rows.Count - 2].Delete();
                dtg.Rows[dtg.Rows.Count - 2].Delete();
                dtg.Rows[dtg.Rows.Count - 1][3] = "Grand Total";

                DataRow dr = dtg.NewRow();
                dtg.Rows.InsertAt(dr, 0);
                for (int i = 0; i < dtg.Columns.Count; i++)
                {
                    dtg.Rows[0][i] = dtg.Rows[dtg.Rows.Count - 1][i];
                }
                dtg.Rows[dtg.Rows.Count - 1].Delete();
            }

            dtCardGamePlayDetails = dtg;
            IEnumerable<DataRow> firstSetOfGamePlayRows = dtg.AsEnumerable().Skip(0).Take(dgvGamePlayPageSize);
            if (firstSetOfGamePlayRows.Any())
            {
                dtCardGamePlayLoaded = firstSetOfGamePlayRows.CopyToDataTable();
            }
            else
            {
                dtCardGamePlayLoaded = dtCardGamePlayDetails;
            }
            dataGridViewGamePlay.DataSource = dtCardGamePlayLoaded;

            dataGridViewGamePlay.Columns["card_id"].Visible = false;
            dataGridViewGamePlay.Columns["gameplay_id"].Visible = false;
            dataGridViewGamePlay.Columns["task_id"].Visible = false;
            dataGridViewGamePlay.Columns["ReverseGamePlay"].Visible = true;
            dataGridViewGamePlay.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dataGridViewGamePlay.Columns["Bonus"].DefaultCellStyle =
            dataGridViewGamePlay.Columns["Credits"].DefaultCellStyle =
            dataGridViewGamePlay.Columns["Courtesy"].DefaultCellStyle =
            dataGridViewGamePlay.Columns["Time"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dataGridViewGamePlay.Columns["Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dataGridViewGamePlay.Columns["e-Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dataGridViewGamePlay.Columns["Manual Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dataGridViewGamePlay.Columns["T.Eater Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dataGridViewGamePlay.Columns["Tickets"].HeaderText = POSStatic.TicketTermVariant;

            if (Extended)
            {
                try
                {
                    dataGridViewGamePlay.Columns["CPCardBalance"].DefaultCellStyle =
                    dataGridViewGamePlay.Columns["CPCredits"].DefaultCellStyle =
                    dataGridViewGamePlay.Columns["CardGame"].DefaultCellStyle =
                    dataGridViewGamePlay.Columns["CPBonus"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                }
                catch { }
            }

            ResetDGVGamePlayRowBackColor(dataGridViewGamePlay, 0);

            dataGridViewGamePlay.Refresh();

            //- following  section was added on 13-07-2015 for language setting in Card Activities
            try
            {
                Utilities.setLanguage(dataGridViewGamePlay);
                Utilities.setLanguage(dataGridViewPurchases);
                dataGridViewGamePlay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            }
            catch { }
            //-changes on 13-07-2015 end

            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        //private static int OpenShiftId = -1;
        public static string OpenShiftUserName = "";

        public static int GetOpenShiftId(string shiftLoginId)
        {
            log.LogMethodEntry(shiftLoginId);
            int shiftId = -1;
            if (OpenShiftListDTOList == null)
            {
                GetOpenShiftDTOList(-1);
            }
            if (OpenShiftListDTOList != null && OpenShiftListDTOList.Any())
            {
                OpenShiftListDTOList = OpenShiftListDTOList.Where(x => x.ShiftAction == ShiftDTO.ShiftActionType.Open.ToString()).ToList();
            }
            if (OpenShiftListDTOList != null && OpenShiftListDTOList.Any() && OpenShiftListDTOList.Exists(x => x.ShiftLoginId == shiftLoginId))
            {
                shiftId = OpenShiftListDTOList.Find(x => x.ShiftLoginId == shiftLoginId).ShiftKey;
            }
            log.LogMethodExit(shiftId);
            return shiftId;
        }

        /// <summary>
        /// This method checks if refresh is required or not
        /// </summary>
        /// <returns>bool</returns>
        public static bool RefreshShiftDTOList()
        {
            log.LogMethodEntry();
            Utilities Utilities = POSStatic.Utilities;
            ShiftListBL shiftListBL = new ShiftListBL(Utilities.ExecutionContext);
            DateTime? lastupdateTime = shiftListBL.GetShiftModuleLastUpdateTime(Utilities.ExecutionContext.GetSiteId());

            if (lastupdateTime.HasValue && shiftModuleLastUpdatedTime.HasValue
                && shiftModuleLastUpdatedTime >= lastupdateTime)
            {
                log.LogMethodExit(false, "No changes in Shift List module since " + shiftModuleLastUpdatedTime);
                return false;
            }
            else
            {
                shiftModuleLastUpdatedTime = lastupdateTime;
                log.LogMethodExit(true);
                return true;
            }
        }
        public static List<ShiftDTO> GetOpenShiftDTOList(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            Utilities Utilities = POSStatic.Utilities;
            if (posMachineId == -1)
            {
                posMachineId = Utilities.ParafaitEnv.POSMachineId;
            }
            if ((OpenShiftListDTOList == null) || RefreshShiftDTOList())
            {
                POSMachines pOSMachinesBL = new POSMachines(Utilities.ExecutionContext, posMachineId);
                OpenShiftListDTOList = pOSMachinesBL.GetAllOpenShifts();
                log.LogMethodExit(OpenShiftListDTOList);
                return OpenShiftListDTOList;
            }
            else
            {
                log.LogMethodExit(OpenShiftListDTOList);
                return OpenShiftListDTOList;
            }
        }

        public static void AddToShift(ref string message, string shiftAction, double shiftAmount, int cardCount, string reason, string remarks, string approverId = "",
                                         DateTime? approvalTime = null)
        {
            log.LogMethodEntry(shiftAction, shiftAmount, cardCount, reason, remarks, approverId, approvalTime);
            Utilities Utilities = POSStatic.Utilities;
            string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER");
            //Added on 05/08/2020 
            bool cashOut = false;
            cashOut = shiftAction.Equals("Paid In") ? false : true;

            //Added on Dec-15-2015      
            MyPrintDocument = new System.Drawing.Printing.PrintDocument();
            MyPrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);

            try
            {
                message = Utilities.MessageUtils.getMessage(294);
                //Begin: Added to make an entry in shift log table each time cas is taken from the cash drawer,Print recipt for the same on Dec-14-2015//
                if (OpenShiftListDTOList.Any())
                {
                    int shiftId = OpenShiftListDTOList[0].ShiftKey;
                    ShiftBL shiftBL = new ShiftBL(Utilities.ExecutionContext, shiftId,true,true);
                    shiftBL.ShiftDTO.CardCount += shiftAction.Equals("Paid In") ? cardCount : -1 * cardCount;
                    shiftBL.ShiftDTO.ShiftAmount += shiftAction.Equals("Paid In") ? shiftAmount : -1 * shiftAmount;

                    ShiftLogDTO shiftLogDTO = new ShiftLogDTO();
                    shiftLogDTO.ShiftKey = shiftBL.ShiftDTO.ShiftKey;
                    shiftLogDTO.ShiftAction = shiftAction;
                    shiftLogDTO.ShiftAmount = shiftAmount;
                    shiftLogDTO.CardCount = cardCount;
                    shiftLogDTO.ShiftReason = reason;
                    shiftLogDTO.ShiftRemarks = remarks;
                    shiftLogDTO.ApproverID = approverId;
                    shiftLogDTO.ApprovalTime = approvalTime;
                    shiftBL.ShiftDTO.ShiftLogDTOList.Add(shiftLogDTO);
                    shiftBL.Save();
                    // Gets the recently added log records
                    shiftBL = new ShiftBL(Utilities.ExecutionContext, shiftId,true,true);
                    ShiftLogDTO shiftLogDTOFiscal = shiftBL.ShiftDTO.ShiftLogDTOList.OrderByDescending(x => x.ShiftLogId).FirstOrDefault();

                    count = shiftBL.ShiftDTO.ShiftLogDTOList.Where(x => (x.ShiftAction.Equals("Paid In") || x.ShiftAction.Equals("Paid Out"))).ToList().Count;
                    amount = Math.Sign(shiftAmount) == -1 ? -1 * shiftAmount : shiftAmount;
                    if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y" &&
                                (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.ELTRADE.ToString())))
                    {
                        if ((Application.OpenForms["POS"] as Parafait_POS.POS).fiscalPrinter == null)
                            (Application.OpenForms["POS"] as Parafait_POS.POS).fiscalPrinter = new FiscalPrinter(Utilities);
                        object shiftTotalAmount = Utilities.executeScalar(@"select sum(shift_amount) from (
                                                                              SELECT shift_amount from shift where shift_key = @shiftId
                                                                               union all
                                                                               select sum(tp.Amount) from TrxPayments tp Join trx_header th
                                                                                       on tp.TrxId = th.TrxId
                                                                                       Where tp.PaymentDate BETWEEN @shiftfromTime AND @shiftToTime
                                                                                       and exists(select 1 from PaymentModes p where isCash = 'Y' and p.PaymentModeId = tp.PaymentModeId)
                                                                                     and isnull(tp.PosMachine, th.pos_machine) = @posMachine)a"
                                                                                    , new SqlParameter("@shiftId", shiftId)
                                                                                    , new SqlParameter("@posMachine", POSStatic.ParafaitEnv.POSMachine),
                                                                                   new SqlParameter("@shiftfromTime", POSStatic.ParafaitEnv.LoginTime),
                                                                                   new SqlParameter("@shiftToTime", ServerDateTime.Now));
                        try
                        {
                            if (!(Application.OpenForms["POS"] as Parafait_POS.POS).fiscalPrinter.DepositeInDrawer(amount, Convert.ToDouble(shiftTotalAmount), cashOut))
                            {
                                log.Error("Error while printing ");
                                throw new Exception("Error while printing");
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            throw new Exception(ex.Message);
                        }
                    }
                    // To Fiscaltrust printer sets UseFiscalPrinter  = false . So need to check here
                    else if (fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
                    {
                        if (string.IsNullOrEmpty(fiscalPrinterType) == false)
                        {
                            (Application.OpenForms["POS"] as POS).fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterType);
                        }
                        Utilities.ExecutionContext.SetMachineId(Utilities.ParafaitEnv.POSMachineId);
                        FiscalizationBL fiscalizationHelper = new FiscalizationBL(Utilities.ExecutionContext);
                        FiscalizationRequest fiscalizationRequest = fiscalizationHelper.BuildShiftFiscalizationRequest(shiftLogDTOFiscal);
                        bool isSuccess = (Application.OpenForms["POS"] as Parafait_POS.POS).fiscalPrinter.PrintReceipt(fiscalizationRequest, ref fiscalSignature);
                        if (isSuccess)
                        {
                            fiscalizationHelper.UpdateShiftPaymentReference(fiscalizationRequest, fiscalSignature);
                            printshiftDetails();
                        }
                    }
                    else if (POSUtils.ParafaitMessageBox("Do you want to Print?", "Print Paid Out Receipt", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        printshiftDetails();
                    }
                    else
                    {
                        log.LogMethodExit("Do you want to Print? return null");
                        return;
                    }
                }
                log.LogMethodEntry();
                //End: Added to make an entry in shift log table each time cas is taken from the cash drawer,Print recipt for the same on Dec-14-2015//
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error(ex);
            }
        }

        //Begin: Added to print initiate printing parameters on Dec-14-2015
        private static bool SetupThePrinting()
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
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            MyPrintDialog.UseEXDialog = true;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;

            MyPrintDocument.DocumentName = POSStatic.Utilities.MessageUtils.getMessage("POS Paid In/Out Receipt");
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(10, 10, 20, 20);
            log.LogMethodExit(true);
            return true;
        }
        //End: Added to print initiate printing parameters on Dec-14-2015

        //Begin: Added to print the receipt each time cash is taken from the cash drawer on Dec-14-2015//
        public static bool printshiftDetails()
        {
            log.LogMethodEntry();
            if (SetupThePrinting())
            {
                try
                {
                    MyPrintDocument.Print();
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message, POSStatic.MessageUtils.getMessage("Paid Out Print Error"));
                    log.Error(ex);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        //end//
        //Begin: Added to print the receipt each time cash is taken from the cash drawer on Dec-14-2015//
        public static void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            ShiftLogDTO shiftLogDTO = null;
            Utilities Utilities = POSStatic.Utilities;
            int col1x = 2;
            int yLocation = 20;
            int yIncrement = 20;
            Font defaultFont = new System.Drawing.Font("courier narrow", 10f);
            e.Graphics.DrawString(POSStatic.ParafaitEnv.SiteName, new Font(defaultFont.FontFamily, 12F, FontStyle.Bold), Brushes.Black, col1x, yLocation);
            yLocation += yIncrement + 5;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("POS Paid In/Out Receipt"), new Font(defaultFont.FontFamily, 12F, FontStyle.Bold), Brushes.Black, col1x, yLocation);
            yLocation += yIncrement + 10;

            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("POS Name") + ": " + POSStatic.ParafaitEnv.POSMachine, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("User Name: ") + " " + POSStatic.ParafaitEnv.Username, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            ShiftBL shiftBL = new ShiftBL(Utilities.ExecutionContext, OpenShiftListDTOList[0].ShiftKey, true, true);
            List<ShiftLogDTO> shiftLogDTOList = shiftBL.ShiftDTO.ShiftLogDTOList.Where(x => (x.ShiftAction.Equals("Paid In") || x.ShiftAction.Equals("Paid Out"))).ToList();
            if (shiftLogDTOList != null && shiftLogDTOList.Any())
            {
                shiftLogDTOList = shiftLogDTOList.OrderByDescending(x => x.ShiftLogId).ToList();
                shiftLogDTO = shiftLogDTOList.FirstOrDefault();
            }
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Approver : ") + " " + shiftLogDTO.ApproverID, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement + 10;

            e.Graphics.DrawString(Utilities.MessageUtils.getMessage(shiftLogDTO.ShiftAction + " Cash: ") + " " + amount.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            if (!(shiftLogDTO.CardCount <= 0))
            {
                e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Card " + (shiftLogDTO.ShiftAction == "Paid In" ? "Added" : "Removed") + " : ") + " " + shiftLogDTO.CardCount, defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
            }
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage(shiftLogDTO.ShiftAction + " Time: ") + " " + ServerDateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage(shiftLogDTO.ShiftAction + " Reason: ") + " " + shiftLogDTO.ShiftReason, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage(shiftLogDTO.ShiftAction + " Remarks: ") + " " + shiftLogDTO.ShiftRemarks, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement + 10;

            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Paid In/Out Count: ") + " " + count, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            // Adding signature QR code If the printer is  German ficalization 
            string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER");
            if (string.IsNullOrEmpty(fiscalPrinterType) == false && fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
            {
                FiskaltrustMapper fiskaltrustMapper = new FiskaltrustMapper(Utilities.ExecutionContext);
                if (String.IsNullOrEmpty(fiscalSignature) == false)
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData =
                        qrGenerator.CreateQrCode(fiscalSignature, QRCodeGenerator.ECCLevel.M);
                    QRCode qrCode = new QRCode(qrCodeData);
                    if (qrCode != null)
                    {
                        int pixelPerModule = 1;
                        Image bitmap = qrCode.GetGraphic(pixelPerModule);
                        yLocation += yIncrement;
                        e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Fiscalization Reference "), defaultFont, Brushes.Black, col1x, yLocation);
                        yLocation += yIncrement;
                        e.Graphics.DrawString(Utilities.MessageUtils.getMessage("QRCode : "), defaultFont, Brushes.Black, col1x, yLocation);
                        yLocation += yIncrement;
                        e.Graphics.DrawImage(bitmap, 100, yLocation);
                    }
                    else
                    {
                        yLocation += yIncrement;
                        e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Fiscalization Reference  :" + fiskaltrustMapper.GetSingatureErrorMessage()), defaultFont, Brushes.Black, col1x, yLocation);
                    }
                }
            }
            log.LogMethodExit();
        }
        //End//

        /*public static bool FirstDataTransactionExists(int TrxId = -1)
        {
            List<int> PaymentsMode = new List<int>();
            Utilities utilities = POSStatic.Utilities;
            DataTable dTable;

            dTable = utilities.executeDataTable(@"select 1
                                                    from TrxPayments tp
                                                    where  Amount>0 and  
                                                    exists (select PaymentModeId 
	                                                    from  PaymentModes 
	                                                    where Gateway 
	                                                    in(select LookupValueId 
		                                                    from LookupValues 
		                                                    where exists(select lookupid 
					                                                    from Lookups 
					                                                    where LookupName='PAYMENT_GATEWAY'
					                                                    ) 
			                                                    and LookupValue='FirstData'
		                                                    ) and PaymentModeId= tp.PaymentModeId
	                                                    )	
                                                        and not exists(select PaymentId from trxpayments where ParentPaymentId=tp.PaymentId and TrxId=tp.TrxId)	
	                                                    and exists(select ResponseID 
	                                                                from CCTransactionsPGW  
				                                                    where TranCode = 'Authorization' and ResponseID = tp.CCResponseId) 
                                                        and (Trxid = @trxid or @trxid = -1) and PosMachine='" + utilities.ParafaitEnv.POSMachine + "' order by Trxid ",
                                                                 new SqlParameter("@trxid", TrxId));
            if (dTable != null && dTable.Rows.Count > 0)
            {
                return dTable.Rows[0][0].ToString().Equals("1");
            }
            return false;
        }*/
        public static bool IsOpenTransactionExists(int posmachineId = -1)
        {
            log.LogMethodEntry();
            Utilities utilities = POSStatic.Utilities;
            DataTable dTable;
            dTable = utilities.executeDataTable(@"select * 
                                                      from trx_header 
                                                     where Status IN ('OPEN','INITIATED','ORDERED','PREPARED', 'PENDING') 
                                                       and POSMachineId = @posMachineId 
                                                        AND trxdate <= case when getdate() between 00 and 06
					                                    then dateadd(hour,6,convert(datetime,convert(date,getdate())))
					                                    else dateadd(hour,6,convert(datetime,convert(date,getdate() + 1)))
					                                    end
                                                    UNION ALL
                                                    select h.* 
                                                      from trx_header h, bookings b
                                                     where h.Status IN ('RESERVED') 
                                                       and b.trxid = h.trxid
                                                       and POSMachineId = @posMachineId
	                                                    AND trxdate <= case when getdate() between 00 and 06
					                                                    then dateadd(hour,6,convert(datetime,convert(date,getdate())))
					                                                    else dateadd(hour,6,convert(datetime,convert(date,getdate() + 1)))
					                                                    end ",
                                                                 new SqlParameter("@posMachineId", posmachineId));
            if (dTable != null && dTable.Rows.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }
        public static bool IsTransactionExists(string status, bool isCurrentPOSMachine, int userId, bool isForCurrentBussinessDay)
        {
            log.LogMethodEntry(status, isCurrentPOSMachine, userId, isForCurrentBussinessDay);
            Utilities utilities = POSStatic.Utilities;
            DataTable dTable;
            int hour = Convert.ToInt32(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"));
            DateTime fromDate = ServerDateTime.Now.Date.AddHours(hour);
            DateTime todate = fromDate.AddDays(1);
            if (ServerDateTime.Now.CompareTo(fromDate) < 0)
            {
                todate = fromDate;
                fromDate = todate.AddDays(-1);
            }
            dTable = utilities.executeDataTable(@"select * from trx_header where Status IN ('OPEN', 'INITIATED','ORDERED','PREPARED', 'PENDING') and (POSMachineId = @posMachineId or @posMachineId = -1) and (user_id = @userId or @userId = -1) " + ((isForCurrentBussinessDay) ? "and TrxDate >= '" + fromDate.ToString("yyyy-MM-dd HH:mm") + "' and TrxDate <= '" + todate.ToString("yyyy-MM-dd HH:mm") + "'" : ""),
                                                                 new SqlParameter("@posMachineId", ((isCurrentPOSMachine) ? utilities.ParafaitEnv.POSMachineId : -1)),
                                                                 //new SqlParameter("@status", (string.IsNullOrEmpty(status))?"ALL": status),
                                                                 new SqlParameter("@userId", userId));
            if (dTable != null && dTable.Rows.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }
        public static bool IsPendingPaymentExists(string LogingId = null)
        {
            log.LogMethodEntry();
            bool returnValue = false;
            //The Default value ALLOW_CREDIT_CARD_AUTHORIZATION will be changed to a generic PAYMENT_GATEWAY_AUTHORIZATION_ENABLED
            if (POSStatic.Utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
            {
                try
                {
                    PaymentModeList paymentModesListBL = new PaymentModeList(POSStatic.Utilities.ExecutionContext);
                    List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                    if (paymentModesDTOList != null && paymentModesDTOList.Count > 0)
                    {
                        foreach (var paymentModesDTO in paymentModesDTOList)
                        {
                            PaymentMode paymentModesBL = new PaymentMode(POSStatic.Utilities.ExecutionContext, paymentModesDTO);
                            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway);
                            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = paymentGateway.GetAllUnsettledCreditCardTransactions();
                            if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                            {
                                List<int> responseIdList = new List<int>();
                                foreach (var cCTransactionsPGWDTO in cCTransactionsPGWDTOList)
                                {
                                    responseIdList.Add(cCTransactionsPGWDTO.ResponseID);
                                }
                                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                                //searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.POS_MACHINE, POSStatic.Utilities.ParafaitEnv.POSMachine));
                                searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.LAST_UPDATED_USER, LogingId));
                                List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParameters, responseIdList);
                                if (transactionPaymentsDTOList != null)
                                {
                                    returnValue = true;
                                    break;
                                }

                            }
                        }
                    }
                }
                catch (Exception)
                {
                    log.Error("Exception Occurred");
                    log.LogMethodExit(false);
                    returnValue = false;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public static void EnterTransactionRemarks(Transaction Trx)
        {
            log.LogMethodEntry();
            if (Trx == null)
            {
                log.LogMethodExit("trx == null");
                return;
            }

            string TrxRemarks = Trx.Remarks;
            GenericDataEntry trxRemarks = new GenericDataEntry(1);
            trxRemarks.Text = POSStatic.MessageUtils.getMessage(295);
            trxRemarks.DataEntryObjects[0].mandatory = false;
            trxRemarks.DataEntryObjects[0].label = "Transaction Remarks";
            trxRemarks.DataEntryObjects[0].data = TrxRemarks;
            if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                if (string.Empty.Equals(TrxRemarks))
                {
                    log.LogMethodExit("TrxRemarks is empty");
                    return;
                }
                Trx.Remarks = TrxRemarks;
            }
            else
            {
                log.LogMethodExit("trxRemarks.ShowDialog() != System.Windows.Forms.DialogResult.OK");
                return;
            }
        }

        public static void LogMessage()
        {
            log.LogMethodEntry();
            Utilities Utilities = POSStatic.Utilities;
            string logMessage;
            GenericDataEntry logMessageFrm = new GenericDataEntry(1);
            logMessageFrm.Text = POSStatic.MessageUtils.getMessage(296);
            logMessageFrm.DataEntryObjects[0].mandatory = false;
            logMessageFrm.DataEntryObjects[0].label = "Message";
            logMessageFrm.DataEntryObjects[0].multiline = true;
            logMessageFrm.DataEntryObjects[0].height = 90;
            logMessageFrm.DataEntryObjects[0].maxlength = 250;
            if (logMessageFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                logMessage = logMessageFrm.DataEntryObjects[0].data;
                if (string.Empty.Equals(logMessage))
                {
                    log.LogMethodExit("logMessage is empty");
                    return;
                }
            }
            else
            {
                log.LogMethodExit("logMessageFrm.ShowDialog() != System.Windows.Forms.DialogResult.OK");
                return;
            }

            Utilities.EventLog.logEvent("POS", 'D', logMessage, "", "User Message", 0);
        }

        public static DataTable dtOtherPaymentModes = new DataTable();
        public static DataTable dtCreditCards = new DataTable();
        public static DataTable dtAllPaymentModes = new DataTable();

        public static void initializePaymentModeDT()
        {
            log.LogMethodEntry();
            Utilities Utilities = POSStatic.Utilities;
            dtOtherPaymentModes = Utilities.executeDataTable(@"select PPI.PaymentModeId, ISNULL(PPI.friendlyName, PaymentMode) PaymentMode
                                                                from PaymentModes pm, POSPaymentModeInclusions PPI
                                                                where isCreditCard = 'N' 
	                                                            and isCash = 'N' 
	                                                            and isDebitCard = 'N' 
	                                                            and POSAvailable = 1 
	                                                            and pm.PaymentModeId = ppi.PaymentModeId
	                                                            and ppi.POSMachineId = @POSMachineId
	                                                            order by isnull(displayOrder, 9999), 2",
                                                                new SqlParameter("@PosMachineId", POSStatic.Utilities.ParafaitEnv.POSMachineId));

            dtCreditCards = Utilities.executeDataTable(@"select p.PaymentModeId, ISNULL(ppi.friendlyName, PaymentMode) PaymentMode, CreditCardSurchargePercentage, lv.LookupValue Gateway
                                                            from POSPaymentModeInclusions PPI, PaymentModes p
                                                            left outer join LookupView lv
                                                            on lv.LookupValueId = p.Gateway
                                                            and lv.LookupName = 'PAYMENT_GATEWAY'
                                                            where isCreditCard = 'Y' 
                                                                and POSAvailable = 1 
		                                                        and PPI.PaymentModeId = p.PaymentModeId
		                                                        and PPI.POSMachineId = @POSMachineId
                                                            order by isnull(displayOrder, 9999), 2",
                                                         new SqlParameter("@PosMachineId", POSStatic.Utilities.ParafaitEnv.POSMachineId));

            dtAllPaymentModes = Utilities.executeDataTable(@"select pm.PaymentModeId, isNULL(friendlyName, pm.PaymentMode) PaymentMode, isCreditCard, CreditCardSurchargePercentage, 
                                                                   pm.Guid, pm.SynchStatus, pm.site_id, isCash
                                                                   , isDebitCard, Gateway, ManagerApprovalRequired, isRoundOff, POSAvailable, DisplayOrder, ImageFileName
	                                                               , pm.MasterEntityId, IsQRCode, pm.CreatedBy, pm.CreationDate, pm.LastUpdatedBy, pm.LastUpdateDate, 
	                                                               PaymentReferenceMandatory, pm.IsActive
                                                              from PaymentModes pm, POSPaymentModeInclusions PPI
                                                             where POSAvailable = 1
                                                               and pm.PaymentModeId = ppi.PaymentModeId
                                                               and ppi.POSMachineId = @POSMachineId
                                                             order by isnull(DisplayOrder, 9999), iscash desc, isDebitCard desc, isCreditCard desc, PaymentMode",
                                                               new SqlParameter("@PosMachineId", POSStatic.Utilities.ParafaitEnv.POSMachineId));
            log.LogMethodExit();
        }

        public static DataGridView CopyDataGridView(DataGridView dgv_org)
        {
            log.LogMethodEntry();
            DataGridView dgv_copy = new DataGridView();
            try
            {
                if (dgv_copy.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in dgv_org.Columns)
                    {
                        dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }

                DataGridViewRow row = new DataGridViewRow();

                for (int i = 0; i < dgv_org.Rows.Count; i++)
                {
                    row = (DataGridViewRow)dgv_org.Rows[i].Clone();
                    int intColIndex = 0;
                    foreach (DataGridViewCell cell in dgv_org.Rows[i].Cells)
                    {
                        row.Cells[intColIndex].Value = cell.Value;
                        intColIndex++;
                    }
                    dgv_copy.Rows.Add(row);
                }
                dgv_copy.AllowUserToAddRows = false;
                dgv_copy.Refresh();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("DataGridView Exception : ", ex);
                POSUtils.ParafaitMessageBox(ex.Message, "Copy DataGridView");
            }

            dgv_copy.ReadOnly = dgv_org.ReadOnly;
            dgv_copy.AllowUserToDeleteRows = dgv_org.AllowUserToDeleteRows;
            POSStatic.Utilities.setupDataGridProperties(ref dgv_copy);
            dgv_copy.Height = dgv_org.Height;
            dgv_copy.BackgroundColor = dgv_org.BackgroundColor;

            int width = 0;
            foreach (DataGridViewColumn column in dgv_copy.Columns)
                if (column.Visible == true)
                    width += column.Width;
            dgv_copy.Width = width + 20;
            log.LogMethodExit(dgv_copy);
            return dgv_copy;
        }

        static Timer alertTimer = new Timer();
        static Button lblAlert = new Button();
        static List<string> alertMessages = new List<string>();
        static int alertMessageIndex = 0;



        public static void displayCardAlerts(Card card, Button labelAlert)
        {
            log.LogMethodEntry(card, labelAlert);
            Utilities Utilities = POSStatic.Utilities;
            string message = "";
            clearAlerts();

            string vipMessage = "";
            if (card.customer_id != -1 && card.MembershipId != -1)
            {
                SqlCommand cmdLoyalty = Utilities.getCommand();
                SqlCommand cmdMembership = Utilities.getCommand();
                cmdMembership.CommandText = @"select m.membershipName, m.membershipId , mr.QualifyingPoints, mr.QualificationWindow, mr.UnitOfQualificationWindow
                                                from Membership m ,
                                                     membershipRule mr
                                               where m.MembershipRuleID = mr.MembershipRuleID
                                                 and m.IsActive = 1
                                                 and m.BaseMembershipID = @membershipId";
                cmdMembership.Parameters.AddWithValue("@membershipId", card.MembershipId);
                SqlDataAdapter dap = new SqlDataAdapter(cmdMembership);
                DataTable dtMembership = new DataTable();
                dap.Fill(dtMembership);
                if (dtMembership.Rows.Count > 0)
                {
                    int qualificationWindow = 0;
                    int qualifyingPoints = 0;
                    string unitOfQualificationWindow = "";
                    string membershipName = "";
                    DateTime fromDate;
                    DateTime baseDate = ServerDateTime.Now;
                    if (dtMembership.Rows[0]["QualificationWindow"] != null && dtMembership.Rows[0]["QualificationWindow"].ToString() != "")
                        qualificationWindow = Convert.ToInt32(dtMembership.Rows[0]["QualificationWindow"]);
                    if (dtMembership.Rows[0]["QualifyingPoints"] != null && dtMembership.Rows[0]["QualifyingPoints"].ToString() != "")
                        qualifyingPoints = Convert.ToInt32(dtMembership.Rows[0]["QualifyingPoints"]);
                    if (dtMembership.Rows[0]["UnitOfQualificationWindow"] != null)
                        unitOfQualificationWindow = dtMembership.Rows[0]["UnitOfQualificationWindow"].ToString();
                    if (dtMembership.Rows[0]["membershipName"] != null)
                        membershipName = dtMembership.Rows[0]["membershipName"].ToString();
                    if (qualificationWindow > 0 && qualifyingPoints > 0)
                    {

                        DateTime dateValue = ((unitOfQualificationWindow == "D") ? baseDate.AddDays(qualificationWindow) : ((unitOfQualificationWindow == "M") ? baseDate.AddMonths(qualificationWindow) : ((unitOfQualificationWindow == "Y") ? baseDate.AddYears(qualificationWindow) : DateTime.MinValue)));
                        if (dateValue != DateTime.MinValue)
                        {

                            System.TimeSpan diff = dateValue.Subtract(baseDate);
                            fromDate = baseDate - diff;

                            cmdLoyalty.CommandText = @" select sum(CreditPlus) loyaltyPoints
                                                          from CardCreditPlus cp, cards c
                                                         where cp.card_id = c.card_id
                                                           and c.valid_flag='Y'
                                                           and c.customer_id = @customerId
                                                           and cp.creditPLusType = 'L'
                                                           and ISNULL(cp.PeriodFrom, c.issue_date) >= @fromDate
                                                           and isnull(cp.PeriodFrom, @toDate) <= @toDate";
                            cmdLoyalty.Parameters.AddWithValue("@fromDate", fromDate);
                            cmdLoyalty.Parameters.AddWithValue("@toDate", baseDate);
                            cmdLoyalty.Parameters.AddWithValue("@customerId", card.customer_id);

                            SqlDataAdapter dapl = new SqlDataAdapter(cmdLoyalty);
                            DataTable dtLoyaltyPts = new DataTable();
                            dapl.Fill(dtLoyaltyPts);
                            if (dtLoyaltyPts.Rows.Count > 0)
                            {
                                int customerPoints = 0;
                                if (dtLoyaltyPts.Rows[0]["loyaltyPoints"] != null && dtLoyaltyPts.Rows[0]["loyaltyPoints"].ToString() != "")
                                    customerPoints = Convert.ToInt32(dtLoyaltyPts.Rows[0]["loyaltyPoints"]);
                                if ((qualifyingPoints - customerPoints) > 0)
                                {
                                    vipMessage = MessageContainerList.GetMessage(Utilities.ExecutionContext, "You need ") + (qualifyingPoints - customerPoints).ToString(POSStatic.ParafaitEnv.NUMBER_FORMAT)
                                                 + MessageContainerList.GetMessage(Utilities.ExecutionContext, " points to become ") + membershipName;
                                }
                            }
                        }
                    }

                }
                if (vipMessage != "")
                    alertMessages.Add(vipMessage);
                vipMessage = "";
                cmdMembership.Parameters.Clear();
                cmdLoyalty.Parameters.Clear();
                cmdMembership.CommandText = @"select m.membershipName, m.membershipId ,  mr.RetentionPoints, mr.RetentionWindow, mr.UnitOfRetentionWindow
                                                from Membership m ,
                                                     membershipRule mr
                                               where m.MembershipRuleID = mr.MembershipRuleID
                                                 and m.IsActive = 1
                                                 and m.membershipId = @membershipId";
                cmdMembership.Parameters.AddWithValue("@membershipId", card.MembershipId);
                SqlDataAdapter dapin = new SqlDataAdapter(cmdMembership);
                DataTable dtMembershipOne = new DataTable();
                dapin.Fill(dtMembershipOne);
                if (dtMembershipOne.Rows.Count > 0)
                {
                    int retentionPoints = 0;
                    int retentionWindow = 0;
                    string membershipName = "";
                    string unitOfRetentionWindow = "";
                    DateTime fromDate;
                    DateTime baseDate = ServerDateTime.Now;
                    if (dtMembershipOne.Rows[0]["RetentionPoints"] != null && dtMembershipOne.Rows[0]["RetentionPoints"].ToString() != "")
                        retentionPoints = Convert.ToInt32(dtMembershipOne.Rows[0]["RetentionPoints"]);
                    if (dtMembershipOne.Rows[0]["RetentionWindow"] != null && dtMembershipOne.Rows[0]["RetentionWindow"].ToString() != "")
                        retentionWindow = Convert.ToInt32(dtMembershipOne.Rows[0]["RetentionWindow"]);
                    if (dtMembershipOne.Rows[0]["UnitOfRetentionWindow"] != null)
                        unitOfRetentionWindow = dtMembershipOne.Rows[0]["UnitOfRetentionWindow"].ToString();
                    if (dtMembershipOne.Rows[0]["membershipName"] != null)
                        membershipName = dtMembershipOne.Rows[0]["membershipName"].ToString();
                    if (retentionPoints > 0)
                    {
                        cmdMembership.Parameters.Clear();
                        cmdMembership.CommandText = @"select EffectiveFromDate, EffectiveToDate from MembershipProgression 
                                                        where customerid = @customerId
                                                           and membershipId = @membershipId
                                                           and effectiveToDate >= getdate()
                                                           order by EffectiveToDate desc ";
                        cmdMembership.Parameters.AddWithValue("@membershipId", card.MembershipId);
                        cmdMembership.Parameters.AddWithValue("@customerId", card.customer_id);
                        SqlDataAdapter dapinn = new SqlDataAdapter(cmdMembership);
                        DataTable dtMembershipTwo = new DataTable();
                        dapinn.Fill(dtMembershipTwo);
                        if (dtMembershipTwo.Rows.Count > 0)
                        {
                            DateTime effectiveFromDate = DateTime.MinValue;
                            DateTime effectiveToDate = DateTime.MinValue;
                            if (dtMembershipTwo.Rows[0]["EffectiveFromDate"] != null && dtMembershipTwo.Rows[0]["EffectiveFromDate"].ToString() != "")
                            {
                                effectiveFromDate = Convert.ToDateTime(dtMembershipTwo.Rows[0]["EffectiveFromDate"]);
                            }

                            if (dtMembershipTwo.Rows[0]["EffectiveToDate"] != null && dtMembershipTwo.Rows[0]["EffectiveToDate"].ToString() != "")
                            {
                                effectiveToDate = Convert.ToDateTime(dtMembershipTwo.Rows[0]["EffectiveToDate"]);
                            }

                            DateTime dateValue = ((unitOfRetentionWindow == "D") ? baseDate.AddDays(retentionWindow) : ((unitOfRetentionWindow == "M") ? baseDate.AddMonths(retentionWindow) : ((unitOfRetentionWindow == "Y") ? baseDate.AddYears(retentionWindow) : DateTime.MinValue)));
                            if (dateValue != DateTime.MinValue)
                            {

                                System.TimeSpan diff = dateValue.Subtract(baseDate);
                                fromDate = effectiveToDate - diff;
                                cmdLoyalty.CommandText = @" select sum(CreditPlus) loyaltyPoints
                                                          from CardCreditPlus cp, cards c
                                                         where cp.card_id = c.card_id
                                                           and c.valid_flag='Y'
                                                           and c.customer_id = @customerId
                                                           and cp.creditPLusType = 'L'
                                                           and ISNULL(cp.PeriodFrom, c.issue_date) >= @fromDate
                                                           and isnull(cp.PeriodFrom, @toDate) <= @toDate";
                                cmdLoyalty.Parameters.AddWithValue("@fromDate", fromDate);
                                cmdLoyalty.Parameters.AddWithValue("@toDate", effectiveToDate);
                                cmdLoyalty.Parameters.AddWithValue("@customerId", card.customer_id);
                                SqlDataAdapter dapl = new SqlDataAdapter(cmdLoyalty);
                                DataTable dtLoyaltyPts = new DataTable();
                                dapl.Fill(dtLoyaltyPts);
                                if (dtLoyaltyPts.Rows.Count > 0)
                                {
                                    int customerPoints = 0;
                                    if (dtLoyaltyPts.Rows[0]["loyaltyPoints"] != null && dtLoyaltyPts.Rows[0]["loyaltyPoints"].ToString() != "")
                                        customerPoints = Convert.ToInt32(dtLoyaltyPts.Rows[0]["loyaltyPoints"]);
                                    if ((retentionPoints - customerPoints) > 0)
                                    {
                                        vipMessage = MessageContainerList.GetMessage(Utilities.ExecutionContext, "You need ") + (retentionPoints - customerPoints).ToString(POSStatic.ParafaitEnv.NUMBER_FORMAT)
                                                    + MessageContainerList.GetMessage(Utilities.ExecutionContext, " points by ") + effectiveToDate.ToString(POSStatic.ParafaitEnv.DATE_FORMAT)
                                                    + MessageContainerList.GetMessage(Utilities.ExecutionContext, " to retain ") + membershipName;
                                    }
                                }
                            }
                        }
                    }
                }
                if (vipMessage != "")
                    alertMessages.Add(vipMessage);
            }

            if (card.vip_customer == 'N')
            {
                if (card.TotalRechargeAmount >= POSStatic.ParafaitEnv.VIP_POS_ALERT_RECHARGE_THRESHOLD)
                {
                    double VIPRechargeDiff = (double)POSStatic.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS - card.TotalRechargeAmount;
                    if (VIPRechargeDiff > 0)
                        message = Utilities.MessageUtils.getMessage(297, VIPRechargeDiff.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT));
                    else if (POSStatic.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0)
                        message = POSStatic.MessageUtils.getMessage(298) + Environment.NewLine + Utilities.MessageUtils.getMessage(299);
                }

                if (!message.StartsWith(Utilities.MessageUtils.getMessage(298)))
                {
                    if (card.credits_played >= POSStatic.ParafaitEnv.VIP_POS_ALERT_SPEND_THRESHOLD)
                    {
                        double VIPSpendDiff = (double)POSStatic.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS - card.credits_played;
                        if (VIPSpendDiff > 0)
                            message += Environment.NewLine + Utilities.MessageUtils.getMessage(300, VIPSpendDiff.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT));
                        else if (POSStatic.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                            message = Environment.NewLine + Utilities.MessageUtils.getMessage(298) + Environment.NewLine + Utilities.MessageUtils.getMessage(299);
                    }
                }
            }

            if (message != "")
                alertMessages.Add(message);

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = @"SELECT isnull(l.Attribute, 'Other') Type
                                ,sum([CreditPlusBalance]) Balance
                                ,[PeriodTo] Period_to
                            FROM [CardCreditPlus] cp left outer join LoyaltyAttributes l on cp.CreditPlusType = l.CreditPlusType
                            where card_id = @CardId
                            and PeriodTo + 1 >= getdate()
                            group by isnull(l.Attribute, 'Other'), [PeriodTo]
                            having sum(CreditPlusBalance) > 0
                            order by PeriodTo";
            cmd.Parameters.AddWithValue("@CardId", card.card_id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                alertMessages.Add(Convert.ToDecimal(dt.Rows[i]["Balance"]).ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT)
                                                + " " + dt.Rows[i]["Type"].ToString()
                                                + " " + Utilities.MessageUtils.getMessage(301, Convert.ToDateTime(dt.Rows[i]["Period_to"]).ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT)));
            }

            cmd.CommandText = @"select da.ScheduleDateTime ScheduleTime, PlayName, BookingId 
                                from AttractionBookings atb, DayAttractionSchedule da, AttractionPlays atp, Trx_Header h 
                                where h.TrxId = atb.TrxId 
                                and h.PrimaryCardId = @CardId 
                                and atb.DayAttractionScheduleId = da.DayAttractionScheduleId
                                and da.ScheduleDateTime > getdate()
                                and (atb.ExpiryDate is null or atb.ExpiryDate > getdate())
                                and atp.AttractionPlayId = da.AttractionPlayId";
            da.SelectCommand = cmd;
            DataTable dtAtt = new DataTable();
            da.Fill(dtAtt);
            foreach (DataRow dr in dtAtt.Rows)
            {
                object o = Utilities.executeScalar(@"declare @s varchar(100)
                                                          set @s = ''
                                                          select @s = @s + ', '+ seatname from FacilitySeats fs, AttractionBookingSeats abs where abs.SeatId = fs.SeatId and abs.BookingId = " + dr["BookingId"].ToString() +
                                                          "select substring(@s, 3, 100)", new SqlParameter[] { });
                string seats = "";
                if (o != null)
                    seats = o.ToString();
                alertMessages.Add(dr["PlayName"].ToString() + " " + Convert.ToDateTime(dr["ScheduleTime"]).ToString("dd-MMM-yyyy h:mm tt") + (seats == "" ? "" : " Seats:" + seats));
            }

            LockerAllocationDTO lockerAllocationDTO = null;
            Locker lockerBl = null;
            if (POSStatic.READ_LOCKER_INFO_ON_CARD_READ)
            {
                ParafaitLockCardHandler locker;
                locker = new ParafaitLockCardHandler(card.ReaderDevice, card.Utilities.ExecutionContext);

                try
                {
                    if (card.card_id > 0)
                    {
                        lockerAllocationDTO = locker.GetLockerAllocationCardDetails(card.card_id);//ref LockerNumber, ref LockerName, ref id, ref panelName, ref LockerAllocationId)
                        if (lockerAllocationDTO.LockerId > -1)
                        {
                            LockerZones lockerZones = new LockerZones(card.Utilities.ExecutionContext);
                            lockerZones.LoadLockerZonebyLockerId(lockerAllocationDTO.LockerId);
                            if (lockerZones.GetLockerZonesDTO != null)
                            {
                                string lockerMake = string.IsNullOrEmpty(lockerZones.GetLockerZonesDTO.LockerMake) ? POSStatic.LOCKER_LOCK_MAKE : lockerZones.GetLockerZonesDTO.LockerMake;
                                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()) && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IS_ONLINE_OPTION_ENABLED").Equals("N"))
                                {
                                    int lockerNumber = -1;
                                    locker = new PassTechLockCardHandler(card.ReaderDevice, card.Utilities.ExecutionContext);
                                    locker.SetAllocation(lockerAllocationDTO);
                                    locker.GetReadCardDetails(ref lockerNumber);
                                }
                            }
                            lockerBl = new Locker(lockerAllocationDTO.LockerId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                if (lockerAllocationDTO != null && lockerAllocationDTO.LockerId > -1)//(LockerAllocationId >= 0)
                {
                    if (lockerBl.getLockerDTO != null && lockerBl.getLockerDTO.LockerId > -1)
                    {
                        message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Locker: ") + lockerBl.getLockerDTO.LockerName;
                        if (!string.IsNullOrEmpty(message))
                            addAlertMessage(message);
                        message = ((lockerBl.getLockerDTO.BatteryStatus != -1) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, "Battery: ")
                            + ((lockerBl.getLockerDTO.BatteryStatus == 0) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, "Normal") : MessageContainerList.GetMessage(Utilities.ExecutionContext, "Low")) : "");
                        if (!string.IsNullOrEmpty(message))
                            addAlertMessage(message);
                        message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Locker Expire On: ") + lockerAllocationDTO.ValidToTime.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT);
                        if (!string.IsNullOrEmpty(message))
                            addAlertMessage(message);
                    }
                }
            }

            //Modified as part of COBRA to add consumption rule in POS on 20-Sep-2019.
            ConsumptionMessage(card);


            lblAlert = labelAlert;
            if (alertTimer.Interval == 100) // first time
            {
                alertTimer = new Timer();
                alertTimer.Interval = 3000;
                alertTimer.Tick += new EventHandler(alertTimer_Tick);
            }

            if (alertMessages.Count > 0)
            {
                alertTimer.Start();
                alertTimer_Tick(alertTimer, null);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method displays Consumption Rule Message in POS.
        /// </summary>
        /// <param name="card">Card</param>
        public static void ConsumptionMessage(Card card)
        {
            log.LogMethodEntry(card);
            //Products products;
            //Category category;
            string productName;
            string categoryName;
            DateTime periodTo;
            DateTime serverTime;
            string message;
            log.Debug("Start - Generate AccountCreditPlus List");
            //AccountBL accountBL = new AccountBL(POSStatic.Utilities.ExecutionContext, card.card_id, true, true, null);
            AccountCreditPlusListBL accountCreditPlusListBL = new AccountCreditPlusListBL(POSStatic.Utilities.ExecutionContext);
            List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> accountCreditPlusSearchParams = new List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>>();
            accountCreditPlusSearchParams.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.ACCOUNT_ID, card.card_id.ToString()));
            accountCreditPlusSearchParams.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.ISACTIVE, "1"));
            accountCreditPlusSearchParams.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.HAS_CONSUMPTION_RULE, "1"));
            List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusListBL.GetAccountCreditPlusDTOList(accountCreditPlusSearchParams, true, true);

            log.Debug("End - Generate AccountCreditPlus List generated");
            if (accountCreditPlusDTOList != null)
            {
                log.Debug("Total Credit Plus records: " + accountCreditPlusDTOList.Count());
                foreach (AccountCreditPlusDTO creditplus in accountCreditPlusDTOList)
                {
                    periodTo = Convert.ToDateTime(creditplus.PeriodTo == null ? DateTime.MinValue : creditplus.PeriodTo);
                    serverTime = POSStatic.Utilities.getServerTime();
                    log.LogVariableState("Expiry Date is", periodTo);
                    if (creditplus.AccountCreditPlusConsumptionDTOList != null)
                    {
                        foreach (AccountCreditPlusConsumptionDTO creditplusConsumption in creditplus.AccountCreditPlusConsumptionDTOList)
                        {
                            if (creditplusConsumption.ConsumptionBalance > 0)
                            {
                                if (periodTo.CompareTo(DateTime.MinValue) != 0 && periodTo.CompareTo(serverTime) < 0)
                                {
                                    continue;
                                }
                                message = creditplusConsumption.ConsumptionBalance.ToString();
                                if (creditplusConsumption.ProductId >= 0)
                                {
                                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(POSStatic.Utilities.ExecutionContext, creditplusConsumption.ProductId);
                                    productName = productsContainerDTO.ProductName;// products.GetProductsDTO.ProductName;
                                    log.LogVariableState("Product Name is", productName);
                                    message = " " + message + " " + productName;

                                }
                                else if (creditplusConsumption.CategoryId >= 0)
                                {
                                    //category = new Category(POSStatic.Utilities.ExecutionContext, creditplusConsumption.CategoryId, null);
                                    CategoryContainerDTO containerDTO= CategoryContainerList.GetCategoryContainerDTO(POSStatic.Utilities.ExecutionContext, creditplusConsumption.CategoryId);
                                    categoryName = containerDTO.Name;//category.GetCategoryDTO.Name;
                                    log.LogVariableState("Category Name is", categoryName);
                                    message = " " + message + " " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2284, categoryName);
                                }
                                if (creditplusConsumption.DiscountedPrice > 0)
                                {
                                    message = message + " " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2281, string.Format("{0:" + POSStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", creditplusConsumption.DiscountedPrice));
                                }
                                else if (creditplusConsumption.DiscountAmount > 0)
                                {
                                    message = message + " " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2282, string.Format("{0:" + POSStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", creditplusConsumption.DiscountAmount));
                                }
                                else if (creditplusConsumption.DiscountPercentage > 0)
                                {
                                    message = message + " " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2283, creditplusConsumption.DiscountPercentage);
                                }

                                if (periodTo.CompareTo(serverTime) >= 0)
                                {
                                    message = message + " " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2285, Convert.ToDateTime(periodTo).ToString(POSStatic.Utilities.ParafaitEnv.DATE_FORMAT));
                                }
                                if (!string.IsNullOrEmpty(message))
                                {
                                    addAlertMessage(message);
                                    message = string.Empty;
                                }

                            }
                            else
                            {
                                log.Debug("Consumption Rule is not applicable");
                            }
                        }
                    }
                }
                accountCreditPlusDTOList = null;
            }

            log.LogMethodExit();
        }

        static void alertTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (alertMessageIndex >= alertMessages.Count)
                alertMessageIndex = 0;

            lblAlert.Text = alertMessages[alertMessageIndex++];
            log.LogMethodExit();
        }

        public static void clearAlerts()
        {
            log.LogMethodEntry();
            alertMessages.Clear();
            alertMessageIndex = 0;
            alertTimer.Stop();
            lblAlert.Text = "";
            log.LogMethodExit();
        }

        public static void addAlertMessage(string message)
        {
            log.LogMethodEntry(message);
            alertMessages.Add(message);
            log.LogMethodExit();
        }

        public static void CheckInCheckOutExternalInterfaces(ref string Message)
        {
            log.LogMethodEntry();
            DataTable dtTables = POSStatic.Utilities.executeDataTable(@"select ft.*, ci.TableId checkedIn 
                                                              from FacilityTables ft 
                                                                left outer join 
                                                                    (select ci.TableId
                                                                    from CheckIns ci, CheckInDetails cd
                                                                    where ci.CheckInId = cd.CheckInId
                                                                    and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) ci
                                                                on ci.TableId = ft.TableId
                                                              where ft.active = 'Y' 
                                                              and isnull(rtrim(ltrim(ft.InterfaceInfo1)), '') != ''");

            foreach (DataRow dr in dtTables.Rows)
            {
                try
                {
                    if (dr["checkedIn"] == DBNull.Value)
                    {
                        FacilityTableDTO facilityTableDTO = new FacilityTables(POSStatic.Utilities.ExecutionContext, Convert.ToInt32(dr["TableId"])).FacilityTableDTO;
                        ExternalInterfaces.SwitchOff(facilityTableDTO);
                    }
                    else
                    {
                        FacilityTableDTO facilityTableDTO = new FacilityTables(POSStatic.Utilities.ExecutionContext, Convert.ToInt32(dr["checkedIn"])).FacilityTableDTO;
                        ExternalInterfaces.SwitchOn(facilityTableDTO);
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Message = "Check In-Out: " + ex.Message;
                }
            }
            log.LogMethodExit();
        }

        static Semnox.Core.GenericUtilities.Logger POSLogger = new Semnox.Core.GenericUtilities.Logger(Semnox.Core.GenericUtilities.Logger.LogType.POS);
        public static void logPOSDebug(string message)
        {
            log.LogMethodEntry(message);
            POSLogger.WriteLog(message);
            log.LogMethodExit();
        }

        public static ReservationDTO GetReservationDTO(ExecutionContext executionContext, int transactionNumber)
        {
            log.LogMethodEntry(transactionNumber, executionContext);
            List<ReservationDTO> ReservationDTOList = null;
            if (transactionNumber > 0)
            {
                ReservationListBL reservationListBL = new ReservationListBL(executionContext);
                List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, transactionNumber.ToString()));
                searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                ReservationDTOList = reservationListBL.GetReservationDTOList(searchParameters);
            }
            ReservationDTO reservationDTO = ReservationDTOList != null && ReservationDTOList.Any() ? ReservationDTOList[0] : null;
            log.LogMethodExit(reservationDTO);
            return reservationDTO;
        }

        public static class ToolTipHelper
        {
            private static readonly Dictionary<string, ToolTip> tooltips = new Dictionary<string, ToolTip>();

            public static ToolTip GetControlToolTip(string controlName)
            {
                log.LogMethodEntry(controlName);
                if (tooltips.ContainsKey(controlName))
                {
                    log.LogMethodExit(tooltips[controlName]);
                    return tooltips[controlName];
                }
                else
                {
                    ToolTip tt = new ToolTip();
                    tooltips.Add(controlName, tt);
                    log.LogMethodExit(tt);
                    return tt;
                }
            }

            public static ToolTip GetControlToolTip(Control control)
            {
                log.LogMethodEntry(control);
                ToolTip ret = GetControlToolTip(control.Name);
                log.LogMethodExit(ret);
                return ret;
            }

            public static ToolTip SetToolTip(Control control, string text)
            {
                log.LogMethodEntry(text);
                ToolTip tt = GetControlToolTip(control);
                tt.SetToolTip(control, text);
                log.LogMethodExit(tt);
                return tt;
            }
        }

        //Begin: Booking related Mofified the query to get all the reservations //
        public static void RefreshReservationsList(DataGridView dgvAll)
        {
            log.LogMethodEntry();
            Utilities Utilities = POSStatic.Utilities;
            //DataTable dt = Utilities.executeDataTable(@"select rr.BookingId, BookingName Booking_name, isnull(c.customer_name + isnull(c.last_name, ''), customerName) Customer, 
            //                                                              p.product_name booking_class, 
            //                                                      Isnull((select top (1) FacilityName 
            //                                                                from  AttractionSchedules asch left join 
            //                                                                                      CheckInFacility cif on asch.FacilityId=cif.FacilityId 
            //                                                               where asch.AttractionScheduleId=rr.AttractionScheduleId ),'Facility') Facility, 
            //                                                         rr.FromDate time_from, rr.ToDate time_to, rr.Quantity, 
            //                                                         rr.ReservationCode reservation_code, rr.Status, 
            //                                                              rr.CardNumber card_number, 
            //                                                              rr.Remarks, " +
            //                                                               "case recur_flag when 'Y' then 'Yes' else 'No' end \"Recurring?\", " +
            //                                                               "case recur_frequency when 'D' then 'Daily' when 'W' then 'Weekly' else '' end as recur_frequency, " +
            //                                                               "recur_end_date, rr.TrxId, th.Status TrxStatus, th.TrxNetAmount  " +
            //                                                         @"from Bookings rr
            //                                                         left outer join CustomerView(@PassPhrase) c
            //                                                         on c.customer_id = rr.CustomerId
            //                                                            left outer join trx_header th
            //                                                            on th.trxId = rr.TrxId
            //                                                            inner join Products p
            //						on p.product_id = rr.BookingProductId               
            //                                                        where (rr.FromDate >= @FromDate or (recur_flag = 'Y' and recur_end_date >= @FromDate))
            //                                                        and (rr.FromDate < @ToDate + 1 or (recur_flag = 'Y' and recur_end_date >= @Todate))
            //                                                        and (ReservationCode = @RCode or @RCode is null)
            //                                                        and (rr.Status = @Status or @Status = 'ALL')
            //                                                        and (rr.Status not in ('SYSTEMABANDONED'))
            //                                                        and (CustomerName like @customer or customer_name + isnull(c.last_name, '') like @customer or @customer is null)
            //                                                        and (CardNumber like @cardNumber or @cardNumber is null)
            //                                                        and (rr.BookingProductId = @BookingProductId or @BookingProductId = -1)
            //                                                        order by rr.FromDate desc",
            //                                                               new SqlParameter("@FromDate", DateTime.Now.Date),
            //                                                               new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetDecryptedParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")),
            //                                                               new SqlParameter("@ToDate", DateTime.Now.Date.AddDays(365)),
            //                                                               new SqlParameter("@RCode", DBNull.Value),
            //                                                               new SqlParameter("@Status", "ALL"),
            //                                                               new SqlParameter("@customer", DBNull.Value),
            //                                                               new SqlParameter("@cardNumber", DBNull.Value),
            //                                                               new SqlParameter("@BookingProductId", -1));

            ReservationListBL reservationListBL = new ReservationListBL(Utilities.ExecutionContext);
            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE, Utilities.getServerTime().Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_TO_DATE, Utilities.getServerTime().Date.AddDays(365).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN, "'SYSTEMABANDONED'"));
            searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParameters);
            //if (reservationDTOList != null && reservationDTOList.Any())
            //{
            //    for (int i = 0; i < reservationDTOList.Count; i++)
            //    {
            //        ReservationBL reservationBL = new ReservationBL(Utilities.ExecutionContext, Utilities, reservationDTOList[i]);
            //        reservationDTOList[i].FacilityMapName = reservationBL.GetReservationFacilities();
            //    } 
            //}

            dgvAll.DataSource = reservationDTOList;

            Utilities.setupDataGridProperties(ref dgvAll);
            dgvAll.Columns["bookingClassId"].Visible = false;
            dgvAll.Columns["cardId"].Visible = false;
            dgvAll.Columns["customerId"].Visible = false;
            dgvAll.Columns["expiryTime"].Visible = false;
            dgvAll.Columns["channel"].Visible = false;
            dgvAll.Columns["createdBy"].Visible = false;
            dgvAll.Columns["creationDate"].Visible = false;
            dgvAll.Columns["lastUpdatedBy"].Visible = false;
            dgvAll.Columns["lastUpdateDate"].Visible = false;
            dgvAll.Columns["guid"].Visible = false;
            dgvAll.Columns["synchStatus"].Visible = false;
            dgvAll.Columns["siteId"].Visible = false;
            dgvAll.Columns["contactNo"].Visible = false;
            dgvAll.Columns["alternateContactNo"].Visible = false;
            dgvAll.Columns["email"].Visible = false;
            dgvAll.Columns["isEmailSent"].Visible = false;
            dgvAll.Columns["age"].Visible = false;
            dgvAll.Columns["gender"].Visible = false;
            dgvAll.Columns["postalAddress"].Visible = false;
            dgvAll.Columns["bookingProductId"].Visible = false;
            dgvAll.Columns["attractionScheduleId"].Visible = false;
            dgvAll.Columns["extraGuests"].Visible = false;
            dgvAll.Columns["masterEntityId"].Visible = false;
            dgvAll.Columns["trxNumber"].Visible = false;
            dgvAll.Columns["facilityMapId"].Visible = false;
            dgvAll.Columns["facilityMapName"].Visible = false;
            //dgvAll.Columns["eventHostId"].Visible = false;
            //dgvAll.Columns["eventHostName"].Visible = false;
            //dgvAll.Columns["checkListTaskGroupId"].Visible = false;
            //dgvAll.Columns["checkListTaskGroupName"].Visible = false;

            dgvAll.Columns["bookingId"].DisplayIndex = 0;
            dgvAll.Columns["bookingName"].DisplayIndex = 1;
            dgvAll.Columns["customerName"].DisplayIndex = 2;
            dgvAll.Columns["bookingProductName"].DisplayIndex = 3;
            dgvAll.Columns["facilityName"].DisplayIndex = 4;
            dgvAll.Columns["fromDate"].DisplayIndex = 5;
            dgvAll.Columns["toDate"].DisplayIndex = 6;
            dgvAll.Columns["Quantity"].DisplayIndex = 7;
            dgvAll.Columns["reservationCode"].DisplayIndex = 8;
            dgvAll.Columns["status"].DisplayIndex = 9;
            dgvAll.Columns["cardNumber"].DisplayIndex = 10;
            dgvAll.Columns["Remarks"].DisplayIndex = 11;
            dgvAll.Columns["RecurFlag"].DisplayIndex = 12;
            dgvAll.Columns["RecurFrequency"].DisplayIndex = 13;
            dgvAll.Columns["RecurEndDate"].DisplayIndex = 14;
            dgvAll.Columns["trxId"].DisplayIndex = 15;
            dgvAll.Columns["trxStatus"].DisplayIndex = 16;
            dgvAll.Columns["trxNetAmount"].DisplayIndex = 17;

            dgvAll.Columns["RecurEndDate"].DefaultCellStyle = Utilities.gridViewDateCellStyle();
            dgvAll.Columns["fromDate"].DefaultCellStyle =
            dgvAll.Columns["toDate"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();

            dgvAll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            log.LogMethodExit();
        }
        //End: Booking related //
        public static Color getColor(string inColor)
        {
            log.LogMethodEntry(inColor);
            try
            {
                Color color;
                if (inColor.Contains(",")) // RGB
                {
                    int p = inColor.IndexOf(',');
                    int r = Convert.ToInt32(inColor.Substring(0, p));
                    inColor = inColor.Substring(p + 1).Trim();

                    p = inColor.IndexOf(',');
                    int g = Convert.ToInt32(inColor.Substring(0, p));
                    inColor = inColor.Substring(p + 1).Trim();

                    int b = Convert.ToInt32(inColor);

                    color = Color.FromArgb(r, g, b);
                }
                else
                {
                    color = Color.FromName(inColor);
                    if (color.ToArgb() == 0)
                        color = Color.FromArgb(Int32.Parse(inColor, System.Globalization.NumberStyles.HexNumber));

                    if (color.ToArgb() == 0)
                        return Color.FromArgb(0);
                }
                log.LogMethodExit(color);
                return color;
            }
            catch
            {
                log.Error("Exception: returning Color.FromArgb(0)");
                return Color.FromArgb(0);
            }
        }

        public static Bitmap ChangeColor(Bitmap scrBitmap, Color newColor)
        {
            log.LogMethodEntry(newColor);
            Color actulaColor;
            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actulaColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actulaColor.A > 150)
                        newBitmap.SetPixel(i, j, newColor);
                    else
                        newBitmap.SetPixel(i, j, actulaColor);
                }
            }
            log.LogMethodExit(newBitmap);
            return newBitmap;
        }

        public static Bitmap pressedButtonImage(Bitmap scrBitmap)
        {
            log.LogMethodEntry(scrBitmap);
            Bitmap ret = Properties.Resources.ProductPressed;
            log.LogMethodExit(ret);
            return ret;
        }

        public static DialogResult ParafaitMessageBox(string Message)
        {
            log.LogMethodEntry(Message);
            frmParafaitMessageBox fmsg = new frmParafaitMessageBox(Message, "", MessageBoxButtons.OK);
            DialogResult ret = fmsg.ShowDialog();
            log.LogMethodExit(ret);
            return ret;
        }

        public static DialogResult ParafaitMessageBox(string Message, string Title)
        {
            log.LogMethodEntry(Message, Title);
            frmParafaitMessageBox fmsg = new frmParafaitMessageBox(Message, Title, MessageBoxButtons.OK);
            DialogResult ret = fmsg.ShowDialog();
            log.LogMethodExit(ret);
            return ret;
        }

        public static DialogResult ParafaitMessageBox(string Message, string Title, MessageBoxButtons msgboxButtons = MessageBoxButtons.YesNo)
        {
            log.LogMethodEntry(Message, Title);
            frmParafaitMessageBox fmsg = new frmParafaitMessageBox(Message, Title, msgboxButtons);
            DialogResult ret = fmsg.ShowDialog();
            log.LogMethodExit(ret);
            return ret;
        }

        public static DialogResult ParafaitMessageBox(string Message, string Title, MessageBoxButtons msgboxButtons = MessageBoxButtons.YesNo, string yesBtnText = "", string noBtnText = "")
        {
            log.LogMethodEntry(Message, Title, yesBtnText, noBtnText);
            frmParafaitMessageBox fmsg = new frmParafaitMessageBox(Message, Title, msgboxButtons, yesBtnText, noBtnText);
            DialogResult ret = fmsg.ShowDialog();
            log.LogMethodExit(ret);
            return ret;
        }

        public static DialogResult ParafaitMessageBox(string Message, string Title, MessageBoxButtons msgboxButtons = MessageBoxButtons.YesNo, MessageBoxIcon msgboxIcon = MessageBoxIcon.None)
        {
            log.LogMethodEntry(Message, Title);
            frmParafaitMessageBox fmsg = new frmParafaitMessageBox(Message, Title, msgboxButtons);
            DialogResult ret = fmsg.ShowDialog();
            log.LogMethodExit(ret);
            return ret;
        }

        public static string getDateMonthFormat()
        {
            log.LogMethodEntry();
            string dateFormat = POSStatic.Utilities.getDateFormat();
            if (POSStatic.IGNORE_CUSTOMER_BIRTH_YEAR)
            {
                if (dateFormat.StartsWith("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    dateFormat = dateFormat.TrimStart('y', 'Y');
                    dateFormat = dateFormat.Substring(1);
                }
                else
                {
                    int pos = dateFormat.IndexOf("Y", StringComparison.CurrentCultureIgnoreCase);
                    if (pos > 0)
                        dateFormat = dateFormat.Substring(0, pos - 1);
                }
            }
            log.LogMethodExit(dateFormat);
            return dateFormat;
        }

        internal static bool refreshCardFromHQ(ref Card CurrentCard, ref string message)
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodExit();
                return POSStatic.CardUtilities.getCardFromHQ(CardRoamingRemotingClient, ref CurrentCard, ref message);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("fault"))
                {
                    message = POSStatic.MessageUtils.getMessage(216);
                    try
                    {
                        CardRoamingRemotingClient = new RemotingClient();
                    }
                    catch (Exception exe)
                    {
                        POSUtils.ParafaitMessageBox(exe.Message);
                        message = POSStatic.MessageUtils.getMessage(217);
                    }
                }
                else
                    message = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "On-Demand Roaming: ") + ex.Message;

                log.LogMethodExit(false);
                return false;
            }
        }

        internal static bool refreshCardFromHQForced(ref Card CurrentCard, ref string message)
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodExit();
                return POSStatic.CardUtilities.getCardFromHQForced(CardRoamingRemotingClient, ref CurrentCard, ref message);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("fault"))
                {
                    message = POSStatic.MessageUtils.getMessage(216);
                    try
                    {
                        CardRoamingRemotingClient = new RemotingClient();
                    }
                    catch (Exception exe)
                    {
                        POSUtils.ParafaitMessageBox(exe.Message);
                        message = POSStatic.MessageUtils.getMessage(217);
                    }
                }
                else
                    message = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "On-Demand Roaming: ") + ex.Message;

                log.LogMethodExit(false);
                return false;
            }
        }

        public static void IncrementDGVPurchasesPageNo()
        {
            log.LogMethodEntry(dgvPurchasesPageNo);
            dgvPurchasesPageNo++;
            log.LogMethodExit(dgvPurchasesPageNo);
        }

        public static void IncrementDGVGamePlayPageNo()
        {
            log.LogMethodEntry(dgvGamePlayPageNo);
            dgvGamePlayPageNo++;
            log.LogMethodExit(dgvGamePlayPageNo);
        }

        public static DataTable LoadNextPageForDGVPurchase()
        {
            log.LogMethodEntry();
            if (dtCardPurchaseLoaded.Rows.Count < dtCardPurchaseDetails.Rows.Count)
            {
                IEnumerable<DataRow> nextSetOfRows = dtCardPurchaseDetails.AsEnumerable().Skip((dgvPurchasesPageNo - 1) * dgvPurchasesPageSize).Take(dgvPurchasesPageSize);
                if (nextSetOfRows.Any())
                {
                    log.LogVariableState("Do  dtCardPurchaseLoaded.Merge", nextSetOfRows.Count());
                    dtCardPurchaseLoaded.Merge(nextSetOfRows.CopyToDataTable());
                    IncrementDGVPurchasesPageNo();
                }
            }
            log.LogMethodExit(dtCardPurchaseLoaded);
            return dtCardPurchaseLoaded;
        }

        public static DataTable LoadNextPageForDGVGamePlay()
        {
            log.LogMethodEntry();
            if (dtCardGamePlayLoaded.Rows.Count < dtCardGamePlayDetails.Rows.Count)
            {
                IEnumerable<DataRow> nextSetOfRows = dtCardGamePlayDetails.AsEnumerable().Skip((dgvGamePlayPageNo - 1) * dgvGamePlayPageSize).Take(dgvGamePlayPageSize);
                if (nextSetOfRows.Any())
                {
                    log.LogVariableState("Do  dtCardGamePlayLoaded.Merge", nextSetOfRows.Count());
                    dtCardGamePlayLoaded.Merge(nextSetOfRows.CopyToDataTable());
                    IncrementDGVGamePlayPageNo();
                }
            }
            log.LogMethodExit(dtCardGamePlayLoaded);
            return dtCardGamePlayLoaded;
        }

        public static DataGridView ResetDGVPurchaseRowPrinterImage(DataGridView dataGridViewPurchases, int startFromRow)
        {
            log.LogMethodEntry(dataGridViewPurchases, startFromRow);
            int rowNumber = 0;
            Image emptyImage = new Bitmap(1, 1);
            foreach (DataGridViewRow dr in dataGridViewPurchases.Rows)
            {
                if (rowNumber < startFromRow)
                {
                    rowNumber++;
                    continue;
                }
                if (dr.Cells["ActivityType"].Value.ToString() != "TRANSACTION"
                    && dr.Cells["ActivityType"].Value.ToString() != "PAYMENT")
                {
                    (dr.Cells["dcBtnCardActivityTrxPrint"]).Value = emptyImage;
                }
            }
            log.LogMethodExit(dataGridViewPurchases);
            return dataGridViewPurchases;
        }

        public static DataGridView ResetDGVGamePlayRowBackColor(DataGridView dataGridViewGamePlay, int startFromRow)
        {
            log.LogMethodEntry(dataGridViewGamePlay, startFromRow);
            int rowNumber = 0;
            foreach (DataGridViewRow dr in dataGridViewGamePlay.Rows)
            {
                if (rowNumber < startFromRow)
                {
                    rowNumber++;
                    continue;
                }
                if (dr.Cells["task_id"].Value.Equals(DBNull.Value) == false) // refunded gameplay
                {
                    dr.DefaultCellStyle.BackColor = Color.Tomato;
                    dr.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
                }
            }
            log.LogMethodExit(dataGridViewGamePlay);
            return dataGridViewGamePlay;
        }

        public static int GetCardPurchaseDetailsRowCount()
        {
            log.LogMethodEntry();
            log.LogMethodExit(dtCardPurchaseDetails.Rows.Count);
            return dtCardPurchaseDetails.Rows.Count;
        }

        public static int GetCardGamePlayDetailsRowCount()
        {
            log.LogMethodEntry();
            log.LogMethodExit(dtCardGamePlayDetails.Rows.Count);
            return dtCardGamePlayDetails.Rows.Count;
        }

        public static DataTable FullLoadForDGVPurchase()
        {
            log.LogMethodEntry();
            if (dtCardPurchaseLoaded.Rows.Count < dtCardPurchaseDetails.Rows.Count)
            {
                dtCardPurchaseLoaded = dtCardPurchaseDetails;
                dgvPurchasesPageNo = Convert.ToInt32(dtCardPurchaseDetails.Rows.Count / dgvPurchasesPageSize);
            }
            log.LogMethodExit(dtCardPurchaseLoaded);
            return dtCardPurchaseLoaded;
        }

        public static DataTable FullLoadForDGVGamePlay()
        {
            log.LogMethodEntry();
            if (dtCardGamePlayLoaded.Rows.Count < dtCardGamePlayDetails.Rows.Count)
            {
                dtCardGamePlayLoaded = dtCardGamePlayDetails;
                dgvGamePlayPageNo = Convert.ToInt32(dtCardGamePlayDetails.Rows.Count / dgvGamePlayPageSize);
            }
            log.LogMethodExit(dtCardGamePlayLoaded);
            return dtCardGamePlayLoaded;
        }

        public static void AttachFormEvents()
        {
            log.LogMethodEntry();
            for (int i = Application.OpenForms.Count - 1; i > 0; i--)
            {
                if (Application.OpenForms[i].Name != "POS" && Application.OpenForms[i].Name != "UIActionStatusDialog")
                {
                    AttachEvents(Application.OpenForms[i]);
                }
            }
            log.LogMethodExit();
        }
        private static void AttachEvents(Form form)
        {
            log.LogMethodEntry(form.Name);
            try
            {
                DoTimerReset();

                EventHandlerList events = (EventHandlerList)typeof(Component)
                .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(form, null);

                object key = typeof(Form)
                    .GetField("EVENT_FORMCLOSED", BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null);

                Delegate handlers = events[key];

                object secondKey = typeof(Control)
                    .GetField("EventKeyPress", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

                Delegate handlersKeyPresss = events[secondKey];

                object keyDeactivate = typeof(Form)
                    .GetField("EVENT_DEACTIVATE", BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null);

                Delegate handlersDeactivate = events[keyDeactivate];

                if (handlers != null)
                {
                    bool eventAttached = false;
                    foreach (Delegate handler in handlers.GetInvocationList())
                    {
                        MethodInfo method = handler.Method;
                        if (method.Name == "CallTimerResetOnFormClose")
                        {
                            eventAttached = true;
                            break;
                        }
                    }

                    if (eventAttached == false)
                    {
                        form.FormClosed += CallTimerResetOnFormClose;
                    }
                }
                if (handlersKeyPresss != null)
                {
                    bool eventAttached = false;
                    foreach (Delegate handlersKeyPress in handlersKeyPresss.GetInvocationList())
                    {
                        MethodInfo method = handlersKeyPress.Method;
                        if (method.Name == "CallTimerResetOnKeyPress")
                        {
                            eventAttached = true;
                            break;
                        }
                    }

                    if (eventAttached == false)
                    {
                        form.KeyPreview = true;
                        form.KeyPress += CallTimerResetOnKeyPress;
                    }
                }
                if (handlersDeactivate != null)
                {
                    bool eventAttached = false;
                    foreach (Delegate handlerDeactivate in handlersDeactivate.GetInvocationList())
                    {
                        MethodInfo method = handlerDeactivate.Method;
                        if (method.Name == "CallOnFormDeactivate")
                        {
                            eventAttached = true;
                            break;
                        }
                    }

                    if (eventAttached == false)
                    {
                        form.Deactivate += CallOnFormDeactivate;
                    }
                }

                if (handlers == null)
                {
                    form.FormClosed += CallTimerResetOnFormClose;
                }
                if (handlersKeyPresss == null)
                {
                    form.KeyPreview = true;
                    form.KeyPress += CallTimerResetOnKeyPress;
                }
                if (handlersDeactivate == null)
                {
                    form.Deactivate += CallOnFormDeactivate;
                } 
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private static List<string> formWithActivityTimeOutLogic = new List<string>() { "frmReservationUI", "frmReservationSearch","frmBasicReservationInputs","formLogin","frmParafaitMessageBox"};
        static void CallTimerResetOnFormClose(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Form frmObject = sender as Form;
            if (frmObject == null || formWithActivityTimeOutLogic.Exists(f => f == frmObject.Name) == false)
            {
                DoTimerReset();
            }
            log.LogMethodExit();
        }

        public static void CallTimerResetOnKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            DoTimerReset();
            log.LogMethodExit();
        }

        public static void DoTimerReset()
        {
            log.LogMethodEntry();
            try
            {
                if (Application.OpenForms != null && Application.OpenForms[0] as POS != null)
                {
                    (Application.OpenForms[0] as POS).lastTrxActivityTime = DateTime.Now;// ServerDateTime.Now;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public static void DoForcedTimeout()
        {
            log.LogMethodEntry();
            if (Application.OpenForms != null && Application.OpenForms[0] as POS != null)
            {
                (Application.OpenForms[0] as POS).lastTrxActivityTime = DateTime.Now.AddDays(-1); 
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPOSLastTrxActivityTime
        /// </summary>
        /// <returns></returns>
        public static DateTime GetPOSLastTrxActivityTime()
        {
            log.LogMethodEntry();
            DateTime lastTrxActivityTimeValue = DateTime.Now.AddDays(-1);
            if (Application.OpenForms != null && Application.OpenForms[0] as POS != null)
            {
                lastTrxActivityTimeValue = (Application.OpenForms[0] as POS).lastTrxActivityTime;
            }
            log.LogMethodExit(lastTrxActivityTimeValue);
            return lastTrxActivityTimeValue;
        }
        public static void CallPerformActivityTimeOutChecks(int inactivityPeriodSec)
        {
            log.LogMethodEntry(inactivityPeriodSec);
            try
            {
                if (Application.OpenForms != null && Application.OpenForms[0] as POS != null)
                {
                    (Application.OpenForms[0] as POS).PerformActivityTimeOutChecks(inactivityPeriodSec);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public static decimal GetVariableDiscountAmount()
        {
            log.LogMethodEntry();
            decimal variableAmount = 0;
            string discAmount;
            GenericDataEntry variablediscountAmount = new GenericDataEntry(1);
            variablediscountAmount.DataEntryObjects[0].dataType = "Number";
            variablediscountAmount.Text = POSStatic.Utilities.MessageUtils.getMessage("Enter the amount");
            variablediscountAmount.DataEntryObjects[0].mandatory = true;
            variablediscountAmount.DataEntryObjects[0].label = POSStatic.Utilities.MessageUtils.getMessage("Variable Discount");
            if (variablediscountAmount.ShowDialog() == DialogResult.OK)
            {
                discAmount = variablediscountAmount.DataEntryObjects[0].data;
                if (string.IsNullOrWhiteSpace(discAmount) == false)
                {
                    decimal.TryParse(discAmount, out variableAmount);
                }
            }
            if (variableAmount <= 0)
            {
                log.LogMethodExit("Variable amount mandatory for this discount");
                throw new VariableDiscountException("Variable amount mandatory for this discount.");
            }
            log.LogMethodExit(variableAmount);
            return variableAmount;
        }

        static void CallOnFormDeactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.AttachFormEvents();
                POSUtils.DisableFormMinimizeOption();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public static void DisableFormMinimizeOption()
        {
            log.LogMethodEntry();
            try
            { 
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    if (Application.OpenForms[i].Name != "POS" && Application.OpenForms[i].Name != "UIActionStatusDialog")
                    {
                        log.Info((Application.OpenForms[i] != null ? Application.OpenForms[i].Name : "Blank Form"));
                        if (Application.OpenForms[i].MinimizeBox == true)
                        {
                            Application.OpenForms[i].MinimizeBox = false;
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
          
        internal static int GetIndexOfOpenForm(string formName)
        {
            log.LogMethodEntry(formName);
            int formIndex = -1;
            for (int i = Application.OpenForms.Count - 1; i > 0; i--)
            {
                if (Application.OpenForms[i].Name == formName)
                {
                    formIndex = i;
                    break;
                }
            }
            log.LogMethodExit(formIndex);
            return formIndex;
        }

        internal static void ForceCloseCurrentScreen(Form frmObject)
        {
            log.LogMethodEntry((frmObject != null? frmObject.Name: string.Empty));
            int formIndex = POSUtils.GetIndexOfOpenForm(frmObject.Name);
            if (Application.OpenForms.Count > 1)
            {
                for (int i = Application.OpenForms.Count - 1; i > formIndex; i--)
                {
                    Application.OpenForms[i].Close();
                    System.Threading.Thread.Sleep(20);
                }
                frmObject.Close();
                POSUtils.DoForcedTimeout();
            }
            log.LogMethodExit();
        }

        internal static void SetLastActivityDateTime()
        {
            POSUtils.DoTimerReset(); 
        }
        internal static bool MessageBoxIsOpen()
        { 
            bool msgBoxIsOpen = false;
            string msgFormName = "frmParafaitMessageBox";
            int formIndex = POSUtils.GetIndexOfOpenForm(msgFormName); 
            if (formIndex > -1)
            {
                msgBoxIsOpen = true;
            }
            return msgBoxIsOpen;
        }
    }


}

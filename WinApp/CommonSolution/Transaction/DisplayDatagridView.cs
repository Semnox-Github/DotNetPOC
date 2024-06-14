using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
//using POSCore;
//using Semnox.Parafait.DiscountCoupons;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    public static class DisplayDatagridView
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static DataGridViewCellStyle getSpecialGridRowFormat(DataGridViewCellStyle refDGV, float FontIncrement = 1.0f)
        {
            log.LogMethodEntry(refDGV, FontIncrement);
            DataGridViewCellStyle dgv;
            if (refDGV != null)
                dgv = new DataGridViewCellStyle(refDGV);
            else
                dgv = new DataGridViewCellStyle();
            dgv.BackColor =
            dgv.SelectionBackColor = Color.Gray;
            dgv.ForeColor =
            dgv.SelectionForeColor = Color.White;
            dgv.Font = new Font(refDGV.Font.FontFamily, refDGV.Font.Size + FontIncrement, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)(0));

            log.LogMethodExit(dgv);
            return dgv;
        }

        private static int AddCardDetailsLine(Transaction Trx, DataGridView dataGridViewTransaction, Utilities utilities, int rowcount)
        {
            log.LogMethodEntry(Trx, dataGridViewTransaction, utilities, rowcount);
            for (int i = 0; i < Trx.TrxLines.Count; i++) // display card lines
            {
                if (Trx.TrxLines[i].LineValid && !Trx.TrxLines[i].LineProcessed
                    && Trx.TrxLines[i].CardNumber != null)
                {
                    dataGridViewTransaction.Rows.Add();

                    string cardnumber = Trx.TrxLines[i].CardNumber;
                    if (cardnumber != null)
                    {
                        dataGridViewTransaction.Rows[rowcount].Cells["Card_Number"].Value = cardnumber;
                        dataGridViewTransaction.Rows[rowcount].Cells["Product_Type"].Value = utilities.MessageUtils.getMessage("Card Sale");
                        dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = cardnumber;
                        dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = i;
                        dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = "Card";

                        dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle);

                        dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

                        rowcount++;

                        for (int j = i; j < Trx.TrxLines.Count; j++)
                        {
                            if (cardnumber == Trx.TrxLines[j].CardNumber && Trx.TrxLines[j].LineValid && Trx.TrxLines[j].LineProcessed == false)
                            {
                                dataGridViewTransaction.Rows.Add();
                                if (Trx.TrxLines[j].ProductTypeCode == "ATTRACTION")
                                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = Trx.TrxLines[j].ProductName + (string.IsNullOrEmpty(Trx.TrxLines[j].AttractionDetails) ? "" : "-" + Trx.TrxLines[j].AttractionDetails) + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);
                                else if (Trx.TrxLines[j].ProductTypeCode == "LOCKER")
                                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = Trx.TrxLines[j].ProductName + "-Locker:" + Trx.TrxLines[j].LockerName + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);
                                else
                                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = Trx.TrxLines[j].ProductName + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);

                                dataGridViewTransaction.Rows[rowcount].Cells["Quantity"].Value = Trx.TrxLines[j].quantity;
                                dataGridViewTransaction.Rows[rowcount].Cells["Price"].Value = Trx.TrxLines[j].Price;
                                dataGridViewTransaction.Rows[rowcount].Cells["Remarks"].Value = Trx.TrxLines[j].Remarks;
                                dataGridViewTransaction.Rows[rowcount].Cells["AttractionDetails"].Value = Trx.TrxLines[j].AttractionDetails;
                                dataGridViewTransaction.Rows[rowcount].Cells["TrxProfileId"].Value = Trx.TrxLines[j].TrxProfileId;

                                if (Trx.TrxLines[j].Price != Trx.TrxLines[j].OriginalPrice)
                                {
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.BackColor = Color.LightGreen;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.SelectionBackColor = Color.LightGreen;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].ToolTipText = utilities.MessageUtils.getMessage("Promo Price") + " [" + Trx.TrxLines[j].OriginalPrice.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT) + "]";
                                }

                                if (utilities.ParafaitEnv.specialPricingId != -1)
                                {
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.BackColor = Color.OrangeRed;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.SelectionBackColor = Color.OrangeRed;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].ToolTipText = utilities.MessageUtils.getMessage("Special Price");
                                }

                                if (Trx.TrxLines[j].UserPrice)
                                {
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.BackColor = Color.Goldenrod;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.SelectionBackColor = Color.Goldenrod;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].ToolTipText = utilities.MessageUtils.getMessage("User Price");
                                }

                                dataGridViewTransaction.Rows[rowcount].Cells["Tax"].Value = Trx.TrxLines[j].tax_amount;
                                dataGridViewTransaction.Rows[rowcount].Cells["TaxName"].Value = Trx.TrxLines[j].taxName;
                                dataGridViewTransaction.Rows[rowcount].Cells["Line_Amount"].Value = Trx.TrxLines[j].LineAmount;
                                dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = j;
                                dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = Trx.TrxLines[j].ProductTypeCode;
                                dataGridViewTransaction.Rows[rowcount].Cells["Card_Number"].Value = cardnumber;
                                rowcount++;
                                Trx.TrxLines[j].LineProcessed = true;
                            }
                        }
                    }
                }
            }

            log.LogMethodExit(rowcount);
            return rowcount;
        }

        private static int AddNonCardLines(Transaction Trx, DataGridView dataGridViewTransaction, Utilities Utilities, int rowcount)
        {
            log.LogMethodEntry();
            for (int i = 0; i < Trx.TrxLines.Count; i++) // display non-card trx lines
            {
                if (Trx.TrxLines[i].LineValid && !Trx.TrxLines[i].LineProcessed
                    && Trx.TrxLines[i].CardNumber == null)
                {
                    dataGridViewTransaction.Rows.Add();
                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Type"].Value = Utilities.MessageUtils.getMessage("Item Sale");
                    dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = i;
                    dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = Trx.TrxLines[i].ProductTypeCode;

                    dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle);
                    dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

                    rowcount++;

                    for (int j = i; j < Trx.TrxLines.Count; j++)
                    {
                        if (Trx.TrxLines[j].CardNumber == null && Trx.TrxLines[j].LineValid && !Trx.TrxLines[j].LineProcessed)
                        {
                            //Modification on 17-Nov-2016, for printing parent order details
                            if (Trx.TrxLines[j].ParentLine != null && Trx.TrxLines.Equals(Trx.TrxLines[j].ParentLine))
                            {
                                var index = Trx.TrxLines.FindIndex(a => a == Trx.TrxLines[j].ParentLine);

                                if (index > -1 && index != j)
                                {
                                    displayNonCardLine(dataGridViewTransaction, Trx, index, ref rowcount);
                                }
                            }
                            else
                            {
                                displayNonCardLine(dataGridViewTransaction, Trx, j, ref rowcount);
                            }
                            //end modification
                        }
                    }
                }
            }
            log.LogMethodExit(rowcount);
            return rowcount;
        }

        public static void RefreshTrxDataGrid(Transaction Trx, DataGridView dataGridViewTransaction, Utilities utilities)
        {
            log.LogMethodEntry(Trx, dataGridViewTransaction);

            Utilities Utilities = (Trx == null) ? utilities : Trx.Utilities;
            ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;

            dataGridViewTransaction.Rows.Clear();
            if (Trx == null)
            {
                log.LogMethodExit(null);
                return;
            }

            for (int i = 0; i < Trx.TrxLines.Count; i++)
            {
                if (Trx.TrxLines[i].ProductTypeCode == "LOYALTY")
                    Trx.TrxLines[i].LineProcessed = true;
                else
                    Trx.TrxLines[i].LineProcessed = false;
            }

            dataGridViewTransaction.Columns["Tax"].DefaultCellStyle.Format =
            dataGridViewTransaction.Columns["Price"].DefaultCellStyle.Format =
            dataGridViewTransaction.Columns["Line_Amount"].DefaultCellStyle.Format =
            dataGridViewTransaction.Columns["Price"].DefaultCellStyle.Format = ParafaitEnv.AMOUNT_FORMAT;

            int rowcount = 0;
            for (int i = 0; i < Trx.TrxLines.Count; i++) // display card lines
            {
                if (Trx.TrxLines[i].LineValid && !Trx.TrxLines[i].LineProcessed
                    && Trx.TrxLines[i].CardNumber != null
                    &&  Trx.TrxLines[i].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[i].ProductTypeCode != ProductTypeValues.GRATUITY)
                {
                    dataGridViewTransaction.Rows.Add();

                    string cardnumber = Trx.TrxLines[i].CardNumber;
                    if (cardnumber != null)
                    {
                        dataGridViewTransaction.Rows[rowcount].Cells["Card_Number"].Value = cardnumber;
                        dataGridViewTransaction.Rows[rowcount].Cells["Product_Type"].Value = Utilities.MessageUtils.getMessage("Card Sale");
                        dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = cardnumber;
                        dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = i;
                        dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = "Card";

                        dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle);

                        dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

                        rowcount++;

                        for (int j = i; j < Trx.TrxLines.Count; j++)
                        {
                            if (cardnumber == Trx.TrxLines[j].CardNumber && Trx.TrxLines[j].LineValid && Trx.TrxLines[j].LineProcessed == false
                                && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
                            {
                                dataGridViewTransaction.Rows.Add();
                                string highlight = "";
                                if (Trx.TrxLines[j].TransactionDiscountsDTOList != null)
                                {
                                    foreach (var transactionDiscountsDTO in Trx.TrxLines[j].TransactionDiscountsDTOList)
                                    {
                                        if (Trx.DiscountsSummaryDTODictionary != null &&
                                           Trx.DiscountsSummaryDTODictionary.ContainsKey(transactionDiscountsDTO.DiscountId))
                                        {
                                            highlight += Trx.DiscountsSummaryDTODictionary[transactionDiscountsDTO.DiscountId].DisplayChar;
                                        }
                                    }
                                }
                                if(string.IsNullOrWhiteSpace(highlight) == false)
                                {
                                    highlight = highlight + " ";
                                }
                                if (Trx.TrxLines[j].ProductTypeCode == "ATTRACTION")
                                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = highlight + Trx.TrxLines[j].ProductName + (string.IsNullOrEmpty(Trx.TrxLines[j].AttractionDetails) ? "" : "-" + Trx.TrxLines[j].AttractionDetails) + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);
                                else if (Trx.TrxLines[j].ProductTypeCode == "LOCKER")
                                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = highlight + Trx.TrxLines[j].ProductName + "-Locker:" + Trx.TrxLines[j].LockerName + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);
                                else
                                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = highlight + Trx.TrxLines[j].ProductName + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);

                                dataGridViewTransaction.Rows[rowcount].Cells["Quantity"].Value = Trx.TrxLines[j].quantity;
                                dataGridViewTransaction.Rows[rowcount].Cells["Price"].Value = Trx.TrxLines[j].Price;
                                dataGridViewTransaction.Rows[rowcount].Cells["Remarks"].Value = Trx.TrxLines[j].Remarks;
                                dataGridViewTransaction.Rows[rowcount].Cells["AttractionDetails"].Value = Trx.TrxLines[j].AttractionDetails;
                                dataGridViewTransaction.Rows[rowcount].Cells["TrxProfileId"].Value = Trx.TrxLines[j].TrxProfileId;

                                if (Trx.TrxLines[j].Price != Trx.TrxLines[j].OriginalPrice)
                                {
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.BackColor = Color.LightGreen;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.SelectionBackColor = Color.LightGreen;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].ToolTipText = Utilities.MessageUtils.getMessage("Promo Price") + " [" + Trx.TrxLines[j].OriginalPrice.ToString(ParafaitEnv.AMOUNT_FORMAT) + "]";
                                }

                                if (Utilities.ParafaitEnv.specialPricingId != -1)
                                {
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.BackColor = Color.OrangeRed;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.SelectionBackColor = Color.OrangeRed;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].ToolTipText = Utilities.MessageUtils.getMessage("Special Price");
                                }

                                if (Trx.TrxLines[j].UserPrice)
                                {
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.BackColor = Color.Goldenrod;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].Style.SelectionBackColor = Color.Goldenrod;
                                    dataGridViewTransaction.Rows[rowcount].Cells["Price"].ToolTipText = Utilities.MessageUtils.getMessage("User Price");
                                }

                                dataGridViewTransaction.Rows[rowcount].Cells["Tax"].Value = Trx.TrxLines[j].tax_amount;
                                dataGridViewTransaction.Rows[rowcount].Cells["TaxName"].Value = Trx.TrxLines[j].taxName;
                                dataGridViewTransaction.Rows[rowcount].Cells["Line_Amount"].Value = Trx.TrxLines[j].LineAmount;
                                dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = j;
                                dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = Trx.TrxLines[j].ProductTypeCode;
                                dataGridViewTransaction.Rows[rowcount].Cells["Card_Number"].Value = cardnumber;
                                rowcount++;
                                Trx.TrxLines[j].LineProcessed = true;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < Trx.TrxLines.Count; i++) // display non-card trx lines
            {
                if (Trx.TrxLines[i].LineValid && !Trx.TrxLines[i].LineProcessed
                    && Trx.TrxLines[i].CardNumber == null
                    && Trx.TrxLines[i].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[i].ProductTypeCode != ProductTypeValues.GRATUITY)
                {
                    dataGridViewTransaction.Rows.Add();
                    dataGridViewTransaction.Rows[rowcount].Cells["Product_Type"].Value = Utilities.MessageUtils.getMessage("Item Sale");
                    dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = i;
                    dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = Trx.TrxLines[i].ProductTypeCode;

                    dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle);
                    dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

                    rowcount++;

                    for (int j = i; j < Trx.TrxLines.Count; j++)
                    {
                        if (Trx.TrxLines[j].CardNumber == null && Trx.TrxLines[j].LineValid && !Trx.TrxLines[j].LineProcessed
                            && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
                        {
                            //Modification on 17-Nov-2016, for printing parent order details
                            if (Trx.TrxLines[j].ParentLine != null && Trx.TrxLines.Equals(Trx.TrxLines[j].ParentLine))
                            {
                                var index = Trx.TrxLines.FindIndex(a => a == Trx.TrxLines[j].ParentLine);

                                if (index > -1 && index != j)
                                {
                                    displayNonCardLine(dataGridViewTransaction, Trx, index, ref rowcount);
                                }
                            }
                            else
                            {
                                displayNonCardLine(dataGridViewTransaction, Trx, j, ref rowcount);
                            }
                            //end modification
                        }
                    }
                }
            }

            int selectedRowIndex = rowcount - 1;
            if (selectedRowIndex < 0)
                selectedRowIndex = 0;

            //rowcount++;
            bool headerChargesDone = false;
            for (int i = 0; i < Trx.TrxLines.Count; i++) // display service charge lines
            {
                if (Trx.TrxLines[i].ProductTypeCode == ProductTypeValues.SERVICECHARGE && Trx.TrxLines[i].LineValid)
                {
                    if (!headerChargesDone)
                    {
                        dataGridViewTransaction.Rows.Add();
                        SetChargesHeder(dataGridViewTransaction, Utilities, rowcount, i, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Charges"),
                            MessageContainerList.GetMessage(Utilities.ExecutionContext, "Charges"));
                        rowcount++;
                        headerChargesDone = true;
                    }
                    displayNonCardLine(dataGridViewTransaction, Trx, i, ref rowcount);
                    //rowcount++;
                }
            }
            for (int i = 0; i < Trx.TrxLines.Count; i++) // display gratuity lines
            {
                if (Trx.TrxLines[i].ProductTypeCode == ProductTypeValues.GRATUITY && Trx.TrxLines[i].LineValid)
                {
                    if (!headerChargesDone)
                    {
                        dataGridViewTransaction.Rows.Add();
                        SetChargesHeder(dataGridViewTransaction, Utilities, rowcount, i, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Charges"),
                            MessageContainerList.GetMessage(Utilities.ExecutionContext, "Charges"));
                        rowcount++;
                        headerChargesDone = true;
                    }
                    displayNonCardLine(dataGridViewTransaction, Trx, i, ref rowcount);
                    //rowcount++;
                }
            }
            // display trx total
            dataGridViewTransaction.Rows.Add();
            dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = Utilities.MessageUtils.getMessage("Transaction Total");
            dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = "Transaction Total";
            dataGridViewTransaction.Rows[rowcount].Cells["Tax"].Value = Trx.Tax_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            dataGridViewTransaction.Rows[rowcount].Cells["Line_Amount"].Value = (Trx.Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

            DataGridViewCellStyle dgvtot = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle, 0);// new DataGridViewCellStyle();
            dgvtot.BackColor =
            dgvtot.SelectionBackColor = Color.LightGray;
            dgvtot.ForeColor =
            dgvtot.SelectionForeColor = Color.Black;
            dgvtot.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvtot.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = dgvtot;
            dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

            rowcount++;
            if (Trx.DiscountsSummaryDTOList != null && Trx.DiscountsSummaryDTOList.Any())
            {
                bool headerDone = false;
                for (int i = 0; i < Trx.DiscountsSummaryDTOList.Count; i++) // display discount lines
                {
                    if (Trx.DiscountsSummaryDTOList[i].DiscountAmount > 0)
                    {
                        if (!headerDone)
                        {
                            dataGridViewTransaction.Rows.Add();

                            dataGridViewTransaction.Rows[rowcount].Cells["Product_Type"].Value = Utilities.MessageUtils.getMessage("Discount");
                            dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = Trx.DiscountsSummaryDTOList[i].DiscountId;
                            dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = "Discount";

                            dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle);
                            dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

                            rowcount++;
                            headerDone = true;
                        }

                        dataGridViewTransaction.Rows.Add();
                        dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = Trx.DiscountsSummaryDTOList[i].DisplayChar + " " + Trx.DiscountsSummaryDTOList[i].DiscountName;
                        dataGridViewTransaction.Rows[rowcount].Cells["Quantity"].Value = Trx.DiscountsSummaryDTOList[i].Count;
                        if (Trx.DiscountsSummaryDTOList[i].CouponNumbers != null &&
                            Trx.DiscountsSummaryDTOList[i].CouponNumbers.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder(" (Coupon:");
                            int count = 0;
                            foreach (var couponNumber in Trx.DiscountsSummaryDTOList[i].CouponNumbers)
                            {
                                sb.Append(count == 0 ? "" : ", ");
                                sb.Append(couponNumber);
                                count++;
                            }
                            sb.Append(")");
                            dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value + sb.ToString();
                            dataGridViewTransaction.Rows[rowcount].Cells["Quantity"].Value = Trx.DiscountsSummaryDTOList[i].CouponNumbers.Count;
                        }
                        dataGridViewTransaction.Rows[rowcount].Cells["Price"].Value = Trx.DiscountsSummaryDTOList[i].DiscountPercentage.ToString(ParafaitEnv.AMOUNT_FORMAT) + "%";
                        dataGridViewTransaction.Rows[rowcount].Cells["Line_Amount"].Value = Trx.DiscountsSummaryDTOList[i].DiscountAmount;//.ToString(ParafaitUtils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = Trx.DiscountsSummaryDTOList[i].DiscountId;
                        dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = "Discount";
                        rowcount++;
                    }
                }
            }

            // display grand total
            dataGridViewTransaction.Rows.Add();
            dataGridViewTransaction.Rows[rowcount].Cells["Product_Name"].Value = Utilities.MessageUtils.getMessage("Grand Total");
            dataGridViewTransaction.Rows[rowcount].Cells["Line_Amount"].Value = (Trx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

            DataGridViewCellStyle dgvgrandtot = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle, 2);// new DataGridViewCellStyle();
            dgvgrandtot.BackColor =
            dgvgrandtot.SelectionBackColor = Color.Black;
            dgvgrandtot.ForeColor =
            dgvgrandtot.SelectionForeColor = Color.White;
            dgvgrandtot.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = dgvgrandtot;
            dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

            rowcount++;

            if (dataGridViewTransaction.Rows.Count > 1)
            {
                dataGridViewTransaction.Rows[selectedRowIndex].Selected = true;
                dataGridViewTransaction.Rows[0].Selected = false;
                dataGridViewTransaction.FirstDisplayedScrollingRowIndex = selectedRowIndex;
            }

            dataGridViewTransaction.Refresh();
            log.LogMethodExit(null);
        }

        private static void SetChargesHeder(DataGridView dataGridViewTransaction, Utilities Utilities, int rowcount, int i, string productType, string lineTYpe)
        {
            log.LogMethodEntry();
            dataGridViewTransaction.Rows[rowcount].Cells["Product_Type"].Value = MessageContainerList.GetMessage(Utilities.ExecutionContext, productType);
            dataGridViewTransaction.Rows[rowcount].Cells["LineId"].Value = i;
            dataGridViewTransaction.Rows[rowcount].Cells["Line_Type"].Value = lineTYpe;

            dataGridViewTransaction.Rows[rowcount].DefaultCellStyle = getSpecialGridRowFormat(dataGridViewTransaction.DefaultCellStyle);
            dataGridViewTransaction.Rows[rowcount].MinimumHeight = dataGridViewTransaction.Rows[rowcount].MinimumHeight + 25;

            log.LogMethodExit();
        }

        static void displayNonCardLine(DataGridView dataGridViewTransaction, Transaction Trx, int j, ref int rowcount)
        {
            log.LogMethodEntry(dataGridViewTransaction, Trx, j, rowcount);

            Utilities Utilities = Trx.Utilities;
            ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;
            bool addtoExistingLine = false;
            decimal existingQuantity = 0;
            double existingAmount = 0;
            double existingTaxAmount = 0;
            int targetRow = rowcount;
            List<Transaction.TransactionLine> trxLines = null;

            string productName = Trx.TrxLines[j].ProductName + (string.IsNullOrEmpty(Trx.TrxLines[j].AttractionDetails) ? "" : "-" + Trx.TrxLines[j].AttractionDetails) + (string.IsNullOrEmpty(Trx.TrxLines[j].Remarks) ? "" : "-" + Trx.TrxLines[j].Remarks);
            string highlight = "";
            //if (Trx.TrxLines[j].ParentLine == null) // display a special character to highlight discounted lines, only for highest level lines
            //{
            if (Trx.TrxLines[j].TransactionDiscountsDTOList != null)
            {
                foreach (var transactionDiscountsDTO in Trx.TrxLines[j].TransactionDiscountsDTOList)
                {
                    if (Trx.DiscountsSummaryDTODictionary != null &&
                       Trx.DiscountsSummaryDTODictionary.ContainsKey(transactionDiscountsDTO.DiscountId))
                    {
                        highlight += Trx.DiscountsSummaryDTODictionary[transactionDiscountsDTO.DiscountId].DisplayChar;
                    }
                }

                if (highlight != "")
                    productName = highlight + " " + productName;
            }
            //}

            int offset = 1;
            string sOffset = "";
            Transaction.TransactionLine tl = Trx.TrxLines[j].ParentLine;
            if (tl != null && tl != Trx.TrxLines[j])
            {
                while (tl.ParentLine != null && tl != tl.ParentLine)
                {
                    tl = tl.ParentLine;
                    offset++;
                }
                //changed ascii 192 to byte based conversion. This is used for displaying child products
                byte[] b = new byte[] { 20, 37 };
                sOffset = Encoding.Unicode.GetString(b);
                //End Modification - 18-Jan-2016
                sOffset = sOffset.PadLeft(offset * 3 + 1, ' ') + " ";
            }

            productName = sOffset + productName;

            // group lines which have same product name and price. however, do not group the lines with child lines
            bool hasChildLines = false;

            foreach (Transaction.TransactionLine tlChild in Trx.TrxLines)
            {
                if (tlChild.LineValid && Trx.TrxLines[j].Equals(tlChild.ParentLine))
                {
                    hasChildLines = true;
                    break;
                }
            }

            //Begin modiification - on -07-Dec-2016 for removing combo product one at a time
            if (hasChildLines)
            {
                if (Trx.TrxLines[j].ProductTypeCode == "COMBO")
                {
                    for (int row = 0; row < rowcount; row++)
                    {
                        if (productName.Equals(dataGridViewTransaction["Product_Name", row].Value == null ? "" : dataGridViewTransaction["Product_Name", row].Value.ToString())
                            && Trx.TrxLines[j].ProductTypeCode.Equals(dataGridViewTransaction["Line_Type", row].Value == null ? "" : dataGridViewTransaction["Line_Type", row].Value.ToString())
                            && (dataGridViewTransaction["Line_Type", row].Value == null ? "" : dataGridViewTransaction["Line_Type", row].Value.ToString()) != "COMBO"
                            && Trx.TrxLines[j].Price == Convert.ToDouble(dataGridViewTransaction["price", row].Value == null ? -874833.233 : dataGridViewTransaction["price", row].Value)
                            && Trx.TrxLines[j].ReceiptPrinted == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].ReceiptPrinted
                            && Trx.TrxLines[j].KDSSent == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].KDSSent
                            && Trx.TrxLines[j].CancelledLine == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].CancelledLine)
                        {
                            targetRow = row;
                            addtoExistingLine = true;
                            existingAmount = Convert.ToDouble(dataGridViewTransaction.Rows[targetRow].Cells["Line_Amount"].Value);
                            existingTaxAmount = Convert.ToDouble(dataGridViewTransaction.Rows[targetRow].Cells["Tax"].Value);
                            existingQuantity = Convert.ToDecimal(dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Value);
                            break;
                        }
                    }
                }

                if (!addtoExistingLine)
                {
                    dataGridViewTransaction.Rows.Add();
                    dataGridViewTransaction.Rows[targetRow].Cells["Product_Name"].Value = productName;
                }
            }
            else
            {
                if (Trx.TrxLines[j].ProductTypeCode == "MANUAL"
                    || Trx.TrxLines[j].ProductTypeCode == "DEPOSIT"
                    || Trx.TrxLines[j].ProductTypeCode == "RENTAL"
                    || Trx.TrxLines[j].ProductTypeCode == "RENTAL_RETURN")
                {
                    for (int row = 0; row < rowcount; row++)
                    {
                        bool isallowed = false;
                        if (Trx.TrxLines[j].ParentLine != null && Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].ParentLine != null)
                        {
                            if (Trx.TrxLines[j].ParentLine.ProductTypeCode == "COMBO")
                            {
                                //if (Trx.TrxLines[j].ParentLine.ProductID == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].ParentLine.ProductID)
                                if (Trx.TrxLines[j].ParentLine.Equals(Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].ParentLine))
                                    isallowed = true;
                            }
                            else
                            {
                                if (Trx.TrxLines[j].ParentLine == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].ParentLine)
                                    isallowed = true;
                            }
                        }
                        else
                        {
                            if (productName.Equals(dataGridViewTransaction["Product_Name", row].Value == null ? "" : dataGridViewTransaction["Product_Name", row].Value.ToString()))
                            {
                                if (trxLines == null)
                                    trxLines = new List<Transaction.TransactionLine>();
                                trxLines = Trx.TrxLines.FindAll(x => x.LineValid == true && x.ParentLine != null && x.ParentLine == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)]);
                                if (trxLines.Count <= 0)
                                    isallowed = true;
                            }
                        }
                        if (productName.Equals(dataGridViewTransaction["Product_Name", row].Value == null ? "" : dataGridViewTransaction["Product_Name", row].Value.ToString())
                            && Trx.TrxLines[j].ProductTypeCode.Equals(dataGridViewTransaction["Line_Type", row].Value == null ? "" : dataGridViewTransaction["Line_Type", row].Value.ToString())
                            && Trx.TrxLines[j].Price == Convert.ToDouble(dataGridViewTransaction["price", row].Value == null ? -874833.233 : dataGridViewTransaction["price", row].Value)
                            && Trx.TrxLines[j].ReceiptPrinted == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].ReceiptPrinted
                            && Trx.TrxLines[j].KDSSent == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].KDSSent
                            && Trx.TrxLines[j].CancelledLine == Trx.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", row].Value)].CancelledLine
                            && Trx.TrxLines[j].TrxProfileId.Equals(dataGridViewTransaction["TrxProfileId", row].Value == null ? -1 : dataGridViewTransaction["TrxProfileId", row].Value)
                            && isallowed)
                        {
                            targetRow = row;
                            addtoExistingLine = true;
                            existingAmount = Convert.ToDouble(dataGridViewTransaction.Rows[targetRow].Cells["Line_Amount"].Value);
                            existingTaxAmount = Convert.ToDouble(dataGridViewTransaction.Rows[targetRow].Cells["Tax"].Value);
                            existingQuantity = Convert.ToDecimal(dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Value);
                            break;
                        }
                    }
                }

                if (!addtoExistingLine)
                {
                    dataGridViewTransaction.Rows.Add();
                    dataGridViewTransaction.Rows[targetRow].Cells["Product_Name"].Value = productName;
                }
            }
            //End modiification - on -07-Dec-2016 for removing combo product one at a time

            dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Value = existingQuantity + Trx.TrxLines[j].quantity;
            dataGridViewTransaction.Rows[targetRow].Cells["Price"].Value = Trx.TrxLines[j].Price;

            List<int> groupedLines;
            if (dataGridViewTransaction.Rows[targetRow].Tag == null)
                groupedLines = new List<int>();
            else
                groupedLines = dataGridViewTransaction.Rows[targetRow].Tag as List<int>;

            groupedLines.Add(j);
            dataGridViewTransaction.Rows[targetRow].Tag = groupedLines;

            if (Trx.TrxLines[j].Price != Trx.TrxLines[j].OriginalPrice
                && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
            {
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].Style.BackColor = Color.LightGreen;
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].Style.SelectionBackColor = Color.LightGreen;
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].ToolTipText = Utilities.MessageUtils.getMessage("Promo Price") + " [" + Trx.TrxLines[j].OriginalPrice.ToString(ParafaitEnv.AMOUNT_FORMAT) + "]";
            }

            dataGridViewTransaction.Rows[targetRow].Cells["Remarks"].Value = Trx.TrxLines[j].Remarks;
            dataGridViewTransaction.Rows[targetRow].Cells["AttractionDetails"].Value = Trx.TrxLines[j].AttractionDetails;
            dataGridViewTransaction.Rows[targetRow].Cells["TrxProfileId"].Value = Trx.TrxLines[j].TrxProfileId;
            if (Utilities.ParafaitEnv.specialPricingId != -1
                && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
            {
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].Style.BackColor =
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].Style.SelectionBackColor = Color.OrangeRed;
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].ToolTipText = Utilities.MessageUtils.getMessage("Special Price");
            }

            if (Trx.TrxLines[j].UserPrice && Trx.TrxLines[j].AllowEdit
                && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
            {
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].Style.BackColor = Color.Goldenrod;
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].Style.SelectionBackColor = Color.Goldenrod;
                dataGridViewTransaction.Rows[targetRow].Cells["Price"].ToolTipText = Utilities.MessageUtils.getMessage("User Price");
            }

            dataGridViewTransaction.Rows[targetRow].Cells["Tax"].Value = existingTaxAmount + Trx.TrxLines[j].tax_amount;
            dataGridViewTransaction.Rows[targetRow].Cells["TaxName"].Value = Trx.TrxLines[j].taxName;
            dataGridViewTransaction.Rows[targetRow].Cells["Line_Amount"].Value = existingAmount + Trx.TrxLines[j].LineAmount;
            dataGridViewTransaction.Rows[targetRow].Cells["LineId"].Value = j;
            dataGridViewTransaction.Rows[targetRow].Cells["Line_Type"].Value = Trx.TrxLines[j].ProductTypeCode;

            if ((Trx.TrxLines[j].ProductTypeCode == "MANUAL" || Trx.TrxLines[j].ProductTypeCode == "RENTAL")
                && Trx.TrxLines[j].AllowEdit)
            {
                dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Style.BackColor =
                dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Style.SelectionBackColor = Color.LightBlue;
                dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Style.ForeColor =
                dataGridViewTransaction.Rows[targetRow].Cells["Quantity"].Style.SelectionForeColor = Color.Blue;
            }

            if (Trx.TrxLines[j].TransactionDiscountsDTOList != null &&
                Trx.TrxLines[j].TransactionDiscountsDTOList.Count > 0 &&
                Trx.TrxLines[j].AllowEdit
                && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.SERVICECHARGE && Trx.TrxLines[j].ProductTypeCode != ProductTypeValues.GRATUITY)
            {
                dataGridViewTransaction.Rows[targetRow].Cells["Product_Name"].Style.SelectionForeColor =
                    dataGridViewTransaction.Rows[targetRow].Cells["Product_Name"].Style.ForeColor = Color.Green;
            }

            if (!addtoExistingLine)
                rowcount++;

            Trx.TrxLines[j].LineProcessed = true;

            for (int k = j + 1; k < Trx.TrxLines.Count; k++)
            {
                if (Trx.TrxLines[j].Equals(Trx.TrxLines[k].ParentLine) && Trx.TrxLines[k].LineValid && !Trx.TrxLines[k].LineProcessed)
                    displayNonCardLine(dataGridViewTransaction, Trx, k, ref rowcount);
            }

            log.LogVariableState("rowcount", rowcount);
            log.LogMethodExit(null);
        }

        public static DataGridView createRefTrxDatagridview(Utilities Utilities)
        {
            log.LogMethodEntry(Utilities);

            DataGridView dgvRefTrxDatagridView = new DataGridView();
            dgvRefTrxDatagridView.AllowUserToAddRows = false;
            dgvRefTrxDatagridView.AllowUserToDeleteRows = false;
            dgvRefTrxDatagridView.AllowUserToResizeColumns = false;
            dgvRefTrxDatagridView.AllowUserToResizeRows = false;
            dgvRefTrxDatagridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
            dgvRefTrxDatagridView.BackgroundColor = System.Drawing.Color.Gray;
            dgvRefTrxDatagridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvRefTrxDatagridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvRefTrxDatagridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            System.Windows.Forms.DataGridViewTextBoxColumn Product_Type;
            System.Windows.Forms.DataGridViewTextBoxColumn Card_Number;
            System.Windows.Forms.DataGridViewTextBoxColumn Product_Name;
            System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
            System.Windows.Forms.DataGridViewTextBoxColumn Price;
            System.Windows.Forms.DataGridViewTextBoxColumn Tax;
            System.Windows.Forms.DataGridViewTextBoxColumn TaxName;
            System.Windows.Forms.DataGridViewTextBoxColumn Line_Amount;
            System.Windows.Forms.DataGridViewTextBoxColumn LineId;
            System.Windows.Forms.DataGridViewTextBoxColumn Line_Type;
            System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
            System.Windows.Forms.DataGridViewTextBoxColumn AttractionDetails;
            System.Windows.Forms.DataGridViewTextBoxColumn TrxProfileId;

            Product_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Card_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Product_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            TaxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Line_Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            LineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Line_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            AttractionDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            TrxProfileId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dgvRefTrxDatagridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            Product_Type,
            Card_Number,
            Product_Name,
            Quantity,
            Price,
            Tax,
            TaxName,
            Line_Amount,
            LineId,
            Line_Type,
            Remarks,
            AttractionDetails,
            TrxProfileId });
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvRefTrxDatagridView.DefaultCellStyle = dataGridViewCellStyle12;
            dgvRefTrxDatagridView.EnableHeadersVisualStyles = false;
            dgvRefTrxDatagridView.Location = new System.Drawing.Point(4, 6);
            dgvRefTrxDatagridView.Name = "dataGridViewTransaction";
            dgvRefTrxDatagridView.RowHeadersVisible = false;
            dgvRefTrxDatagridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvRefTrxDatagridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvRefTrxDatagridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            dgvRefTrxDatagridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dgvRefTrxDatagridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvRefTrxDatagridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvRefTrxDatagridView.Size = new System.Drawing.Size(550, 561);
            dgvRefTrxDatagridView.TabIndex = 4;
            dgvRefTrxDatagridView.TabStop = false;
            // 
            // Product_Type
            // 
            Product_Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            Product_Type.DefaultCellStyle = dataGridViewCellStyle5;
            Product_Type.HeaderText = Utilities.MessageUtils.getMessage("Type");
            Product_Type.Name = "Product_Type";
            Product_Type.ReadOnly = true;
            Product_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Product_Type.Width = 36;
            // 
            // Card_Number
            // 
            Card_Number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Card_Number.DefaultCellStyle = dataGridViewCellStyle6;
            Card_Number.HeaderText = Utilities.MessageUtils.getMessage("Card Number");
            Card_Number.Name = "Card_Number";
            Card_Number.ReadOnly = true;
            Card_Number.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Card_Number.Visible = false;

            // 
            // TrxProfileId
            // 
            TrxProfileId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TrxProfileId.DefaultCellStyle = dataGridViewCellStyle13;
            TrxProfileId.HeaderText = Utilities.MessageUtils.getMessage("TrxProfileId");
            TrxProfileId.Name = "TrxProfileId";
            TrxProfileId.ReadOnly = true;
            TrxProfileId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            TrxProfileId.Visible = false;
            // 
            // TaxName
            //
            TaxName.HeaderText = Utilities.MessageUtils.getMessage("Tax Name");
            TaxName.Name = "TaxName";
            TaxName.Visible = false;
            // 
            // Product_Name
            // 
            Product_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            Product_Name.DefaultCellStyle = dataGridViewCellStyle7;
            Product_Name.HeaderText = Utilities.MessageUtils.getMessage("Product");
            Product_Name.MinimumWidth = 30;
            Product_Name.Name = "Product_Name";
            Product_Name.ReadOnly = true;
            Product_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Product_Name.FillWeight = 500;
            // 
            // Quantity
            // 
            Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N" + Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS.ToString();
            Quantity.DefaultCellStyle = dataGridViewCellStyle8;
            Quantity.HeaderText = Utilities.MessageUtils.getMessage("Quantity");
            Quantity.Name = "Quantity";
            Quantity.ReadOnly = true;
            Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Quantity.Width = 52;
            // 
            // Price
            // 
            Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = null;
            Price.DefaultCellStyle = dataGridViewCellStyle9;
            Price.HeaderText = Utilities.MessageUtils.getMessage("Price");
            Price.MinimumWidth = 30;
            Price.Name = "Price";
            Price.ReadOnly = true;
            Price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Price.Width = 36;
            Price.FillWeight = 100;
            // 
            // Tax
            // 
            Tax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            Tax.DefaultCellStyle = dataGridViewCellStyle10;
            Tax.HeaderText = Utilities.MessageUtils.getMessage("Tax");
            Tax.Name = "Tax";
            Tax.ReadOnly = true;
            Tax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Tax.Width = 30;
            Tax.MinimumWidth = 30;
            Tax.FillWeight = 100;
            // 
            // Line_Amount
            // 
            Line_Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            Line_Amount.DefaultCellStyle = dataGridViewCellStyle11;
            Line_Amount.HeaderText = Utilities.MessageUtils.getMessage("Amount");
            Line_Amount.MinimumWidth = 100;
            Line_Amount.Name = "Line_Amount";
            Line_Amount.ReadOnly = true;
            Line_Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LineId
            // 
            LineId.HeaderText = "LineId";
            LineId.Name = "LineId";
            LineId.Visible = false;
            // 
            // AttractionDetails
            // 
            AttractionDetails.HeaderText = "AttractionDetails";
            AttractionDetails.Name = "AttractionDetails";
            AttractionDetails.Visible = false;
            // 
            // Line_Type
            // 
            Line_Type.HeaderText = "Line_Type";
            Line_Type.Name = "Line_Type";
            Line_Type.Visible = false;
            // 
            // Remarks
            // 
            Remarks.HeaderText = "Remarks";
            Remarks.Name = "Remarks";
            Remarks.Visible = false;

            log.LogMethodExit(dgvRefTrxDatagridView);
            return dgvRefTrxDatagridView;
        }
    }
}

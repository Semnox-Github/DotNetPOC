/********************************************************************************************
 * Project Name - DiscountCouponsUI
 * Description  - Discount Coupons UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-APR-2019   Raghuveera      Created 
 *2.80        28-Jun-2020   Deeksha         Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    public class ImportDiscountCoupon:ImportToExcelUI
    {
        private DiscountCouponsExcelDTODefinition discountCouponsExcelDTODefinition;
        private int discountHeaderId;
        private int discountId;
        private int paymentModeId = -1;
        private ManagementStudioSwitch managementStudioSwitch;
        public ImportDiscountCoupon(Utilities utilities, int discountId, int discountHeaderId)
            : base(utilities)
        {
            log.LogMethodEntry(utilities);
            this.Text = MessageContainerList.GetMessage(utilities.ExecutionContext,"Discount Coupons",utilities.ParafaitEnv.DATE_FORMAT);
            this.discountHeaderId = discountHeaderId;
            this.discountId = discountId;
            importingItemName = "Coupon";
            discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(utilities.ExecutionContext,null,false, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"));
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }
        public ImportDiscountCoupon(Utilities utilities, int discountId, int discountHeaderId, int paymentModeid)
            : this(utilities, discountId, discountHeaderId)
        {
            log.LogMethodEntry(utilities, discountId, discountHeaderId, paymentModeid);
            this.paymentModeId = paymentModeid;
            discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(utilities.ExecutionContext, null, true, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT"));
            this.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Payment Coupons", utilities.ParafaitEnv.DATE_FORMAT);
            log.LogMethodExit();
        }
        protected override void ImportRecords()
        {
            log.LogMethodEntry();
            SqlTransaction sqlTransaction = null;
            SqlConnection sqlConnection = utilities.getConnection();
            List<DiscountCouponsDTO> discountCouponsDTOList = new List<DiscountCouponsDTO>();
            totalRecordCount = uploadedSheet.Rows.Count;
            for (int i = 1; i < uploadedSheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    DiscountCouponsDTO discountCouponDTO = (DiscountCouponsDTO)discountCouponsExcelDTODefinition.Deserialize(uploadedSheet[0], uploadedSheet[i], ref index);
                    if (discountCouponDTO.CouponSetId < 0)
                    {
                        try
                        {
                            discountCouponDTO.DiscountId = discountId;
                            discountCouponDTO.DiscountCouponHeaderId = discountHeaderId;
                            discountCouponDTO.PaymentModeId = paymentModeId;
                            discountCouponsDTOList.Add(discountCouponDTO);                            
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while importing discount coupon.", ex);
                    log.LogVariableState("Row", uploadedSheet[i]);
                    if (errorSheet == null)
                    {
                        errorSheet = new Sheet();
                        errorSheet.AddRow(uploadedSheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                    }
                    errorSheet.AddRow(uploadedSheet[i]);
                    string errorMessage = "";
                    string seperator = "";
                    if (ex is ValidationException)
                    {
                        foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                        {
                            errorMessage += seperator;
                            errorMessage += validationError.Message;
                            seperator = ", ";
                        }
                    }
                    else
                    {
                        errorMessage = uploadedSheet[0].Cells[index].Value+": "+ ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                    errorRecordCount++;
                    progressBar.Invoke((Action)(() => progressBar.Value = (50 + (10 * (i + 1) / totalRecordCount))));
                }
            }
            try
            {
                
                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(utilities.ExecutionContext, discountCouponsDTOList);
                List<DiscountCouponsDTO> validDiscountCouponsDTOList = new List<DiscountCouponsDTO>();
                List<ValidationError> validationErrors = discountCouponsListBL.Validate(sqlTransaction);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    if (errorSheet == null)
                    {
                        errorSheet = new Sheet();
                        errorSheet.AddRow(uploadedSheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                    }

                    //string errorMessage = "";
                    //string seperator = "";
                    for (int i = 0; i < validationErrors.Count; i++)
                    {
                        if (uploadedSheet.Rows.Count > (validationErrors[i].RecordIndex + 1) &&
                            validationErrors[i].RecordIndex >= 0)
                        {
                            Row row = uploadedSheet[validationErrors[i].RecordIndex + 1];
                            row.AddCell(new Cell(validationErrors[i].FieldName +": "+ validationErrors[i].Message));
                            errorSheet.AddRow(row);
                        }
                        errorRecordCount++;
                    }

                    for (int i = 0; i < discountCouponsDTOList.Count; i++)
                    {
                        if(validationErrors.Any(x => x.RecordIndex == i) == false)
                        {
                            validDiscountCouponsDTOList.Add(discountCouponsDTOList[i]);
                        }
                    }
                }
                else
                {
                    validDiscountCouponsDTOList = discountCouponsDTOList;
                }
                if(validDiscountCouponsDTOList.Any())
                {
                    sqlTransaction = sqlConnection.BeginTransaction();
                    discountCouponsListBL = new DiscountCouponsListBL(utilities.ExecutionContext, validDiscountCouponsDTOList);
                    discountCouponsListBL.Save(sqlTransaction);
                    sqlTransaction.Commit();
                    importedRecordCount= validDiscountCouponsDTOList.Count;
                }
                
                progressBar.Invoke((Action)(() => progressBar.Value = (100)));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while importing discount coupon.", ex);
                string errorMessage = "";
                string seperator = "";
                if (errorSheet == null)
                {
                    errorSheet = new Sheet();
                    errorSheet.AddRow(uploadedSheet[0]);
                    errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                }
                if (ex is ValidationException)
                {
                    List<ValidationError> validationErrors = (ex as ValidationException).ValidationErrorList;
                   
                    if (validationErrors != null && validationErrors.Count > 0)
                    {
                        for (int i = 0; i < validationErrors.Count; i++)
                        {
                            errorSheet.AddRow(uploadedSheet[i]);
                            errorMessage += seperator;
                            errorMessage += validationErrors[i].Message;
                            seperator = ", ";
                            if (errorSheet.Rows.Count > validationErrors[i].RecordIndex &&
                                validationErrors[i].RecordIndex >= 0)
                            {
                                Row row = errorSheet[validationErrors[i].RecordIndex];
                                if (row != null)
                                    row.AddCell(new Cell(errorMessage));
                            }
                            errorRecordCount++;
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                    errorRecordCount++;
                }
                else
                {
                    errorMessage = ex.Message;
                }
                errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                errorRecordCount++;

                if (sqlTransaction != null)
                    sqlTransaction.Rollback();
            }
            sqlConnection.Close();
            log.LogMethodExit();
        }

        protected override AttributeDefinition GetAttributeDefinition()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return discountCouponsExcelDTODefinition;
        }
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                btnTemplate.Enabled = true;
                btnUpload.Enabled = true;

            }
            else
            {
                btnTemplate.Enabled = false;
                btnUpload.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - ImportDiscountCouponUsed
 * Description  - ImportDiscountCouponUsed
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-APR-2019   Raghuveera          Created 
 *2.70.2        12-Aug-2019   Girish Kundar       Modified: Removed unused namespace's. 
* 2.80        25-Jun-2020      Deeksha            Modified to Make Product module 
*                                                 read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    public class ImportDiscountCouponUsed : ImportToExcelUI
    {
        DiscountCouponUsedExcelDTODefinition discountCouponUsedExcelDTODefinition;
        private ManagementStudioSwitch managementStudioSwitch;
        public ImportDiscountCouponUsed(Utilities utilities)
            : base(utilities)
        {
            log.LogMethodEntry(utilities);
            this.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Discount Coupons Used");
            discountCouponUsedExcelDTODefinition = new DiscountCouponUsedExcelDTODefinition(utilities.ExecutionContext, "");
            importingItemName = "Coupon";
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }
        protected override void ImportRecords()
        {
            log.LogMethodEntry();
            SqlConnection sqlConnection = utilities.getConnection();
            totalRecordCount = uploadedSheet.Rows.Count;
            DiscountCouponsBL discountCouponsBL;
            DiscountsBL discountsBL;
            string publishedSite = "";
            SqlTransaction sqlTransaction = null;
            for (int i = 1; i < uploadedSheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    DiscountCouponsUsedDTO discountCouponUsedDTO = (DiscountCouponsUsedDTO)discountCouponUsedExcelDTODefinition.Deserialize(uploadedSheet[0], uploadedSheet[i], ref index);
                    if (discountCouponUsedDTO.Id < 0)
                    {
                        sqlTransaction = sqlConnection.BeginTransaction();
                        try
                        {
                            discountCouponsBL = new DiscountCouponsBL(utilities.ExecutionContext, discountCouponUsedDTO.CouponNumber, sqlTransaction);
                            if (discountCouponsBL.DiscountCouponsDTO != null)
                            {
                                discountCouponUsedDTO.CouponSetId = discountCouponsBL.DiscountCouponsDTO.CouponSetId;
                                discountCouponUsedDTO.DiscountCouponHeaderId = discountCouponsBL.DiscountCouponsDTO.DiscountCouponHeaderId;
                                if (discountCouponsBL.DiscountCouponsDTO.Count>0 && discountCouponsBL.DiscountCouponsDTO.UsedCount == discountCouponsBL.DiscountCouponsDTO.Count)
                                {
                                    if (errorSheet == null)
                                    {
                                        errorSheet = new Sheet();
                                        errorSheet.AddRow(uploadedSheet[0]);
                                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                                    }
                                    errorSheet.AddRow(uploadedSheet[i]);
                                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell("Coupon(s) already used."));
                                    errorRecordCount++;
                                    sqlTransaction.Rollback();
                                    continue;
                                }
                            }
                            else
                            {
                                if (errorSheet == null)
                                {
                                    errorSheet = new Sheet();
                                    errorSheet.AddRow(uploadedSheet[0]);
                                    errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                                }
                                errorSheet.AddRow(uploadedSheet[i]);
                                errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell("Invalid discount coupon."));
                                errorRecordCount++;
                                sqlTransaction.Rollback();
                                continue;
                            }
                            
                            DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(utilities.ExecutionContext, discountCouponUsedDTO);
                            discountCouponsUsedBL.Save(sqlTransaction);
                            discountCouponsUsedBL = new DiscountCouponsUsedBL(utilities.ExecutionContext, discountCouponUsedDTO.Id, sqlTransaction);
                            discountCouponsBL.DiscountCouponsDTO.UsedCount++;                            
                            discountCouponsBL.Save(sqlTransaction);
                            if(utilities.ParafaitEnv.IsCorporate)
                            {
                                log.LogVariableState("DiscountCouponsDTO", discountCouponsBL.DiscountCouponsDTO);
                                using(UnitOfWork unitOfWork = new UnitOfWork())
                                {
                                    discountsBL = new DiscountsBL(utilities.ExecutionContext, unitOfWork, discountCouponsBL.DiscountCouponsDTO.DiscountId);
                                    log.LogVariableState("DiscountsDTO", discountsBL.DiscountsDTO);
                                }
                                publishedSite = DBSynch.getRoamingSitesForEntity("Discounts", utilities.ExecutionContext.GetSiteId(), Guid.Parse(discountsBL.DiscountsDTO.Guid), sqlTransaction.Connection, sqlTransaction);
                                log.LogVariableState("DiscountCouponsUsedDTO", discountCouponsUsedBL.DiscountCouponsUsedDTO);
                                log.LogVariableState("publishedSite", publishedSite);
                                log.LogVariableState("Guid", discountCouponsUsedBL.DiscountCouponsUsedDTO.Guid);
                                log.LogVariableState("site_id", utilities.ExecutionContext.GetSiteId());
                                log.LogVariableState("ServerTime", utilities.getServerTime());
                                log.LogVariableState("sqlTransaction", sqlTransaction);
                                DBSynch.CreateRoamingData("DiscountCouponsused", Guid.Parse(discountCouponsUsedBL.DiscountCouponsUsedDTO.Guid), utilities.ExecutionContext.GetSiteId(), publishedSite, utilities.getServerTime(), sqlTransaction.Connection, sqlTransaction);
                            }
                            sqlTransaction.Commit();
                            importedRecordCount++;
                        }
                        catch (Exception)
                        {                            
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while importing discount coupon used.", ex);
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
                        errorMessage = ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                    errorRecordCount++;
                    if(sqlTransaction != null)
                    sqlTransaction.Rollback();
                }
                progressBar.Invoke((Action)(() => progressBar.Value = (50 + (10 * (i + 1) / totalRecordCount))));
            }
            sqlConnection.Close();            
            log.LogMethodExit();
        }

        protected override AttributeDefinition GetAttributeDefinition()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return discountCouponUsedExcelDTODefinition;
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

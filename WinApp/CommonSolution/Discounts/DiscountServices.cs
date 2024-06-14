/********************************************************************************************
 * Project Name - Discount                                                                          
 * Description  - Manages the DiscountCouponsDTO object. 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0      27-Jun-2019   Mehraj         Created     
 * 2.70.0     10-Oct-2019   Rakesh         Modify BulkUpload() and BuildTemplate() method
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    public class DiscountServices
    {
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Sheet responseSheet;
        private Sheet errorSheet;
        Utilities utilities;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DiscountServices(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Build the sheet object templete DiscountCouponsDTO for webmanagement
        /// </summary>
        /// <returns></returns>
        public Sheet BuildTemplete(int discountId, string activityType, bool buildTemplate, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(discountId, activityType, buildTemplate, sqlTransaction);
                DiscountCouponsExcelDTODefinition discountCouponsExcelDTODefinition = null;
                Sheet sheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();
                ///Mapper class thats map sheet object
                switch (activityType.ToUpper().ToString())
                {
                    case "PRODUCTDISCOUNTCOUPONS":
                        discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"), false);
                        break;
                    case "PAYMENTMODECOUPONS":
                        discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"), true);
                        break;
                }
                ///Building headers from DiscountCouponsExcelDTODefinition
                discountCouponsExcelDTODefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);

                if (!buildTemplate && activityType.ToUpper()== "PRODUCTDISCOUNTCOUPONS")
                {
                    DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
                    List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    if (discountId > -1)
                    {
                        searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID, discountId.ToString()));
                    }
                    List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsDataHandler.GetDiscountCouponsDTOList(searchParameters);
                    if (discountCouponsDTOList != null && discountCouponsDTOList.Any())
                    {
                        foreach (DiscountCouponsDTO discountCouponsDTO in discountCouponsDTOList)
                        {
                            discountCouponsExcelDTODefinition.Configure(discountCouponsDTO);

                            Row row = new Row();
                            discountCouponsExcelDTODefinition.Serialize(row, discountCouponsDTO);
                            sheet.AddRow(row);
                        }
                    }
                }

                log.LogMethodExit();
                return sheet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Insert DiscountCoupons after Deserializing from sheet object and saving 
        /// Converts sheet object of webmanagement to DiscountCouponsDTO
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="discountId"></param>
        /// <param name="discountHeaderId"></param>
        /// <returns></returns>
        public Sheet BulkUpload(Sheet sheet, int discountId, int discountHeaderId, int paymentModeId)
        {
            log.LogMethodEntry(sheet, discountId, discountHeaderId, paymentModeId);
            DiscountCouponsExcelDTODefinition discountCouponsExcelDTODefinition;
            //Mapper class initialization. This class does all the converstions for sheet
            //If paymentModeId > 0 then DTODefination is for Payment coupon else DTODefination is for Discount Coupon
            if (paymentModeId > 0)
            {
                discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"), true);
            }
            else
            {
                discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"), false);
            }
            //int errorMachineCount = 0;
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            List<DiscountCouponsDTO> discountCouponsDTOList = new List<DiscountCouponsDTO>();
            DiscountCouponsDTO rowDiscountDTO = new DiscountCouponsDTO();
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;

                try
                {
                    rowDiscountDTO = (DiscountCouponsDTO)discountCouponsExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    if (rowDiscountDTO.CouponSetId < 0)
                    {
                        try
                        {
                            rowDiscountDTO.DiscountId = discountId;
                            rowDiscountDTO.DiscountCouponHeaderId = discountHeaderId;
                            rowDiscountDTO.PaymentModeId = paymentModeId;
                            discountCouponsDTOList.Add(rowDiscountDTO);
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while importing discount coupon.", ex);
                    log.LogVariableState("Row", sheet[i]);
                    if (errorSheet == null)
                    {
                        errorSheet = new Sheet();
                        errorSheet.AddRow(sheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                    }
                    errorSheet.AddRow(sheet[i]);
                    string errorMessage = string.Empty;
                    string seperator = string.Empty;
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
                        errorMessage = sheet[0].Cells[index].Value + ": " + ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                }
            }

            try
            {
                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(executionContext, discountCouponsDTOList);
                List<DiscountCouponsDTO> validDiscountCouponsDTOList = new List<DiscountCouponsDTO>();
                List<ValidationError> validationErrors = discountCouponsListBL.Validate(parafaitDBTrx.SQLTrx);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    if (errorSheet == null)
                    {
                        errorSheet = new Sheet();
                        errorSheet.AddRow(sheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Errors")));
                    }
                    for (int i = 0; i < validationErrors.Count; i++)
                    {
                        if (sheet.Rows.Count > (validationErrors[i].RecordIndex + 1) &&
                            validationErrors[i].RecordIndex >= 0)
                        {
                            Row row = sheet[validationErrors[i].RecordIndex + 1];
                            row.AddCell(new Cell(validationErrors[i].FieldName + ": " + validationErrors[i].Message));
                            errorSheet.AddRow(row);
                        }
                        //errorRecordCount++;
                    }

                    for (int i = 0; i < discountCouponsDTOList.Count; i++)
                    {
                        if (validationErrors.Any(x => x.RecordIndex == i) == false)
                        {
                            validDiscountCouponsDTOList.Add(discountCouponsDTOList[i]);
                        }
                    }
                }
                else
                {
                    validDiscountCouponsDTOList = discountCouponsDTOList;
                }
                if (validDiscountCouponsDTOList.Any())
                {
                    parafaitDBTrx.BeginTransaction();
                    discountCouponsListBL = new DiscountCouponsListBL(executionContext, validDiscountCouponsDTOList);
                    discountCouponsListBL.Save(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.SQLTrx.Commit();

                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while importing discount coupon.", ex);
                string errorMessage = string.Empty;
                string seperator = string.Empty;
                if (errorSheet == null)
                {
                    errorSheet = new Sheet();
                    errorSheet.AddRow(sheet[0]);
                    errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Errors")));
                }
                if (ex is ValidationException)
                {
                    List<ValidationError> validationErrors = (ex as ValidationException).ValidationErrorList;

                    if (validationErrors != null && validationErrors.Count > 0)
                    {
                        for (int i = 0; i < validationErrors.Count; i++)
                        {
                            errorSheet.AddRow(sheet[i]);
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
                            //errorRecordCount++;
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell(errorMessage));
                    if (parafaitDBTrx.SQLTrx != null)
                        parafaitDBTrx.SQLTrx.Rollback();
                }
            }
            log.LogMethodExit(errorSheet);
            return errorSheet;
        }

        /// <summary>
        /// Build the sheet object templete DiscountCouponUsedDTO for webmanagement
        /// </summary>
        /// <returns></returns>
        public Sheet DiscountUsedCouponBuildTemplete(bool buildTemplate, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                Sheet sheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();
                ///Mapper class thats map sheet object
                DiscountCouponUsedExcelDTODefinition discountCouponUsedExcelDTO = new DiscountCouponUsedExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"));
                ///Building headers from DiscountCouponUsedExcelDTODefinition
                discountCouponUsedExcelDTO.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);

                if (!buildTemplate)
                {

                    DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
                    List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedDataHandler.GetDiscountCouponsUsedDTOList(searchParameters);
                    if (discountCouponsUsedDTOList != null && discountCouponsUsedDTOList.Any())
                    {
                        foreach (DiscountCouponsUsedDTO discountCouponsUsedDTO in discountCouponsUsedDTOList)
                        {
                            discountCouponUsedExcelDTO.Configure(discountCouponsUsedDTO);

                            Row row = new Row();
                            discountCouponUsedExcelDTO.Serialize(row, discountCouponsUsedDTO);
                            sheet.AddRow(row);
                        }
                    }
                }

                log.LogMethodExit(sheet);
                return sheet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Insert DiscountCouponsUsedDTO after Deserializing from sheet object and saving 
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public Sheet BulkUploadUsedCoupons(Sheet sheet)
        {
            log.LogMethodEntry(sheet);
            DiscountCouponsBL discountCouponsBL;
            DiscountsBL discountsBL;
            string publishedSite = string.Empty;
            //Mapper class initialization. This class does all the converstions for sheet
            DiscountCouponUsedExcelDTODefinition discountCouponUsedExcelDTODefinition = new DiscountCouponUsedExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"));
            //int errorMachineCount = 0;
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;

                try
                {
                    DiscountCouponsUsedDTO discountCouponUsedDTO = (DiscountCouponsUsedDTO)discountCouponUsedExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    if (discountCouponUsedDTO.Id < 0)
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {
                            discountCouponsBL = new DiscountCouponsBL(executionContext, discountCouponUsedDTO.CouponNumber);
                            if (discountCouponsBL.DiscountCouponsDTO != null)
                            {
                                discountCouponUsedDTO.CouponSetId = discountCouponsBL.DiscountCouponsDTO.CouponSetId;
                                discountCouponUsedDTO.DiscountCouponHeaderId = discountCouponsBL.DiscountCouponsDTO.DiscountCouponHeaderId;
                                if (discountCouponsBL.DiscountCouponsDTO.Count > 0 && discountCouponsBL.DiscountCouponsDTO.UsedCount == discountCouponsBL.DiscountCouponsDTO.Count)
                                {
                                    if (errorSheet == null)
                                    {
                                        errorSheet = new Sheet();
                                        errorSheet.AddRow(sheet[0]);
                                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Errors")));
                                    }
                                    errorSheet.AddRow(sheet[i]);
                                    errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell("Coupon(s) already used."));
                                    //errorRecordCount++;
                                    parafaitDBTrx.SQLTrx.Rollback();
                                    continue;
                                }
                            }
                            else
                            {
                                if (errorSheet == null)
                                {
                                    errorSheet = new Sheet();
                                    errorSheet.AddRow(sheet[0]);
                                    errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Errors")));
                                }
                                errorSheet.AddRow(sheet[i]);
                                errorSheet[errorSheet.Rows.Count - 1].AddCell(new Cell("Invalid discount coupon."));
                                //errorRecordCount++;
                                //sqlTransaction.Rollback();
                                continue;
                            }
                            DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(executionContext, discountCouponUsedDTO);
                            discountCouponsUsedBL.Save(parafaitDBTrx.SQLTrx);
                            discountCouponsUsedBL = new DiscountCouponsUsedBL(executionContext ,discountCouponUsedDTO.Id, parafaitDBTrx.SQLTrx);
                            discountCouponsBL.DiscountCouponsDTO.UsedCount++;
                            discountCouponsBL.Save(parafaitDBTrx.SQLTrx);
                            if (executionContext.GetIsCorporate())
                            {
                                log.LogVariableState("DiscountCouponsDTO", discountCouponsBL.DiscountCouponsDTO);
                                discountsBL = new DiscountsBL(executionContext, new ExternallyManagedUnitOfWork(parafaitDBTrx.SQLTrx),discountCouponsBL.DiscountCouponsDTO.DiscountId);
                                log.LogVariableState("DiscountsDTO", discountsBL.DiscountsDTO);
                                publishedSite = DBSynch.getRoamingSitesForEntity("Discounts", executionContext.GetSiteId(), Guid.Parse(discountsBL.DiscountsDTO.Guid), parafaitDBTrx.SQLTrx.Connection, parafaitDBTrx.SQLTrx);
                                log.LogVariableState("DiscountCouponsUsedDTO", discountCouponsUsedBL.DiscountCouponsUsedDTO);
                                log.LogVariableState("publishedSite", publishedSite);
                                log.LogVariableState("Guid", discountCouponsUsedBL.DiscountCouponsUsedDTO.Guid);
                                log.LogVariableState("site_id", executionContext.GetSiteId());
                                log.LogVariableState("ServerTime", utilities.getServerTime());
                                log.LogVariableState("sqlTransaction", parafaitDBTrx.SQLTrx);
                                DBSynch.CreateRoamingData("DiscountCouponsused", Guid.Parse(discountCouponsUsedBL.DiscountCouponsUsedDTO.Guid), executionContext.GetSiteId(), publishedSite, utilities.getServerTime(), parafaitDBTrx.SQLTrx.Connection, parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.SQLTrx.Commit();
                            //importedRecordCount++;
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
                    log.LogVariableState("Row", sheet[i]);
                    if (errorSheet == null)
                    {
                        errorSheet = new Sheet();
                        errorSheet.AddRow(sheet[0]);
                        errorSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(utilities.ExecutionContext, "Errors")));
                    }
                    errorSheet.AddRow(sheet[i]);
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
                    //errorRecordCount++;
                    if (parafaitDBTrx.SQLTrx != null)
                        parafaitDBTrx.SQLTrx.Rollback();
                }

            }
            parafaitDBTrx.EndTransaction();
            log.LogMethodExit(errorSheet);
            return errorSheet;
        }
    }
}

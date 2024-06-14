/********************************************************************************************
 * Project Name - Receipt Print Template Header BL
 * Description  - Business logic to handle Receipt Template Header
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2018      Mathew Ninan     Created
 *2.60        17-Mar-2019      Jagan Mohana Rao added SaveUpdateReceiptPrintTemplateHeaderList() method in ReceiptPrintTemplateHeaderListBL class
              07-May-2019      Mushahid Faizan  Modified Try-catch block and added Sql Transaction in SaveUpdateReceiptPrintTemplateHeaderList()
 *2.70.2      18-Jul-2019      Deeksha          Modifications as per 3 tier standard.
 *2.70.3      11-Feb-2020      Deeksha          Invariant culture-Font Issue Fix
 *2.70.3      1- Apr-2020      Girish Kundar    Modified: SaveList() method to catch unique template name value violation in WMS UI
 *2.120.0     10-May-2021      Mushahid Faizan  Modified: GetReceiptPrintTemplateHeaderDTOList() method to load active/Inactive child records in WMS.
 *2.140       14-Sep-2021      Fiona            Modified: Issue fix in Id Constructor
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// BL class for ReceiptPrintTemplateHeader
    /// </summary>
    public class ReceiptPrintTemplateHeaderBL
    {
        private ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ReceiptPrintTemplateBL class
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ReceiptPrintTemplateHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            receiptPrintTemplateHeaderDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Id as the parameter
        /// Would fetch the Receipt Print Template Header object from the database based 
        /// on the template id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="templateId">Id</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReceiptPrintTemplateHeaderBL(ExecutionContext executionContext, int templateId, bool loadChildRecords, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, templateId, loadChildRecords, sqlTransaction);
            ReceiptPrintTemplateHeaderDataHandler receiptPrintTemplateHeaderDataHandler = new ReceiptPrintTemplateHeaderDataHandler(sqlTransaction);
            receiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderDataHandler.GetReceiptPrintTemplateHeader(templateId);
            if (loadChildRecords && receiptPrintTemplateHeaderDTO != null)
            {
                ReceiptPrintTemplateListBL receiptPrintTemplateListBL = new ReceiptPrintTemplateListBL(executionContext);
                List<KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>> receiptPrintTemplateSearchParams = new List<KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>>();
                receiptPrintTemplateSearchParams.Add(new KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>(ReceiptPrintTemplateDTO.SearchByParameters.TEMPLATE_ID, receiptPrintTemplateHeaderDTO.TemplateId.ToString()));
                receiptPrintTemplateSearchParams.Add(new KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>(ReceiptPrintTemplateDTO.SearchByParameters.IS_ACTIVE, "Y"));
                receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList = receiptPrintTemplateListBL.GetReceiptPrintTemplateDTOList(receiptPrintTemplateSearchParams,sqlTransaction);
                ReceiptPrintTemplateHeaderBuildBL receiptPrintTemplateBuildBL = new ReceiptPrintTemplateHeaderBuildBL(executionContext);
                if (receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                {
                    receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList = receiptPrintTemplateBuildBL.GetReceiptFont(receiptPrintTemplateHeaderDTO).ReceiptPrintTemplateDTOList;
                }
                receiptPrintTemplateHeaderDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ReceiptPrintTemplateHeaderBL object using the receiptPrintTemplateHeaderDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="receiptPrintTemplateHeaderDTO">receiptPrintTemplateHeaderDTO object</param>
        public ReceiptPrintTemplateHeaderBL(ExecutionContext executionContext, ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, receiptPrintTemplateHeaderDTO);
            this.receiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Receipt Print Template Header
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ReceiptPrintTemplateHeaderDataHandler receiptPrintTemplateHeaderDataHandler = new ReceiptPrintTemplateHeaderDataHandler(sqlTransaction);
            if (receiptPrintTemplateHeaderDTO.TemplateId < 0)
            {
                receiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderDataHandler.InsertReceiptPrintTemplateHeader(receiptPrintTemplateHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                receiptPrintTemplateHeaderDTO.AcceptChanges();
            }
            else
            {
                if (receiptPrintTemplateHeaderDTO.IsChanged)
                {
                    receiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderDataHandler.UpdateReceiptPrintTemplateHeader(receiptPrintTemplateHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    receiptPrintTemplateHeaderDTO.AcceptChanges();
                }
            }
            if (receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null && receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count > 0)
            {
                foreach (ReceiptPrintTemplateDTO receiptPrintTemplateDTO in receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList)
                {
                    if (receiptPrintTemplateDTO.TemplateId == -1)
                    {
                        receiptPrintTemplateDTO.TemplateId = receiptPrintTemplateHeaderDTO.TemplateId;
                    }
                    ReceiptPrintTemplateBL receiptPrintTemplateBL = new ReceiptPrintTemplateBL(executionContext, receiptPrintTemplateDTO);
                    receiptPrintTemplateBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// For Management Studio, support Import option. 
        /// This will give query output in text file
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="templateName">templateName</param>
        /// <param name="duplicate">duplicate</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>query string</returns>
        public void ExecuteImportQuery(string query, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(query, sqlTransaction);
            try
            {
                ReceiptPrintTemplateHeaderDataHandler receiptPrintTemplateHeaderDataHandler = new ReceiptPrintTemplateHeaderDataHandler(sqlTransaction);
                receiptPrintTemplateHeaderDataHandler.ExecuteImportQuery(query, executionContext.GetUserId());

            }
            catch
            {
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ReceiptPrintTemplateHeaderDTO ReceiptPrintTemplateHeaderDTO
        {
            get
            {
                return receiptPrintTemplateHeaderDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Receipt Print Template Header
    /// </summary>
    public class ReceiptPrintTemplateHeaderListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ReceiptPrintTemplateHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="receiptPrintTemplateHeaderDTOList">ReceiptPrintTemplateHeaderDTO</param>
        public ReceiptPrintTemplateHeaderListBL(ExecutionContext executionContext, List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList)
        {
            log.LogMethodEntry(executionContext, receiptPrintTemplateHeaderDTOList);
            this.receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Printer list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Printer list</returns>
        public List<ReceiptPrintTemplateHeaderDTO> GetReceiptPrintTemplateHeaderDTOList(List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords, SqlTransaction sqlTransaction = null, bool loadActiveChildRecords = false)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, sqlTransaction);
            ReceiptPrintTemplateHeaderDataHandler receiptPrintTemplateHeaderDataHandler = new ReceiptPrintTemplateHeaderDataHandler(sqlTransaction);
            List<ReceiptPrintTemplateHeaderDTO> returnValue = receiptPrintTemplateHeaderDataHandler.GetReceiptPrintTemplateHeaderList(searchParameters);
            if (loadChildRecords && returnValue != null)
            {
                foreach (ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO in returnValue)
                {
                    ReceiptPrintTemplateListBL receiptPrintTemplateListBL = new ReceiptPrintTemplateListBL(executionContext);
                    List<KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>> receiptPrintTemplateSearchParams = new List<KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>>();
                    receiptPrintTemplateSearchParams.Add(new KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>(ReceiptPrintTemplateDTO.SearchByParameters.TEMPLATE_ID, receiptPrintTemplateHeaderDTO.TemplateId.ToString()));
                    if (loadActiveChildRecords)
                    {
                        receiptPrintTemplateSearchParams.Add(new KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>(ReceiptPrintTemplateDTO.SearchByParameters.IS_ACTIVE, "Y"));
                    }
                    receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList = receiptPrintTemplateListBL.GetReceiptPrintTemplateDTOList(receiptPrintTemplateSearchParams, sqlTransaction);
                    ReceiptPrintTemplateHeaderBuildBL receiptPrintTemplateBuildBL = new ReceiptPrintTemplateHeaderBuildBL(executionContext);
                    if (receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                    {
                        receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList = receiptPrintTemplateBuildBL.GetReceiptFont(receiptPrintTemplateHeaderDTO).ReceiptPrintTemplateDTOList;
                        receiptPrintTemplateHeaderDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public List<ReceiptPrintTemplateHeaderDTO> PopulateTemplate()
        {
            ReceiptPrintTemplateHeaderDataHandler receiptPrintTemplateHeaderDataHandler = new ReceiptPrintTemplateHeaderDataHandler(null);
            List<ReceiptPrintTemplateHeaderDTO> returnValue = receiptPrintTemplateHeaderDataHandler.PopulateTemplate();
            return returnValue;
        }
        /// <summary>
        /// For Management Studio, support Export option. 
        /// This will give query output in text file
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="templateName">templateName</param>
        /// <param name="duplicate">duplicate</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>query string</returns>
        public string GetExportQueries(int templateId, string templateName, bool duplicate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(templateId, templateName, duplicate, sqlTransaction);
            string finalQuery;
            ReceiptPrintTemplateHeaderDataHandler receiptPrintTemplateHeaderDataHandler = new ReceiptPrintTemplateHeaderDataHandler(sqlTransaction);
            finalQuery = receiptPrintTemplateHeaderDataHandler.GetExportQueries(templateId, templateName, duplicate, executionContext.GetUserId());
            log.LogMethodExit(finalQuery);
            return finalQuery;
        }

        /// <summary>
        /// This method should be used to Save and Update the ReceiptPrintTemplateHeader details for Web Management Studio.
        /// </summary>
        public void SaveUpdateReceiptPrintTemplateHeaderList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (receiptPrintTemplateHeaderDTOList != null)
            {
                foreach (ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDto in receiptPrintTemplateHeaderDTOList)
                {
                    try
                    {
                        ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(executionContext, receiptPrintTemplateHeaderDto);
                        receiptPrintTemplateHeaderBL.Save(sqlTransaction);
                    }
                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        log.Error("Error occurred while executing SaveUpdateReceiptPrintTemplateHeaderList()" + valEx.Message);
                        throw valEx;
                    }
                    catch (SqlException sqlEx)
                    {
                        log.Error(sqlEx);
                        log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                        if (sqlEx.Number == 2601)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872)); // cannot insert duplicate records
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit();
            }
        }
    }

    /// <summary>
    /// Functions within Receipt Print Template class
    /// </summary>
    public class ReceiptPrintTemplateHeaderBuildBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Common logic within ReceiptPrintTemplateHeader
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ReceiptPrintTemplateHeaderBuildBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets reciept Font
        /// </summary>
        /// <param name="receiptPrintTemplateHeaderDTO">receiptPrintTemplateHeaderDTO</param>
        /// <returns>receiptPrintTemplateHeaderDTO</returns>
        public ReceiptPrintTemplateHeaderDTO GetReceiptFont(ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO)
        {
            log.LogMethodEntry(receiptPrintTemplateHeaderDTO);
            Font ReceiptFont;
            Font defaultFont = new Font("Arial Narrow", 9);
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            foreach (ReceiptPrintTemplateDTO receiptPrintTemplateDTO in receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList)
            {
                try
                {
                    string font = receiptPrintTemplateDTO.FontName;
                    if (string.IsNullOrEmpty(font))
                        font = receiptPrintTemplateHeaderDTO.FontName;
                    if (string.IsNullOrEmpty(font))
                    {
                        if (receiptPrintTemplateDTO.FontSize.Equals(DBNull.Value))
                        {
                            if (receiptPrintTemplateHeaderDTO.FontSize.Equals(DBNull.Value))
                            {
                                ReceiptFont = defaultFont;
                            }
                            else
                            {
                                ReceiptFont = new Font(defaultFont.FontFamily, (float)Convert.ToDouble(receiptPrintTemplateHeaderDTO.FontSize));
                            }
                        }
                        else
                        {
                            ReceiptFont = new Font(defaultFont.FontFamily, (float)Convert.ToDouble(receiptPrintTemplateDTO.FontSize));
                        }
                    }
                    else
                    {
                        ReceiptFont = CustomFontConverter.ConvertStringToFont(executionContext, font);
                        if (!receiptPrintTemplateDTO.FontSize.Equals(DBNull.Value) && receiptPrintTemplateDTO.FontSize > -1)
                            ReceiptFont = new Font(ReceiptFont.FontFamily, (float)Convert.ToDouble(receiptPrintTemplateDTO.FontSize));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while calculating receipt font", ex);
                    ReceiptFont = defaultFont;
                }
                receiptPrintTemplateDTO.ReceiptFont = ReceiptFont;
                receiptPrintTemplateDTO.AcceptChanges();
            }
            receiptPrintTemplateHeaderDTO.AcceptChanges();
            log.LogMethodExit(receiptPrintTemplateHeaderDTO);
            return receiptPrintTemplateHeaderDTO;
        }
    }
}
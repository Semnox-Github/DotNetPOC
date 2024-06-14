/********************************************************************************************
 * Project Name - ProductExclusions BL
 * Description  - Buisness Logic class for ProductExclusion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.0      08-Mar-2019   Archana                 Created 
 *2.60.2      10-Jun-2019   Akshay Gulaganji        Code merge from Development to WebManagementStudio
 *2.70.0      29-June-2019  Jagan Mohana            Created DeletePOSproductExclusions() method.
 *2.80        10-Mar-2020   Vikas Dwivedi           Modified as per the Standards for RESTAPI Phase 1 changes.// Added DBAudit log for this entity
 *2.110.00    30-Nov-2020   Abhishek                Modified : Modified to 3 Tier Standard
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for POSProductExclusion class.
    /// </summary>
    public class POSProductExclusionsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private POSProductExclusionsDTO posProductExclusionsDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of POSProductExclusionsBL class
        /// </summary>
        private POSProductExclusionsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates POSProductExclusionsBL object using the posProductExclusionsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="posProductExclusionsDTO">POSProductExclusionsDTO object</param>
        public POSProductExclusionsBL(ExecutionContext executionContext, POSProductExclusionsDTO posProductExclusionsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posProductExclusionsDTO);
            this.posProductExclusionsDTO = posProductExclusionsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the exclusion id as the parameter
        /// Would fetch the posProductExclusionsDTO object from the database based on the exclusion id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public POSProductExclusionsBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            POSProductExclusionsDataHandler posProductExclusionsDataHandler = new POSProductExclusionsDataHandler(sqlTransaction);
            posProductExclusionsDTO = posProductExclusionsDataHandler.GetPOSProductExclusionsDTO(id);
            if (posProductExclusionsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " POS Product Exclusion ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductDisplayGroupFormat based on the DisplayGroup id.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            // load child records for - ProductDisplayGroupFormat
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchByProductDisplayGroupFormatParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchByProductDisplayGroupFormatParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_ID, posProductExclusionsDTO.ProductDisplayGroupFormatId.ToString()));
            if (activeChildRecords)
            {
                searchByProductDisplayGroupFormatParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, "1"));
            }
            posProductExclusionsDTO.PosProductDisplayGroupFormatList = productDisplayGroupList.GetOnlyUsedProductDisplayGroup(searchByProductDisplayGroupFormatParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the POSProductExclusions
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posProductExclusionsDTO.IsChangedRecursive == false
                 && posProductExclusionsDTO.ExclusionId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            POSProductExclusionsDataHandler posProductExclusionsDataHandler = new POSProductExclusionsDataHandler(sqlTransaction);
            if (posProductExclusionsDTO.IsActive)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, 14773);
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (posProductExclusionsDTO.ExclusionId < 0)
                {
                    log.LogVariableState("POSProductExclusionDTO", posProductExclusionsDTO);
                    posProductExclusionsDTO = posProductExclusionsDataHandler.Insert(posProductExclusionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    posProductExclusionsDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(posProductExclusionsDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("POSProductExclusions", posProductExclusionsDTO.Guid);
                    }
                }
                else if (posProductExclusionsDTO.IsChanged)
                {
                    log.LogVariableState("POSProductExclusionDTO", posProductExclusionsDTO);
                    posProductExclusionsDTO = posProductExclusionsDataHandler.Update(posProductExclusionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    posProductExclusionsDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(posProductExclusionsDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("POSProductExclusions", posProductExclusionsDTO.Guid);
                    }
                }
                SaveProductDisplayGroupFormat(sqlTransaction);
            }
            else
            {
                if (posProductExclusionsDTO.ExclusionId >= 0)
                {
                    posProductExclusionsDataHandler.Delete(posProductExclusionsDTO.ExclusionId);
                }
            }
            posProductExclusionsDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the child records : ProductDisplayGroupFormatDTOList 
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        private void SaveProductDisplayGroupFormat(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posProductExclusionsDTO.PosProductDisplayGroupFormatList != null &&
                posProductExclusionsDTO.PosProductDisplayGroupFormatList.Any())
            {
                List<ProductDisplayGroupFormatDTO> updatedProductDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
                foreach (var productDisplayGroupFormatDTO in posProductExclusionsDTO.PosProductDisplayGroupFormatList)
                {
                    if (productDisplayGroupFormatDTO.Id != posProductExclusionsDTO.ProductDisplayGroupFormatId)
                    {
                        productDisplayGroupFormatDTO.Id = posProductExclusionsDTO.ProductDisplayGroupFormatId;
                    }
                    if (productDisplayGroupFormatDTO.IsChanged)
                    {
                        updatedProductDisplayGroupFormatList.Add(productDisplayGroupFormatDTO);
                    }
                }
                if (updatedProductDisplayGroupFormatList.Any())
                {
                    ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext, updatedProductDisplayGroupFormatList);
                    productDisplayGroupList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the POSProductExclusionsDTO, ProductDisplayGroupFormatDTO - child only if saving is needed. 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(posProductExclusionsDTO.ExclusionId.ToString()))
            {
                validationErrorList.Add(new ValidationError("POSProductExclusions", "ExclusionId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Exclusion Id"))));
            }

            if (!string.IsNullOrWhiteSpace(posProductExclusionsDTO.ExclusionId.ToString()) && posProductExclusionsDTO.ExclusionId.ToString().Length > 50)
            {
                validationErrorList.Add(new ValidationError("POSProductExclusions", "ExclusionId", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Exclusion Id"), 50)));
            }
            //Use Only if validation before saving of child record  is needed.
            if (posProductExclusionsDTO.PosProductDisplayGroupFormatList != null)
            {
                foreach (var posProductDisplayGroupFormatDTO in posProductExclusionsDTO.PosProductDisplayGroupFormatList)
                {
                    if (posProductDisplayGroupFormatDTO.IsChanged)
                    {
                        ProductDisplayGroupFormat productDisplayGroupFormat = new ProductDisplayGroupFormat(executionContext, posProductDisplayGroupFormatDTO);
                        validationErrorList.AddRange(productDisplayGroupFormat.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public POSProductExclusionsDTO POSProductExclusionsDTO
        {
            get
            {
                return posProductExclusionsDTO;
            }
        }
    }


    /// <summary>
    /// Manages the list of Attraction Schedules
    /// </summary>
    public class posProductExclusionsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = new List<POSProductExclusionsDTO>();
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public posProductExclusionsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public posProductExclusionsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="pOSProductExclusionsDTOList"></param>
        public posProductExclusionsListBL(ExecutionContext executionContext, List<POSProductExclusionsDTO> pOSProductExclusionsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSProductExclusionsDTOList);
            this.pOSProductExclusionsDTOList = pOSProductExclusionsDTOList;
            log.LogMethodExit();
        }

        public List<POSProductExclusionsDTO> GetPOSProductExclusionDTOList(List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            POSProductExclusionsDataHandler posProductExclusionsDataHandler = new POSProductExclusionsDataHandler(sqlTransaction);
            List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = posProductExclusionsDataHandler.GetPOSProductExclusionsDTOList(searchParameters);
            log.LogMethodExit(pOSProductExclusionsDTOList);
            return pOSProductExclusionsDTOList;
        }

        /// <summary>
        /// GetComboProductDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<ComboProductDTO></returns>
        public List<POSProductExclusionsDTO> GetPOSProductExclusionDTOList(List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>> searchParameters,
                              bool loadChildRecords = false, bool loadActiveChildRecords = false,
                              SqlTransaction sqlTransaction = null)
        {
            // child records needs to be build
            log.LogMethodEntry(searchParameters, sqlTransaction);
            POSProductExclusionsDataHandler posProductExclusionsDataHandler = new POSProductExclusionsDataHandler(sqlTransaction);
            List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = posProductExclusionsDataHandler.GetPOSProductExclusionsDTOList(searchParameters);
            if (pOSProductExclusionsDTOList != null && pOSProductExclusionsDTOList.Any() && loadChildRecords)
            {
                Build(pOSProductExclusionsDTOList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(pOSProductExclusionsDTOList);
            return pOSProductExclusionsDTOList;
        }

        /// <summary>
        /// Builds the List of POSMachine object based on the list of POSMachine id.
        /// </summary>
        /// <param name="pOSMachineDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<POSProductExclusionsDTO> pOSProductExclusionsDTOList, bool activeChildRecords = true,
                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSProductExclusionsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, POSProductExclusionsDTO> posProductExclusionIdPOSProductExclusionDictionary = new Dictionary<int, POSProductExclusionsDTO>();
            string productDisplayGroupFormatIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < pOSProductExclusionsDTOList.Count; i++)
            {
                if (pOSProductExclusionsDTOList[i].ProductDisplayGroupFormatId == -1 ||
                    posProductExclusionIdPOSProductExclusionDictionary.ContainsKey(pOSProductExclusionsDTOList[i].ProductDisplayGroupFormatId))
                {
                    continue;
                }
                if (i != 0 && sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(pOSProductExclusionsDTOList[i].ProductDisplayGroupFormatId);
                posProductExclusionIdPOSProductExclusionDictionary.Add(pOSProductExclusionsDTOList[i].ExclusionId, pOSProductExclusionsDTOList[i]);
            }
            productDisplayGroupFormatIdSet = sb.ToString();

            // loads child records - ProductDisplayGroupFormat
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList();
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchByPosProductExclusionParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchByPosProductExclusionParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_FORMAT_ID_LIST, productDisplayGroupFormatIdSet.ToString()));
            if (activeChildRecords)
            {
                searchByPosProductExclusionParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, "1"));
            }
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList = productDisplayGroupList.GetAllProductDisplayGroup(searchByPosProductExclusionParams, true, activeChildRecords, sqlTransaction);
            if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Any())
            {
                log.LogVariableState("ProductDisplayGroupFormatDTOList", productDisplayGroupFormatDTOList);
                foreach (var productDisplayGroupFormatDTO in productDisplayGroupFormatDTOList)
                {
                    if (posProductExclusionIdPOSProductExclusionDictionary.ContainsKey(productDisplayGroupFormatDTO.Id))
                    {
                        if (posProductExclusionIdPOSProductExclusionDictionary[productDisplayGroupFormatDTO.Id].PosProductDisplayGroupFormatList == null)
                        {
                            posProductExclusionIdPOSProductExclusionDictionary[productDisplayGroupFormatDTO.Id].PosProductDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
                        }
                        posProductExclusionIdPOSProductExclusionDictionary[productDisplayGroupFormatDTO.Id].PosProductDisplayGroupFormatList.Add(productDisplayGroupFormatDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the POSProductExclusionBL List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSProductExclusionsDTOList == null ||
                pOSProductExclusionsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < pOSProductExclusionsDTOList.Count; i++)
            {
                var pOSProductExclusionDTO = pOSProductExclusionsDTOList[i];
                if (pOSProductExclusionDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    POSProductExclusionsBL pOSProductExclusionBL = new POSProductExclusionsBL(executionContext, pOSProductExclusionDTO);
                    pOSProductExclusionBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving POSProductExclusionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("POSProductExclusionDTO", pOSProductExclusionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the POSProductExclusionsDTO List for POSMachineIdList
        /// </summary>
        /// <param name="pOSMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of POSProductExclusionsDTO</returns>
        public List<POSProductExclusionsDTO> GetPOSProductExclusionsDTOList(List<int> pOSMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineIdList, activeRecords, sqlTransaction);
            POSProductExclusionsDataHandler pOSProductExclusionsDataHandler = new POSProductExclusionsDataHandler(sqlTransaction);
            List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = pOSProductExclusionsDataHandler.GetPOSProductExclusionsDTOList(pOSMachineIdList, activeRecords);
            log.LogMethodExit(pOSProductExclusionsDTOList);
            return pOSProductExclusionsDTOList;
        }

    }
}

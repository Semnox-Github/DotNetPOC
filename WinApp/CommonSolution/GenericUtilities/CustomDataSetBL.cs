/********************************************************************************************
 * Project Name - CustomDataSet BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-May-2017      Lakshminarayana     Created 
 *2.60.2      29-May-2019      Jagan Mohan         Code merge from Development to WebManagementStudio
 *2.70.2        26-Jul-2019      Dakshakh raj        Modified : Log method entries/exits
 ********************************************************************************************/

using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Business logic for CustomDataSet class.
    /// </summary>
    public class CustomDataSetBL
    {
        private CustomDataSetDTO customDataSetDTO;
        private readonly Semnox.Core.Utilities.ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Parameterized constructor of CustomDataSetBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CustomDataSetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            customDataSetDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customDataSet id as the parameter
        /// Would fetch the customDataSet object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomDataSetBL(ExecutionContext executionContext, int id, bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomDataSetDataHandler customDataSetDataHandler = new CustomDataSetDataHandler(sqlTransaction);
            customDataSetDTO = customDataSetDataHandler.GetCustomDataSetDTO(id);
            if (customDataSetDTO != null && loadChildRecords)
            {
                CustomDataSetBuilderBL customDataSetBuilderBL = new CustomDataSetBuilderBL(executionContext);
                customDataSetBuilderBL.Build(customDataSetDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomDataSetBL object using the CustomDataSetDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customDataSetDTO">CustomDataSetDTO object</param>
        public CustomDataSetBL(ExecutionContext executionContext, CustomDataSetDTO customDataSetDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(customDataSetDTO, executionContext);
            this.customDataSetDTO = customDataSetDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomDataSet
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (customDataSetDTO.IsChanged)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    log.LogMethodExit(null, "Validation failed : " + string.Join(", ", validationErrors.Select(x => x.Message)));
                    throw new ValidationException("Validation failed.", validationErrors);
                }
                CustomDataSetDataHandler customDataSetDataHandler = new CustomDataSetDataHandler(sqlTransaction);
                customDataSetDataHandler.Save(customDataSetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            if (customDataSetDTO.CustomDataDTOList == null ||
                customDataSetDTO.CustomDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "CustomDataDTOList is empty");
                return;
            }
            List<CustomDataDTO> updatedCustomDataDTOList = new List<CustomDataDTO>();
            for (int i = 0; i < customDataSetDTO.CustomDataDTOList.Count; i++)
            {
                if (customDataSetDTO.CustomDataDTOList[i].IsChanged == false)
                {
                    continue;
                }
                if (customDataSetDTO.CustomDataDTOList[i].CustomDataSetId != customDataSetDTO.CustomDataSetId)
                {
                    customDataSetDTO.CustomDataDTOList[i].CustomDataSetId = customDataSetDTO.CustomDataSetId;
                }
                updatedCustomDataDTOList.Add(customDataSetDTO.CustomDataDTOList[i]);
            }
            if (updatedCustomDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "updatedCustomDataDTOList is empty");
                return;
            }
            CustomDataListBL customDataListBL = new CustomDataListBL(executionContext, customDataSetDTO.CustomDataDTOList);
            customDataListBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the custom data dto for corresponding custom attributeId
        /// </summary>
        /// <param name="customAttributeId">customAttributeId</param>
        /// <returns></returns>
        public CustomDataDTO GetCustomDataDTO(int customAttributeId)
        {
            log.LogMethodEntry(customAttributeId);
            CustomDataDTO customDataDTO = null;
            if (customDataSetDTO.CustomDataDTOList != null)
            {
                customDataDTO = customDataSetDTO.CustomDataDTOList.FirstOrDefault(x => x.CustomAttributeId == customAttributeId);
            }
            log.LogMethodExit(customDataDTO);
            return customDataDTO;
        }

        /// <summary>
        /// Validates the custom dataset. if any of the field values not valid.
        /// </summary>
        /// <param name="customAttributesDTOMap">customAttributesDTOMap</param>
        /// <returns></returns>
        public List<ValidationError> Validate(Dictionary<int, CustomAttributesDTO> customAttributesDTOMap)
        {
            log.LogMethodEntry(customAttributesDTOMap);
            if (customAttributesDTOMap == null)
            {
                log.LogMethodExit(null, "Throwing Augument Exception");
                throw new ArgumentException("Invalid customAttributesDTOMap");
            }
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (customDataSetDTO == null ||
                customDataSetDTO.CustomDataDTOList == null ||
                customDataSetDTO.CustomDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "CustomDataDTOList is empty");
                return validationErrorList;
            }
            foreach (var customDataDTO in customDataSetDTO.CustomDataDTOList)
            {
                if (customDataDTO.IsChanged == false)
                {
                    continue;
                }
                if (customAttributesDTOMap.ContainsKey(customDataDTO.CustomAttributeId) == false)
                {
                    ValidationError validationError = new ValidationError("CustomData", "CustomAttributeId", MessageContainerList.GetMessage(executionContext, 1144, "CustomAttributeId"));
                    validationErrorList.Add(validationError);
                    continue;
                }
                CustomDataBL customDataBL = new CustomDataBL(executionContext, customDataDTO);
                validationErrorList.AddRange(customDataBL.Validate(customAttributesDTOMap[customDataDTO.CustomAttributeId]));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Validates the custom dataset.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (customDataSetDTO == null ||
                customDataSetDTO.CustomDataDTOList == null ||
                customDataSetDTO.CustomDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "CustomDataDTOList is empty");
                return validationErrorList;
            }
            foreach (var customDataDTO in customDataSetDTO.CustomDataDTOList)
            {
                CustomDataBL customDataBL = new CustomDataBL(executionContext, customDataDTO);
                validationErrorList.AddRange(customDataBL.Validate(sqlTransaction));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomDataSetDTO CustomDataSetDTO
        {
            get
            {
                return customDataSetDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of CustomDataSet
    /// </summary>
    public class CustomDataSetListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        private readonly List<CustomDataSetDTO> customDataSetDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomDataSetListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="customDataSetDTOList">customDataSetDTOList</param>
        public CustomDataSetListBL(ExecutionContext executionContext, List<CustomDataSetDTO> customDataSetDTOList)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customDataSetDTOList = customDataSetDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the CustomDataSet list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<CustomDataSetDTO> GetCustomDataSetDTOList(List<KeyValuePair<CustomDataSetDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomDataSetDataHandler CustomDataSetDataHandler = new CustomDataSetDataHandler(sqlTransaction);
            List<CustomDataSetDTO> result = CustomDataSetDataHandler.GetCustomDataSetDTOList(searchParameters);
            if (loadChildRecords == false ||
                result == null ||
                result.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            CustomDataSetBuilderBL CustomDataSetBuilder = new CustomDataSetBuilderBL(executionContext);
            CustomDataSetBuilder.Build(result, activeChildRecords, sqlTransaction);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CustomDataSetDTO List maching primary keys with customDataSetIdList
        /// </summary>
        /// <param name="customDataSetIdList">customDataSetIdList</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<CustomDataSetDTO> GetCustomDataSetDTOList(List<int> customDataSetIdList,
                                                              bool loadChildRecords = false,
                                                              bool activeChildRecords = true,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customDataSetIdList, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomDataSetDataHandler CustomDataSetDataHandler = new CustomDataSetDataHandler(sqlTransaction);
            List<CustomDataSetDTO> result = CustomDataSetDataHandler.GetCustomDataSetDTOList(customDataSetIdList);
            if (loadChildRecords == false ||
                result == null ||
                result.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            CustomDataSetBuilderBL CustomDataSetBuilder = new CustomDataSetBuilderBL(executionContext);
            CustomDataSetBuilder.Build(result, activeChildRecords, sqlTransaction);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates and saves the customDataSetDTOList
        /// </summary>
        /// <param name="customAttributesDTOMap">customAttributesDTOMap</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(Dictionary<int, CustomAttributesDTO> customAttributesDTOMap, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction, customAttributesDTOMap);
            if (customDataSetDTOList == null ||
                customDataSetDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            for (int i = 0; i < customDataSetDTOList.Count; i++)
            {
                if (customDataSetDTOList[i].IsChangedRecursive == false)
                {
                    continue;
                }
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customDataSetDTOList[i]);
                List<ValidationError> validationErrors = customDataSetBL.Validate(customAttributesDTOMap);
                if (validationErrors.Any())
                {
                    log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException("Validation failed for CustomDataSet.", validationErrors, i);
                }
            }
            Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// saves the validated customDataSetDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customDataSetDTOList == null ||
                customDataSetDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<CustomDataSetDTO> updatedCustomDataSetDTOList = new List<CustomDataSetDTO>(customDataSetDTOList.Count);
            for (int i = 0; i < customDataSetDTOList.Count; i++)
            {
                /*
                 * Custom Data Set DTO will always be a child of a aggregate.
                 * Validation should be handled by the aggregate.
                 * Validation code is remove from here for duplication
                 */
                //if (customDataSetDTOList[i].IsChangedRecursive == false)
                //{
                //    continue;
                //}
                //if(validate)
                //{
                //    CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customDataSetDTOList[i]);
                //    List<ValidationError> validationErrors = customDataSetBL.Validate(customAttributesDTOMap);
                //    if (validationErrors.Any())
                //    {
                //        log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                //        throw new ValidationException("Validation failed for CustomDataSet.", validationErrors, i);
                //    }
                //}
                if (customDataSetDTOList[i].IsChanged == false)
                {
                    continue;
                }
                updatedCustomDataSetDTOList.Add(customDataSetDTOList[i]);
            }
            if (updatedCustomDataSetDTOList.Any())
            {
                CustomDataSetDataHandler customDataSetDataHandler = new CustomDataSetDataHandler(sqlTransaction);
                customDataSetDataHandler.Save(updatedCustomDataSetDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            List<CustomDataDTO> updatedCustomDataDTOList = new List<CustomDataDTO>();
            for (int i = 0; i < customDataSetDTOList.Count; i++)
            {
                CustomDataSetDTO customDataSetDTO = customDataSetDTOList[i];
                if (customDataSetDTO.CustomDataDTOList == null ||
                    customDataSetDTO.CustomDataDTOList.Any() == false)
                {
                    continue;
                }
                for (int j = 0; j < customDataSetDTO.CustomDataDTOList.Count; j++)
                {
                    CustomDataDTO customDataDTO = customDataSetDTO.CustomDataDTOList[j];
                    if (customDataDTO.IsChanged == false)
                    {
                        continue;
                    }
                    if (customDataDTO.CustomDataSetId != customDataSetDTO.CustomDataSetId)
                    {
                        customDataDTO.CustomDataSetId = customDataSetDTO.CustomDataSetId;
                    }
                    updatedCustomDataDTOList.Add(customDataDTO);
                }
            }
            if (updatedCustomDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "updatedCustomDataDTOList is empty");
                return;
            }
            CustomDataListBL customDataListBL = new CustomDataListBL(executionContext, updatedCustomDataDTOList);
            customDataListBL.Save(sqlTransaction);
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Builds the complex CustomDataSet entity structure
    /// </summary>
    public class CustomDataSetBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomDataSetBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the complex CustomDataSet DTO structure
        /// </summary>
        /// <param name="customDataSetDTO">CustomDataSet dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(CustomDataSetDTO customDataSetDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customDataSetDTO, activeChildRecords, sqlTransaction);
            if (customDataSetDTO != null && customDataSetDTO.CustomDataSetId != -1)
            {
                CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
                List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, customDataSetDTO.CustomDataSetId.ToString()));
                customDataSetDTO.CustomDataDTOList = customDataListBL.GetCustomDataDTOList(searchParameters, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the complex CustomDataSetDTO structure
        /// </summary>
        /// <param name="customDataSetDTOList">CustomDataSet dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<CustomDataSetDTO> customDataSetDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customDataSetDTOList, activeChildRecords, sqlTransaction);
            if (customDataSetDTOList != null &&
                customDataSetDTOList.Any() == false)
            {
                log.LogMethodExit(null, "customDataSetDTOList is empty");
                return;
            }
            Dictionary<int, CustomDataSetDTO> customDataSetDictionary = new Dictionary<int, CustomDataSetDTO>();
            List<int> customDataSetIdList = new List<int>();
            for (int i = 0; i < customDataSetDTOList.Count; i++)
            {
                if (customDataSetDTOList[i].CustomDataSetId != -1 &&
                    customDataSetDictionary.ContainsKey(customDataSetDTOList[i].CustomDataSetId) == false)
                {
                    customDataSetDictionary.Add(customDataSetDTOList[i].CustomDataSetId, customDataSetDTOList[i]);
                    customDataSetIdList.Add(customDataSetDTOList[i].CustomDataSetId);
                }
            }
            CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
            List<CustomDataDTO> customDataDTOList = customDataListBL.GetCustomDataDTOListOfCustomDataSets(customDataSetIdList, sqlTransaction);
            if (customDataDTOList == null ||
                customDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "customDataDTOList empty");
                return;
            }
            for (int i = 0; i < customDataDTOList.Count; i++)
            {
                if (customDataSetDictionary.ContainsKey(customDataDTOList[i].CustomDataSetId) == false)
                {
                    continue;
                }
                CustomDataSetDTO customDataSetDTO = customDataSetDictionary[customDataDTOList[i].CustomDataSetId];
                if (customDataSetDTO.CustomDataDTOList == null)
                {
                    customDataSetDTO.CustomDataDTOList = new List<CustomDataDTO>();
                }
                customDataSetDTO.CustomDataDTOList.Add(customDataDTOList[i]);
            }
            log.LogMethodExit();
        }
    }

}

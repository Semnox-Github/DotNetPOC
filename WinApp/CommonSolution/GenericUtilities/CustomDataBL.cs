/********************************************************************************************
 * Project Name - CustomData BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *1.00        17-May-2017      Lakshminarayana     Created 
 *2.60.2      29-May-2019      Jagan Mohan         Code merge from Development to WebManagementStudio
 *2.70.2        25-Jul-2019      Dakshakh raj        Modified : Log method entries/exits
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
    /// Business logic for CustomData class.
    /// </summary>
    public class CustomDataBL
    {
        private CustomDataDTO customDataDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomDataBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CustomDataBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            customDataDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customData id as the parameter
        /// Would fetch the customData object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomDataBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomDataDataHandler customDataDataHandler = new CustomDataDataHandler(sqlTransaction);
            customDataDTO = customDataDataHandler.GetCustomDataDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomDataBL object using the CustomDataDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customDataDTO">CustomDataDTO object</param>
        public CustomDataBL(ExecutionContext executionContext, CustomDataDTO customDataDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customDataDTO);
            this.customDataDTO = customDataDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomData
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (customDataDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "customDataDTO is not changed.");
                return;
            }
            CustomDataDataHandler customDataDataHandler = new CustomDataDataHandler(sqlTransaction);
            customDataDataHandler.Save(customDataDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomDataDTO CustomDataDTO
        {
            get
            {
                return customDataDTO;
            }
        }

        /// <summary>
        /// Validates the custom data. if any of the field values are not valid returns a list of ValidationErrors.
        /// </summary>
        /// <param name="customAttributesDTO">customAttributesDTO</param>
        /// <returns></returns>
        public List<ValidationError> Validate(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            bool validationError = false;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, customAttributesDTO.Name) == "M")
            {
                switch (customAttributesDTO.Type)
                {
                    case "TEXT":
                        {
                            if (string.IsNullOrWhiteSpace(customDataDTO.CustomDataText))
                            {
                                validationError = true;
                            }
                            break;
                        }
                    case "NUMBER":
                        {
                            if (customDataDTO.CustomDataNumber == null)
                            {
                                validationError = true;
                            }
                            break;
                        }
                    case "DATE":
                        {
                            if (customDataDTO.CustomDataDate == null)
                            {
                                validationError = true;
                            }
                            break;
                        }
                    case "LIST":
                        {
                            if (customDataDTO.ValueId == -1)
                            {
                                validationError = true;
                            }
                            break;
                        }
                }
                if (validationError)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, customAttributesDTO.Name);
                    validationErrorList.Add(new ValidationError("CustomData", customAttributesDTO.Name, errorMessage));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Validates the custom data. throws validation exception if any of the field values not not valid.
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList;
            if (customDataDTO.CustomAttributeId < 0)
            {
                validationErrorList = new List<ValidationError>() { new ValidationError("CustomData", "CustomAttributeId", MessageContainerList.GetMessage(executionContext, 1144, "CustomAttributeId")) };
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            CustomAttributesBL customAttributesBL = new CustomAttributesBL(executionContext, customDataDTO.CustomAttributeId);
            validationErrorList = Validate(customAttributesBL.CustomAttributesDTO);
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of CustomData
    /// </summary>
    public class CustomDataListBL
    {
        private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<CustomDataDTO> customDataDTOList;

        public CustomDataListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomDataListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="customDataDTOList">customDataDTOList</param>
        public CustomDataListBL(ExecutionContext executionContext, List<CustomDataDTO> customDataDTOList)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customDataDTOList = customDataDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CustomData list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<CustomDataDTO> GetCustomDataDTOList(List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomDataDataHandler customDataDataHandler = new CustomDataDataHandler(sqlTransaction);
            List<CustomDataDTO> returnValue = customDataDataHandler.GetCustomDataDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the CustomDataDTO List for CustomDataSet Id List
        /// </summary>
        /// <param name="customDataSetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CustomDataSetDTO</returns>
        public List<CustomDataDTO> GetCustomDataDTOListOfCustomDataSets(List<int> customDataSetIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customDataSetIdList, sqlTransaction);
            CustomDataDataHandler customDataDataHandler = new CustomDataDataHandler(sqlTransaction);
            List<CustomDataDTO> list = customDataDataHandler.GetCustomDataDTOListOfCustomDataSets(customDataSetIdList);
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the CustomDataViewDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<CustomDataViewDTO> GetCustomDataViewDTOList(List<KeyValuePair<CustomDataViewDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomDataDataHandler customDataDataHandler = new CustomDataDataHandler(sqlTransaction);
            List<CustomDataViewDTO> customDataViewDTOList = customDataDataHandler.GetCustomDataViewDTOList(searchParameters);
            log.LogMethodExit(customDataViewDTOList);
            return customDataViewDTOList;
        }

        /// <summary>
        /// Validates and saves the customDataDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customDataDTOList == null ||
                customDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            /*
                CustomDataDTO should not be accessed directly. It should be accessed through
                CustomDataSet.
                Removing below check because same check is done in the CustomDataSetList Save and CustomDataSet Save.
            */
            //List<CustomDataDTO> updatedCustomDataDTOList = new List<CustomDataDTO>(customDataDTOList.Count);
            //for (int i = 0; i < customDataDTOList.Count; i++)
            //{
            //    if (customDataDTOList[i].IsChanged == false)
            //    {
            //        continue;
            //    }
            //    updatedCustomDataDTOList.Add(customDataDTOList[i]);
            //}
            //if (updatedCustomDataDTOList.Any() == false)
            //{
            //    log.LogMethodExit(null, "List is not changed.");
            //    return;
            //}
            CustomDataDataHandler customDataDataHandler = new CustomDataDataHandler(sqlTransaction);
            customDataDataHandler.Save(customDataDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}

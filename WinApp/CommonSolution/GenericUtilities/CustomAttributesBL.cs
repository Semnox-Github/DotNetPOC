/********************************************************************************************
 * Project Name - CustomAttributes BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-May-2017      Lakshminarayana     Created 
 *2.60        08-Mar-2019      Jagan Mohana        Created method SaveUpdateCustomAttributesList()
 *2.60        26-Apr-2019      Mushahid Faizan     Modified try-catch block in SaveUpdateCustomAttributesList().
 *2.70.2        25-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *            02-Aug-2019      Mushahid Faizan     Added delete in Save() method for Hard-deletion.
 *            19-Dec-2019      Akshay G            modified GetCustomAttributesDTOMap()
 *2.160.0     03-Mar-2023      Deeksha             Added SaveUpdateCustomerCustomAttribute Logic
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Business logic for CustomAttributes class.
    /// </summary>
    public class CustomAttributesBL
    {
        private CustomAttributesDTO customAttributesDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of CustomAttributesBL class
        /// </summary>
        public CustomAttributesBL() : this(ExecutionContext.GetExecutionContext())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of CustomAttributesBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CustomAttributesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            customAttributesDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customAttributes id as the parameter
        /// Would fetch the customAttributes object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomAttributesBL(int id, bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(ExecutionContext.GetExecutionContext())
        {
            log.LogMethodEntry(id, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomAttributesDataHandler customAttributesDataHandler = new CustomAttributesDataHandler(sqlTransaction);
            customAttributesDTO = customAttributesDataHandler.GetCustomAttributesDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customAttributes id as the parameter
        /// Would fetch the customAttributes object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomAttributesBL(ExecutionContext executionContext, int id, bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomAttributesDataHandler customAttributesDataHandler = new CustomAttributesDataHandler(sqlTransaction);
            customAttributesDTO = customAttributesDataHandler.GetCustomAttributesDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomAttributesBL object using the CustomAttributesDTO
        /// </summary>
        /// <param name="customAttributesDTO">CustomAttributesDTO object</param>
        public CustomAttributesBL(CustomAttributesDTO customAttributesDTO)
            : this(ExecutionContext.GetExecutionContext())
        {
            log.LogMethodEntry(customAttributesDTO);
            this.customAttributesDTO = customAttributesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomAttributesBL object using the CustomAttributesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customAttributesDTO">CustomAttributesDTO object</param>
        public CustomAttributesBL(ExecutionContext executionContext, CustomAttributesDTO customAttributesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customAttributesDTO);
            this.customAttributesDTO = customAttributesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// returns whether it is a boolean list
        /// </summary>
        /// <returns></returns>
        public bool IsBoolenList()
        {
            bool result = false;
            if (customAttributesDTO.Type == "LIST")
            {
                if (customAttributesDTO.CustomAttributeValueListDTOList.Count == 2 &&
                (customAttributesDTO.CustomAttributeValueListDTOList[0].Value == "0" ||
                customAttributesDTO.CustomAttributeValueListDTOList[0].Value == "1"))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// returns the value id for a given value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetValueId(string value)
        {
            int valueId = -1;
            if (customAttributesDTO.CustomAttributeValueListDTOList != null &&
               customAttributesDTO.CustomAttributeValueListDTOList.Count > 0)
            {
                if (IsBoolenList())
                {
                    if (value == "Y")
                    {
                        value = "1";
                    }
                    else if (value == "N")
                    {
                        value = "0";
                    }
                }
                foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                {
                    if (customAttributeValueListDTO.Value == value)
                    {
                        valueId = customAttributeValueListDTO.ValueId;
                        break;
                    }
                }
            }
            return valueId;
        }

        /// <summary>
        /// returns the value for the given valueId
        /// </summary>
        /// <param name="valueId">valueId</param>
        /// <returns></returns>
        public string GetValue(int valueId)
        {
            string value = string.Empty;
            if (customAttributesDTO.CustomAttributeValueListDTOList != null &&
               customAttributesDTO.CustomAttributeValueListDTOList.Count > 0)
            {
                foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                {
                    if (customAttributeValueListDTO.ValueId == valueId)
                    {
                        value = customAttributeValueListDTO.Value;
                        break;
                    }
                }
                if (IsBoolenList())
                {
                    if (value == "1")
                    {
                        value = "Y";
                    }
                    else if (value == "0")
                    {
                        value = "N";
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Saves the CustomAttributes
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomAttributesDataHandler customAttributesDataHandler = new CustomAttributesDataHandler(sqlTransaction);
            SaveUpdateCustomerCustomAttribute(sqlTransaction);
            if (customAttributesDTO.CustomAttributeId < 0)
            {
                customAttributesDTO = customAttributesDataHandler.InsertCustomAttributes(customAttributesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customAttributesDTO.AcceptChanges();
            }
            else
            {
                if (customAttributesDTO.IsChanged)
                {
                    customAttributesDTO = customAttributesDataHandler.UpdateCustomAttributes(customAttributesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customAttributesDTO.AcceptChanges();
                }
            }
            if (customAttributesDTO.CustomAttributeValueListDTOList != null)
            {
                foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                {
                    if (customAttributeValueListDTO.IsChanged)
                    {
                        customAttributeValueListDTO.CustomAttributeId = customAttributesDTO.CustomAttributeId;
                        CustomAttributeValueListBL customAttributeValueListBL = new CustomAttributeValueListBL(executionContext, customAttributeValueListDTO);
                        customAttributeValueListBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SaveUpdateCustomerCustomAttribute(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customAttributesDTO.Applicability == "CUSTOMER")
            {
                CustomAttributesBL exsistingCustomAttributesBL = new CustomAttributesBL(executionContext, customAttributesDTO.CustomAttributeId);
                CustomAttributesDTO existingCustomAttributeDTO = exsistingCustomAttributesBL.CustomAttributesDTO;
                ParafaitDefaultsDTO parafaitDefaultsDTO = null;
                ParafaitDefaultsBL parafaitDefaultsBL = null;
                if (existingCustomAttributeDTO!= null && existingCustomAttributeDTO.Name != customAttributesDTO.Name)
                {
                    parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, existingCustomAttributeDTO.Name);
                }
                else
                {
                    parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, customAttributesDTO.Name);
                }
                if (parafaitDefaultsBL != null)
                {
                    List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>(DefaultDataTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                    searchParameters.Add(new KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>(DefaultDataTypeDTO.SearchByParameters.DATA_TYPE, "CustomFieldDisplayOptions"));
                    DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext);
                    List<DefaultDataTypeDTO> defaultDataTypeDTOList = defaultDataTypeListBL.GetDefaultDataTypeValues(searchParameters);
                    if (parafaitDefaultsBL.ParafaitDefaultsDTO == null && defaultDataTypeDTOList != null && defaultDataTypeDTOList.Any())
                    {
                        parafaitDefaultsDTO = new ParafaitDefaultsDTO(-1, customAttributesDTO.Name, customAttributesDTO.Name,
                                                            "O", "Customer", defaultDataTypeDTOList[0].DatatypeId, "N", "N", "N", true);
                    }
                    else
                    {
                        parafaitDefaultsDTO = parafaitDefaultsBL.ParafaitDefaultsDTO;
                        parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValueName = customAttributesDTO.Name;
                        parafaitDefaultsBL.ParafaitDefaultsDTO.Description = customAttributesDTO.Name;
                        parafaitDefaultsBL.ParafaitDefaultsDTO.IsActive = customAttributesDTO.IsActive;
                    }
                    ParafaitDefaultsBL SaveparafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, parafaitDefaultsDTO);
                    SaveparafaitDefaultsBL.Save(sqlTransaction);
                }
                if (existingCustomAttributeDTO != null && existingCustomAttributeDTO.IsActive && !customAttributesDTO.IsActive)
                {
                    parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, customAttributesDTO.Name);
                    parafaitDefaultsBL.ParafaitDefaultsDTO.IsActive = false;
                    if (parafaitDefaultsBL.ParafaitDefaultsDTO.ParafaitOptionValuesDTOList.Any())
                    {
                        List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList = parafaitDefaultsBL.ParafaitDefaultsDTO.ParafaitOptionValuesDTOList.Select(x => { x.IsActive = false; return x; }).ToList();
                        parafaitDefaultsBL.ParafaitDefaultsDTO.ParafaitOptionValuesDTOList = parafaitOptionValuesDTOList;
                    }
                    ParafaitDefaultsBL parafaitDefaultsBLSave = new ParafaitDefaultsBL(executionContext, parafaitDefaultsBL.ParafaitDefaultsDTO);
                    parafaitDefaultsBLSave.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomAttributesDTO CustomAttributesDTO
        {
            get
            {
                return customAttributesDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of CustomAttributes
    /// </summary>
    public class CustomAttributesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomAttributesDTO> customAttributesDTOList;

        /// <summary>
        /// Default constroctor
        /// </summary>
        public CustomAttributesListBL()
        {
            log.LogMethodEntry();
            executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomAttributesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customAttributesDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customAttributesDTOList">customAttributesDTOList</param>
        public CustomAttributesListBL(ExecutionContext executionContext, List<CustomAttributesDTO> customAttributesDTOList)
        {
            log.LogMethodEntry(executionContext, customAttributesDTOList);
            this.customAttributesDTOList = customAttributesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CustomAttributes list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>CustomAttributes list</returns>
        public List<CustomAttributesDTO> GetCustomAttributesDTOList(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomAttributesDataHandler CustomAttributesDataHandler = new CustomAttributesDataHandler(sqlTransaction);
            List<CustomAttributesDTO> CustomAttributesDTOList = CustomAttributesDataHandler.GetCustomAttributesDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (CustomAttributesDTOList != null && CustomAttributesDTOList.Count > 0)
                {
                    CustomAttributesBuilderBL CustomAttributesBuilder = new CustomAttributesBuilderBL(executionContext);
                    CustomAttributesBuilder.Build(CustomAttributesDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(CustomAttributesDTOList);
            return CustomAttributesDTOList;
        }

        /// <summary>
        /// Returns the map of the custom attributes for the given applicability and site
        /// </summary>
        /// <param name="applicability">applicability</param>
        /// <returns>Returns the map</returns>
        public Dictionary<int, CustomAttributesDTO> GetCustomAttributesDTOMap(Applicability applicability)
        {
            log.LogMethodEntry(applicability);
            Dictionary<int, CustomAttributesDTO> result = new Dictionary<int, CustomAttributesDTO>();
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParametes = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
            searchParametes.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, applicability.ToString()));
            searchParametes.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CustomAttributesDTO> customAttributesDTOs = customAttributesListBL.GetCustomAttributesDTOList(searchParametes, true);
            if (customAttributesDTOs == null ||
                customAttributesDTOs.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            for (int i = 0; i < customAttributesDTOs.Count; i++)
            {
                if (result.ContainsKey(customAttributesDTOs[i].CustomAttributeId))
                {
                    continue;
                }
                result.Add(customAttributesDTOs[i].CustomAttributeId, customAttributesDTOs[i]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Save or update the custom attributes
        /// </summary>
        public void SaveUpdateCustomAttributesList()
        {
            log.LogMethodEntry();
            try
            {
                if (customAttributesDTOList != null && customAttributesDTOList.Count > 0)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (CustomAttributesDTO customAttributesDTO in customAttributesDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                CustomAttributesBL customAttributesBL = new CustomAttributesBL(executionContext, customAttributesDTO);
                                customAttributesBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        internal DateTime? GetcustomAttributesLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomAttributesDataHandler customAttributesDataHandler = new CustomAttributesDataHandler(sqlTransaction);
            DateTime? result = customAttributesDataHandler.GetcustomAttributesLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }

    /// <summary>
    /// Builds the complex CustomAttributes entity structure
    /// </summary>
    public class CustomAttributesBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomAttributesBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the complex CustomAttributes DTO structure
        /// </summary>
        /// <param name="customAttributesDTO">CustomAttributes dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(CustomAttributesDTO customAttributesDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customAttributesDTO, activeChildRecords, sqlTransaction);
            if (customAttributesDTO != null && customAttributesDTO.CustomAttributeId != -1)
            {
                CustomAttributeValueListListBL customAttributeValueListListBL = new CustomAttributeValueListListBL(executionContext);
                List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>(CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, customAttributesDTO.CustomAttributeId.ToString()));
                customAttributesDTO.CustomAttributeValueListDTOList = customAttributeValueListListBL.GetCustomAttributeValueListDTOList(searchParameters);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the complex CustomAttributesDTO structure
        /// </summary>
        /// <param name="customAttributesDTOList">CustomAttributes dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<CustomAttributesDTO> customAttributesDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customAttributesDTOList, activeChildRecords, sqlTransaction);
            if (customAttributesDTOList != null && customAttributesDTOList.Count > 0)
            {
                StringBuilder customAttributeIdListStringBuilder = new StringBuilder("");
                Dictionary<int, CustomAttributesDTO> customAttributesDictionary = new Dictionary<int, CustomAttributesDTO>();
                string customAttributeIdList;
                for (int i = 0; i < customAttributesDTOList.Count; i++)
                {
                    if (customAttributesDTOList[i].CustomAttributeId != -1)
                    {
                        customAttributesDictionary.Add(customAttributesDTOList[i].CustomAttributeId, customAttributesDTOList[i]);
                    }

                    if (i != 0)
                    {
                        customAttributeIdListStringBuilder.Append(",");
                    }
                    customAttributeIdListStringBuilder.Append(customAttributesDTOList[i].CustomAttributeId.ToString());
                }
                customAttributeIdList = customAttributeIdListStringBuilder.ToString();

                CustomAttributeValueListListBL customAttributeValueListListBL = new CustomAttributeValueListListBL(executionContext);
                List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>(CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID_LIST, customAttributeIdList));
                List<CustomAttributeValueListDTO> customAttributeValueListDTOList = customAttributeValueListListBL.GetCustomAttributeValueListDTOList(searchParameters);
                if (customAttributeValueListDTOList != null)
                {
                    foreach (var customAttributeValueListDTO in customAttributeValueListDTOList)
                    {
                        if (customAttributesDictionary.ContainsKey(customAttributeValueListDTO.CustomAttributeId))
                        {
                            CustomAttributesDTO customAttributesDTO = customAttributesDictionary[customAttributeValueListDTO.CustomAttributeId];
                            if (customAttributesDTO.CustomAttributeValueListDTOList == null)
                            {
                                customAttributesDTO.CustomAttributeValueListDTOList = new List<CustomAttributeValueListDTO>();
                            }
                            customAttributesDTO.CustomAttributeValueListDTOList.Add(customAttributeValueListDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }

}

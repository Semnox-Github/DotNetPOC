/********************************************************************************************
 * Project Name - ParafaitOptionValuesCustomProperties DTO
 * Description  - Data object of Parafaitoptionvalues
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        14-Mar-2019   Muhammed Mehraj         Created 
 *2.60        02-Apr-2019   Jagan Mohana            Created the ParafaitOptionValuesDTO method
 *2.60        30-Apr-2019   Mushahid Faizan         Created the parafaitDefaultsDTO,GetDefaultCustomPropertiesDTO and
                                                    GetParafaitConfigurationValues,SaveUpdateParafaitConfigurationValueList method
 *2.60        02-May-2019   Akshay Gulaganji        modified GetParafaitConfigurationValues(), SaveUpdateParafaitValueList(),ParafaitOptionValuesDTO(),GetCustomPropertiesDTO(),
 *                                                  created ToNormalizationText() method and added Encryptions for EncryptionPassword
 *2.70.3      30-Mar-2020   Girish Kundar           Modified : GetCustomPropertiesDTO() method to encrypt the value if type is 'EncryptPassword'                                                  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Semnox.Core.GenericUtilities
{
    public class ParafaitOptionValuesCustomBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ParafaitOptionValuesCustomBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Retrun the parafait option values custom list 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>List ParafaitOptionValuesCustomPropertiesDTO</returns>

        public List<ParafaitOptionValuesCustomPropertiesDTO> GetParafaitOptionValues(List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesCustomPropertiesList = new List<ParafaitOptionValuesCustomPropertiesDTO>();
            try
            {
                ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext);
                List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList = parafaitOptionValuesListBL.GetParafaitOptionValuesDTOList(searchParameters);
                if (parafaitOptionValuesDTOList != null)
                {
                    foreach (ParafaitOptionValuesDTO parafaitOptionValuesDTO in parafaitOptionValuesDTOList)
                    {
                        List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(Utilities.ParafaitDefaultsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(Utilities.ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_ID, Convert.ToString(parafaitOptionValuesDTO.OptionId)));
                        ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                        List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchByParameters);
                        if (parafaitDefaultsDTOList != null)
                        {
                            foreach (ParafaitDefaultsDTO parafaitDefaultsDTO in parafaitDefaultsDTOList)
                            {
                                /// It will check the data type of option value and return the specific type whether SQL/Custom/Flag/File/Folder/Text/Hour/Number/Decimal/Password/EncryptedPassword
                                DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(executionContext);
                                DataTable dTable = defaultDataTypeBL.GetDataType(Convert.ToString(parafaitDefaultsDTO.DataTypeId));
                                string dataType = dTable.Rows[0]["datatype"].ToString();
                                if (dataType.StartsWith("Custom", StringComparison.CurrentCultureIgnoreCase))
                                    dataType = "Custom";
                                else if (dataType.StartsWith("SQL", StringComparison.CurrentCultureIgnoreCase))
                                    dataType = "SQL";

                                ParafaitOptionValuesCustomPropertiesDTO parafaitOptionCustomPropertiesValues = new ParafaitOptionValuesCustomPropertiesDTO();
                                parafaitOptionCustomPropertiesValues.Items = new List<CommonLookupDTO>();

                                /// Based on the data type, get the option values dto and convert the object in to ParafaitOptionValuesCustomPropertiesDTO object 
                                switch (dataType)
                                {

                                    case "Text":
                                    case "Password":
                                    case "Folder":
                                    case "File":
                                    case "Flag":
                                    case "Decimal":
                                    case "Number":
                                        {
                                            parafaitOptionCustomPropertiesValues = GetCustomPropertiesDTO(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
                                        }
                                        break;
                                    case "EncryptedPassword":
                                        {
                                            parafaitOptionCustomPropertiesValues = GetCustomPropertiesDTO(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
                                            parafaitOptionCustomPropertiesValues.OptionValue = Encryption.Decrypt(parafaitOptionCustomPropertiesValues.OptionValue);
                                            parafaitOptionCustomPropertiesValues.AcceptChanges();
                                        }
                                        break;
                                    case "Hour":
                                        {
                                            parafaitOptionCustomPropertiesValues = GetCustomPropertiesDTO(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
                                            string hourNbr = string.Empty;
                                            string ampm = string.Empty;
                                            string hour = string.Empty;
                                            for (int j = 0; j < 24; j++)
                                            {
                                                if (j < 12)
                                                {
                                                    ampm = "AM";
                                                }
                                                else
                                                {
                                                    ampm = "PM";
                                                }

                                                if (j <= 12)
                                                {
                                                    hour = j.ToString();
                                                }
                                                else
                                                {
                                                    hour = (j - 12).ToString();
                                                }
                                                hourNbr = j.ToString();

                                                if (hour == "0")
                                                {
                                                    CommonLookupDTO lookupDataObject;
                                                    lookupDataObject = new CommonLookupDTO(hourNbr, "12:00 " + ampm);
                                                    parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                                }
                                                else
                                                {
                                                    CommonLookupDTO lookupDataObject;
                                                    lookupDataObject = new CommonLookupDTO(hourNbr, hour + ":00 " + ampm);
                                                    parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                                }
                                            }
                                        }
                                        break;
                                    case "SQL":
                                        {
                                            parafaitOptionCustomPropertiesValues = GetCustomPropertiesDTO(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
                                            var dataTypes = defaultDataTypeBL.FetchValues(dTable.Rows[0]["datatype"].ToString());
                                            if (dataTypes.Count > 0)
                                            {
                                                foreach (var item in dataTypes)
                                                {
                                                    CommonLookupDTO lookupDataObject;
                                                    lookupDataObject = new CommonLookupDTO(item.Key, item.Value);
                                                    parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                                }
                                            }
                                        }
                                        break;
                                    case "Custom":
                                        {
                                            parafaitOptionCustomPropertiesValues = GetCustomPropertiesDTO(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
                                            var dataTypes = defaultDataTypeBL.FetchValues(dTable.Rows[0]["datatype"].ToString());
                                            if (dataTypes.Count > 0)
                                            {
                                                foreach (var item in dataTypes)
                                                {
                                                    CommonLookupDTO lookupDataObject;
                                                    lookupDataObject = new CommonLookupDTO(item.Key, item.Value);
                                                    parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            parafaitOptionCustomPropertiesValues = GetCustomPropertiesDTO(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
                                        }
                                        break;

                                }
                                parafaitOptionValuesCustomPropertiesList.Add(parafaitOptionCustomPropertiesValues);

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
            log.LogMethodExit(parafaitOptionValuesCustomPropertiesList);
            return parafaitOptionValuesCustomPropertiesList;
        }

        /// <summary>
        /// This method is created for Parafait Configuration Value list.
        /// Returns the parafait option values custom list 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>List ParafaitOptionValuesCustomPropertiesDTO</returns>

        public List<ParafaitOptionValuesCustomPropertiesDTO> GetParafaitConfigurationValues(List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesCustomPropertiesList = new List<ParafaitOptionValuesCustomPropertiesDTO>();
            try
            {
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParameters);
                if (parafaitDefaultsDTOList != null)
                {
                    foreach (ParafaitDefaultsDTO parafaitDefaultsDTO in parafaitDefaultsDTOList)
                    {
                        /// It will check the data type of option value and return the specific type whether SQL/Custom/Flag/File/Folder/Text/Hour/Number/Decimal/Password/EncryptedPassword
                        DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(executionContext);
                        DataTable dTable = defaultDataTypeBL.GetDataType(Convert.ToString(parafaitDefaultsDTO.DataTypeId));
                        if (dTable.Rows.Count > 0)
                        {
                            string dataType = string.Empty;
                            if (!string.IsNullOrEmpty(dTable.Rows[0]["datatype"].ToString()))
                            {
                                dataType = dTable.Rows[0]["datatype"].ToString();
                            }
                            if (dataType.StartsWith("Custom", StringComparison.CurrentCultureIgnoreCase))
                                dataType = "Custom";
                            else if (dataType.StartsWith("SQL", StringComparison.CurrentCultureIgnoreCase))
                                dataType = "SQL";

                            ParafaitOptionValuesCustomPropertiesDTO parafaitOptionCustomPropertiesValues = new ParafaitOptionValuesCustomPropertiesDTO()
                            {
                                Items = new List<CommonLookupDTO>()
                            };

                            /// Based on the data type, get the option values dto and convert the object in to ParafaitOptionValuesCustomPropertiesDTO object 
                            switch (dataType)
                            {

                                case "Text":
                                case "Password":
                                case "Folder":
                                case "File":
                                case "Flag":
                                case "Decimal":
                                case "Number":
                                    {
                                        parafaitOptionCustomPropertiesValues = GetDefaultCustomPropertiesDTO(parafaitDefaultsDTO, dataType);
                                    }
                                    break;
                                case "EncryptedPassword":
                                    {
                                        parafaitOptionCustomPropertiesValues = GetDefaultCustomPropertiesDTO(parafaitDefaultsDTO, dataType);
                                        parafaitOptionCustomPropertiesValues.OptionValue = Encryption.Decrypt(parafaitOptionCustomPropertiesValues.OptionValue);
                                    }
                                    break;
                                case "Hour":
                                    {
                                        parafaitOptionCustomPropertiesValues = GetDefaultCustomPropertiesDTO(parafaitDefaultsDTO, dataType);
                                        string hourNbr = string.Empty;
                                        string ampm = string.Empty;
                                        string hour = string.Empty;
                                        for (int j = 0; j < 24; j++)
                                        {
                                            if (j < 12)
                                            {
                                                ampm = "AM";
                                            }
                                            else
                                            {
                                                ampm = "PM";
                                            }

                                            if (j <= 12)
                                            {
                                                hour = j.ToString();
                                            }
                                            else
                                            {
                                                hour = (j - 12).ToString();
                                            }
                                            hourNbr = j.ToString();

                                            if (hour == "0")
                                            {
                                                CommonLookupDTO lookupDataObject;
                                                lookupDataObject = new CommonLookupDTO(hourNbr, "12:00 " + ampm);
                                                parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                            }
                                            else
                                            {
                                                CommonLookupDTO lookupDataObject;
                                                lookupDataObject = new CommonLookupDTO(hourNbr, hour + ":00 " + ampm);
                                                parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                            }
                                        }
                                    }
                                    break;
                                case "SQL":
                                    {
                                        parafaitOptionCustomPropertiesValues = GetDefaultCustomPropertiesDTO(parafaitDefaultsDTO, dataType);
                                        parafaitOptionCustomPropertiesValues.Items = new List<CommonLookupDTO>();
                                        Dictionary<string, string> dataTypes = new Dictionary<string, string>();
                                        if (!string.IsNullOrEmpty(dTable.Rows[0]["datatype"].ToString()))
                                        {
                                            //var dataTypes = defaultDataTypeBL.FetchValues(dTable.Rows[0]["datatype"].ToString());
                                            dataTypes = defaultDataTypeBL.FetchValues(dTable.Rows[0]["datatype"].ToString());
                                            if (dataTypes.Count != 0)
                                            {
                                                foreach (var item in dataTypes)
                                                {
                                                    CommonLookupDTO lookupDataObject;
                                                    lookupDataObject = new CommonLookupDTO(item.Key, item.Value);
                                                    parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case "Custom":
                                    {
                                        parafaitOptionCustomPropertiesValues = GetDefaultCustomPropertiesDTO(parafaitDefaultsDTO, dataType);
                                        parafaitOptionCustomPropertiesValues.Items = new List<CommonLookupDTO>();
                                        Dictionary<string, string> dataTypes = new Dictionary<string, string>();
                                        if (!string.IsNullOrEmpty(dTable.Rows[0]["datatype"].ToString()))
                                        {
                                            dataTypes = defaultDataTypeBL.FetchValues(dTable.Rows[0]["datatype"].ToString());
                                            if (dataTypes.Count > 0)
                                            {
                                                foreach (var item in dataTypes)
                                                {
                                                    CommonLookupDTO lookupDataObject;
                                                    lookupDataObject = new CommonLookupDTO(item.Key, item.Value);
                                                    parafaitOptionCustomPropertiesValues.Items.Add(lookupDataObject);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        parafaitOptionCustomPropertiesValues = GetDefaultCustomPropertiesDTO(parafaitDefaultsDTO, dataType);
                                    }
                                    break;
                            }
                            if (parafaitOptionCustomPropertiesValues != null)
                            {
                                parafaitOptionValuesCustomPropertiesList.Add(parafaitOptionCustomPropertiesValues);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitOptionValuesCustomPropertiesList);
            return parafaitOptionValuesCustomPropertiesList;
        }
        /// <summary>
        /// Normalizes the string - Removes UnderScore and Replaces first Letter of each word by Upper Letter
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ToNormalizationText(string str)
        {
            log.LogMethodEntry(str);
            string[] inputString = str.Split('_');
            string outputString = string.Empty;
            for (int i = 0; i < inputString.Length; i++)
            {
                if (!string.IsNullOrEmpty(inputString[i]))
                {
                    inputString[i] = char.ToUpper(inputString[i][0]) + inputString[i].Substring(1).ToLower();
                }
                outputString += inputString[i] + ' ';
            }
            log.LogMethodExit(outputString);
            return outputString;
        }

        /// <summary>
        /// Convert the option values to option values custom object
        /// </summary>
        /// <param name="parafaitOptionValuesDTO"></param>
        /// <param name="parafaitDefaultsDTO"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private ParafaitOptionValuesCustomPropertiesDTO GetCustomPropertiesDTO(ParafaitOptionValuesDTO parafaitOptionValuesDTO, ParafaitDefaultsDTO parafaitDefaultsDTO, string dataType)
        {
            log.LogMethodEntry(parafaitOptionValuesDTO, parafaitDefaultsDTO, dataType);
            ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO = new ParafaitOptionValuesCustomPropertiesDTO();
            parafaitOptionValuesCustomPropertiesDTO.OptionId = parafaitDefaultsDTO.DefaultValueId;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValueId = parafaitOptionValuesDTO.OptionValueId;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValueName = parafaitDefaultsDTO.DefaultValueName;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValueNameText = ToNormalizationText(parafaitDefaultsDTO.DefaultValueName);
            parafaitOptionValuesCustomPropertiesDTO.OptionValue = parafaitOptionValuesDTO.OptionValue;
            parafaitOptionValuesCustomPropertiesDTO.Type = dataType;
            parafaitOptionValuesCustomPropertiesDTO.ScreenGroup = parafaitDefaultsDTO.ScreenGroup;
            parafaitOptionValuesCustomPropertiesDTO.Description = parafaitDefaultsDTO.Description;
            parafaitOptionValuesCustomPropertiesDTO.POSMachineId = parafaitOptionValuesDTO.PosMachineId;
            parafaitOptionValuesCustomPropertiesDTO.UserId = parafaitOptionValuesDTO.UserId;
            parafaitOptionValuesCustomPropertiesDTO.IsActive = parafaitOptionValuesDTO.IsActive;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValue = parafaitDefaultsDTO.DefaultValue;
            parafaitOptionValuesCustomPropertiesDTO.IsProtected = parafaitDefaultsDTO.IsProtected;
            parafaitOptionValuesCustomPropertiesDTO.SiteId = parafaitOptionValuesDTO.SiteId;
            parafaitOptionValuesCustomPropertiesDTO.DataTypeId = parafaitDefaultsDTO.DataTypeId;
            parafaitOptionValuesCustomPropertiesDTO.UserLevel = parafaitDefaultsDTO.UserLevel;
            parafaitOptionValuesCustomPropertiesDTO.POSLevel = parafaitDefaultsDTO.POSLevel;
            parafaitOptionValuesCustomPropertiesDTO.MasterEntityId = parafaitOptionValuesDTO.MasterEntityId;
            parafaitOptionValuesCustomPropertiesDTO.Items = new List<CommonLookupDTO>();
            log.LogMethodExit(parafaitOptionValuesCustomPropertiesDTO);
            return parafaitOptionValuesCustomPropertiesDTO;
        }
        /// <summary>
        /// Gets DefaultCustomProperties
        /// </summary>
        /// <param name="parafaitDefaultsDTO"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private ParafaitOptionValuesCustomPropertiesDTO GetDefaultCustomPropertiesDTO(ParafaitDefaultsDTO parafaitDefaultsDTO, string dataType)
        {
            log.LogMethodEntry(parafaitDefaultsDTO, dataType);
            ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO = new ParafaitOptionValuesCustomPropertiesDTO();
            parafaitOptionValuesCustomPropertiesDTO.DefaultValueId = parafaitDefaultsDTO.DefaultValueId;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValue = parafaitDefaultsDTO.DefaultValue;
            parafaitOptionValuesCustomPropertiesDTO.OptionValue = parafaitDefaultsDTO.DefaultValue;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValueName = parafaitDefaultsDTO.DefaultValueName;
            parafaitOptionValuesCustomPropertiesDTO.DefaultValueNameText = ToNormalizationText(parafaitDefaultsDTO.DefaultValueName);
            parafaitOptionValuesCustomPropertiesDTO.DataTypeId = parafaitDefaultsDTO.DataTypeId;
            parafaitOptionValuesCustomPropertiesDTO.Type = dataType;
            parafaitOptionValuesCustomPropertiesDTO.IsActive = parafaitDefaultsDTO.IsActive;
            parafaitOptionValuesCustomPropertiesDTO.IsChanged = parafaitDefaultsDTO.IsChanged;
            parafaitOptionValuesCustomPropertiesDTO.IsProtected = parafaitDefaultsDTO.IsProtected;
            parafaitOptionValuesCustomPropertiesDTO.LastUpdatedBy = parafaitDefaultsDTO.LastUpdatedBy;
            parafaitOptionValuesCustomPropertiesDTO.LastUpdatedDate = parafaitDefaultsDTO.LastUpdatedDate;
            parafaitOptionValuesCustomPropertiesDTO.MasterEntityId = parafaitDefaultsDTO.MasterEntityId;
            parafaitOptionValuesCustomPropertiesDTO.POSLevel = parafaitDefaultsDTO.POSLevel;
            parafaitOptionValuesCustomPropertiesDTO.SiteId = parafaitDefaultsDTO.SiteId;
            parafaitOptionValuesCustomPropertiesDTO.Guid = parafaitDefaultsDTO.Guid;
            parafaitOptionValuesCustomPropertiesDTO.SynchStatus = parafaitDefaultsDTO.SynchStatus;
            parafaitOptionValuesCustomPropertiesDTO.UserLevel = parafaitDefaultsDTO.UserLevel;
            parafaitOptionValuesCustomPropertiesDTO.ScreenGroup = parafaitDefaultsDTO.ScreenGroup;
            parafaitOptionValuesCustomPropertiesDTO.Description = parafaitDefaultsDTO.Description;
            parafaitOptionValuesCustomPropertiesDTO.Items = new List<CommonLookupDTO>();
            log.LogMethodExit(parafaitOptionValuesCustomPropertiesDTO);
            return parafaitOptionValuesCustomPropertiesDTO;
        }
        /// <summary>
        /// Save or update Option values list
        /// </summary>
        /// <param name="parafaitOptionValuesCustomPropertiesDTOList"></param>

        public void SaveUpdateParafaitValueList(List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesCustomPropertiesDTOList)
        {
            log.LogMethodEntry(parafaitOptionValuesCustomPropertiesDTOList);
            try
            {
                List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList = new List<ParafaitOptionValuesDTO>();
                foreach (ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO in parafaitOptionValuesCustomPropertiesDTOList)
                {
                    switch (parafaitOptionValuesCustomPropertiesDTO.Type)
                    {
                        case "Flag":
                        case "Custom":
                        case "SQL":
                        case "Hour":
                        case "Decimal":
                        case "Number":
                            {
                                parafaitOptionValuesDTOList.Add(ParafaitOptionValuesDTO(parafaitOptionValuesCustomPropertiesDTO));
                            }
                            break;
                        case "EncryptedPassword":
                            {
                                if (!string.IsNullOrEmpty(parafaitOptionValuesCustomPropertiesDTO.OptionValue))
                                {
                                    parafaitOptionValuesCustomPropertiesDTO.OptionValue = Encryption.Encrypt(parafaitOptionValuesCustomPropertiesDTO.OptionValue);
                                }
                                else
                                    parafaitOptionValuesCustomPropertiesDTO.OptionValue = "";
                                parafaitOptionValuesDTOList.Add(ParafaitOptionValuesDTO(parafaitOptionValuesCustomPropertiesDTO));
                            }
                            break;
                        default:
                            {
                                parafaitOptionValuesDTOList.Add(ParafaitOptionValuesDTO(parafaitOptionValuesCustomPropertiesDTO));
                            }
                            break;
                    }
                }
                ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext, parafaitOptionValuesDTOList);
                parafaitOptionValuesListBL.SaveUpdateParafaitOptionValuesDTOList();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Convert the custom properties object to ParafaitOptionValuesDTO object
        /// </summary>
        /// <param name="parafaitOptionValuesCustomPropertiesDTO"></param>
        /// <returns></returns>
        private ParafaitOptionValuesDTO ParafaitOptionValuesDTO(ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO)
        {
            log.LogMethodEntry(parafaitOptionValuesCustomPropertiesDTO);
            ParafaitOptionValuesDTO parafaitOptionValuesDTO = new ParafaitOptionValuesDTO();
            parafaitOptionValuesDTO.OptionId = parafaitOptionValuesCustomPropertiesDTO.OptionId;
            parafaitOptionValuesDTO.OptionValue = parafaitOptionValuesCustomPropertiesDTO.OptionValue;
            parafaitOptionValuesDTO.OptionValueId = parafaitOptionValuesCustomPropertiesDTO.DefaultValueId;
            parafaitOptionValuesDTO.PosMachineId = parafaitOptionValuesCustomPropertiesDTO.POSMachineId;
            parafaitOptionValuesDTO.UserId = parafaitOptionValuesCustomPropertiesDTO.UserId;
            parafaitOptionValuesDTO.MasterEntityId = parafaitOptionValuesCustomPropertiesDTO.MasterEntityId;
            parafaitOptionValuesDTO.SiteId = parafaitOptionValuesCustomPropertiesDTO.SiteId;
            log.LogMethodExit(parafaitOptionValuesDTO);
            return parafaitOptionValuesDTO;
        }


        /// <summary>
        /// This method is used for save/update the Parafait Configuration Values
        /// </summary>
        /// <param name="parafaitOptionValuesList"></param>

        public void SaveUpdateParafaitConfigurationValues(List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesList)
        {
            log.LogMethodEntry(parafaitOptionValuesList);
            try
            {
                if (parafaitOptionValuesList != null && parafaitOptionValuesList.Count != 0)
                {
                    List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = new List<ParafaitDefaultsDTO>();
                    foreach (ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO in parafaitOptionValuesList)
                    {
                        parafaitDefaultsDTOList.Add(GetParafaitDefaultsDTO(parafaitOptionValuesCustomPropertiesDTO));
                    }
                    if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count != 0)
                    {
                        ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext, parafaitDefaultsDTOList);
                        parafaitDefaultsListBL.SaveUpdateParafaitDefaultList();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Convert the custom properties object to parafaitDefaultsDTO object
        /// </summary>
        /// <param name="parafaitOptionValuesCustomPropertiesDTO"></param>
        /// <returns></returns>
        private ParafaitDefaultsDTO GetParafaitDefaultsDTO(ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO)
        {
            log.LogMethodEntry(parafaitOptionValuesCustomPropertiesDTO);
            ParafaitDefaultsDTO parafaitDefaultsDTO = new ParafaitDefaultsDTO();
            if (parafaitOptionValuesCustomPropertiesDTO.Type == "EncryptedPassword")
            {
                if (!string.IsNullOrEmpty(parafaitOptionValuesCustomPropertiesDTO.OptionValue))
                {
                    parafaitOptionValuesCustomPropertiesDTO.OptionValue = Encryption.Encrypt(parafaitOptionValuesCustomPropertiesDTO.OptionValue);
                }
                else
                {
                    parafaitOptionValuesCustomPropertiesDTO.OptionValue = "";
                }
            }
            parafaitDefaultsDTO.DefaultValueId = parafaitOptionValuesCustomPropertiesDTO.DefaultValueId;
            parafaitDefaultsDTO.DefaultValue = parafaitOptionValuesCustomPropertiesDTO.OptionValue;
            parafaitDefaultsDTO.DefaultValueName = parafaitOptionValuesCustomPropertiesDTO.DefaultValueName;
            parafaitDefaultsDTO.DataTypeId = parafaitOptionValuesCustomPropertiesDTO.DataTypeId;
            parafaitDefaultsDTO.IsActive = parafaitOptionValuesCustomPropertiesDTO.IsActive;
            parafaitDefaultsDTO.IsProtected = parafaitOptionValuesCustomPropertiesDTO.IsProtected;
            parafaitDefaultsDTO.MasterEntityId = parafaitOptionValuesCustomPropertiesDTO.MasterEntityId;
            parafaitDefaultsDTO.POSLevel = parafaitOptionValuesCustomPropertiesDTO.POSLevel;
            parafaitDefaultsDTO.UserLevel = parafaitOptionValuesCustomPropertiesDTO.UserLevel;
            parafaitDefaultsDTO.ScreenGroup = parafaitOptionValuesCustomPropertiesDTO.ScreenGroup;
            parafaitDefaultsDTO.Description = parafaitOptionValuesCustomPropertiesDTO.Description;
            parafaitDefaultsDTO.IsChanged = parafaitOptionValuesCustomPropertiesDTO.IsChanged;

            log.LogMethodExit(parafaitDefaultsDTO);
            return parafaitDefaultsDTO;
        }
        /// <summary>
        ///  This method is used for save/update the Parafait Configuration Settings
        /// </summary>
        /// <param name="parafaitOptionValuesCustomPropertiesDTOList"></param>
        public void SaveUpdateParafaitConfigurationSettings(List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesCustomPropertiesDTOList)
        {
            log.LogMethodEntry(parafaitOptionValuesCustomPropertiesDTOList);
            try
            {
                if (parafaitOptionValuesCustomPropertiesDTOList != null && parafaitOptionValuesCustomPropertiesDTOList.Count > 0)
                {
                    foreach (ParafaitOptionValuesCustomPropertiesDTO parafaitOptionValuesCustomPropertiesDTO in parafaitOptionValuesCustomPropertiesDTOList)
                    {
                        ParafaitDefaultsDTO parafaitDefaultsDTO = new ParafaitDefaultsDTO();
                        parafaitDefaultsDTO.DefaultValueId = parafaitOptionValuesCustomPropertiesDTO.DefaultValueId;
                        parafaitDefaultsDTO.DefaultValue = parafaitOptionValuesCustomPropertiesDTO.DefaultValue;
                        parafaitDefaultsDTO.DefaultValueName = parafaitOptionValuesCustomPropertiesDTO.DefaultValueName;
                        parafaitDefaultsDTO.DataTypeId = parafaitOptionValuesCustomPropertiesDTO.DataTypeId;
                        parafaitDefaultsDTO.IsActive = parafaitOptionValuesCustomPropertiesDTO.IsActive;
                        parafaitDefaultsDTO.IsProtected = parafaitOptionValuesCustomPropertiesDTO.IsProtected;
                        parafaitDefaultsDTO.MasterEntityId = parafaitOptionValuesCustomPropertiesDTO.MasterEntityId;
                        parafaitDefaultsDTO.POSLevel = parafaitOptionValuesCustomPropertiesDTO.POSLevel;
                        parafaitDefaultsDTO.UserLevel = parafaitOptionValuesCustomPropertiesDTO.UserLevel;
                        parafaitDefaultsDTO.ScreenGroup = parafaitOptionValuesCustomPropertiesDTO.ScreenGroup;
                        parafaitDefaultsDTO.Description = parafaitOptionValuesCustomPropertiesDTO.Description;
                        parafaitDefaultsDTO.IsChanged = parafaitOptionValuesCustomPropertiesDTO.IsChanged;

                        ParafaitDefaultsBL parafaitDefaults = new ParafaitDefaultsBL(executionContext, parafaitDefaultsDTO);
                        parafaitDefaults.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

/********************************************************************************************
 * Project Name - CustomAttributes Wrapper BL
 * Description  - Business logic
 * This wrapper class been written because in the existing 3 tier, attributes and the values have been pulled and displayed at the UI layer in teh windows  application
 * The same aprpoach cannot be followed in Angular. Hence wrapper will take attributes and the values and returns DTO to Angular UI
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.40        03-Oct-2018      Jagan     Created 
 *2.70.2        12-Aug-2019      Deeksha   Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Business logic for CustomAttributes Wrapper class.
    /// </summary>
    public class CustomAttributesWrapperBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext = null;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomAttributesWrapperBL(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executioncontext);
            executionContext = executioncontext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CustomAttributes Wrapper list
        /// </summary>
        public List<CustomAttributesWrapperDTO> GetCustomeAttributes(string applicability, int entityId)
        {
            log.LogMethodEntry(applicability, entityId);
            try
            {
                log.LogMethodEntry(applicability,entityId);
                List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchParameters = null;
                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
                List<CustomAttributesWrapperDTO> customAttributesDataList = new List<CustomAttributesWrapperDTO>();

                List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> customAttributesSearchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                customAttributesSearchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, applicability));
                customAttributesSearchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));

                List<CustomAttributesDTO> customAttributes = customAttributesListBL.GetCustomAttributesDTOList(customAttributesSearchParameters);
                int customDataSetId = 0;
                if (customAttributes != null)
                {                    
                    List<CustomDataDTO> customData = new List<CustomDataDTO>();
                    switch (applicability)
                    {
                        case "GAMES":
                            
                            Game game = new Game(entityId, executionContext);
                            var gameObj = game.GetGameDTO;
                            if (gameObj.CustomDataSetId != 0)
                            {
                                customDataSetId = gameObj.CustomDataSetId;
                            }
                            break;
                        case "GAME_PROFILE":
                            GameProfile gameProfile = new GameProfile(entityId, executionContext);
                            var gameProfileObj = gameProfile.GetGameProfileDTO;
                            if (gameProfileObj.CustomDataSetId != 0)
                            {
                                customDataSetId = gameProfileObj.CustomDataSetId;
                            }
                            break;
                        case "MACHINE":
                            Machine machine = new Machine(entityId, executionContext);
                            var machineObj = machine.GetMachineDTO;
                            if (machineObj.CustomDataSetId != 0)
                            {
                                customDataSetId = machineObj.CustomDataSetId;
                            }
                            break;
                        case "PRODUCT":

                            break;
                        case "CUSTOMER":
                            break;
                        case "CARDGAMES":
                            break;
                    }
                    if (customDataSetId != 0 && customDataSetId != -1)
                    {
                        List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> customDataSearchParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                        customDataSearchParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, Convert.ToString(customDataSetId)));
                        CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
                        customData = customDataListBL.GetCustomDataDTOList(customDataSearchParameters);
                    }
                    foreach (var item in customAttributes)
                    {
                        CustomAttributesWrapperDTO cawDTO = new CustomAttributesWrapperDTO();
                        cawDTO.CustomAttributeId = item.CustomAttributeId;
                        cawDTO.Name = item.Name;
                        cawDTO.Type = item.Type;
                        cawDTO.Applicability = item.Applicability;
                        cawDTO.CustomDataSetId = customDataSetId;
                        cawDTO.IsChanged = false;
                        if (customData.Count != 0 && customDataSetId != -1 && customData != null)
                        {
                            var customDataValues = customData.Find(x => x.CustomDataSetId == customDataSetId && x.CustomAttributeId == item.CustomAttributeId);
                            if (customDataValues != null)
                            {
                                cawDTO.CustomDataId = customDataValues.CustomDataId;
                                cawDTO.CustomDataText = customDataValues.CustomDataText;
                                cawDTO.CustomDataNumber = customDataValues.CustomDataNumber;
                                cawDTO.CustomDataDate = customDataValues.CustomDataDate;
                                cawDTO.ValueId = customDataValues.ValueId;
                                cawDTO.IsChanged = false;
                                cawDTO.MasterEntityId = customDataValues.MasterEntityId;
                                cawDTO.SiteId = customDataValues.SiteId;
                                cawDTO.SynchStatus = customDataValues.SynchStatus;
                                cawDTO.CreatedDate = customDataValues.CreationDate;
                                cawDTO.CreatedBy = customDataValues.CreatedBy;
                                cawDTO.LastUpdatedDate = customDataValues.LastUpdateDate;
                                cawDTO.LastUpdatedBy = customDataValues.LastUpdatedBy;
                            }
                        }
                        if (item.Type == "LIST")
                        {
                            CustomAttributeValueListListBL customAttributeValueListBL = new CustomAttributeValueListListBL(executionContext);
                            List<CustomAttributeValueListDTO> CustomAttributeValueList = customAttributeValueListBL.GetCustomAttributeValueListDTOList(searchParameters);
                            if (CustomAttributeValueList.Count != 0)
                            {
                                var valuesdata = CustomAttributeValueList.Where(x => x.CustomAttributeId == item.CustomAttributeId).ToList();
                                if (valuesdata.Count != 0)
                                {
                                    List<CustomAttributeValueListWrapperDTO> valueList = new List<CustomAttributeValueListWrapperDTO>();
                                    foreach (var valuedata in valuesdata)
                                    {
                                        CustomAttributeValueListWrapperDTO cavlDTO = new CustomAttributeValueListWrapperDTO();
                                        cavlDTO.CustomAttributeId = item.CustomAttributeId;
                                        cavlDTO.Value = valuedata.Value;
                                        cavlDTO.ValueId = valuedata.ValueId;
                                        valueList.Add(cavlDTO);
                                    }
                                    cawDTO.ValueList = valueList;
                                }
                            }
                        }
                        customAttributesDataList.Add(cawDTO);                        
                    }
                }
                log.LogMethodExit(customAttributesDataList);
                return customAttributesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        //List<CustomAttributesWrapperDTO> customAttributeWrapperList;

        /// <summary>
        /// Save or update the Custom attributes list details
        /// </summary>
        public int SaveUpdateCustomAttributeWrapperList(List<CustomAttributesWrapperDTO> customAttributesWrapperDTOs, string commonModuleId)
        {
            log.LogMethodEntry(customAttributesWrapperDTOs, commonModuleId);
            try
            {
                int result = 0;
                log.LogMethodEntry(customAttributesWrapperDTOs, commonModuleId);
                string applicability = string.Empty;
                CustomDataSetDTO customDataSetDTO = new CustomDataSetDTO();
                List<CustomDataDTO> customDataListDTO = new List<CustomDataDTO>();
                foreach (CustomAttributesWrapperDTO customAttributesDto in customAttributesWrapperDTOs)
                {
                    CustomDataDTO customDataDTO = new CustomDataDTO();
                    customDataDTO.CustomDataSetId = customAttributesDto.CustomDataSetId;
                    customDataDTO.CustomAttributeId = customAttributesDto.CustomAttributeId;
                    customDataDTO.CustomDataText = customAttributesDto.CustomDataText;
                    customDataDTO.CustomDataNumber = customAttributesDto.CustomDataNumber;
                    customDataDTO.CustomDataDate = customAttributesDto.CustomDataDate;
                    customDataDTO.MasterEntityId = customAttributesDto.MasterEntityId;
                    customDataDTO.SynchStatus = customAttributesDto.SynchStatus;
                    customDataDTO.SiteId = executionContext.GetSiteId();
                    customDataDTO.CreatedBy = executionContext.GetUserId();
                    customDataDTO.CreationDate = DateTime.Now;
                    customDataDTO.LastUpdatedBy = executionContext.GetUserId();
                    customDataDTO.LastUpdateDate = DateTime.Now;
                    applicability = customAttributesDto.Applicability;
                    if (customAttributesDto.ValueList != null)
                    {
                        List<CustomAttributeValueListDTO> customAttributeValueListDTO = new List<CustomAttributeValueListDTO>();
                        foreach (var customAttributeValue in customAttributesDto.ValueList)
                        {
                            CustomAttributeValueListDTO customAttributeValueDTO = new CustomAttributeValueListDTO();
                            customAttributeValueDTO.CustomAttributeId = customAttributeValue.CustomAttributeId;
                            customAttributeValueDTO.Value = customAttributeValue.Value;
                            customAttributeValueDTO.ValueId = customAttributeValue.ValueId;
                            customAttributeValueDTO.IsChanged = customAttributeValue.IsChanged;
                            customAttributeValueListDTO.Add(customAttributeValueDTO);
                        }
                        foreach (var customAttributeValue in customAttributeValueListDTO)
                        {
                            if (customAttributeValue.IsChanged)
                            {
                                customAttributeValue.CustomAttributeId = customAttributesDto.CustomAttributeId;
                                CustomAttributeValueListBL customAttributeValueListBL = new CustomAttributeValueListBL(executionContext, customAttributeValue);
                                customAttributeValueListBL.Save(null);
                                customDataDTO.ValueId = customAttributeValue.ValueId;
                            }
                        }
                    }
                    customDataListDTO.Add(customDataDTO);
                }

                int commonId = Convert.ToInt32(commonModuleId);
                customDataSetDTO.CustomDataDTOList = customDataListDTO;
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customDataSetDTO);
                customDataSetBL.Save();
                var customDataSetDTOObj = customDataSetBL.CustomDataSetDTO;
                int mainCustomDataSetId = customDataSetDTOObj.CustomDataSetId;
                log.Debug(string.Format("CustomDataSetId {0} has been stored from SaveUpdateCustomAttributeWrapperList() ", mainCustomDataSetId));

                switch (applicability)
                {
                    case "GAMES":
                        Game game = new Game(commonId, executionContext);
                        var gameObj = game.GetGameDTO;
                        if (gameObj.CustomDataSetId != 0)
                        {
                            gameObj.CustomDataSetId = mainCustomDataSetId;
                            gameObj.IsChanged = true;
                            game.InsertUpdateGames();
                        }
                        break;
                    case "GAME_PROFILE":
                        GameProfile gameProfile = new GameProfile(commonId, executionContext);
                        var gameProfileObj = gameProfile.GetGameProfileDTO;
                        if (gameProfileObj.CustomDataSetId != 0)
                        {
                            gameProfileObj.CustomDataSetId = mainCustomDataSetId;
                            gameProfileObj.IsChanged = true;
                            gameProfile.Save();
                        }
                        break;
                    case "MACHINE":
                        Machine machine = new Machine(commonId, executionContext);
                        var machineObj = machine.GetMachineDTO;
                        if (machineObj.CustomDataSetId != 0)
                        {
                            machineObj.CustomDataSetId = mainCustomDataSetId;
                            machineObj.IsChanged = true;
                            machine.Save();
                        }
                        break;
                    case "PRODUCT":

                        break;
                    case "CUSTOMER":
                        break;
                    case "CARDGAMES":
                        break;
                }
                log.LogMethodExit(result);
                return result;
            }
            catch(Exception ex)
            {
                log.Error("Error while executing SaveUpdateCustomAttributeWrapperList()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw;
            }
        }
    }
}

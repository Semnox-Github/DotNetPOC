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
 *2.40        03-Oct-2018      Jagan               Created 
 *2.40        04-Oct-2018      Mehraj              Modified
 *2.60        18-Mar-2019      Akshay Gulaganji    Modified CaseNames for ProductsSetup as of Lookup/Localization case names 
 *            04-Apr-2019      Indrajeet Kumar     Modified- The SaveUpdateCustomAttributeWrapperList()
 *2.80        02-Jun-2020      Girish Kundar       Modified- Added customDataSetId update for customer entity
 *2.150.0     07-Jul-2022      Abhishek            Modified:Get and Save CustomAttributes to the Inventory Product  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Product;
using Semnox.Parafait.Game;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Inventory;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Helpers
{
    /// <summary>
    /// Business logic for CustomAttributes Wrapper class.
    /// </summary>
    public class CustomAttributesWrapperBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomAttributesWrapperBL(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }
        /// <summary>
        /// Returns the CustomAttributes Wrapper list
        /// </summary>
        public List<CustomAttributesWrapperDTO> GetCustomeAttributes(string applicability, int entityId)
        {
            try
            {
                log.LogMethodEntry(applicability, entityId);
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
                    List<CustomDataDTO> customDataDTOList = new List<CustomDataDTO>();
                    switch (applicability)
                    {
                        case "GAMES":

                            Game game = new Game(entityId, executionContext);
                            GameDTO gameDTO = game.GetGameDTO;
                            if (gameDTO.CustomDataSetId != 0)
                            {
                                customDataSetId = gameDTO.CustomDataSetId;
                            }
                            break;
                        case "GAME_PROFILE":
                            GameProfile gameProfile = new GameProfile(entityId, executionContext);
                            GameProfileDTO gameProfileDTO = gameProfile.GetGameProfileDTO;
                            if (gameProfileDTO.CustomDataSetId != 0)
                            {
                                customDataSetId = gameProfileDTO.CustomDataSetId;
                            }
                            break;
                        case "MACHINE":
                            Machine machine = new Machine(entityId, executionContext);
                            MachineDTO machineDTO = machine.GetMachineDTO;
                            if (machineDTO.CustomDataSetId != 0)
                            {
                                customDataSetId = machineDTO.CustomDataSetId;
                            }
                            break;
                        case "PRODUCT":
                            ProductsList productsList = new ProductsList(executionContext);
                            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productsSearchList = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                            productsSearchList.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, entityId.ToString()));
                            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(productsSearchList);
                            if (productsDTOList != null)
                            {
                                foreach (var item in productsDTOList)
                                {
                                    customDataSetId = item.CustomDataSetId;
                                }
                            }
                            break;
                        case "CUSTOMER":
                            CustomerBL customerBL = new CustomerBL(executionContext, entityId);
                            CustomerDTO customerDTO = customerBL.CustomerDTO;
                            if(customerDTO.CustomDataSetId != 0)
                            {
                                customDataSetId = customerDTO.CustomDataSetId;
                            }
                            break;
                        case "CARDGAMES":
                            AccountGameBL accountGameBL = new AccountGameBL(executionContext, entityId);
                            AccountGameDTO accountGameDTO = accountGameBL.AccountGameDTO;
                            if (accountGameDTO.CustomDataSetId != 0)
                            {
                                customDataSetId = accountGameDTO.CustomDataSetId;
                            }
                            break;
                        case "LOCATION":
                            LocationBL locationBL = new LocationBL(executionContext, entityId);
                            if (locationBL.GetLocationDTO != null)
                            {
                                LocationDTO locationDTO = locationBL.GetLocationDTO;
                                if (locationDTO.CustomDataSetId != 0)
                                {
                                    customDataSetId = locationDTO.CustomDataSetId;
                                }
                            }
                            break;
                        case "INVPRODUCT":
                            ProductList productList = new ProductList(executionContext);
                            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productSearchList = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                            productSearchList.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, entityId.ToString()));
                            List<ProductDTO> productDTOList = productList.GetAllProducts(productSearchList);
                            if (productDTOList != null)
                            {
                                foreach (var item in productDTOList)
                                {
                                    customDataSetId = item.CustomDataSetId;
                                }
                            }
                            break;
                    }
                    string customDataSetGuid = string.Empty;
                    if (customDataSetId != 0 && customDataSetId != -1)
                    {
                        CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customDataSetId, false);
                        customDataSetGuid = customDataSetBL.CustomDataSetDTO.Guid;
                        List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> customDataSearchParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                        customDataSearchParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, Convert.ToString(customDataSetId)));
                        CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
                        customDataDTOList = customDataListBL.GetCustomDataDTOList(customDataSearchParameters);
                    }
                    foreach (var item in customAttributes)
                    {
                        CustomAttributesWrapperDTO customAttributesWrapperDTO = new CustomAttributesWrapperDTO();
                        customAttributesWrapperDTO.CustomAttributeId = item.CustomAttributeId;
                        customAttributesWrapperDTO.Name = item.Name;
                        customAttributesWrapperDTO.Type = item.Type;
                        customAttributesWrapperDTO.CustomeDataSetGuid = customDataSetGuid;
                        customAttributesWrapperDTO.Applicability = item.Applicability;
                        customAttributesWrapperDTO.CustomDataSetId = customDataSetId;
                        customAttributesWrapperDTO.IsChanged = false;
                        if (customDataDTOList.Count != 0 && customDataSetId != -1 && customDataDTOList != null)
                        {
                            var customDataValues = customDataDTOList.Find(x => x.CustomDataSetId == customDataSetId && x.CustomAttributeId == item.CustomAttributeId);
                            if (customDataValues != null)
                            {
                                customAttributesWrapperDTO.CustomDataId = customDataValues.CustomDataId;
                                customAttributesWrapperDTO.CustomDataText = customDataValues.CustomDataText;
                                customAttributesWrapperDTO.CustomDataNumber = customDataValues.CustomDataNumber;
                                customAttributesWrapperDTO.CustomDataDate = customDataValues.CustomDataDate;
                                customAttributesWrapperDTO.ValueId = customDataValues.ValueId;
                                customAttributesWrapperDTO.IsChanged = false;
                                customAttributesWrapperDTO.MasterEntityId = customDataValues.MasterEntityId;
                                customAttributesWrapperDTO.SiteId = customDataValues.SiteId;
                                customAttributesWrapperDTO.SynchStatus = customDataValues.SynchStatus;
                                customAttributesWrapperDTO.CreatedDate = customDataValues.CreationDate;
                                customAttributesWrapperDTO.CreatedBy = customDataValues.CreatedBy;
                                customAttributesWrapperDTO.LastUpdatedDate = customDataValues.LastUpdateDate;
                                customAttributesWrapperDTO.LastUpdatedBy = customDataValues.LastUpdatedBy;
                                customAttributesWrapperDTO.CustomeDataGuid = customDataValues.Guid;
                            }
                        }
                        if (item.Type == "LIST")
                        {
                            CustomAttributeValueListListBL customAttributeValueListBL = new CustomAttributeValueListListBL(executionContext);
                            List<CustomAttributeValueListDTO> customAttributeValueList = customAttributeValueListBL.GetCustomAttributeValueListDTOList(searchParameters);
                            if (customAttributeValueList.Count != 0)
                            {
                                List<CustomAttributeValueListDTO> customAttributeValueListDTO = customAttributeValueList.Where(x => x.CustomAttributeId == item.CustomAttributeId).ToList();
                                if (customAttributeValueListDTO.Count != 0)
                                {
                                    List<CustomAttributeValueListWrapperDTO> customAttributeValueListWrapperDTOList = new List<CustomAttributeValueListWrapperDTO>();
                                    foreach (CustomAttributeValueListDTO customAttributeValueDTO in customAttributeValueListDTO)
                                    {
                                        CustomAttributeValueListWrapperDTO customAttributeValueListWrapperDTO = new CustomAttributeValueListWrapperDTO();
                                        customAttributeValueListWrapperDTO.CustomAttributeId = item.CustomAttributeId;
                                        customAttributeValueListWrapperDTO.Value = customAttributeValueDTO.Value;
                                        customAttributeValueListWrapperDTO.ValueId = customAttributeValueDTO.ValueId;
                                        customAttributeValueListWrapperDTOList.Add(customAttributeValueListWrapperDTO);
                                    }
                                    customAttributesWrapperDTO.ValueList = customAttributeValueListWrapperDTOList;
                                }
                            }
                        }
                        customAttributesDataList.Add(customAttributesWrapperDTO);
                    }
                }
                log.LogMethodExit(customAttributesDataList);
                return customAttributesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Save or update the Custom attributes list details
        /// </summary>
        public int SaveUpdateCustomAttributeWrapperList(List<CustomAttributesWrapperDTO> customAttributesWrapperDTOList, string commonModuleId)
        {
            try
            {
                int result = 0;
                log.LogMethodEntry(customAttributesWrapperDTOList, commonModuleId);
                string applicability = string.Empty;
                CustomDataSetDTO customDataSetDTO = new CustomDataSetDTO();
                List<CustomDataDTO> customDataListDTO = new List<CustomDataDTO>();
                foreach (CustomAttributesWrapperDTO customAttributesWrapperDTO in customAttributesWrapperDTOList)
                {
                    CustomDataDTO customDataDTO = new CustomDataDTO();
                    customDataSetDTO.CustomDataSetId = customAttributesWrapperDTO.CustomDataSetId;
                    customDataDTO.CustomDataSetId = customAttributesWrapperDTO.CustomDataSetId;
                    customDataDTO.CustomDataId = customAttributesWrapperDTO.CustomDataId;
                    customDataDTO.CustomAttributeId = customAttributesWrapperDTO.CustomAttributeId;
                    customDataDTO.CustomDataText = customAttributesWrapperDTO.CustomDataText;
                    customDataDTO.CustomDataNumber = customAttributesWrapperDTO.CustomDataNumber;
                    customDataDTO.CustomDataDate = customAttributesWrapperDTO.CustomDataDate;
                    customDataDTO.MasterEntityId = customAttributesWrapperDTO.MasterEntityId;
                    customDataDTO.SynchStatus = customAttributesWrapperDTO.SynchStatus;
                    customDataDTO.SiteId = executionContext.GetSiteId();
                    customDataDTO.CreatedBy = executionContext.GetUserId();
                    customDataDTO.CreationDate = DateTime.Now;
                    customDataDTO.LastUpdatedBy = executionContext.GetUserId();
                    customDataDTO.LastUpdateDate = DateTime.Now;
                    customDataDTO.IsChanged = customAttributesWrapperDTO.IsChanged;
                    if(customAttributesWrapperDTO.CustomDataId > -1)
                    {
                        customDataDTO.Guid = customAttributesWrapperDTO.CustomeDataGuid;
                    }
                    if (customAttributesWrapperDTO.CustomDataSetId > -1)
                    {
                        customDataSetDTO.Guid = customAttributesWrapperDTO.CustomeDataSetGuid;
                    }
                    applicability = customAttributesWrapperDTO.Applicability;
                    if (customAttributesWrapperDTO.ValueList != null)
                    {
                        List<CustomAttributeValueListDTO> customAttributeValueListDTO = new List<CustomAttributeValueListDTO>();
                        foreach (CustomAttributeValueListWrapperDTO customAttributeValueListWrapperDTO in customAttributesWrapperDTO.ValueList)
                        {
                            CustomAttributeValueListDTO customAttributeValueDTO = new CustomAttributeValueListDTO();
                            customAttributeValueDTO.CustomAttributeId = customAttributeValueListWrapperDTO.CustomAttributeId;
                            customAttributeValueDTO.Value = customAttributeValueListWrapperDTO.Value;
                            customAttributeValueDTO.ValueId = customAttributeValueListWrapperDTO.ValueId;
                            customAttributeValueDTO.IsChanged = customAttributeValueListWrapperDTO.IsChanged;
                            customAttributeValueListDTO.Add(customAttributeValueDTO);
                        }
                        foreach (CustomAttributeValueListDTO customAttributeValue in customAttributeValueListDTO)
                        {
                            if (customAttributeValue.IsChanged)
                            {
                                customAttributeValue.CustomAttributeId = customAttributesWrapperDTO.CustomAttributeId;
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
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productsSearchList = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        productsSearchList.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, commonId.ToString()));
                        var productsObj = productsList.GetProductsDTOList(productsSearchList);
                        if (productsObj != null)
                        {
                            foreach (var item in productsObj)
                            {
                                if (item.CustomDataSetId != 0)
                                {
                                    item.CustomDataSetId = mainCustomDataSetId;
                                    item.IsChanged = true;
                                    productsList = new ProductsList(executionContext, productsObj);
                                    productsList.Save();
                                }
                            }
                        }
                        break;
                    case "CUSTOMER":
                        CustomerBL customerBL = new CustomerBL(executionContext, commonId);
                        if (customerBL.CustomerDTO != null)
                        {
                            customerBL.CustomerDTO.CustomDataSetId = mainCustomDataSetId;
                            customerBL.CustomerDTO.IsChanged = true;
                            customerBL = new CustomerBL(executionContext, customerBL.CustomerDTO);
                            customerBL.Save(null);
                        }
                        break;
                    case "CARDGAMES":
                        AccountGameBL cardGames = new AccountGameBL(executionContext, commonId);
                        var accountGameDTO = cardGames.AccountGameDTO;
                        if (accountGameDTO.CustomDataSetId != 0)
                        {
                            accountGameDTO.CustomDataSetId = mainCustomDataSetId;
                            accountGameDTO.IsChanged = true;
                            cardGames = new AccountGameBL(executionContext, accountGameDTO);
                            cardGames.Validate();
                        }
                        break;
                    case "LOCATION":
                        LocationBL locationBL = new LocationBL(executionContext, commonId);
                        LocationDTO locationDTO = locationBL.GetLocationDTO;
                        if (locationBL.GetLocationDTO != null)
                        {
                            locationBL.GetLocationDTO.CustomDataSetId = mainCustomDataSetId;
                            locationBL.GetLocationDTO.IsChanged = true;
                            locationBL = new LocationBL(executionContext, locationBL.GetLocationDTO);
                            locationBL.Save(null);
                        }
                        break;
                    case "INVPRODUCT":
                        ProductList productList = new ProductList(executionContext);
                        List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productSearchList = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                        productSearchList.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, commonId.ToString()));
                        List<ProductDTO> productDTOList = productList.GetAllProducts(productSearchList);
                        if (productDTOList != null)
                        {
                            foreach (var item in productDTOList)
                            {
                                if (item.CustomDataSetId != 0)
                                {
                                    item.CustomDataSetId = mainCustomDataSetId;
                                    item.IsChanged = true;
                                    productList = new ProductList(executionContext, productDTOList);
                                    productList.Save();
                                }
                            }
                        }
                        break;
                }
                log.LogMethodExit(result);
                return result;
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

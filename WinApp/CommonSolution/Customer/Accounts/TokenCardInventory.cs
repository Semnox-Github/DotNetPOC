/********************************************************************************************
 * Project Name - TokenCardInventory
 * Description  - Business logic of TokenCardInventory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       6-July-2017    Amaresh          Created 
 *2.50.0     14-Dec-2018    Guru S A         Application security changes
 *2.60       22-Feb-2019    Nagesh Badiger   Added SaveInventory() & TokenCardInventoryList() parameterized Constructor.
 *2.60.2     22-May-2019    Jagan Moahan   
 *2.70.2        19-Jul-2019   Girish Kundar      Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// class of TokenCardInventory
    /// </summary>
    public class TokenCardInventory
    {
        private TokenCardInventoryDTO tokenCardInventoryDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>    
        /// <param name="executionContext"></param>
        public TokenCardInventory(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.tokenCardInventoryDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TokenCardInventory object using the TokenCardInventoryDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="tokenCardInventoryDTO">tokenCardInventoryDTO object</param>
        public TokenCardInventory(ExecutionContext executionContext, TokenCardInventoryDTO tokenCardInventoryDTO)
        {
            log.LogMethodEntry(executionContext, tokenCardInventoryDTO);
            this.executionContext = executionContext;
            this.tokenCardInventoryDTO = tokenCardInventoryDTO;
            log.LogMethodExit();
        } 


        /// <summary>
        /// Returns the Total card count issued till now
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int GetCardsIssued(int siteId , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            TokenCardInventoryDataHandler tokenCardInventoryDataHandler = new TokenCardInventoryDataHandler(sqlTransaction);
            log.LogMethodExit();
            return tokenCardInventoryDataHandler.GetCardsIssued(siteId);
        }

        /// <summary>
        /// Returns the cards stock count
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int GetCardStock(int siteId , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            TokenCardInventoryDataHandler tokenCardInventoryDataHandler = new TokenCardInventoryDataHandler(sqlTransaction);
            log.LogMethodExit();
            return tokenCardInventoryDataHandler.GetCardStock(siteId);
        }

        /// <summary>
        /// Saves the TokenCardInventory  
        /// TokenCardInventory   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            TokenCardInventoryDataHandler tokenCardInventoryDataHandler = new TokenCardInventoryDataHandler(sqlTransaction);
            ValidateAddCardKey();
            if (tokenCardInventoryDTO.cardInventoryKeyId < 0)
            {
                tokenCardInventoryDTO = tokenCardInventoryDataHandler.InsertTokenCardInventory(tokenCardInventoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                tokenCardInventoryDTO.AcceptChanges();
            }
            else
            {
                if (tokenCardInventoryDTO.IsChanged)
                {
                    tokenCardInventoryDTO = tokenCardInventoryDataHandler.UpdateTokenCardInventory(tokenCardInventoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    tokenCardInventoryDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void ValidateAddCardKey()
        {
            log.LogMethodEntry();
            if (this.tokenCardInventoryDTO != null && String.IsNullOrEmpty(this.tokenCardInventoryDTO.AddCardKey) == false)
            {
                TokenCardInventoryList tokenCardInventoryList = new TokenCardInventoryList(executionContext);
                List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParms = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
                searchParms.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParms.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ADDCARD_KEY, this.tokenCardInventoryDTO.AddCardKey));
                List<TokenCardInventoryDTO> tokenCardInventoryDTOList = tokenCardInventoryList.GetAllTokenCardInventory(searchParms);

                if (tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0 
                    && tokenCardInventoryDTOList.Exists( cardInv => String.IsNullOrEmpty(cardInv.AddCardKey) == false &&  cardInv.cardInventoryKeyId != this.tokenCardInventoryDTO.cardInventoryKeyId))
                {
                    log.Error("Add Cards Key is already used");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5075));
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of TokenCardInventory List
    /// </summary>
    public class TokenCardInventoryList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TokenCardInventoryDTO> tokenCardInventoryDTOList;
        private readonly ExecutionContext executionContext;
        int tagTypeToken = -1;
        int tagTypeCard = -1;

        int activityTypeOnHand = -1;
        int activityTypePurchase = -1;
        int activityTypeTransfer = -1;
        int activityTypeOther = -1;

        int posMachineType = -1;
        int kioskMachineType = -1;
        int otherMachineType = -1;

        /// <summary>
        /// Parameterized constructor
        /// </summary>    
        /// <param name="executionContext"></param>
        public TokenCardInventoryList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.tokenCardInventoryDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor to initialize TokenCardInventoryDTO and executionContext
        /// </summary>
        /// <param name="tokenCardInventoryDTOList"></param>
        /// <param name="executionContext"></param>
        public TokenCardInventoryList(List<TokenCardInventoryDTO> tokenCardInventoryDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(tokenCardInventoryDTOList, executionContext);
            this.executionContext = executionContext;
            this.tokenCardInventoryDTOList = tokenCardInventoryDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TokenCardInventoryDTO
        /// </summary>
        /// <param name="cardInventoryKey">cardInventoryKey</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>TokenCardInventoryDTO</returns>
        public TokenCardInventoryDTO GetTokenCardInventory(int cardInventoryKey ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardInventoryKey, sqlTransaction);
            TokenCardInventoryDataHandler tokenCardInventoryDataHandler = new TokenCardInventoryDataHandler(sqlTransaction);
            TokenCardInventoryDTO tokenCardInventoryDTO =  tokenCardInventoryDataHandler.GetTokenCardInventory(cardInventoryKey);
            log.LogMethodExit(tokenCardInventoryDTO);
            return tokenCardInventoryDTO;
        }

        /// <summary>
        /// Returns the List of TokenCardInventoryDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of TokenCardInventoryDTO</returns>
        public List<TokenCardInventoryDTO> GetAllTokenCardInventory(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TokenCardInventoryDataHandler tokenCardInventoryDataHandler = new TokenCardInventoryDataHandler(sqlTransaction);
            List<TokenCardInventoryDTO> tokenCardInventoryDTOList = tokenCardInventoryDataHandler.GetTokenCardInventoryList(searchParameters);
            log.LogMethodExit(tokenCardInventoryDTOList);
            return tokenCardInventoryDTOList;
        }

        /// <summary>
        /// Returns the List of TokenCardInventoryDTO for displaying in Report
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List TokenCardInventoryDTO</returns>
        public List<TokenCardInventoryDTO> GetReportTokenInventoryList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TokenCardInventoryDataHandler tokenCardInventoryDataHandler = new TokenCardInventoryDataHandler();
            List<TokenCardInventoryDTO> tokenCardInventoryDTOList = tokenCardInventoryDataHandler.GetTokenInventoryList(searchParameters);
            log.LogMethodExit(tokenCardInventoryDTOList);
            return tokenCardInventoryDTOList;
        }

        /// <summary>
        /// Returns Lookups collection
        /// </summary>
        /// <param name="lookupName">lookupName</param>
        /// <param name="lookupValue">lookupValue</param>
        /// <returns></returns>
        private LookupValuesDTO GetLookupValuesDTO(string lookupName, string lookupValue)//Added on 20 Mar by Nagesh
        {
            log.LogMethodEntry(lookupName, lookupValue);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> SearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            SearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(SearchParameters);

            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                LookupValuesDTO posLookUpDTO = lookupValuesDTOList.Find(x => x.LookupValue == lookupValue);

                if (posLookUpDTO != null)
                {
                    log.LogMethodExit(posLookUpDTO);
                    return posLookUpDTO;
                }
            }
            log.LogMethodExit(lookupValuesDTOList);
            return new LookupValuesDTO();
        }

        /// <summary>
        /// Update Lookup Values
        /// </summary>
        private void UpdateLookUpValues()
        {
            log.LogMethodEntry();
            tagTypeToken = GetLookupValuesDTO("TAG_TYPE", "TOKEN").LookupValueId;
            tagTypeCard = GetLookupValuesDTO("TAG_TYPE", "CARD").LookupValueId;

            activityTypeOnHand = GetLookupValuesDTO("ACTIVITY_TYPE", "ON_HAND").LookupValueId;
            activityTypePurchase = GetLookupValuesDTO("ACTIVITY_TYPE", "PURCHASE").LookupValueId;
            activityTypeTransfer = GetLookupValuesDTO("ACTIVITY_TYPE", "TRANSFER").LookupValueId;
            activityTypeOther = GetLookupValuesDTO("ACTIVITY_TYPE", "OTHER").LookupValueId;

            posMachineType = GetLookupValuesDTO("MACHINE_TYPE", "POS").LookupValueId;
            kioskMachineType = GetLookupValuesDTO("MACHINE_TYPE", "KIOSK").LookupValueId;
            otherMachineType = GetLookupValuesDTO("MACHINE_TYPE", "OTHER").LookupValueId;
            log.LogMethodExit();
        }

        /// <summary>
        /// To validate lookups values
        /// </summary>       
        private bool ValidateLookupValues()
        {
            log.LogMethodEntry();
            if (posMachineType == -1 || kioskMachineType == -1 || otherMachineType == -1 //Check MachineType
                || activityTypeOnHand == -1 || activityTypeTransfer == -1 || activityTypePurchase == -1 || activityTypeOther == -1 // Check activityType 
                || tagTypeCard == -1 || tagTypeToken == -1) //Check TagType
            {
                return false;
            }
            log.LogMethodExit();
            return true;
        }
        internal List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> SetFromSiteTimeOffset(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchparams = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
            if (searchParameters != null)
            {
                searchparams.AddRange(searchParameters);
            }
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (searchparams != null && searchparams.Any())
                {
                    foreach (KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string> searchParameter in searchparams)
                    {
                        if ((searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE))
                        {
                            if (!string.IsNullOrWhiteSpace(searchParameter.Value))
                            {
                                int index = searchParameters.IndexOf(searchParameter);
                                searchParameters[index] = new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(searchParameter.Key, SiteContainerList.ToSiteDateTime(executionContext.GetSiteId(), DateTime.Parse(searchParameter.Value)).ToString("MM-dd-yyyy hh", CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
            log.LogMethodEntry(searchParameters);
            return searchParameters;
        }
        internal List<TokenCardInventoryDTO> SetFromSiteTimeOffsetforSave(List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            log.LogMethodEntry(tokenCardInventoryDTOList);
            List<TokenCardInventoryDTO> tokenCardInventoryDTOs = new List<TokenCardInventoryDTO>();
            if (tokenCardInventoryDTOList != null)
            {
                tokenCardInventoryDTOs.AddRange(tokenCardInventoryDTOList);
            }
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (tokenCardInventoryDTOs != null && tokenCardInventoryDTOs.Any())
                {
                    foreach (TokenCardInventoryDTO tokenCardInventoryDTO in tokenCardInventoryDTOs)
                    {
                        if (tokenCardInventoryDTO.Actiondate != DateTime.MinValue)
                        {
                            tokenCardInventoryDTO.Actiondate = SiteContainerList.ToSiteDateTime(executionContext.GetSiteId(), tokenCardInventoryDTO.Actiondate);
                        }
                    }
                }
            }
            log.LogMethodEntry(tokenCardInventoryDTOs);
            return tokenCardInventoryDTOs;
        }
        /// <summary>
        /// Get all token card inventory details
        /// MachinesTypeName is setting based on the condition of lookupvalueId(machineType,tagType and activityType)
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>lstTokenInventoryDTO</returns>
        public List<TokenCardInventoryDTO> GetAllTokenCardInventoryDTOsList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters)//Added on 20 Mar 2019
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                UpdateLookUpValues();
                if (ValidateLookupValues())
                {
                    TokenCardInventoryList tokenCardInventoryList = new TokenCardInventoryList(executionContext);
                    SetFromSiteTimeOffset(searchParameters);
                    List<TokenCardInventoryDTO> tokenCardInventoryDTOList = tokenCardInventoryList.GetAllTokenCardInventory(searchParameters);
                    TokenCardInventoryDTO tokenCardInventoryDTO = new TokenCardInventoryDTO();

                    if (tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0)
                    {
                        tokenCardInventoryDTO = tokenCardInventoryDTOList.Find(x => x.MachineType == posMachineType);
                        if (tokenCardInventoryDTO != null)
                        {
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENPOS;
                        }
                        else
                        {
                            tokenCardInventoryDTO = new TokenCardInventoryDTO();
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENPOS;
                            tokenCardInventoryDTO.TagType = tagTypeToken;
                            tokenCardInventoryDTO.MachineType = posMachineType;
                            tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        }
                        tokenCardInventoryDTO = tokenCardInventoryDTOList.Find(x => x.MachineType == kioskMachineType);
                        if (tokenCardInventoryDTO != null)
                        {
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENKIOSK;
                        }
                        else
                        {
                            tokenCardInventoryDTO = new TokenCardInventoryDTO();
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENKIOSK;
                            tokenCardInventoryDTO.TagType = tagTypeToken;
                            tokenCardInventoryDTO.MachineType = kioskMachineType;
                            tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        }

                        tokenCardInventoryDTO = tokenCardInventoryDTOList.Find(x => x.TagType == tagTypeToken && x.MachineType == otherMachineType && x.ActivityType == activityTypeOnHand);
                        if (tokenCardInventoryDTO != null)
                        {
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENHAND;
                        }
                        else
                        {
                            tokenCardInventoryDTO = new TokenCardInventoryDTO();
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENHAND;
                            tokenCardInventoryDTO.TagType = tagTypeToken;
                            tokenCardInventoryDTO.MachineType = otherMachineType;
                            tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        }
                        tokenCardInventoryDTO = tokenCardInventoryDTOList.Find(x => x.TagType == tagTypeToken && x.MachineType == otherMachineType && x.ActivityType == activityTypeTransfer);
                        if (tokenCardInventoryDTO != null)
                        {
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TRANSFERREDTOKEN;
                        }
                        else
                        {
                            tokenCardInventoryDTO = new TokenCardInventoryDTO();
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TRANSFERREDTOKEN;
                            tokenCardInventoryDTO.TagType = tagTypeToken;
                            tokenCardInventoryDTO.MachineType = otherMachineType;
                            tokenCardInventoryDTO.ActivityType = activityTypeTransfer;
                            tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        }

                        tokenCardInventoryDTO = tokenCardInventoryDTOList.Find(x => x.TagType == tagTypeCard && x.MachineType == otherMachineType && x.ActivityType == activityTypeOnHand);
                        if (tokenCardInventoryDTO != null)
                        {
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.CARDSONHAND;
                        }
                        else
                        {
                            tokenCardInventoryDTO = new TokenCardInventoryDTO();
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.CARDSONHAND;
                            tokenCardInventoryDTO.TagType = tagTypeCard;
                            tokenCardInventoryDTO.MachineType = otherMachineType;
                            tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        }

                        tokenCardInventoryDTO = tokenCardInventoryDTOList.Find(x => x.TagType == tagTypeCard && x.MachineType == otherMachineType && x.ActivityType == activityTypePurchase);
                        if (tokenCardInventoryDTO != null)
                        {
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.CARDPURCHASED;
                        }
                        else
                        {
                            tokenCardInventoryDTO = new TokenCardInventoryDTO();
                            tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.CARDPURCHASED;
                            tokenCardInventoryDTO.TagType = tagTypeCard;
                            tokenCardInventoryDTO.MachineType = otherMachineType;
                            tokenCardInventoryDTO.ActivityType = activityTypePurchase;
                            tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        }
                    }
                    else
                    {
                        tokenCardInventoryDTOList = new List<TokenCardInventoryDTO>();
                        tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENPOS;
                        tokenCardInventoryDTO.TagType = tagTypeToken;
                        tokenCardInventoryDTO.MachineType = posMachineType;
                        tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                        tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        tokenCardInventoryDTO = new TokenCardInventoryDTO();
                        tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENKIOSK;
                        tokenCardInventoryDTO.TagType = tagTypeToken;
                        tokenCardInventoryDTO.MachineType = kioskMachineType;
                        tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                        tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        tokenCardInventoryDTO = new TokenCardInventoryDTO();
                        tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TOKENHAND;
                        tokenCardInventoryDTO.TagType = tagTypeToken;
                        tokenCardInventoryDTO.MachineType = otherMachineType;
                        tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                        tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        tokenCardInventoryDTO = new TokenCardInventoryDTO();
                        tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.TRANSFERREDTOKEN;
                        tokenCardInventoryDTO.TagType = tagTypeToken;
                        tokenCardInventoryDTO.MachineType = otherMachineType;
                        tokenCardInventoryDTO.ActivityType = activityTypeTransfer;
                        tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        tokenCardInventoryDTO = new TokenCardInventoryDTO();
                        tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.CARDSONHAND;
                        tokenCardInventoryDTO.TagType = tagTypeCard;
                        tokenCardInventoryDTO.MachineType = otherMachineType;
                        tokenCardInventoryDTO.ActivityType = activityTypeOnHand;
                        tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                        tokenCardInventoryDTO = new TokenCardInventoryDTO();
                        tokenCardInventoryDTO.MachinesTypeNames = TokenCardInventoryDTO.MachinesTypeName.CARDPURCHASED;
                        tokenCardInventoryDTO.TagType = tagTypeCard;
                        tokenCardInventoryDTO.MachineType = otherMachineType;
                        tokenCardInventoryDTO.ActivityType = activityTypePurchase;
                        tokenCardInventoryDTOList.Add(tokenCardInventoryDTO);
                    }
                    log.LogMethodExit(tokenCardInventoryDTOList);

                    return tokenCardInventoryDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1198));
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Save or update the card inventory details
        /// </summary>
        public void SaveUpdateCardInventory()
        {
            log.LogMethodEntry();
            try
            { 
                if (tokenCardInventoryDTOList != null)
                {
                    SetFromSiteTimeOffsetforSave(tokenCardInventoryDTOList);
                    foreach (TokenCardInventoryDTO tokenCardInventoryDTO in tokenCardInventoryDTOList)
                    {
                        try
                        {
                            TokenCardInventory tokenCardInventory = new TokenCardInventory(executionContext, tokenCardInventoryDTO);
                            tokenCardInventory.Save();
                        }
                        catch
                        {
                            //if (tokenCardInventoryDTO.Action!=null && tokenCardInventoryDTO.Action.ToUpper().ToString() == "REDUCE")
                            //{
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 588));
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }
    }
}

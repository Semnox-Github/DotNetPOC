/********************************************************************************************
 * Project Name - ProductKey Srvice BL
 * Description  - Contains helper methods to add cards
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70        23-Oct-2019      Rakesh          Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Site;
using System.Text;

namespace Semnox.Parafait.Customer
{
    public class ProductKeyServiceBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ProductKeyServiceBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public int SaveMaxCards(int addCards, string siteKey, string cardValue)
        {
            log.LogMethodEntry(addCards, siteKey, cardValue);
            DataAccessHandler dataAccessHandle = new DataAccessHandler();
            SiteList siteList = new SiteList(executionContext);
            AccountListBL accountListBL = new AccountListBL(executionContext);
            AccountSearchCriteria accountSearchCriteria = null;
            Semnox.Parafait.Site.Site site = new Semnox.Parafait.Site.Site(executionContext, executionContext.GetSitePKId(), null);
            string MaxCardsEncrypted = "";
            int MaxCards = 0;
            try
            {
                if (addCards != 0)
                {
                    MaxCardsEncrypted = site.getSitedTO.MaxCards;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(MaxCardsEncrypted) == false)
                        {
                            MaxCards = Convert.ToInt32(Encryption.Decrypt(MaxCardsEncrypted, siteKey.PadRight(8, '0').Substring(0, 8)));
                        }
                        else
                        {
                            MaxCards = 10000;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to Decrypt", ex);
                        MaxCards = 0;
                    }
                    if (addCards < 0)
                    {
                        int siteId = executionContext.SitePKId;
                        string numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
                        accountSearchCriteria = new AccountSearchCriteria();
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.NOT_LIKE, "T%");
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "Y");
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.SITE_ID, Operator.EQUAL_TO, siteId);
                        List<AccountDTO> accountDTOs = accountListBL.GetAccountDTOList(accountSearchCriteria);
                        accountSearchCriteria = new AccountSearchCriteria();
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.NOT_LIKE, "T%");
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "N");
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.REFUND_FLAG, Operator.EQUAL_TO, "N");
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.SITE_ID, Operator.EQUAL_TO, siteId);
                        accountSearchCriteria.And(AccountDTO.SearchByParameters.EXPIRY_DATE, Operator.LESSER_THAN, DateTime.Now.Date);
                        List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria);
                        int issuedCards = (accountDTOs != null ? accountDTOs.Count : 0) + (accountDTOList != null ? accountDTOList.Count : 0);

                        if (issuedCards > MaxCards + addCards)
                        {
                            throw new Exception("Revised Max Cards value (" + (MaxCards + addCards).ToString(numberFormat) + ") can not be more than total issued cards (" + issuedCards.ToString(numberFormat) + ") ");
                        }
                    }
                    CreateCardInventoryEntry(addCards, cardValue);
                    string encrypted = Encryption.Encrypt((MaxCards + addCards).ToString(), siteKey.PadRight(8, '0').Substring(0, 8));
                    site.getSitedTO.MaxCards = encrypted;
                    site.Save();
                    MaxCards += addCards;
                }
                log.LogMethodExit(MaxCards);
                return MaxCards;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void CreateCardInventoryEntry(int addCards, string cardValue)
        {
            log.LogMethodEntry(addCards, cardValue);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "TAG_TYPE"));
            searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "CARD"));
            searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> tagTypeList = lookupValuesList.GetAllLookupValues(searchParams);
            searchParams.Clear();
            searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ACTIVITY_TYPE"));
            searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "PURCHASE"));
            searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> cardActivityTypeList = lookupValuesList.GetAllLookupValues(searchParams);
            int tagType = -1;
            if (tagTypeList != null && tagTypeList.Count > 0)
            {
                tagType = tagTypeList[0].LookupValueId;
            }
            int activityType = -1;
            if (cardActivityTypeList != null && cardActivityTypeList.Count > 0)
            {
                activityType = cardActivityTypeList[0].LookupValueId;
            }
            TokenCardInventoryDTO tokenCardInventoryDTO = new TokenCardInventoryDTO(-1, "", "", addCards, DateTime.Now, executionContext.GetUserId(), "Add", "", false, -1, tagType, -1, activityType, DateTime.Now, executionContext.GetUserId(), executionContext.GetSiteId(), DateTime.Now, executionContext.GetUserId(), cardValue);
            TokenCardInventory tokenCardInventoryBL = new TokenCardInventory(executionContext, tokenCardInventoryDTO);
            tokenCardInventoryBL.Save();
            log.LogMethodExit();
        }
    }
}
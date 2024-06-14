/********************************************************************************************
 * Project Name - Product
 * Description  - TransactionProfileContainer class to get the data from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0     1-Sep-2021       Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class TransactionProfileContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, TransactionProfileContainerDTO> transactionProfileContainerDTODictionary = new Dictionary<int, TransactionProfileContainerDTO>();
        private readonly TransactionProfileContainerDTOCollection transactionProfileContainerDTOCollection;
        private readonly DateTime? transactionProfileModuleLastUpdateTime;
        private readonly int siteId;
        private readonly List<TransactionProfileDTO> transactionProfileDTOList;

        public TransactionProfileContainer(int siteId) : this(siteId, GetTransactionProfileDTOList(siteId), GetTransactionProfileModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public TransactionProfileContainer(int siteId, List<TransactionProfileDTO> transactionProfileDTOList, DateTime? transactionProfileModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.transactionProfileDTOList = transactionProfileDTOList;
            this.transactionProfileModuleLastUpdateTime = transactionProfileModuleLastUpdateTime;
            List<TransactionProfileContainerDTO> transactionProfileContainerDTOList = new List<TransactionProfileContainerDTO>();
            foreach (TransactionProfileDTO transactionProfileDTO in transactionProfileDTOList)
            {
                TransactionProfileContainerDTO transactionProfileContainerDTO = new TransactionProfileContainerDTO(transactionProfileDTO.TransactionProfileId, transactionProfileDTO.ProfileName, transactionProfileDTO.VerificationRequired, transactionProfileDTO.PriceListId);
                List<TransactionProfileTaxRuleContainerDTO> transactionProfileTaxRuleContainerDTOList = new List<TransactionProfileTaxRuleContainerDTO>();
                if (transactionProfileDTO.TransactionProfileTaxRulesDTOList != null)
                {
                    
                    foreach (var transactionProfileTaxRulesDTO in transactionProfileDTO.TransactionProfileTaxRulesDTOList)
                    {
                        TransactionProfileTaxRuleContainerDTO transactionProfileTaxRuleContainerDTO = new TransactionProfileTaxRuleContainerDTO(transactionProfileTaxRulesDTO.Id, 
                                                                                                                                                transactionProfileTaxRulesDTO.TrxProfileId,
                                                                                                                                                transactionProfileTaxRulesDTO.TaxId,
                                                                                                                                                transactionProfileTaxRulesDTO.TaxStructure,
                                                                                                                                                transactionProfileTaxRulesDTO.Exempt == "Y");
                        transactionProfileTaxRuleContainerDTOList.Add(transactionProfileTaxRuleContainerDTO);
                    }
                }
                transactionProfileContainerDTO.TransactionProfileTaxRuleContainerDTOList = transactionProfileTaxRuleContainerDTOList;
                transactionProfileContainerDTOList.Add(transactionProfileContainerDTO);
                transactionProfileContainerDTODictionary.Add(transactionProfileDTO.TransactionProfileId, transactionProfileContainerDTO);

            }
            transactionProfileContainerDTOCollection = new TransactionProfileContainerDTOCollection(transactionProfileContainerDTOList);
            log.LogMethodExit();
        }

        private static List<TransactionProfileDTO> GetTransactionProfileDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<TransactionProfileDTO> transactionProfileDTOList = null;
            try
            {
                TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
                List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the transaction profile.", ex);
            }

            if (transactionProfileDTOList == null)
            {
                transactionProfileDTOList = new List<TransactionProfileDTO>();
            }
            log.LogMethodExit(transactionProfileDTOList);
            return transactionProfileDTOList;
        }

        internal List<TransactionProfileContainerDTO> GetTransactionProfileContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(transactionProfileContainerDTOCollection.TransactionProfileContainerDTOList);
            return transactionProfileContainerDTOCollection.TransactionProfileContainerDTOList;
        }

        private static DateTime? GetTransactionProfileModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
                result = transactionProfileListBL.GetTransactionProfileModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the transaction profile max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public TransactionProfileContainerDTO GetTransactionProfileContainerDTO(int transactionProfileId)
        {
            log.LogMethodEntry(transactionProfileId);
            if (transactionProfileContainerDTODictionary.ContainsKey(transactionProfileId) == false)
            {
                string errorMessage = "transaction profile with Product Type Id :" + transactionProfileId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            TransactionProfileContainerDTO result = transactionProfileContainerDTODictionary[transactionProfileId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public TransactionProfileContainerDTOCollection GetTransactionProfileContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(transactionProfileContainerDTOCollection);
            return transactionProfileContainerDTOCollection;
        }

        public TransactionProfileContainer Refresh()
        {
            log.LogMethodEntry();
            TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
            DateTime? updateTime = transactionProfileListBL.GetTransactionProfileModuleLastUpdateTime(siteId);
            if (transactionProfileModuleLastUpdateTime.HasValue
                && transactionProfileModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in transaction profile since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            TransactionProfileContainer result = new TransactionProfileContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

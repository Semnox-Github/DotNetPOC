/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - RemoteTokenCardInventoryUseCase
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Customer.Accounts
{
    public class RemoteTokenCardInventoryUseCases : RemoteUseCases, ITokenCardInventoryUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TOKEN_CARD_INVENTORY_URL = "api/Task/TokenCardInventory";

        public RemoteTokenCardInventoryUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        

        

        public async Task<List<TokenCardInventoryDTO>> GetAllTokenCardInventoryDTOsList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            
            searchParameterList.AddRange(BuildSearchParameter(searchParameters));

            try
            {
                List<TokenCardInventoryDTO> tokenCardInventoryDTOs = await Get<List<TokenCardInventoryDTO>>(TOKEN_CARD_INVENTORY_URL, searchParameterList);
                log.LogMethodExit(tokenCardInventoryDTOs);
                return tokenCardInventoryDTOs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<TokenCardInventoryDTO>> SaveCardInventory(List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            log.LogMethodEntry(tokenCardInventoryDTOList);
            try
            {
                if (tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0)
                {
                    foreach (TokenCardInventoryDTO tokenCardInventoryDTO in tokenCardInventoryDTOList)
                    {
                        tokenCardInventoryDTO.Actiondate = SiteContainerList.FromSiteDateTime(this.executionContext.SiteId, tokenCardInventoryDTO.Actiondate);
                    }
                }
                List<TokenCardInventoryDTO> tokenCardInventoryDTOlist = await Post<List<TokenCardInventoryDTO>>(TOKEN_CARD_INVENTORY_URL, tokenCardInventoryDTOList);
                log.LogMethodExit();
                return tokenCardInventoryDTOlist;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<TokenCardInventoryDTO>> UpdateCardInventory(List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            log.LogMethodEntry(tokenCardInventoryDTOList);
            try
            {
                if (tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0)
                {
                    foreach (TokenCardInventoryDTO tokenCardInventoryDTO in tokenCardInventoryDTOList)
                    {
                        tokenCardInventoryDTO.Actiondate = SiteContainerList.FromSiteDateTime(this.executionContext.SiteId, tokenCardInventoryDTO.Actiondate);
                    }
                }
                List<TokenCardInventoryDTO> list = await Put<List<TokenCardInventoryDTO>>(TOKEN_CARD_INVENTORY_URL, tokenCardInventoryDTOList);
                log.LogMethodExit();
                return list;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> tokenCardInventorySearchParams)
        {
            log.LogMethodEntry(tokenCardInventorySearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string> searchParameter in tokenCardInventorySearchParams)
            {
                switch (searchParameter.Key)
                {

                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE:
                        {
                            DateTime siteDatetime = Convert.ToDateTime(searchParameter.Value);
                            siteDatetime = SiteContainerList.FromSiteDateTime(executionContext.SiteId, siteDatetime);
                            searchParameterList.Add(new KeyValuePair<string, string>("date".ToString(), siteDatetime.ToString()));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("action".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTIVITY_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activityType".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ADDCARD_KEY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("addCardKey".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.CARD_INVENTORY_KEY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardInventoryKey".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.FROM_DATE:
                        {
                            DateTime siteFromDatetime = Convert.ToDateTime(searchParameter.Value);
                            siteFromDatetime = SiteContainerList.FromSiteDateTime(executionContext.SiteId, siteFromDatetime);
                            searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), siteFromDatetime.ToString()));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MACHINE_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineType".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MASTER_ENTITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterEntityId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TAG_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tagType".ToString(), searchParameter.Value));
                        }
                        break;
                    case TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TO_DATE:
                        {
                            DateTime siteToDatetime = Convert.ToDateTime(searchParameter.Value);
                            siteToDatetime = SiteContainerList.FromSiteDateTime(executionContext.SiteId, siteToDatetime);
                            searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), siteToDatetime.ToString()));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}
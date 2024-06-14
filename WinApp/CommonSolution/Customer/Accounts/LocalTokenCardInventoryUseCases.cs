/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - LocalTokenCardInventoryUseCase
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public class LocalTokenCardInventoryUseCases : LocalUseCases, ITokenCardInventoryUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        public LocalTokenCardInventoryUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<TokenCardInventoryDTO>> GetAllTokenCardInventoryDTOsList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            return await Task<List<TokenCardInventoryDTO>>.Factory.StartNew(() =>
            {
                TokenCardInventoryList tokenCardInventoryList = null;
                List<TokenCardInventoryDTO> tokenCardInventoryDTOs = null;                
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        tokenCardInventoryList = new TokenCardInventoryList(executionContext);
                        tokenCardInventoryDTOs = (tokenCardInventoryList.GetAllTokenCardInventoryDTOsList(searchParameters)).OrderByDescending((x) => x.LastUpdatedDate).ToList();
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(tokenCardInventoryDTOs);
                //if(tokenCardInventoryDTOs != null && tokenCardInventoryDTOs.Count > 0)
                //{
                //    foreach (TokenCardInventoryDTO tokenCardInventoryDTO in tokenCardInventoryDTOs)
                //    {
                //        tokenCardInventoryDTO.Actiondate = SiteContainerList.ToSiteDateTime(executionContext.SiteId, tokenCardInventoryDTO.Actiondate);
                //        //tokenCardInventoryDTO.CreationDate = SiteContainerList.ToSiteDateTime(executionContext.SiteId, tokenCardInventoryDTO.CreationDate);
                //        //tokenCardInventoryDTO.LastUpdatedDate = SiteContainerList.ToSiteDateTime(executionContext.SiteId, tokenCardInventoryDTO.LastUpdatedDate);
                //    }
                //}
                return tokenCardInventoryDTOs;
            });
        }

        public async Task<List<TokenCardInventoryDTO>> SaveCardInventory(List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            log.LogMethodEntry(tokenCardInventoryDTOList);
            return await Task<List<TokenCardInventoryDTO>>.Factory.StartNew(() =>
            {
                TokenCardInventoryList tokenCardInventoryList = null;
                if(tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0)
                {
                    foreach(TokenCardInventoryDTO tokenCardInventoryDTO in tokenCardInventoryDTOList)
                    {
                        tokenCardInventoryDTO.Actiondate = SiteContainerList.FromSiteDateTime(this.executionContext.SiteId, tokenCardInventoryDTO.Actiondate);
                    }
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        tokenCardInventoryList = new TokenCardInventoryList(tokenCardInventoryDTOList, executionContext);
                        tokenCardInventoryList.SaveUpdateCardInventory();
                        parafaitDBTrx.EndTransaction();
                        
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(tokenCardInventoryDTOList);
                return tokenCardInventoryDTOList;
            });
                

        }

       

        public async Task<List<TokenCardInventoryDTO>> UpdateCardInventory(List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            log.LogMethodEntry(tokenCardInventoryDTOList);
            return await Task<List<TokenCardInventoryDTO>>.Factory.StartNew(() =>
            {
                TokenCardInventoryList tokenCardInventoryList = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        tokenCardInventoryList = new TokenCardInventoryList(tokenCardInventoryDTOList, executionContext);
                        tokenCardInventoryList.SaveUpdateCardInventory();
                        parafaitDBTrx.EndTransaction();                        
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(tokenCardInventoryDTOList);
                return tokenCardInventoryDTOList;
            });               
        }
    }
}

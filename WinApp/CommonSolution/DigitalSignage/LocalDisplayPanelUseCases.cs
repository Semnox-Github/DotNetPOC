/********************************************************************************************
* Project Name -DigitalSignage
* Description  - LocalDisplayPanelUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.140.00    21-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Implementation of displayPanel use-cases
    /// </summary>
    public class LocalDisplayPanelUseCases:IDisplayPanelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalDisplayPanelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<DisplayPanelDTO>> GetDisplayPanels(List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>
                        searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<DisplayPanelDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);

                DisplayPanelListBL panelListBL = new DisplayPanelListBL(executionContext);
                List<DisplayPanelDTO> displayPanelDTOList = panelListBL.GetDisplayPanelDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(displayPanelDTOList);
                return displayPanelDTOList;
            });
        }
        public async Task<string> SaveDisplayPanels(List<DisplayPanelDTO> displayPanelDTOList)
        {
          return await Task<string>.Factory.StartNew(() =>
                {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(displayPanelDTOList);
                    if (displayPanelDTOList == null)
                    {
                        throw new ValidationException("displayPanelDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DisplayPanelListBL displayPanelList = new DisplayPanelListBL(executionContext, displayPanelDTOList);
                            displayPanelList.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                        result = "Success";
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        result = "Falied";
                    }
                    log.LogMethodExit(result);
                    return result;
                });
        }
        public async Task<string> SaveStartPCs(List<DisplayPanelDTO> displayPanelDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(displayPanelDTOList);
                    if (displayPanelDTOList == null)
                    {
                        throw new ValidationException("displayPanelDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DisplayPanelListBL displayPanelBL = new DisplayPanelListBL(executionContext, displayPanelDTOList);
                            displayPanelBL.DisplayPanelListStartPC();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> SaveShutdownPCs(List<DisplayPanelDTO> displayPanelDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(displayPanelDTOList);
                    if (displayPanelDTOList == null)
                    {
                        throw new ValidationException("displayPanelDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DisplayPanelListBL displayPanelBL = new DisplayPanelListBL(executionContext, displayPanelDTOList);
                            displayPanelBL.DisplayPanelListShutdownPC();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// GetDisplayPanelContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns>DisplayPanelContainerDTOCollection</returns>
        public async Task<DisplayPanelContainerDTOCollection> GetDisplayPanelContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<DisplayPanelContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    DisplayPanelContainerList.Rebuild(siteId);
                }
                DisplayPanelContainerDTOCollection result = DisplayPanelContainerList.GetDisplayPanelContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

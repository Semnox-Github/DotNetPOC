/********************************************************************************************
 * Project Name - Product
 * Description  - LocalModifierSetUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    08-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 *2.140.00   14-Sep-2021      Prajwal S                   Modified : Container Get Use cases.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalModifierSetUseCases:IModifierSetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalModifierSetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ModifierSetDTO>> GetModifierSets(List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>
                         searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ModifierSetDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters,loadChildRecords,loadActiveChildRecords,sqlTransaction);

                ModifierSetDTOList modifierSetDetails = new ModifierSetDTOList(executionContext);
                List<ModifierSetDTO> modifierSetDTOList = modifierSetDetails.GetAllModifierSetDTOList(searchParameters, loadChildRecords, loadActiveChildRecords);

                log.LogMethodExit(modifierSetDTOList);
                return modifierSetDTOList;
            });
        }
        public async Task<string> SaveModifierSets(List<ModifierSetDTO> modifierSetDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(modifierSetDTOList);
                    if (modifierSetDTOList == null)
                    {
                        throw new ValidationException("modifierSetDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ModifierSetDTOList modifierSetDetails = new ModifierSetDTOList(executionContext, modifierSetDTOList);
                            modifierSetDetails.Save();
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
                    throw;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> Delete(List<ModifierSetDTO> modifierSetDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(modifierSetDTOList);
                    ModifierSetDTOList modifierSetDetails = new ModifierSetDTOList(executionContext, modifierSetDTOList);
                    modifierSetDetails.Delete();
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
        /// Gets Container Data
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public async Task<ModifierSetContainerDTOCollection> GetModifierSetContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<ModifierSetContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                   ModifierSetContainerList.Rebuild(siteId);
                }
                ModifierSetContainerDTOCollection result = ModifierSetContainerList.GetModifierSetContainerDTOCollection(siteId);
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

/********************************************************************************************
 * Project Name - Device  
 * Description  - LocalEnabledAttributesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class LocalEnabledAttributesUseCases : LocalUseCases, IEnabledAttributesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalEnabledAttributesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<EnabledAttributesDTO>> GetEnabledAttributes(List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<EnabledAttributesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                EnabledAttibutesListBL enabledAttributessListBL = new EnabledAttibutesListBL(executionContext);
                
                List<EnabledAttributesDTO> enabledAttributessDTOList = enabledAttributessListBL.GetEnabledAttibutes(searchParameters);
                log.LogMethodExit(enabledAttributessDTOList);
                return enabledAttributessDTOList;
            });
        }

        public async Task<EnabledAttributesContainerDTOCollection> GetEnabledAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<EnabledAttributesContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    EnabledAttributesContainerList.Rebuild(siteId);
                }
                EnabledAttributesContainerDTOCollection result = EnabledAttributesContainerList.GetEnabledAttributesContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> SaveEnabledAttributes(List<EnabledAttributesDTO> enabledAttributesDTOList)
        {
            log.LogMethodEntry(enabledAttributesDTOList);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (enabledAttributesDTOList == null)
                {
                    throw new ValidationException("enabledAttributesDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        EnabledAttibutesListBL enabledAttributesList = new EnabledAttibutesListBL(executionContext, enabledAttributesDTOList);
                        enabledAttributesList.Save(parafaitDBTrx.SQLTrx);
                    }

                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

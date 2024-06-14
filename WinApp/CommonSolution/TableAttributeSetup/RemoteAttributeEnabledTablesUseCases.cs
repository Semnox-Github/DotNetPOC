/********************************************************************************************
 * Project Name - Device  
 * Description  - RemoteAttributeEnabledTablesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      24-Aug-2021      Fiona                     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class RemoteAttributeEnabledTablesUseCases : RemoteUseCases, IAttributeEnabledTablesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ATTRIBUTES_ENABLED_TABLES_CONTAINER_URL = "api/TableAttributeSetup/AttributeEnabledTablesContainer";
        private const string ATTRIBUTES_ENABLED_TABLES_URL = "api/TableAttributeSetUp/AttributeEnabledTables";

        public RemoteAttributeEnabledTablesUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public async Task<AttributeEnabledTablesContainerDTOCollection> GetAttributeEnabledTablesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            AttributeEnabledTablesContainerDTOCollection result = await Get<AttributeEnabledTablesContainerDTOCollection>(ATTRIBUTES_ENABLED_TABLES_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<string> SaveAttributeEnabledTables(List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList)
        {
            log.LogMethodEntry(attributeEnabledTablesDTOList);
            try
            {
                string responseString = await Post<string>(ATTRIBUTES_ENABLED_TABLES_URL, attributeEnabledTablesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<AttributeEnabledTablesDTO>> GetAttributeEnabledTables(List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool loadActiveChild = false)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<AttributeEnabledTablesDTO> result = await Get<List<AttributeEnabledTablesDTO>>(ATTRIBUTES_ENABLED_TABLES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private IEnumerable<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string> searchParameter in parameters)
            {
                switch (searchParameter.Key)
                {
                    case AttributeEnabledTablesDTO.SearchByParameters.ATTRIBUTE_ENABLED_TABLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("enabledAttributeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttributeEnabledTablesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttributeEnabledTablesDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}

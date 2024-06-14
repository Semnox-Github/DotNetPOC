/********************************************************************************************
 * Project Name - TableAttributeDetailsUtils  
 * Description  - RemoteTableAttributeDetailsUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      13-Sep-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.Parafait.TableAttributeDetailsUtils
{
    public class RemoteTableAttributeDetailsUseCases : RemoteUseCases, ITableAttributeDetailsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TABLE_ATTRIBUTE_DETAIL_URL_SQL_DATA_LIST = "api/TableAttributeSetup/GetSQLDataList";
        private const string TABLE_ATTRIBUTE_DETAIL_URL = "api/TableAttributeSetup/TableAtrributeDetails";

        public RemoteTableAttributeDetailsUseCases(ExecutionContext executionContext)
        : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetSQLDataList
        /// </summary>
        /// <param name="sqlSource"></param>
        /// <param name="sqlDisplayMember"></param>
        /// <param name="sqlValueMember"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetSQLDataList(string sqlSource, string sqlDisplayMember, string sqlValueMember)
        {
            log.LogMethodEntry(sqlSource, sqlDisplayMember, sqlValueMember);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("sqlSource".ToString(), sqlSource.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("sqlDisplayMember".ToString(), sqlDisplayMember.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("sqlValueMember".ToString(), sqlValueMember.ToString()));
            try
            {
                List<KeyValuePair<string, string>> result = await Get<List<KeyValuePair<string, string>>>(TABLE_ATTRIBUTE_DETAIL_URL_SQL_DATA_LIST, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// GetTableAttributeDetailsDTOList
        /// </summary>
        /// <param name="attributeEnabledTable"></param>
        /// <param name="enabledTableName"></param>
        /// <param name="recordGuid"></param>
        /// <returns></returns>
        public async Task<List<TableAttributeDetailsDTO>> GetTableAttributeDetailsDTOList(EnabledAttributesDTO.TableWithEnabledAttributes enabledTableName, string recordGuid)
        {
            log.LogMethodEntry(enabledTableName, recordGuid);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            //searchParameterList.Add(new KeyValuePair<string, string>("attributeEnabledTable".ToString(), attributeEnabledTable.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("enabledTableName".ToString(), enabledTableName.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("recordGuid".ToString(), recordGuid.ToString()));
            try
            {
                List<TableAttributeDetailsDTO> result = await Get<List<TableAttributeDetailsDTO>>(TABLE_ATTRIBUTE_DETAIL_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

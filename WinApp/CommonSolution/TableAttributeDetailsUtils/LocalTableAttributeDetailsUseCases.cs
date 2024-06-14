/********************************************************************************************
 * Project Name - TableAttributeDetailsUtils  
 * Description  - LocalTableAttributeDetailsUseCases class to get the data  from API by doing remote call  
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
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.Parafait.TableAttributeDetailsUtils
{
    public class LocalTableAttributeDetailsUseCases : LocalUseCases, ITableAttributeDetailsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalTableAttributeDetailsUseCases(ExecutionContext executionContext)
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
            return await Task<List<KeyValuePair<string, string>>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(sqlSource, sqlDisplayMember, sqlValueMember);
                TableAttributeDetailsListBL tableAttributeDetailsListBL = new TableAttributeDetailsListBL();
                List<KeyValuePair<string, string>> result = tableAttributeDetailsListBL.GetSQLDataList(sqlSource, sqlDisplayMember, sqlValueMember);
                return result;
            });

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
            return await Task<List<TableAttributeDetailsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(enabledTableName, recordGuid);
                TableAttributeDetailsListBL tableAttributeDetailsListBL = new TableAttributeDetailsListBL(executionContext);
                List<TableAttributeDetailsDTO> result = tableAttributeDetailsListBL.GetTableAttributeDetailsDTOList(enabledTableName, recordGuid);
                return result;
            });
        }
    }
}

/********************************************************************************************
 * Project Name - CommonLookupBL                                                                         
 * Description  - BL for the Lookups tables.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.50        20-Nov-2018    Jagan Mohana          Created new GetLookUpMasterDataList class to hold the all lookup values
 *                                                 for all dropdowns in the all modules
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.DigitalSignage
{
    public class CommonLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityNames;
        private string moduleName;
        private ExecutionContext executionContext;

        /// <summary>
        /// Constructor for the method CommonLookupBL()
        /// </summary>
        /// <param name="masterLookupDataList"></param>
        /// <param name="executioncontext"></param>
        public CommonLookupBL(string moduleName, string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.moduleName = moduleName;
            entityNames = entityName;
            executionContext = executioncontext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the All look ups for all dropdowns based on the page in the all module.
        /// </summary>       
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> commonLookupsList = new List<CommonLookupsDTO>();
                switch (moduleName.ToUpper().ToString())
                {
                    case "GAMES":
                        break;                        
                    case "DIGITALSIGNAGE":
                        DigitalSignageLookupBL lokkupList = new DigitalSignageLookupBL(entityNames, executionContext);
                        commonLookupsList = lokkupList.GetLookUpMasterDataList();
                        break;
                    case "PRODUCTS":
                        break;                        
                }
                log.LogMethodExit(commonLookupsList);
                return commonLookupsList;
            }
            catch
            {
                throw;
            }
        }
    }
}
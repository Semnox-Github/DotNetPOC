/********************************************************************************************
 * Project Name - ManagementStudioSwitch
 * Description  - Class to handle enable / disable for windows management studio app
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        04-May-2019      Guru S A       Created
 *2.70.3      17-Feb-2020      Deeksha        Modified to Make DigitalSignage & Cards module 
 *                                            read only in Windows Management Studio.
 *2.80        24-Jun-2020      Deeksha        Modified to Make Product module 
 *                                            read only in Windows Management Studio.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    public class ManagementStudioSwitch
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private bool enableGameModule;
        private bool enableDigitalSignageModule;
        private bool enableCardsModule;
        private bool enablProductModule;

        public bool EnableGameModule {get {return enableGameModule;} }

        public bool EnableDigitalSignageModule { get { return enableDigitalSignageModule; } }

        public bool EnableCardsModule {  get { return enableCardsModule; } }

        public bool EnablProductModule { get { return enablProductModule; } }

        public ManagementStudioSwitch(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.enableGameModule = true;
            this.enableDigitalSignageModule = true;
            this.enableCardsModule = true;
            this.enablProductModule = true;
            SetModuleAccess();
            log.LogMethodExit();
        }
        private void SetModuleAccess()
        {
            log.LogMethodEntry();
            List<SystemOptionsDTO> systemOptionsDTOs;
            SystemOptionsList systemOptionsList = new SystemOptionsList(executionContext);
            List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.OPTION_TYPE, "Management Studio Access"));
            searchParameter.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            systemOptionsDTOs = systemOptionsList.GetSystemOptionsDTOList(searchParameter);
            if (systemOptionsDTOs != null && systemOptionsDTOs.Count > 0)
            {
                foreach (SystemOptionsDTO systemOptionsDTO in systemOptionsDTOs)
                {
                    switch (systemOptionsDTO.OptionName)
                    {
                        case "Game Module":
                            enableGameModule = (systemOptionsDTO.OptionValue.ToString() == "1" ? true : false);
                            break;
                        case "DigitalSignage Module":
                            enableDigitalSignageModule = (systemOptionsDTO.OptionValue.ToString() == "1" ? true : false);
                            break;
                        case "Cards Module":
                            enableCardsModule = (systemOptionsDTO.OptionValue.ToString() == "1" ? true : false);
                            break;
                        case "Product Module":
                            enablProductModule = (systemOptionsDTO.OptionValue.ToString() == "1" ? true : false);
                            break;
                        default:
                            break;
                    }
                }
            } 
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - External Card System Factory                                                      
 * Description  - External Card System Factory
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100.0       19-Aug-2020   Dakshakh             Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;


namespace Parafait_POS
{
    /// <summary>
    /// External Card Systems which parafait supports  
    /// </summary>
    public enum ExternalCardSystems
    {
        /// <summary>
        /// No External Card System.
        /// </summary>
        NONE,
        /// <summary>
        /// Unis External Card System
        /// </summary>
        UNIS,
        /// <summary>
        /// ClubSpeed External Card System
        /// </summary>
        CLUBSPEED,

    }
    public class ExternalCardSystemFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ExternalCardSystemFactory externalCardSystemFactory;
        private static LegacyCardDTO legacyCardDTO;

        bool isUnisSystem = false;
        bool isClubSpeed = false;

        List<LookupValuesDTO> lookupValuesDTOList;

        /// <summary>
        /// GetInstance
        /// </summary>
        /// <param name="legacyCDTO"></param>
        /// <returns></returns>
        public static ExternalCardSystemFactory GetInstance(LegacyCardDTO legacyCDTO)
        {
            log.LogMethodEntry(legacyCDTO);
            legacyCardDTO = legacyCDTO;
            if (externalCardSystemFactory == null)
            {
                externalCardSystemFactory = new ExternalCardSystemFactory();
            }
            log.LogMethodExit(externalCardSystemFactory);
            return externalCardSystemFactory;
        }

        /// <summary>
        /// Get External Card System
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public ExternalCardSystem GetExternalCardSystem(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ExternalCardSystem externalCardSystem = null;
            List<LookupValuesDTO> lookupValuesList = new List<LookupValuesDTO>();
            lookupValuesList = GetLegacyConfiguration(executionContext);
            isUnisSystem = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("IS_UNIS_SYSTEM")).ToList<LookupValuesDTO>()[0].Description.Equals("Y");
            isClubSpeed = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("IS_CLUB_SPEED_ENVIRONMENT")).ToList<LookupValuesDTO>()[0].Description.Equals("Y");
            if (isUnisSystem)
            {
                log.LogMethodExit(externalCardSystem);
                return externalCardSystem = new UnisCardSystem(executionContext, legacyCardDTO);
            }
            else if (isClubSpeed)
            {
                log.LogMethodExit(externalCardSystem);
                return externalCardSystem = new ClubSpeedCardSystem(executionContext, legacyCardDTO);
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Get Leacy Configuration
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        private List<LookupValuesDTO> GetLegacyConfiguration(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LEGACY_CARD_TRANSFER_CONFIGURATIONS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                log.LogMethodExit(lookupValuesDTOList);
                return lookupValuesDTOList;
            }
            catch (Exception e)
            {
                log.Error(e); 
                log.LogMethodExit(null);
                return null;
            }
        }
    }
}

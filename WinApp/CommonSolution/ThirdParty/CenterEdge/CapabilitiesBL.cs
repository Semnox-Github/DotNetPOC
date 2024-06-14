/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CapabilitiesBL class - This class holds the capability configuration values for the CenterEdge
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class holds the capability configuration values for the CenterEdge
    /// </summary>
    public sealed class CapabilitiesBL
    {
        private CapabilityDTO capabilityDTO;
        private static readonly  logging.Logger log = new  logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static CapabilitiesBL instance = null;
        private static readonly object padlock = new object();

        public static CapabilitiesBL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CapabilitiesBL();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Parameterized constructor of CapabilitiesBL class
        /// </summary>
        public CapabilitiesBL()
        {
            log.LogMethodEntry();
            BuildCapabilityResponse();
        }

        private void BuildCapabilityResponse()
        {
            log.LogMethodEntry();
            int maxDecimalPlaces = 2;
            string AMOUNT_FORMAT = string.Empty;
            try
            {
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "AMOUNT_FORMAT"));
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL();
                List< ParafaitDefaultsDTO> ParafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParams);
                if(ParafaitDefaultsDTOList != null && ParafaitDefaultsDTOList.Any())
                {
                    AMOUNT_FORMAT = ParafaitDefaultsDTOList.FirstOrDefault().DefaultValue;
                }
                if (AMOUNT_FORMAT.Contains("#"))
                {
                    int pos = AMOUNT_FORMAT.IndexOf(".");
                    if (pos >= 0)
                    {
                        maxDecimalPlaces = AMOUNT_FORMAT.Length - pos - 1;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                maxDecimalPlaces = 2;
            }
            RegularPoints regularPoints = new RegularPoints(true, maxDecimalPlaces); 
            BonusPoints bonusPoints = new BonusPoints(true, maxDecimalPlaces);
            RedemptionTickets redemptionTickets = new RedemptionTickets(true, 0);
            PointTypes pointTypes = new PointTypes(regularPoints, bonusPoints, redemptionTickets);
            Privileges privileges = new Privileges(true,false);
            BulkIssues bulkIssues = new BulkIssues(true,false);
            CapabilityAdjustments capabilityAdjustments = new CapabilityAdjustments(3);
            Minute minute = new Minute(true, false, false);
            TimePlay timePlay = new TimePlay(3,minute);

            capabilityDTO = new CapabilityDTO(  "Parafait",
                                               1.3,
                                               pointTypes,
                                               privileges,
                                               bulkIssues,
                                               capabilityAdjustments,
                                               timePlay,
                                               true, true, true);
            log.LogMethodExit(capabilityDTO);
        }

        public CapabilityDTO GetCapabilityDTO
        {
            get { return capabilityDTO; }
        }
    }
}

/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the customer game play level results details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Vignesh Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    public class Points
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Get/Set for Value
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Points()
        {
            log.LogMethodEntry();
            Type = string.Empty;
            Value = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary> 
        public Points(string type, decimal? value)
        {
            log.LogMethodEntry(type, value);
            this.Type = type;
            this.Value = value;
            log.LogMethodExit();
        }
    }

    public class ExternalCustomerGamePlayLevelResultsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for CustomerGamePlayLevelResultId
        /// </summary>
        public int CustomerGamePlayLevelResultId { get; set; }

        /// <summary>
        /// Get/Set for GamePlayId
        /// </summary>
        public int GamePlayId { get; set; }

        /// <summary>
        /// Get/Set for GameMachineLevelId
        /// </summary>
        public int GameMachineLevelId { get; set; }

        /// <summary>
        /// Get/Set for CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Get/Set for Points
        /// </summary>
        public Points Points { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalCustomerGamePlayLevelResultsDTO()
        {
            log.LogMethodEntry();
            CustomerGamePlayLevelResultId = -1;
            GamePlayId = -1;
            GameMachineLevelId = -1;
            CustomerId = -1;
            Points = new Points();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary> 
        public ExternalCustomerGamePlayLevelResultsDTO(int customerGamePlayLevelResultId, int gamePlayId, int gameMachineLevelId, int customerId,
               Points points)
        {
            log.LogMethodEntry(customerGamePlayLevelResultId, gamePlayId, gameMachineLevelId, customerId,
                             points);
            this.CustomerGamePlayLevelResultId = customerGamePlayLevelResultId;
            this.GamePlayId = gamePlayId;
            this.GameMachineLevelId = gameMachineLevelId;
            this.CustomerId = customerId;
            this.Points = points;
            log.LogMethodExit();
        }
    }
}

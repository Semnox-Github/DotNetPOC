/*/********************************************************************************************
 * Project Name - POS
 * Description  - Data Object File for LegacyCard SUmmary DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     20-Feb-2022    Dakshakh               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    public class LegacyCardSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set method of the TotalCredits field
        /// </summary>
        public decimal TotalCredits
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the TotalBonus field
        /// </summary>
        public decimal TotalBonus
        {
            get; set;
        }

        /// <summary>
        /// Get/Set method of the TotalTime field
        /// </summary>
        public decimal TotalTime
        {
            get; set;
        }
        /// <summary>
        /// Get/Set method of the TotalTickets field
        /// </summary>
        public decimal TotalTickets
        {
            get; set;
        }
        /// <summary>
        /// Get/Set method of the TotalLoyaltyPoints field
        /// </summary>
        public decimal TotalLoyaltyPoints
        {
            get; set;
        } 
        
        /// <summary>
        /// Get/Set method of the TotalGames field
        /// </summary>
        public decimal TotalGames
        {
            get; set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LegacyCardSummaryDTO()
        {
            log.LogMethodEntry();
            TotalCredits = 0;
            TotalBonus = 0;
            TotalTickets = 0;
            TotalTime = 0;
            TotalLoyaltyPoints = 0;
            TotalGames = 0;
            log.LogMethodExit();
        }

    }
}

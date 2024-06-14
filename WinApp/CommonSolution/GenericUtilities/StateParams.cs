/********************************************************************************************
 * Project Name - State Params Programs 
 * Description  - Data object of the StateParams
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        20-April-2017   Rakshith           Created 
 *2.70.2        10-Aug-2019     Deeksha            Added logger methods.
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class StateParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int stateId;
        private string state;
        private int countryId;
        private int siteId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public StateParams()
        {
            log.LogMethodEntry();
            this.stateId = -1;
            this.state = "";
            this.countryId = -1;
            this.siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the StateId field
        /// </summary>
        [DisplayName("StateId")]
        [DefaultValue(-1)]
        public int StateId { get { return stateId; } set { stateId = value; } }

        /// <summary>
        /// Get/Set method of the State field
        /// </summary>
        [DisplayName("State")]
        public string State { get { return state; } set { state = value; } }

        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        [DefaultValue(-1)]
        public int CountryId { get { return countryId; } set { countryId = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
    }
}

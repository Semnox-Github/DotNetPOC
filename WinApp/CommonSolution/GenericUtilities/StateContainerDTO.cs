/********************************************************************************************
 * Project Name - State DTO
 * Description  - Data object of State
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.0      12-july-2021  Prajwal S          Created
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class StateContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int stateId;
        private string state;
        private string description;
        private int countryId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public StateContainerDTO()
        {
            log.LogMethodEntry();
            stateId = -1;
            state = "";
            description = "";
            countryId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required data fields
        /// </summary>
        public StateContainerDTO(int stateId, string state, string description, int countryId)
            : this()
        {
            log.LogMethodEntry(stateId, state, description, countryId);
            this.stateId = stateId;
            this.state = state;
            this.description = description;
            this.countryId = countryId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the StateId field
        /// </summary>
        [DisplayName("StateId")]
        public int StateId { get { return stateId; } set { stateId = value;} }

        /// <summary>
        /// Get/Set method of the State field
        /// </summary>
        [DisplayName("State")]
        public string State { get { return state; } set { state = value;} }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value;} }

        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        public int CountryId { get { return countryId; } set { countryId = value;} }
    }
}

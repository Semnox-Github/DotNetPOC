/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CountryContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
   public class CountryContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int countryId;
        private string countryName;
        private string countryCode;
        private bool isActive;
        private List<StateContainerDTO> stateContainerDTOList;

        public CountryContainerDTO()
        {
            log.LogMethodEntry();
            stateContainerDTOList = new List<StateContainerDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public CountryContainerDTO(int countryIdPassed, string countryNamePassed, string countryCode, bool isActive) : this()
        {
            log.LogMethodEntry(countryIdPassed, countryNamePassed, countryCode, isActive);
            this.countryId = countryIdPassed;
            this.countryName = countryNamePassed;
            this.countryCode = countryCode;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        public int CountryId { get { return countryId; } set { countryId = value; } }

        /// <summary>
        /// Get/Set method of the CountryName field
        /// </summary>
        [DisplayName("CountryName")]
        public string CountryName { get { return countryName; } set { countryName = value;} }

        /// <summary>
        /// Get/Set method of the CountryCode field
        /// </summary>
        [DisplayName("CountryCode")]
        public string CountryCode { get { return countryCode; } set { countryCode = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the CountryName field
        /// </summary>
        [DisplayName("CountryName")]
        public List<StateContainerDTO> StateContainerDTOList { get { return stateContainerDTOList; } set { stateContainerDTOList = value; } }
    }
}

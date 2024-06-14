/********************************************************************************************
* Project Name - WaiverSet
* Description  - WaiverSetContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Waiver
{
    public class WaiverSetContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int waiverSetId;
        private string name;
        private List<WaiversContainerDTO> waiversContainerDTOList = new List<WaiversContainerDTO>();
        private List<WaiverSetSigningOptionsContainerDTO> waiverSetSigningOptionsContainerDTOList = new List<WaiverSetSigningOptionsContainerDTO>();

        public WaiverSetContainerDTO()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public WaiverSetContainerDTO(int waiverSetIdPassed, string name) : this()
        {
            log.LogMethodEntry(waiverSetIdPassed, name);
            this.waiverSetId = waiverSetIdPassed;
            this.name = name;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Get/Set method of the WaiversContainerDTO field
        /// </summary>
        public List<WaiversContainerDTO> WaiversContainerDTO
        {
            get
            {
                return waiversContainerDTOList;
            }

            set
            {
                waiversContainerDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the WaiverSetSigningOptionsContainerDTO field
        /// </summary>
        public List<WaiverSetSigningOptionsContainerDTO> WaiverSetSigningOptionsContainerDTO
        {
            get
            {
                return waiverSetSigningOptionsContainerDTOList;
            }

            set
            {
                waiverSetSigningOptionsContainerDTOList = value;
            }
        }
    }
}

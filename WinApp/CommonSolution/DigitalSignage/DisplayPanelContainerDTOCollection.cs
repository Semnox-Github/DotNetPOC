/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - DisplayPanelContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *2.150.2    06-Dec-2022    Abhishek         Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Implementation of displayPanel use-cases
    /// </summary>
    public class DisplayPanelContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DisplayPanelContainerDTO> displayPanelContainerDTOList;
        private string hash;

        /// <summary>
        /// DisplayPanelContainerDTOCollection 
        /// </summary>
        public DisplayPanelContainerDTOCollection()
        {
            log.LogMethodEntry();
            displayPanelContainerDTOList = new List<DisplayPanelContainerDTO>();
            log.LogMethodExit();
        }

        public DisplayPanelContainerDTOCollection(List<DisplayPanelContainerDTO> displayPanelContainerDTOList)
        {
            log.LogMethodEntry(displayPanelContainerDTOList);
            this.displayPanelContainerDTOList = displayPanelContainerDTOList;
            if (displayPanelContainerDTOList == null)
            {
                displayPanelContainerDTOList = new List<DisplayPanelContainerDTO>();
            }
            hash = new DtoListHash(displayPanelContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the DisplayPanelContainerDTOList field
        /// </summary>
        public List<DisplayPanelContainerDTO> DisplayPanelContainerDTOList
        {
            get
            {
                return displayPanelContainerDTOList;
            }

            set
            {
                displayPanelContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get method of the Hash field
        /// </summary>
        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}

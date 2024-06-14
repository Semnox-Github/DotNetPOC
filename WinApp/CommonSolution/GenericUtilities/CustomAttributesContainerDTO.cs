/********************************************************************************************
 * Project Name - CustomAttributesContainer DTO
 * Description  - Data object of CustomAttributesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.120.0       12-07-2021   Prajwal          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomAttributes data object class. This acts as data holder for the CustomAttributes business object
    /// </summary>
    public class CustomAttributesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customAttributeId;
        private string name;
        private int sequence;
        private string type;
        private string applicability;
        private string access;

        private List<CustomAttributeValueListContainerDTO> customAttributeValueContainerListDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomAttributesContainerDTO()
        {
            log.LogMethodEntry();
            customAttributeId = -1;
            customAttributeValueContainerListDTO = new List<CustomAttributeValueListContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomAttributesContainerDTO(int customAttributeId, string name, int sequence, string type, string applicability,
                                   string access)
            : this()
        {
            log.LogMethodEntry(customAttributeId, name, sequence, type, applicability, access);
            this.customAttributeId = customAttributeId;
            this.name = name;
            this.sequence = sequence;
            this.type = type;
            this.applicability = applicability;
            this.access = access;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CustomAttributesId field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int CustomAttributeId
        {
            get
            {
                return customAttributeId;
            }

            set
            {
                
                customAttributeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Sequence field
        /// </summary>
        [DisplayName("Sequence")]
        public int Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                
                sequence = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                
                type = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Applicability field
        /// </summary>
        [DisplayName("Applicability")]
        public string Applicability
        {
            get
            {
                return applicability;
            }

            set
            {
                
                applicability = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Access field
        /// </summary>
        [DisplayName("Access")]
        public string Access
        {
            get
            {
                return access;
            }

            set
            {
                
                access = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomAttributeValueListDTOList field
        /// </summary>
        [Browsable(false)]
        public List<CustomAttributeValueListContainerDTO> CustomAttributeValueListContainerDTOList
        {
            get
            {
                return customAttributeValueContainerListDTO;
            }

            set
            {
                customAttributeValueContainerListDTO = value;
            }
        }

    }
}

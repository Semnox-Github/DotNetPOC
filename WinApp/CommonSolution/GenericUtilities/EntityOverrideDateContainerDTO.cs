/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Container DTO of  Obligation Status entity
 **************
 **Version Log
 **************
 *Version     Date             Modified By                   Remarks          
 *********************************************************************************************
 *2.150.0     28-Mar-2022      Lakshminarayana               Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Container Data Object for EntityOverrideDate.
    /// </summary>
    public class EntityOverrideDateContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private string entityName;
        private string entityGuid;
        private string overrideDate;
        private bool includeExcludeFlag;
        private int day;
        private string remarks;

        /// <summary>
        /// Default Constructor with no parameters
        /// </summary>
        public EntityOverrideDateContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public EntityOverrideDateContainerDTO(int id, string entityName, string entityGuid,
                                              string exclusionDate, bool includeExclude,
                                              int day, string remarks)
        {
            log.LogMethodEntry(id, entityName, entityGuid, exclusionDate, includeExclude, day, remarks);
            this.id = id;
            this.entityName = entityName;
            this.entityGuid = entityGuid;
            this.overrideDate = exclusionDate;
            this.includeExcludeFlag = includeExclude;
            this.day = day;
            this.remarks = remarks;
            log.LogMethodExit();
        }


        /// <summary>
        /// Copy constructor
        /// </summary>
        public EntityOverrideDateContainerDTO(EntityOverrideDateContainerDTO entityOverrideDateContainerDTO)
            : this(entityOverrideDateContainerDTO.id, entityOverrideDateContainerDTO.entityName,
                 entityOverrideDateContainerDTO.entityGuid, entityOverrideDateContainerDTO.overrideDate,
                 entityOverrideDateContainerDTO.includeExcludeFlag, entityOverrideDateContainerDTO.day,
                 entityOverrideDateContainerDTO.remarks)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ID field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int ID { get { return id; } set { id = value; } }

        /// <summary>
        /// Get/Set method of the EntityName field
        /// </summary>
        [DisplayName("Entity Name")]
        public string EntityName { get { return entityName; } set { entityName = value; } }

        /// <summary>
        /// Get/Set method of the EntityGuid field
        /// </summary>
        [DisplayName("Entity Guid")]
        public string EntityGuid { get { return entityGuid; } set { entityGuid = value; } }

        /// <summary>
        /// Get/Set method of the OverrideDate field
        /// </summary>        
        [DisplayName("Override Date")]
        public string OverrideDate { get { return overrideDate; } set { overrideDate = value; } }

        /// <summary>
        /// Get/Set method of the IncludeExcludeFlag field
        /// </summary>        
        [DisplayName("Include This Day? ")]
        public bool IncludeExcludeFlag { get { return includeExcludeFlag; } set { includeExcludeFlag = value; } }

        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>        
        [DisplayName("Day")]
        public int Day { get { return day; } set { day = value; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>        
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; } }

    }

}

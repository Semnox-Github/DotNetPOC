/********************************************************************************************
 * Project Name - Site
 * Description  - Business Logic of Organization Structure details
 **************
 **Version Log
 ************** 
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        11-Mar-2019   Jagan Mohan           Created 
 *2.60        01-Apr-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit and modified Parameterized Constructor.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Site
{
    public class OrganizationStructure
    {
        private OrganizationStructureDTO organizationStructureDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="organizationStructureDTO"></param>
        public OrganizationStructure(ExecutionContext executionContext, OrganizationStructureDTO organizationStructureDTO)
        {
            log.LogMethodEntry(organizationStructureDTO, executionContext);
            this.organizationStructureDTO = organizationStructureDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the organizations  Structure
        /// Records will be inserted if Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            OrganizationStructureDataHandler organizationStructureDataHandler = new OrganizationStructureDataHandler(sqlTransaction);

            if (organizationStructureDTO.StructureId < 0)
            {
                int structureId = organizationStructureDataHandler.InsertOrganizationStructure(organizationStructureDTO, executionContext.GetUserId());
                organizationStructureDTO.StructureId = structureId;
            }
            else
            {
                if (organizationStructureDTO.IsChanged)
                {
                    organizationStructureDataHandler.UpdateOrganizationStructure(organizationStructureDTO, executionContext.GetUserId());
                    organizationStructureDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }
    /// <summary>
    /// Manages the list of organization structure
    /// </summary>
    public class OrganizationStructureList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the organization list
        /// </summary>
        public List<OrganizationStructureDTO> GetAllOrganizationStructure(List<KeyValuePair<OrganizationStructureDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            OrganizationStructureDataHandler organizationStructureDataHandler = new OrganizationStructureDataHandler();
            log.LogMethodExit();
            return organizationStructureDataHandler.GetOrganizationStructureList(searchParameters);
        }
    }
}

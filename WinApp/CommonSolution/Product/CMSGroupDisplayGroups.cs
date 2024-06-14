
/********************************************************************************************
 * Project Name - CMSGroupDisplayGroups
 * Description  - Bussiness logic of the CMSGroupDisplayGroups class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       26-Sept-2016   Rakshith          Created 
 *2.3.0      25-Jun-2018    Guru S A          Modifications handle products exclusion at user role
 *                                            level 
 *2.70.3     31-Mar-2020    Jeevan            Removed syncstatus from update query      
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  CMSDisplayGroup 
    /// </summary>

    public class CMSGroupDisplayGroups
    {
        CMSGroupDisplayGroupDTO cmsDisplayGroupDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of CMSGroupsDisplayGroup class
        /// </summary>
        public CMSGroupDisplayGroups()
        {
            log.Debug("Starts-CMSGroupDisplayGroups() default constructor.");
            cmsDisplayGroupDTO = null;
            log.Debug("Ends-CMSGroupDisplayGroups() default constructor.");
        }

        /// <summary>
        /// Constructor with the cmsDisplayGroupId id as the parameter
        /// Would fetch the CmsDisplayGroupDTO object from the database based on the cmsDisplayGroupId passed. 
        /// </summary>
        /// <param name="cmsDisplayGroupId">Display Group</param>
        public CMSGroupDisplayGroups(int cmsGroupDisplayGroupId)
            : this()
        {
            log.Debug("Starts-CMSGroupsDisplayGroup(cmsDisplayGroupId) parameterized constructor.");

            CMSGroupDisplayGroupDataHandler cmsDisplayGroupDataHandler = new CMSGroupDisplayGroupDataHandler();

            cmsDisplayGroupDTO = cmsDisplayGroupDataHandler.GetDisplayGroups(cmsGroupDisplayGroupId);
            log.Debug("Ends-CMSGroupsDisplayGroup(cmsDisplayGroupId) parameterized constructor.");
        }

        /// <summary>
        /// Creates cmsDisplayGroupDTO object using the cmsDisplayGroupDTO
        /// </summary>
        /// <param name="cmsDisplayGroupDTO">cmsDisplayGroupDTO object</param>
        public CMSGroupDisplayGroups(CMSGroupDisplayGroupDTO cmsDisplayGroupDTO)
            : this()
        {
            log.Debug("Starts-CMSGroupDisplayGroups(cmsDisplayGroupDTO) Parameterized constructor.");
            this.cmsDisplayGroupDTO = cmsDisplayGroupDTO;
            log.Debug("Ends-CMSGroupDisplayGroups(CMSGroupsDisplayGroup) Parameterized constructor.");
        }

        /// <summary>
        /// Saves the cmsDisplayGroupDTO 
        /// Checks if the CMSGroupDisplayGroupId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");

            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

            CMSGroupDisplayGroupDataHandler cmsDisplayGroupDataHandler = new CMSGroupDisplayGroupDataHandler();


            if (cmsDisplayGroupDTO.GroupDisplayGroupId < 0)
            {
                int roleDisplayGroupId = cmsDisplayGroupDataHandler.InsertDisplayGroups(cmsDisplayGroupDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                cmsDisplayGroupDTO.GroupDisplayGroupId = roleDisplayGroupId;
            }
            else
            {
                if (cmsDisplayGroupDTO.IsChanged == true)
                {
                    cmsDisplayGroupDataHandler.UpdateDisplayGroups(cmsDisplayGroupDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsDisplayGroupDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CMSGroupDisplayGroupDTO GetCmsDisplayGroupDTO
        {
            get { return cmsDisplayGroupDTO; }
        }

        /// <summary>
        /// Delete the CmsDisplayGroupDTO based on Id
        /// </summary>
        public int Delete(int cmsGroupDisplayGroupId)
        {
            log.Debug("Starts-Delete(int displayGroupId) Method.");
            try
            {
                CMSGroupDisplayGroupDataHandler cmsDisplayGroupDataHandler = new CMSGroupDisplayGroupDataHandler();

                log.Debug("Ends-Delete(int cmsGroupDisplayGroupId) Method.");
                return cmsDisplayGroupDataHandler.Delete(cmsGroupDisplayGroupId);
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }

        }
        /// <summary>
        /// Delete the CmsDisplayGroupDTO based on groupId
        /// </summary>
        public int DeleteByGroupGroupId(int groupId)
        {
            log.Debug("Starts-DeleteByGroupId(int groupId) Method.");
            try
            {
                CMSGroupDisplayGroupDataHandler cmsDisplayGroupDataHandler = new CMSGroupDisplayGroupDataHandler();

                log.Debug("Ends-DeleteByGroupId(int groupId) Method.");
                return cmsDisplayGroupDataHandler.DeleteByGroupId(groupId);
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }

        }

    }

    /// <summary>
    /// Manages the list of CMSDisplayGroup
    /// </summary>
    public class CMSDisplayGroupList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the Product Display GroupList
        /// </summary>
        public List<CMSGroupDisplayGroupDTO> GetAllCmsDisplayGroupList(List<KeyValuePair<CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllCmsDisplayGroupList(searchParameters) method");
            CMSGroupDisplayGroupDataHandler cmsDisplayGroupDataHandler = new CMSGroupDisplayGroupDataHandler();
            log.Debug("Ends-GetAllCmsDisplayGroupList(searchParameters) method by returning the result of cmsDisplayGroupDataHandler.GetDisplayGroupsList(searchParameters) call");
            return cmsDisplayGroupDataHandler.GetDisplayGroupsList(searchParameters);
        }

    }
}

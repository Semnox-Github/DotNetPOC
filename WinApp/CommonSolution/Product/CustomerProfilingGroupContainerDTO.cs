/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingGroupContainerDTO
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      24-Mar-2022     Girish Kundar              Created : Check in check out changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public  class CustomerProfilingGroupContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerProfilingGroupId;
        private string groupName;
        private List<CustomerProfilingContainerDTO> customerProfilingContainerDTOList;
        //private int? ageUpperLimit;
        //private int? ageLowerLimit;
        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerProfilingGroupContainerDTO()
        {
            log.LogMethodEntry();
            customerProfilingGroupId = -1;
            //ageUpperLimit = null;
            //ageLowerLimit = null;
            groupName = string.Empty;
            customerProfilingContainerDTOList = new List<CustomerProfilingContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerProfilingGroupContainerDTO(int customerProfilingGroupId, string groupName,/*int? ageUpperLimit,int? ageLowerLimit,*/
                        List<CustomerProfilingContainerDTO> customerProfilingContainerDTOList)
        {
            log.LogMethodEntry(customerProfilingGroupId, groupName, customerProfilingContainerDTOList);
            this.customerProfilingGroupId = customerProfilingGroupId;
            this.groupName = groupName;
            //this.ageUpperLimit = ageUpperLimit;
            //this.ageLowerLimit = ageLowerLimit;
            this.customerProfilingContainerDTOList = customerProfilingContainerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the customerProfilingGroupId field
        /// </summary>
        public int CustomerProfilingGroupId
        {
            get { return customerProfilingGroupId; }
            set { customerProfilingGroupId = value; }
        }

        /// <summary>
        /// Get/Set method of the groupName field
        /// </summary>
        public string ProfilingGroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        ///// <summary>
        ///// Get/Set method of the AgeUpperLimit field
        ///// </summary>
        //public int? AgeUpperLimit
        //{
        //    get { return ageUpperLimit; }
        //    set { ageUpperLimit = value; }
        //}
        ///// <summary>
        ///// Get/Set method of the ageLowerLimit field
        ///// </summary>
        //public int? AgeLowerLimit
        //{
        //    get { return ageLowerLimit; }
        //    set { ageLowerLimit = value; }
        //}

        /// <summary>
        /// Get/Set method of the CustomerProfilingContainerDTOList field
        /// </summary>
        public List<CustomerProfilingContainerDTO> CustomerProfilingContainerDTOList
        {
            get { return customerProfilingContainerDTOList; }
            set { customerProfilingContainerDTOList = value; }
        }
    }
}

/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the External Product Group  details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Ashish Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{

    public class ExternalProductGroupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for GroupName
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Get/Set for GroupId
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Get/Set for Products
        /// </summary>
        public List<ExternalProductDTO> Products { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExternalProductGroupDTO()
        {
            log.LogMethodEntry();
            GroupName = string.Empty;
            GroupId = -1;
            Products = new List<ExternalProductDTO>();
            log.LogMethodExit();

        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ExternalProductGroupDTO(string groupName,int groupId,List<ExternalProductDTO> products)
        {
            log.LogMethodEntry(groupName,groupId,products);

            this.GroupName = groupName;
            this.GroupId = groupId;
            this.Products = products;
            log.LogMethodExit();

        }
    }

}

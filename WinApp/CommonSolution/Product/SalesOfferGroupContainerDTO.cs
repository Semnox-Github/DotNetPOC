/********************************************************************************************
 * Project Name - Sales Offer Group Container DTO
 * Description  - Data object of sales offer group DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.00    29-Dec-2021   Prajwal S     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the sales offer group data object class. This acts as data holder for the sales offer group business object
    /// </summary>
    public class SalesOfferGroupContainerDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int saleGroupId;
        string name;
        bool isUpsell;
        string guid;
        List<SaleGroupProductMapContainerDTO> saleGroupProductMapDTOContainerList;


        /// <summary>
        /// Default Contructor
        /// </summary>
        public SalesOfferGroupContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public SalesOfferGroupContainerDTO(int saleGroupId, string name, bool isUpsell, string guid)
        {
            log.LogMethodEntry(saleGroupId, name, isUpsell);
            this.saleGroupId = saleGroupId;
            this.name = name;
            this.isUpsell = isUpsell;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the SaleGroupId field
        /// </summary>
        public int SaleGroupId { get { return saleGroupId; } set { saleGroupId = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value;  } }


        /// <summary>
        /// Get/Set method of the IsUpsell field
        /// </summary>
        public bool IsUpsell { get { return isUpsell; } set { isUpsell = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value;  } }

        /// <summary>
        /// Get/Set method of the SaleGroupProductMapDTOList field
        /// </summary>
        public List<SaleGroupProductMapContainerDTO> SaleGroupProductMapDTOContainerList { get { return saleGroupProductMapDTOContainerList; } set { saleGroupProductMapDTOContainerList = value; } }
    }
}

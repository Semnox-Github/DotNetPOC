/********************************************************************************************
 * Project Name - Product
 * Description  - price container data transfer object
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      17-Aug-2021      Lakshminarayana           Created : price container enhancement
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// price container data transfer object
    /// </summary>
    public class PriceContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private int membershipId;
        private int transactionProfileId;
        private int userRoleId;

        private List<PriceContainerDetailDTO> priceContainerDetailDTOList = new List<PriceContainerDetailDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PriceContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with fields
        /// </summary>
        public PriceContainerDTO(int productId)
        : this(productId, -1, -1,-1)
        {
            log.LogMethodEntry(productId);

            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the fields
        /// </summary>
        public PriceContainerDTO(int productId,
                                 int membershipId,
                                 int transactionProfileId,
                                 int userRoleId)
        {
            log.LogMethodEntry(productId, membershipId, transactionProfileId,userRoleId);
            this.productId = productId;
            this.membershipId = membershipId;
            this.transactionProfileId = transactionProfileId;
            this.userRoleId = userRoleId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Copy Constructor
        /// </summary>
        public PriceContainerDTO(PriceContainerDTO priceContainerDTO)
        :this(priceContainerDTO.productId, priceContainerDTO.membershipId, priceContainerDTO.transactionProfileId,
                priceContainerDTO.userRoleId)
        {
            log.LogMethodEntry(priceContainerDTO);
            if(priceContainerDTO.priceContainerDetailDTOList != null)
            {
                foreach (var priceContainerDetailDTO in priceContainerDTO.priceContainerDetailDTOList)
                {
                    priceContainerDetailDTOList.Add(new PriceContainerDetailDTO(priceContainerDetailDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        public int MembershipId
        {
            get { return membershipId; }
            set { membershipId = value; }
        }

        /// <summary>
        /// Get/Set method of the transactionProfileId field
        /// </summary>
        public int TransactionProfileId
        {
            get { return transactionProfileId; }
            set { transactionProfileId = value; }
        }

        /// <summary>
        /// Get/Set method of the userRoleId field
        /// </summary>
        public int UserRoleId
        {
            get { return userRoleId; }
            set { userRoleId = value; }
        }

        /// <summary>
        /// Get/Set method of the priceContainerDetailDTOList field
        /// </summary>
        public List<PriceContainerDetailDTO> PriceContainerDetailDTOList
        {
            get { return priceContainerDetailDTOList; }
            set { priceContainerDetailDTOList = value; }
        }
    }
}

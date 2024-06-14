/********************************************************************************************
 * Project Name - Product
 * Description  - Data structure of ProductTypeContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1     24-Jun-2021    Abhishek       Created: POS Redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data structure of ProductTypeContainer
    /// </summary>
    public class ProductTypeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productTypeId;
        private string productType;
        private string description;
        private bool cardSale;
        private string reportGroup;
        private int orderTypeId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductTypeContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ProductTypeContainerDTO(int productTypeId, string productType, string description, bool cardSale, string reportGroup, int orderTypeId)
            : this()
        {
            log.LogMethodEntry(productTypeId, productType, description, cardSale, reportGroup, orderTypeId);
            this.productTypeId = productTypeId;
            this.productType = productType;
            this.description = description;
            this.cardSale = cardSale;
            this.reportGroup = reportGroup;
            this.orderTypeId = orderTypeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the productTypeId field
        /// </summary>
        public int ProductTypeId
        {
            get { return productTypeId; }
            set { productTypeId = value; }
        }

        /// <summary>
        /// Get/Set method of the productType field
        /// </summary>
        public string ProductType
        {
            get { return productType; }
            set { productType = value; }
        }

        /// <summary>
        /// Get/Set method of the description field
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Get/Set method of the cardSale field
        /// </summary>
        public bool CardSale
        {
            get { return cardSale; }
            set { cardSale = value; }
        }

        /// <summary>
        /// Get/Set method of the optionType field
        /// </summary>
        public string ReportGroup
        {
            get { return reportGroup; }
            set { reportGroup = value; }
        }

        /// <summary>
        /// Get/Set method of the orderTypeId field
        /// </summary>
        public int OrderTypeId
        {
            get { return orderTypeId; }
            set { orderTypeId = value; }
        }





    }
}

/********************************************************************************************
 * Project Name - Products
 * Description  - ModifierSetDetailContainer class to hold Modifier details data.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S          Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ModifierSetDetailsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int modifierSetId;
        private int modifierProductId;
        private double price;
        private int sortOrder;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ModifierSetDetailsContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ModifierSetDetailsContainerDTO(int id, int modifierSetId, double price, int sortOrder, int modifierProductId)
            : this()
        {
            log.LogMethodEntry(id, modifierSetId, modifierProductId, price, sortOrder);
            this.id = id;
            this.ModifierSetId = modifierSetId;
            this.modifierSetId = modifierSetId;
            this.modifierProductId = modifierProductId;
            this.price = price;
            this.sortOrder = sortOrder;
            log.LogMethodExit();
        }

        /// <summary>
        /// copy constructor with Required parameters
        /// </summary>
        public ModifierSetDetailsContainerDTO(ModifierSetDetailsContainerDTO modifierSetDetailsContainerDTO)
            : this(modifierSetDetailsContainerDTO.id, modifierSetDetailsContainerDTO.modifierSetId, modifierSetDetailsContainerDTO.price, modifierSetDetailsContainerDTO.sortOrder, modifierSetDetailsContainerDTO.modifierProductId)
        {
            log.LogMethodEntry(id, modifierSetId, modifierProductId, price, sortOrder);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int ModifierSetDetailId
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the modifierSetId field
        /// </summary>
        public int ModifierSetId
        {
            get
            {
                return modifierSetId;
            }

            set
            {
                modifierSetId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the modifierProductId field
        /// </summary>
        public int ModifierProductId
        {
            get
            {
                return modifierProductId;
            }

            set
            {
                modifierProductId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        public int SortOrder
        {
            get
            {
                return sortOrder;
            }

            set
            {
                sortOrder = value;
            }
        }


    }
}

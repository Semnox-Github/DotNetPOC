/********************************************************************************************
 * Project Name - Products
 * Description  - ModifierSetContainerDTO class to hold Modifier Set data   
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S              Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ModifierSetContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int modifierSetId;
        private string setName;
        private int minQuantity;
        private int maxQuantity;
        private int freeQuantity;
        private ModifierSetContainerDTO parentModifierSetDTO;
        private List<ModifierSetDetailsContainerDTO> modifierSetDetailsContainerDTOList;
        private ModifierSetContainerDTO modifierSetContainerDTO;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ModifierSetContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ModifierSetContainerDTO(int modifierSetId, string setName, int minQuantity, int maxQuantity, int freeQuantity)
            : this()
        {
            log.LogMethodEntry(modifierSetId, setName, minQuantity, maxQuantity, freeQuantity);
            this.ModifierSetId = modifierSetId;
            this.setName = setName;
            this.minQuantity = minQuantity;
            this.maxQuantity = maxQuantity;
            this.freeQuantity = freeQuantity;
            this.modifierSetDetailsContainerDTOList = new List<ModifierSetDetailsContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        public ModifierSetContainerDTO(ModifierSetContainerDTO modifierSetContainerDTO)
            : this(modifierSetContainerDTO.modifierSetId, modifierSetContainerDTO.setName, modifierSetContainerDTO.minQuantity, modifierSetContainerDTO.maxQuantity, modifierSetContainerDTO.freeQuantity)
        {
            log.LogMethodEntry(modifierSetContainerDTO);
            if(modifierSetContainerDTO.modifierSetDetailsContainerDTOList != null)
            {
                modifierSetDetailsContainerDTOList = new List<ModifierSetDetailsContainerDTO>();
                foreach (var modifierSetDetailsContainerDTO in modifierSetContainerDTO.modifierSetDetailsContainerDTOList)
                {
                    ModifierSetDetailsContainerDTO modifierSetDetailsContainerDTOCopy = new ModifierSetDetailsContainerDTO(modifierSetDetailsContainerDTO);
                    modifierSetDetailsContainerDTOList.Add(modifierSetDetailsContainerDTOCopy);
                }
            }
            
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ModifierSetId field
        /// </summary>       
        public int ModifierSetId { get { return modifierSetId; } set { modifierSetId = value; } }

        /// <summary>
        /// Get/Set method of the setName field
        /// </summary>       
        public string SetName { get { return setName; } set { setName = value; } }

        /// <summary>
        /// Get/Set method of the minQuantity field
        /// </summary>        
        public int MinQuantity { get { return minQuantity; } set { minQuantity = value; } }

        /// <summary>
        /// Get/Set method of the ParentModifierSetDTO field
        /// </summary>    
        public ModifierSetContainerDTO ParentModifierSetDTO { get { return parentModifierSetDTO; } set { parentModifierSetDTO = value; } }

        /// <summary>
        /// Get/Set method of the maxQuantity field
        /// </summary>       
        public int MaxQuantity { get { return maxQuantity; } set { maxQuantity = value; } }

        /// <summary>
        /// Get/Set method of the freeQuantity field
        /// </summary>       
        public int FreeQuantity { get { return freeQuantity; } set { freeQuantity = value; } }

        /// <summary>
        /// Get/Set method of the ModifierSetDetailsContainerDTOList field
        /// </summary>       
        public List<ModifierSetDetailsContainerDTO> ModifierSetDetailsContainerDTOList { get { return modifierSetDetailsContainerDTOList; } set { modifierSetDetailsContainerDTOList = value; } }

    }

}
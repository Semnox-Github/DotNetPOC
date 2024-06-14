/********************************************************************************************
 * Project Name - Product
 * Description  - Data structure of ProductDisplayGroupFormatContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     25-Jun-2021   Abhishek            Created: POS Redesign
 *2.130.11    13-Oct-2022   Vignesh Bhat        Modified to support BackgroundImageFileName property
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data structure of ProductDisplayGroupFormatContainer
    /// </summary>
    public class ProductDisplayGroupFormatContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private string displayGroup;
        private int sortOrder;
        private string imageUrl;
        private List<int> productIdList;
        private string buttonColor;
        private string textColor;
        private string font;
        private string description;
        private string externalSourceReference;
        private string backgroundImageFileName;

        private List<ProductsDisplayGroupContainerDTO> productsDisplayGroupContainerDTOList = new List<ProductsDisplayGroupContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductDisplayGroupFormatContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ProductDisplayGroupFormatContainerDTO(int id, string displayGroup, int sortOrder, string imageUrl, string buttonColor, string textColor, string font, string description, string externalSourceReference, string backgroundImageFileName)
            : this()
        {
            log.LogMethodEntry(id, displayGroup, sortOrder, imageUrl);
            this.id = id;
            this.displayGroup = displayGroup;
            this.sortOrder = sortOrder;
            this.imageUrl = imageUrl;
            this.buttonColor = buttonColor;
            this.textColor = textColor;
            this.font = font;
            this.description = description;
            this.externalSourceReference = externalSourceReference;
            this.backgroundImageFileName = backgroundImageFileName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Get/Set method of the productType field
        /// </summary>
        public string DisplayGroup
        {
            get { return displayGroup; }
            set { displayGroup = value; }
        }

        /// <summary>
        /// Get/Set method of the ButtonColor field
        /// </summary>
        public string ButtonColor
        {
            get { return buttonColor; }
            set { buttonColor = value; }
        }

        /// <summary>
        /// Get/Set method of the productType field
        /// </summary>
        public string TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        public string Font
        {
            get { return font; }
            set { font = value; }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Get/Set method of the ExternalSourceReference field
        /// </summary>
        public string ExternalSourceReference
        {
            get { return externalSourceReference; }
            set { externalSourceReference = value; }
        }

        /// <summary>
        /// Get/Set method of the sortOrder field
        /// </summary>
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        /// <summary>
        /// Get/Set method of the imageUrl field
        /// </summary>
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        /// <summary>
        /// Get/Set method of the BackgroundImageFileName field
        /// </summary>
        public string BackgroundImageFileName
        {
            get { return backgroundImageFileName; }
            set { backgroundImageFileName = value; }
        }
        /// <summary>
        /// Get/Set method of the productIdList field
        /// </summary> 
        public List<int> ProductIdList
        {
            get { return productIdList; }
            set { productIdList = value; }
        }

        /// <summary>
        /// Get/Set method of the productsDisplayGroupContainerDTOList field
        /// </summary> 
        public List<ProductsDisplayGroupContainerDTO> ProductsDisplayGroupContainerDTOList
        {
            get { return productsDisplayGroupContainerDTOList; }
            set { productsDisplayGroupContainerDTOList = value; }
        }

    }
}

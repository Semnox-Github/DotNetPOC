/********************************************************************************************
 * Project Name - Segment categorization value
 * Description  - Bussiness logic of segment categorization value
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Raghuveera          Created 
 *2.70        07-Mar-2018   Muhammed Mehraj     Modified
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment categorization value will creates and modifies the segment categorization value
    /// </summary>
    public class SegmentCategorizationValue
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SegmentCategorizationValueDTO segmentCategorizationValueDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public SegmentCategorizationValue(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.segmentCategorizationValueDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="segmentCategorizationValueDTO">Parameter of the type SegmentCategorizationValueDTO</param>
        public SegmentCategorizationValue(ExecutionContext executionContext, SegmentCategorizationValueDTO segmentCategorizationValueDTO)
        {
            log.LogMethodEntry(executionContext, segmentCategorizationValueDTO);
            this.executionContext = executionContext;
            this.segmentCategorizationValueDTO = segmentCategorizationValueDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the segment categorization value
        /// segment categorization value will be inserted if SegmentCategoryValueId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SegmentCategorizationValueDataHandler segmentCategorizationValueDataHandler = new SegmentCategorizationValueDataHandler(sqlTransaction);
            if (segmentCategorizationValueDTO.SegmentCategoryValueId < 0)
            {
                int SegmentCategoryValueId = segmentCategorizationValueDataHandler.InsertSegmentCategorizationValue(segmentCategorizationValueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                segmentCategorizationValueDTO.SegmentCategoryValueId = SegmentCategoryValueId;
                segmentCategorizationValueDTO.AcceptChanges();
            }
            else
            {
                if (segmentCategorizationValueDTO.IsChanged == true)
                {
                    segmentCategorizationValueDataHandler.UpdateSegmentCategorizationValue(segmentCategorizationValueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    segmentCategorizationValueDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the segment categorization value       
        /// </summary>
        public void SaveSegmentCategorizationValues(List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOList, int productId, string applicability, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(segmentCategorizationValueDTOList, productId, applicability);
            //int count = 0;
            int segmentCategoryId = -1;
            SegmentCategorizationValueDataHandler segmentCategorizationValueDataHandler = new SegmentCategorizationValueDataHandler(sqlTransaction);

            //If the current combination is already exists then getting the segment category id
            foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
            {                    //if the combination is not exists and the new combination then inserting one record to categorization table to get new combination id
                if (segmentCategoryId == -1 && segmentCategorizationValueDTO.SegmentCategoryId == -1)
                {
                    SegmentCategorizationDTO segmentCategorizationDTO = new SegmentCategorizationDTO();
                    SegmentCategorization segmentCategorization = new SegmentCategorization(executionContext, segmentCategorizationDTO);
                    segmentCategorization.Save(sqlTransaction);
                    segmentCategorizationValueDTO.SegmentCategoryId = segmentCategoryId = segmentCategorizationDTO.SegmentCategoryId;
                }
                else if (segmentCategorizationValueDTO.SegmentCategoryId == -1 && segmentCategoryId != -1)
                {
                    segmentCategorizationValueDTO.SegmentCategoryId = segmentCategoryId;
                }
                if (segmentCategorizationValueDTO.SegmentCategoryId != -1)
                {
                    segmentCategoryId = segmentCategorizationValueDTO.SegmentCategoryId;
                    this.segmentCategorizationValueDTO = segmentCategorizationValueDTO;
                    Save(sqlTransaction);//saving the categorization value record.
                }
                else
                {
                    throw (new Exception("Failed to get SegmentCategoryId."));
                }

            }
            switch (applicability.ToUpper().ToString())
            {
                case "PRODUCT"://Updating the Product table with segment category id
                    ProductDTO productDTO = new ProductDTO();
                    ProductList productList = new ProductList();
                    productDTO = productList.GetProduct(productId, sqlTransaction);
                    if (productDTO != null)
                    {
                        productDTO.SegmentCategoryId = segmentCategoryId;
                        ProductBL product = new ProductBL(productDTO);
                        product.Save(sqlTransaction);
                    }
                    break;
                case "POS PRODUCTS":
                    Products products = new Products(); //commented by suneetha on Apr-03-17 for resolving issue calling ProductsBL in SegmentCategorization
                    products.UpdateProductsSegmentCategoryId(productId, segmentCategoryId);
                    break;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of segment categorization value
    /// </summary>
    public class SegmentCategorizationValueList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOList;
        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        public SegmentCategorizationValueList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.segmentCategorizationValueDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the segment categorization value list
        /// </summary>
        public List<SegmentCategorizationValueDTO> GetAllSegmentCategorizationValues(List<KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            SegmentCategorizationValueDataHandler segmentCategorizationValueDataHandler = new SegmentCategorizationValueDataHandler();
            log.LogMethodExit();
            return segmentCategorizationValueDataHandler.GetSegmentCategorizationValueList(searchParameters);
        }

    }
}

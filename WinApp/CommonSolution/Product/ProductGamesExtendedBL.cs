/********************************************************************************************
 * Project Name - Product Games Extended BL
 * Description  - Bussiness logic of Product Games Extended
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        01-Feb-2019   Akshay Gulaganji    Created 
 *2.70        29-June-2019  Indrajeet Kumar     Created DeleteProductGameExtended() method for Hard Deletion and Added SqlTrasaction.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for ProductGamesExtendedBL class.
    /// </summary>
    public class ProductGamesExtendedBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private ProductGamesExtendedDTO productGamesExtendedDTO;

        /// <summary>
        /// Default constructor of ProductGamesExtendedBL class
        /// </summary>
        public ProductGamesExtendedBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productGamesExtendedDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the ProductGamesExtended id as the parameter Would fetch the ProductGamesExtendedDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public ProductGamesExtendedBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductGamesExtendedDataHandler productGamesExtendedDataHandler = new ProductGamesExtendedDataHandler(sqlTransaction);
            this.productGamesExtendedDTO = productGamesExtendedDataHandler.GetProductGamesExtendedDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductGamesExtendedDTO as the Parameter 
        /// </summary>
        /// <param name="productGamesExtendedDTO">productGamesExtendedDTO</param>
        /// <param name="executionContext">executionContext</param>
        public ProductGamesExtendedBL(ExecutionContext executionContext, ProductGamesExtendedDTO productGamesExtendedDTO)
        {
            log.LogMethodEntry(executionContext, productGamesExtendedDTO);
            this.productGamesExtendedDTO = productGamesExtendedDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductGamesExtended
        /// Checks if the  id is not less than 0
        /// If it is less than or equal to 0, then inserts
        /// else updates the record
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductGamesExtendedDataHandler productGamesExtendedDataHandler = new ProductGamesExtendedDataHandler(sqlTransaction);
            if (productGamesExtendedDTO.Id < 0)
            {
                int idInserted = productGamesExtendedDataHandler.InsertProductGamesExtended(productGamesExtendedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productGamesExtendedDTO.Id = idInserted;
                productGamesExtendedDTO.AcceptChanges();
                log.Debug(string.Format("Id {0} has been stored from Save() of ProductGamesExtendedBL ", idInserted));
            }
            else
            {
                if (productGamesExtendedDTO.Id >= 0 && productGamesExtendedDTO.IsChanged)
                {
                    int rowsUpdated = productGamesExtendedDataHandler.UpdateProductGamesExtended(productGamesExtendedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productGamesExtendedDTO.AcceptChanges();
                    log.Debug(string.Format("{0} rows has been updated from Save() of ProductGamesExtendedBL ", rowsUpdated));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the ProductGameExtended record - Hard Deletion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public void DeleteProductGameExtended(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            try
            {
                ProductGamesExtendedDataHandler productGamesExtendedDataHandler = new ProductGamesExtendedDataHandler(sqlTransaction);
                productGamesExtendedDataHandler.DeleteProductGameExtended(id);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ProductGamesExtendedDTO
        /// </summary>
        public ProductGamesExtendedDTO ProductGamesExtendedDTO
        {
            get
            {
                return productGamesExtendedDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of ProductGamesExtendedListBL
    /// </summary>
    public class ProductGamesExtendedListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ProductGamesExtendedDTO> productGamesExtendedDTOList;
        /// <summary>
        /// Default constructor of ProductGamesExtendedListBL class
        /// </summary>
        public ProductGamesExtendedListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productGamesExtendedDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor of ProductGamesExtendedListBL class
        /// </summary>
        /// <param name="productGamesExtendedDTOList"></param>
        /// <param name="executionContext"></param>
        public ProductGamesExtendedListBL(ExecutionContext executionContext, List<ProductGamesExtendedDTO> productGamesExtendedDTOList)
        {
            log.LogMethodEntry(executionContext, productGamesExtendedDTOList);
            this.executionContext = executionContext;
            this.productGamesExtendedDTOList = productGamesExtendedDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the ProductGamesExtendedDTO list
        /// </summary>
        public List<ProductGamesExtendedDTO> GetProductGamesExtendedList(List<KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductGamesExtendedDataHandler productGamesExtendedDataHandler = new ProductGamesExtendedDataHandler(sqlTransaction);
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList = productGamesExtendedDataHandler.GetProductGamesExtendedDTOList(searchParameters);
            log.LogMethodExit(productGamesExtendedDTOList);
            return productGamesExtendedDTOList;
        }
        /// <summary>
        /// Gets the ProductGamesExtendedDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="productGameIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductGamesExtendedDTO</returns>
        public List<ProductGamesExtendedDTO> GetProductGamesExtendedDTOList(List<int> productGameIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productGameIdList, activeRecords, sqlTransaction);
            ProductGamesExtendedDataHandler productGamesExtendedDataHandler = new ProductGamesExtendedDataHandler(sqlTransaction);
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList = productGamesExtendedDataHandler.GetProductGamesExtendedDTOList(productGameIdList, activeRecords);
            log.LogMethodExit(productGamesExtendedDTOList);
            return productGamesExtendedDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - Sale Group Product Map
 * Description  - The bussiness logic for sale group product map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-May-2017   Raghuveera     Created
 *2.50        21-Jan-2019   Jagan Mohana   Created the constructor SaleGroupProductMapList and
 *                                         added new methods SaveUpdateSetupOfferGroupProductMap
 *2.70        17-Mar-2019   Manoj Durgam       Added ExecutionContext to the constructor
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
    ///Bussiness logic class for Sale Group Product Map operations
    /// </summary>
    public class SaleGroupProductMap
    {
        SaleGroupProductMapDTO saleGroupProductMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of SaleGroupProductMap class
        /// </summary>
        public SaleGroupProductMap(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            saleGroupProductMapDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the Sale Group Product Map DTO based on the saleGroupProductMap Id passed 
        /// </summary>
        /// <param name="saleGroupProductMapId">saleGroupProductMapId</param>
        public SaleGroupProductMap(ExecutionContext executionContext, int saleGroupProductMapId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, saleGroupProductMapId);
            SaleGroupProductMapDataHandler saleGroupProductMapDataHandler = new SaleGroupProductMapDataHandler();
            saleGroupProductMapDTO = saleGroupProductMapDataHandler.GetSaleGroupProductMapDTO(saleGroupProductMapId);
            log.LogMethodExit(saleGroupProductMapDTO);
        }

        /// <summary>
        /// Creates sales offer group object using the SaleGroupProductMapDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="saleGroupProductMap">SaleGroupProductMapDTO object</param>
        public SaleGroupProductMap(ExecutionContext executionContext, SaleGroupProductMapDTO saleGroupProductMap)
            : this(executionContext)
        {
            log.LogMethodEntry(saleGroupProductMap, executionContext);
            this.saleGroupProductMapDTO = saleGroupProductMap;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// save and updates the record 
        /// </summary>
        /// <param name="sqltrxn">Holds the sql transaction object</param>
        public void Save(SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(sqltrxn);
            SaleGroupProductMapDataHandler saleGroupProductMapDataHandler = new SaleGroupProductMapDataHandler();
            if (saleGroupProductMapDTO.TypeMapId < 0)
            {
                int typeMapId = saleGroupProductMapDataHandler.InsertSaleGroupProductMap(saleGroupProductMapDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqltrxn);
                saleGroupProductMapDTO.TypeMapId = typeMapId;
                saleGroupProductMapDTO.AcceptChanges();
            }
            else
            {
                if (saleGroupProductMapDTO.IsChanged == true)
                {
                    saleGroupProductMapDataHandler.UpdateSaleGroupProductMap(saleGroupProductMapDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqltrxn);
                    saleGroupProductMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SaleGroupProductMapDTO GetSaleGroupProductMapDTO { get { return saleGroupProductMapDTO; } }
    }

    /// <summary>
    /// Manages the list of sales offer groups
    /// </summary>
    public class SaleGroupProductMapList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SaleGroupProductMapDTO> saleGroupProductMapList;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SaleGroupProductMapList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.saleGroupProductMapList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="saleGroupProductMapList"></param>
        /// <param name="executionContext"></param>

        public SaleGroupProductMapList(ExecutionContext executionContext, List<SaleGroupProductMapDTO> saleGroupProductMapList)
        {
            log.LogMethodEntry(executionContext, saleGroupProductMapList);
            this.executionContext = executionContext;
            this.saleGroupProductMapList = saleGroupProductMapList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the sales offer group list
        /// </summary>
        public List<SaleGroupProductMapDTO> GetAllSaleGroupProductMaps(List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllSaleGroupProductMaps(searchParameters) method");
            SaleGroupProductMapDataHandler saleGroupProductMapDataHandler = new SaleGroupProductMapDataHandler();
            log.Debug("Ends-GetAllSaleGroupProductMaps(searchParameters) method by returning the result of saleGroupProductMapDataHandler.GetSaleGroupProductMapList(searchParameters) call");
            return saleGroupProductMapDataHandler.GetSaleGroupProductMapList(searchParameters);
        }
        /// <summary>
        /// Save or update sales offer group
        /// </summary>
        public void SaveUpdateSetupOfferGroupProductMap()
        {
            try
            {
                log.LogMethodEntry();
                if (saleGroupProductMapList != null)
                {
                    foreach (SaleGroupProductMapDTO salesGroupProductMapdto in saleGroupProductMapList)
                    {
                        SaleGroupProductMap salesGroupProductMapObj = new SaleGroupProductMap(executionContext, salesGroupProductMapdto);
                        salesGroupProductMapObj.Save(null);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the SaleGroupProductMapDTO List for SaleOfferGroupList
        /// </summary>
        /// <param name="saleGroupIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of SaleGroupProductMapDTO</returns>
        public List<SaleGroupProductMapDTO> GetSaleGroupProductMapDTOList(List<int> saleGroupIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(saleGroupIdList, activeRecords, sqlTransaction);
            SaleGroupProductMapDataHandler saleGroupProductMapDataHandler = new SaleGroupProductMapDataHandler();
            List<SaleGroupProductMapDTO> saleGroupProductMapDTOList = saleGroupProductMapDataHandler.GetSaleGroupProductMapDTOList(saleGroupIdList, activeRecords, sqlTransaction);
            log.LogMethodExit(saleGroupProductMapDTOList);
            return saleGroupProductMapDTOList;
        }
    }
}

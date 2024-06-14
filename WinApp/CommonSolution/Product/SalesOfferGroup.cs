/********************************************************************************************
 * Project Name - Sales Offer Group
 * Description  - The bussiness logic for sales offer group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera     Created
 *2.70        21-Jan-2018   Jagan Mohana   Created constructor SalesOfferGroupList and
 *                                         new method SaveUpdateSalesOfferGroupsList
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
    ///Bussiness logic class for Sales Offer Group operations
    /// </summary>
    public class SalesOfferGroup
    {
        SalesOfferGroupDTO salesOfferGroupDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of SalesOfferGroup class
        /// </summary>
        public SalesOfferGroup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            salesOfferGroupDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the SalesOfferGroup DTO based on the salesOfferGroup Id passed 
        /// </summary>
        /// <param name="salesOfferGroupId">salesOfferGroupId</param>
        public SalesOfferGroup(ExecutionContext executionContext, int salesOfferGroupId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, salesOfferGroupId);
            SalesOfferGroupDataHandler salesOfferGroupDataHandler = new SalesOfferGroupDataHandler();
            salesOfferGroupDTO = salesOfferGroupDataHandler.GetSalesOfferGroup(salesOfferGroupId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates sales offer group object using the SalesOfferGroupDTO
        /// </summary>
        /// <param name="salesOfferGroup">SalesOfferGroupDTO object</param>
        public SalesOfferGroup(ExecutionContext executionContext, SalesOfferGroupDTO salesOfferGroup)
        {
            log.LogMethodEntry(salesOfferGroup, executionContext);
            this.salesOfferGroupDTO = salesOfferGroup;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// CSaves and updates the record
        /// </summary>
        /// <param name="sqltrxn">Holds the sql transaction object</param>
        public void Save(SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(sqltrxn);
            SalesOfferGroupDataHandler salesOfferGroupDataHandler = new SalesOfferGroupDataHandler();
            if (salesOfferGroupDTO.SaleGroupId < 0)
            {
                int salesOfferGroupId = salesOfferGroupDataHandler.InsertSalesOfferGroup(salesOfferGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqltrxn);
                salesOfferGroupDTO.SaleGroupId = salesOfferGroupId;
            }
            else
            {
                if (salesOfferGroupDTO.IsChanged == true)
                {
                    salesOfferGroupDataHandler.UpdateSalesOfferGroup(salesOfferGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqltrxn);
                    salesOfferGroupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SalesOfferGroupDTO GetSalesOfferGroupDTO { get { return salesOfferGroupDTO; } }
    }

    /// <summary>
    /// Manages the list of sales offer groups
    /// </summary>
    public class SalesOfferGroupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SalesOfferGroupDTO> salesOfferGroupsList;
        private ExecutionContext executionContext;

        /// <summary>
        ///No Parameter Constructor
        /// </summary>
        public SalesOfferGroupList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametried Constructor
        /// </summary>
        public SalesOfferGroupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.salesOfferGroupsList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametried constructor.
        /// </summary>
        /// <param name="salesOfferGroupList"></param>
        /// <param name="executionContext"></param>
        public SalesOfferGroupList(ExecutionContext executionContext, List<SalesOfferGroupDTO> salesOfferGroupList)
        {
            log.LogMethodEntry(salesOfferGroupList, executionContext);
            this.executionContext = executionContext;
            this.salesOfferGroupsList = salesOfferGroupList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the sales offer group list
        /// </summary>
        public List<SalesOfferGroupDTO> GetAllSalesOfferGroups(List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> searchParameters, bool activeRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            SalesOfferGroupDataHandler salesOfferGroupDataHandler = new SalesOfferGroupDataHandler();
            List<SalesOfferGroupDTO> salesOfferGroupDTOList = salesOfferGroupDataHandler.GetSalesOfferGroupList(searchParameters);
            if (salesOfferGroupDTOList != null && salesOfferGroupDTOList.Count != 0)
            {
                Build(salesOfferGroupDTOList, activeRecords, sqlTransaction);
            }
            log.LogMethodExit();
            return salesOfferGroupDataHandler.GetSalesOfferGroupList(searchParameters);
        }
        /// <summary>
        ///  Save or Updated the Offer Groups.
        /// </summary>
        public void SaveUpdateSalesOfferGroupsList()
        {
            log.LogMethodEntry();
            try
            {
                if (salesOfferGroupsList != null)
                {
                    foreach (SalesOfferGroupDTO salesOfferGroupsDto in salesOfferGroupsList)
                    {
                        SalesOfferGroup SalesOfferGroupsObj = new SalesOfferGroup(executionContext, salesOfferGroupsDto);
                        SalesOfferGroupsObj.Save(null);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private void Build(List<SalesOfferGroupDTO> salesOfferGroupDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(salesOfferGroupDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, SalesOfferGroupDTO> salesOfferGroupDTOIdMap = new Dictionary<int, SalesOfferGroupDTO>();
            List<int> salesOfferGroupIdList = new List<int>();
            for (int i = 0; i < salesOfferGroupDTOList.Count; i++)
            {
                if (salesOfferGroupDTOIdMap.ContainsKey(salesOfferGroupDTOList[i].SaleGroupId))
                {
                    continue;
                }
                salesOfferGroupDTOIdMap.Add(salesOfferGroupDTOList[i].SaleGroupId, salesOfferGroupDTOList[i]);
                salesOfferGroupIdList.Add(salesOfferGroupDTOList[i].SaleGroupId);
            }
            SaleGroupProductMapList saleGroupProductMapListBL = new SaleGroupProductMapList(executionContext);
            List<SaleGroupProductMapDTO> saleGroupProductMapDTOList = saleGroupProductMapListBL.GetSaleGroupProductMapDTOList(salesOfferGroupIdList, activeChildRecords, sqlTransaction);
            if (saleGroupProductMapDTOList != null && saleGroupProductMapDTOList.Any())
            {
                for (int i = 0; i < saleGroupProductMapDTOList.Count; i++)
                {
                    if (salesOfferGroupDTOIdMap.ContainsKey(saleGroupProductMapDTOList[i].SaleGroupId) == false)
                    {
                        continue;
                    }
                    SalesOfferGroupDTO salesOfferGroupDTO = salesOfferGroupDTOIdMap[saleGroupProductMapDTOList[i].SaleGroupId];
                    if (salesOfferGroupDTO.SaleGroupProductMapDTOList == null)
                    {
                        salesOfferGroupDTO.SaleGroupProductMapDTOList = new List<SaleGroupProductMapDTO>();
                    }
                    salesOfferGroupDTO.SaleGroupProductMapDTOList.Add(saleGroupProductMapDTOList[i]);
                }
            }
        }
    }
}
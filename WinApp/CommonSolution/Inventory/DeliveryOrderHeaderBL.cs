/********************************************************************************************
 * Project Name - Inventory
 * Description  -Business logic -DeliveryOrderHeaderBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    public class DeliveryOrderHeaderBL
    {
        private DeliveryOrderHeaderDTO deliveryOrderHeaderDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DeliveryOrderHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.deliveryOrderHeaderDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="deliveryOrderHeaderDTO"></param>
        public DeliveryOrderHeaderBL(ExecutionContext executionContext, DeliveryOrderHeaderDTO deliveryOrderHeaderDTO)
        {
            log.LogMethodEntry(executionContext, deliveryOrderHeaderDTO);
            this.executionContext = executionContext;
            this.deliveryOrderHeaderDTO = deliveryOrderHeaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DeliveryOrderHeaderBLBL id as the parameter
        /// Would fetch the deliveryOrderHeaderDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public DeliveryOrderHeaderBL(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            DeliveryOrderHeaderDataHandler DeliveryOrderHeaderBLDataHandler = new DeliveryOrderHeaderDataHandler(sqlTransaction);
            deliveryOrderHeaderDTO = DeliveryOrderHeaderBLDataHandler.GetDeliveryOrderHeaderDTO(id, sqlTransaction);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate adBroadcast list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            DeliveryOrderLineBLList deliveryOrderLineBLList = new DeliveryOrderLineBLList(executionContext);
            List<KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>(DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_ID, deliveryOrderHeaderDTO.DeliveryOrderId.ToString()));
            deliveryOrderHeaderDTO.DeliveryOrderLineDTOList = deliveryOrderLineBLList.GetDeliveryOrderLineDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DeliveryOrderHeaderBL
        /// DeliveryOrderHeaderBL will be inserted if DeliveryOrderHeaderBL is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            DeliveryOrderHeaderDataHandler deliveryOrderHeaderDataHandler = new DeliveryOrderHeaderDataHandler(sqlTransaction);

            if (deliveryOrderHeaderDTO.DeliveryOrderId < 0)
            {
                deliveryOrderHeaderDTO = deliveryOrderHeaderDataHandler.Insert(deliveryOrderHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                deliveryOrderHeaderDTO.AcceptChanges();
            }
            else
            {
                if (deliveryOrderHeaderDTO.IsChanged == true)
                {
                    deliveryOrderHeaderDTO = deliveryOrderHeaderDataHandler.Update(deliveryOrderHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    deliveryOrderHeaderDTO.AcceptChanges();
                }
            }
            if (deliveryOrderHeaderDTO.DeliveryOrderLineDTOList != null && deliveryOrderHeaderDTO.DeliveryOrderLineDTOList.Count > 0)
            {
                foreach (DeliveryOrderLineDTO deliveryOrderLineDTO in deliveryOrderHeaderDTO.DeliveryOrderLineDTOList)
                {
                    if (deliveryOrderLineDTO.IsChanged)
                    {
                        DeliveryOrderLineBL deliveryOrderLineBL = new DeliveryOrderLineBL(executionContext, deliveryOrderLineDTO);
                        deliveryOrderLineBL.Save(sqlTransaction);
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// get deliveryOrderHeaderDTO Object
        /// </summary>
        public DeliveryOrderHeaderDTO GetDeliveryOrderHeaderDTO
        {
            get { return deliveryOrderHeaderDTO; }
        }

    }
    /// <summary>
    /// Manages the list of DeliveryOrderHeaderBL
    /// </summary>
    public class DeliveryOrderHeaderBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DeliveryOrderHeaderDTO> deliveryOrderHeaderDTOList;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DeliveryOrderHeaderBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.deliveryOrderHeaderDTOList = new List<DeliveryOrderHeaderDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="deliveryOrderHeaderDTOList"></param>
        public DeliveryOrderHeaderBLList(ExecutionContext executionContext, List<DeliveryOrderHeaderDTO> deliveryOrderHeaderDTOList)
        {
            log.LogMethodEntry(executionContext, deliveryOrderHeaderDTOList);
            this.executionContext = executionContext;
            this.deliveryOrderHeaderDTOList = deliveryOrderHeaderDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DeliveryOrderHeaderBL list
        /// </summary>
        public List<DeliveryOrderHeaderDTO> GetDeliveryOrderHeaderDTOList(List<KeyValuePair<DeliveryOrderHeaderDTO.SearchByParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DeliveryOrderHeaderDataHandler deliveryOrderHeaderDataHandler = new DeliveryOrderHeaderDataHandler(sqlTransaction);
            List<DeliveryOrderHeaderDTO> deliveryOrderHeaderDTOList = deliveryOrderHeaderDataHandler.GetDeliveryOrderHeaderDTOList(searchParameters, sqlTransaction);
            if (loadChildRecords)
            {
                if (deliveryOrderHeaderDTOList != null && deliveryOrderHeaderDTOList.Count != 0)
                {
                    DeliveryOrderLineBLList deliveryOrderLineBLList = new DeliveryOrderLineBLList(executionContext);
                    foreach (DeliveryOrderHeaderDTO deliveryOrderHeaderDTO in deliveryOrderHeaderDTOList)
                    {
                        List<KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>>();
                        searchByParams.Add(new KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>(DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_ID, deliveryOrderHeaderDTO.DeliveryOrderId.ToString()));
                        searchByParams.Add(new KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>(DeliveryOrderLineDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        if (loadActiveRecords)
                        {
                            searchByParams.Add(new KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>(DeliveryOrderLineDTO.SearchByParameters.IS_ACTIVE, "1"));
                        }
                        List<DeliveryOrderLineDTO> deliveryOrderLineDTOList = deliveryOrderLineBLList.GetDeliveryOrderLineDTOList(searchByParams, sqlTransaction);
                        if (deliveryOrderLineDTOList != null)
                        {
                            deliveryOrderHeaderDTO.DeliveryOrderLineDTOList = new List<DeliveryOrderLineDTO>(deliveryOrderLineDTOList);
                        }
                    }
                }
            }
            log.LogMethodExit(deliveryOrderHeaderDTOList);
            return deliveryOrderHeaderDTOList;
        }

        /// <summary>
        /// Save or update DeliveryOrderHeaderBL for Web Management Studio
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (deliveryOrderHeaderDTOList != null)
                {
                    foreach (DeliveryOrderHeaderDTO deliveryOrderHeaderDTO in deliveryOrderHeaderDTOList)
                    {
                        DeliveryOrderHeaderBL DeliveryOrderHeaderBL = new DeliveryOrderHeaderBL(executionContext, deliveryOrderHeaderDTO);
                        DeliveryOrderHeaderBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
    }
}

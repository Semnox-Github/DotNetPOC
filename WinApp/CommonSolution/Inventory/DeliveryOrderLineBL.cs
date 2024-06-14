/********************************************************************************************
 * Project Name - Inventory
 * Description  -Business logic -DeliveryOrderLineBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    public class DeliveryOrderLineBL
    {
        private DeliveryOrderLineDTO deliveryOrderLineDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DeliveryOrderLineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.deliveryOrderLineDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="deliveryOrderLineDTO"></param>
        public DeliveryOrderLineBL(ExecutionContext executionContext, DeliveryOrderLineDTO deliveryOrderLineDTO)
        {
            log.LogMethodEntry(executionContext, deliveryOrderLineDTO);
            this.executionContext = executionContext;
            this.deliveryOrderLineDTO = deliveryOrderLineDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="adId">Parameter of the type adId</param>
        public DeliveryOrderLineBL(int id , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(deliveryOrderLineDTO);
            DeliveryOrderLineDataHandler deliveryOrderLineDataHandler = new DeliveryOrderLineDataHandler(sqlTransaction);
            this.deliveryOrderLineDTO = deliveryOrderLineDataHandler.GetDeliveryOrderLineDTO(id, sqlTransaction);
            log.LogMethodExit(deliveryOrderLineDTO);
        }

        /// <summary>
        /// Saves the DeliveryOrderLineBL
        /// ads will be inserted if ads is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            DeliveryOrderLineDataHandler deliveryOrderLineDataHandler = new DeliveryOrderLineDataHandler(sqlTransaction);
           
                if (deliveryOrderLineDTO.DeliveryOrderLineId <0)
                {
                    deliveryOrderLineDTO = deliveryOrderLineDataHandler.Insert(deliveryOrderLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    deliveryOrderLineDTO.AcceptChanges();
                }
                else
                {
                    if (deliveryOrderLineDTO.IsChanged == true)
                    {
                        deliveryOrderLineDTO = deliveryOrderLineDataHandler.Update(deliveryOrderLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        deliveryOrderLineDTO.AcceptChanges();
                    }
                }
            
            log.LogMethodExit();
        }

        /// <summary>
        /// get deliveryOrderLineDTO Object
        /// </summary>
        public DeliveryOrderLineDTO GetdeliveryOrderLineDTO
        {
            get { return deliveryOrderLineDTO; }
        }
    }
    /// <summary>
    /// Manages the list of Ads
    /// </summary>
    public class DeliveryOrderLineBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DeliveryOrderLineDTO> deliveryOrderLineDTOList;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DeliveryOrderLineBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            deliveryOrderLineDTOList = new List<DeliveryOrderLineDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the DeliveryOrderLineBL list
        /// </summary>
        public List<DeliveryOrderLineDTO> GetDeliveryOrderLineDTOList(List<KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DeliveryOrderLineDataHandler deliveryOrderLineDataHandler = new DeliveryOrderLineDataHandler(sqlTransaction);
            List<DeliveryOrderLineDTO> deliveryOrderLineDTOList = deliveryOrderLineDataHandler.GetDeliveryOrderLineDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(deliveryOrderLineDTOList);
            return deliveryOrderLineDTOList;
        }
       
    }
}

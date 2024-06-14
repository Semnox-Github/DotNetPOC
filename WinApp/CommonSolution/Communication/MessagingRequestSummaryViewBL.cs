/********************************************************************************************
 * Project Name - Communication 
 * Description  - Business logic for MessagingRequestSummaryView
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
  2.150.01     03-Feb-2023    Yashodhara C H         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Communication
{
    public class MessagingRequestSummaryViewBL
    {
        private MessagingRequestSummaryViewDTO messagingRequestSummaryViewDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of MessagingRequestViewBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private MessagingRequestSummaryViewBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MessagingRequestViewBL id as the parameter
        /// Would fetch the MessagingRequest object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public MessagingRequestSummaryViewBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            MessagingRequestSummaryViewDataHandler messagingRequestSummaryViewDataHandler = new MessagingRequestSummaryViewDataHandler(sqlTransaction);
            messagingRequestSummaryViewDTO = messagingRequestSummaryViewDataHandler.GetMessagingRequestSummaryView(id);
            if(messagingRequestSummaryViewDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2196, "MessagingRequestSummaryView", id);
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get MessagingRequestSummaryViewDTO Object
        /// </summary>
        public MessagingRequestSummaryViewDTO GetMessagingRequestViewDTO
        {
            get { return messagingRequestSummaryViewDTO; }
        }
    }

    /// <summary>
    /// Manages the list of MessagingRequestViewListBL
    /// </summary>
    public class MessagingRequestSummaryViewListBL
    {
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public MessagingRequestSummaryViewListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the MessagingRequestSummaryViewDTO  List
        /// </summary>
        public List<MessagingRequestSummaryViewDTO> GetAllMessagingRequestViewList(List<KeyValuePair<MessagingRequestSummaryViewDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int numberOfRecords = -1)
        {
            log.LogMethodEntry(searchParameters, pageNumber, numberOfRecords);
            MessagingRequestSummaryViewDataHandler messagingRequestSummaryViewDataHandler = new MessagingRequestSummaryViewDataHandler(null);
            List<MessagingRequestSummaryViewDTO> messagingRequestSummaryViewDTOList = messagingRequestSummaryViewDataHandler.GetMessagingRequestSummaryViewDTOList(searchParameters, executionContext, pageNumber, numberOfRecords);
            log.LogMethodExit(messagingRequestSummaryViewDTOList);
            return messagingRequestSummaryViewDTOList;
        }
    }
}



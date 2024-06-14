/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - BL Class for  CustomSegmentDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0    28-Jun-2019      Mehraj        Created                          
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    class CustomSegmentBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomSegmentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
    }

    class CustomSegmentList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomSegmentList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the list of segments
        /// </summary>
        /// <param name="executionContext"></param>
        public List<CustomSegmentDTO> GetSegments(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomSegmentDataHandler customSegmentHandler = new CustomSegmentDataHandler();
            log.LogMethodExit();
            return customSegmentHandler.GetSegments(siteId);
        }

    }
}

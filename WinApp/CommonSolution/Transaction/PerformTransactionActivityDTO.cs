/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Data holder for transaction related operations
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80         26-Nov-2019   Nitin Pai            Created for Virtual store enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Transaction
{
    public class PerformTransactionActivityDTO
    {
        public enum ActivityType
        {
            BLANKACTIVITY,//Place holder 
            VIRTUALSTORE_ONLINEPRINT,
        }
    }
}